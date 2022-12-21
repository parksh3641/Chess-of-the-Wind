using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCharacterManager : MonoBehaviour
{
    public WindCharacter[] windCharacters;

    public Transform main;

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

    public void Stop()
    {
        for (int i = 0; i < windCharacters.Length; i++)
        {
            windCharacters[i].gameObject.SetActive(false);
        }
    }
}
