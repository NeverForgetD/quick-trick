using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    /// <summary>
    /// ���� �̸�
    /// </summary>
    public string soundName { private set; get; }

    /// <summary>
    /// ����� AudioClip
    /// </summary>
    public AudioClip audioClip;

    /// <summary>
    /// ���� (0-1)
    /// </summary>
    [Range(0f, 1f)] public float volume = 1f;

    /// <summary>
    /// ��ġ
    /// </summary>
    [Range(0f, 1f)] public float pitch = 1f;

    private void OnValidate() // �����Ϳ��� soundName�� ���� �̸����� ���� (���Ǽ�)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            soundName = name;
        }
    }
}
