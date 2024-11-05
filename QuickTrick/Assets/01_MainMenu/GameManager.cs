using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, INetworkSceneManager
{
    public string RoomName { get; set; }
    // ��
    public bool IsBusy => throw new System.NotImplementedException();

    public Scene MainRunnerScene => throw new System.NotImplementedException();
    //����

    [SerializeField, Tooltip("���� ���� ���� �� ������ ��Ʈ��ũ ���� ������")]
    private NetworkRunner _networkRunnerPrefab;
    private NetworkRunner networkRunner;

    [SerializeField, Tooltip("��Ī ��� �߿� ����� ���UI")]
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
        // ��� UI ó���� ����
        HideWaitingUI();
    }

    //���� ���� ����
    public void OnPlayButtonClick()
    {
        // play��ư ��Ȳ��ȭ �� ȿ��
        ShowWaitingUI();

        MatchGame(true); // ���� ���� ����
    }

    // ���� ���� X, Ŀ���� ���
    public void OnCustomPlayButtonClick()
    {
        ShowWaitingUI();
        // MatchGame(false);
    }

    public async void MatchGame(bool joinRandomRoom)
    {
        // ��Ʈ��ũ ���� �ʱ�ȭ
        networkRunner = Instantiate(_networkRunnerPrefab);

        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = joinRandomRoom ? string.Empty : RoomName,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount= maxPlayer,
            MatchmakingMode = Fusion.Photon.Realtime.MatchmakingMode.FillRoom,
        });

        if (result.Ok)            // ��Ī ����
        {
            StartCoroutine(WaitFor2Players());
        }
        else            // ��Ī ����
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
        // �� ���� �ð� �ְ�, ���� �ð� ���� �����ϸ� �������� ������ ��� �����ϱ�

        //������ ���۵˴ϴ�.
        HideWaitingUI();
        StartGame();
    }

    // �÷��̾� �𿴴��� ���� Ȯ��
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
            // �� ������
            // ������
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
            Debug.Log(" ��ε� ��");
            networkRunner.LoadScene(SceneRef.FromIndex(2));
            networkRunner.UnloadScene(SceneRef.FromIndex(1));
        }
        else
        {
            Debug.Log("NO");
        }
        //networkRunner.LoadScene(SceneRef.FromIndex(2));
    }

    //��
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
