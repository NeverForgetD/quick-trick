using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    [SerializeField, Tooltip("��Ī ��� �߿� ����� ���UI")]
    private CanvasGroup guideUI;
    [SerializeField, Tooltip("�ȳ� ������ ��� �ؽ�Ʈ")]
    private TextMeshProUGUI guideText;

    // ��Ī ������ ���� ��ġ����Ŀ ��ü �ν��Ͻ�
    [SerializeField]
    private MatchMaker _matchMakerInstance;
    [SerializeField]
    private NetworkRunner _runnerInstance;

    private bool isWaiting = false; // ���߿� ��� �ð� UI ���� ���� �ʿ� �̴�� �Ϸ��� UI �Ŵ������� �����ؼ� �����ؼ� �ϴ��� ����

    private void OnEnable()
    {
        // ���� ���� �̺�Ʈ ����
        UIManager.Instance.OnRunnerStatusUpdated -= HandleRunnerStatusUI;
        UIManager.Instance.OnRunnerStatusUpdated += HandleRunnerStatusUI;
    }

    private void OnDisable()
    {
        // ���� ���� �̺�Ʈ ���� ���
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
                //{NetworkManager.Instance.elapsedTime} // ��� �ð� ǥ�� �ļ��� �۾�
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
