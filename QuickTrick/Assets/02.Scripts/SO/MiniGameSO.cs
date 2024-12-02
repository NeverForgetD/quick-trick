using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGameSO", menuName = "Scriptable Objects/MiniGameSO")]
public class MiniGameSO : ScriptableObject
{
    /// <summary>
    /// 미니게임별 프리팹
    /// </summary>
    public MiniGameBase[] miniGamePrefabs;

    /// <summary>
    /// 미니게임별 게임 방법 설명하는 텍스트
    /// </summary>
    public string[] miniGameGuideTexts;


    public MiniGameBase GetMiniGamePrefab(int index)
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
