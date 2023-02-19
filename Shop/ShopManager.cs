using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopView;

    public ShopContent shopContent;
    public Transform shopContentTransform;

    List<ShopContent> shopContentList = new List<ShopContent>();

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        shopView.SetActive(false);

        for (int i = 0; i < 5; i++)
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
    }

    public void BuyUnderworldBox(int number)
    {
        playerDataBase.UnderworldBox = number;
    }

    public void BuyItem(ShopType type)
    {
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
                break;
            case ShopType.UpgradeTicket_R:
                Debug.Log("R 등급 강화권 구매");
                playerDataBase.SetUpgradeTicket(RankType.R, 5);
                break;
            case ShopType.UpgradeTicket_SR:
                Debug.Log("SR 등급 강화권 구매");
                playerDataBase.SetUpgradeTicket(RankType.SR, 5);
                break;
            case ShopType.UpgradeTicket_SSR:
                Debug.Log("SSR 등급 강화권 구매");
                playerDataBase.SetUpgradeTicket(RankType.SSR, 5);
                break;
            case ShopType.UpgradeTicket_UR:
                Debug.Log("UR 등급 강화권 구매");
                playerDataBase.SetUpgradeTicket(RankType.UR, 5);
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

        NotionManager.instance.UseNotion(NotionType.BuyTicket);
    }

    #endregion
}
