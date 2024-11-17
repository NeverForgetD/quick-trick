using Fusion;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class GameManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    // ���� ���� ����
    [Networked] int player1Score { get; set; } // �÷��̾� 1 �¸� Ƚ��_Host
    [Networked] int player2Score { get; set; } // �÷��̾� 2 �¸� Ƚ��_Client
    [Networked] Define.GameMode selectedGameMode { get; set; } // �̴� ���� ����
    [Networked] TickTimer tickTimer { get; set; }
    [Networked] float triggerTime { get; set; } // Ʈ���� ���ð�
    [Networked] bool triggerOn { get; set; } // Ʈ���� �̺�Ʈ ����
    [Networked] bool isGameActive { get; set; } // ������ ��ȿ����

    private const int scoreRequiresToWin = 3; // �̱�� ���� �ʿ��� �� ��

    // �÷��̾� ����
    public Player localPlayer { get; private set; }
    [SerializeField] Player playerPrefab;

    public override void Spawned()
    {

        if (Object.HasStateAuthority)
        {
            Debug.Log("GM Spawned");
            //StartNewGame();
        }
    }

    // �÷��̾� ���� �� ���� �÷��̾� ���� �� ���ʿ� ����
    public void PlayerJoined(PlayerRef playerRef)
    {
        if (HasStateAuthority == false)
            return;

        var player = Runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, playerRef);
        Runner.SetPlayerObject(playerRef, player.Object);

        if (localPlayer == null || localPlayer.Object == null || localPlayer.Object.IsValid == false)
        {
            var playerObject = Runner.GetPlayerObject(Runner.LocalPlayer);
            localPlayer = playerObject != null ? playerObject.GetComponent<Player>() : null;
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


    public void StartNewGame()
    {
        player1Score = 0;
        player2Score = 0;
        isGameActive = true;

        StartRound();
    }


    async void StartRound()
    {
        // if (!isGameActive) { return; } // �� �κ� ����

        if (Object.HasStateAuthority)
        {
            // Ŭ�� �� �ϰ� ����
            triggerOn = false; // �������� Ŭ�� �� �ϰ� �� �� ����ϰ� �����Ѵ�.

            // ������ �̴ϰ��� ���� �� ����ȭ
            int randomGameIndex = UnityEngine.Random.Range(0, (int)Define.GameMode.MaxCount);
            // selectedGameMode = (Define.GameMode)randomGameIndex;
            RPC_UpdateSelectedGame(randomGameIndex); // TODO

            await WaitForTickTimer(5);
            //await Task.Delay(3000); // ����
            // �̱� �ִϸ��̼� ���
            RPC_PlayGachaAnimation();
            int waitGachaTime = MiniGameManager.Instance.waitGachaTime;
            //await Task.Delay(waitGachaTime); // �̱� �ִϸ��̼� �ð� ��� �ð�
            //await Task.Delay(3000);

            await WaitForTickTimer(3);

            // Ʈ���� �ð� ���� �� ����ȭ
            triggerTime = UnityEngine.Random.Range(2f, 8f);

            // �̴ϰ��� �ε� �� ���
            RPC_StartMiniGame();
            //await Task.Delay(1000); // �ִϸ��̼Ǹ��� �ٸ��Լ���...?


            //await Task.Delay((int)(triggerTime * 1000));

            // Ʈ���� �̺�Ʈ �߻�
            triggerOn = true;
            RPC_EnableTriggerEvent();
        }
    }

    #region RPC

    // RPC

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
    private void RPC_UpdateTriggerTime(float n_Time)
    {
        MiniGameManager.Instance.UpdateTriggerTime(n_Time);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_StartMiniGame()
    {
        int playerID = Runner.LocalPlayer.PlayerId;
        MiniGameManager.Instance.StartMiniGame(playerID);
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_EnableTriggerEvent()
    {
        //MiniGameManager.Instance.ShowClickPrompt();
    }
    #endregion

    // TickTimer ������� �񵿱� ���
    async Task WaitForTickTimer(int sec)
    {
        tickTimer = TickTimer.CreateFromSeconds(Runner, sec);
        while (!tickTimer.Expired(Runner))
        {
            await Task.Yield(); // ���� �����ӱ��� ���
        }
    }

    void DetermineWiiner()
    {
        
    }



















    #region legacy
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