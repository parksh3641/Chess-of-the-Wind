using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MailContent : MonoBehaviour
{
    public UnityEvent unityEvent;

    private RewardType rewardType = RewardType.Gold;
    private int count = 0;

    public ReceiveContent receiveContent;

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
        string[] strArray = name.Split('_');

        count = int.Parse(strArray[1]);

        switch (strArray[0])
        {
            case "Gold":
                rewardType = RewardType.Gold;
                break;
            case "UpgradeTicket":
                rewardType = RewardType.UpgradeTicket;
                break;
            case "Box":
                rewardType = RewardType.Box;
                break;
            case "BoxN":
                rewardType = RewardType.Box_N;
                break;
            case "BoxR":
                rewardType = RewardType.Box_R;
                break;
            case "BoxSR":
                rewardType = RewardType.Box_SR;
                break;
            case "BoxSSR":
                rewardType = RewardType.Box_SSR;
                break;
            case "BoxUR":
                rewardType = RewardType.Box_UR;
                break;
            case "BoxNR":
                rewardType = RewardType.Box_NR;
                break;
            case "BoxRSR":
                rewardType = RewardType.Box_RSR;
                break;
            case "BoxSRSSR":
                rewardType = RewardType.Box_SRSSR;
                break;
        }

        receiveContent.Initialize(rewardType, count);
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

        unityEvent.Invoke();

        gameObject.SetActive(false);
    }
}
