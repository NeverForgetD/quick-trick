using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour // UI. �ݿ� ���¸� �˷��ִ� enum�� �����ϴ� �Ŵ���...
{
    static UIManager instance;
    public static UIManager Instance { get { Init(); return instance; } }

    public Define.MainMenuUI mainMenuUI { get; private set; }

    public delegate void MainMenuUIChanged(Define.MainMenuUI newState); // ���� ���� �� ȣ��� ��������Ʈ
    public event MainMenuUIChanged OnMainMenuUIChanged; // ���� ���� �̺�Ʈ

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
    /// <summary>
    ///         NONE, CONNECTINGSERVER, WAITING, FINDROOM, TIMEOUT, �߿� ���ÿ�
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Define.MainMenuUI ChangeMainMenuUI(string state)
    {
        // MainMenuUI ���� ���� �õ� & ���� ���� �̺�Ʈ�� ���� UIMainMenu���� �˸���
        if (System.Enum.TryParse(state, true, out Define.MainMenuUI newState))
        {
            OnMainMenuUIChanged?.Invoke(newState);
            return newState;
        }
        else
            throw new ArgumentException($"Invalid game state: {state}");

    }
}
