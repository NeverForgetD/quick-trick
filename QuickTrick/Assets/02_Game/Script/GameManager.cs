using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    // 플레이어 세팅 관련
    public Player localPlayer { get; private set; } // 플레이어 테스트 진행중
    [Tooltip("플레이어 프리팹")]
    public Player _playerPrefab;

    // 게임에 필요한 정보 관련
    [Networked] private int player1Wins { get ; set; } // 플레이어1 승리 횟수
    [Networked] private int player2Wins { get; set; } // 플레이어2 승리 횟수
    [Networked] int roundCount { get; set; } // 현재 라운드 수
    [Networked] private bool isGameActive { get; set; } // 게임 활성화 여부
    [Networked] private int randomGameIndex { get; set; } // 미니 게임 종류(인덱스)
    [Networked] private Define.GameMode gameMode { get; set; } // 미니 게임 종류

    private const int winsRequiredForVictory= 3; // 최종 우승에 필요한 승리 횟수

    public override void Spawned()
    {
        if (_playerPrefab == null)
        {
            _playerPrefab = Resources.Load<Player>("Prefabs/Player");
        }

        localPlayer = Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, Runner.LocalPlayer);
        Runner.SetPlayerObject(Runner.LocalPlayer, localPlayer.Object);

        if (Object.HasStateAuthority) // 이 코드는 서버 또는 권한을 가진 클라이언트에서만 실행된다.
        {
            AssignPlayer();
            StartNewGame();
            Debug.Log("GM의 스폰 함수가 성공적으로 발동");
        }
    }

    void AssignPlayer()
    {
        
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

        Debug.Log(gameMode);
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
        /*
        if (winningPlayer == Runner.GetPlayerRef(0))
            player1Wins++;
        else if (winningPlayer == PlayerRef.)
            player2Wins++;
        */
    }
}
