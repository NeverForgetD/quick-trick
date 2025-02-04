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
        // 투명화
        seq.Append(ClosedArmCrane.DOFade(0, 0));
        seq.Join(OpenArmCrane.DOFade(0, 0));
        seq.Join(ClosedArmCrane.transform.DOLocalMoveY(330, 0));
        seq.Join(OpenArmCrane.transform.DOLocalMoveY(700, 0));

        seq.Join(leftCapsule.DOFade(0, 0));
        seq.Join(rightCapsule.DOFade(0, 0));
        seq.Join(icon00.DOFade(0, 0));
        seq.Join(icon01.DOFade(0, 0));
        seq.Join(icon02.DOFade(0, 0));
        seq.Join(icon03.DOFade(0, 0));

        seq.AppendInterval(0.4f);
        // 무더기 올라옴
        seq.Append(pile.transform.DOLocalMoveY(-1000, 0.6f).From());

        // 집게 내려오고
        seq.Append(OpenArmCrane.DOFade(1, 0));
        seq.Join(OpenArmCrane.transform.DOLocalMoveY(1500, 0.6f).From());

        // 좌우 이동
        seq.Append(OpenArmCrane.transform.DOLocalMoveX(-120, 0.6f));
        seq.Append(OpenArmCrane.transform.DOLocalMoveX(120, 0.6f));
        seq.Append(OpenArmCrane.transform.DOLocalMoveX(0, 0.6f));

        // 무더기 안에 들어가고 무더기 흔들림
        seq.Append(OpenArmCrane.transform.DOLocalMoveY(330, 0.2f));
        seq.Append(pile.transform.DOShakePosition(0.2f, 30, 3, 2));
        seq.Join(OpenArmCrane.transform.DOShakePosition(0.2f, 30, 3, 2));

        seq.AppendInterval(0.6f);
        // 집게 교환
        seq.Append(ClosedArmCrane.DOFade(1, 0));
        seq.Join(OpenArmCrane.DOFade(0, 0));
        // 집게 올라옴, 무더기 내려감
        seq.Append(ClosedArmCrane.transform.DOLocalMoveY(330, 0.6f).From());
        seq.Join(pile.transform.DOMoveY(-240, 0.8f));
        // 집게 사라짐
        //seq.AppendInterval(0.2f);
        seq.Append(ClosedArmCrane.DOFade(0, 0.8f));

        // 캡슐조각 나타남
        seq.Join(leftCapsule.DOFade(1, 0.2f));
        seq.Join(rightCapsule.DOFade(1, 0.2f));

        // 캡슐 양쪽으로 움직임
        seq.Join(leftCapsule.transform.DOLocalMoveX(0, 0.4f).From());
        seq.Join(rightCapsule.transform.DOLocalMoveX(0, 0.4f).From());

        // 캡슐 사라짐
        seq.Append(rightCapsule.DOFade(0, 0.6f));
        seq.Join(leftCapsule.DOFade(0, 0.6f));

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
        //seq.AppendCallback(() => { MiniGameManager.Instance.; });
    }
    #endregion

    private void GetMiniGameIndex()
    {
        //miniGameIndex = MiniGameManager.Instance.selectedGameIndex;
        miniGameIndex = 2;

    }
}
