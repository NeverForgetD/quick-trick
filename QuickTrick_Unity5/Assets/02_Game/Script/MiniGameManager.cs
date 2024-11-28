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
        // �ִϸ��̼� play logic
        // �ִϸ��̼� �÷��� ���� ���� ���� �󸶳� ��ٷ��� �ϴ��� ��ȯ

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
        // �̴� ���� ���� �ִϸ��̼�

        // ���� ���� (Wait For Green...) && �̶����� Ŭ���ϸ� ���� && TriggerTime �� ��������.
        miniGameStarted = true;
        await Task.Delay(2000);

       // TriggerTime ������ ���� Ʈ���� �̺�Ʈ �߻�
       triggerOn = true;
    }
}