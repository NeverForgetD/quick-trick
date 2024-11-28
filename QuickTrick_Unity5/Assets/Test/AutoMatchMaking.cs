using System;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoMatchMaking : MonoBehaviour
{
    private NetworkRunner runnerPrefab;
    private NetworkRunner runnerInstance;

    
    private void InitRunner()
    {
        if (runnerInstance == null)
        {
            runnerPrefab = Resources.Load<NetworkRunner>("Prefabs/NetworkRunner");
            runnerInstance = Instantiate(runnerPrefab);
            DontDestroyOnLoad(runnerInstance);
        }
    }

    private void Start()
    {
        MatchGame();
    }
    
    public async void MatchGame()
    {
        await Disconnect();
        // 네트워크 러너 & 게임매니저 프리팹 초기화
        InitRunner();
        //InitPrefabs();

        var result = await runnerInstance.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            PlayerCount = 2,
            MatchmakingMode = Fusion.Photon.Realtime.MatchmakingMode.FillRoom,
        });
        /*
        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = joinRandomRoom ? string.Empty : RoomName, // 세션 이름은 joinRandomRoom이 true이면 empty, false이면 룸네임(나중에 인풋 받아서 저장)으로 한다
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = maxPlayer,
            MatchmakingMode = Fusion.Photon.Realtime.MatchmakingMode.FillRoom,
        });
        */
    }

    public async Task Disconnect()
    {
        if (runnerInstance == null)
            return;

        await runnerInstance.Shutdown();
        runnerInstance = null;

        SceneManager.LoadScene(3);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //UIManager.Instance.ChangeMainMenuUI("None");
    }
}
