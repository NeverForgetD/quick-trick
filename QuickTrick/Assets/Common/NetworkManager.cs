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

    [SerializeField, Tooltip("���� ���� ���� �� ������ ��Ʈ��ũ ���� ������")]
    private NetworkRunner _networkRunnerPrefab;
    private NetworkRunner networkRunner;

    private const int maxPlayer = 2;
    private float maxWaitingTime = 30.0f;


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

    // Ŀ���� ��Ī �ʿ�� joinRandomRoom ���
    public async void MatchGame(bool joinRandomRoom = true)
    {
        await Disconnect();
        UIManager.Instance.mainMenuUI = Define.MainMenuUI.CONNECTINGSERVER;

        // ��Ʈ��ũ ���� �ʱ�ȭ
        networkRunner = Instantiate(_networkRunnerPrefab); // ������ Ȯ���ϰ� ���� �ʿ�
        DontDestroyOnLoad(networkRunner);
        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = joinRandomRoom ? string.Empty : RoomName, // ���� �̸��� joinRandomRoom�� true�̸� empty, false�̸� �����(���߿� ��ǲ �޾Ƽ� ����)���� �Ѵ�
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount= maxPlayer,
            MatchmakingMode = Fusion.Photon.Realtime.MatchmakingMode.FillRoom,
        });

        if (result.Ok)            // ��Ī ����
        {
            UIManager.Instance.mainMenuUI = Define.MainMenuUI.WAITING;
            StartCoroutine(WaitFor2Players());
        }
        else            // ��Ī ����
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
                UIManager.Instance.mainMenuUI = Define.MainMenuUI.FINDROOM;
                // yield return new WaitForSeconds(2f); // 2�� �� ����
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
        UIManager.Instance.mainMenuUI = Define.MainMenuUI.TIMEOUT;
        await Disconnect();
    }

    private async void GoToGame()
    {
        if (networkRunner != null && networkRunner.SessionInfo != null)
        {
            Debug.Log("Success to play");
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
    }
}
