using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    #region Singleton
    public static MiniGameManager Instance;
    // DontDestory�� �ʿ��ϸ� ���߿� ����
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    [SerializeField] MiniGameSO miniGameSO;
    public MiniGameSO _MiniGameSo => miniGameSO;

    public float player1ReactionTime { get; private set; } // t
    public float player2ReactionTime { get; private set; } // t

    public bool miniGameStarted { get; private set; }
    public bool triggerOn { get; private set; }

    public float triggerTime { get; private set; }

    public GameObject _miniGameInstance {get; private set;}

    /// <summary>
    /// ������ MGM���� GM���� ������������, �̷��� 2�� ���۵ȴ�. GM ��ü������ ��� �� �ֵ��� �����ؾ��Ѵ�.
    /// </summary>
    public int waitGachaTime { get; private set; }
    /// <summary>
    /// ���߿� �ʿ������ �ε����θ� ��������
    /// </summary>
    public Define.GameMode selectedGameMode { get; private set; }
    public int selectedGameIndex { get; private set; }


    //test
    public TextMeshProUGUI text;

    public void UpdateSelectedMiniGame(int randomGameIndex)
    {
        selectedGameMode = (Define.GameMode)randomGameIndex;
        // test
        text.text = $"updated selectedMiniGame : {selectedGameMode}";

        selectedGameIndex = 0; // �ʱ�ȭ
        selectedGameIndex = randomGameIndex;
    }

    public void PlayGachaAnimation()
    {
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

    /// <summary>
    /// �������� ���� ���� RPC�� ȣ��� ���ĺ��� �̴ϰ��� ��ƾ�� å������ �޼���
    /// </summary>
    public async void StartMiniGame()
    {
        // �̴� ���� ���� �ִϸ��̼�
        GameObject miniGamePrefab = miniGameSO.GetMiniGamePrefab(selectedGameIndex);
        _miniGameInstance = Instantiate(miniGamePrefab);

        // ���� ���� (Wait For Green...) && �̶����� Ŭ���ϸ� ���� && TriggerTime �� ��������.
        miniGameStarted = true;
        await Task.Delay(2000);

       // TriggerTime ������ ���� Ʈ���� �̺�Ʈ �߻�
       triggerOn = true;
    }
}