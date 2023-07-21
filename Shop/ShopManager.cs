using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject shopView;

    public RectTransform shopRectTransform;

    public GameObject[] boxArray;

    [Space]
    [Title("Value")]
    private int[] goldValue = new int[] { 1000, 2000, 3000, 4000, 5000, 6000 };


    public ShopContent shopContent;

    public Transform shopContentGoldTransform;
    public Transform shopContentTransform;

    List<ShopContent> shopContentGoldList = new List<ShopContent>();
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
            monster.transform.parent = shopContentGoldTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            monster.Initialize(ShopType.UpgradeTicket_N + i, MoneyType.Gold, this);
            monster.gameObject.SetActive(true);

            shopContentGoldList.Add(monster);
        }

        for (int i = 0; i < 6; i++)
        {
            ShopContent monster = Instantiate(shopContent);
            monster.transform.parent = shopContentTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            monster.Initialize(ShopType.UpgradeTicket_N + i, MoneyType.Crystal, this);
            monster.gameObject.SetActive(true);

            //if (i == 4) monster.gameObject.SetActive(false);

            shopContentList.Add(monster);
        }

        shopRectTransform.sizeDelta = new Vector2(0, -999);
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

    public void BuyItem(ShopType type, MoneyType moneyType)
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
                if(moneyType == MoneyType.Gold)
                {
                    if(playerDataBase.Gold >= goldValue[0])
                    {
                        PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, goldValue[0]);
                    }
                    else
                    {
                        NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                        return;
                    }
                }
                else
                {
                    PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 1500);
                }

                playerDataBase.SetUpgradeTicket(RankType.N, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert(type.ToString(), playerDataBase.GetUpgradeTicket(RankType.N));

                break;
            case ShopType.UpgradeTicket_R:
                if (moneyType == MoneyType.Gold)
                {
                    if (playerDataBase.Gold >= goldValue[1])
                    {
                        PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, goldValue[1]);
                    }
                    else
                    {
                        NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                        return;
                    }
                }
                else
                {
                    PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 2500);
                }

                playerDataBase.SetUpgradeTicket(RankType.R, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert(type.ToString(), playerDataBase.GetUpgradeTicket(RankType.R));

                break;
            case ShopType.UpgradeTicket_SR:
                if (moneyType == MoneyType.Gold)
                {
                    if (playerDataBase.Gold >= goldValue[2])
                    {
                        PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, goldValue[2]);
                    }
                    else
                    {
                        NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                        return;
                    }
                }
                else
                {
                    PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 5000);
                }

                playerDataBase.SetUpgradeTicket(RankType.SR, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert(type.ToString(), playerDataBase.GetUpgradeTicket(RankType.SR));

                break;
            case ShopType.UpgradeTicket_SSR:
                if (moneyType == MoneyType.Gold)
                {
                    if (playerDataBase.Gold >= goldValue[3])
                    {
                        PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, goldValue[3]);
                    }
                    else
                    {
                        NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                        return;
                    }
                }
                else
                {
                    PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 10000);
                }

                playerDataBase.SetUpgradeTicket(RankType.SSR, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert(type.ToString(), playerDataBase.GetUpgradeTicket(RankType.SSR));

                break;
            case ShopType.UpgradeTicket_UR:
                if (moneyType == MoneyType.Gold)
                {
                    if (playerDataBase.Gold >= goldValue[4])
                    {
                        PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, goldValue[4]);
                    }
                    else
                    {
                        NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                        return;
                    }
                }
                else
                {
                    PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 15000);
                }

                playerDataBase.SetUpgradeTicket(RankType.UR, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert(type.ToString(), playerDataBase.GetUpgradeTicket(RankType.UR));

                break;
            case ShopType.DefDestroyTicket:
                if (moneyType == MoneyType.Gold)
                {
                    if (playerDataBase.Gold >= goldValue[5])
                    {
                        PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, goldValue[5]);
                    }
                    else
                    {
                        NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                        return;
                    }
                }
                else
                {
                    PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 50000);
                }

                playerDataBase.DefDestroyTicket += 5;

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("DefDestroyTicket", playerDataBase.DefDestroyTicket);

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

        NotionManager.instance.UseNotion(NotionType.BuyShopItem);

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
