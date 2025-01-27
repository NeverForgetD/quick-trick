using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class TitleScaleEffect : MonoBehaviour
{
    [Header("UI Scale Settings")]
    public Image targetImage;  // UI �̹��� (Inspector���� �Ҵ�)
    public float scaleFactor = 1.5f;  // Ŭ�� �� ������ ũ�� ����
    public float maxScale = 3.0f;  // �ִ� ũ�� (�� ũ�⿡ �����ϸ� ����)
    public float animationDuration = 0.2f;  // �ִϸ��̼� �ӵ�
    public float shrinkDuration = 0.2f;  // ���� �� ���� ũ��� ���ư��� �ð�

    [Header("Capsule Explosion Settings")]
    public GameObject capsulePrefab;  // ĸ�� ������ (Inspector���� �Ҵ�)
    public int minCapsuleCount = 50;  // ���� �� �ּ� ĸ�� ����
    public int maxCapsuleCount = 100; // ���� �� �ִ� ĸ�� ����
    public float explosionForce = 10f;  // ���� ��
    public float fadeDuration = 0.8f;  // ĸ�� ������� �ð�

    private Vector3 originalScale;  // ���� ũ�� ����

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

        // ���� ũ�� ���� �� ����
        Vector3 currentScale = targetImage.rectTransform.localScale;
        Vector3 newScale = currentScale * scaleFactor;

        // ũ�� ���� üũ
        if (newScale.x < maxScale)
        {
            targetImage.rectTransform.DOScale(newScale, animationDuration)
                .SetEase(Ease.OutBack);
        }
        else
        {
            ExplodeCapsules();
            //ResetScale();
            // ��Ī���� �Ѿ��
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
        // ũ�� ���� ����
        targetImage.rectTransform.DOScale(originalScale, shrinkDuration)
            .SetEase(Ease.InOutQuad);
    }

    void ExpandScale()
    {
        targetImage.rectTransform.DOScale(5, 0.8f);
    }
}
