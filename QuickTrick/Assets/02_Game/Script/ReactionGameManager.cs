using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionGameManager : NetworkBehaviour
{
    [Networked] private int player1Wins { get ; set; } // 플레이어1 승리 횟수
    [Networked] private int player2Wins { get; set; } // 플레이어2 승리 횟수
    [Networked] int roundCount { get; set; } // 현재 라운드 수
    [Networked] private bool isGameActive { get; set; } // 게임 활성화 여부

    private const int winsRequiredForVictory= 3; // 최종 우승에 필요한 승리 횟수

    public override void Spawned()
    {
        if (Object.HasStateAuthority) // 이 코드는 서버 또는 권한을 가진 클라이언트에서만 실행된다.
        {
            StartNewGame();
        }
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
        }
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
