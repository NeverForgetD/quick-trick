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

    [SerializeField, Tooltip("게임 시작 했을 때 생성될 네트워크 러너 프리팹")]
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

    // 커스텀 매칭 필요시 joinRandomRoom 사용
    public async void MatchGame(bool joinRandomRoom = true)
    {
        await Disconnect();
        UIManager.Instance.mainMenuUI = Define.MainMenuUI.CONNECTINGSERVER;

        // 네트워크 러너 초기화
        networkRunner = Instantiate(_networkRunnerPrefab); // 프리팹 확실하게 저장 필요
        DontDestroyOnLoad(networkRunner);
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
            UIManager.Instance.mainMenuUI = Define.MainMenuUI.WAITING;
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
                UIManager.Instance.mainMenuUI = Define.MainMenuUI.FINDROOM;
                // yield return new WaitForSeconds(2f); // 2초 후 입장
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
    }
}
