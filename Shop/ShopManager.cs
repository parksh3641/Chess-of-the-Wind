using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopView;

    public GameObject[] boxArray;

    public ShopContent shopContent;
    public Transform shopContentTransform;

    List<ShopContent> shopContentList = new List<ShopContent>();

    bool isDelay = false;

    public UIManager uIManager;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        shopView.SetActive(false);

        for (int i = 0; i < 6; i++)
        {
            ShopContent monster = Instantiate(shopContent);
            monster.transform.parent = shopContentTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            monster.Initialize(ShopType.UpgradeTicket_N + i, this);
            monster.gameObject.SetActive(true);

            shopContentList.Add(monster);
        }
    }

    public void OpenShopView()
    {
        if(!shopView.activeSelf)
        {
            shopView.SetActive(true);

            boxArray[0].SetActive(false);
            boxArray[1].SetActive(false);

            if (playerDataBase.Formation == 2)
            {
                boxArray[1].SetActive(true);
            }
            else
            {
                boxArray[0].SetActive(true);
            }
        }
    }

    public void CloseShopView()
    {
        shopView.SetActive(false);
    }

    #region RandomBox
    public void BuySnowBox(int number)
    {
        playerDataBase.SnowBox = number;

        playerDataBase.BuySnowBox += number;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuySnowBox", playerDataBase.BuySnowBox);

        if (number == 1)
        {
            PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 1000);
        }
        else
        {
            PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 9500);
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
    }

    public void BuyUnderworldBox(int number)
    {
        playerDataBase.UnderworldBox = number;

        playerDataBase.BuyUnderworldBox += number;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuyUnderworldBox", playerDataBase.BuyUnderworldBox);

        if (number == 1)
        {
            PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 1000);
        }
        else
        {
            PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 9500);
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
    }

    public void BuyItem(ShopType type)
    {
        if (isDelay) return;

        switch (type)
        {
            case ShopType.RemoveAds:
                break;
            case ShopType.WatchAd:
                break;
            case ShopType.DailyReward:
                break;
            case ShopType.UpgradeTicket_N:
                Debug.Log("N 등급 강화권 구매");
                playerDataBase.SetUpgradeTicket(RankType.N, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert(type.ToString(), playerDataBase.GetUpgradeTicket(RankType.N));
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 1500);

                break;
            case ShopType.UpgradeTicket_R:
                Debug.Log("R 등급 강화권 구매");
                playerDataBase.SetUpgradeTicket(RankType.R, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert(type.ToString(), playerDataBase.GetUpgradeTicket(RankType.R));
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 2500);

                break;
            case ShopType.UpgradeTicket_SR:
                Debug.Log("SR 등급 강화권 구매");
                playerDataBase.SetUpgradeTicket(RankType.SR, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert(type.ToString(), playerDataBase.GetUpgradeTicket(RankType.SR));
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 5000);

                break;
            case ShopType.UpgradeTicket_SSR:
                Debug.Log("SSR 등급 강화권 구매");
                playerDataBase.SetUpgradeTicket(RankType.SSR, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert(type.ToString(), playerDataBase.GetUpgradeTicket(RankType.SSR));
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 10000);

                break;
            case ShopType.UpgradeTicket_UR:
                Debug.Log("UR 등급 강화권 구매");
                playerDataBase.SetUpgradeTicket(RankType.UR, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert(type.ToString(), playerDataBase.GetUpgradeTicket(RankType.UR));
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 15000);

                break;
            case ShopType.DefDestroyTicket:
                Debug.Log("파괴 방지권 구매");
                playerDataBase.DefDestroyTicket += 5;

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("DefDestroyTicket", playerDataBase.DefDestroyTicket);
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 50000);

                break;
            case ShopType.PresentA:
                break;
            case ShopType.PresentB:
                break;
            case ShopType.PresentC:
                break;
            case ShopType.PresentD:
                break;
            case ShopType.PresentE:
                break;
        }

        uIManager.Renewal();

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        NotionManager.instance.UseNotion(NotionType.BuyTicket);

        isDelay = true;
        Invoke("Delay", 0.5f);
    }

    void Delay()
    {
        isDelay = false;
    }

    #endregion

    public void PlusMoney()
    {
        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 100000);
    }

    public void MinusMoney()
    {
        PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, 100000);
    }
}
