using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField, Tooltip("��Ī ��� �߿� ����� ���UI")]
    private CanvasGroup waitingUI;

    private void Start()
    {
        HideWaitingUI();
    }

    public void OnPlayButtonClick()
    {
        // play��ư ��Ȳ��ȭ �� ȿ��
        ShowWaitingUI();

        NetworkManager.Instance.MatchGame(true); // ���� ���� ����
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