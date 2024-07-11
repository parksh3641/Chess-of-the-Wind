using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeContent : MonoBehaviour
{
    public int index = 0;

    public LocalizationContent titleText;
    public LocalizationContent infoText;

    public ReceiveContent receiveContent;

    public GameObject foucsObj;
    public GameObject lockObj;
    public GameObject clearObj;

    public EventManager eventManager;

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

    }

    public void Initialize(int number, bool check, EventManager manager)
    {
        eventManager = manager;

        titleText.localizationName = "Welcome" + (index + 1);
        titleText.ReLoad();

        infoText.localizationName = "Welcome" + (index + 1) + "_Info";

        foucsObj.SetActive(false);
        lockObj.SetActive(true);
        clearObj.SetActive(false);

        if (!check)
        {
            switch (index)
            {
                case 0:
                    if (playerDataBase.CheckBlockLevel(3))
                    {
                        lockObj.SetActive(false);
                    }
                    break;
                case 1:
                    if(GameStateManager.instance.GameRankType > GameRankType.Bronze_1)
                    {
                        lockObj.SetActive(false);
                    }
                    break;
                case 2:
                    if(playerDataBase.Coin >= 25000)
                    {
                        lockObj.SetActive(false);
                    }
                    break;
                case 3:
                    if (playerDataBase.CheckBlockLevel(8))
                    {
                        lockObj.SetActive(false);
                    }
                    break;
                case 4:
                    if (GameStateManager.instance.GameRankType > GameRankType.Sliver_4)
                    {
                        lockObj.SetActive(false);
                    }
                    break;
                case 5:
                    if (GameStateManager.instance.GameRankType > GameRankType.Sliver_3)
                    {
                        lockObj.SetActive(false);
                    }
                    break;
                case 6:
                    if (GameStateManager.instance.GameRankType > GameRankType.Sliver_2)
                    {
                        lockObj.SetActive(false);
                    }
                    break;
            }
        }

        if (index == number)
        {
            if (!check)
            {
                foucsObj.SetActive(true);
            }
        }
        else
        {
            if (index > number)
            {
                lockObj.SetActive(true);
            }
            else
            {
                foucsObj.SetActive(false);
                clearObj.SetActive(true);
            }
        }
    }

    public void ReceiveButton()
    {
        eventManager.WelcomeReceiveButton(index, SuccessReceive);
    }

    public void SuccessReceive()
    {
        foucsObj.SetActive(false);
        clearObj.SetActive(true);
    }
}
