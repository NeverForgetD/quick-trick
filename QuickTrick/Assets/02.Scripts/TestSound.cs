using UnityEngine;

public class TestSound : MonoBehaviour
{
    public GameObject tri;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayBGM("MainBGM");
        tri.SetActive(true);
        Debug.Log("Testing");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
