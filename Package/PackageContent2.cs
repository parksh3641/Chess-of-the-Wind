using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PackageContent2 : MonoBehaviour
{
    public PackageType packageType = PackageType.Default;

    public LocalizationContent titleText;

    public CodelessIAPButton iapButton;
    public LocalizationContent iapPriceText;

    public Text valueText;

    public ReceiveContent[] receiveContents;

    public GameObject lockedObj;

    PackageInfomation packageInfomation;

    PackageManager2 packageManager2;

    PackageDataBase packageDataBase;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (packageDataBase == null) packageDataBase = Resources.Load("PackageDataBase") as PackageDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void Initialize(PackageType type, PackageManager2 manager)
    {
        packageType = type;
        packageManager2 = manager;

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

        iapPriceText.localizationName = "PackagePrice_" + packageType.ToString();
        iapPriceText.ReLoad();

        lockedObj.SetActive(true);

        valueText.text = "400%";

        switch (type)
        {
            case PackageType.Daily1:
                titleText.localizationName = "Daily";
                titleText.plusText = " 1";

                iapButton.productId = "shop.windchess.packagedaily1";

                if (playerDataBase.ResetInfo.package_Daily1 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Daily2:
                titleText.localizationName = "Daily";
                titleText.plusText = " 2";

                iapButton.productId = "shop.windchess.packagedaily2";

                if (playerDataBase.ResetInfo.package_Daily2 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Daily3:
                titleText.localizationName = "Daily";
                titleText.plusText = " 3";

                iapButton.productId = "shop.windchess.packagedaily3";

                if (playerDataBase.ResetInfo.package_Daily3 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Daily4:
                titleText.localizationName = "Daily";
                titleText.plusText = " 4";

                iapButton.productId = "shop.windchess.packagedaily4";

                if (playerDataBase.ResetInfo.package_Daily4 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Daily5:
                titleText.localizationName = "Daily";
                titleText.plusText = " 5";

                iapButton.productId = "shop.windchess.packagedaily5";

                if (playerDataBase.ResetInfo.package_Daily5 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Weekly1:
                titleText.localizationName = "Weekly";
                titleText.plusText = " 1";

                iapButton.productId = "shop.windchess.packageweekly1";

                if (playerDataBase.ResetInfo.package_Weekly1 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Weekly2:
                titleText.localizationName = "Weekly";
                titleText.plusText = " 2";

                iapButton.productId = "shop.windchess.packageweekly2";

                if (playerDataBase.ResetInfo.package_Weekly2 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Weekly3:
                titleText.localizationName = "Weekly";
                titleText.plusText = " 3";

                iapButton.productId = "shop.windchess.packageweekly3";

                if (playerDataBase.ResetInfo.package_Weekly3 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Weekly4:
                titleText.localizationName = "Weekly";
                titleText.plusText = " 4";

                iapButton.productId = "shop.windchess.packageweekly4";

                if (playerDataBase.ResetInfo.package_Weekly4 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Weekly5:
                titleText.localizationName = "Weekly";
                titleText.plusText = " 5";

                iapButton.productId = "shop.windchess.packageweekly5";

                if (playerDataBase.ResetInfo.package_Weekly5 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Monthly1:
                titleText.localizationName = "Monthly";
                titleText.plusText = " 1";

                iapButton.productId = "shop.windchess.packagemonthly1";

                if (playerDataBase.ResetInfo.package_Monthly1 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Monthly2:
                titleText.localizationName = "Monthly";
                titleText.plusText = " 2";

                iapButton.productId = "shop.windchess.packagemonthly2";

                if (playerDataBase.ResetInfo.package_Monthly2 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Monthly3:
                titleText.localizationName = "Monthly";
                titleText.plusText = " 3";

                iapButton.productId = "shop.windchess.packagemonthly3";

                if (playerDataBase.ResetInfo.package_Monthly3 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Monthly4:
                titleText.localizationName = "Monthly";
                titleText.plusText = " 4";

                iapButton.productId = "shop.windchess.packagemonthly4";

                if (playerDataBase.ResetInfo.package_Monthly4 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Monthly5:
                titleText.localizationName = "Monthly";
                titleText.plusText = " 5";

                iapButton.productId = "shop.windchess.packagemonthly5";

                if (playerDataBase.ResetInfo.package_Monthly5 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
        }

        titleText.ReLoad();
    }

    public void BuyPurchase()
    {
        packageManager2.BuyPurchase(packageInfomation);
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
