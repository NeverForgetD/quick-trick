using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchMaker : MonoBehaviour
{
    // ��Ʈ��ũ ���� ����
    [SerializeField, Tooltip("��Ʈ��ũ ���� ������")]
    private NetworkRunner RunnerPrefab;
    private NetworkRunner _runnerInstance;
    private static string _shutdownStatus;

    // ��Ī ���� ����
    public TMP_InputField RoomText;
    private const int maxPlayerCount = 2; // �÷��̾� ��

    //��Ī ��� ����
    private const float maxWaitingTime = 10f; // ��Ī �ִ� ��� �ð� (��) _ 30�ʷ� ����, �׽�Ʈȯ�� 10��
    public float elapsedTime { get; private set; } // ���� ��� �ð� (��)

    private void Start()
    {
        MatchGame();
    }

    private void InitRunner()
    {
        if (RunnerPrefab == null)
            RunnerPrefab = Resources.Load<NetworkRunner>("Prefabs/NetworkRunner");

        _runnerInstance = Instantiate(RunnerPrefab);
    }

    private async void MatchGame(bool joinRandomRoom = true)
    {
        await Disconnect();
        UIManager.Instance.UpdateRunnerStatus("CONNECTINGSERVER");

        // PlayerPrefs.SetString("PlayerName", NickNameText.text);

        InitRunner();

        var events = _runnerInstance.GetComponent<NetworkEvents>();
        events.OnShutdown.AddListener(OnShutDown);

        var sceneInfo = new NetworkSceneInfo();
        sceneInfo.AddSceneRef(SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex));

        var startArguments = new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            Scene = sceneInfo,
            PlayerCount = maxPlayerCount,
            MatchmakingMode = Fusion.Photon.Realtime.MatchmakingMode.FillRoom,
            SessionName = joinRandomRoom ? string.Empty : RoomText.text, // �׳� RoomText.text �ص� �Ǵµ�...
        };


        var startTask = _runnerInstance.StartGame(startArguments);
        await startTask;

        if (startTask.Result.Ok)
        {
            //����
            UIManager.Instance.UpdateRunnerStatus("WAITING");
            StartCoroutine(WaitFor2Players());
        }
        else
        {
            //����
        }
    }

    // STATUS WAITING�� ���� ��, 2���� ���� ���� ��ٸ��� �Ѵ�.
    private IEnumerator WaitFor2Players()
    {
        elapsedTime = 0f;
        while (elapsedTime < maxWaitingTime)
        {
            if (_runnerInstance.SessionInfo.PlayerCount == maxPlayerCount)
            {
                //UIManager.Instance.ChangeMainMenuUI("FINDROOM");
                //yield return new WaitForSeconds(1f); // UI �߰��� ���� ��� ��� "�� ������ ���۵˴ϴ�."
                StartGame();
                yield break;
            }

            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
            Debug.Log($"��� �ð� : {elapsedTime}, ���� �ο� : {_runnerInstance.SessionInfo.PlayerCount}");
        }

        OnTimeOut();
    }

    private void StartGame()
    {
        // ���ΰ��� ��ũ��Ʈ�� ���ӽ��� �޼��� ����
    }

    /*
    public void OnTimeOut()
    {
        // �ٽ� ��Ī, �޴��� ���ư��� UI ����
        // �ϴ� ���� ����
        
    }
    */

    // TEMP
    private async void OnTimeOut()
    {
        await Disconnect();
        SceneManager.LoadScene(1);
    }

    public async void DisconnectClicked()
    {
        await Disconnect();
    }

    public async void BackToMenu()
    {
        await Disconnect();

        //SceneManager.LoadScene();
    }

    public async Task Disconnect()
    {
        if (_runnerInstance == null)
            return;

        // ������� UI
        UIManager.Instance.UpdateRunnerStatus("DISCONNECTING");
        
        var events = _runnerInstance.GetComponent<NetworkEvents>();
        events.OnShutdown.RemoveListener(OnShutDown);


        await _runnerInstance.Shutdown();
        _runnerInstance= null;

        //UI �������
        UIManager.Instance.UpdateRunnerStatus("NONE");
    }

    private void OnShutDown(NetworkRunner runner, ShutdownReason reason)
    {
        //  ����ġ ���� �˴ٿ� �߻� ��, �޽��� ���
        _shutdownStatus = $"Shutdown: {reason}";
        Debug.LogWarning(_shutdownStatus);

        // �� ���� �� ���ε�
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

