using Febucci.UI;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    public abstract int miniGameIndex { get; }
    
    private MiniGameSO miniGameSo;
    private string guideTextValue;

    [SerializeField] protected TypewriterByCharacter typewritter;

    #region Initialize
    private void OnEnable()
    {
        OnMiniGameInitialized();
    }

    public virtual void OnMiniGameInitialized()
    {
        miniGameSo = MiniGameManager.Instance._MiniGameSo;
        guideTextValue = miniGameSo.GetTextForMiniGame(miniGameIndex);
    }
    #endregion

    #region protected Method
    protected void ShowText()
    {
        typewritter.ShowText(guideTextValue);
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
    public abstract void OnLocalPlayerClicked();

    /// <summary>
    /// ���� �÷��̾� ���
    /// </summary>
    public abstract void OnLocalPlayerWin();

    /// <summary>
    /// ���� �÷��̾� �й�
    /// </summary>
    public abstract void OnLocalPlayerLose();

    //public abstract void OnLocalPlayerFail();
    //public abstract void OnOpponentPlayerWarn();
    #endregion
}
