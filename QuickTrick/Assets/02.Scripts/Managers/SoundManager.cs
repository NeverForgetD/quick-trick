using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton&Init
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        Init();
        InitAudioSource();
    }

    private void Init()
    {
        if (Instance == null)
        {
            Instance = this;
            // �� ��ȯ �� �ı����� �ʵ��� ���� (�ʿ� ��)
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitAudioSource()
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
        }

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
    }
    #endregion

    #region Privates
    AudioSource bgmSource;
    AudioSource sfxSource;

    bool isBGMPlaying = false;
    #endregion

    #region Dependencies
    [Header("Sound Database")]
    [SerializeField] SoundDB soundDB;
    #endregion

    #region Sound Play Util
    // MP3 Player   -> AudioSource
    // MP3 ����   -> AudioClip
    // ����(��)    -> AudioListner

    /// <summary>
    /// BGM ���
    /// </summary>
    /// <param name="bgmName"></param>
    public void PlayBGM(string bgmName)
    {
        if (isBGMPlaying)
            return;

        SoundData bgm = FindSound(soundDB.bgmList, bgmName); // �˻�
        if (bgm != null && !isBGMPlaying)
        {
            bgmSource.clip = bgm.audioClip;
            bgmSource.volume = bgm.volume;
            bgmSource.Play();
            isBGMPlaying = true;
        }
        else
        {
            Debug.Log($"Failed to find sound data_BGM : {bgmName}");
        }
    }

    public void StopBGM()
    {
        if (!isBGMPlaying)
            return;
        bgmSource.Stop();
        isBGMPlaying = false;
    }

    /// <summary>
    /// SFX ���
    /// </summary>
    /// <param name="sfxName"></param>
    public void PlaySFX(string sfxName)
    {
        SoundData sfx = FindSound(soundDB.sfxList, sfxName); // �˻�
        if (sfx == null)
        {
            Debug.Log($"Failed to find sound data_SFX : {sfxName}");
        }
        else
        {
            sfxSource.pitch = sfx.pitch;
            sfxSource.volume = sfx.volume;
            sfxSource.PlayOneShot(sfx.audioClip);
        }
    }

    /// <summary>
    /// SoundData �迭���� �ش� �̸��� sound data�� �˻�
    /// </summary>
    /// <param name="soundList"></param>
    /// <param name="soundName"></param>
    /// <returns></returns>
    private SoundData FindSound(SoundData[] soundList, string soundName)
    {
        foreach (SoundData sound in soundList)
        {
            if (sound.soundName == soundName) // �̸� ��
            {
                return sound;
            }
        }
        return null;
    }

    /// <summary>
    /// SFX�� ��� ������ Ȯ��
    /// </summary>
    /// <returns></returns>
    public bool IsSfxPlaying()
    {
        return sfxSource.isPlaying;
    }
    /*
    /// <summary>
    /// SFX ��� ���� ������ ���
    /// </summary>
    /// <returns></returns>
    public async Task WaitForSfxEnd()
    {
        while (sfxSource.isPlaying)
        {
            await Task.Yield();
        }
    }
    */
    #endregion
}
