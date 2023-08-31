using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationUIManager : MonoBehaviour
{
    public bool isActive = true;

    public Image mainBackground;

    public Image[] background;

    public Image characterImg;

    public Image topBackground;
    public Image bottomBackground;
    public Image[] selectBackground;
    public Image[] lobbyButton;


    Sprite[] characterArray;
    Sprite[] formationArray;
    Sprite[] formationRedArray;

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        characterArray = imageDataBase.GetCharacterArray();
        formationArray = imageDataBase.GetFormationArray();
        formationRedArray = imageDataBase.GetFormationRedArray();

        characterImg.enabled = false;
    }

    private void Start()
    {
        switch (GameStateManager.instance.WindCharacterType)
        {
            case WindCharacterType.Winter:
                mainBackground.sprite = formationArray[0];
                break;
            case WindCharacterType.UnderWorld:
                mainBackground.sprite = formationRedArray[0];
                break;
            default:
                mainBackground.sprite = formationArray[0];
                break;
        }
    }

    public void Initialize()
    {
        if (playerDataBase.Formation == 2)
        {
            characterImg.sprite = characterArray[1];

            if (isActive)
            {
                for (int i = 0; i < background.Length; i++)
                {
                    background[i].sprite = formationRedArray[0];
                }

                topBackground.sprite = formationRedArray[1];
                bottomBackground.sprite = formationRedArray[1];

                for (int i = 0; i < selectBackground.Length; i++)
                {
                    selectBackground[i].sprite = formationRedArray[2];
                }

                for (int i = 0; i < lobbyButton.Length; i++)
                {
                    lobbyButton[i].sprite = formationRedArray[3];
                }
            }
        }
        else
        {
            characterImg.sprite = characterArray[0];

            if (isActive)
            {
                for (int i = 0; i < background.Length; i++)
                {
                    background[i].sprite = formationArray[0];
                }

                topBackground.sprite = formationArray[1];
                bottomBackground.sprite = formationArray[1];

                for (int i = 0; i < selectBackground.Length; i++)
                {
                    selectBackground[i].sprite = formationArray[2];
                }

                for (int i = 0; i < lobbyButton.Length; i++)
                {
                    lobbyButton[i].sprite = formationArray[3];
                }
            }
        }

        characterImg.enabled = true;
    }
}
