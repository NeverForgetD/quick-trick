using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    // UI 관련
    [SerializeField, Tooltip("매칭 대기 중에 띄워질 대기UI")]
    private CanvasGroup guideUI;
    [SerializeField, Tooltip("안내 문구로 띄울 텍스트")]
    private TextMeshProUGUI guideText;
    [SerializeField, Tooltip("매칭 TIMEOUT UI")]
    private CanvasGroup timeoutUI;


    // 매칭 정보를 위한 매치메이커 객체 인스턴스
    //[SerializeField, Tooltip("지정안해주면 알아서 찾아옴")]
    private MatchMaker _matchMakerInstance;
    //[SerializeField, Tooltip("지정안해주면 알아서 찾아옴")]
    private NetworkRunner _runnerInstance;

    private bool isWaiting = false; // 나중에 대기 시간 UI 로직 개선 필요 이대로 하려면 UI 매니저까지 연결해서 통합해서 하던가 하자

    private void OnEnable()
    {
        // 상태 변경 이벤트 구독
        //UIManager.Instance.OnRunnerStatusUpdated -= HandleRunnerStatusUI;
        UIManager.Instance.OnRunnerStatusUpdated += HandleRunnerStatusUI;
    }

    private void OnDisable()
    {
        // 상태 변경 이벤트 구독 취소
        //UIManager.Instance.OnRunnerStatusUpdated += HandleRunnerStatusUI;
        UIManager.Instance.OnRunnerStatusUpdated -= HandleRunnerStatusUI;
    }

    private void HandleRunnerStatusUI(Define.RunnerStatus newState)
    {
        TurnOfffUI(guideUI);
        TurnOfffUI(timeoutUI);

        switch (newState)
        {
            case Define.RunnerStatus.NONE:
                isWaiting = false;
                TurnOfffUI(guideUI);
                break;
            case Define.RunnerStatus.CONNECTINGSERVER:
                TurnOnUI(guideUI);
                guideText.text = "Connecting to server...";
                break;
            case Define.RunnerStatus.WAITING:
                TurnOnUI(guideUI);
                isWaiting = true;
                break;
            case Define.RunnerStatus.TIMEOUT:
                //TurnOfffUI(guideUI);
                TurnOnUI(timeoutUI);
                break;
            case Define.RunnerStatus.GAME:
                //TurnOfffUI(guideUI);
                break;
            case Define.RunnerStatus.DISCONNECTING:
                TurnOnUI(guideUI);
                guideText.text = "Disconnecting...";
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
    private void UpdateMathcingTime()
    {
        if (isWaiting)
        { 
            InitInstance();
            guideText.text = $"elapsedTime : {_matchMakerInstance.elapsedTime}, playerCount : {_runnerInstance.SessionInfo.PlayerCount}";
        }
    }

    private void InitInstance()
    {
        if (_matchMakerInstance == null)
            _matchMakerInstance = FindFirstObjectByType<MatchMaker>();
            //_matchMakerInstance = FindObjectOfType<MatchMaker>();
        if (_runnerInstance == null)
            _runnerInstance = FindFirstObjectByType<NetworkRunner>();
            //_runnerInstance = FindObjectOfType<NetworkRunner>();
    }

    void Update()
    {
        UpdateMathcingTime();
    }

    // TIMEOUT UI
    public void OnBackToMenuButtonClicked()
    {
        
    }

}
