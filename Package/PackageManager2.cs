using Firebase.Analytics;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PackageManager2 : MonoBehaviour
{
    PackageType packageType = PackageType.Default;

    public GameObject packageView;

    public GameObject alarm;

    [Title("Timer")]
    public Text timerText;

    [Title("TopMenu")]
    public Image[] topMenuImgArray;
    public Sprite[] topMenuSpriteArray;
    public GameObject[] scrollView;
    public RectTransform[] grid;

    public PackageContent2[] dailyPackageContent;
    public PackageContent2[] weeklyPackageContent;
    public PackageContent2[] monthlyPackageContent;

    private int hours;
    private int minutes;
    private int seconds;

    string localization_Reset = "";
    string localization_Days = "";
    string localization_Hours = "";
    string localization_Minutes = "";

    private int topNumber = 0;
    private int count = 0;

    private int gold = 0;

    DateTime f, g, i, j, l, m;
    TimeSpan h, k, n;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        packageView.SetActive(false);
        alarm.SetActive(true);

        for(int i = 0; i < grid.Length; i ++)
        {
            grid[i].anchoredPosition = new Vector2(0, -9999);
        }

        topNumber = -1;
    }

    public void ChangeTopMenu(int number)
    {
        if (topNumber != number)
        {
            topNumber = number;

            localization_Reset = LocalizationManager.instance.GetString("Reset");
            localization_Days = LocalizationManager.instance.GetString("Days");
            localization_Hours = LocalizationManager.instance.GetString("Hours");
            localization_Minutes = LocalizationManager.instance.GetString("Minutes");

            j = DateTime.ParseExact(playerDataBase.nextMonday, "yyyyMMdd", null);
            m = DateTime.ParseExact(playerDataBase.nextMonth, "yyyyMMdd", null);

            StartCoroutine(TimerCoroution());

            for (int i = 0; i < topMenuImgArray.Length; i++)
            {
                topMenuImgArray[i].sprite = topMenuSpriteArray[0];
                scrollView[i].SetActive(false);
            }
            topMenuImgArray[number].sprite = topMenuSpriteArray[1];
            scrollView[number].SetActive(true);

            switch(number)
            {
                case 0:
                    for(int i = 0; i < dailyPackageContent.Length; i ++)
                    {
                        dailyPackageContent[i].Initialize(PackageType.Daily1 + i, this);
                    }
                    break;
                case 1:
                    for (int i = 0; i < weeklyPackageContent.Length; i++)
                    {
                        weeklyPackageContent[i].Initialize(PackageType.Weekly1 + i, this);
                    }
                    break;
                case 2:
                    for (int i = 0; i < monthlyPackageContent.Length; i++)
                    {
                        monthlyPackageContent[i].Initialize(PackageType.Monthly1 + i, this);
                    }
                    break;
            }
        }
    }

    public void OpenPackageView()
    {
        if (!packageView.activeSelf)
        {
            packageView.SetActive(true);

            alarm.SetActive(false);

            if (playerDataBase.AttendanceDay == DateTime.Today.ToString("yyyyMMdd"))
            {
                ResetManager.instance.Initialize();
            }

            if (topNumber == -1)
            {
                ChangeTopMenu(0);
            }
        }
        else
        {
            StopAllCoroutines();

            packageView.SetActive(false);
        }
    }

    public void BuyPurchase(PackageInfomation type)
    {
        Debug.LogError(type.packageType + " ±¸¸Å");

        FirebaseAnalytics.LogEvent("Buy_Package_" + type.ToString());

        packageType = type.packageType;

        switch (packageType)
        {
            case PackageType.Daily1:
                ResetManager.instance.SetResetInfo(ResetType.Package_Daily1);

                dailyPackageContent[0].Locked();
                break;
            case PackageType.Daily2:
                ResetManager.instance.SetResetInfo(ResetType.Package_Daily2);

                dailyPackageContent[1].Locked();
                break;
            case PackageType.Daily3:
                ResetManager.instance.SetResetInfo(ResetType.Package_Daily3);

                dailyPackageContent[2].Locked();
                break;
            case PackageType.Daily4:
                ResetManager.instance.SetResetInfo(ResetType.Package_Daily4);

                dailyPackageContent[3].Locked();
                break;
            case PackageType.Daily5:
                ResetManager.instance.SetResetInfo(ResetType.Package_Daily5);

                dailyPackageContent[4].Locked();
                break;
            case PackageType.Weekly1:
                ResetManager.instance.SetResetInfo(ResetType.Package_Weekly1);

                weeklyPackageContent[0].Locked();
                break;
            case PackageType.Weekly2:
                ResetManager.instance.SetResetInfo(ResetType.Package_Weekly2);

                weeklyPackageContent[1].Locked();
                break;
            case PackageType.Weekly3:
                ResetManager.instance.SetResetInfo(ResetType.Package_Weekly3);

                weeklyPackageContent[2].Locked();
                break;
            case PackageType.Weekly4:
                ResetManager.instance.SetResetInfo(ResetType.Package_Weekly4);

                weeklyPackageContent[3].Locked();
                break;
            case PackageType.Weekly5:
                ResetManager.instance.SetResetInfo(ResetType.Package_Weekly5);

                weeklyPackageContent[4].Locked();
                break;
            case PackageType.Monthly1:
                ResetManager.instance.SetResetInfo(ResetType.Package_Monthly1);

                monthlyPackageContent[0].Locked();
                break;
            case PackageType.Monthly2:
                ResetManager.instance.SetResetInfo(ResetType.Package_Monthly2);

                monthlyPackageContent[1].Locked();
                break;
            case PackageType.Monthly3:
                ResetManager.instance.SetResetInfo(ResetType.Package_Monthly3);

                monthlyPackageContent[2].Locked();
                break;
            case PackageType.Monthly4:
                ResetManager.instance.SetResetInfo(ResetType.Package_Monthly4);

                monthlyPackageContent[3].Locked();
                break;
            case PackageType.Monthly5:
                ResetManager.instance.SetResetInfo(ResetType.Package_Monthly5);

                monthlyPackageContent[4].Locked();
                break;
        }

        gold = 0;

        for (int i = 0; i < type.receiveInformationList.Count; i++)
        {
            count = type.receiveInformationList[i].count;

            switch (type.receiveInformationList[i].rewardType)
            {
                case RewardType.Gold:
                    PlayfabManager.instance.UpdateAddGold(count);
                    break;
                case RewardType.UpgradeTicket:
                    ItemAnimManager.instance.GetUpgradeTicket(count);
                    break;
                case RewardType.Box:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_Normal = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox", count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_Normal = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", count);
                            break;
                    }
                    break;
                case RewardType.Box_N:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_N = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_N", count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_N = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_N", count);
                            break;
                    }
                    break;
                case RewardType.Box_R:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_R = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_R", count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_R = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_R", count);
                            break;
                    }
                    break;
                case RewardType.Box_SR:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_SR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SR", count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_SR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SR", count);
                            break;
                    }
                    break;
                case RewardType.Box_SSR:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_SSR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SSR", count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_SSR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SSR", count);
                            break;
                    }
                    break;
                case RewardType.Box_UR:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_UR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_UR", count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_UR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_UR", count);
                            break;
                    }
                    break;
                case RewardType.Box_NR:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_NR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_NR", count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_NR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_NR", count);
                            break;
                    }
                    break;
                case RewardType.Box_RSR:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_RSR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_RSR", count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_RSR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_RSR", count);
                            break;
                    }
                    break;
                case RewardType.Box_SRSSR:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_SRSSR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SRSSR", count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_SRSSR = count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SRSSR", count);
                            break;
                    }
                    break;
                case RewardType.ExclusiveTitle:
                    break;
                case RewardType.None:
                    break;
                case RewardType.GoldShop1:
                    gold += Random.Range(7500, 20001);

                    break;
                case RewardType.GoldShop2:
                    gold += Random.Range(5000, 100001);

                    break;
                case RewardType.GoldShop3:
                    gold += Random.Range(50000, 1000001);

                    break;
            }
        }

        if(gold > 0)
        {
            PlayfabManager.instance.UpdateAddGold(gold);

            gold = 0;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        NotionManager.instance.UseNotion(NotionType.BuyShopItem);
    }

    void ReStartTimer()
    {
        StartCoroutine(TimerCoroution());
    }

    IEnumerator TimerCoroution()
    {
        switch(topNumber)
        {
            case 0:
                f = System.DateTime.Now;
                g = System.DateTime.Today.AddDays(1);
                h = g - f;

                timerText.text = localization_Reset + " : " + h.Hours.ToString("D2") + localization_Hours + " " + h.Minutes.ToString("D2") + localization_Minutes;

                if (playerDataBase.AttendanceDay == DateTime.Today.ToString("yyyyMMdd"))
                {
                    ResetManager.instance.Initialize();

                    Invoke("RestartTimer", 2.0f);
                    yield break;
                }

                break;
            case 1:
                i = System.DateTime.Now;
                k = j - i;

                timerText.text = localization_Reset + " : " + k.Days.ToString("D2") + localization_Days + " " + k.Hours.ToString("D2") + localization_Hours;

                break;
            case 2:
                l = System.DateTime.Now;
                n = m - l;

                timerText.text = localization_Reset + " : " + n.Days.ToString("D2") + localization_Days + " " + n.Hours.ToString("D2") + localization_Hours;

                break;
        }

        yield return waitForSeconds;
        StartCoroutine(TimerCoroution());
    }
}
