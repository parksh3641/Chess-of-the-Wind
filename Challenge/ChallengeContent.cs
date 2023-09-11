using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeContent : MonoBehaviour
{
    public int index = 0;

    public LocalizationContent titleText;
    public LocalizationContent infoText;

    public ReceiveContent receiveContent;

    public GameObject focusObj;
    public GameObject[] lockObj;
    public GameObject[] clearObj;

    public ChallengeManager challengeManager;

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
    }

    public void Initialize(int number, ChallengeManager manager)
    {
        challengeManager = manager;

        titleText.localizationName = "Challenge" + (index + 1);
        titleText.ReLoad();

        infoText.localizationName = "Challenge" + (index + 1) + "_Info";

        focusObj.SetActive(false);

        lockObj[0].SetActive(true);
        lockObj[1].SetActive(true);

        clearObj[0].SetActive(false);
        clearObj[1].SetActive(false);

        if (index == number)
        {
            focusObj.SetActive(true);
            lockObj[0].SetActive(false);

            switch (index)
            {
                case 0:
                    if (playerDataBase.CheckEquipBlock_Newbie())
                    {
                        lockObj[1].SetActive(false);
                    }
                    break;
                case 1:
                    if(playerDataBase.NewbieWin > 0)
                    {
                        lockObj[1].SetActive(false);
                    }
                    break;
                case 2:
                    if (playerDataBase.NewbieWin > 1)
                    {
                        lockObj[1].SetActive(false);
                    }
                    break;
                case 3:
                    if (playerDataBase.CheckEquipBlock_Gosu() == 3)
                    {
                        lockObj[1].SetActive(false);
                    }
                    break;
                case 4:
                    if (playerDataBase.CheckBlockLevelCount() == 3)
                    {
                        lockObj[1].SetActive(false);
                    }
                    break;
                case 5:
                    if (playerDataBase.GosuWin > 0)
                    {
                        lockObj[1].SetActive(false);
                    }
                    break;
            }
        }

        if(index < number)
        {
            clearObj[0].SetActive(true);
            clearObj[1].SetActive(true);
        }
    }

    public void ReceiveButton()
    {
        challengeManager.ReceiveButton(index, SuccessReceive);
    }

    public void SuccessReceive()
    {
        focusObj.SetActive(false);
        clearObj[0].SetActive(true);
        clearObj[1].SetActive(true);
    }

    public void ShortCutButton()
    {
        challengeManager.ShortCutButton(index);
    }
}
