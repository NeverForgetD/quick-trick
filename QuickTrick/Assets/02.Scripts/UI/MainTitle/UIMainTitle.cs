using UnityEngine;
using DG.Tweening;

public class UIMainTitle : MonoBehaviour
{
    [SerializeField] Transform title;

    #region Unity LifyCycle
    private void Start()
    {
        ShowTitle();
    }
    #endregion

    #region DoTween Animation
    public void ShowTitle()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(title.DOLocalMoveY(-150, 0.4f).SetEase(Ease.InOutSine))
                .Append(title.DOLocalMoveY(30, 0.3f).SetEase(Ease.OutCubic))
                .Append(title.DOLocalMoveY(0, 0.2f).SetEase(Ease.InOutCubic));

        sequence.Play();
    }


    public void HideTitle()
    {
        //title.DOLocalMoveY(1000, 0.4f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(title.DOLocalMoveY(-150, 0.4f))
                .Append(title.DOLocalMoveY(1000, 0.4f));

        sequence.Play();
    }
    #endregion

    #region Button Method
    public void OnPlayButton()
    {
        HideTitle();

    }
    #endregion
}
