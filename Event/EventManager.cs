using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public GameObject eventView;

    public GameObject welcomeView;

    public RectTransform welcomGrid;

    public WelcomeContent[] welcomeContentArray;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        eventView.SetActive(false);
        welcomeView.SetActive(false);

        welcomGrid.anchoredPosition = new Vector2(0, -999);
    }


    public void OpenEventView()
    {
        if(!eventView.activeInHierarchy)
        {
            eventView.SetActive(true);
        }
        else
        {
            eventView.SetActive(false);
        }
    }


    #region Welcome
    public void OpenWelcomView()
    {
        if (!welcomeView.activeInHierarchy)
        {
            welcomeView.SetActive(true);

            InitializeWelcome();

            CheckWelcome();
        }
        else
        {
            welcomeView.SetActive(false);
        }
    }

    public void InitializeWelcome()
    {
        welcomeContentArray[0].receiveContent.Initialize(RewardType.Gold, 2000);
        welcomeContentArray[1].receiveContent.Initialize(RewardType.Box_R, 1);
        welcomeContentArray[2].receiveContent.Initialize(RewardType.Box, 5);
        welcomeContentArray[3].receiveContent.Initialize(RewardType.Gold, 10000);
        welcomeContentArray[4].receiveContent.Initialize(RewardType.Box, 10);
        welcomeContentArray[5].receiveContent.Initialize(RewardType.UpgradeTicket, 5);
        welcomeContentArray[6].receiveContent.Initialize(RewardType.Box_SSR, 1);
    }

    public void CheckWelcome()
    {
        welcomeContentArray[0].Initialize();
        welcomeContentArray[1].Initialize();
        welcomeContentArray[2].Initialize();
        welcomeContentArray[3].Initialize();
        welcomeContentArray[4].Initialize();
        welcomeContentArray[5].Initialize();
        welcomeContentArray[6].Initialize();
    }

    public void WelcomeReceiveButton(int number, Action action)
    {
        if (playerDataBase.WelcomeCheck) return;

        switch (number)
        {
            case 0:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 2000);
                break;
            case 1:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_R = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_R", 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_R = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_R", 1);
                        break;
                }
                break;
            case 2:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox = 5;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox", 5);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox = 5;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", 5);
                        break;
                }
                break;
            case 3:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 10000);
                break;
            case 4:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox = 10;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox", 10);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox = 10;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", 10);
                        break;
                }
                break;
            case 5:
                playerDataBase.SetUpgradeTicket(RankType.N, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));
                break;
            case 6:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_SSR = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SSR", 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_SSR = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SSR", 1);
                        break;
                }
                break;
        }

        playerDataBase.WelcomeCount += 1;
        playerDataBase.WelcomeCheck = true;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeCount", playerDataBase.WelcomeCount);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeCheck", 1);

        action.Invoke();

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        NotionManager.instance.UseNotion(NotionType.GetReward);
    }

    #endregion
}
