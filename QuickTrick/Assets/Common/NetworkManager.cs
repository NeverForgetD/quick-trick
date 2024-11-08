using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    static NetworkManager instance;
    public static NetworkManager Instance { get { Init(); return instance; } }

    public string RoomName { get; set; }

    [SerializeField, Tooltip("���� ���� ��ư�� ������ �� ������ ��Ʈ��ũ ���� ������")]
    private NetworkRunner _networkRunnerPrefab;
    private NetworkRunner networkRunner;

    [SerializeField, Tooltip("���� ���������� ���� ������ �ֵ��� �Ŵ��� ������")]
    private GameManager _gameManagerPrefab;

    private const int maxPlayer = 2;
    private float maxWaitingTime = 10.0f;

    public float elapsedTime { get; private set; }

    private void Start()
    {
        Init();
    }
    
    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("@NetworkManager");
            if (go == null)
            {
                go = new GameObject { name = "@NetworkManager" };
                go.AddComponent<NetworkManager>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<NetworkManager>();
        }
    }

    private void InitPrefabs()
    {
        if (_networkRunnerPrefab == null)
        {
            _networkRunnerPrefab = Resources.Load<NetworkRunner>("Prefabs/NetworkRunner");
        }
        if (_gameManagerPrefab == null)
        {
            _gameManagerPrefab = Resources.Load<GameManager>("Prefabs/@GameManager");
        }
    }

    // Ŀ���� ��Ī �ʿ�� joinRandomRoom = false ���
    public async void MatchGame(bool joinRandomRoom = true)
    {
        await Disconnect();
        UIManager.Instance.ChangeMainMenuUI("CONNECTINGSERVER");

        // ��Ʈ��ũ ���� & ���ӸŴ��� ������ �ʱ�ȭ
        InitPrefabs();
        networkRunner = Instantiate(_networkRunnerPrefab);
        DontDestroyOnLoad(networkRunner);

        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = joinRandomRoom ? string.Empty : RoomName, // ���� �̸��� joinRandomRoom�� true�̸� empty, false�̸� �����(���߿� ��ǲ �޾Ƽ� ����)���� �Ѵ�
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount= maxPlayer,
            MatchmakingMode = Fusion.Photon.Realtime.MatchmakingMode.FillRoom,
        });
        
        /*
        var startArguments = new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = joinRandomRoom ? string.Empty : RoomName, // ���� �̸��� joinRandomRoom�� true�̸� empty, false�̸� �����(���߿� ��ǲ �޾Ƽ� ����)���� �Ѵ�
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = maxPlayer,
            MatchmakingMode = Fusion.Photon.Realtime.MatchmakingMode.FillRoom,
        };

        var startTask = networkRunner.StartGame(startArguments);
        await startTask;
        if (startTask.Result.Ok)
        {
            Debug.Log("����");
        }
        */
        
        if (result.Ok)            // ��Ī ����
        {
            UIManager.Instance.ChangeMainMenuUI("WAITING");
            StartCoroutine(WaitFor2Players());
        }
        else            // ��Ī ����
        {
            Debug.Log($"Failed to match... [Server Error]");
        }
        
    }

    private IEnumerator WaitFor2Players()
    {
        elapsedTime = 0f;
        while (elapsedTime < maxWaitingTime)
        {
            if (networkRunner.SessionInfo.PlayerCount == maxPlayer)
            {
                UIManager.Instance.ChangeMainMenuUI("FINDROOM");
                yield return new WaitForSeconds(1f); // UI �߰��� ���� ��� ��� (�����ص� ������) _ 
                GoToGame();
                yield break;
            }

            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
            Debug.Log($"��� �ð� : {elapsedTime}, ���� �ο� : {networkRunner.SessionInfo.PlayerCount}");
        }

        OnTimeOut();
    }

    private async void OnTimeOut()
    {
        // disconnect ���ְ� ��ư���� �����
        // disconnect or try again
        await Disconnect();
        UIManager.Instance.ChangeMainMenuUI("TIMEOUT");
    }

    // �� �׽�Ʈ �κ�
    private async void GoToGame()
    {
        if (networkRunner != null && networkRunner.SessionInfo != null)
        {
            //GameManager go;
            //go = networkRunner.Spawn(_gameManagerPrefab);
            //DontDestroyOnLoad(go);
            SceneManager.LoadScene(2);

        }
        else
        {
            await Disconnect();
        }
    }

    // ��Ȳ�� ���� �ٸ� �α׸� ����ϴ� Disconnect ���...�ϴ� ���߿�
    /*
    private void QuitGame()
    {
        if (networkRunner != null)
        {
            // �� ������
            // ������
        }
    }
    */

    public async Task Disconnect()
    {
        if (networkRunner == null)
            return;

        await networkRunner.Shutdown();
        networkRunner = null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UIManager.Instance.ChangeMainMenuUI("None");
    }
}
