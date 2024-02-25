using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PackageContent : MonoBehaviour
{
    public PackageType packageType = PackageType.Default;

    public LocalizationContent titleText;

    public CodelessIAPButton iapButton;
    public LocalizationContent iapPriceText;

    public ReceiveContent[] receiveContents;

    PackageInfomation packageInfomation;

    PackageManager packageManager;

    PackageDataBase packageDataBase;

    private void Awake()
    {
        if (packageDataBase == null) packageDataBase = Resources.Load("PackageDataBase") as PackageDataBase;
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
            case PackageType.NewRank:
                break;
            case PackageType.NewRank2:
                break;
            case PackageType.NewRank3:
                break;
        }
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
}
