using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField, Tooltip("매칭 대기 중에 띄워질 대기UI")]
    private CanvasGroup waitingUI;

    private void OnEnable()
    {
        // 상태 변경 이벤트 구독
        UIManager.Instance.OnMainMenuUIChanged -= HandleMainMenuUI;
        UIManager.Instance.OnMainMenuUIChanged += HandleMainMenuUI;
    }

    private void OnDisable()
    {
        // 상태 변경 이벤트 구독 취소
        UIManager.Instance.OnMainMenuUIChanged += HandleMainMenuUI;
        UIManager.Instance.OnMainMenuUIChanged -= HandleMainMenuUI;
    }


    private void Start()
    {
        //HideWaitingUI();
    }

    public void OnPlayButtonClick()
    {
        // play버튼 비황성화 및 효과
        //ShowWaitingUI();

        NetworkManager.Instance.MatchGame(true); // 랜덤 게임 참여
    }

    private void HandleMainMenuUI(Define.MainMenuUI newState)
    {
        // 모든 UI 비활성화

    }

    /*
     * public void ToggleUI()
{
    bool isActive = canvasGroup.alpha == 1f;  // 현재 alpha 값으로 활성화 여부 확인
    canvasGroup.alpha = isActive ? 0f : 1f;  // alpha 값으로 UI 투명도를 조절
    canvasGroup.interactable = !isActive;  // 상호작용 가능 여부 변경
    canvasGroup.blocksRaycasts = !isActive;  // 클릭 가능한지 여부 변경
}
     */

}