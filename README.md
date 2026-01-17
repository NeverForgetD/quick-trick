# 🎮 Quick Trick!

> 네트워크 환경에서도 공정한 반응속도 대결을 목표로 설계·구현한  
> **멀티플레이 캐주얼 게임 프로젝트**입니다.

<img width="4378" height="866" alt="QuickTrick_banner" src="https://github.com/user-attachments/assets/7b970c64-b90e-49c5-ba68-e63579eb1259" />

---

## 🧩 프로젝트 개요

- **플랫폼**: PC, Windows
- **엔진**: Unity
- **네트워크**: Photon Fusion
- **장르**: 멀티플레이 / 캐주얼 / 반응속도
- **개발 형태**: 개인 개발 (아트 2명, 사운드 1명 협업)
- **성과**: 넥슨 대학생 게임 공모전(NDM)  
  - 🏆 임직원 투표 베스트상

Quick Trick!은  
짧은 시간 안에 누가 더 빠르게 반응했는지를 겨루는 게임으로,  
오프라인 행사 환경에서도 처음 접한 플레이어가 즉시 이해하고 참여할 수 있도록 설계했습니다.

[플레이 영상](https://youtu.be/l9BJzEqFgyA)

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



## 🧩 세부 구현 코드
<details>
<summary>매치메이킹 및 세션 흐름 관리</summary>

```{csharp}
using Fusion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class GameManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    // 게임 로직 관련
    #region Networked
    [Networked] int player1Score { get; set; } // 플레이어 1 승리 횟수_Host
    [Networked] int player2Score { get; set; } // 플레이어 2 승리 횟수_Client
    [Networked] int randomGameIndex { get; set; } // 미니 게임 종류_인덱스로 저장 및 전달
    [Networked] TickTimer tickTimer { get; set; }
    [Networked] float triggerTime { get; set; } // 트리거 대기시간
    [Networked] bool isGameActive { get; set; } // 게임이 유효한지
    #endregion

    #region privates
    private const int scoreRequiresToWin = 3; // 이기기 위해 필요한 판 수

    // 매 판 ClearData로 초기화
    private Dictionary<int, float> playersResponseTime = new Dictionary<int, float>();

    [Networked] float player1Time { get; set; }
    [Networked] float player2Time { get; set; }

    private bool isResultSent;
    #endregion

    #region SerializeField
    [SerializeField] float minTriggerTime;
    [SerializeField] float maxTriggerTime;

    [SerializeField] CustomMatch customMatch;
    #endregion

    // 플레이어 관련
    public Player localPlayer { get; private set; }
    [SerializeField] Player playerPrefab;

    public TextMeshProUGUI text;

    public GameManager Instance { get; private set; }

    private void OnEnable()
    {
        Player.OnPlayerClicked += ReceivePlayerClicked;
    }

    private void OnDisable()
    {
        Player.OnPlayerClicked -= ReceivePlayerClicked;
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            //StartNewGame();
        }
    }

    // 플레이어 입장 시 로컬 플레이어 생성 및 러너에 저장
    public void PlayerJoined(PlayerRef playerRef)
    {
        if (HasStateAuthority == false)
            return;

        //Debug.Log($"\nplayer joined {Runner.LocalPlayer.PlayerId}\n");

        var player = Runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, playerRef);
        Runner.SetPlayerObject(playerRef, player.Object);
        
        if (localPlayer == null || localPlayer.Object == null || localPlayer.Object.IsValid == false)
        {
            var playerObject = Runner.GetPlayerObject(Runner.LocalPlayer);
            localPlayer = playerObject != null ? playerObject.GetComponent<Player>() : null;
        }
        //Debug.Log($"\nplayer joined {Runner.LocalPlayer.PlayerId}\n");
        Debug.Log($"YYY player joined {playerRef.PlayerId}");

        if (Runner.SessionInfo.PlayerCount == 2)
        {
            StartNewGame();
        }
    }
    
    // 플레이어 퇴장 시
    public void PlayerLeft(PlayerRef player)
    {
        if (HasStateAuthority == false)
            return;

        // 필요 시 누구 나갔을 때 남은 플레이어 승리 메서드 구현
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        // 로컬 플레이어 초기화
        localPlayer = null;
    }

    /// <summary>
    /// 게임 세트를 시작한다.
    /// </summary>
    public void StartNewGame()
    {
        player1Score = 0;
        player2Score = 0;
        isGameActive = true;

        ClearData();

        StartRound();
    }

    /// <summary>
    /// 메인 게임 루프(라운드) 게임 종료 조건까지 게임을 진행시킨다.
    /// </summary>
    private async void StartRound()
    {


        if (player1Score == 3 || player2Score == 3)
            isGameActive = false;

        if (!isGameActive)
        {
            //EndGame();
        }

        if (Object.HasStateAuthority)
        {

            // 랜덤한 미니게임 결정 및 동기화
            //randomGameIndex = UnityEngine.Random.Range(0, (int)Define.GameMode.MaxCount);
            if (customMatch.customMiniGameIndex == 0)
                randomGameIndex = UnityEngine.Random.Range(1, 4);
            else
            {
                randomGameIndex = customMatch.customMiniGameIndex;
            }
            RPC_UpdateSelectedGame(randomGameIndex);

            // 뽑기 애니메이션 재생
            RPC_PlayGachaAnimation();
            await WaitForTickTimer(MiniGameManager.Instance.waitGachaTime);
            RPC_EndGachaAnimation();

            // 트리거 시간 결정
            triggerTime = UnityEngine.Random.Range(minTriggerTime, maxTriggerTime);
            // TODO
            // 미니게임마다 주기가 필요한경우(할리갈리) 해당 주기를 전달

            // 미니게임 로드 및 대기 & 트리거 타임 전달
            RPC_StartMiniGame(triggerTime);

            // await
            // 두 플레이어가
            await WaitForPlayerResultArrive();

            RPC_AnnounceWinner();
            await WaitForTickTimer(3);
            //StartRound();
            RPC_EndGame();

        }
    }

    #region RPC Methods
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_UpdateSelectedGame(int index)
    {
        MiniGameManager.Instance.UpdateSelectedMiniGame(index);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlayGachaAnimation()
    {
        MiniGameManager.Instance.PlayGachaAnimation();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_EndGachaAnimation()
    {
        MiniGameManager.Instance.EndGachaAnimation();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_StartMiniGame(float triggerTime)
    {
        MiniGameManager.Instance.UpdateTriggerTime(triggerTime);
        MiniGameManager.Instance.StartMiniGame();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_AnnounceWinner()
    {
        int winnerID = DetermineWiiner();
        //MiniGameManager.Instance.EndMiniGame(winnerID, playersResponseTime[1], playersResponseTime[2]);
        MiniGameManager.Instance.EndMiniGame(winnerID, player1Time, player2Time);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_EndGame()
    {
        MiniGameManager.Instance.EndGame();
        //UIManager.Instance.UpdateRunnerStatus("TITLE");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion


    // TickTimer 만료까지 비동기 대기
    async Task WaitForTickTimer(int sec)
    {
        tickTimer = TickTimer.CreateFromSeconds(Runner, sec);
        while (!tickTimer.Expired(Runner))
        {
            await Task.Yield();
        }
    }

    /// <summary>
    /// 플레이어에게서 정보를 받는다.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="responseTime"></param>
    /// <param name="isValid"></param>
    private void ReceivePlayerClicked(int playerID, float responseTime, bool isValid)
    {
        if (!isValid)
        {
            //playersResponseTime.Add(playerID, -1f);
            if (playerID == 1)
                player1Time = -1;
            else
                player2Time = -1;
            isResultSent = true;
        }
        else
        {
            //playersResponseTime.Add(playerID, responseTime);
            if (playerID == 1)
                player1Time = responseTime;
            else // 여기서 playerID 0으로 나오는 오류
                player2Time = responseTime;
            Debug.Log($"second player in, id is {playerID} : {responseTime}");
        }

        /*
        if (playersResponseTime.Count == 2 && !isResultSent)
        {
            isResultSent = true;
        }
        */
        if (player1Time != 0 && player2Time != 0 && !isResultSent)
            isResultSent = true;

        Debug.Log($"{playerID} in! {responseTime}");
        Debug.Log($"{player1Time} and {player2Time}");
        Debug.Log($"{isResultSent}");
    }

    /// <summary>
    /// 두 플레이어에게서 정보를 받을 때까지 대기하는 Task
    /// </summary>
    /// <returns></returns>
    async Task WaitForPlayerResultArrive()
    {
        while (!isResultSent)
        {
            await Task.Yield();
        }
    }

    /// <summary>
    /// 승자의 인덱스를 return
    /// </summary>
    /// <returns></returns>
    int DetermineWiiner()
    {
        if (player1Time == -1)
        {
            player2Score++;
            return 2;
        }
        else if (player2Time == -1)
        {
            player1Score++;
            return 1;
        }
        /*
        if (playersResponseTime[1] > playersResponseTime[2]) // player2 win
        {
            player2Score++;
            text.text = $"player2 ::: {playersResponseTime[2]} Win!!";
            return 2;
        }
        else
        {
            player1Score++;
            text.text = $"player1 ::: {playersResponseTime[1]} Win!!";
            return 1;
        }*/
        else if (player1Time > player2Time)
        {
            player2Score++;
            Debug.Log($"player2 ::: {player2Time} Win!! 1:::${player1Time}");
            return 2;
        }
        else if (player1Time < player2Time)
        {
            player1Score++;
            Debug.Log($"player1 ::: {player1Time} Win!!2:::${player2Time}");
            return 1;
        }
        else
        {
            Debug.Log("Same!");
            return 1;
        }
    }

    void ClearData()
    {
        player1Time = 0;
        player2Time = 0;
        isResultSent = false;
    }
}
```
</details>

<details>
<summary>멀티플레이 게임 흐름 동기화 (RPC 기반)</summary>

```{csharp}
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

    public int waitGachaTime { get; private set; }

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
```
</details>

<details>
<summary>확장 가능한 미니게임 구조 설계 (객체지향)</summary>

```{csharp}
public abstract class MiniGameBase : MonoBehaviour
{
    public abstract int miniGameIndex { get; }
    
    private MiniGameSO miniGameSo;
    private string guideTextValue;

    [SerializeField] protected TypewriterByCharacter typewritter;
    [SerializeField] protected TypewriterByCharacter playerText;
    [SerializeField] protected TypewriterByCharacter opponentText;
    [SerializeField] protected GameObject panel;
    [SerializeField] protected GameObject WinSignal;
    [SerializeField] protected GameObject LoseSignal;

    #region Initialize
    private void OnEnable()
    {
        OnMiniGameInitialized();
    }

    public virtual void OnMiniGameInitialized()
    {
        // SO에서 정보를 불러와 나타낼 미니게임 설명을 저장한다.
        miniGameSo = MiniGameManager.Instance._MiniGameSo;
        guideTextValue = miniGameSo.GetTextForMiniGame(miniGameIndex);
    }
    #endregion

    #region protected Method
    protected void ShowExplanationText()
    {
        typewritter.ShowText(guideTextValue);
    }

    protected void HideExplanationText()
    {
        typewritter.StartDisappearingText();
    }

    protected void ShowPlayerText(float time)
    {
        if (time == -1)
            playerText.ShowText("<rainb><wave a=0.2>too fast...");
        else if (time != 0)
        {
            playerText.ShowText($"<rainb><wave a=0.2>{time}");
        }
    }

    protected void ShowOpponentText(float time)
    {
        if (time == -1)
            opponentText.ShowText("<rainb><wave a=0.2>too fast...");
        else if (time != 0)
            opponentText.ShowText($"<rainb><wave a=0.2>{time}</wave>");
    }

    #endregion

    #region Virtual_공통 로직
    // public virtual
    #endregion

    #region Abstract_필수 구현
    /// <summary>
    /// 게임 시작하고 triggerEvent 전까지 실행
    /// </summary>
    public abstract void OnStandBy();

    /// <summary>
    /// 트리거 이벤트 실행
    /// </summary>
    public abstract void OnTriggerEvent();

    /// <summary>
    /// 플레이어가 눌렀을 때 컷인 재생
    /// </summary>
    public abstract void OnLocalPlayerClicked(float responseTime);

    /// <summary>
    /// 로컬 플레이어 우승
    /// </summary>
    public abstract void OnLocalPlayerWin(float opponentResponseTime);

    /// <summary>
    /// 로컬 플레이어 패배
    /// </summary>
    public abstract void OnLocalPlayerLose(float opponentResponseTime);

    //public abstract void OnLocalPlayerFail();
    //public abstract void OnOpponentPlayerWarn();
    #endregion
}

```
</details>

<details>
<summary>ScriptableObject 기반 데이터 중심 설계</summary>

```{csharp}
[CreateAssetMenu(fileName = "MiniGameSO", menuName = "Scriptable Objects/MiniGameSO")]
public class MiniGameSO : ScriptableObject
{
    /// <summary>
    /// 미니게임별 프리팹
    /// </summary>
    public MiniGameBase[] miniGamePrefabs;

    /// <summary>
    /// 미니게임별 게임 방법 설명하는 텍스트
    /// </summary>
    public string[] miniGameGuideTexts;


    public MiniGameBase GetMiniGamePrefab(int index)
    {
        if (index < 0 || index >= miniGamePrefabs.Length)
            return null;
        return miniGamePrefabs[index];
    }

    public string GetTextForMiniGame(int index)
    {
        if (index < 0 || index >= miniGameGuideTexts.Length)
            return null;
        return miniGameGuideTexts[index];
    }
}



[CreateAssetMenu(fileName = "SoundDB", menuName = "Scriptable Objects/SoundDB")]
public class SoundDB : ScriptableObject
{
    public SoundData[] bgmList;
    public SoundData[] sfxList;
}



[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    /// <summary>
    /// 사운드 이름
    /// </summary>
    public string soundName
    {
        get => this.name;
    }

    /// <summary>
    /// 재생할 AudioClip
    /// </summary>
    public AudioClip audioClip;

    /// <summary>
    /// 볼륨 (0-1)
    /// </summary>
    [Range(0f, 1f)] public float volume = 1f;

    /// <summary>
    /// 피치
    /// </summary>
    [Range(0f, 1f)] public float pitch = 1f;
}
```
</details>

<details>
<summary>런타임 시각 효과 및 물리 상호작용 구현</summary>

```{csharp}
public class PopcornEffect : MonoBehaviour
{
    public GameObject ballPrefab; // 하나의 공 프리팹
    public Sprite[] ballSprites;  // 16개의 공 이미지 배열

    [Header("Ball Count Range")]
    public int minBallCount = 2;   // 최소 공 개수 (Inspector에서 설정)
    public int maxBallCount = 6;  // 최대 공 개수 (Inspector에서 설정)

    public float explosionForce = 15f;  // 폭발력 (Inspector에서 설정)
    public float animationDuration = 1.5f; // 애니메이션 지속 시간
    public float fadeDuration = 0.5f;  // 페이드 아웃 시간

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // 마우스 클릭 감지
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;  // 2D 환경에서는 Z축을 0으로 설정

            CreateExplosion(clickPosition);
        }
    }

    public void CreateExplosion(Vector3 position)
    {
        SoundManager.Instance.PlaySFX("Bloop");
        // Inspector에서 지정한 범위 내에서 랜덤한 공 개수 설정
        int ballCount = Random.Range(minBallCount, maxBallCount + 1);

        for (int i = 0; i < ballCount; i++)
        {
            // 공 오브젝트 생성
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            SpriteRenderer spriteRenderer = ball.GetComponent<SpriteRenderer>();

            // 랜덤 스프라이트 적용
            int randomIndex = Random.Range(0, ballSprites.Length);
            spriteRenderer.sprite = ballSprites[randomIndex];

            // Rigidbody2D 추가 및 force 적용
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = ball.AddComponent<Rigidbody2D>();
            
            // Collider2D 비활성화 후 일정 시간 후 활성화
            Collider2D col = ball.GetComponent<Collider2D>();
            if (col == null)
                col = ball.AddComponent<CircleCollider2D>();  // 원하는 Collider 종류 선택
            col.enabled = false;  // 생성 시 충돌 비활성화

            DOVirtual.DelayedCall(0.4f, () =>
            {
                col.enabled = true;  // n초 후 충돌 활성화
            });

            // 랜덤 방향으로 force 적용 (Inspector의 explosionForce 값 사용)
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDirection * explosionForce, ForceMode2D.Impulse);

            // 투명도 조정 후 제거
            spriteRenderer.DOFade(0, fadeDuration)
                .SetDelay(animationDuration)
                .OnComplete(() => Destroy(ball));
        }
    }
}
```
</details>

전체 구현 코드는 [Scripts 폴더](QuickTrick/Assets/02.Scripts)를 참고해주세요.



