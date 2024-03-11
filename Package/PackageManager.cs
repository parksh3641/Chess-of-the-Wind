using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager : MonoBehaviour
{
    PackageType packageType = PackageType.Default;

    public GameObject specialShop;

    public RectTransform packageGrid;
    public PackageContent[] packageContentArray;

    private int count = 0;
    bool first = false;

    public MatchingManager matchingManager;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void Initialize()
    {
        for(int i = 0; i < packageContentArray.Length; i ++)
        {
            packageContentArray[i].gameObject.SetActive(true);
            packageContentArray[i].Initialize(this);
        }
    }

    public void OpenShop()
    {
        if(!first)
        {
            Initialize();

            packageGrid.anchoredPosition = new Vector2(9999, 0);

            first = true;
        }

        specialShop.SetActive(true);

        if (playerDataBase.ShopNewbie == 0)
        {
            packageContentArray[0].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray[0].gameObject.SetActive(false);
        }

        if (playerDataBase.ShopSliver == 0 && GameStateManager.instance.GameRankType > GameRankType.Bronze_1)
        {
            packageContentArray[1].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray[1].gameObject.SetActive(false);
        }

        if (playerDataBase.ShopGold == 0 && GameStateManager.instance.GameRankType > GameRankType.Sliver_1)
        {
            packageContentArray[2].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray[2].gameObject.SetActive(false);
        }

        if (playerDataBase.ShopPlatinum == 0 && GameStateManager.instance.GameRankType > GameRankType.Gold_1)
        {
            packageContentArray[3].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray[3].gameObject.SetActive(false);
        }

        if (playerDataBase.ShopDiamond == 0 && GameStateManager.instance.GameRankType > GameRankType.Platinum_1)
        {
            packageContentArray[4].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray[4].gameObject.SetActive(false);
        }

        if (playerDataBase.ShopLegend == 0 && GameStateManager.instance.GameRankType == GameRankType.Legend_4)
        {
            packageContentArray[5].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray[5].gameObject.SetActive(false);
        }

        CheckSpeicalShop();
    }

    void CheckSpeicalShop()
    {
        bool check = false;
        for (int i = 0; i < packageContentArray.Length; i++)
        {
            if (packageContentArray[i].gameObject.activeInHierarchy)
            {
                check = true;
                break;
            }
        }

        if(!check)
        {
            specialShop.SetActive(false);
        }
    }

    public void BuyPurchase(PackageInfomation type)
    {
        Debug.LogError(type.packageType + " ±¸¸Å");

        FirebaseAnalytics.LogEvent("Buy_Package_Speical_" + type.ToString());

        packageType = type.packageType;

        switch (packageType)
        {
            case PackageType.Default:
                break;
            case PackageType.Newbie:
                playerDataBase.ShopNewbie += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("ShopNewbie", playerDataBase.ShopNewbie);
                break;
            case PackageType.Sliver:
                playerDataBase.ShopSliver += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("ShopSliver", playerDataBase.ShopSliver);
                break;
            case PackageType.Gold:
                playerDataBase.ShopGold += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("ShopGold", playerDataBase.ShopGold);
                break;
            case PackageType.Platinum:
                playerDataBase.ShopPlatinum += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("ShopPlatinum", playerDataBase.ShopPlatinum);
                break;
            case PackageType.Diamond:
                playerDataBase.ShopDiamond += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("ShopDiamond", playerDataBase.ShopDiamond);
                break;
            case PackageType.Legend:
                playerDataBase.ShopLegend += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("ShopLegend", playerDataBase.ShopLegend);
                break;
            case PackageType.Supply:
                playerDataBase.ShopSupply += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("ShopSupply", playerDataBase.ShopSupply);
                break;
        }

        for (int i = 0; i < type.receiveInformationList.Count; i++)
        {
            count = type.receiveInformationList[i].count;

            switch (type.receiveInformationList[i].rewardType)
            {
                case RewardType.Gold:
                    PlayfabManager.instance.UpdateAddGold(count);
                    break;
                case RewardType.UpgradeTicket:
                    ItemAnimManager.instance.GetUpgradeTicket(count);
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
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        NotionManager.instance.UseNotion(NotionType.BuyShopItem);

        Invoke("OffObj", 0.5f);
    }

    public void OffObj()
    {
        if(packageType == PackageType.Supply)
        {
            matchingManager.CloseSupplyPackage();
        }
        else
        {
            packageContentArray[(int)packageType - 1].gameObject.SetActive(false);
        }
    }
}
