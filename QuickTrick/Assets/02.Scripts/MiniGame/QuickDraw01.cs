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
        cutIn.gameObject.SetActive(true);
        cutIn.transform.DOMoveX(10, 1).From();
    }

    public override void OnLocalPlayerLose(float opponentResponseTime)
    {
        cutIn.gameObject.SetActive(false);
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
        cutIn.gameObject.SetActive(false);
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
        seq.Append(tumbleWeed.transform.DOMoveX(-20, 3f).From());
        seq.Join(tumbleWeed.transform.DORotate(new Vector3(0, 0, 1080), 3f));

        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => { ShowExplanationText(); });
        seq.AppendInterval(2f);
        seq.AppendCallback(() => { HideExplanationText(); });

        seq.AppendCallback(() => { MiniGameManager.Instance.GameReady(); });
    }

    public override void OnTriggerEvent()
    {
        SunFlash.gameObject.SetActive(true);
    }
}
