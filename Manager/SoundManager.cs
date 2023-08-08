using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;

    public AudioClip[] bgmAudio; 
    public AudioSource[] sfxAudio;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        instance = this;

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void Initialize()
    {
        if (GameStateManager.instance.Music)
        {
            audioSource.volume = 1;
        }
        else
        {
            audioSource.volume = 0;
        }

        if (playerDataBase.Formation == 2)
        {
            PlayBGM(GameBgmType.Main_Under);
        }
        else
        {
            PlayBGM(GameBgmType.Main_Snow);
        }
    }


    public void PlayBGM(GameBgmType type)
    {
        if (!GameStateManager.instance.Music) return;

        bool check = false;

        for (int i = 0; i < bgmAudio.Length; i++)
        {
            if (bgmAudio[i].name.Equals(type.ToString()))
            {
                check = true;
                audioSource.Stop();
                audioSource.clip = bgmAudio[i];
                audioSource.Play();
            }
        }

        if(!check)
        {
            PlayBGM(GameBgmType.Game_Gosu);
        }
    }

    public void PlayBGM()
    {
        audioSource.volume = 1;
    }

    public void StopBGM()
    {
        audioSource.volume = 0;
    }

    public void PlayBGMLow()
    {
        audioSource.volume = 0f;
    }

    public void PlaySFX(GameSfxType type)
    {
        if (!GameStateManager.instance.Sfx) return;

        for (int i = 0; i < sfxAudio.Length; i++)
        {
            if (sfxAudio[i].name.Equals(type.ToString()))
            {
                sfxAudio[i].Play();
                sfxAudio[i].loop = false;
            }
        }
    }

    public void StopSFX(GameSfxType type)
    {
        for (int i = 0; i < sfxAudio.Length; i++)
        {
            if (sfxAudio[i].name.Equals(type.ToString()))
            {
                sfxAudio[i].Stop();
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
        if (!GameStateManager.instance.Sfx) return;

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
