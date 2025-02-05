using DG.Tweening;
using UnityEngine;

public class CardGame03 : MiniGameBase
{
    public override int miniGameIndex => 3;

    #region SerializeField
    [SerializeField] SpriteRenderer leftHand;
    [SerializeField] SpriteRenderer rightHand;
    [SerializeField] SpriteRenderer cutin;

    [SerializeField] SpriteRenderer card1;
    [SerializeField] SpriteRenderer card2;
    [SerializeField] SpriteRenderer card3;
    [SerializeField] SpriteRenderer card4;
    [SerializeField] SpriteRenderer card5;
    [SerializeField] SpriteRenderer card6;
    #endregion

    private float handTime = 0.4f;

    public override void OnLocalPlayerClicked(float responseTime)
    {
        SoundManager.Instance.PlaySFX("React");
        ShowPlayerText(responseTime);

        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => cutin.gameObject.SetActive(true));
        seq.AppendInterval(1f);
        seq.Append(cutin.DOFade(0, 0.4f));
    }

    public override void OnLocalPlayerLose(float opponentResponseTime)
    {
        SoundManager.Instance.PlaySFX("Lose");

        panel.SetActive(true);
        LoseSignal.SetActive(true);
        LoseSignal.transform.DOMoveX(-1200, 0.4f).From();
        ShowOpponentText(opponentResponseTime);
    }

    public override void OnLocalPlayerWin(float opponentResponseTime)
    {
        SoundManager.Instance.PlaySFX("Win");

        panel.SetActive(true);
        WinSignal.SetActive(true);
        WinSignal.transform.DOMoveX(-1200, 0.4f).From();
        ShowOpponentText(opponentResponseTime);
    }

    public override void OnStandBy()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => { ShowExplanationText(); });
        seq.AppendInterval(2f);
        seq.AppendCallback(() => { HideExplanationText(); });

        seq.Append(leftHand.transform.DOLocalMoveY(-3, handTime));
        seq.AppendInterval(0.2f);
        seq.JoinCallback(() => card1.gameObject.SetActive(true));
        seq.Append(leftHand.transform.DOLocalMoveY(-22, handTime));

        seq.AppendInterval(0.4f);

        seq.Append(rightHand.transform.DOLocalMoveY(3, handTime));
        seq.AppendInterval(0.4f);
        seq.JoinCallback(() => card2.gameObject.SetActive(true));
        seq.Append(rightHand.transform.DOLocalMoveY(22, handTime));

        seq.AppendInterval(0.8f);

        seq.Append(leftHand.transform.DOLocalMoveY(-3, 1f));
        seq.AppendInterval(3f);
        seq.JoinCallback(() => card3.gameObject.SetActive(true));
        seq.Append(leftHand.transform.DOLocalMoveY(-22, 0.2f));

        seq.AppendInterval(1.5f);

        seq.Append(rightHand.transform.DOLocalMoveY(3, 0.2f));
        seq.AppendInterval(0.2f);
        seq.JoinCallback(() => card4.gameObject.SetActive(true));
        seq.Append(rightHand.transform.DOShakePosition(0.8f, 5, 5, 10));
        seq.Append(rightHand.transform.DOLocalMoveY(22, 0.8f));

        seq.AppendInterval(1f);

        seq.Append(leftHand.transform.DOLocalMoveY(-3, 0.2f));
        seq.AppendInterval(1f);
        seq.JoinCallback(() => card5.gameObject.SetActive(true));
        seq.Append(leftHand.transform.DOLocalMoveY(-22, handTime));

        seq.AppendInterval(0.8f);

        seq.Append(rightHand.transform.DOLocalMoveY(3, 1f));
        seq.AppendCallback(() => card6.gameObject.SetActive(true));

        seq.AppendCallback(() => { MiniGameManager.Instance.GameReady(); });
    }

    public override void OnTriggerEvent()
    {
        SoundManager.Instance.PlaySFX("Trigger");
        rightHand.transform.DOLocalMoveY(22, 0.2f);
    }
}
