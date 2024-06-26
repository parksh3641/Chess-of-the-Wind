using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PackageContent : MonoBehaviour
{
    public PackageType packageType = PackageType.Default;

    public LocalizationContent titleText;
    public LocalizationContent infoText;
    public GameObject valueObj;

    public CodelessIAPButton iapButton;
    public LocalizationContent iapPriceText;

    public ReceiveContent[] receiveContents;

    public GameObject lockedObj;

    PackageInfomation packageInfomation;

    PackageManager packageManager;

    PackageDataBase packageDataBase;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (packageDataBase == null) packageDataBase = Resources.Load("PackageDataBase") as PackageDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void Initialize(PackageManager manager)
    {
        packageManager = manager;

        packageInfomation = packageDataBase.GetPackageInfomation(packageType);

        for (int i = 0; i < receiveContents.Length; i++)
        {
            receiveContents[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < packageInfomation.receiveInformationList.Count; i++)
        {
            receiveContents[i].gameObject.SetActive(true);
            receiveContents[i].Initialize(packageInfomation.receiveInformationList[i].rewardType, packageInfomation.receiveInformationList[i].count);
        }

        titleText.localizationName = "Package_" + packageType.ToString();
        titleText.ReLoad();

        iapPriceText.localizationName = "PackagePrice_" + packageType.ToString();
        iapPriceText.ReLoad();

        lockedObj.SetActive(false);
        valueObj.SetActive(true);

        infoText.localizationName = "BuyOnce";

        switch (packageType)
        {
            case PackageType.Default:
                break;
            case PackageType.Newbie:
                iapButton.productId = "shop.windchess.shopnewbie";
                break;
            case PackageType.Sliver:
                iapButton.productId = "shop.windchess.shopsliver";
                break;
            case PackageType.Gold:
                iapButton.productId = "shop.windchess.shopgold";
                break;
            case PackageType.Platinum:
                iapButton.productId = "shop.windchess.shopplatinum";
                break;
            case PackageType.Diamond:
                iapButton.productId = "shop.windchess.shopdiamond";
                break;
            case PackageType.Legend:
                iapButton.productId = "shop.windchess.shoplegend";
                break;
            case PackageType.Supply:
                iapButton.productId = "shop.windchess.shopsupply";
                break;
            case PackageType.Trials:
                break;
            case PackageType.ShopDaily1:
                iapButton.productId = "shop.windchess.packageshopdaily1";

                infoText.localizationName = "BuyDailyOne";

                valueObj.SetActive(false);

                if (playerDataBase.ResetInfo.package_ShopDaily1 == 1)
                {
                    lockedObj.SetActive(true);
                }
                break;
            case PackageType.ShopDaily2:
                iapButton.productId = "shop.windchess.packageshopdaily2";

                infoText.localizationName = "BuyDailyOne";

                valueObj.SetActive(false);

                if (playerDataBase.ResetInfo.package_ShopDaily2 == 1)
                {
                    lockedObj.SetActive(true);
                }
                break;
            case PackageType.ShopDaily3:
                iapButton.productId = "shop.windchess.packageshopdaily3";

                infoText.localizationName = "BuyDailyOne";

                valueObj.SetActive(false);

                if (playerDataBase.ResetInfo.package_ShopDaily3 == 1)
                {
                    lockedObj.SetActive(true);
                }
                break;
            case PackageType.ShopWeekly1:
                iapButton.productId = "shop.windchess.packageshopweekly1";

                infoText.localizationName = "BuyWeeklyOne";

                valueObj.SetActive(false);

                if (playerDataBase.ResetInfo.package_ShopWeekly1 == 1)
                {
                    lockedObj.SetActive(true);
                }
                break;
            case PackageType.ShopWeekly2:
                iapButton.productId = "shop.windchess.packageshopweekly2";

                infoText.localizationName = "BuyWeeklyOne";

                valueObj.SetActive(false);

                if (playerDataBase.ResetInfo.package_ShopWeekly2 == 1)
                {
                    lockedObj.SetActive(true);
                }
                break;
            case PackageType.ShopWeekly3:
                iapButton.productId = "shop.windchess.packageshopweekly3";

                infoText.localizationName = "BuyWeeklyOne";

                valueObj.SetActive(false);

                if (playerDataBase.ResetInfo.package_ShopWeekly3 == 1)
                {
                    lockedObj.SetActive(true);
                }
                break;
        }

        infoText.ReLoad();
    }

    public void BuyPurchase()
    {
        packageManager.BuyPurchase(packageInfomation);
    }

    public void Failed()
    {
        SoundManager.instance.PlaySFX(GameSfxType.Wrong);

        NotionManager.instance.UseNotion(NotionType.CancelPurchase);
    }

    public void Locked()
    {
        lockedObj.SetActive(true);
    }
}
