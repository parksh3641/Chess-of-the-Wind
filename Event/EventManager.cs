using Firebase.Analytics;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public GameObject eventView;

    public RectTransform rectTransform;

    public GameObject appReviewEvent;

    [Space]
    [Title("WelcomBox")]
    public GameObject welcomeBoxView;
    public GameObject welcomeBoxAlarm;
    public GameObject welcomeBoxAlarm2;
    public WelcomeBoxContent[] welcomeBoxContentArray;


    [Space]
    [Title("Welcome")]
    //public GameObject welcomeEnterButton;
    public GameObject welcomeView;
    public RectTransform welcomeGrid;
    public GameObject welcomeAlarm;
    public GameObject welcomeAlarm2;
    public WelcomeContent[] welcomeContentArray;

    [Space]
    [Title("RankUp")]
    public GameObject rankUpView;
    public RectTransform rankUpGrid;
    public GameObject rankUpAlarm;
    public GameObject rankUpAlarm2;
    public RankUpContent[] rankUpContentArray;

    public RankUpContent rankUpContent;

    public List<RankUpContent> rankUpContentList = new List<RankUpContent>();



    string[] strArray = new string[2];

    Sprite[] rankIconArray;

    PlayerDataBase playerDataBase;
    RankUpDataBase rankUpDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (rankUpDataBase == null) rankUpDataBase = Resources.Load("RankUpDataBase") as RankUpDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rankIconArray = imageDataBase.GetRankIconArray();

        eventView.SetActive(false);

        rectTransform.anchoredPosition = new Vector2(0, -9999);

        welcomeBoxView.SetActive(false);
        welcomeBoxAlarm.SetActive(false);
        welcomeBoxAlarm2.SetActive(false);

        welcomeView.SetActive(false);
        welcomeAlarm.SetActive(false);
        welcomeAlarm2.SetActive(false);
        welcomeGrid.anchoredPosition = new Vector2(0, -9999);


        rankUpView.SetActive(false);
        rankUpAlarm.SetActive(false);
        rankUpAlarm2.SetActive(false);
        rankUpGrid.anchoredPosition = new Vector2(0, -9999);
    }

    public void Initialize()
    {
        if (playerDataBase.WelcomeBoxCount < 9)
        {
            if (!playerDataBase.WelcomeBoxCheck)
            {
                OnWelcomeBoxAlarm();

                OpenWelcomView();
            }
        }
        else
        {
            welcomeBoxView.SetActive(false);
        }

        if(playerDataBase.WelcomeCount < 6)
        {
            if (!playerDataBase.WelcomeCheck)
            {
                OnWelcomeAlarm();
            }
        }
        else
        {
            welcomeView.SetActive(false);
        }


        for (int i = 0; i < rankUpDataBase.rankUpInfomationList.Count; i++)
        {
            RankUpContent monster = Instantiate(rankUpContent);
            monster.transform.SetParent(rankUpGrid);
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;

            strArray = rankUpDataBase.rankUpInfomationList[i].gameRankType.ToString().Split("_");

            monster.Initialize(i, rankUpDataBase.rankUpInfomationList[i], rankIconArray[i + 4], strArray[0], strArray[1], this);

            rankUpContentList.Add(monster);
        }

        CheckingRankUp();

        if(playerDataBase.ChallengeCount >= 6)
        {
            if (playerDataBase.WelcomeBoxCount < 9 && !playerDataBase.WelcomeCheck)
            {
                OpenWelcomBoxView();
            }
        }
    }

    public void CheckingRankUp()
    {
        if (playerDataBase.RankUpCount + 4 <= (int)GameStateManager.instance.GameRankType)
        {
            OnSetRankUpAlarm();
        }
    }


    public void OpenEventView()
    {
        if(!eventView.activeInHierarchy)
        {
            eventView.SetActive(true);

            appReviewEvent.SetActive(true);

            if(playerDataBase.ReviewNumber == 1 || GameStateManager.instance.StoreType == StoreType.OneStore)
            {
                appReviewEvent.SetActive(false);
            }

            FirebaseAnalytics.LogEvent("Open_Event");
        }
        else
        {
            eventView.SetActive(false);
            welcomeView.SetActive(false);
            rankUpView.SetActive(false);
        }
    }

    #region WelcomeBox
    public void OpenWelcomBoxView()
    {
        if (!welcomeBoxView.activeInHierarchy)
        {
            welcomeBoxView.SetActive(true);

            InitializeWelcomeBox();

            CheckWelcomeBox();

            if (playerDataBase.AttendanceDay == DateTime.Today.ToString("yyyyMMdd"))
            {
                ResetManager.instance.Initialize();
            }

            FirebaseAnalytics.LogEvent("Open_WelcomeBox");
        }
        else
        {
            welcomeBoxView.SetActive(false);
        }
    }
    public void InitializeWelcomeBox()
    {
        welcomeBoxContentArray[0].receiveContent.Initialize(RewardType.Box_Normal, 10);
        welcomeBoxContentArray[1].receiveContent.Initialize(RewardType.Box_Normal, 10);
        welcomeBoxContentArray[2].receiveContent.Initialize(RewardType.Box_Normal, 10);
        welcomeBoxContentArray[3].receiveContent.Initialize(RewardType.Box_Normal, 10);
        welcomeBoxContentArray[4].receiveContent.Initialize(RewardType.Box_Normal, 10);
        welcomeBoxContentArray[5].receiveContent.Initialize(RewardType.Box_Normal, 10);
        welcomeBoxContentArray[6].receiveContent.Initialize(RewardType.Box_Normal, 10);
        welcomeBoxContentArray[7].receiveContent.Initialize(RewardType.Box_Normal, 10);
        welcomeBoxContentArray[8].receiveContent.Initialize(RewardType.Box_Normal, 20);
    }

    public void CheckWelcomeBox()
    {
        welcomeBoxContentArray[0].Initialize(playerDataBase.WelcomeBoxCount, playerDataBase.WelcomeBoxCheck, this);
        welcomeBoxContentArray[1].Initialize(playerDataBase.WelcomeBoxCount, playerDataBase.WelcomeBoxCheck, this);
        welcomeBoxContentArray[2].Initialize(playerDataBase.WelcomeBoxCount, playerDataBase.WelcomeBoxCheck, this);
        welcomeBoxContentArray[3].Initialize(playerDataBase.WelcomeBoxCount, playerDataBase.WelcomeBoxCheck, this);
        welcomeBoxContentArray[4].Initialize(playerDataBase.WelcomeBoxCount, playerDataBase.WelcomeBoxCheck, this);
        welcomeBoxContentArray[5].Initialize(playerDataBase.WelcomeBoxCount, playerDataBase.WelcomeBoxCheck, this);
        welcomeBoxContentArray[6].Initialize(playerDataBase.WelcomeBoxCount, playerDataBase.WelcomeBoxCheck, this);
        welcomeBoxContentArray[7].Initialize(playerDataBase.WelcomeBoxCount, playerDataBase.WelcomeBoxCheck, this);
        welcomeBoxContentArray[8].Initialize(playerDataBase.WelcomeBoxCount, playerDataBase.WelcomeBoxCheck, this);
    }

    public void WelcomeBoxReceiveButton(Action action)
    {
        if (playerDataBase.WelcomeBoxCheck) return;

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        switch (GameStateManager.instance.WindCharacterType)
        {
            case WindCharacterType.Winter:
                playerDataBase.SnowBox_Normal = 10;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", 10);
                break;
            case WindCharacterType.UnderWorld:
                playerDataBase.UnderworldBox_Normal = 10;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", 10);
                break;
        }

        playerDataBase.WelcomeBoxCount += 1;
        playerDataBase.WelcomeBoxCheck = true;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeBoxCount", playerDataBase.WelcomeBoxCount);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeBoxCheck", 1);

        action.Invoke();

        CheckWelcomeBox();

        OffWelcomeBoxAlarm();

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        NotionManager.instance.UseNotion(NotionType.GetReward);

        FirebaseAnalytics.LogEvent("Clear_WelcomeBox");
    }

    public void OnWelcomeBoxAlarm()
    {
        welcomeBoxAlarm.SetActive(true);
        welcomeBoxAlarm2.SetActive(true);
    }

    public void OffWelcomeBoxAlarm()
    {
        welcomeBoxAlarm.SetActive(true);
        welcomeBoxAlarm2.SetActive(true);
    }

    #endregion


    #region Welcome
    public void OpenWelcomView()
    {
        if (!welcomeView.activeInHierarchy)
        {
            welcomeView.SetActive(true);

            InitializeWelcome();

            CheckWelcome();

            if (playerDataBase.AttendanceDay == DateTime.Today.ToString("yyyyMMdd"))
            {
                ResetManager.instance.Initialize();
            }

            FirebaseAnalytics.LogEvent("Open_Welcome");
        }
        else
        {
            welcomeView.SetActive(false);
        }
    }

    public void InitializeWelcome()
    {
        welcomeContentArray[0].receiveContent.Initialize(RewardType.Gold, 3000);
        welcomeContentArray[1].receiveContent.Initialize(RewardType.Box_Normal, 5);
        welcomeContentArray[2].receiveContent.Initialize(RewardType.Box_Normal, 10);
        welcomeContentArray[3].receiveContent.Initialize(RewardType.Gold, 15000);
        welcomeContentArray[4].receiveContent.Initialize(RewardType.Box_Normal, 20);
        welcomeContentArray[5].receiveContent.Initialize(RewardType.UpgradeTicket, 100);
        welcomeContentArray[6].receiveContent.Initialize(RewardType.Box_Epic, 10);
    }

    public void CheckWelcome()
    {
        welcomeContentArray[0].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, this);
        welcomeContentArray[1].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, this);
        welcomeContentArray[2].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, this);
        welcomeContentArray[3].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, this);
        welcomeContentArray[4].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, this);
        welcomeContentArray[5].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, this);
        welcomeContentArray[6].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, this);
    }

    public void WelcomeReceiveButton(int number, Action action)
    {
        if (playerDataBase.WelcomeCheck) return;

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        switch (number)
        {
            case 0:
                PlayfabManager.instance.UpdateAddGold(3000);
                break;
            case 1:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Normal = 5;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", 5);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Normal = 5;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", 5);
                        break;
                }
                break;
            case 2:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Normal = 10;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", 10);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Normal = 10;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", 10);
                        break;
                }
                break;
            case 3:
                PlayfabManager.instance.UpdateAddGold(15000);
                break;
            case 4:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Normal = 15;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", 20);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Normal = 15;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", 20);
                        break;
                }
                break;
            case 5:
                ItemAnimManager.instance.GetUpgradeTicket(100);
                break;
            case 6:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Epic = 10;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Epic", 10);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Epic = 10;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Epic", 10);
                        break;
                }
                break;
        }

        playerDataBase.WelcomeCount += 1;
        playerDataBase.WelcomeCheck = true;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeCount", playerDataBase.WelcomeCount);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeCheck", 1);

        action.Invoke();

        CheckWelcome();

        OffWelcomeAlarm();

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        NotionManager.instance.UseNotion(NotionType.GetReward);

        FirebaseAnalytics.LogEvent("Clear_Welcome : " + playerDataBase.WelcomeCount);
    }

    public void OnWelcomeAlarm()
    {
        welcomeAlarm.SetActive(true);
        welcomeAlarm2.SetActive(true);
    }

    public void OffWelcomeAlarm()
    {
        welcomeAlarm.SetActive(false);
        welcomeAlarm2.SetActive(false);
    }

    #endregion

    #region RankUp
    public void OpenRankUpView()
    {
        if (!rankUpView.activeInHierarchy)
        {
            rankUpAlarm.SetActive(false);
            rankUpAlarm2.SetActive(false);

            rankUpView.SetActive(true);

            CheckRankUp();

            FirebaseAnalytics.LogEvent("Open_RankUp");
        }
        else
        {
            rankUpView.SetActive(false);
        }
    }

    void CheckRankUp()
    {
        for(int i = 0; i < rankUpContentList.Count; i ++)
        {
            rankUpContentList[i].CheckReceived();
        }    
    }

    public void RankUpReceiveButton(int number)
    {
        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        for (int i = 0; i < rankUpDataBase.rankUpInfomationList[number].receiveInformationList.Count; i++)
        {
            switch (rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].rewardType)
            {
                case RewardType.Gold:
                    PlayfabManager.instance.UpdateAddGold(rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count);
                    break;
                case RewardType.UpgradeTicket:
                    ItemAnimManager.instance.GetUpgradeTicket(rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count);
                    break;
                case RewardType.Box_Normal:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_Normal = rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_Normal = rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count);
                            break;
                    }
                    break;
                case RewardType.Box_Epic:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_Epic = rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Epic", rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_Epic = rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Epic", rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count);
                            break;
                    }
                    break;
                case RewardType.Box_Speical:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox_Speical = rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Speical", rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox_Speical = rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Speical", rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count);
                            break;
                    }
                    break;
            }
        }

        playerDataBase.RankUpCount += 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("RankUpCount", playerDataBase.RankUpCount);

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        NotionManager.instance.UseNotion(NotionType.GetReward);

        FirebaseAnalytics.LogEvent("Clear_RankUp : " + number);

        CheckRankUp();
    }

    public void OnSetRankUpAlarm()
    {
        rankUpAlarm.SetActive(true);
        rankUpAlarm2.SetActive(true);
    }

    #endregion
}
