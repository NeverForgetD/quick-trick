using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    // NONE, CONNECTINGSERVER, WAITING, FINDROOM, TIMEOUT, <<< ������ �� >>>

    [SerializeField, Tooltip("��Ī ��� �߿� ����� ���UI")]
    private CanvasGroup guideUI;

    [SerializeField, Tooltip("�ȳ� ������ ��� �ؽ�Ʈ")]
    private TextMeshProUGUI guideText;

    private void OnEnable()
    {
        // ���� ���� �̺�Ʈ ����
        UIManager.Instance.OnMainMenuUIChanged -= HandleMainMenuUI;
        UIManager.Instance.OnMainMenuUIChanged += HandleMainMenuUI;
    }

    private void OnDisable()
    {
        // ���� ���� �̺�Ʈ ���� ���
        UIManager.Instance.OnMainMenuUIChanged += HandleMainMenuUI;
        UIManager.Instance.OnMainMenuUIChanged -= HandleMainMenuUI;
    }


    private void Start()
    {
        TurnOfffUI(guideUI);
    }

    public void OnPlayButtonClick()
    {
        // play��ư ��Ȳ��ȭ �� ȿ��

        NetworkManager.Instance.MatchGame(true); // ���� ���� ����
    }

    private void HandleMainMenuUI(Define.MainMenuUI newState)
    {
        // ��� UI ��Ȱ��ȭ
        TurnOfffUI(guideUI);

        switch(newState)
        {
            case Define.MainMenuUI.NONE:
                TurnOfffUI(guideUI);
                break;
            case Define.MainMenuUI.CONNECTINGSERVER:
                TurnOnUI(guideUI);
                guideText.text = "Connecting to server...";
                break;
            case Define.MainMenuUI.WAITING:
                TurnOnUI(guideUI);
                //{NetworkManager.Instance.elapsedTime} // ��� �ð� ǥ�� �ļ��� �۾�
                guideText.text = "Finding Opponent...Please Wait";
                break;
            case Define.MainMenuUI.FINDROOM:
                TurnOnUI(guideUI);
                guideText.text = "Game will soon be started! Get Ready!";
                break;
            case Define.MainMenuUI.TIMEOUT:
                TurnOnUI(guideUI);
                guideText.text = "Time Out... please try again...";
                break;
        }
    }

    // �ʹݿ� ���� �� �صξ�� �Ѵ�. alpha 0, interactable X, blocksRaycasts X
    private void ToggleUI(CanvasGroup canvas)
    {
        bool isActive = canvas.alpha == 1f;  // ���� alpha ������ Ȱ��ȭ ���� Ȯ��
        canvas.alpha = isActive ? 0f : 1f;  // alpha ������ UI ������ ����
        canvas.interactable = !isActive;  // ��ȣ�ۿ� ���� ���� ����
        canvas.blocksRaycasts = !isActive;  // Ŭ�� �������� ���� ����
    }

    private void TurnOfffUI(CanvasGroup canvas)
    {
        canvas.alpha = 0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }

    private void TurnOnUI(CanvasGroup canvas)
    {
        canvas.alpha = 1f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }

}