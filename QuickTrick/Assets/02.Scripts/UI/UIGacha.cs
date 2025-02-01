using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIGacha : MonoBehaviour
{
    #region SerializeField
    [SerializeField] Image pile;
    [SerializeField] Image OpenArmCrane;
    [SerializeField] Image ClosedArmCrane;
    [SerializeField] Image leftCapsule;
    [SerializeField] Image rightCapsule;
    //[SerializeField] GameObject Title;
    #endregion

    private void Start()
    {
        Debug.Log("go");
        PlayGachaAnimation();
    }

    #region Dotween Sequence
    public void PlayGachaAnimation()
    {
        Sequence seq = DOTween.Sequence();
        // ���� ��������
        seq.Append(pile.transform.DOLocalMoveY(-1000, 0.6f).From());
        seq.Append(OpenArmCrane.transform.DOLocalMoveY(725, 0.6f));
        seq.Append(OpenArmCrane.transform.DOLocalMoveX(-120, 0.6f));
        seq.Append(OpenArmCrane.transform.DOLocalMoveX(120, 0.6f));
        seq.Append(OpenArmCrane.transform.DOLocalMoveX(0, 0.6f));
        // ������ �ȿ� ����
        seq.Append(OpenArmCrane.transform.DOLocalMoveY(330, 0.2f));
        seq.Append(pile.transform.DOShakePosition(0.2f, 30, 3, 2));

        seq.AppendInterval(0.6f);
        // ���� ��ȯ
        seq.Append(ClosedArmCrane.DOFade(1, 0));
        seq.Join(OpenArmCrane.DOFade(0, 0));
        // ���� �ö��, ������ ������
        seq.Join(ClosedArmCrane.transform.DOLocalMoveY(725, 0.6f));
        seq.Join(pile.transform.DOMoveY(-240, 0.8f));
        // ���� �����
        //seq.AppendInterval(0.2f);
        seq.Append(ClosedArmCrane.DOFade(0, 0.8f));

        // ĸ������ ��Ÿ��
        seq.Join(leftCapsule.DOFade(1, 0.2f));
        seq.Join(rightCapsule.DOFade(1, 0.2f));

        // ĸ�� �������� ������
        seq.Join(leftCapsule.transform.DOLocalMoveX(50, 0.4f).From());
        seq.Join(rightCapsule.transform.DOLocalMoveX(-50, 0.4f).From());

        // ĸ�� �����
        seq.Append(rightCapsule.DOFade(0, 0.6f));
        seq.Join(leftCapsule.DOFade(0, 0.6f));

        //seq.Append(localPlayer.transform.DOLocalMoveY(-600, 0.6f).From());
        //seq.Join(localPlayer.transform.DOLocalMoveX(-600, 0.6f).From());

        //seq.Append(signal.transform.DOLocalMoveY(1000, 1).From());

        //seq.AppendCallback(() => { Spark.SetActive(true); });
        //seq.AppendInterval(0.4f);


        //seq.AppendInterval(0.2f);
        //seq.AppendCallback(() => { ShowExplanationText(); });
        //seq.AppendInterval(0.8f);
        // �ؽ�Ʈ �������


        //seq.AppendCallback(() => { MiniGameManager.Instance.GameReady(); });
    }
    #endregion
}
