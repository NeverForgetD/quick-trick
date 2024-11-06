using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game00 : MonoBehaviour
{
    public GameObject green;
    public GameObject blue;
    public GameObject red;
    public TextMeshProUGUI log;
    public float givenTime = 5.0f;

    private void Start()
    {
        green.SetActive(false);
        blue.SetActive(false);
        red.SetActive(false);

        StartCoroutine(WaitForStart());
    }
    public bool isClicked = false;
    public bool canClick = false;
    public IEnumerator WaitForStart()
    {
        green.SetActive(true);
        log.text = "click when the screen turn blue";
        yield return new WaitForSeconds(givenTime);
        green.SetActive(false);
        blue.SetActive(true);
        // 클릭 받기
        float timeStart = Time.time;
        canClick= true;

        yield return new WaitUntil(() => isClicked == true);
        blue.SetActive(false);
        red.SetActive(true);
        
        float record = Time.time - timeStart;
        log.text = $"your response time : {record}";


    }
    private void Update()
    {
        if (canClick && Input.GetMouseButtonDown(0))
        {
            isClicked = true;
        }

    }
}
