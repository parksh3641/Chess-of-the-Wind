using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
    public ShopType shopType = ShopType.UpgradeTicket_N;
    public MoneyType moneyType = MoneyType.Gold;

    public Image backgroundImg;

    public Sprite[] backgroundImgArray;

    public Image icon;

    public LocalizationContent titleText;

    public GameObject adButton;

    public GameObject buyButton;
    public LocalizationContent priceText;

    public GameObject goldObj;
    public Text goldText;

    Sprite[] shopContentArray;

    ImageDataBase imageDataBase;
    ShopManager shopManager;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        shopContentArray = imageDataBase.shopContentArray;
    }

    public void Initialize(ShopType type, MoneyType money, ShopManager manager)
    {
        shopType = type;
        shopManager = manager;

        moneyType = money;

        buyButton.SetActive(true);

        if (moneyType == MoneyType.Gold)
        {
            adButton.SetActive(false);
            goldObj.SetActive(true);

            priceText.gameObject.SetActive(false);
        }
        else
        {
            adButton.SetActive(true);
            goldObj.SetActive(false);

            priceText.gameObject.SetActive(true);

            priceText.localizationName = shopType + "_Price";
            //priceText.ReLoad();
        }


        icon.sprite = shopContentArray[(int)type];

        switch (type)
        {
            case ShopType.RemoveAds:
                break;
            case ShopType.WatchAd:
                break;
            case ShopType.DailyReward:
                break;
            case ShopType.UpgradeTicket_N:
                backgroundImg.sprite = backgroundImgArray[1];

                if(moneyType == MoneyType.Gold)
                {
                    titleText.localizationName = "GradeN";
                    titleText.localizationName2 = "UpgradeTicket";
                    titleText.plusText = " x1";
                    goldText.text = "1,000";
                }
                else
                {
                    titleText.localizationName = "GradeN";
                    titleText.localizationName2 = "UpgradeTicket";
                    titleText.plusText = " x5";
                }

                break;
            case ShopType.UpgradeTicket_R:
                backgroundImg.sprite = backgroundImgArray[2];

                if (moneyType == MoneyType.Gold)
                {
                    titleText.localizationName = "GradeR";
                    titleText.localizationName2 = "UpgradeTicket";
                    titleText.plusText = " x1";
                    goldText.text = "2,000";
                }
                else
                {
                    titleText.localizationName = "GradeR";
                    titleText.localizationName2 = "UpgradeTicket";
                    titleText.plusText = " x5";
                }

                break;
            case ShopType.UpgradeTicket_SR:
                backgroundImg.sprite = backgroundImgArray[3];

                if (moneyType == MoneyType.Gold)
                {
                    titleText.localizationName = "GradeSR";
                    titleText.localizationName2 = "UpgradeTicket";
                    titleText.plusText = " x1";
                    goldText.text = "3,000";
                }
                else
                {
                    titleText.localizationName = "GradeSR";
                    titleText.localizationName2 = "UpgradeTicket";
                    titleText.plusText = " x5";
                }

                break;
            case ShopType.UpgradeTicket_SSR:
                backgroundImg.sprite = backgroundImgArray[4];

                if (moneyType == MoneyType.Gold)
                {
                    titleText.localizationName = "GradeSSR";
                    titleText.localizationName2 = "UpgradeTicket";
                    titleText.plusText = " x1";
                    goldText.text = "4,000";
                }
                else
                {
                    titleText.localizationName = "GradeSSR";
                    titleText.localizationName2 = "UpgradeTicket";
                    titleText.plusText = " x5";
                }

                break;
            case ShopType.UpgradeTicket_UR:
                backgroundImg.sprite = backgroundImgArray[5];

                if (moneyType == MoneyType.Gold)
                {
                    titleText.localizationName = "GradeUR";
                    titleText.localizationName2 = "UpgradeTicket";
                    titleText.plusText = " x1";
                    goldText.text = "5,000";
                }
                else
                {
                    titleText.localizationName = "GradeUR";
                    titleText.localizationName2 = "UpgradeTicket";
                    titleText.plusText = " x5";
                }

                break;
            case ShopType.DefDestroyTicket:
                backgroundImg.sprite = backgroundImgArray[0];

                if (moneyType == MoneyType.Gold)
                {
                    titleText.localizationName = "DefDestroyTicket";
                    titleText.plusText = " x1";
                    goldText.text = "6,000";
                }
                else
                {
                    titleText.localizationName = "DefDestroyTicket";
                    titleText.plusText = " x5";
                }
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
        shopManager.BuyItem(shopType,moneyType);
    }
}
