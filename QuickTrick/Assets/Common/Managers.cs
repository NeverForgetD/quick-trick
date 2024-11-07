using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    /*
    static Managers instance;
    public static Managers Instance { get { Init(); return instance; } }
    
    // Add Manager Here
    public NetworkManager Network { get; private set; }

    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();

            // Add Managers Here
            instance.Network = go.AddComponent<NetworkManager>();

            // Init Here
            //instance.Network.Init();
        }
    }
    */
}
