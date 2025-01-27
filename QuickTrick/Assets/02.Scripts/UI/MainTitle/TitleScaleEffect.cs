using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class TitleScaleEffect : MonoBehaviour
{
    [Header("UI Scale Settings")]
    public Image targetImage;  // UI 이미지 (Inspector에서 할당)
    public float scaleFactor = 1.5f;  // 클릭 시 증가할 크기 비율
    public float maxScale = 3.0f;  // 최대 크기 (이 크기에 도달하면 터짐)
    public float animationDuration = 0.2f;  // 애니메이션 속도
    public float shrinkDuration = 0.2f;  // 터진 후 원래 크기로 돌아가는 시간

    [Header("Capsule Explosion Settings")]
    public GameObject capsulePrefab;  // 캡슐 프리팹 (Inspector에서 할당)
    public int minCapsuleCount = 50;  // 터질 때 최소 캡슐 개수
    public int maxCapsuleCount = 100; // 터질 때 최대 캡슐 개수
    public float explosionForce = 10f;  // 폭발 힘
    public float fadeDuration = 0.8f;  // 캡슐 사라지는 시간

    private Vector3 originalScale;  // 원래 크기 저장

    private PopcornEffect popcornEffect;

    void Start()
    {
        if (popcornEffect == null)
        {
            popcornEffect = GetComponent<PopcornEffect>();
        }
        if (targetImage != null)
        {
            originalScale = targetImage.rectTransform.localScale;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ScaleUp();
        }
        ResetScale();
    }

    void ScaleUp()
    {
        if (targetImage == null) return;

        // 현재 크기 저장 및 증가
        Vector3 currentScale = targetImage.rectTransform.localScale;
        Vector3 newScale = currentScale * scaleFactor;

        // 크기 제한 체크
        if (newScale.x < maxScale)
        {
            targetImage.rectTransform.DOScale(newScale, animationDuration)
                .SetEase(Ease.OutBack);
        }
        else
        {
            ExplodeCapsules();
            //ResetScale();
            // 매칭으로 넘어가기
            ExpandScale();
        }
    }

    void ExplodeCapsules()
    {
        for (int i = 0; i < 50; i++)
        {
            popcornEffect.CreateExplosion(Vector3.zero);
        }
    }

    void ResetScale()
    {
        // 크기 원상 복구
        targetImage.rectTransform.DOScale(originalScale, shrinkDuration)
            .SetEase(Ease.InOutQuad);
    }

    void ExpandScale()
    {
        targetImage.rectTransform.DOScale(5, 0.8f);
    }
}
