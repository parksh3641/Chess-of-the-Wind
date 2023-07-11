using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] bgmAudio; 
    public AudioSource[] sfxAudio;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        audioSource.volume = 0;
    }

    public void Initialize()
    {
        audioSource.volume = 1;

        if (playerDataBase.Formation >= 1)
        {
            PlayBGM(GameBgmType.Main_Snow);
        }
        else
        {
            PlayBGM(GameBgmType.Main_Under);
        }
    }


    public void PlayBGM(GameBgmType type)
    {
        for (int i = 0; i < bgmAudio.Length; i++)
        {
            if (bgmAudio[i].name.Equals(type.ToString()))
            {
                audioSource.Stop();
                audioSource.clip = bgmAudio[i];
                audioSource.Play();
            }
        }
    }

    public void PlaySFX(GameSfxType type)
    {
        for (int i = 0; i < sfxAudio.Length; i++)
        {
            if (sfxAudio[i].name.Equals(type.ToString()))
            {
                if (!sfxAudio[i].isPlaying) sfxAudio[i].Play();
            }
        }
    }

    public void StopSFX(GameSfxType type)
    {
        for (int i = 0; i < sfxAudio.Length; i++)
        {
            if (sfxAudio[i].name.Equals(type.ToString()))
            {
                if (sfxAudio[i].isPlaying) sfxAudio[i].Stop();
            }
        }
    }

    public void StopAllSFX()
    {
        for (int i = 0; i < sfxAudio.Length; i++)
        {
            if (sfxAudio[i].isPlaying)
            {
                sfxAudio[i].Stop();
                sfxAudio[i].loop = false;
            }
        }
    }

    public void PlayLoopSFX(GameSfxType type)
    {
        for (int i = 0; i < sfxAudio.Length; i++)
        {
            if (sfxAudio[i].name.Equals(type.ToString()))
            {
                if (!sfxAudio[i].isPlaying)
                {
                    sfxAudio[i].Play();
                    sfxAudio[i].loop = true;
                }
            }
        }
    }

    public void StopLoopSFX(GameSfxType type)
    {
        for (int i = 0; i < sfxAudio.Length; i++)
        {
            if (sfxAudio[i].name.Equals(type.ToString()))
            {
                if (sfxAudio[i].isPlaying)
                {
                    sfxAudio[i].Stop();
                    sfxAudio[i].loop = false;
                }
            }
        }
    }
}
