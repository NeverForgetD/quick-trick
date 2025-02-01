using UnityEngine;
using DG.Tweening;

public class ExplodingCapsul : MonoBehaviour
{
    public float scaleFactor = 1.2f; // 클릭할 때마다 증가할 크기 비율
    public float maxScale = 2.0f; // 사라지는 최대 크기
    public float animationDuration = 0.2f; // 크기 변화 애니메이션 지속 시간

    [Header("Popcorn Effect Settings")]
    public GameObject spawnPrefab; // 팝콘처럼 생성될 오브젝트 프리팹
    public int minSpawnCount = 15; // 최소 소환 개수
    public int maxSpawnCount = 15; // 최대 소환 개수
    public float explosionForce = 20f; // 팝콘처럼 터지는 힘
    public float fadeDuration = 0.5f; // 소환된 오브젝트 사라지는 시간

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // 원래 크기 저장
    }

    void OnMouseDown()
    {
        ExpandObject();
    }

    void ExpandObject()
    {
        // 현재 크기 증가
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

        Destroy(gameObject); // 원본 오브젝트 삭제
    }
}
