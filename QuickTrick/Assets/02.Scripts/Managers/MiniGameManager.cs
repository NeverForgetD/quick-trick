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
    // DontDestory가 필요하면 나중에 넣자
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
    /// 지금은 MGM에서 GM에게 전달해주지만, 이러면 2번 전송된다. GM 자체적으로 운영될 수 있도록 수정해야한다.
    /// </summary>
    public int waitGachaTime { get; private set; }
    /// <summary>
    /// 나중에 필요없으면 인덱스로만 저장하자
    /// </summary>
    public Define.GameMode selectedGameMode { get; private set; }
    public int selectedGameIndex { get; private set; }

    /// <summary>
    /// Runner 기준으로 플레이어의  ID를 저장
    /// </summary>
    private int playerID = 0;

    public void UpdateSelectedMiniGame(int randomGameIndex)
    {
        selectedGameMode = (Define.GameMode)randomGameIndex;

        selectedGameIndex = 0; // 초기화
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
    /// 서버에서 게임 시작 RPC가 호출된 이후부터 미니게임 루틴을 책임지는 메서드
    /// </summary>
    public async void StartMiniGame()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM("GameBGM");
        effects.gameObject.SetActive(false);
        miniGameReady = false;
        // 미니 게임 띄우는 애니메이션
        MiniGameBase miniGamePrefab = miniGameSO.GetMiniGamePrefab(selectedGameIndex);
        _miniGameInstance = Instantiate(miniGamePrefab);

        _miniGameInstance.OnStandBy();
        // MiniGameBase에서 Standby 끝날 때까지 대기
        await WaitForGameReady();
        Debug.Log("Ready");

        await RunTrigger(triggerTime);
    }

    /// <summary>
    /// Standby 작업이 끝날 때 까지 대기하는 Task
    /// </summary>
    private async Task WaitForGameReady()
    {
        while (!miniGameReady)
        {
            await Task.Yield();
        }
    }

    /// <summary>
    /// 전달받은 triggerTime 이후에 트리거를 키고, 미니게임인스턴스를 통해 시각화한다.
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
    /// 미니게임 입력처리가 끝난 후, 결과 발표 단계
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
    /// MiniGameBase에서 Standby 애니메이션 작업 끝나면 호출, Player에게 전달
    /// </summary>
    public void GameReady()
    {
        miniGameReady = true;
    }

    /// <summary>
    /// Player에서 호출. 클릭 허용하지 않게 변경
    /// </summary>
    public void GameDone()
    {
        miniGameReady = false;
    }

    /// <summary>
    /// player가 스폰될 때 플레이어가 호스트인지 클라이언트인지 확인해주는 인덱스 발급
    /// </summary>
    /// <param name="runnerPlayerID"></param>
    public void SetPlayerID(int runnerPlayerID)
    {
        playerID = runnerPlayerID;
    }
}