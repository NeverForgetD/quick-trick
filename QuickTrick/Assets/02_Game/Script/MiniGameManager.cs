using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    public float player1ReactionTime { get; private set; }
    public float player2ReactionTime { get; private set; }
    public float triggerTime { get; private set; }
    public int waitGachaTime { get; private set; }
    public Define.GameMode selectedGameMode { get; private set; }

    public static event Action<PlayerRef, float> OnPlayerInputReceived; // 플레이어 인풋이 발생했을 때 전달할 이벤트

    public TextMeshProUGUI text;

    private void Awake()
    {
        Instance = this;
    }


    public void UpdateSelectedMiniGame(int randomGameIndex)
    {
        selectedGameMode = (Define.GameMode)randomGameIndex;

        // test
        text.text = $"updated selectedMiniGame : {selectedGameMode}";
    }

    public void PlayGachaAnimation()
    {
        // 애니메이션 play logic
        // 애니메이션 플레이 시작 시전 포함 얼마나 기다려야 하는지 반환

        // test
        text.text = $"playing Gacha animation {selectedGameMode}";
        waitGachaTime = 3000;
    }

    public void UpdateTriggerTime(float n_Time)
    {
        triggerTime = n_Time;

        // test
        text.text = $"tirgger time : {triggerTime}";
    }

    public void StartMiniGame(int playerID)
    {
        // selectedGameMode에 맞는 것으로 가져온다.
        // 미니게임 화면 띄워준다.


        // 미니게임은 대기_시작_상호작용_발표

        // test
        text.text = $"game started\n player : {playerID}";
    }

    public void MiniGamePhase(int index)
    {
        // 페이지 변환
        // 페이즈 1 _ 대기
        // 페이즈 2 _ 시작
        // 페이즈 3 _ 상호작용
        // 페이즈 4 _ 발표
    }

    //test
    private bool canPlay;
    private void Update()
    {
        if (canPlay && Input.GetMouseButtonDown(0))
        {
            // 입력 데이터 발생 (로컬 플레이어의 반응 시간 전달
            // 자신이 누군지 알아야 한다.
            // OnPlayerInputReceived?.Invoke(,)
        }
    }



    public void RecordReactionTime(PlayerRef player, float reactionTime)
    {

    }

}