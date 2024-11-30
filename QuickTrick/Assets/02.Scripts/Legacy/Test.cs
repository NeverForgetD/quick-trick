using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Test : MonoBehaviour
{
    public TextMeshProUGUI testText;
    private NetworkRunner _runnerInstance;
    private void InitInstance()
    {
        if (_runnerInstance == null)
            _runnerInstance = FindFirstObjectByType<NetworkRunner>();
    }

    private void ShowRoomNumber()
    {
        InitInstance();
        string text = $"Session name : {_runnerInstance.SessionInfo.Name}\n," +
            $"player : {_runnerInstance.SessionInfo.PlayerCount}\n," +
            $"player ID : {_runnerInstance.LocalPlayer.PlayerId}\n," +
            $"IsServer : {_runnerInstance.IsServer}";
        testText.text = text;

    }

    private void OnEnable()
    {
        //ShowRoomNumber();
    }

    private void Update()
    {
        //ShowRoomNumber();
    }
}
