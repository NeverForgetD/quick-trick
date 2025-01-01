using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    /// <summary>
    /// 사운드 이름
    /// </summary>
    public string soundName { private set; get; }

    /// <summary>
    /// 재생할 AudioClip
    /// </summary>
    public AudioClip audioClip;

    /// <summary>
    /// 볼륨 (0-1)
    /// </summary>
    [Range(0f, 1f)] public float volume = 1f;

    /// <summary>
    /// 피치
    /// </summary>
    [Range(0f, 1f)] public float pitch = 1f;

    private void OnValidate() // 에디터에서 soundName을 파일 이름으로 지정 (편의성)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            soundName = name;
        }
    }
}
