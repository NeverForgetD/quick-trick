using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour // UI. �ݿ� ���¸� �˷��ִ� enum�� �����ϴ� �Ŵ���
{

    #region Singleton

    #endregion
    static UIManager instance;
    public static UIManager Instance { get { Init(); return instance; } }

    public delegate void RunnerStatusUpdated(Define.RunnerStatus newState); // ���� ���� �� ȣ��� ��������Ʈ
    public event RunnerStatusUpdated OnRunnerStatusUpdated; // ���� ���� �̺�Ʈ

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
        // RunnerStatus ���� ���� �õ� & ���� ���� �̺�Ʈ�� ���� MatchMaker���� �˸���
        if (System.Enum.TryParse(state, true, out Define.RunnerStatus newState))
        {
            OnRunnerStatusUpdated?.Invoke(newState);
            return newState;
        }
        else
            throw new ArgumentException($"Invalid game state: {state}");

    }
}
