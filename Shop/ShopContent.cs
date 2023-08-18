using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
    public MoneyType moneyType = MoneyType.Gold;
    public ShopType shopType = ShopType.UpgradeTicket;
    public int price = 0;
    public int number = 0;

    public Image backgroundImg;

    public Sprite[] backgroundImgArray;

    public Image icon;

    public LocalizationContent titleText;

    public GameObject buyButton;
    public Text priceText;

    public Text goldText;
    public GameObject freeButton;

    public GameObject[] rmButton;
    public LocalizationContent[] rmButtonText;

    public GameObject lockObj;

    Sprite[] shopContentArray;

    ImageDataBase imageDataBase;
    RankDataBase rankDataBase;

    public ShopManager shopManager;

    private void Awake()
    {
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

                if (GameStateManager.instance.DailyReward)
                {
                    lockObj.SetActive(true);
                }

                if(GameStateManager.instance.GameRankType > GameRankType.Sliver_4)
                {
                    price = rankDataBase.GetRankInformation(GameRankType.Sliver_4).stakes;
                }
                else
                {
                    price = rankDataBase.GetRankInformation(GameStateManager.instance.GameRankType).stakes;
                }

                goldText.text = MoneyUnitString.ToCurrencyString(price);
                freeButton.gameObject.SetActive(true);

                break;
            case ShopType.DailyReward_WatchAd:
                break;
            case ShopType.UpgradeTicket:
                if(moneyType == MoneyType.Gold)
                {
                    lockObj.SetActive(false);

                    buyButton.SetActive(true);

                    price = price * number;

                    priceText.text = MoneyUnitString.ToCurrencyString(price);
                }
                else
                {
                    if(number == 10)
                    {
                        rmButton[0].SetActive(true);
                        rmButtonText[0].localizationName = "UpgradeTicket_10";
                        rmButtonText[0].ReLoad();
                    }
                    else if (number == 100)
                    {
                        rmButton[1].SetActive(true);
                        rmButtonText[1].localizationName = "UpgradeTicket_100";
                        rmButtonText[1].ReLoad();
                    }
                    else if (number == 1000)
                    {
                        rmButton[2].SetActive(true);
                        rmButtonText[2].localizationName = "UpgradeTicket_1000";
                        rmButtonText[2].ReLoad();
                    }
                }

                break;
        }

        titleText.localizationName = type.ToString();

        if (number > 0)
        {
            titleText.plusText = " x" + number;
        }
        titleText.ReLoad();

        icon.sprite = shopContentArray[(int)type];
    }

    public void Locked()
    {
        lockObj.SetActive(true);
    }

    public void BuyButton()
    {
        shopManager.BuyItem(shopType, price, number);
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
