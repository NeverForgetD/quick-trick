# 🎮 Quick Trick!

> 네트워크 환경에서도 공정한 반응속도 대결을 목표로 설계·구현한  
> **멀티플레이 캐주얼 게임 프로젝트**입니다.

<img width="4378" height="866" alt="QuickTrick_banner" src="https://github.com/user-attachments/assets/7b970c64-b90e-49c5-ba68-e63579eb1259" />

---

## 1. 프로젝트 개요

- **플랫폼**: PC (오프라인 행사 기준)
- **엔진**: Unity
- **네트워크**: Photon Fusion
- **장르**: 멀티플레이 / 캐주얼 / 반응속도
- **개발 형태**: 개인 개발 (아트 2명, 사운드 1명 협업)
- **성과**: 넥슨 대학생 게임 공모전(NDM)  
  - 🏆 임직원 투표 베스트상

Quick Trick!은  
짧은 시간 안에 누가 더 빠르게 반응했는지를 겨루는 게임으로,  
오프라인 행사 환경에서도 처음 접한 플레이어가 즉시 이해하고 참여할 수 있도록 설계했습니다.

---

## 🧩 세부 구현 내용

### 🔹 매치메이킹 및 세션 흐름 관리
- 플레이어 매칭부터 게임 시작까지의 전체 흐름 설계
<details>
<summary>💡Code</summary>

```{csharp}

```
</details>


### 🔹 멀티플레이 게임 흐름 동기화 (RPC 기반)
- 입력 허용 시점과 결과 판정을 서버 기준으로 동기화
<details>
<summary>💡Code</summary>

```{csharp}

```
</details>

### 🔹 확장 가능한 미니게임 구조 설계 (객체지향)
- 공통 인터페이스 기반의 미니게임 확장 구조
<details>
<summary>💡Code</summary>

```{csharp}

```
</details>

### 🔹 ScriptableObject 기반 데이터 중심 설계
- 사운드 및 미니게임 설정을 데이터로 분리 관리
<details>
<summary>💡Code</summary>

```{csharp}

```
</details>

### 🔹 런타임 시각 효과 및 물리 상호작용 구현
- 랜덤성과 물리를 결합한 동적 연출 구현
<details>
<summary>💡Code</summary>

```{csharp}
public class PopcornEffect : MonoBehaviour
{
    public GameObject ballPrefab; // 하나의 공 프리팹
    public Sprite[] ballSprites;  // 16개의 공 이미지 배열

    [Header("Ball Count Range")]
    public int minBallCount = 2;   // 최소 공 개수 (Inspector에서 설정)
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

    public void CreateExplosion(Vector3 position)
    {
        SoundManager.Instance.PlaySFX("Bloop");
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
            
            // Collider2D 비활성화 후 일정 시간 후 활성화
            Collider2D col = ball.GetComponent<Collider2D>();
            if (col == null)
                col = ball.AddComponent<CircleCollider2D>();  // 원하는 Collider 종류 선택
            col.enabled = false;  // 생성 시 충돌 비활성화

            DOVirtual.DelayedCall(0.4f, () =>
            {
                col.enabled = true;  // n초 후 충돌 활성화
            });

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
```
</details>

전체 구현 코드는 [Scripts 폴더](QuickTrick/Assets/02.Scripts)를 참고해주세요.



