using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PackageContent2 : MonoBehaviour
{
    public PackageType packageType = PackageType.Default;

    public CodelessIAPButton iapButton;
    public LocalizationContent iapPriceText;

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

        switch (type)
        {
            case PackageType.Daily1:
                iapButton.productId = "shop.windchess.packagedaily1";

                if (playerDataBase.Package_Daily1 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Daily2:
                iapButton.productId = "shop.windchess.packagedaily2";

                if (playerDataBase.Package_Daily2 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Daily3:
                iapButton.productId = "shop.windchess.packagedaily3";

                if (playerDataBase.Package_Daily3 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Daily4:
                iapButton.productId = "shop.windchess.packagedaily4";

                if (playerDataBase.Package_Daily4 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Daily5:
                iapButton.productId = "shop.windchess.packagedaily5";

                if (playerDataBase.Package_Daily5 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Weekly1:
                iapButton.productId = "shop.windchess.packageweekly1";

                if (playerDataBase.Package_Weekly1 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Weekly2:
                iapButton.productId = "shop.windchess.packageweekly2";

                if (playerDataBase.Package_Weekly2 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Weekly3:
                iapButton.productId = "shop.windchess.packageweekly3";

                if (playerDataBase.Package_Weekly3 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Weekly4:
                iapButton.productId = "shop.windchess.packageweekly4";

                if (playerDataBase.Package_Weekly4 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Weekly5:
                iapButton.productId = "shop.windchess.packageweekly5";

                if (playerDataBase.Package_Weekly5 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Monthly1:
                iapButton.productId = "shop.windchess.packagemonthly1";

                if (playerDataBase.Package_Monthly1 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Monthly2:
                iapButton.productId = "shop.windchess.packagemonthly2";

                if (playerDataBase.Package_Monthly2 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Monthly3:
                iapButton.productId = "shop.windchess.packagemonthly3";

                if (playerDataBase.Package_Monthly3 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Monthly4:
                iapButton.productId = "shop.windchess.packagemonthly4";

                if (playerDataBase.Package_Monthly4 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
            case PackageType.Monthly5:
                iapButton.productId = "shop.windchess.packagemonthly5";

                if (playerDataBase.Package_Monthly5 == 0)
                {
                    lockedObj.SetActive(false);
                }
                break;
        }
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
