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

    [SerializeField] Image icon00;
    [SerializeField] Image icon01;
    [SerializeField] Image icon02;
    [SerializeField] Image icon03;
    #endregion

    private int miniGameIndex;

    private void Start()
    {
    }

    private void OnEnable()
    {
        PlayGachaAnimation();
    }

    private void OnDisable()
    {
        //DoKill();
    }


    #region Dotween Sequence
    public void PlayGachaAnimation()
    {
        Sequence seq = DOTween.Sequence();
        GetMiniGameIndex();
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
        //seq.AppendCallback(() => { MiniGameManager.Instance.GameReady(); });

        switch (miniGameIndex)
        {
            case 0:
                seq.Append(icon00.DOFade(1, 0.2f));
                break;
            case 1:
                seq.Append(icon01.DOFade(1, 0.2f));
                break;
            case 2:
                seq.Append(icon02.DOFade(1, 0.2f));
                break;
            case 3:
                seq.Append(icon03.DOFade(1, 0.2f));
                break;
        }
    }
    #endregion

    private void GetMiniGameIndex()
    {
        miniGameIndex = MiniGameManager.Instance.selectedGameIndex;

    }
}
