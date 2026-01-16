# 🎮 Quick Trick!

> 네트워크 환경에서도 공정한 반응속도 대결을 목표로 설계·구현한  
> **멀티플레이 캐주얼 게임 프로젝트**입니다.

<img width="4378" height="866" alt="QuickTrick_banner" src="https://github.com/user-attachments/assets/7b970c64-b90e-49c5-ba68-e63579eb1259" />

---

## 1. 프로젝트 개요

- **플랫폼**: PC (오프라인 행사 기준)
- **엔진**: Unity
- **네트워크**: Photon Fusion
- **장르**: 멀티플레이 / 캐주얼 / 반응속도
- **개발 형태**: 개인 개발 (아트 2명, 사운드 1명 협업)
- **성과**: 넥슨 대학생 게임 공모전(NDM)  
  - 🏆 임직원 투표 베스트상

Quick Trick!은  
짧은 시간 안에 누가 더 빠르게 반응했는지를 겨루는 게임으로,  
오프라인 행사 환경에서도 처음 접한 플레이어가 즉시 이해하고 참여할 수 있도록 설계했습니다.

---

## 🧩 세부 구현 내용

### 🔹 매치메이킹 및 세션 흐름 관리
- 플레이어 매칭부터 게임 시작까지의 전체 흐름 설계

### 🔹 멀티플레이 게임 흐름 동기화 (RPC 기반)
- 입력 허용 시점과 결과 판정을 서버 기준으로 동기화

### 🔹 확장 가능한 미니게임 구조 설계 (객체지향)
- 공통 인터페이스 기반의 미니게임 확장 구조

### 🔹 ScriptableObject 기반 데이터 중심 설계
- 사운드 및 미니게임 설정을 데이터로 분리 관리

### 🔹 런타임 시각 효과 및 물리 상호작용 구현
- 랜덤성과 물리를 결합한 동적 연출 구현

전체 구현 코드는 [Scripts 폴더](./Assets/Scripts)를 참고해주세요.

<details>
<summary>💡 C# 코드 보기</summary>
  
```csharp
test code
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
    [SerializeField] GameObject EndGameUI;

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
    private int playerID;

    public void UpdateSelectedMiniGame(int randomGameIndex)
    {
        selectedGameMode = (Define.GameMode)randomGameIndex;

        selectedGameIndex = 0; // 초기화
        selectedGameIndex = randomGameIndex;
    }

    public void PlayGachaAnimation()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM("Wait");
        //Instantiate(GachaUI);
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
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM("End");
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

    public void EndGame()
    {
        EndGameUI.SetActive(true);
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
        Debug.Log($"this com {playerID}");
    }
}
</details> ```


