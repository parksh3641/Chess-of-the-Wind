using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
    public ShopType shopType = ShopType.UpgradeTicket_N;

    public Image backgroundImg;

    public Image icon;

    public Text titleText;
    public Text priceRMText;
    public GameObject coin;
    public Text priceCoinText;

    Sprite[] upgradeTicket;

    ImageDataBase imageDataBase;
    ShopManager shopManager;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        upgradeTicket = imageDataBase.upgradeTicketArray;
    }

    public void Initialize(ShopType type, ShopManager manager)
    {
        shopType = type;
        shopManager = manager;

        coin.SetActive(false);

        switch (type)
        {
            case ShopType.RemoveAds:
                break;
            case ShopType.WatchAd:
                break;
            case ShopType.DailyReward:
                break;
            case ShopType.UpgradeTicket_N:
                backgroundImg.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);

                titleText.text = "N 등급 강화권 x5";

                icon.sprite = upgradeTicket[0];

                priceRMText.text = "$ 5";

                break;
            case ShopType.UpgradeTicket_R:
                backgroundImg.color = Color.green;

                titleText.text = "R 등급 강화권 x5";

                icon.sprite = upgradeTicket[1];

                priceRMText.text = "$ 10";

                break;
            case ShopType.UpgradeTicket_SR:
                backgroundImg.color = Color.blue;

                icon.sprite = upgradeTicket[2];

                titleText.text = "SR 등급 강화권 x5";

                priceRMText.text = "$ 15";

                break;
            case ShopType.UpgradeTicket_SSR:
                backgroundImg.color = new Color(1, 0, 1);

                icon.sprite = upgradeTicket[3];

                titleText.text = "SSR 등급 강화권 x5";

                priceRMText.text = "$ 20";

                break;
            case ShopType.UpgradeTicket_UR:
                backgroundImg.color = Color.yellow;

                icon.sprite = upgradeTicket[4];

                titleText.text = "UR 등급 강화권 x5";

                priceRMText.text = "$ 25";

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
    }

    public void BuyButton()
    {
        shopManager.BuyItem(shopType);
    }
}
