using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour // UI. �ݿ� ���¸� �˷��ִ� enum�� �����ϴ� �Ŵ���...
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
