using UnityEngine;
using DG.Tweening;

public class UIMainTitle : MonoBehaviour
{
    [SerializeField] RectTransform title;

    public void ShowTitle()
    {
        title.DOLocalMoveY(1000, 0.4f).From();
    }

    public void HideTitle()
    {
        title.DOLocalMoveY(1000, 0.4f);
    }
}
