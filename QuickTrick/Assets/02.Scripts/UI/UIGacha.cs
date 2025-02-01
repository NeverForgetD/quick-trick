using UnityEngine;
using DG.Tweening;

public class UIGacha : MonoBehaviour
{
    #region SerializeField
    [SerializeField] GameObject test;
    #endregion

    #region Dotween Sequence
    public void PlayGachaAnimation()
    {
        Sequence seq = DOTween.Sequence();
        //seq.Append(localPlayer.transform.DOLocalMoveY(-600, 0.6f).From());
        //seq.Join(localPlayer.transform.DOLocalMoveX(-600, 0.6f).From());

        //seq.Append(signal.transform.DOLocalMoveY(1000, 1).From());

        //seq.AppendCallback(() => { Spark.SetActive(true); });
        //seq.AppendInterval(0.4f);


        seq.AppendInterval(0.2f);
        //seq.AppendCallback(() => { ShowExplanationText(); });
        seq.AppendInterval(0.8f);
        // 텍스트 사라지게


        //seq.AppendCallback(() => { MiniGameManager.Instance.GameReady(); });
    }
    #endregion
}
