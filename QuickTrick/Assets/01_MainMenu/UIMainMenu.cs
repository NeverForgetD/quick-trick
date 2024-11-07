using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField, Tooltip("��Ī ��� �߿� ����� ���UI")]
    private CanvasGroup waitingUI;

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
        //HideWaitingUI();
    }

    public void OnPlayButtonClick()
    {
        // play��ư ��Ȳ��ȭ �� ȿ��
        //ShowWaitingUI();

        NetworkManager.Instance.MatchGame(true); // ���� ���� ����
    }

    private void HandleMainMenuUI(Define.MainMenuUI newState)
    {
        // ��� UI ��Ȱ��ȭ

    }

    /*
     * public void ToggleUI()
{
    bool isActive = canvasGroup.alpha == 1f;  // ���� alpha ������ Ȱ��ȭ ���� Ȯ��
    canvasGroup.alpha = isActive ? 0f : 1f;  // alpha ������ UI ������ ����
    canvasGroup.interactable = !isActive;  // ��ȣ�ۿ� ���� ���� ����
    canvasGroup.blocksRaycasts = !isActive;  // Ŭ�� �������� ���� ����
}
     */

}