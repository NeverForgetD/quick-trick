using Fusion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class GameManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    // 게임 로직 관련
    #region Networked
    [Networked] int player1Score { get; set; } // 플레이어 1 승리 횟수_Host
    [Networked] int player2Score { get; set; } // 플레이어 2 승리 횟수_Client
    [Networked] int randomGameIndex { get; set; } // 미니 게임 종류_인덱스로 저장 및 전달
    [Networked] TickTimer tickTimer { get; set; }
    [Networked] float triggerTime { get; set; } // 트리거 대기시간
    [Networked] bool isGameActive { get; set; } // 게임이 유효한지
    #endregion

    #region privates
    private const int scoreRequiresToWin = 3; // 이기기 위해 필요한 판 수

    // 매 판 ClearData로 초기화
    private Dictionary<int, float> playersResponseTime = new Dictionary<int, float>();
    private bool isResultSent;
    #endregion

    #region SerializeField
    [SerializeField] float minTriggerTime;
    [SerializeField] float maxTriggerTime;
    #endregion

    // 플레이어 관련
    public Player localPlayer { get; private set; }
    [SerializeField] Player playerPrefab;

    public TextMeshProUGUI text;

    public GameManager Instance { get; private set; }

    private void OnEnable()
    {
        Player.OnPlayerClicked += ReceivePlayerClicked;
    }

    private void OnDisable()
    {
        Player.OnPlayerClicked -= ReceivePlayerClicked;
    }

    public override void Spawned()
    {
        Debug.Log("GM Spawned--------------------");

        if (Object.HasStateAuthority)
        {
            //StartNewGame();
        }
    }

    // 플레이어 입장 시 로컬 플레이어 생성 및 러너에 저장
    
    public void PlayerJoined(PlayerRef playerRef)
    {
        if (HasStateAuthority == false)
            return;

        //Debug.Log($"\nplayer joined {Runner.LocalPlayer.PlayerId}\n");

        var player = Runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, playerRef);
        Runner.SetPlayerObject(playerRef, player.Object);
        
        if (localPlayer == null || localPlayer.Object == null || localPlayer.Object.IsValid == false)
        {
            var playerObject = Runner.GetPlayerObject(Runner.LocalPlayer);
            localPlayer = playerObject != null ? playerObject.GetComponent<Player>() : null;
        }
        //Debug.Log($"\nplayer joined {Runner.LocalPlayer.PlayerId}\n");
        Debug.Log($"YYY player joined {playerRef.PlayerId}");

        if (Runner.SessionInfo.PlayerCount == 2)
        {
            StartNewGame();
        }
    }
    

    // 플레이어 퇴장 시
    public void PlayerLeft(PlayerRef player)
    {
        if (HasStateAuthority == false)
            return;

        // 필요 시 누구 나갔을 때 남은 플레이어 승리 메서드 구현
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        // 로컬 플레이어 초기화
        localPlayer = null;
    }

    /// <summary>
    /// 게임 세트를 시작한다.
    /// </summary>
    public void StartNewGame()
    {
        player1Score = 0;
        player2Score = 0;
        isGameActive = true;

        ClearData();

        StartRound();
    }

    /// <summary>
    /// 메인 게임 루프(라운드) 게임 종료 조건까지 게임을 진행시킨다.
    /// </summary>
    private async void StartRound()
    {
        if (player1Score == 3 || player2Score == 3)
            isGameActive = false;

        if (!isGameActive)
        {
            //EndGame();
        }

        if (Object.HasStateAuthority)
        {

            // 랜덤한 미니게임 결정 및 동기화
            randomGameIndex = UnityEngine.Random.Range(0, (int)Define.GameMode.MaxCount);
            randomGameIndex = 2; // 테스트용 test
            RPC_UpdateSelectedGame(randomGameIndex);

            // 뽑기 애니메이션 재생
            RPC_PlayGachaAnimation();
            await WaitForTickTimer(MiniGameManager.Instance.waitGachaTime);

            // 트리거 시간 결정
            triggerTime = UnityEngine.Random.Range(minTriggerTime, maxTriggerTime);
            // TODO
            // 미니게임마다 주기가 필요한경우(할리갈리) 해당 주기를 전달

            // 미니게임 로드 및 대기 & 트리거 타임 전달
            RPC_StartMiniGame(triggerTime);

            // await
            // 두 플레이어가
            await WaitForPlayerResultArrive();

            RPC_AnnounceWinner();
            //await WaitForTickTimer(5);
            //StartRound();
        }
    }

    #region RPC Methods
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_UpdateSelectedGame(int index)
    {
        MiniGameManager.Instance.UpdateSelectedMiniGame(index);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlayGachaAnimation()
    {
        MiniGameManager.Instance.PlayGachaAnimation();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_StartMiniGame(float triggerTime)
    {
        MiniGameManager.Instance.UpdateTriggerTime(triggerTime);
        MiniGameManager.Instance.StartMiniGame();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_AnnounceWinner()
    {
        int winnerID = DetermineWiiner();
        MiniGameManager.Instance.EndMiniGame(winnerID, playersResponseTime[1], playersResponseTime[2]);
    }
    #endregion


    // TickTimer 만료까지 비동기 대기
    async Task WaitForTickTimer(int sec)
    {
        tickTimer = TickTimer.CreateFromSeconds(Runner, sec);
        while (!tickTimer.Expired(Runner))
        {
            await Task.Yield();
        }
    }

    /// <summary>
    /// 플레이어에게서 정보를 받는다.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="responseTime"></param>
    /// <param name="isValid"></param>
    private void ReceivePlayerClicked(int playerID, float responseTime, bool isValid)
    {
        // test
        text.text = $"player{playerID} :::::: {responseTime}";

        if (!isValid)
        {
            playersResponseTime.Add(playerID, -1f);
        }
        else
        {
            playersResponseTime.Add(playerID, responseTime);
        }

        
        //Debug.Log($"count is {playersResponseTime.Count}");

        if (playersResponseTime.Count == 2 && !isResultSent)
        {
            isResultSent = true;
        }
    }

    /// <summary>
    /// 두 플레이어에게서 정보를 받을 때까지 대기하는 Task
    /// </summary>
    /// <returns></returns>
    async Task WaitForPlayerResultArrive()
    {
        //tickTimer = TickTimer.CreateFromSeconds(Runner, 2);
        //|| !tickTimer.Expired(Runner)
        while (!isResultSent)
        {
            await Task.Yield();
        }
    }

    /// <summary>
    /// 승자의 인덱스를 return
    /// </summary>
    /// <returns></returns>
    int DetermineWiiner()
    {
        if (playersResponseTime[1] > playersResponseTime[2]) // player2 win
        {
            player2Score++;
            text.text = $"player2 ::: {playersResponseTime[2]} Win!!";
            return 2;
        }
        else
        {
            player1Score++;
            text.text = $"player1 ::: {playersResponseTime[1]} Win!!";
            return 1;
        }
    }

    void ClearData()
    {
        playersResponseTime.Clear();
        isResultSent = false;
    }


    #region Legacy
    /*
    // 플레이어 세팅 관련
    [SerializeField ,Tooltip("플레이어 프리팹")]
    private Player _playerPrefab;
    public Player localPlayer { get; private set; } // 플레이어 인스턴스

    // 게임에 필요한 정보 관련
    [Networked] private int player1Wins { get; set; } // 플레이어1 승리 횟수_호스트 (IsServer)
    [Networked] private int player2Wins { get; set; } // 플레이어2 승리 횟수_게스트
    [Networked] int roundCount { get; set; } // 현재 라운드 수
    [Networked] private bool isGameActive { get; set; } // 게임 활성화 여부
    [Networked] private int randomGameIndex { get; set; } // 미니 게임 종류(인덱스)
    [Networked] private Define.GameMode gameMode { get; set; } // 미니 게임 종류

    

    private const int winsRequiredForVictory= 3; // 최종 우승에 필요한 승리 횟수

    public TextMeshProUGUI text;
    public override void Spawned()
    {
        if (Object.HasStateAuthority) // GameManager는 클라이언트당 한 개가 스폰돼서 총 2개...해당 클라이언트의 것만 StateAuthority를 가진다.
        {
            //AssignPlayer();
            //StartNewGame();
            //text.text = "has authority";
        }
        //Debug.Log(Runner.IsServer);
        //Debug.Log(localPlayer.IsHost);
        //Debug.Log(Object.HasStateAuthority);
        //Debug.Log(Runner.LocalPlayer.PlayerId);
    }

    void AssignPlayer()
    {
        if (_playerPrefab == null)
            _playerPrefab = Resources.Load<Player>("Prefabs/Player");

        localPlayer = Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, Runner.LocalPlayer);
        Runner.SetPlayerObject(Runner.LocalPlayer, localPlayer.Object);
    }

    // 게임 시작 및 초기화
    void StartNewGame()
    {
        player1Wins= 0;
        player2Wins= 0;
        isGameActive = true;

        StartNewRound();
    }

    void StartNewRound()
    {
        if (isGameActive)
        {
            // 라운드 시작 관련 초기화 코드
            // 어떤 모드 게임할 지 결정
            RPC_ChoseRandomGame();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)] // RPC
    public void RPC_ChoseRandomGame()
    {
        randomGameIndex = UnityEngine.Random.Range(0, (int)Define.GameMode.MaxCount); // 랜덤값말고 나중에 룰렛 등으로 하는것도 고려
        gameMode = (Define.GameMode)randomGameIndex;

        //Debug.Log(gameMode);
        text.text = gameMode.ToString();
        // 게임 모드 불러오기
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)] // RPC
    public void RPC_LoadGame()
    {
        //
    }


    // 각 라운드의 승자 업데이트 및 게임 상태 확인
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)] // RPC
    public void RPC_UpdateRoundWinner(int winningPlayerID)
    {
        if (winningPlayerID == 1)
            player1Wins++;
        else if (winningPlayerID == 2)
            player2Wins++;

        CheckForGameWinner();
    }
    
    private void CheckForGameWinner()
    {
        if (player1Wins >= winsRequiredForVictory)
            RPC_AnnounceFinalWinner(1);
        else if (player1Wins >= winsRequiredForVictory)
            RPC_AnnounceFinalWinner(2);
        else
        {
            StartNewRound(); // 3번 먼저 이긴 우승자가 나오기 전까지 새로운 라운드 시작
        }
    }

    // 최종 승자 결정
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)] // RPC
    public void RPC_AnnounceFinalWinner(int winningPlayerID)
    {
        isGameActive = false;
        // 승리,패배 UI 표시나 종료 처리 등
    }

    // 서버에서 승리자 결정 및 알림
    public void EndRound(PlayerRef winningPlayer)
    {
        if (winningPlayer == PlayerRef.None) return;

        // 승리자 업데이트
        
        if (winningPlayer == Runner.GetPlayerRef(0))
            player1Wins++;
        else if (winningPlayer == PlayerRef.)
            player2Wins++;
        
    }
*/
    #endregion
}