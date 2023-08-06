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

    Sprite[] shopContentArray;

    ImageDataBase imageDataBase;
    public ShopManager shopManager;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

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
                goldText.text = "x40";
                freeButton.gameObject.SetActive(true);
                break;
            case ShopType.DailyReward_WatchAd:
                break;
            case ShopType.UpgradeTicket:
                buyButton.SetActive(true);

                number = Random.Range(1, 11);

                price = price * number;

                priceText.text = price.ToString();

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

    public void BuyButton()
    {
        shopManager.BuyItem(shopType, price, number);
    }
}
