using DG.Tweening;
using Febucci.UI;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

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

    [SerializeField] GameObject WinSignal;
    [SerializeField] GameObject LoseSignal;

    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI opponentText;
    #endregion

    private void Start()
    {
        //OnStandBy();

    }
    public override void OnLocalPlayerClicked(float responseTime)
    {
        //localPlayer.SetActive(false);
        //playerWin.SetActive(true);
        //opponentPlayer.SetActive(false);
        //opponentLose.SetActive(true);
        Debug.Log("LocalPlayerClicked");

        // 반응시간 표시
        Debug.Log($"{responseTime} 결과 나왔어요");
    }

    public override void OnLocalPlayerLose(float opponentResponseTime)
    {
        localPlayer.SetActive(false);
        playerLose.SetActive(true);
        opponentPlayer.SetActive(false);
        opponentWin.SetActive(true);
        LoseSignal.SetActive(true);
        Debug.Log($"{opponentResponseTime}Lose");
    }

    public override void OnLocalPlayerWin(float opponentResponseTime)
    {
        localPlayer.SetActive(false);
        playerWin.SetActive(true);
        opponentPlayer.SetActive(false);
        opponentLose.SetActive(true);
        WinSignal.SetActive(true);
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
        Lights.SetActive(false);
    }


}
