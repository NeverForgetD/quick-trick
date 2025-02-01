using UnityEngine;

public class ExplodingCapsuleGenerator : MonoBehaviour
{
    public GameObject objectPrefab; // ������ ������Ʈ ������
    public Sprite[] spriteOptions; // ��������Ʈ �ĺ� ���
    public float minX = -12f; // X�� �ּ� ��
    public float maxX = 12f;  // X�� �ִ� ��
    public float spawnY = 12f; // Y�� ���� ��
    public float spawnInterval = 10f; // ���� ���� (��)

    void Start()
    {
        InvokeRepeating(nameof(SpawnObject), 5f, spawnInterval);
    }

    void SpawnObject()
    {
        if (objectPrefab == null || spriteOptions.Length == 0) return;

        // ������ X ��ġ ���
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0);

        // ������Ʈ ����
        GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // ������ ��������Ʈ ����
        SpriteRenderer spriteRenderer = newObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            int randomIndex = Random.Range(0, spriteOptions.Length);
            spriteRenderer.sprite = spriteOptions[randomIndex];
        }
    }
}
