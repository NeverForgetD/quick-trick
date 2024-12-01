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

    #region Virtual_공통 로직
    // public virtual
    #endregion

    #region Abstract_필수 구현
    /// <summary>
    /// 게임 시작하고 triggerEvent 전까지 실행
    /// </summary>
    public abstract void OnStandBy();

    /// <summary>
    /// 트리거 이벤트 실행
    /// </summary>
    public abstract void OnTriggerEvent();

    /// <summary>
    /// 플레이어가 눌렀을 때 컷인 재생
    /// </summary>
    public abstract void OnLocalPlayerClicked();

    /// <summary>
    /// 로컬 플레이어 우승
    /// </summary>
    public abstract void OnLocalPlayerWin();

    /// <summary>
    /// 로컬 플레이어 패배
    /// </summary>
    public abstract void OnLocalPlayerLose();

    //public abstract void OnLocalPlayerFail();
    //public abstract void OnOpponentPlayerWarn();
    #endregion
}
