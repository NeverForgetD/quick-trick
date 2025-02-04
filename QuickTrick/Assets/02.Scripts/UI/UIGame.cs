using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    #region SerizlizedField
    // UI 관련
    [SerializeField, Tooltip("매칭 대기 중에 띄워질 대기UI")]
    private CanvasGroup guideUI;
    [SerializeField, Tooltip("안내 문구로 띄울 텍스트")]
    private TextMeshProUGUI guideText;
    [SerializeField, Tooltip("매칭 TIMEOUT UI")]
    private CanvasGroup timeoutUI;

    [SerializeField]
    private Button ConnectBtn;
    [SerializeField]
    private Button DisconnectBtn;


    // 매칭 정보를 위한 매치메이커 객체 인스턴스
    //[SerializeField, Tooltip("지정안해주면 알아서 찾아옴")]
    //private MatchMaker _matchMakerInstance;
    //[SerializeField, Tooltip("지정안해주면 알아서 찾아옴")]
    //private NetworkRunner _runnerInstance;
    #endregion

    //private bool isWaiting = false; // 나중에 대기 시간 UI 로직 개선 필요 이대로 하려면 UI 매니저까지 연결해서 통합해서 하던가 하자

    #region EventHandler_RunnerStatus
    private void OnEnable()
    {
        // 상태 변경 이벤트 구독
        UIManager.Instance.OnRunnerStatusUpdated += HandleRunnerStatusUI;
    }

    private void OnDisable()
    {
        // 상태 변경 이벤트 구독 취소
        UIManager.Instance.OnRunnerStatusUpdated -= HandleRunnerStatusUI;
    }

    private void HandleRunnerStatusUI(Define.RunnerStatus newState)
    {
        TurnOfffUI(guideUI);
        TurnOfffUI(timeoutUI);

        switch (newState)
        {
            case Define.RunnerStatus.TITLE:
                SoundManager.Instance.PlayBGM("MainBGM");
                ConnectBtn.gameObject.SetActive(true);
                DisconnectBtn.gameObject.SetActive(false);
                //isWaiting = false;
                TurnOfffUI(guideUI);
                break;
            case Define.RunnerStatus.CONNECTINGSERVER:
                ConnectBtn.gameObject.SetActive(false);
                DisconnectBtn.gameObject.SetActive(true);
                TurnOnUI(guideUI);
                guideText.text = "서버에 연결중...";
                break;
            case Define.RunnerStatus.WAITING:
                TurnOnUI(guideUI);
                guideText.text = "상대를 찾고 있습니다...";
                //isWaiting = true;
                break;
            case Define.RunnerStatus.TIMEOUT:
                TurnOnUI(timeoutUI);
                break;
            case Define.RunnerStatus.GAME:
                DisconnectBtn.gameObject.SetActive(false);
                SoundManager.Instance.StopBGM();
                break;
            case Define.RunnerStatus.DISCONNECTING:
                TurnOnUI(guideUI);
                guideText.text = "돌아가는 중...";
                break;
        }
    }

    #endregion

    #region UI Methods
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
    #endregion

    /*
    #region elapsedTime
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
    #endregion
    */
}