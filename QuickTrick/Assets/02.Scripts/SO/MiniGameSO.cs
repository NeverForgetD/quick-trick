using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGameSO", menuName = "Scriptable Objects/MiniGameSO")]
public class MiniGameSO : ScriptableObject
{
    /// <summary>
    /// �̴ϰ��Ӻ� ������
    /// </summary>
    public GameObject[] miniGamePrefabs;

    /// <summary>
    /// �̴ϰ��Ӻ� ���� ��� �����ϴ� �ؽ�Ʈ
    /// </summary>
    public string[] miniGameGuideTexts;


    public GameObject GetMiniGamePrefab(int index)
    {
        if (index < 0 || index >= miniGamePrefabs.Length)
            return null;
        return miniGamePrefabs[index];
    }

    public string GetTextForMiniGame(int index)
    {
        if (index < 0 || index >= miniGameGuideTexts.Length)
            return null;
        return miniGameGuideTexts[index];
    }
}
