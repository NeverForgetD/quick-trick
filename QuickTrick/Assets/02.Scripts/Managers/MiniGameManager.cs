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

    [SerializeField] UIGacha GachaUI;
    [SerializeField] GameObject effects;

    public MiniGameSO _MiniGameSo => miniGameSO;
    [SerializeField] MiniGameSO miniGameSO;

    public bool miniGameReady { get; private set; }
    public bool triggerOn { get; private set; }

    public float triggerTime { get; private set; }

    public MiniGameBase _miniGameInstance {get; private set;}

    /// <summary>
    /// ������ MGM���� GM���� ������������, �̷��� 2�� ���۵ȴ�. GM ��ü������ ��� �� �ֵ��� �����ؾ��Ѵ�.
    /// </summary>
    public int waitGachaTime { get; private set; }
    /// <summary>
    /// ���߿� �ʿ������ �ε����θ� ��������
    /// </summary>
    public Define.GameMode selectedGameMode { get; private set; }
    public int selectedGameIndex { get; private set; }

    /// <summary>
    /// Runner �������� �÷��̾���  ID�� ����
    /// </summary>
    private int playerID = 0;

    public void UpdateSelectedMiniGame(int randomGameIndex)
    {
        selectedGameMode = (Define.GameMode)randomGameIndex;

        selectedGameIndex = 0; // �ʱ�ȭ
        selectedGameIndex = randomGameIndex;
    }

    public void PlayGachaAnimation()
    {
        GachaUI.gameObject.SetActive(true);
        GachaUI.PlayGachaAnimation();
        waitGachaTime = 11;
    }

    public void EndGachaAnimation()
    {
        GachaUI.gameObject.SetActive(false);
    }

    public void UpdateTriggerTime(float triggerTimeFromServer)
    {
        triggerTime = triggerTimeFromServer;
    }

    /// <summary>
    /// �������� ���� ���� RPC�� ȣ��� ���ĺ��� �̴ϰ��� ��ƾ�� å������ �޼���
    /// </summary>
    public async void StartMiniGame()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM("GameBGM");
        effects.gameObject.SetActive(false);
        miniGameReady = false;
        // �̴� ���� ���� �ִϸ��̼�
        MiniGameBase miniGamePrefab = miniGameSO.GetMiniGamePrefab(selectedGameIndex);
        _miniGameInstance = Instantiate(miniGamePrefab);

        _miniGameInstance.OnStandBy();
        // MiniGameBase���� Standby ���� ������ ���
        await WaitForGameReady();
        Debug.Log("Ready");

        await RunTrigger(triggerTime);
    }

    /// <summary>
    /// Standby �۾��� ���� �� ���� ����ϴ� Task
    /// </summary>
    private async Task WaitForGameReady()
    {
        while (!miniGameReady)
        {
            await Task.Yield();
        }
    }

    /// <summary>
    /// ���޹��� triggerTime ���Ŀ� Ʈ���Ÿ� Ű��, �̴ϰ����ν��Ͻ��� ���� �ð�ȭ�Ѵ�.
    /// </summary>
    /// <returns></returns>
    private async Task RunTrigger(float triggerTime)
    {
        int sec = Mathf.FloorToInt(triggerTime) * 1000;
        await Task.Delay(sec);

        triggerOn = true;
        _miniGameInstance.OnTriggerEvent();
    }

    /// <summary>
    /// �̴ϰ��� �Է�ó���� ���� ��, ��� ��ǥ �ܰ�
    /// </summary>
    public void EndMiniGame(int winnerID, float player1ResponseTime, float player2ResponseTime)
    {
        float opponentResponseTime = playerID == 1 ? player2ResponseTime : player1ResponseTime;
        if (playerID == winnerID)
        {
            _miniGameInstance.OnLocalPlayerWin(opponentResponseTime);
        }
        else
        {
            _miniGameInstance.OnLocalPlayerLose(opponentResponseTime);
        }
        Debug.Log($"1 : {player1ResponseTime} /// 2: {player2ResponseTime}");
    }







    /// <summary>
    /// MiniGameBase���� Standby �ִϸ��̼� �۾� ������ ȣ��, Player���� ����
    /// </summary>
    public void GameReady()
    {
        miniGameReady = true;
    }

    /// <summary>
    /// Player���� ȣ��. Ŭ�� ������� �ʰ� ����
    /// </summary>
    public void GameDone()
    {
        miniGameReady = false;
    }

    /// <summary>
    /// player�� ������ �� �÷��̾ ȣ��Ʈ���� Ŭ���̾�Ʈ���� Ȯ�����ִ� �ε��� �߱�
    /// </summary>
    /// <param name="runnerPlayerID"></param>
    public void SetPlayerID(int runnerPlayerID)
    {
        playerID = runnerPlayerID;
    }
}