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

    [SerializeField] MiniGameSO miniGameSO;
    public MiniGameSO _MiniGameSo => miniGameSO;

    //public float player1ReactionTime { get; private set; } // t
    //public float player2ReactionTime { get; private set; } // t

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


    //test
    public TextMeshProUGUI text;

    public void UpdateSelectedMiniGame(int randomGameIndex)
    {
        selectedGameMode = (Define.GameMode)randomGameIndex;
        // test
        text.text = $"updated selectedMiniGame : {selectedGameMode}";

        selectedGameIndex = 0; // 초기화
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
    /// 서버에서 게임 시작 RPC가 호출된 이후부터 미니게임 루틴을 책임지는 메서드
    /// </summary>
    public async void StartMiniGame()
    {
        miniGameReady = false;
        // 미니 게임 띄우는 애니메이션
        MiniGameBase miniGamePrefab = miniGameSO.GetMiniGamePrefab(selectedGameIndex);
        _miniGameInstance = Instantiate(miniGamePrefab);

        _miniGameInstance.OnStandBy();
        // MiniGameBase에서 Standby 끝날 때까지 대기
        await WaitForGameReady();

        await Task.Delay(3000);
        triggerOn = true;
        _miniGameInstance.OnTriggerEvent();

        // TriggerTime 지나간 이후 트리거 이벤트 발생
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

}