using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public Image characterImg;

    Sprite[] characterArray;


    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;

    void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        characterArray = imageDataBase.GetCharacterArray();

        characterImg.enabled = false;
    }

    public void Initialize()
    {
        if (playerDataBase.Formation == 2)
        {
            characterImg.sprite = characterArray[1];
        }
        else
        {
            characterImg.sprite = characterArray[0];
        }

        characterImg.enabled = true;
    }
}
