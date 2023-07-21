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

    public Text titleText;

    public GameObject adButton;

    public GameObject buyButton;
    public Text priceText;

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
                    titleText.text = LocalizationManager.instance.GetString("GradeN") + " " + LocalizationManager.instance.GetString("UpgradeTicket") + " x1";
                    goldText.text = "1000";
                }
                else
                {
                    titleText.text = LocalizationManager.instance.GetString("GradeN") + " " + LocalizationManager.instance.GetString("UpgradeTicket") + " x5";
                    priceText.text = LocalizationManager.instance.GetString(shopType + "_Price");
                }

                break;
            case ShopType.UpgradeTicket_R:
                backgroundImg.sprite = backgroundImgArray[2];

                if (moneyType == MoneyType.Gold)
                {
                    titleText.text = LocalizationManager.instance.GetString("GradeR") + " " + LocalizationManager.instance.GetString("UpgradeTicket") + " x1";
                    goldText.text = "2000";
                }
                else
                {
                    titleText.text = LocalizationManager.instance.GetString("GradeR") + " " + LocalizationManager.instance.GetString("UpgradeTicket") + " x5";
                    priceText.text = LocalizationManager.instance.GetString(shopType + "_Price");
                }

                break;
            case ShopType.UpgradeTicket_SR:
                backgroundImg.sprite = backgroundImgArray[3];

                if (moneyType == MoneyType.Gold)
                {
                    titleText.text = LocalizationManager.instance.GetString("GradeSR") + " " + LocalizationManager.instance.GetString("UpgradeTicket") + " x1";
                    goldText.text = "3000";
                }
                else
                {
                    titleText.text = LocalizationManager.instance.GetString("GradeSR") + " " + LocalizationManager.instance.GetString("UpgradeTicket") + " x5";
                    priceText.text = LocalizationManager.instance.GetString(shopType + "_Price");
                }

                break;
            case ShopType.UpgradeTicket_SSR:
                backgroundImg.sprite = backgroundImgArray[4];

                if (moneyType == MoneyType.Gold)
                {
                    titleText.text = LocalizationManager.instance.GetString("GradeSSR") + " " + LocalizationManager.instance.GetString("UpgradeTicket") + " x1";
                    goldText.text = "4000";
                }
                else
                {
                    titleText.text = LocalizationManager.instance.GetString("GradeSSR") + " " + LocalizationManager.instance.GetString("UpgradeTicket") + " x5";
                    priceText.text = LocalizationManager.instance.GetString(shopType + "_Price");
                }

                break;
            case ShopType.UpgradeTicket_UR:
                backgroundImg.sprite = backgroundImgArray[5];

                if (moneyType == MoneyType.Gold)
                {
                    titleText.text = LocalizationManager.instance.GetString("GradeUR") + " " + LocalizationManager.instance.GetString("UpgradeTicket") + " x1";
                    goldText.text = "5000";
                }
                else
                {
                    titleText.text = LocalizationManager.instance.GetString("GradeUR") + " " + LocalizationManager.instance.GetString("UpgradeTicket") + " x5";
                    priceText.text = LocalizationManager.instance.GetString(shopType + "_Price");
                }

                break;
            case ShopType.DefDestroyTicket:
                backgroundImg.sprite = backgroundImgArray[0];

                if (moneyType == MoneyType.Gold)
                {
                    titleText.text = LocalizationManager.instance.GetString("DefDestroyTicket") + " x1";
                    goldText.text = "6000";
                }
                else
                {
                    titleText.text = LocalizationManager.instance.GetString("DefDestroyTicket") + " x5";
                    priceText.text = LocalizationManager.instance.GetString(shopType + "_Price");
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
