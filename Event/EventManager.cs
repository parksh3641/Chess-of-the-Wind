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

    [Space]
    [Title("Welcome")]
    public GameObject welcomeEnterView;
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
        if (playerDataBase.WelcomeCount >= 7)
        {
            //welcomeEnterView.SetActive(false);
        }
        else
        {
            welcomeEnterView.SetActive(true);

            if (!playerDataBase.WelcomeCheck)
            {
                OnSetWelcomeAlarm();
            }
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

            FirebaseAnalytics.LogEvent("OpenEvent");
        }
        else
        {
            eventView.SetActive(false);
            welcomeView.SetActive(false);
            rankUpView.SetActive(false);
        }
    }


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

            FirebaseAnalytics.LogEvent("OpenWelcome");
        }
        else
        {
            welcomeView.SetActive(false);
        }
    }

    public void InitializeWelcome()
    {
        welcomeContentArray[0].receiveContent.Initialize(RewardType.Gold, 2000);
        welcomeContentArray[1].receiveContent.Initialize(RewardType.Box_R, 3);
        welcomeContentArray[2].receiveContent.Initialize(RewardType.Box, 10);
        welcomeContentArray[3].receiveContent.Initialize(RewardType.Gold, 15000);
        welcomeContentArray[4].receiveContent.Initialize(RewardType.Box, 10);
        welcomeContentArray[5].receiveContent.Initialize(RewardType.UpgradeTicket, 5);
        welcomeContentArray[6].receiveContent.Initialize(RewardType.Box_SSR, 1);
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
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 2000);
                break;
            case 1:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_R = 3;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_R", 3);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_R = 3;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_R", 3);
                        break;
                }
                break;
            case 2:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox = 10;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox", 10);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox = 10;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", 10);
                        break;
                }
                break;
            case 3:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 15000);
                break;
            case 4:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox = 10;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox", 10);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox = 10;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", 10);
                        break;
                }
                break;
            case 5:
                playerDataBase.SetUpgradeTicket(RankType.N, 5);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));
                break;
            case 6:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_SSR = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SSR", 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_SSR = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SSR", 1);
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

        OnCheckWelcomeAlarm();

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        NotionManager.instance.UseNotion(NotionType.GetReward);
    }

    public void OnSetWelcomeAlarm()
    {
        welcomeAlarm.SetActive(true);
        welcomeAlarm2.SetActive(true);
    }

    public void OnCheckWelcomeAlarm()
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

            FirebaseAnalytics.LogEvent("OpenRankUp");
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
                    PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count);
                    break;
                case RewardType.UpgradeTicket:
                    playerDataBase.SetUpgradeTicket(RankType.N, rankUpDataBase.rankUpInfomationList[number].receiveInformationList[i].count);

                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));
                    break;
                case RewardType.Box:
                    break;
                case RewardType.Box_N:
                    break;
                case RewardType.Box_R:
                    break;
                case RewardType.Box_SR:
                    break;
                case RewardType.Box_SSR:
                    break;
                case RewardType.Box_UR:
                    break;
                case RewardType.Box_NR:
                    break;
                case RewardType.Box_RSR:
                    break;
                case RewardType.Box_SRSSR:
                    break;
            }
        }

        playerDataBase.RankUpCount += 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("RankUpCount", playerDataBase.RankUpCount);

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        NotionManager.instance.UseNotion(NotionType.GetReward);

        CheckRankUp();
    }

    public void OnSetRankUpAlarm()
    {
        rankUpAlarm.SetActive(true);
        rankUpAlarm2.SetActive(true);
    }

    #endregion
}
