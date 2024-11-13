using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TextMeshProUGUI testText;
    private NetworkRunner _runnerInstance;
    private void InitInstance()
    {
        if (_runnerInstance == null)
            _runnerInstance = FindObjectOfType<NetworkRunner>();
    }

    private void ShowRoomNumber()
    {
        InitInstance();
        testText.text = $"Session name : {_runnerInstance.SessionInfo.Name}, player : {_runnerInstance.SessionInfo.PlayerCount}";
    }

    private void OnEnable()
    {
        //ShowRoomNumber();
    }

    private void Update()
    {
        ShowRoomNumber();
    }
}
