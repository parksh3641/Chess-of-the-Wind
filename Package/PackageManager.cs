using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager : MonoBehaviour
{
    PackageType packageType = PackageType.Default;

    public GameObject specialShop;

    public RectTransform packageGrid;
    public RectTransform packageGrid2;

    public PackageContent[] packageContentArray;
    public PackageContent[] packageContentArray2;

    private int count = 0;
    private bool first = false;
    public bool rankUpPackage = false;

    private int gold = 0;

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

        for (int i = 0; i < packageContentArray2.Length; i++)
        {
            packageContentArray2[i].gameObject.SetActive(true);
            packageContentArray2[i].Initialize(this);
        }
    }

    public void OpenShop()
    {
        if(!first)
        {
            Initialize();

            packageGrid.anchoredPosition = new Vector2(9999, 0);
            packageGrid2.anchoredPosition = new Vector2(9999, 0);

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

        if (playerDataBase.ResetInfo.package_ShopDaily1 == 0)
        {
            packageContentArray2[0].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray2[0].gameObject.SetActive(false);
        }

        if (playerDataBase.ResetInfo.package_ShopDaily2 == 0)
        {
            packageContentArray2[1].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray2[1].gameObject.SetActive(false);
        }

        if (playerDataBase.ResetInfo.package_ShopDaily3 == 0)
        {
            packageContentArray2[2].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray2[2].gameObject.SetActive(false);
        }

        if (playerDataBase.ResetInfo.package_ShopWeekly1 == 0)
        {
            packageContentArray2[3].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray2[3].gameObject.SetActive(false);
        }

        if (playerDataBase.ResetInfo.package_ShopWeekly2 == 0)
        {
            packageContentArray2[4].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray2[4].gameObject.SetActive(false);
        }

        if (playerDataBase.ResetInfo.package_ShopWeekly3 == 0)
        {
            packageContentArray2[5].gameObject.SetActive(true);
        }
        else
        {
            packageContentArray2[5].gameObject.SetActive(false);
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
            case PackageType.ShopDaily1:
                if (playerDataBase.ResetInfo.package_ShopDaily1 == 1) return;

                packageContentArray2[0].Locked();

                ResetManager.instance.SetResetInfo(ResetType.Package_ShopDaily1);
                break;
            case PackageType.ShopDaily2:
                if (playerDataBase.ResetInfo.package_ShopDaily2 == 1) return;

                packageContentArray2[1].Locked();

                ResetManager.instance.SetResetInfo(ResetType.Package_ShopDaily2);
                break;
            case PackageType.ShopDaily3:
                if (playerDataBase.ResetInfo.package_ShopDaily3 == 1) return;

                packageContentArray2[2].Locked();

                ResetManager.instance.SetResetInfo(ResetType.Package_ShopDaily3);
                break;
            case PackageType.ShopWeekly1:
                if (playerDataBase.ResetInfo.package_ShopWeekly1 == 1) return;

                packageContentArray2[3].Locked();

                ResetManager.instance.SetResetInfo(ResetType.Package_ShopWeekly1);
                break;
            case PackageType.ShopWeekly2:
                if (playerDataBase.ResetInfo.package_ShopWeekly2 == 1) return;

                packageContentArray2[4].Locked();

                ResetManager.instance.SetResetInfo(ResetType.Package_ShopWeekly2);
                break;
            case PackageType.ShopWeekly3:
                if (playerDataBase.ResetInfo.package_ShopWeekly3 == 1) return;

                packageContentArray2[5].Locked();

                ResetManager.instance.SetResetInfo(ResetType.Package_ShopWeekly3);
                break;
        }

        gold = 0;

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
                case RewardType.GoldShop1:
                    gold += Random.Range(7500, 20001);

                    break;
                case RewardType.GoldShop2:
                    gold += Random.Range(5000, 100001);

                    break;
                case RewardType.GoldShop3:
                    gold += Random.Range(50000, 1000001);

                    break;
            }
        }

        if (gold > 0)
        {
            PlayfabManager.instance.UpdateAddGold(gold);

            gold = 0;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        NotionManager.instance.UseNotion(NotionType.BuyShopItem);

        Invoke("OffObj", 0.5f);
    }

    public void OffObj()
    {
        if(rankUpPackage)
        {
            matchingManager.CloseSupplyPackage();
        }
        else
        {
            packageContentArray[(int)packageType - 1].gameObject.SetActive(false);
        }
    }
}
