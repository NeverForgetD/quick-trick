using DG.Tweening;
using Febucci.UI;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarRace02 : MiniGameBase
{
    public override int miniGameIndex => 2;

    #region SerializeField
    [SerializeField] GameObject signal;
    [SerializeField] GameObject localPlayer;
    [SerializeField] GameObject opponentPlayer;
    [SerializeField] GameObject Spark;
    [SerializeField] GameObject Lights;

    [SerializeField] GameObject playerWin;
    [SerializeField] GameObject opponentWin;
    [SerializeField] GameObject playerLose;
    [SerializeField] GameObject opponentLose;

    [SerializeField] Image CutIn1;
    [SerializeField] Image CutIn2;
    #endregion

    private void Start()
    {
        //OnStandBy();

    }
    public override void OnLocalPlayerClicked(float responseTime)
    {
        SoundManager.Instance.PlaySFX("React");
        ShowPlayerText(responseTime);
        Sequence seq = DOTween.Sequence();
        seq.Append(CutIn1.transform.DOLocalMoveX(-660, 0.2f));
        seq.Append(CutIn2.transform.DOLocalMoveX(-660, 0.4f));
        seq.AppendInterval(1f);
        seq.Append(CutIn1.DOFade(0, 0));
        seq.Join(CutIn2.DOFade(0, 0.4f));
        // 반응시간 표시
        Debug.Log($"{responseTime} 결과 나왔어요");
    }

    public override void OnLocalPlayerLose(float opponentResponseTime)
    {
        SoundManager.Instance.PlaySFX("Lose");
        localPlayer.SetActive(false);
        playerLose.SetActive(true);
        opponentPlayer.SetActive(false);
        opponentWin.SetActive(true);

        panel.SetActive(true);
        LoseSignal.SetActive(true);
        LoseSignal.transform.DOMoveX(-1200, 0.4f).From();
        ShowOpponentText(opponentResponseTime);
        Debug.Log($"{opponentResponseTime}Lose");
    }

    public override void OnLocalPlayerWin(float opponentResponseTime)
    {
        SoundManager.Instance.PlaySFX("Win");
        localPlayer.SetActive(false);
        playerWin.SetActive(true);
        opponentPlayer.SetActive(false);
        opponentLose.SetActive(true);

        panel.SetActive(true);
        WinSignal.SetActive(true);
        WinSignal.transform.DOMoveX(-1200, 0.4f).From();
        ShowOpponentText(opponentResponseTime);
        Debug.Log($"{opponentResponseTime}Win");
    }

    public override async void OnStandBy()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(localPlayer.transform.DOLocalMoveY(-600, 0.6f).From());
        seq.Join(localPlayer.transform.DOLocalMoveX(-600, 0.6f).From());
        seq.Join(opponentPlayer.transform.DOLocalMoveY(-600, 0.6f).From());
        seq.Join(opponentPlayer.transform.DOLocalMoveX(600, 0.6f).From());

        seq.Append(signal.transform.DOLocalMoveY(1000, 1).From());

        seq.AppendCallback(() => { Spark.SetActive(true); });
        seq.AppendInterval(0.4f);
        seq.AppendCallback(() => { Spark.SetActive(false); });
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => { Spark.SetActive(true); });
        seq.AppendInterval(0.4f);
        seq.AppendCallback(() => { Spark.SetActive(false); });

        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => { ShowExplanationText(); });
        seq.AppendInterval(2f);
        seq.AppendCallback(() => { HideExplanationText(); });

        seq.AppendCallback(() => { MiniGameManager.Instance.GameReady(); });
    }

    public override void OnTriggerEvent()
    {
        SoundManager.Instance.PlaySFX("Trigger");
        Lights.SetActive(false);
    }


}
