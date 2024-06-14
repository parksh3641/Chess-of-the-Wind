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

    public GameObject alarm;

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
    [Title("OneStore")]
    public GameObject[] oneStore;

    [Space]
    [Title("Ad")]
    public GameObject[] boxShopLockArray;
    public ReceiveContent[] adShopReceiveContents;
    public GameObject[] adShopLockArray;
    public GameObject[] adShopClearArray;
    public GameObject goldShopLockArray;


    public GameObject adResetView;
    public GameObject adReset;

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

    int random = 0;

    public UIManager uIManager;
    public PackageManager packageManager;

    DateTime f, g, i, j, l, m;
    TimeSpan h, k, n;

    PlayerDataBase playerDataBase;
    RankDataBase rankDataBase;

    private Dictionary<string, string> playerData = new Dictionary<string, string>();

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;

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

        alarm.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(TimerCoroution());
    }

    public void OpenShopView()
    {
        if (!shopView.activeSelf)
        {
            shopView.SetActive(true);

            alarm.SetActive(false);

            if (GameStateManager.instance.StoreType == StoreType.OneStore)
            {
                for (int i = 0; i < oneStore.Length; i++)
                {
                    oneStore[i].SetActive(false);
                }
            }

            if (playerDataBase.AttendanceDay == DateTime.Today.ToString("yyyyMMdd"))
            {
                ResetManager.instance.Initialize();
            }

            if (!first)
            {
                adShopReceiveContents[0].Initialize(RewardType.Gold, rankDataBase.GetRankInformation(GameStateManager.instance.GameRankType).stakes * 2);
                adShopReceiveContents[1].Initialize(RewardType.UpgradeTicket, 1);
                adShopReceiveContents[2].Initialize(RewardType.UpgradeTicket, 10);

                first = true;
            }

            adReset.SetActive(false);
            if (playerDataBase.ResetInfo.dailyReset == 0)
            {
                adReset.SetActive(true);
            }

            Initialize_Ad();

            boxArray[0].SetActive(false);
            boxArray[1].SetActive(false);

            localization_Reset = LocalizationManager.instance.GetString("Reset");
            localization_Days = LocalizationManager.instance.GetString("Days");
            localization_Hours = LocalizationManager.instance.GetString("Hours");
            localization_Minutes = LocalizationManager.instance.GetString("Minutes");

            shopRectTransform.offsetMax = Vector3.zero;

            if(GameStateManager.instance.StoreType == StoreType.OneStore)
            {
                return;
            }

            packageManager.OpenShop();

            if (playerDataBase.Formation == 2)
            {
                boxArray[1].SetActive(true);

                boxSSRTextArray[1].forwardText = (50 - playerDataBase.BuyUnderworldBoxSSRCount).ToString();
                boxSSRTextArray[1].localizationName = "BoxSSRInfo";
                boxSSRTextArray[1].ReLoad();
            }
            else
            {
                boxArray[0].SetActive(true);

                boxSSRTextArray[0].forwardText = (50 - playerDataBase.BuySnowBoxSSRCount).ToString();
                boxSSRTextArray[0].localizationName = "BoxSSRInfo";
                boxSSRTextArray[0].ReLoad();
            }

            Initialize_Count();
        }
    }

    void ReStartTimer()
    {
        StartCoroutine(TimerCoroution());
    }

    public void Change()
    {
        if (shopView.activeSelf)
        {
            if (playerDataBase.Formation == 2)
            {
                boxArray[1].SetActive(true);

                boxSSRTextArray[1].forwardText = (50 - playerDataBase.BuyUnderworldBoxSSRCount).ToString();
                boxSSRTextArray[1].localizationName = "BoxSSRInfo";
                boxSSRTextArray[1].ReLoad();
            }
            else
            {
                boxArray[0].SetActive(true);

                boxSSRTextArray[0].forwardText = (50 - playerDataBase.BuySnowBoxSSRCount).ToString();
                boxSSRTextArray[0].localizationName = "BoxSSRInfo";
                boxSSRTextArray[0].ReLoad();
            }
        }
    }

    void Initialize_Count()
    {
        dailyCountText[0].text = playerDataBase.ResetInfo.dailyNormalBox_1 + "/3";
        dailyCountText[1].text = playerDataBase.ResetInfo.dailyNormalBox_10 + "/1";
        dailyCountText[2].text = playerDataBase.ResetInfo.dailyEpicBox_1 + "/3";
        dailyCountText[3].text = playerDataBase.ResetInfo.dailyEpicBox_10 + "/1";
    }

    public void CloseShopView()
    {
        shopView.SetActive(false);
    }

    #region RandomBox
    public void BuySnowBox1() //상점에서 현금 상자 1개 구입
    {
        GetSnowBox(BoxType.Speical, 1);

        playerDataBase.BuySnowBox += 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuySnowBox", playerDataBase.BuySnowBox);
    }

    public void BuySnowBox2() //상점에서 현금 상자 10개 구입
    {
        GetSnowBox(BoxType.Speical, 10);

        playerDataBase.BuySnowBox += 10;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuySnowBox", playerDataBase.BuySnowBox);
    }

    public void GetSnowBox(BoxType type, int number)
    {
        switch (type)
        {
            case BoxType.Normal:
                playerDataBase.SnowBox_Normal = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", number);
                break;
            case BoxType.Epic:
                playerDataBase.SnowBox_Epic = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Epic", number);
                break;
            case BoxType.Speical:
                playerDataBase.SnowBox_Speical = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Speical", number);
                break;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        NotionManager.instance.UseNotion(NotionType.BuyShopItem);
    }

    public void BuyUnderworldBox1()
    {
        GetUnderworld(BoxType.Normal, 1);

        playerDataBase.BuyUnderworldBox += 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuyUnderworldBox", playerDataBase.BuyUnderworldBox);
    }

    public void BuyUnderworldBox2()
    {
        GetUnderworld(BoxType.Normal, 10);

        playerDataBase.BuyUnderworldBox += 10;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuyUnderworldBox", playerDataBase.BuyUnderworldBox);
    }

    public void GetUnderworld(BoxType type, int number)
    {
        switch (type)
        {
            case BoxType.Normal:
                playerDataBase.UnderworldBox_Normal = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", number);
                break;
            case BoxType.Epic:
                playerDataBase.UnderworldBox_Epic = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Epic", number);
                break;
            case BoxType.Speical:
                playerDataBase.UnderworldBox_Speical = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Speical", number);
                break;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
    }

    public void Buy_NormalBox(int number)
    {
        int price = 25000 * number;

        if (number >= 10)
        {
            if(playerDataBase.ResetInfo.dailyNormalBox_10 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                
                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }

            price -= 25000;
        }
        else
        {
            if (playerDataBase.ResetInfo.dailyNormalBox_1 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }
        }

        UIManager.instance.Renewal();

        if (playerDataBase.Coin >= price)
        {
            PlayfabManager.instance.UpdateSubtractGold(price);

            switch (GameStateManager.instance.WindCharacterType)
            {
                case WindCharacterType.Winter:
                    GetSnowBox(BoxType.Normal, number);
                    break;
                case WindCharacterType.UnderWorld:
                    GetUnderworld(BoxType.Normal, number);
                    break;
            }

            if(number >= 10)
            {
                ResetManager.instance.SetResetInfo(ResetType.DailyNormalBox_10);

                FirebaseAnalytics.LogEvent("BuyBox_Normal10");
            }
            else
            {
                ResetManager.instance.SetResetInfo(ResetType.DailyNormalBox_1);

                FirebaseAnalytics.LogEvent("BuyBox_Normal1");
            }

            Initialize_Count();

            SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
            NotionManager.instance.UseNotion(NotionType.BuyShopItem);
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
        }
    }

    public void Buy_EpicBox(int number)
    {
        int price = 100000 * number;

        if (number >= 10)
        {
            if (playerDataBase.ResetInfo.dailyEpicBox_10 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }

            price -= 100000;
        }
        else
        {
            if (playerDataBase.ResetInfo.dailyEpicBox_1 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }
        }

        UIManager.instance.Renewal();

        if (playerDataBase.Coin >= price)
        {
            PlayfabManager.instance.UpdateSubtractGold(price);

            switch (GameStateManager.instance.WindCharacterType)
            {
                case WindCharacterType.Winter:
                    GetSnowBox(BoxType.Epic, number);
                    break;
                case WindCharacterType.UnderWorld:
                    GetUnderworld(BoxType.Epic, number);
                    break;
            }

            if (number >= 10)
            {
                ResetManager.instance.SetResetInfo(ResetType.DailyEpicBox_10);

                FirebaseAnalytics.LogEvent("BuyBox_Epic10");
            }
            else
            {
                ResetManager.instance.SetResetInfo(ResetType.DailyEpicBox_1);

                FirebaseAnalytics.LogEvent("BuyBox_Epic1");
            }

            Initialize_Count();

            SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
            NotionManager.instance.UseNotion(NotionType.BuyShopItem);
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
                if(playerDataBase.ResetInfo.dailyReward == 0)
                {
                    ResetManager.instance.SetResetInfo(ResetType.DailyReward);

                    dailyContentArray[0].Locked();

                    PlayfabManager.instance.UpdateAddGold(price);

                    FirebaseAnalytics.LogEvent("Clear_DailyReward");
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
                UIManager.instance.Renewal();

                if (playerDataBase.Coin >= price)
                {
                    PlayfabManager.instance.UpdateSubtractGold(price);

                    FirebaseAnalytics.LogEvent("Buy_UpgradeTicket");
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                    NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                    return;
                }

                ItemAnimManager.instance.GetUpgradeTicket(number);

                SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
                NotionManager.instance.UseNotion(NotionType.GetUpgradeTicket);

                if(number < 11)
                {
                    ResetManager.instance.SetResetInfo(ResetType.DailyBuy1);

                    dailyContentArray[1].Locked();
                }
                else
                {
                    ResetManager.instance.SetResetInfo(ResetType.DailyBuy2);

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
                ItemAnimManager.instance.GetUpgradeTicket(1);
                NotionManager.instance.UseNotion(NotionType.GetUpgradeTicket);
                break;
            case 1:
                ItemAnimManager.instance.GetUpgradeTicket(10);
                NotionManager.instance.UseNotion(NotionType.GetUpgradeTicket);
                break;
            case 2:
                ItemAnimManager.instance.GetUpgradeTicket(100);
                NotionManager.instance.UseNotion(NotionType.GetUpgradeTicket);
                break;
            case 3:
                random = Random.Range(5000, 100001);

                PlayfabManager.instance.UpdateAddGold(random);

                SoundManager.instance.PlaySFX(GameSfxType.PlusMoney1);
                NotionManager.instance.UseNotion(NotionType.BuyShopItem);
                break;
            case 4:
                random = Random.Range(50000, 1000001);

                PlayfabManager.instance.UpdateAddGold(random);

                SoundManager.instance.PlaySFX(GameSfxType.PlusMoney2);
                NotionManager.instance.UseNotion(NotionType.BuyShopItem);
                break;
        }

        FirebaseAnalytics.LogEvent("Buy_Purhcase_" + number);

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
    }

    IEnumerator TimerCoroution()
    {
        if (dailyShopCountText.gameObject.activeInHierarchy)
        {
            f = System.DateTime.Now;
            g = System.DateTime.Today.AddDays(1);
            h = g - f;

            localization_Reset = LocalizationManager.instance.GetString("Reset");
            localization_Days = LocalizationManager.instance.GetString("Days");
            localization_Hours = LocalizationManager.instance.GetString("Hours");
            localization_Minutes = LocalizationManager.instance.GetString("Minutes");

            dailyShopCountText.text = localization_Reset + " : " + h.Hours.ToString("D2") + localization_Hours + " " + h.Minutes.ToString("D2") + localization_Minutes;

            if (playerDataBase.AttendanceDay == DateTime.Today.ToString("yyyyMMdd"))
            {
                ResetManager.instance.Initialize();

                Invoke("RestartTimer", 2.0f);
                yield break;
            }
        }

        yield return waitForSeconds;
        StartCoroutine(TimerCoroution());
    }

    public void Failed()
    {
        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
        NotionManager.instance.UseNotion(NotionType.CancelPurchase);
    }


    public void OpenAdResetView()
    {
        if(!adResetView.activeInHierarchy)
        {
            adResetView.SetActive(true);
        }
        else
        {
            adResetView.SetActive(false);
        }
    }

    public void Initialize_Ad()
    {
        boxShopLockArray[0].SetActive(true);
        boxShopLockArray[1].SetActive(true);

        adShopClearArray[0].SetActive(true);
        adShopClearArray[1].SetActive(true);
        adShopClearArray[2].SetActive(true);

        goldShopLockArray.SetActive(true);

        if (playerDataBase.ResetInfo.dailyNormalBox == 1)
        {
            LoadAd_BoxNR();
        }
        else
        {
            SetWatchAd_BoxNR(false);
        }

        if (playerDataBase.ResetInfo.dailyEpicBox == 1)
        {
            LoadAd_BoxRSR();
        }
        else
        {
            SetWatchAd_BoxRSR(false);
        }

        if (playerDataBase.ResetInfo.dailyAdsReward == 0)
        {
            adShopClearArray[0].SetActive(false);
        }

        if (playerDataBase.ResetInfo.dailyAdsReward2 == 0)
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

        if (playerDataBase.ResetInfo.dailyAdsReward3 == 0)
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

        if (playerDataBase.ResetInfo.dailyGoldReward == 0)
        {
            goldShopLockArray.SetActive(false);
        }
    }

    public void GetAdReward(int number)
    {
        switch(number)
        {
            case 0:
                if (playerDataBase.ResetInfo.dailyNormalBox == 1) return;

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Normal = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Normal = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", 1);
                        break;
                }

                SetWatchAd_BoxNR(true);
                break;
            case 1:
                if (playerDataBase.ResetInfo.dailyEpicBox == 1) return;

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Epic = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Epic", 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Epic = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Epic", 1);
                        break;
                }

                SetWatchAd_BoxRSR(true);
                break;
            case 2:
                if (playerDataBase.ResetInfo.dailyAdsReward == 1) return;

                PlayfabManager.instance.UpdateAddGold(rankDataBase.GetRankInformation(GameStateManager.instance.GameRankType).stakes * 2);

                ResetManager.instance.SetResetInfo(ResetType.DailyAdsReward);

                Initialize_Ad();
                break;
            case 3:
                if (playerDataBase.ResetInfo.dailyAdsReward2 == 1) return;

                ItemAnimManager.instance.GetUpgradeTicket(1);

                ResetManager.instance.SetResetInfo(ResetType.DailyAdsReward2);

                Initialize_Ad();
                break;
            case 4:
                if (playerDataBase.ResetInfo.dailyAdsReward3 == 1) return;

                ItemAnimManager.instance.GetUpgradeTicket(10);

                ResetManager.instance.SetResetInfo(ResetType.DailyAdsReward3);

                Initialize_Ad();
                break;
            case 5:
                if (playerDataBase.ResetInfo.dailyGoldReward == 1) return;

                random = Random.Range(7500, 20001);

                PlayfabManager.instance.UpdateAddGold(random);

                ResetManager.instance.SetResetInfo(ResetType.DailyGoldReward);

                Initialize_Ad();
                break;
            case 6:
                if (playerDataBase.ResetInfo.dailyReset == 1) return;

                StartCoroutine(AdResetCoroution());

                break;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        NotionManager.instance.UseNotion(NotionType.GetWatchAdReward);
    }

    IEnumerator AdResetCoroution()
    {
        adReset.SetActive(false);
        adResetView.SetActive(false);

        playerDataBase.ResetInfo.dailyReset = 1;
        playerDataBase.ResetInfo.dailyReward = 0;
        playerDataBase.ResetInfo.dailyBuy1 = 0;
        playerDataBase.ResetInfo.dailyBuy2 = 0;
        playerDataBase.ResetInfo.dailyBuyCount1 = 0;
        playerDataBase.ResetInfo.dailyBuyCount2 = 0;

        playerData.Clear();
        playerData.Add("ResetInfo", JsonUtility.ToJson(playerDataBase.ResetInfo));
        PlayfabManager.instance.SetPlayerData(playerData);

        yield return waitForSeconds;

        for (int i = 0; i < dailyContentArray.Length; i++)
        {
            dailyContentArray[i].UnLocked();
        }

        dailyContentArray[0].Initialize(ShopType.DailyReward);
        dailyContentArray[1].Initialize(ShopType.UpgradeTicket);
        dailyContentArray[2].Initialize(ShopType.UpgradeTicket);
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
            boxShopLockArray[0].SetActive(true);

            playerDataBase.ResetInfo.dailyNormalBox = 1;
            ResetManager.instance.SetResetInfo(ResetType.DailyNormalBox);

            playerData.Clear();
            playerData.Add("ResetInfo", JsonUtility.ToJson(playerDataBase.ResetInfo));
            PlayfabManager.instance.SetPlayerData(playerData);

            PlayerPrefs.SetString("AdCoolTime_BoxNR", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));

            adCoolTime_BoxNR = 86400;

            Debug.Log("Watch Ad BoxNR Play");

            StartCoroutine(WatchAdCorution_BoxNR());
        }
        else
        {
            boxShopLockArray[0].SetActive(false);

            Debug.Log("Watch Ad BoxNR Stop");

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
            boxShopLockArray[1].SetActive(true);

            playerDataBase.ResetInfo.dailyEpicBox = 1;
            ResetManager.instance.SetResetInfo(ResetType.DailyEpicBox);

            playerData.Clear();
            playerData.Add("ResetInfo", JsonUtility.ToJson(playerDataBase.ResetInfo));
            PlayfabManager.instance.SetPlayerData(playerData);

            PlayerPrefs.SetString("AdCoolTime_BoxRSR", DateTime.Now.AddDays(3).ToString("yyyy-MM-dd HH:mm:ss"));

            adCoolTime_BoxRSR = 86400 * 3;

            Debug.Log("Watch Ad BoxRSR Play");

            StartCoroutine(WatchAdCorution_BoxRSR());
        }
        else
        {
            boxShopLockArray[1].SetActive(false);

            Debug.Log("Watch Ad BoxRSR Stop");

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
        PlayfabManager.instance.UpdateAddGold(100000000);
    }

    [Button]
    public void MinusMoney()
    {
        PlayfabManager.instance.UpdateSubtractGold(100000000);
    }

    [Button]
    public void PlusUpgradeTicket()
    {
        ItemAnimManager.instance.GetUpgradeTicket(1000);
    }

    [Button]
    public void OpenRandomBox(int number)
    {
        switch (GameStateManager.instance.WindCharacterType)
        {
            case WindCharacterType.Winter:
                GetSnowBox(BoxType.Normal, number);
                break;
            case WindCharacterType.UnderWorld:
                GetUnderworld(BoxType.Normal, number);
                break;
        }
    }

    [Button]
    public void OpenSnowBox_Normal()
    {
        GetSnowBox(BoxType.Normal, 10);
    }

    [Button]
    public void OpenSnowBox_Epic()
    {
        GetSnowBox(BoxType.Epic, 10);
    }

    [Button]
    public void OpenSnowBox_Speical()
    {
        GetSnowBox(BoxType.Speical, 10);
    }


    [Button]
    public void OpenUnderworld_Normal()
    {
        GetUnderworld(BoxType.Normal, 10);
    }

    [Button]
    public void OpenUnderworld_Epic()
    {
        GetUnderworld(BoxType.Epic, 10);
    }

    [Button]
    public void OpenUnderworld_Speical()
    {
        GetUnderworld(BoxType.Speical, 10);
    }


    #endregion
}
