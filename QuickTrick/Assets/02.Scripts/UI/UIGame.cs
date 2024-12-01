using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    #region SerizlizedField
    // UI ����
    [SerializeField, Tooltip("��Ī ��� �߿� ����� ���UI")]
    private CanvasGroup guideUI;
    [SerializeField, Tooltip("�ȳ� ������ ��� �ؽ�Ʈ")]
    private TextMeshProUGUI guideText;
    [SerializeField, Tooltip("��Ī TIMEOUT UI")]
    private CanvasGroup timeoutUI;


    // ��Ī ������ ���� ��ġ����Ŀ ��ü �ν��Ͻ�
    [SerializeField, Tooltip("���������ָ� �˾Ƽ� ã�ƿ�")]
    private MatchMaker _matchMakerInstance;
    [SerializeField, Tooltip("���������ָ� �˾Ƽ� ã�ƿ�")]
    private NetworkRunner _runnerInstance;
    #endregion

    private bool isWaiting = false; // ���߿� ��� �ð� UI ���� ���� �ʿ� �̴�� �Ϸ��� UI �Ŵ������� �����ؼ� �����ؼ� �ϴ��� ����

    #region EventHandler_RunnerStatus
    private void OnEnable()
    {
        // ���� ���� �̺�Ʈ ����
        UIManager.Instance.OnRunnerStatusUpdated += HandleRunnerStatusUI;
    }

    private void OnDisable()
    {
        // ���� ���� �̺�Ʈ ���� ���
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

    #endregion

    #region UI Methods
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
    #endregion

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

    void Update()
    {
        UpdateMathcingTime();
    }
    #endregion

    public void OnBackToMenuButtonClicked()
    {
        
    }

}
