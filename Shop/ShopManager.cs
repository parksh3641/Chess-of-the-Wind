using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    public GameObject shopView;

    public RectTransform shopRectTransform;

    public ContentSizeFitter contentSizeFitter;

    public GameObject[] boxArray;
    public LocalizationContent[] boxSSRTextArray;

    public ShopContent shopContent;

    public Text[] dailyCountText;
    public Text dailyShopCountText;
    public ShopContent[] dailyContentArray;

    public Transform shopContentGoldTransform;
    public Transform shopContentTransform;

    [Space]
    [Title("Ad")]
    public GameObject[] boxShopLockArray;
    public ReceiveContent[] adShopReceiveContents;
    public GameObject[] adShopLockArray;
    public GameObject[] adShopClearArray;
    public GameObject goldShopLockArray;

    public Text watchAdCountText_BoxNR;
    public Text watchAdCountText_BoxRSR;

    private int adCoolTime_BoxNR = 0;
    private int adCoolTime_BoxRSR = 0;

    TimeSpan timeSpan;
    private int hours;
    private int minutes;
    private int seconds;

    List<ShopContent> shopContentGoldList = new List<ShopContent>();
    List<ShopContent> shopContentList = new List<ShopContent>();

    string localization_Reset = "";
    string localization_Days = "";
    string localization_Hours = "";
    string localization_Minutes = "";

    bool isDelay = false;
    bool first = false;
    bool isTimer = false;

    int random = 0;

    public UIManager uIManager;
    public PackageManager packageManager;

    PlayerDataBase playerDataBase;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        shopView.SetActive(false);

        //for (int i = 0; i < 6; i++)
        //{
        //    ShopContent monster = Instantiate(shopContent);
        //    monster.transform.parent = shopContentGoldTransform;
        //    monster.transform.position = Vector3.zero;
        //    monster.transform.rotation = Quaternion.identity;
        //    monster.transform.localScale = Vector3.one;
        //    monster.Initialize(ShopType.UpgradeTicket_N + i, MoneyType.Gold, this);
        //    monster.gameObject.SetActive(true);

        //    if (i == 4) monster.gameObject.SetActive(false);

        //    shopContentGoldList.Add(monster);
        //}

        //shopRectTransform.sizeDelta = new Vector2(0, -999);
    }

    private void Start()
    {
        isTimer = true;
        StartCoroutine(DailyShopTimer());
    }

    public void OpenShopView()
    {
        if (!shopView.activeSelf)
        {
            shopView.SetActive(true);

            if (playerDataBase.AttendanceDay == DateTime.Today.ToString("yyyyMMdd"))
            {
                ResetManager.instance.Initialize();
            }

            if(!isTimer)
            {
                isTimer = true;
                StartCoroutine(DailyShopTimer());
            }

            if (!first)
            {
                adShopReceiveContents[0].Initialize(RewardType.Gold, 10000);
                adShopReceiveContents[1].Initialize(RewardType.UpgradeTicket, 1);
                adShopReceiveContents[2].Initialize(RewardType.UpgradeTicket, 10);

                first = true;
            }

            Initialize_Ad();

            packageManager.OpenShop();

            boxArray[0].SetActive(false);
            boxArray[1].SetActive(false);

            localization_Reset = LocalizationManager.instance.GetString("Reset");
            localization_Days = LocalizationManager.instance.GetString("Days");
            localization_Hours = LocalizationManager.instance.GetString("Hours");
            localization_Minutes = LocalizationManager.instance.GetString("Minutes");

            shopRectTransform.offsetMax = Vector3.zero;

            if (playerDataBase.Formation == 2)
            {
                boxArray[1].SetActive(true);

                boxSSRTextArray[1].forwardText = (100 - playerDataBase.BuyUnderworldBoxSSRCount).ToString();
                boxSSRTextArray[1].localizationName = "BoxSSRInfo";
                boxSSRTextArray[1].ReLoad();
            }
            else
            {
                boxArray[0].SetActive(true);

                boxSSRTextArray[0].forwardText = (100 - playerDataBase.BuySnowBoxSSRCount).ToString();
                boxSSRTextArray[0].localizationName = "BoxSSRInfo";
                boxSSRTextArray[0].ReLoad();
            }

            Initialize_Count();
        }
    }

    public void Change()
    {
        if (shopView.activeSelf)
        {
            if (playerDataBase.Formation == 2)
            {
                boxArray[1].SetActive(true);

                boxSSRTextArray[1].forwardText = (100 - playerDataBase.BuyUnderworldBoxSSRCount).ToString();
                boxSSRTextArray[1].localizationName = "BoxSSRInfo";
                boxSSRTextArray[1].ReLoad();
            }
            else
            {
                boxArray[0].SetActive(true);

                boxSSRTextArray[0].forwardText = (100 - playerDataBase.BuySnowBoxSSRCount).ToString();
                boxSSRTextArray[0].localizationName = "BoxSSRInfo";
                boxSSRTextArray[0].ReLoad();
            }
        }
    }

    void Initialize_Count()
    {
        dailyCountText[0].text = GameStateManager.instance.DailyNormalBox_1 + "/3";
        dailyCountText[1].text = GameStateManager.instance.DailyNormalBox_10 + "/1";
        dailyCountText[2].text = GameStateManager.instance.DailyEpicBox_1 + "/3";
        dailyCountText[3].text = GameStateManager.instance.DailyEpicBox_10 + "/1";
    }

    public void CloseShopView()
    {
        shopView.SetActive(false);
    }

    #region RandomBox
    public void BuySnowBox1()
    {
        GetSnowBox(BoxType.Random, 1);

        playerDataBase.BuySnowBox += 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuySnowBox", playerDataBase.BuySnowBox);
    }

    public void BuySnowBox2()
    {
        GetSnowBox(BoxType.Random, 10);

        playerDataBase.BuySnowBox += 10;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuySnowBox", playerDataBase.BuySnowBox);
    }

    public void GetSnowBox(BoxType type, int number)
    {
        switch (type)
        {
            case BoxType.Random:
                playerDataBase.SnowBox = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox", number);
                break;
            case BoxType.N:
                playerDataBase.SnowBox_N = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_N", number);
                break;
            case BoxType.R:
                playerDataBase.SnowBox_R = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_R", number);
                break;
            case BoxType.SR:
                playerDataBase.SnowBox_SR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SR", number);
                break;
            case BoxType.SSR:
                playerDataBase.SnowBox_SSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SSR", number);
                break;
            case BoxType.UR:
                playerDataBase.SnowBox_UR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_UR", number);
                break;
            case BoxType.NR:
                playerDataBase.SnowBox_NR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_NR", number);
                break;
            case BoxType.RSR:
                playerDataBase.SnowBox_RSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_RSR", number);
                break;
            case BoxType.SRSSR:
                playerDataBase.SnowBox_SRSSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SRSSR", number);
                break;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        NotionManager.instance.UseNotion(NotionType.BuyShopItem);
    }

    public void BuyUnderworldBox1()
    {
        GetUnderworld(BoxType.Random, 1);

        playerDataBase.BuyUnderworldBox += 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuyUnderworldBox", playerDataBase.BuyUnderworldBox);
    }

    public void BuyUnderworldBox2()
    {
        GetUnderworld(BoxType.Random, 10);

        playerDataBase.BuyUnderworldBox += 10;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuyUnderworldBox", playerDataBase.BuyUnderworldBox);
    }

    public void GetUnderworld(BoxType type, int number)
    {
        switch (type)
        {
            case BoxType.Random:
                playerDataBase.UnderworldBox = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", number);
                break;
            case BoxType.N:
                playerDataBase.UnderworldBox_N = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_N", number);
                break;
            case BoxType.R:
                playerDataBase.UnderworldBox_R = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_R", number);
                break;
            case BoxType.SR:
                playerDataBase.UnderworldBox_SR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SR", number);
                break;
            case BoxType.SSR:
                playerDataBase.UnderworldBox_SSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SSR", number);
                break;
            case BoxType.UR:
                playerDataBase.UnderworldBox_UR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_UR", number);
                break;
            case BoxType.NR:
                playerDataBase.UnderworldBox_NR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_NR", number);
                break;
            case BoxType.RSR:
                playerDataBase.UnderworldBox_RSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_RSR", number);
                break;
            case BoxType.SRSSR:
                playerDataBase.UnderworldBox_SRSSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SRSSR", number);
                break;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
    }

    public void Buy_NR(int number)
    {
        int price = 25000 * number;

        if (number >= 10)
        {
            if(GameStateManager.instance.DailyNormalBox_10 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                
                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }

            price -= 25000;
        }
        else
        {
            if (GameStateManager.instance.DailyNormalBox_1 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }
        }

        if (playerDataBase.Gold >= price)
        {
            PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, price);

            switch (GameStateManager.instance.WindCharacterType)
            {
                case WindCharacterType.Winter:
                    GetSnowBox(BoxType.NR, number);
                    break;
                case WindCharacterType.UnderWorld:
                    GetUnderworld(BoxType.NR, number);
                    break;
            }

            if(number >= 10)
            {
                GameStateManager.instance.DailyNormalBox_10 -= 1;
            }
            else
            {
                GameStateManager.instance.DailyNormalBox_1 -= 1;
            }

            Initialize_Count();

            SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
            NotionManager.instance.UseNotion(NotionType.BuyShopItem);

            FirebaseAnalytics.LogEvent("BuyBox_NR");
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
        }
    }

    public void Buy_RSR(int number)
    {
        int price = 100000 * number;

        if (number >= 10)
        {
            if (GameStateManager.instance.DailyEpicBox_10 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }

            price -= 100000;
        }
        else
        {
            if (GameStateManager.instance.DailyEpicBox_1 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }
        }

        if (playerDataBase.Gold >= price)
        {
            PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, price);

            switch (GameStateManager.instance.WindCharacterType)
            {
                case WindCharacterType.Winter:
                    GetSnowBox(BoxType.RSR, number);
                    break;
                case WindCharacterType.UnderWorld:
                    GetUnderworld(BoxType.RSR, number);
                    break;
            }

            if (number >= 10)
            {
                GameStateManager.instance.DailyEpicBox_10 -= 1;
            }
            else
            {
                GameStateManager.instance.DailyEpicBox_1 -= 1;
            }

            Initialize_Count();

            SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
            NotionManager.instance.UseNotion(NotionType.BuyShopItem);

            FirebaseAnalytics.LogEvent("BuyBox_RSR");
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
        }
    }

    public void BuyItem(ShopType type, int price, int number)
    {
        if (isDelay) return;

        switch (type)
        {
            case ShopType.DailyReward:
                if(!GameStateManager.instance.DailyReward)
                {
                    GameStateManager.instance.DailyReward = true;

                    dailyContentArray[0].Locked();

                    PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, price);

                    FirebaseAnalytics.LogEvent("DailyReward");
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                    NotionManager.instance.UseNotion(NotionType.NotRewardDailyLimit);

                    return;
                }

                break;
            case ShopType.DailyReward_WatchAd:

                break;
            case ShopType.UpgradeTicket:
                if (playerDataBase.Gold >= price)
                {
                    PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, price);

                    FirebaseAnalytics.LogEvent("BuyUpgradeTicket");
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                    NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                    return;
                }

                playerDataBase.SetUpgradeTicket(RankType.N, number);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));

                SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

                NotionManager.instance.UseNotion(NotionType.GetUpgradeTicket);

                if(number < 11)
                {
                    GameStateManager.instance.DailyBuy1 = true;

                    dailyContentArray[1].Locked();
                }
                else
                {
                    GameStateManager.instance.DailyBuy2 = true;

                    dailyContentArray[2].Locked();
                }

                break;
        }

        uIManager.Renewal();

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        isDelay = true;
        Invoke("Delay", 0.5f);
    }

    void Delay()
    {
        isDelay = false;
    }

    public void BuyPurchase(int number)
    {
        switch(number)
        {
            case 0:
                playerDataBase.SetUpgradeTicket(RankType.N, 1);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));

                NotionManager.instance.UseNotion(NotionType.GetUpgradeTicket);
                break;
            case 1:
                playerDataBase.SetUpgradeTicket(RankType.N, 10);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));


                NotionManager.instance.UseNotion(NotionType.GetUpgradeTicket);
                break;
            case 2:
                playerDataBase.SetUpgradeTicket(RankType.N, 100);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));

                NotionManager.instance.UseNotion(NotionType.GetUpgradeTicket);
                break;
            case 3:
                random = Random.Range(5000, 100001);

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, random);

                SoundManager.instance.PlaySFX(GameSfxType.PlusMoney1);

                NotionManager.instance.UseNotion(NotionType.BuyShopItem);
                break;
            case 4:
                random = Random.Range(50000, 1000001);

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, random);

                SoundManager.instance.PlaySFX(GameSfxType.PlusMoney2);

                NotionManager.instance.UseNotion(NotionType.BuyShopItem);
                break;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
    }

    IEnumerator DailyShopTimer()
    {
        if (dailyShopCountText.gameObject.activeInHierarchy)
        {
            System.DateTime f = System.DateTime.Now;
            System.DateTime g = System.DateTime.Today.AddDays(1);
            System.TimeSpan h = g - f;

            localization_Reset = LocalizationManager.instance.GetString("Reset");
            localization_Days = LocalizationManager.instance.GetString("Days");
            localization_Hours = LocalizationManager.instance.GetString("Hours");
            localization_Minutes = LocalizationManager.instance.GetString("Minutes");

            dailyShopCountText.text = localization_Reset + " : " + h.Hours.ToString("D2") + localization_Hours + " " + h.Minutes.ToString("D2") + localization_Minutes;

            if (playerDataBase.AttendanceDay == DateTime.Today.ToString("yyyyMMdd"))
            {
                isTimer = false;
                ResetManager.instance.Initialize();
                yield break;
            }
        }

        yield return waitForSeconds;
        StartCoroutine(DailyShopTimer());
    }

    public void Failed()
    {
        SoundManager.instance.PlaySFX(GameSfxType.Wrong);

        NotionManager.instance.UseNotion(NotionType.CancelPurchase);
    }


    public void Initialize_Ad()
    {
        boxShopLockArray[0].SetActive(true);
        boxShopLockArray[1].SetActive(true);

        adShopClearArray[0].SetActive(true);
        adShopClearArray[1].SetActive(true);
        adShopClearArray[2].SetActive(true);

        goldShopLockArray.SetActive(true);

        if (GameStateManager.instance.DailyNormalBox)
        {
            LoadAd_BoxNR();
        }
        else
        {
            SetWatchAd_BoxNR(false);
        }

        if (GameStateManager.instance.DailyEpicBox)
        {
            LoadAd_BoxRSR();
        }
        else
        {
            SetWatchAd_BoxRSR(false);
        }

        if (!GameStateManager.instance.DailyAdsReward)
        {
            adShopClearArray[0].SetActive(false);
        }

        if (!GameStateManager.instance.DailyAdsReward2)
        {
            adShopClearArray[1].SetActive(false);

            if (GameStateManager.instance.GameRankType > GameRankType.Sliver_2)
            {
                adShopLockArray[0].SetActive(false);
            }
            else
            {
                adShopLockArray[0].SetActive(true);
            }
        }

        if (!GameStateManager.instance.DailyAdsReward3)
        {
            adShopClearArray[2].SetActive(false);

            if (GameStateManager.instance.GameRankType > GameRankType.Platinum_1)
            {
                adShopLockArray[1].SetActive(false);
            }
            else
            {
                adShopLockArray[1].SetActive(true);
            }
        }

        if (!GameStateManager.instance.DailyGoldReward)
        {
            goldShopLockArray.SetActive(false);
        }
    }

    public void GetAdReward(int number)
    {
        switch(number)
        {
            case 0:
                if (GameStateManager.instance.DailyNormalBox) return;

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_NR = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_NR", 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_NR = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_NR", 1);
                        break;
                }

                SetWatchAd_BoxNR(true);
                break;
            case 1:
                if (GameStateManager.instance.DailyEpicBox) return;

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_RSR = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_RSR", 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_RSR = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_RSR", 1);
                        break;
                }

                SetWatchAd_BoxRSR(true);
                break;
            case 2:
                if (GameStateManager.instance.DailyAdsReward) return;

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 10000);

                GameStateManager.instance.DailyAdsReward = true;

                Initialize_Ad();
                break;
            case 3:
                if (GameStateManager.instance.DailyAdsReward2) return;

                playerDataBase.SetUpgradeTicket(RankType.N, 1);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));

                GameStateManager.instance.DailyAdsReward2 = true;

                Initialize_Ad();
                break;
            case 4:
                if (GameStateManager.instance.DailyAdsReward3) return;

                playerDataBase.SetUpgradeTicket(RankType.N, 10);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));

                GameStateManager.instance.DailyAdsReward3 = true;

                Initialize_Ad();
                break;
            case 5:
                if (GameStateManager.instance.DailyGoldReward) return;

                random = Random.Range(75000, 20001);

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, random);

                GameStateManager.instance.DailyGoldReward = true;

                Initialize_Ad();
                break;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        NotionManager.instance.UseNotion(NotionType.GetWatchAdReward);
    }

    #endregion

    #region AdCoolTime

    public void LoadAd_BoxNR()
    {
        DateTime time = DateTime.Parse(PlayerPrefs.GetString("AdCoolTime_BoxNR"));
        DateTime now = DateTime.Now;

        TimeSpan span = time - now;

        if (span.TotalSeconds > 0)
        {
            if (adCoolTime_BoxNR > 0) return;

            adCoolTime_BoxNR = (int)span.TotalSeconds;

            boxShopLockArray[0].SetActive(true);

            StartCoroutine(WatchAdCorution_BoxNR());
        }
        else
        {
            SetWatchAd_BoxNR(false);
        }
    }

    public void SetWatchAd_BoxNR(bool check)
    {
        if (check)
        {
            Debug.Log("Watch Ad BoxNR Play");

            boxShopLockArray[0].SetActive(true);
            GameStateManager.instance.DailyNormalBox = true;

            PlayerPrefs.SetString("AdCoolTime_BoxNR", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));

            adCoolTime_BoxNR = 86400;

            StartCoroutine(WatchAdCorution_BoxNR());
        }
        else
        {
            Debug.Log("Watch Ad BoxNR Stop");

            boxShopLockArray[0].SetActive(false);
            GameStateManager.instance.DailyNormalBox = false;

            adCoolTime_BoxNR = 0;
        }
    }

    IEnumerator WatchAdCorution_BoxNR()
    {
        if (adCoolTime_BoxNR > 0)
        {
            adCoolTime_BoxNR -= 1;
        }
        else
        {
            SetWatchAd_BoxNR(false);
            yield break;
        }

        timeSpan = TimeSpan.FromSeconds(adCoolTime_BoxNR);
        watchAdCountText_BoxNR.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        yield return waitForSeconds;
        StartCoroutine(WatchAdCorution_BoxNR());
    }

    public void LoadAd_BoxRSR()
    {
        DateTime time = DateTime.Parse(PlayerPrefs.GetString("AdCoolTime_BoxRSR"));
        DateTime now = DateTime.Now;

        TimeSpan span = time - now;

        if (span.TotalSeconds > 0)
        {
            if (adCoolTime_BoxRSR > 0) return;

            adCoolTime_BoxRSR = (int)span.TotalSeconds;

            boxShopLockArray[1].SetActive(true);

            StartCoroutine(WatchAdCorution_BoxRSR());
        }
        else
        {
            SetWatchAd_BoxRSR(false);
        }
    }

    public void SetWatchAd_BoxRSR(bool check)
    {
        if (check)
        {
            Debug.Log("Watch Ad BoxRSR Play");

            boxShopLockArray[1].SetActive(true);
            GameStateManager.instance.DailyEpicBox = true;

            PlayerPrefs.SetString("AdCoolTime_BoxRSR", DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss"));

            adCoolTime_BoxRSR = 86400 * 3;

            StartCoroutine(WatchAdCorution_BoxRSR());
        }
        else
        {
            Debug.Log("Watch Ad BoxRSR Stop");

            boxShopLockArray[1].SetActive(false);
            GameStateManager.instance.DailyEpicBox = false;

            adCoolTime_BoxRSR = 0;
        }
    }

    IEnumerator WatchAdCorution_BoxRSR()
    {
        if (adCoolTime_BoxRSR > 0)
        {
            adCoolTime_BoxRSR -= 1;
        }
        else
        {
            SetWatchAd_BoxRSR(false);
            yield break;
        }

        hours = adCoolTime_BoxRSR / 3600;
        minutes = (adCoolTime_BoxRSR % 3600) / 60;
        seconds = adCoolTime_BoxRSR % 60;

        watchAdCountText_BoxRSR.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

        yield return waitForSeconds;
        StartCoroutine(WatchAdCorution_BoxRSR());
    }




    #endregion

    #region Developer

    [Button]
    public void PlusMoney()
    {
        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 10000000);
    }

    [Button]
    public void PlusUpgradeTicket()
    {
        playerDataBase.SetUpgradeTicket(RankType.N, 1000);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));
    }

    [Button]
    public void OpenRandomBox(int number)
    {
        switch (GameStateManager.instance.WindCharacterType)
        {
            case WindCharacterType.Winter:
                GetSnowBox(BoxType.Random, number);
                break;
            case WindCharacterType.UnderWorld:
                GetUnderworld(BoxType.Random, number);
                break;
        }
    }

    [Button]
    public void OpenSnowBox_N()
    {
        GetSnowBox(BoxType.N, 10);
    }

    [Button]
    public void OpenSnowBox_R()
    {
        GetSnowBox(BoxType.R, 10);
    }

    [Button]
    public void OpenSnowBox_SR()
    {
        GetSnowBox(BoxType.SR, 10);
    }

    [Button]
    public void OpenSnowBox_SSR()
    {
        GetSnowBox(BoxType.SSR, 10);
    }

    [Button]
    public void OpenSnowBox_UR()
    {
        GetSnowBox(BoxType.UR, 10);
    }

    [Button]
    public void OpenSnowBox_NR()
    {
        GetSnowBox(BoxType.NR, 10);
    }

    [Button]
    public void OpenSnowBox_RSR()
    {
        GetSnowBox(BoxType.RSR, 10);
    }


    [Button]
    public void OpenSnowBox_SRSSR()
    {
        GetSnowBox(BoxType.SRSSR, 10);
    }

    [Button]
    public void OpenUnderworld_N()
    {
        GetUnderworld(BoxType.N, 10);
    }

    [Button]
    public void OpenUnderworld_R()
    {
        GetUnderworld(BoxType.R, 10);
    }

    [Button]
    public void OpenUnderworld_SR()
    {
        GetUnderworld(BoxType.SR, 10);
    }

    [Button]
    public void OpenUnderworld_SSR()
    {
        GetUnderworld(BoxType.SSR, 10);
    }

    [Button]
    public void OpenUnderworld_UR()
    {
        GetUnderworld(BoxType.UR, 10);
    }

    [Button]
    public void OpenUnderworld_NR()
    {
        GetUnderworld(BoxType.NR, 10);
    }

    [Button]
    public void OpenUnderworld_RSR()
    {
        GetUnderworld(BoxType.RSR, 10);
    }

    [Button]
    public void OpenUnderworld_SRSSR()
    {
        GetUnderworld(BoxType.SRSSR, 10);
    }

    #endregion
}
