using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour // UI. 반영 상태를 알려주는 enum을 저장하는 매니저...
{
    static UIManager instance;
    public static UIManager Instance { get { Init(); return instance; } }

    public Define.MainMenuUI mainMenuUI;

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
}
