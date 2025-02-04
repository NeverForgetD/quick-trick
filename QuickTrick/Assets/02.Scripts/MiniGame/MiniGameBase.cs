using Febucci.UI;
using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    public abstract int miniGameIndex { get; }
    
    private MiniGameSO miniGameSo;
    private string guideTextValue;

    [SerializeField] protected TypewriterByCharacter typewritter;
    [SerializeField] protected TypewriterByCharacter playerText;
    [SerializeField] protected TypewriterByCharacter opponentText;
    [SerializeField] protected GameObject panel;
    [SerializeField] protected GameObject WinSignal;
    [SerializeField] protected GameObject LoseSignal;

    #region Initialize
    private void OnEnable()
    {
        OnMiniGameInitialized();
    }

    public virtual void OnMiniGameInitialized()
    {
        // SO���� ������ �ҷ��� ��Ÿ�� �̴ϰ��� ������ �����Ѵ�.
        miniGameSo = MiniGameManager.Instance._MiniGameSo;
        guideTextValue = miniGameSo.GetTextForMiniGame(miniGameIndex);
    }
    #endregion

    #region protected Method
    protected void ShowExplanationText()
    {
        typewritter.ShowText(guideTextValue);
    }

    protected void HideExplanationText()
    {
        typewritter.StartDisappearingText();
    }

    protected void ShowPlayerText(float time)
    {
        playerText.ShowText($"<rainb><wave a=0.2>{time}");
    }

    protected void ShowOpponentText(float time)
    {
        opponentText.ShowText($"<rainb><wave a=0.2>{time}</wave>");
    }

    #endregion

    #region Virtual_���� ����
    // public virtual
    #endregion

    #region Abstract_�ʼ� ����
    /// <summary>
    /// ���� �����ϰ� triggerEvent ������ ����
    /// </summary>
    public abstract void OnStandBy();

    /// <summary>
    /// Ʈ���� �̺�Ʈ ����
    /// </summary>
    public abstract void OnTriggerEvent();

    /// <summary>
    /// �÷��̾ ������ �� ���� ���
    /// </summary>
    public abstract void OnLocalPlayerClicked(float responseTime);

    /// <summary>
    /// ���� �÷��̾� ���
    /// </summary>
    public abstract void OnLocalPlayerWin(float opponentResponseTime);

    /// <summary>
    /// ���� �÷��̾� �й�
    /// </summary>
    public abstract void OnLocalPlayerLose(float opponentResponseTime);

    //public abstract void OnLocalPlayerFail();
    //public abstract void OnOpponentPlayerWarn();
    #endregion
}
