using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    // NONE, CONNECTINGSERVER, WAITING, FINDROOM, TIMEOUT, <<< 삭제할 것 >>>

    [SerializeField, Tooltip("매칭 대기 중에 띄워질 대기UI")]
    private CanvasGroup guideUI;

    [SerializeField, Tooltip("안내 문구로 띄울 텍스트")]
    private TextMeshProUGUI guideText;

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
        TurnOfffUI(guideUI);
    }

    public void OnPlayButtonClick()
    {
        // play버튼 비황성화 및 효과

        NetworkManager.Instance.MatchGame(true); // 랜덤 게임 참여
    }

    private void HandleMainMenuUI(Define.MainMenuUI newState)
    {
        // 모든 UI 비활성화
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
                //{NetworkManager.Instance.elapsedTime} // 대기 시간 표시 후순위 작업
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

    // 초반에 설정 잘 해두어야 한다. alpha 0, interactable X, blocksRaycasts X
    private void ToggleUI(CanvasGroup canvas)
    {
        bool isActive = canvas.alpha == 1f;  // 현재 alpha 값으로 활성화 여부 확인
        canvas.alpha = isActive ? 0f : 1f;  // alpha 값으로 UI 투명도를 조절
        canvas.interactable = !isActive;  // 상호작용 가능 여부 변경
        canvas.blocksRaycasts = !isActive;  // 클릭 가능한지 여부 변경
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