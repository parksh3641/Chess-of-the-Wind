using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] sfxAudio;

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
