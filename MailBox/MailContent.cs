using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MailContent : MonoBehaviour
{
    public UnityEvent unityEvent;

    public LocalizationContent titleText;

    private RewardType rewardType = RewardType.Gold;
    private TitleSpeicalType titleSpeicalType = TitleSpeicalType.Default;
    private int count = 0;

    List<string> itemList = new List<string>();

    public ReceiveContent[] receiveContent;

    Sprite[] rewardArray;

    ImageDataBase imageDataBase;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rewardArray = imageDataBase.GetRewardArray();
    }


    public void Initialize(string name)
    {
        receiveContent[0].gameObject.SetActive(true);
        receiveContent[1].gameObject.SetActive(false);

        titleSpeicalType = TitleSpeicalType.Default;

        string[] strArray = name.Split('_');

        count = int.Parse(strArray[1]);

        switch (strArray[0])
        {
            case "Gold":
                rewardType = RewardType.Gold;

                titleText.localizationName = "GiftInfo";
                break;
            case "UpgradeTicket":
                rewardType = RewardType.UpgradeTicket;

                titleText.localizationName = "GiftInfo";
                break;
            case "Box":
                rewardType = RewardType.Box;

                titleText.localizationName = "GiftInfo";
                break;
            case "BoxN":
                rewardType = RewardType.Box_N;

                titleText.localizationName = "GiftInfo";
                break;
            case "BoxR":
                rewardType = RewardType.Box_R;

                titleText.localizationName = "GiftInfo";
                break;
            case "BoxSR":
                rewardType = RewardType.Box_SR;

                titleText.localizationName = "GiftInfo";
                break;
            case "BoxSSR":
                rewardType = RewardType.Box_SSR;

                titleText.localizationName = "GiftInfo";
                break;
            case "BoxUR":
                rewardType = RewardType.Box_UR;

                titleText.localizationName = "GiftInfo";
                break;
            case "BoxNR":
                rewardType = RewardType.Box_NR;

                titleText.localizationName = "GiftInfo";
                break;
            case "BoxRSR":
                rewardType = RewardType.Box_RSR;

                titleText.localizationName = "GiftInfo";
                break;
            case "BoxSRSSR":
                rewardType = RewardType.Box_SRSSR;

                titleText.localizationName = "GiftInfo";
                break;
            case "IndieFestival2023":
                rewardType = RewardType.Box;

                count = 10;
                titleSpeicalType = TitleSpeicalType.TitleSpeical1;

                titleText.localizationName = "IndieFestival2023";

                receiveContent[1].gameObject.SetActive(true);
                receiveContent[1].Initialize(RewardType.ExclusiveTitle, 1);
                break;
            case "ComicWorld2023":
                //rewardType = RewardType.Box;

                //count = 10;
                //titleSpeicalType = TitleSpeicalType.TitleSpeical2;

                //titleText.localizationName = "ComicWorld2023";

                //receiveContent[1].gameObject.SetActive(true);
                //receiveContent[1].Initialize(RewardType.ExclusiveTitle, 1);

                rewardType = RewardType.Box;

                count = 10;
                titleSpeicalType = TitleSpeicalType.TitleSpeical1;

                titleText.localizationName = "IndieFestival2023";

                receiveContent[1].gameObject.SetActive(true);
                receiveContent[1].Initialize(RewardType.ExclusiveTitle, 1);
                break;
        }

        titleText.ReLoad();

        receiveContent[0].Initialize(rewardType, count);
    }

    public void Receive()
    {
        switch (rewardType)
        {
            case RewardType.Gold:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, count);
                break;
            case RewardType.UpgradeTicket:
                playerDataBase.SetUpgradeTicket(RankType.N, count);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));
                break;
            case RewardType.Box:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", count);
                        break;
                }
                break;
            case RewardType.Box_N:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_N = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_N", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_N = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_N", count);
                        break;
                }
                break;
            case RewardType.Box_R:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_R = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_R", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_R = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_R", count);
                        break;
                }
                break;
            case RewardType.Box_SR:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_SR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SR", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_SR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SR", count);
                        break;
                }
                break;
            case RewardType.Box_SSR:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_SSR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SSR", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_SSR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SSR", count);
                        break;
                }
                break;
            case RewardType.Box_UR:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_UR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_UR", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_UR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_UR", count);
                        break;
                }
                break;
            case RewardType.Box_NR:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_NR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_NR", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_NR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_NR", count);
                        break;
                }
                break;
            case RewardType.Box_RSR:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_RSR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_RSR", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_RSR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_RSR", count);
                        break;
                }
                break;
            case RewardType.Box_SRSSR:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_SRSSR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SRSSR", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_SRSSR = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SRSSR", count);
                        break;
                }
                break;
        }

        if(titleSpeicalType != TitleSpeicalType.Default)
        {
            if (playerDataBase.CheckSpeicalTitle(titleSpeicalType) == 0)
            {
                itemList.Clear();
                itemList.Add(titleSpeicalType.ToString());

                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                playerDataBase.SetSpeicalTitle(titleSpeicalType);

                Debug.Log("Àü¿ë ÄªÈ£ º¸»ó È¹µæ");
            }
            else
            {
                Debug.Log("ÀÌ¹Ì º¸À¯ÁßÀÎ ÄªÈ£ÀÔ´Ï´Ù.");
            }
        }

        unityEvent.Invoke();

        gameObject.SetActive(false);
    }
}
