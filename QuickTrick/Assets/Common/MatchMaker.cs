using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchMaker : MonoBehaviour
{
    // 네트워크 러너 관련
    [SerializeField, Tooltip("네트워크 러너 프리팹, 지정안해줘도 알아서 찾음")]
    private NetworkRunner RunnerPrefab;
    private NetworkRunner _runnerInstance;
    private static string _shutdownStatus;

    // 매칭 정보 관련
    public TMP_InputField RoomText;
    private const int maxPlayerCount = 2; // 플레이어 수

    //매칭 대기 관련
    private const float maxWaitingTime = 5f; // 매칭 최대 대기 시간 (초) _ 30초로 변경, 테스트환경 n초
    public float elapsedTime { get; private set; } // 현재 대기 시간 (초)

    private void Start()
    {
        MatchGame();
    }

    private void InitRunner()
    {
        if (RunnerPrefab == null)
            RunnerPrefab = Resources.Load<NetworkRunner>("Prefabs/NetworkRunner");

        _runnerInstance = Instantiate(RunnerPrefab);
        DontDestroyOnLoad(_runnerInstance);
    }

    public async void MatchGame(bool joinRandomRoom = true)
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
            SessionName = joinRandomRoom ? string.Empty : RoomText.text, // 그냥 RoomText.text 해도 되는듯...
        };


        var startTask = _runnerInstance.StartGame(startArguments);
        await startTask;

        if (startTask.Result.Ok)
        {
            //성공
            UIManager.Instance.UpdateRunnerStatus("WAITING");
            StartCoroutine(WaitFor2Players());
        }
        else
        {
            //실패
            //UIManager.Instance.UpdateRunnerStatus("");
            // 실패하는 경우가 없기는 하지만 나중에 에러 메시지 출력 기능 추가해야한다.
            await Disconnect();
            SceneManager.LoadScene(1);
        }
    }

    // STATUS WAITING이 됐을 때, 2명이 오기 까지 기다리게 한다.
    private IEnumerator WaitFor2Players()
    {
        elapsedTime = 0f;
        while (elapsedTime < maxWaitingTime)
        {
            if (_runnerInstance.SessionInfo.PlayerCount == maxPlayerCount)
            {
                //UIManager.Instance.ChangeMainMenuUI("FINDROOM");
                //yield return new WaitForSeconds(1f); // UI 추가를 위해 잠시 대기 "곧 게임이 시작됩니다."
                StartGame();
                yield break;
            }

            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
            Debug.Log($"경과 시간 : {elapsedTime}, 참여 인원 : {_runnerInstance.SessionInfo.PlayerCount}");
        }

        // 1초 미만의 시간차로 접속이 되면 한 클라이언트만 룸에 참가되는 오류 방지
        if (_runnerInstance.SessionInfo.PlayerCount == maxPlayerCount)
        {
            StartGame();
        }
        else
        {
            OnTimeOut();
        }
    }

    public GameObject testGameMode;
    private void StartGame()
    {
        // 메인게임 스크립트의 게임시작 메서드 실행
        UIManager.Instance.UpdateRunnerStatus("GAME");
        Instantiate(testGameMode);
    }

    
    public void OnTimeOut()
    {
        UIManager.Instance.UpdateRunnerStatus("TIMEOUT");
    }
    

    // TEMP
    /*
    private async void OnTimeOut()
    {
        //await Disconnect();
        //SceneManager.LoadScene(1);
        //디스커넥트 안 하고 다시 매칭 했을 떄도 유효한 지 확인해야함


    }
    */

    // 아직 안 쓰는 기능
    public async void DisconnectClicked()
    {
        await Disconnect();
    }

    public async void BackToMenu()
    {
        await Disconnect();
        SceneManager.LoadScene(1);
    }

    public async Task Disconnect()
    {
        if (_runnerInstance == null)
            return;

        // 연결끊기 UI
        UIManager.Instance.UpdateRunnerStatus("DISCONNECTING");
        
        var events = _runnerInstance.GetComponent<NetworkEvents>();
        events.OnShutdown.RemoveListener(OnShutDown);


        await _runnerInstance.Shutdown();
        _runnerInstance= null;

        //UI 원래대로
        UIManager.Instance.UpdateRunnerStatus("NONE");
    }

    private void OnShutDown(NetworkRunner runner, ShutdownReason reason)
    {
        //  예상치 못한 셧다운 발생 시, 메시지 출력
        _shutdownStatus = $"Shutdown: {reason}";
        Debug.LogWarning(_shutdownStatus);

        // 씬 리셋 후 리로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

