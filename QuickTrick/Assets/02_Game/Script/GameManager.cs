using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    // �÷��̾� ���� ����
    public Player localPlayer { get; private set; } // �÷��̾� �׽�Ʈ ������
    [Tooltip("�÷��̾� ������")]
    public Player _playerPrefab;

    // ���ӿ� �ʿ��� ���� ����
    [Networked] private int player1Wins { get ; set; } // �÷��̾�1 �¸� Ƚ��
    [Networked] private int player2Wins { get; set; } // �÷��̾�2 �¸� Ƚ��
    [Networked] int roundCount { get; set; } // ���� ���� ��
    [Networked] private bool isGameActive { get; set; } // ���� Ȱ��ȭ ����
    [Networked] private int randomGameIndex { get; set; } // �̴� ���� ����(�ε���)
    [Networked] private Define.GameMode gameMode { get; set; } // �̴� ���� ����

    private const int winsRequiredForVictory= 3; // ���� ��¿� �ʿ��� �¸� Ƚ��

    public override void Spawned()
    {
        if (_playerPrefab == null)
        {
            _playerPrefab = Resources.Load<Player>("Prefabs/Player");
        }

        localPlayer = Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, Runner.LocalPlayer);
        Runner.SetPlayerObject(Runner.LocalPlayer, localPlayer.Object);

        if (Object.HasStateAuthority) // �� �ڵ�� ���� �Ǵ� ������ ���� Ŭ���̾�Ʈ������ ����ȴ�.
        {
            AssignPlayer();
            StartNewGame();
            Debug.Log("GM�� ���� �Լ��� ���������� �ߵ�");
        }
    }

    void AssignPlayer()
    {
        
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

        Debug.Log(gameMode);
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
        /*
        if (winningPlayer == Runner.GetPlayerRef(0))
            player1Wins++;
        else if (winningPlayer == PlayerRef.)
            player2Wins++;
        */
    }
}
