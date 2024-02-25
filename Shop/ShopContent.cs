using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
    public MoneyType moneyType = MoneyType.CoinA;
    public ShopType shopType = ShopType.UpgradeTicket;
    public int index = 0;
    public int price = 0;
    public int number = 0;
    public int totalPrice = 0;

    public Image backgroundImg;

    public Sprite[] backgroundImgArray;

    public LocalizationContent titleText;

    public ReceiveContent receiveContent;

    public GameObject buyButton;
    public Text priceText;

    public Text goldText;
    public GameObject freeButton;

    public GameObject[] rmButton;
    public LocalizationContent[] rmButtonText;

    public GameObject lockObj;

    Sprite[] shopContentArray;

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;
    RankDataBase rankDataBase;

    public ShopManager shopManager;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;

        shopContentArray = imageDataBase.shopContentArray;
    }

    private void Start()
    {
        Initialize(shopType);
    }

    public void Initialize(ShopType type)
    {
        shopType = type;

        buyButton.SetActive(false);

        lockObj.SetActive(false);

        goldText.text = "";
        freeButton.gameObject.SetActive(false);

        for(int i = 0; i < rmButton.Length; i ++)
        {
            rmButton[i].SetActive(false);
        }

        switch (type)
        {
            case ShopType.DailyReward:
                if (playerDataBase.DailyReward == 1)
                {
                    lockObj.SetActive(true);
                }

                price = rankDataBase.GetRankInformation(GameStateManager.instance.GameRankType).stakes;
                totalPrice = price;

                goldText.text = MoneyUnitString.ToCurrencyString(price);
                freeButton.gameObject.SetActive(true);

                receiveContent.Initialize(RewardType.Gold, price);

                break;
            case ShopType.DailyReward_WatchAd:
                break;
            case ShopType.UpgradeTicket:
                if (moneyType == MoneyType.CoinA)
                {
                    lockObj.SetActive(false);

                    if(index == 0)
                    {
                        if(playerDataBase.DailyBuy1 == 1)
                        {
                            lockObj.SetActive(true);

                            number = playerDataBase.DailyBuyCount1;
                        }
                        else
                        {
                            if(playerDataBase.DailyBuyCount1 == 0)
                            {
                                number = Random.Range(1, 10);

                                playerDataBase.DailyBuyCount1 = number;

                                PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyBuyCount1", playerDataBase.DailyBuyCount1);
                            }
                            else
                            {
                                number = playerDataBase.DailyBuyCount1;
                            }
                        }
                    }
                    else
                    {
                        if (playerDataBase.DailyBuy2 == 1)
                        {
                            lockObj.SetActive(true);

                            number = playerDataBase.DailyBuyCount2;
                        }
                        else
                        {
                            if (playerDataBase.DailyBuyCount2 == 0)
                            {
                                number = Random.Range(11, 51);

                                playerDataBase.DailyBuyCount2 = number;

                                PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyBuyCount2", playerDataBase.DailyBuyCount2);
                            }
                            else
                            {
                                number = playerDataBase.DailyBuyCount2;
                            }
                        }
                    }

                    buyButton.SetActive(true);
                    totalPrice = price * number;
                    priceText.text = MoneyUnitString.ToCurrencyString(totalPrice);
                }
                else
                {
                    if(number == 1)
                    {
                        rmButton[0].SetActive(true);
                        rmButtonText[0].localizationName = "UpgradeTicket_10";
                        rmButtonText[0].ReLoad();
                    }
                    else if (number == 10)
                    {
                        rmButton[1].SetActive(true);
                        rmButtonText[1].localizationName = "UpgradeTicket_100";
                        rmButtonText[1].ReLoad();
                    }
                    else if (number == 100)
                    {
                        rmButton[2].SetActive(true);
                        rmButtonText[2].localizationName = "UpgradeTicket_1000";
                        rmButtonText[2].ReLoad();
                    }
                }

                receiveContent.Initialize(RewardType.UpgradeTicket, number);

                break;
        }

        titleText.localizationName = type.ToString();

        //if (number > 0)
        //{
        //    titleText.plusText = " x" + number;
        //}

        titleText.ReLoad();

        //icon.sprite = shopContentArray[(int)type];
    }

    public void UnLocked()
    {
        lockObj.SetActive(false);
    }

    public void Locked()
    {
        lockObj.SetActive(true);
    }

    public void BuyButton()
    {
        shopManager.BuyItem(shopType, totalPrice, number);
    }

    public void BuyUpgradeTicket1()
    {
        shopManager.BuyPurchase(0);
    }

    public void BuyUpgradeTicket2()
    {
        shopManager.BuyPurchase(1);
    }

    public void BuyUpgradeTicket3()
    {
        shopManager.BuyPurchase(2);
    }


    public void Failed()
    {
        SoundManager.instance.PlaySFX(GameSfxType.Wrong);

        NotionManager.instance.UseNotion(NotionType.CancelPurchase);
    }
}
