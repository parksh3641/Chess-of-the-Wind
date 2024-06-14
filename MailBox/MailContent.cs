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
            case "BoxNormal":
                rewardType = RewardType.Box_Normal;

                titleText.localizationName = "GiftInfo";
                break;
            case "BoxEpic":
                rewardType = RewardType.Box_Normal;

                titleText.localizationName = "GiftInfo";
                break;
            case "BoxSpeical":
                rewardType = RewardType.Box_Normal;

                titleText.localizationName = "GiftInfo";
                break;
            case "IndieFestival2023":
                rewardType = RewardType.Box_Normal;

                count = 10;
                titleSpeicalType = TitleSpeicalType.TitleSpeical1;

                titleText.localizationName = "IndieFestival2023";

                receiveContent[1].gameObject.SetActive(true);
                receiveContent[1].Initialize(RewardType.ExclusiveTitle, 1);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("IndieFestival2023", 1);
                break;
            case "ComicWorld2023":
                rewardType = RewardType.Box_Normal;

                count = 10;
                titleSpeicalType = TitleSpeicalType.TitleSpeical1;

                titleText.localizationName = "IndieFestival2023";

                receiveContent[1].gameObject.SetActive(true);
                receiveContent[1].Initialize(RewardType.ExclusiveTitle, 1);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("IndieFestival2023", 1);
                break;
            case "AppReview":
                rewardType = RewardType.Box_Epic;

                titleText.localizationName = "AppReviewTitle";

                playerDataBase.ReviewNumber = 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("ReviewNumber", 1);
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
                PlayfabManager.instance.UpdateAddGold(count);
                break;
            case RewardType.UpgradeTicket:
                ItemAnimManager.instance.GetUpgradeTicket(count);
                break;
            case RewardType.Box_Normal:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Normal = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Normal = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", count);
                        break;
                }
                break;
            case RewardType.Box_Epic:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Epic = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Epic", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Epic = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Epic", count);
                        break;
                }
                break;
            case RewardType.Box_Speical:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Speical = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Speical", count);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Speical = count;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Speical", count);
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

                Debug.Log("���� Īȣ ���� ȹ��");
            }
            else
            {
                Debug.Log("�̹� �������� Īȣ�Դϴ�.");
            }
        }

        unityEvent.Invoke();

        gameObject.SetActive(false);
    }
}
