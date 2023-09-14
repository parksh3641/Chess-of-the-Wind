using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackageContent : MonoBehaviour
{
    public PackageType packageType = PackageType.Default;

    public LocalizationContent titleText;
    public GameObject[] buyButton;

    public LocalizationContent[] buyButtonTotalText;
    public LocalizationContent[] buyButtonPriceText;

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

        for(int i = 0; i < buyButton.Length; i ++)
        {
            buyButton[i].SetActive(false);
        }

        titleText.localizationName = "Package_" + packageType.ToString();

        buyButton[(int)packageType - 1].SetActive(true);
        buyButtonTotalText[(int)packageType - 1].localizationName = "PackageTotal_" + packageType.ToString();
        buyButtonPriceText[(int)packageType - 1].localizationName = "PackagePrice_" + packageType.ToString();

        titleText.ReLoad();
        buyButtonTotalText[(int)packageType - 1].ReLoad();
        buyButtonPriceText[(int)packageType - 1].ReLoad();
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
