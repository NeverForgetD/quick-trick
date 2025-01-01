using UnityEngine;

public class TestSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayBGM("MainBGM");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
