using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, INetworkSceneManager
{
    public string RoomName { get; set; }
    // 씬
    public bool IsBusy => throw new System.NotImplementedException();

    public Scene MainRunnerScene => throw new System.NotImplementedException();
    //씬끝

    [SerializeField, Tooltip("게임 시작 했을 때 생성될 네트워크 러너 프리팹")]
    private NetworkRunner _networkRunnerPrefab;
    private NetworkRunner networkRunner;

    [SerializeField, Tooltip("매칭 대기 중에 띄워질 대기UI")]
    private CanvasGroup waitingUI;

    private const int maxPlayer = 2;
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
        // 네트워크 러너 초기화
        networkRunner = Instantiate(_networkRunnerPrefab);

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
            Debug.Log($"Failed to match /// members :{networkRunner.SessionInfo.PlayerCount}");
        }
    }

    private IEnumerator WaitFor2Players()
    {
        while (networkRunner.SessionInfo.PlayerCount < maxPlayer)
        {
            Debug.Log("Try Game");
            yield return new WaitForSeconds(1f);
        }
        // 총 제한 시간 주고, 제한 시간 내에 실패하면 원점으로 돌리는 기능 구현하기

        //게임이 시작됩니다.
        HideWaitingUI();
        StartGame();
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
        networkRunner.LoadScene(SceneRef.FromIndex(2), LoadSceneMode.Additive);
        networkRunner.UnloadScene(SceneRef.FromIndex(1));
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

        // Disconnecting...
        await networkRunner.Shutdown();
        networkRunner = null;

        SceneManager.LoadScene(1);
    }

    public void TestNewScene()
    {
        //SceneManager.LoadScene(2);
        if (networkRunner == null)
            networkRunner = Instantiate(_networkRunnerPrefab);
        //networkRunner.UnloadScene(SceneRef.FromIndex(1));
        networkRunner.InvokeSceneLoadStart(SceneRef.FromIndex(1));

        if (networkRunner.IsSceneAuthority)
        {
            Debug.Log(" 언로드 씬");
            networkRunner.LoadScene(SceneRef.FromIndex(2));
            networkRunner.UnloadScene(SceneRef.FromIndex(1));
        }
        else
        {
            Debug.Log("NO");
        }
        //networkRunner.LoadScene(SceneRef.FromIndex(2));
    }

    //씬
    public void Initialize(NetworkRunner runner)
    {
        throw new System.NotImplementedException();
    }

    public void Shutdown()
    {
        throw new System.NotImplementedException();
    }

    public bool IsRunnerScene(Scene scene)
    {
        throw new System.NotImplementedException();
    }

    public bool TryGetPhysicsScene2D(out PhysicsScene2D scene2D)
    {
        throw new System.NotImplementedException();
    }

    public bool TryGetPhysicsScene3D(out PhysicsScene scene3D)
    {
        throw new System.NotImplementedException();
    }

    public void MakeDontDestroyOnLoad(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    public bool MoveGameObjectToScene(GameObject gameObject, SceneRef sceneRef)
    {
        throw new System.NotImplementedException();
    }

    public NetworkSceneAsyncOp LoadScene(SceneRef sceneRef, NetworkLoadSceneParameters parameters)
    {
        throw new System.NotImplementedException();
    }

    public NetworkSceneAsyncOp UnloadScene(SceneRef sceneRef)
    {
        throw new System.NotImplementedException();
    }

    public SceneRef GetSceneRef(GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }

    public SceneRef GetSceneRef(string sceneNameOrPath)
    {
        throw new System.NotImplementedException();
    }

    public bool OnSceneInfoChanged(NetworkSceneInfo sceneInfo, NetworkSceneInfoChangeSource changeSource)
    {
        throw new System.NotImplementedException();
    }
}
