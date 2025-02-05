using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Febucci.UI;

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

    [SerializeField] TypewriterByCharacter text;
    #endregion

    private int miniGameIndex;

    private void Start()
    {
    }

    private void OnEnable()
    {
        //PlayGachaAnimation();
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
        // ����ȭ
        seq.Append(ClosedArmCrane.DOFade(0, 0));
        seq.Join(OpenArmCrane.DOFade(0, 0));
        seq.Join(ClosedArmCrane.transform.DOLocalMoveY(330, 0));
        seq.Join(OpenArmCrane.transform.DOLocalMoveY(1500, 0));

        seq.Join(leftCapsule.DOFade(0, 0));
        seq.Join(rightCapsule.DOFade(0, 0));
        seq.Join(leftCapsule.transform.DOLocalMoveX(-150, 0));
        seq.Join(rightCapsule.transform.DOLocalMoveX(150, 0));

        seq.Join(icon00.DOFade(0, 0));
        seq.Join(icon01.DOFade(0, 0));
        seq.Join(icon02.DOFade(0, 0));
        seq.Join(icon03.DOFade(0, 0));
        seq.Join(pile.transform.DOLocalMoveY(-900, 0));

        //SoundManager.Instance.PlaySFX("Match");
        seq.JoinCallback(() => SoundManager.Instance.PlaySFX("Match"));
        seq.JoinCallback(() => { text.ShowText("��븦 ã�ҽ��ϴ�!"); });
        seq.AppendInterval(1f);
        seq.AppendCallback(() => { text.StartDisappearingText(); });

        // ������ �ö��
        seq.Append(pile.transform.DOLocalMoveY(-250, 0.6f));

        // ���� ��������
        seq.Append(OpenArmCrane.DOFade(1, 0));
        seq.Append(OpenArmCrane.transform.DOLocalMoveY(700, 0.6f));

        // �¿� �̵�
        seq.Append(OpenArmCrane.transform.DOLocalMoveX(-120, 0.6f));
        seq.Append(OpenArmCrane.transform.DOLocalMoveX(120, 0.6f));
        seq.Append(OpenArmCrane.transform.DOLocalMoveX(0, 0.6f));

        // ������ �ȿ� ���� ������ ��鸲
        seq.Append(OpenArmCrane.transform.DOLocalMoveY(330, 0.2f));
        seq.Append(pile.transform.DOShakePosition(0.2f, 30, 3, 2));
        seq.JoinCallback(() => SoundManager.Instance.PlaySFX("Pile"));
        seq.Join(OpenArmCrane.transform.DOShakePosition(0.2f, 30, 3, 2));

        seq.AppendInterval(0.6f);
        // ���� ��ȯ
        seq.Append(ClosedArmCrane.DOFade(1, 0));
        seq.Join(OpenArmCrane.DOFade(0, 0));
        // ���� �ö��, ������ ������
        seq.Append(ClosedArmCrane.transform.DOLocalMoveY(700, 0.6f));
        seq.Join(pile.transform.DOLocalMoveY(-900, 0.8f));
        // ���� �����
        //seq.AppendInterval(0.2f);
        seq.Append(ClosedArmCrane.DOFade(0, 0.8f));

        // ĸ������ ��Ÿ��
        seq.Join(leftCapsule.DOFade(1, 0.2f));
        seq.Join(rightCapsule.DOFade(1, 0.2f));

        // ĸ�� �������� ������
        seq.Join(leftCapsule.transform.DOLocalMoveX(-400, 0.4f));
        seq.Join(rightCapsule.transform.DOLocalMoveX(400, 0.4f));
        seq.JoinCallback(() => SoundManager.Instance.PlaySFX("Crack"));

        // ĸ�� �����
        seq.Append(rightCapsule.DOFade(0, 0.4f));
        seq.Join(leftCapsule.DOFade(0, 0.4f));

        switch (miniGameIndex)
        {
            case 0:
                seq.Join(icon00.DOFade(1, 0.2f));
                seq.Append(icon00.transform.DOShakePosition(1, 20, 5, 10));
                break;
            case 1:
                seq.Join(icon01.DOFade(1, 0.2f));
                seq.Append(icon01.transform.DOShakePosition(1, 20, 5, 10));
                break;
            case 2:
                seq.Join(icon02.DOFade(1, 0.2f));
                seq.Append(icon02.transform.DOShakePosition(1, 20, 5, 10));
                break;
            case 3:
                seq.Join(icon03.DOFade(1, 0.2f));
                seq.Append(icon03.transform.DOShakePosition(1, 20, 5, 10));
                break;
        }

        seq.AppendCallback(() => { text.ShowText("������ �����մϴ�"); });
        seq.JoinCallback(() => SoundManager.Instance.PlaySFX("Start"));
        seq.AppendInterval(1.5f);
        seq.AppendCallback(() => { text.StartDisappearingText(); });
        //seq.AppendCallback(() => { MiniGameManager.Instance.; });
    }
    #endregion

    private void GetMiniGameIndex()
    {
        if (MiniGameManager.Instance != null)
            miniGameIndex = MiniGameManager.Instance.selectedGameIndex;
        //miniGameIndex = 2;
        if (MiniGameManager.Instance == null)
        {
            miniGameIndex = 2;
        }

    }
}
