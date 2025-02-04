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
        // SO에서 정보를 불러와 나타낼 미니게임 설명을 저장한다.
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
    public abstract void OnLocalPlayerClicked(float responseTime);

    /// <summary>
    /// 로컬 플레이어 우승
    /// </summary>
    public abstract void OnLocalPlayerWin(float opponentResponseTime);

    /// <summary>
    /// 로컬 플레이어 패배
    /// </summary>
    public abstract void OnLocalPlayerLose(float opponentResponseTime);

    //public abstract void OnLocalPlayerFail();
    //public abstract void OnOpponentPlayerWarn();
    #endregion
}
