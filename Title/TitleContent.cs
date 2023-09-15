using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleContent : MonoBehaviour
{
    public TitleNormalType titleNormalType = TitleNormalType.Default;
    public TitleSpeicalType titleSpeicalType = TitleSpeicalType.Default;

    public Image background;

    public Text titleText;
    public Text titleInfoText;

    public GameObject alarmObj;

    public GameObject equipButton;
    public GameObject checkMark;

    public bool isActive = false;

    public Sprite lockedBackground;

    Sprite[] rankBackgroundArray;

    public TitleManager titleManager;

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rankBackgroundArray = imageDataBase.GetRankBackgroundArray();

        isActive = false;

        equipButton.SetActive(false);
        checkMark.SetActive(false);
        alarmObj.SetActive(false);
    }

    public void Initialize()
    {
        if (titleNormalType == TitleNormalType.Default)
        {
            if (titleSpeicalType == TitleSpeicalType.Default)
            {
                titleText.text = LocalizationManager.instance.GetString("NoTitle");
                titleInfoText.text = "";

                equipButton.SetActive(true);

                isActive = true;
            }
            else
            {
                titleText.text = LocalizationManager.instance.GetString(titleSpeicalType.ToString());
                titleInfoText.text = LocalizationManager.instance.GetString(titleSpeicalType.ToString() + "_Info");

                if(playerDataBase.CheckSpeicalTitle(titleSpeicalType) == 1)
                {
                    equipButton.SetActive(true);

                    isActive = true;

                    background.sprite = rankBackgroundArray[2];
                }
                else
                {
                    isActive = false;

                    background.sprite = lockedBackground;
                }
            }
        }
        else
        {
            titleText.text = LocalizationManager.instance.GetString(titleNormalType.ToString());
            titleInfoText.text = LocalizationManager.instance.GetString(titleNormalType.ToString() + "_Info");

            if (playerDataBase.CheckNormalTitle(titleNormalType) == 1)
            {
                equipButton.SetActive(true);

                isActive = true;

                InitializeNormalBackground();
            }
            else
            {
                isActive = false;

                background.sprite = lockedBackground;
            }
        }
    }

    void InitializeNormalBackground()
    {
        background.sprite = rankBackgroundArray[(int)(titleNormalType - 1) % 5];
    }

    public void OnClick()
    {
        if(isActive)
        {
            if(titleSpeicalType == TitleSpeicalType.Default)
            {
                titleManager.SetNormalTitle(titleNormalType);
            }
            else
            {
                titleManager.SetSpeicalTitle(titleSpeicalType);
            }

            SetAlarm(false);
        }
    }

    public void Equip()
    {
        checkMark.SetActive(true);
    }

    public void UnEquip()
    {
        checkMark.SetActive(false);
    }

    public void SetAlarm(bool check)
    {
        alarmObj.SetActive(check);
    }
}
