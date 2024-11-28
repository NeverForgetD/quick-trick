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
        // ��Ʈ��ũ ���� & ���ӸŴ��� ������ �ʱ�ȭ
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
            SessionName = joinRandomRoom ? string.Empty : RoomName, // ���� �̸��� joinRandomRoom�� true�̸� empty, false�̸� �����(���߿� ��ǲ �޾Ƽ� ����)���� �Ѵ�
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
