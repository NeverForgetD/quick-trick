using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour // UI. 반영 상태를 알려주는 enum을 저장하는 매니저
{

    #region Singleton

    #endregion
    static UIManager instance;
    public static UIManager Instance { get { Init(); return instance; } }

    public delegate void RunnerStatusUpdated(Define.RunnerStatus newState); // 상태 변경 시 호출될 델리게이트
    public event RunnerStatusUpdated OnRunnerStatusUpdated; // 상태 변경 이벤트

    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("@UIManager");
            if (go == null)
            {
                go = new GameObject { name = "@UIManager" };
                go.AddComponent<UIManager>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<UIManager>();
        }
    }

    public Define.RunnerStatus UpdateRunnerStatus(string state)
    {
        // RunnerStatus 상태 변경 시도 & 상태 변경 이벤트를 통해 MatchMaker에게 알리기
        if (System.Enum.TryParse(state, true, out Define.RunnerStatus newState))
        {
            OnRunnerStatusUpdated?.Invoke(newState);
            return newState;
        }
        else
            throw new ArgumentException($"Invalid game state: {state}");

    }
}
