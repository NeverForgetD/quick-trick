using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    [SerializeField, Tooltip("매칭 대기 중에 띄워질 대기UI")]
    private CanvasGroup guideUI;
    [SerializeField, Tooltip("안내 문구로 띄울 텍스트")]
    private TextMeshProUGUI guideText;

    // 매칭 정보를 위한 매치메이커 객체 인스턴스
    [SerializeField]
    private MatchMaker _matchMakerInstance;
    [SerializeField]
    private NetworkRunner _runnerInstance;

    private bool isWaiting = false; // 나중에 대기 시간 UI 로직 개선 필요 이대로 하려면 UI 매니저까지 연결해서 통합해서 하던가 하자

    private void OnEnable()
    {
        // 상태 변경 이벤트 구독
        UIManager.Instance.OnRunnerStatusUpdated -= HandleRunnerStatusUI;
        UIManager.Instance.OnRunnerStatusUpdated += HandleRunnerStatusUI;
    }

    private void OnDisable()
    {
        // 상태 변경 이벤트 구독 취소
        UIManager.Instance.OnRunnerStatusUpdated += HandleRunnerStatusUI;
        UIManager.Instance.OnRunnerStatusUpdated -= HandleRunnerStatusUI;
    }

    private void HandleRunnerStatusUI(Define.RunnerStatus newState)
    {
        TurnOfffUI(guideUI);

        switch (newState)
        {
            case Define.RunnerStatus.NONE:
                isWaiting = true;
                TurnOfffUI(guideUI);
                break;
            case Define.RunnerStatus.CONNECTINGSERVER:
                TurnOnUI(guideUI);
                guideText.text = "Connecting to server...";
                break;
            case Define.RunnerStatus.WAITING:
                TurnOnUI(guideUI);
                //{NetworkManager.Instance.elapsedTime} // 대기 시간 표시 후순위 작업
                //guideText.text = "Finding Opponent...Please Wait";
                isWaiting = true;
                break;
            case Define.RunnerStatus.FINDROOM:
                TurnOnUI(guideUI);
                guideText.text = "Game will soon be started! Get Ready!";
                break;
            case Define.RunnerStatus.TIMEOUT:
                TurnOnUI(guideUI);
                guideText.text = "Time Out... please try again...";
                break;
        }
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
        if (_matchMakerInstance == null)
            _matchMakerInstance = FindObjectOfType<MatchMaker>();
        if (_runnerInstance == null)
            _runnerInstance = FindObjectOfType<NetworkRunner>();

        guideText.text = $"elapsedTime : {_matchMakerInstance.elapsedTime}, playerCount : {_runnerInstance.SessionInfo.PlayerCount}";
    }

    private void Start()
    {
        //Instantiate(Mode00);
    }

    void Update()
    {
        //if (runner == null) { runner = FindObjectOfType<NetworkRunner>(); }
        //text.text = runner.SessionInfo.PlayerCount.ToString();
        if (isWaiting)
            UpdateMathcingTime();
    }

}
