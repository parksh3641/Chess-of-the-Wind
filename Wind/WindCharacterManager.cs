using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCharacterManager : MonoBehaviour
{
    public WindCharacter[] windCharacters;

    public ParticleSystem[] windParticleArray;

    public Transform main;

    public PhotonView PV;

    private void Awake()
    {
        Stop();
    }

    [Button]
    public void Initialize(Transform target)
    {
        main.position = target.position;

        for(int i = 0; i < windCharacters.Length; i ++)
        {
            windCharacters[i].gameObject.SetActive(true);
            windCharacters[i].Initialize(target);
        }
    }

    public void MyWhich(int number)
    {
        for (int i = 0; i < windCharacters.Length; i++)
        {
            windCharacters[i].GetComponent<MeshRenderer>().material.color = Color.white;
        }

        windCharacters[number].GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void Stop()
    {
        for (int i = 0; i < windCharacters.Length; i++)
        {
            windCharacters[i].gameObject.SetActive(false);
        }
    }

    [Button]
    public void WindBlowing(int number)
    {
        windParticleArray[number].Play();
    }
}
