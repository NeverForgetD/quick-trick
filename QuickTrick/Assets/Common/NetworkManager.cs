using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    static NetworkManager instance;
    static NetworkManager Instance { get { return instance; } }

    public string RoomName { get; set; }

    [SerializeField, Tooltip("게임 시작 했을 때 생성될 네트워크 러너 프리팹")]
    private NetworkRunner _networkRunnerPrefab;
    private NetworkRunner networkRunner;

    [SerializeField, Tooltip("매칭 대기 중에 띄워질 대기UI")]
    private CanvasGroup waitingUI;

    private const int maxPlayer = 2;
    [SerializeField] private float maxWaitingTime = 5.0f;
    /*
    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }*/

    private void Start()
    {
        // 대기 UI 처음에 숨김
        HideWaitingUI();
    }

    //랜덤 게임 참여
    public void OnPlayButtonClick()
    {
        // play버튼 비황성화 및 효과
        ShowWaitingUI();

        MatchGame(true); // 랜덤 게임 참여
    }

    // 아직 개발 X, 커스텀 대결
    public void OnCustomPlayButtonClick()
    {
        ShowWaitingUI();
        // MatchGame(false);
    }

    public async void MatchGame(bool joinRandomRoom)
    {
        await Disconnect();

        // 네트워크 러너 초기화
        networkRunner = Instantiate(_networkRunnerPrefab);
        DontDestroyOnLoad(networkRunner);
        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = joinRandomRoom ? string.Empty : RoomName,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount= maxPlayer,
            MatchmakingMode = Fusion.Photon.Realtime.MatchmakingMode.FillRoom,
        });

        if (result.Ok)            // 매칭 성공
        {
            StartCoroutine(WaitFor2Players());
        }
        else            // 매칭 실패
        {
            Debug.Log($"Failed to match... [Server Error]");
        }
    }

    private IEnumerator WaitFor2Players()
    {
        float elapsedTime = 0f;
        while (elapsedTime < maxWaitingTime)
        {
            if (networkRunner.SessionInfo.PlayerCount == maxPlayer)
            {
                HideWaitingUI();
                StartGame();
                yield break;
            }

            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
            Debug.Log($"경과 시간 : {elapsedTime}, 참여 인원 : {networkRunner.SessionInfo.PlayerCount}");
        }

        OnTimeOut();
    }

    private async void OnTimeOut()
    {
        await Disconnect();
        ShowTimeOutPopup();
    }

    // 플레이어 모였는지 재차 확인
    private void StartGame()
    {
        if (networkRunner != null && networkRunner.SessionInfo != null)
        {
            if (networkRunner.SessionInfo.PlayerCount == maxPlayer)
            {
                GoToGame();
            }
        }
    }

    private void GoToGame()
    {
        Debug.Log("Success to play");
        //networkRunner.LoadScene(SceneRef.FromIndex(2), LoadSceneMode.Additive);
        //networkRunner.UnloadScene(SceneRef.FromIndex(1));
        SceneManager.LoadScene(2);
    }

    private void QuitGame()
    {
        if (networkRunner != null)
        {
            // 방 나가기
            // 나가기
        }
    }


    private void ShowWaitingUI()
    {
        waitingUI.alpha = 1;
    }

    private void HideWaitingUI()
    {
        waitingUI.alpha = 0;
    }

    public async Task Disconnect()
    {
        if (networkRunner == null)
            return;

        await networkRunner.Shutdown();
        networkRunner = null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowTimeOutPopup()
    {

    }

}
