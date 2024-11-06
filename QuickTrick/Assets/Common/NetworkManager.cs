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

    [SerializeField, Tooltip("���� ���� ���� �� ������ ��Ʈ��ũ ���� ������")]
    private NetworkRunner _networkRunnerPrefab;
    private NetworkRunner networkRunner;

    [SerializeField, Tooltip("��Ī ��� �߿� ����� ���UI")]
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
        await Disconnect();

        // ��Ʈ��ũ ���� �ʱ�ȭ
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

        if (result.Ok)            // ��Ī ����
        {
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
                HideWaitingUI();
                StartGame();
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
        await Disconnect();
        ShowTimeOutPopup();
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
        //networkRunner.LoadScene(SceneRef.FromIndex(2), LoadSceneMode.Additive);
        //networkRunner.UnloadScene(SceneRef.FromIndex(1));
        SceneManager.LoadScene(2);
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

        await networkRunner.Shutdown();
        networkRunner = null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowTimeOutPopup()
    {

    }

}
