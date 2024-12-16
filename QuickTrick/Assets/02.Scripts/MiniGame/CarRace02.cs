using DG.Tweening;
using Febucci.UI;
using System.Threading.Tasks;
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
    #endregion

    private void Start()
    {
        //OnStandBy();

    }
    public override void OnLocalPlayerClicked(float responseTime)
    {
        localPlayer.SetActive(false);
        playerWin.SetActive(true);
        opponentPlayer.SetActive(false);
        opponentLose.SetActive(true);
        Debug.Log("LocalPlayerClicked");

        // �����ð� ǥ��
    }

    public override void OnLocalPlayerLose(float opponentResponseTime)
    {
        Debug.Log("Lose");
    }

    public override void OnLocalPlayerWin(float opponentResponseTime)
    {
        Debug.Log("Win");
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
        seq.AppendInterval(0.8f);
        // �ؽ�Ʈ �������


        seq.AppendCallback(() => { MiniGameManager.Instance.GameReady(); });
    }

    public override void OnTriggerEvent()
    {
        // ���߿� ������ �ٲ���
        Lights.SetActive(false);
    }


}
