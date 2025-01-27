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
    // ���� ���� ����
    #region Networked
    [Networked] int player1Score { get; set; } // �÷��̾� 1 �¸� Ƚ��_Host
    [Networked] int player2Score { get; set; } // �÷��̾� 2 �¸� Ƚ��_Client
    [Networked] int randomGameIndex { get; set; } // �̴� ���� ����_�ε����� ���� �� ����
    [Networked] TickTimer tickTimer { get; set; }
    [Networked] float triggerTime { get; set; } // Ʈ���� ���ð�
    [Networked] bool isGameActive { get; set; } // ������ ��ȿ����
    #endregion

    #region privates
    private const int scoreRequiresToWin = 3; // �̱�� ���� �ʿ��� �� ��

    // �� �� ClearData�� �ʱ�ȭ
    private Dictionary<int, float> playersResponseTime = new Dictionary<int, float>();
    private bool isResultSent;
    #endregion

    #region SerializeField
    [SerializeField] float minTriggerTime;
    [SerializeField] float maxTriggerTime;
    #endregion

    // �÷��̾� ����
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

    // �÷��̾� ���� �� ���� �÷��̾� ���� �� ���ʿ� ����
    
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
    

    // �÷��̾� ���� ��
    public void PlayerLeft(PlayerRef player)
    {
        if (HasStateAuthority == false)
            return;

        // �ʿ� �� ���� ������ �� ���� �÷��̾� �¸� �޼��� ����
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        // ���� �÷��̾� �ʱ�ȭ
        localPlayer = null;
    }

    /// <summary>
    /// ���� ��Ʈ�� �����Ѵ�.
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
    /// ���� ���� ����(����) ���� ���� ���Ǳ��� ������ �����Ų��.
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

            // ������ �̴ϰ��� ���� �� ����ȭ
            randomGameIndex = UnityEngine.Random.Range(0, (int)Define.GameMode.MaxCount);
            randomGameIndex = 2; // �׽�Ʈ�� test
            RPC_UpdateSelectedGame(randomGameIndex);

            // �̱� �ִϸ��̼� ���
            RPC_PlayGachaAnimation();
            await WaitForTickTimer(MiniGameManager.Instance.waitGachaTime);

            // Ʈ���� �ð� ����
            triggerTime = UnityEngine.Random.Range(minTriggerTime, maxTriggerTime);
            // TODO
            // �̴ϰ��Ӹ��� �ֱⰡ �ʿ��Ѱ��(�Ҹ�����) �ش� �ֱ⸦ ����

            // �̴ϰ��� �ε� �� ��� & Ʈ���� Ÿ�� ����
            RPC_StartMiniGame(triggerTime);

            // await
            // �� �÷��̾
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


    // TickTimer ������� �񵿱� ���
    async Task WaitForTickTimer(int sec)
    {
        tickTimer = TickTimer.CreateFromSeconds(Runner, sec);
        while (!tickTimer.Expired(Runner))
        {
            await Task.Yield();
        }
    }

    /// <summary>
    /// �÷��̾�Լ� ������ �޴´�.
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
    /// �� �÷��̾�Լ� ������ ���� ������ ����ϴ� Task
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
    /// ������ �ε����� return
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
    // �÷��̾� ���� ����
    [SerializeField ,Tooltip("�÷��̾� ������")]
    private Player _playerPrefab;
    public Player localPlayer { get; private set; } // �÷��̾� �ν��Ͻ�

    // ���ӿ� �ʿ��� ���� ����
    [Networked] private int player1Wins { get; set; } // �÷��̾�1 �¸� Ƚ��_ȣ��Ʈ (IsServer)
    [Networked] private int player2Wins { get; set; } // �÷��̾�2 �¸� Ƚ��_�Խ�Ʈ
    [Networked] int roundCount { get; set; } // ���� ���� ��
    [Networked] private bool isGameActive { get; set; } // ���� Ȱ��ȭ ����
    [Networked] private int randomGameIndex { get; set; } // �̴� ���� ����(�ε���)
    [Networked] private Define.GameMode gameMode { get; set; } // �̴� ���� ����

    

    private const int winsRequiredForVictory= 3; // ���� ��¿� �ʿ��� �¸� Ƚ��

    public TextMeshProUGUI text;
    public override void Spawned()
    {
        if (Object.HasStateAuthority) // GameManager�� Ŭ���̾�Ʈ�� �� ���� �����ż� �� 2��...�ش� Ŭ���̾�Ʈ�� �͸� StateAuthority�� ������.
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

    // ���� ���� �� �ʱ�ȭ
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
            // ���� ���� ���� �ʱ�ȭ �ڵ�
            // � ��� ������ �� ����
            RPC_ChoseRandomGame();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)] // RPC
    public void RPC_ChoseRandomGame()
    {
        randomGameIndex = UnityEngine.Random.Range(0, (int)Define.GameMode.MaxCount); // ���������� ���߿� �귿 ������ �ϴ°͵� ���
        gameMode = (Define.GameMode)randomGameIndex;

        //Debug.Log(gameMode);
        text.text = gameMode.ToString();
        // ���� ��� �ҷ�����
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)] // RPC
    public void RPC_LoadGame()
    {
        //
    }


    // �� ������ ���� ������Ʈ �� ���� ���� Ȯ��
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
            StartNewRound(); // 3�� ���� �̱� ����ڰ� ������ ������ ���ο� ���� ����
        }
    }

    // ���� ���� ����
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)] // RPC
    public void RPC_AnnounceFinalWinner(int winningPlayerID)
    {
        isGameActive = false;
        // �¸�,�й� UI ǥ�ó� ���� ó�� ��
    }

    // �������� �¸��� ���� �� �˸�
    public void EndRound(PlayerRef winningPlayer)
    {
        if (winningPlayer == PlayerRef.None) return;

        // �¸��� ������Ʈ
        
        if (winningPlayer == Runner.GetPlayerRef(0))
            player1Wins++;
        else if (winningPlayer == PlayerRef.)
            player2Wins++;
        
    }
*/
    #endregion
}