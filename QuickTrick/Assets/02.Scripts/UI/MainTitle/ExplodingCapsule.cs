using UnityEngine;
using DG.Tweening;

public class ExplodingCapsul : MonoBehaviour
{
    public float scaleFactor = 1.2f; // Ŭ���� ������ ������ ũ�� ����
    public float maxScale = 2.0f; // ������� �ִ� ũ��
    public float animationDuration = 0.2f; // ũ�� ��ȭ �ִϸ��̼� ���� �ð�

    [Header("Popcorn Effect Settings")]
    public GameObject spawnPrefab; // ����ó�� ������ ������Ʈ ������
    public int minSpawnCount = 15; // �ּ� ��ȯ ����
    public int maxSpawnCount = 15; // �ִ� ��ȯ ����
    public float explosionForce = 20f; // ����ó�� ������ ��
    public float fadeDuration = 0.5f; // ��ȯ�� ������Ʈ ������� �ð�

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // ���� ũ�� ����
    }

    void OnMouseDown()
    {
        ExpandObject();
    }

    void ExpandObject()
    {
        // ���� ũ�� ����
        Vector3 newScale = transform.localScale * scaleFactor;

        if (newScale.x < maxScale)
        {
            transform.DOScale(newScale, animationDuration)
                .SetEase(Ease.OutQuad);
        }
        else
        {
            TriggerExplosion();
        }
    }

    void TriggerExplosion()
    {
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount);
        Vector3 spawnPosition = transform.position;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject spawned = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = spawned.GetComponent<Rigidbody2D>();

            if (rb == null)
                rb = spawned.AddComponent<Rigidbody2D>();

            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDirection * explosionForce, ForceMode2D.Impulse);

            SpriteRenderer sr = spawned.GetComponent<SpriteRenderer>();
            sr.DOFade(0, fadeDuration).SetDelay(1f).OnComplete(() => Destroy(spawned));
        }

        Destroy(gameObject); // ���� ������Ʈ ����
    }
}
