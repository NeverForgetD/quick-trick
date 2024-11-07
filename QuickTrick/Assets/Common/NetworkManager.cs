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

    //[SerializeField, Tooltip("게임 시작 했을 때 생성될 네트워크 러너 프리팹")]
    private NetworkRunner _networkRunnerPrefab;
    private NetworkRunner networkRunner;

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

    private void InitiateRunner()
    {
        if (_networkRunnerPrefab == null)
        {
            _networkRunnerPrefab = Resources.Load<NetworkRunner>("Prefabs/NetworkRunner");
        }

        networkRunner = Instantiate(_networkRunnerPrefab); // 프리팹 확실하게 저장 필요
        DontDestroyOnLoad(networkRunner);
    }

    // 커스텀 매칭 필요시 joinRandomRoom 사용
    public async void MatchGame(bool joinRandomRoom = true)
    {
        await Disconnect();
        UIManager.Instance.ChangeMainMenuUI("CONNECTINGSERVER");

        // 네트워크 러너 초기화
        InitiateRunner();
        //networkRunner = Instantiate(_networkRunnerPrefab); // 프리팹 확실하게 저장 필요
        //DontDestroyOnLoad(networkRunner);
        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = joinRandomRoom ? string.Empty : RoomName, // 세션 이름은 joinRandomRoom이 true이면 empty, false이면 룸네임(나중에 인풋 받아서 저장)으로 한다
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount= maxPlayer,
            MatchmakingMode = Fusion.Photon.Realtime.MatchmakingMode.FillRoom,
        });

        if (result.Ok)            // 매칭 성공
        {
            UIManager.Instance.ChangeMainMenuUI("WAITING");
            StartCoroutine(WaitFor2Players());
        }
        else            // 매칭 실패
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
                yield return new WaitForSeconds(1f); // UI 추가를 위해 잠시 대기 (삭제해도 괜찮음) _ 
                GoToGame();
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
        // disconnect 없애고 버튼으로 만들기
        // disconnect or try again
        await Disconnect();
        UIManager.Instance.ChangeMainMenuUI("TIMEOUT");
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

    

    // 상황에 따라 다른 로그를 사용하는 Disconnect 방법...일단 나중에
    /*
    private void QuitGame()
    {
        if (networkRunner != null)
        {
            // 방 나가기
            // 나가기
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
