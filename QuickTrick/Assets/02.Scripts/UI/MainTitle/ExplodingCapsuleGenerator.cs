using UnityEngine;

public class ExplodingCapsuleGenerator : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 오브젝트 프리팹
    public Sprite[] spriteOptions; // 스프라이트 후보 목록
    public float minX = -12f; // X축 최소 값
    public float maxX = 12f;  // X축 최대 값
    public float spawnY = 12f; // Y축 고정 값
    public float spawnInterval = 10f; // 생성 간격 (초)

    void Start()
    {
        InvokeRepeating(nameof(SpawnObject), 5f, spawnInterval);
    }

    void SpawnObject()
    {
        if (objectPrefab == null || spriteOptions.Length == 0) return;

        // 랜덤한 X 위치 계산
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0);

        // 오브젝트 생성
        GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // 랜덤한 스프라이트 적용
        SpriteRenderer spriteRenderer = newObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            int randomIndex = Random.Range(0, spriteOptions.Length);
            spriteRenderer.sprite = spriteOptions[randomIndex];
        }
    }
}
