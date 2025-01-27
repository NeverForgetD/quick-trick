using UnityEngine;
using DG.Tweening;

public class PopcornEffectWithRandomCount : MonoBehaviour
{
    public GameObject ballPrefab; // 하나의 공 프리팹
    public Sprite[] ballSprites;  // 16개의 공 이미지 배열

    [Header("Ball Count Range")]
    public int minBallCount = 3;   // 최소 공 개수 (Inspector에서 설정)
    public int maxBallCount = 6;  // 최대 공 개수 (Inspector에서 설정)

    public float explosionForce = 15f;  // 폭발력 (Inspector에서 설정)
    public float animationDuration = 1.5f; // 애니메이션 지속 시간
    public float fadeDuration = 0.5f;  // 페이드 아웃 시간

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // 마우스 클릭 감지
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;  // 2D 환경에서는 Z축을 0으로 설정

            CreateExplosion(clickPosition);
        }
    }

    void CreateExplosion(Vector3 position)
    {
        // Inspector에서 지정한 범위 내에서 랜덤한 공 개수 설정
        int ballCount = Random.Range(minBallCount, maxBallCount + 1);

        for (int i = 0; i < ballCount; i++)
        {
            // 공 오브젝트 생성
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            SpriteRenderer spriteRenderer = ball.GetComponent<SpriteRenderer>();

            // 랜덤 스프라이트 적용
            int randomIndex = Random.Range(0, ballSprites.Length);
            spriteRenderer.sprite = ballSprites[randomIndex];

            // Rigidbody2D 추가 및 force 적용
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = ball.AddComponent<Rigidbody2D>();

            // 랜덤 방향으로 force 적용 (Inspector의 explosionForce 값 사용)
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDirection * explosionForce, ForceMode2D.Impulse);

            // 투명도 조정 후 제거
            spriteRenderer.DOFade(0, fadeDuration)
                .SetDelay(animationDuration)
                .OnComplete(() => Destroy(ball));
        }
    }
}
