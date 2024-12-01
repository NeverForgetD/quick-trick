using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGameSO", menuName = "Scriptable Objects/MiniGameSO")]
public class MiniGameSO : ScriptableObject
{
    public GameObject[] miniGamePrefabs;
}
