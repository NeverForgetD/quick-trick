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

    // 매우 러프한 스케치
    // 코루틴 인터페이스로 만들자
    // 중간에 실행할 함수 (색깔 바꾸기) 를 변수로 받아서 채워넣기만 하는 꼴로 설계

    private void OnEnable()
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
