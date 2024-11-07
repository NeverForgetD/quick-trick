using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour // UI. 반영 상태를 알려주는 enum을 저장하는 매니저...
{
    static UIManager instance;
    public static UIManager Instance { get { Init(); return instance; } }

    public Define.MainMenuUI mainMenuUI { get; private set; }

    public delegate void MainMenuUIChanged(Define.MainMenuUI newState); // 상태 변경 시 호출될 델리게이트
    public event MainMenuUIChanged OnMainMenuUIChanged; // 상태 변경 이벤트

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
    ///         NONE, CONNECTINGSERVER, WAITING, FINDROOM, TIMEOUT, 중에 고르시오
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Define.MainMenuUI ChangeMainMenuUI(string state)
    {
        // MainMenuUI 상태 변경 시도 & 상태 변경 이벤트를 통해 UIMainMenu에게 알리기
        if (System.Enum.TryParse(state, true, out Define.MainMenuUI newState))
        {
            OnMainMenuUIChanged?.Invoke(newState);
            return newState;
        }
        else
            throw new ArgumentException($"Invalid game state: {state}");

    }
}
