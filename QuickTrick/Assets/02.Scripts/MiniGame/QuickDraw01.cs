using DG.Tweening;
using UnityEngine;

public class QuickDraw01 : MiniGameBase
{
    public override int miniGameIndex => 1;

    #region SerializeField
    [SerializeField] SpriteRenderer cutIn;

    [SerializeField] SpriteRenderer playerIdle;
    [SerializeField] SpriteRenderer playerWin;
    [SerializeField] SpriteRenderer playerLose;

    [SerializeField] SpriteRenderer enemyIdle;
    [SerializeField] SpriteRenderer enemyWin;
    [SerializeField] SpriteRenderer enemyLose;

    [SerializeField] SpriteRenderer SunFlash;

    [SerializeField] SpriteRenderer tumbleWeed;
    #endregion

    public override void OnLocalPlayerClicked(float responseTime)
    {
        SoundManager.Instance.PlaySFX("React");
        ShowPlayerText(responseTime);

        Sequence seq = DOTween.Sequence();
        cutIn.gameObject.SetActive(true);

        seq.Append(cutIn.transform.DOMoveX(-15, 0.4f).From());
        seq.AppendInterval(1f);
        seq.Append(cutIn.DOFade(0, 0.4f));

    }

    public override void OnLocalPlayerLose(float opponentResponseTime)
    {
        SoundManager.Instance.PlaySFX("Lose");
        //cutIn.gameObject.SetActive(false);
        playerIdle.gameObject.SetActive(false);
        enemyIdle.gameObject.SetActive(false);
        playerLose.gameObject.SetActive(true);
        enemyWin.gameObject.SetActive(true);

        panel.SetActive(true);
        LoseSignal.SetActive(true);
        LoseSignal.transform.DOMoveX(-1200, 0.4f).From();
        ShowOpponentText(opponentResponseTime);
    }

    public override void OnLocalPlayerWin(float opponentResponseTime)
    {
        SoundManager.Instance.PlaySFX("Win");
        //cutIn.gameObject.SetActive(false);
        playerIdle.gameObject.SetActive(false);
        enemyIdle.gameObject.SetActive(false);
        playerWin.gameObject.SetActive(true);
        enemyLose.gameObject.SetActive(true);

        panel.SetActive(true);
        WinSignal.SetActive(true);
        WinSignal.transform.DOMoveX(-1200, 0.4f).From();
        ShowOpponentText(opponentResponseTime);
    }

    public override void OnStandBy()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(tumbleWeed.transform.DOMoveX(-5, 3f).From());
        //seq.Join(tumbleWeed.transform.DOShakePosition(3f, 5, 5, 10));

        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => { ShowExplanationText(); });
        seq.AppendInterval(2f);
        seq.AppendCallback(() => { HideExplanationText(); });

        seq.AppendCallback(() => { MiniGameManager.Instance.GameReady(); });
    }

    public override void OnTriggerEvent()
    {
        SoundManager.Instance.PlaySFX("Trigger");
        SunFlash.gameObject.SetActive(true);
    }
}
