using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
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

        goldText.text = "";
        freeButton.gameObject.SetActive(false);

        switch (type)
        {
            case ShopType.DailyReward:

                if (GameStateManager.instance.DailyReward)
                {
                    lockObj.SetActive(true);
                }
                else
                {
                    lockObj.SetActive(false);
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
                lockObj.SetActive(false);

                buyButton.SetActive(true);

                price = price * number;

                priceText.text = MoneyUnitString.ToCurrencyString(price);

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
}
