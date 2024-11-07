using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField, Tooltip("매칭 대기 중에 띄워질 대기UI")]
    private CanvasGroup waitingUI;

    private void Start()
    {
        HideWaitingUI();
    }

    public void OnPlayButtonClick()
    {
        // play버튼 비황성화 및 효과
        ShowWaitingUI();

        NetworkManager.Instance.MatchGame(true); // 랜덤 게임 참여
    }


    private void ShowWaitingUI()
    {
        waitingUI.alpha = 1;
    }

    private void HideWaitingUI()
    {
        waitingUI.alpha = 0;
    }

}