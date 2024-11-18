using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    public float player1ReactionTime { get; private set; } // t
    public float player2ReactionTime { get; private set; } // t

    public bool miniGameStarted { get; private set; }
    public bool triggerOn { get; private set; }

    public float triggerTime { get; private set; }
    public int waitGachaTime { get; private set; }
    public Define.GameMode selectedGameMode { get; private set; }

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
        waitGachaTime = 3;
    }

    public void UpdateTriggerTime(float triggerTimeFromServer)
    {
        triggerTime = triggerTimeFromServer;
        // test
        text.text = $"tirgger time : {triggerTime}";
    }

    public async void StartMiniGame()
    {
        // 미니 게임 띄우는 애니메이션

        // 게임 설명 (Wait For Green...) && 이때부터 클릭하면 진다 && TriggerTime 이 지나간다.
        miniGameStarted = true;
        await Task.Delay(2000);

       // TriggerTime 지나간 이후 트리거 이벤트 발생
       triggerOn = true;
    }
}