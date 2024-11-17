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

    public static event Action<PlayerRef, float> OnPlayerInputReceived; // �÷��̾� ��ǲ�� �߻����� �� ������ �̺�Ʈ

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
        // selectedGameMode�� �´� ������ �����´�.
        // �̴ϰ��� ȭ�� ����ش�.


        // �̴ϰ����� ���_����_��ȣ�ۿ�_��ǥ

        // test
        text.text = $"game started\n player : {playerID}";
    }

    public void MiniGamePhase(int index)
    {
        // ������ ��ȯ
        // ������ 1 _ ���
        // ������ 2 _ ����
        // ������ 3 _ ��ȣ�ۿ�
        // ������ 4 _ ��ǥ
    }

    //test
    private bool canPlay;
    private void Update()
    {
        if (canPlay && Input.GetMouseButtonDown(0))
        {
            // �Է� ������ �߻� (���� �÷��̾��� ���� �ð� ����
            // �ڽ��� ������ �˾ƾ� �Ѵ�.
            // OnPlayerInputReceived?.Invoke(,)
        }
    }



    public void RecordReactionTime(PlayerRef player, float reactionTime)
    {

    }

}