﻿using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WindCharacterClass
{
    public WindCharacterType windCharacterType = WindCharacterType.Winter;
    public PresentType presentType = PresentType.A; //좋아하는 선물

    public int friendship_Level = 0; //호감도 레벨
    public int friendship_Exp = 0; //호감도 경험치

    public int storyProgress = 0; //스토리 진행도

    public int windlevel = 0; //레벨
    public int windPower = 0; //바람 세기

    public bool hidden = false;
}

[System.Serializable]
public class BlockClass
{
    public BlockType blockType = BlockType.Default;
    public RankType rankType = RankType.N;
    public string instanceId = "";

    public int level = 0;
    public int ssrLevel = 0;
    public int equipInfo = 0;
}

[System.Serializable]
public class WindCharacterUpgrade
{
    public int max = 99;

    public int gold = 0;
    public int addGold = 0;

    public int exp = 0; //레벨 업에 필요한 경험치
    public int addExp = 0;

    public int friendshipUnlock = 0; //다이아로 스토리 해금
    public int addFriendshipUnlock = 0;

    public float saleCardUpgradecost = 0; //카드 강화 비용 감소
}

[System.Serializable]
public class PresentClass
{
    public PresentType presentType = PresentType.A;

    public int holdNumber = 0;
}

[System.Serializable]
public class UpgradeTicketClass
{
    public RankType rankType = RankType.N;
    public int holdNumber = 0;
}


[CreateAssetMenu(fileName = "PlayerDataBase", menuName = "ScriptableObjects/PlayerDataBase")]
public class PlayerDataBase : ScriptableObject
{
    [Title("Money")]
    [SerializeField]
    private long coin = 0;
    [SerializeField]
    private long coinA = 0;
    [SerializeField]
    private long coinB = 0;
    [SerializeField]
    private int crystal = 0;
    [SerializeField]
    private int millage = 0;

    [Space]
    [Title("User")]
    [SerializeField]
    private int formation = 0;
    [SerializeField]
    private int nowRank = 0;
    [SerializeField]
    private int highRank = 0;
    [SerializeField]
    private int star = 0;
    [SerializeField]
    private int playTime = 0;
    [SerializeField]
    private int adCount = 0;
    [SerializeField]
    private int challengeCount = 0;
    [SerializeField]
    private int rankUpCount = 0;
    [SerializeField]
    private int testAccount = 0;
    [SerializeField]
    private int os = 0;

    [Space]
    [Title("Daily")]
    [SerializeField]
    private int dailyWin = 0;
    [SerializeField]
    private int dailyReward = 0;
    [SerializeField]
    private int dailyBuy1 = 0;
    [SerializeField]
    private int dailyBuy2 = 0;
    [SerializeField]
    private int dailyBuyCount1 = 0;
    [SerializeField]
    private int dailyBuyCount2 = 0;
    [SerializeField]
    private int dailyNormalBox = 0;
    [SerializeField]
    private int dailyEpicBox = 0;
    [SerializeField]
    private int dailyNormalBox_1 = 0;
    [SerializeField]
    private int dailyNormalBox_10 = 0;
    [SerializeField]
    private int dailyEpicBox_1 = 0;
    [SerializeField]
    private int dailyEpicBox_10 = 0;
    [SerializeField]
    private int dailyAdsReward = 0;
    [SerializeField]
    private int dailyAdsReward2 = 0;
    [SerializeField]
    private int dailyAdsReward3 = 0;
    [SerializeField]
    private int dailyGoldReward = 0;
    [SerializeField]
    private int dailyReset = 0;

    [Space]
    [Title("Package_Daily")]
    [SerializeField]
    private int package_Daily1 = 0;
    [SerializeField]
    private int package_Daily2 = 0;
    [SerializeField]
    private int package_Daily3 = 0;
    [SerializeField]
    private int package_Daily4 = 0;
    [SerializeField]
    private int package_Daily5 = 0;

    [Space]
    [Title("Package_Weekly")]
    [SerializeField]
    private int package_Weekly1 = 0;
    [SerializeField]
    private int package_Weekly2 = 0;
    [SerializeField]
    private int package_Weekly3 = 0;
    [SerializeField]
    private int package_Weekly4 = 0;
    [SerializeField]
    private int package_Weekly5 = 0;

    [Space]
    [Title("Package_Monthly")]
    [SerializeField]
    private int package_Monthly1 = 0;
    [SerializeField]
    private int package_Monthly2 = 0;
    [SerializeField]
    private int package_Monthly3 = 0;
    [SerializeField]
    private int package_Monthly4 = 0;
    [SerializeField]
    private int package_Monthly5 = 0;

    [Space]
    [Title("Achievement")]
    [SerializeField]
    private int destroyBlockCount = 0;
    [SerializeField]
    private int winGetMoney = 0;
    [SerializeField]
    private int totalRaf = 0;
    [SerializeField]
    private int synthesisGetBlock = 0;
    [SerializeField]
    private int rankDownCount = 0;
    [SerializeField]
    private int rankDownStreak = 0;
    [SerializeField]
    private int winNumber = 0;
    [SerializeField]
    private int winQueen = 0;
    [SerializeField]
    private int goalAchieveCount = 0;
    [SerializeField]
    private int chargingRM = 0;
    [SerializeField]
    private int boxOpenCount = 0;
    [SerializeField]
    private int accessDate = 0;
    [SerializeField]
    private int upgradeSuccessCount = 0;
    [SerializeField]
    private int upgradeFailCount = 0;
    [SerializeField]
    private int useUpgradeTicket = 0;
    [SerializeField]
    private int repairBlockCount = 0;
    [SerializeField]
    private int consumeGold = 0;

    [SerializeField]
    private List<AchievementInfo> achievementInfoList = new List<AchievementInfo>();

    [Space]
    [Title("Title")]
    [SerializeField]
    private int titleNumber = 0;

    [SerializeField]
    private List<TitleNormalInformation> titleNormalInformationList = new List<TitleNormalInformation>();
    [SerializeField]
    private List<TitleSpeicalInformation> titleSpeicalInformationList = new List<TitleSpeicalInformation>();

    [Space]
    [Title("Coupon")]
    [SerializeField]
    private int comicWorld2023 = 0;
    [SerializeField]
    private int indieFestival2023 = 0;
    [SerializeField]
    private int naverCafe202310 = 0;
    [SerializeField]
    private int naverCafe202311 = 0;
    [SerializeField]
    private int naverCafe202312 = 0;
    [SerializeField]
    private int naverCafe202401 = 0;
    [SerializeField]
    private int naverCafe202402 = 0;
    [SerializeField]
    private int naverCafe202403 = 0;
    [SerializeField]
    private int naverCafe202404 = 0;
    [SerializeField]
    private int naverCafe202405 = 0;
    [SerializeField]
    private int naverCafe202406 = 0;
    [SerializeField]
    private int naverCafe202407 = 0;
    [SerializeField]
    private int naverCafe202408 = 0;
    [SerializeField]
    private int naverCafe202409 = 0;
    [SerializeField]
    private int naverCafe202410 = 0;
    [SerializeField]
    private int naverCafe202411 = 0;
    [SerializeField]
    private int naverCafe202412 = 0;


    [Space]
    [Title("Reset")]
    [SerializeField]
    public int season = 0;
    [SerializeField]
    public string attendanceDay = "";
    [SerializeField]
    public int attendanceCount = 0;
    [SerializeField]
    public bool attendanceCheck = false;
    [Space]
    [SerializeField]
    public int welcomeCount = 0;
    [SerializeField]
    public bool welcomeCheck = false;
    [Space]
    [SerializeField]
    public int welcomeBoxCount = 0;
    [SerializeField]
    public bool welcomeBoxCheck = false;
    [Space]
    [SerializeField]
    public string nextMonday = "";
    [Space]
    public string nextMonth = "";
    [SerializeField]
    private int update = 0;

    [Space]
    [Title("Info")]
    [SerializeField]
    private int newbieWin = 0;
    [SerializeField]
    private int newbieLose = 0;
    [SerializeField]
    private int gosuWin = 0;
    [SerializeField]
    private int gosuLose = 0;

    [Space]
    [Title("Equip")]
    [SerializeField]
    private string armor = "";
    [SerializeField]
    private string weapon = "";
    [SerializeField]
    private string shield = "";
    [SerializeField]
    private string newbie = "";

    [Space]
    [Title("Emote")]
    [SerializeField]
    private int emote1 = 0;
    [SerializeField]
    private int emote2 = 1;
    [SerializeField]
    private int emote3 = 2;
    [SerializeField]
    private int emote4 = 3;
    [SerializeField]
    private int emote5 = 4;

    [Space]
    [Title("Box_Snow")]
    [SerializeField]
    private int snowBox = 0;
    [SerializeField]
    private int snowBox_N = 0;
    [SerializeField]
    private int snowBox_R = 0;
    [SerializeField]
    private int snowBox_SR = 0;
    [SerializeField]
    private int snowBox_SSR = 0;
    [SerializeField]
    private int snowBox_UR = 0;
    [SerializeField]
    private int snowBox_NR = 0;
    [SerializeField]
    private int snowBox_RSR = 0;
    [SerializeField]
    private int snowBox_SRSSR = 0;

    [Space]
    [Title("Box_Under")]
    [SerializeField]
    private int underworldBox = 0;
    [SerializeField]
    private int underworldBox_N = 0;
    [SerializeField]
    private int underworldBox_R = 0;
    [SerializeField]
    private int underworldBox_SR = 0;
    [SerializeField]
    private int underworldBox_SSR = 0;
    [SerializeField]
    private int underworldBox_UR = 0;
    [SerializeField]
    private int underworldBox_NR = 0;
    [SerializeField]
    private int underworldBox_RSR = 0;
    [SerializeField]
    private int underworldBox_SRSSR = 0;

    [Space]
    [Title("Box_Piece")]
    [SerializeField]
    private int boxPiece_N = 0;
    [SerializeField]
    private int boxPiece_R = 0;
    [SerializeField]
    private int boxPiece_SR = 0;
    [SerializeField]
    private int boxPiece_SSR = 0;
    [SerializeField]
    private int boxPiece_UR = 0;

    [Space]
    [Title("Box Buy Count")]
    [SerializeField]
    private int buySnowBox = 0;
    [SerializeField]
    private int buyUnderworldBox = 0;
    [SerializeField]
    private int buySnowBoxSSRCount = 0;
    [SerializeField]
    private int buyUnderworldSSRCount = 0;

    [Space]
    [Title("Package")]
    [SerializeField]
    private int shopNewbie = 0;
    [SerializeField]
    private int shopSliver = 0;
    [SerializeField]
    private int shopGold = 0;
    [SerializeField]
    private int shopPlatinum = 0;
    [SerializeField]
    private int shopDiamond = 0;
    [SerializeField]
    private int shopLegend = 0;
    [SerializeField]
    private int shopSupply = 0;


    [Space]
    [Title("Wind Character")]
    [SerializeField]
    private List<WindCharacterClass> windCharacterList = new List<WindCharacterClass>();

    [Space]
    [Title("Block")]
    [SerializeField]
    private List<BlockClass> blockList = new List<BlockClass>();
    public List<BlockClass> successionLevel = new List<BlockClass>();
    public List<string> sellBlockList = new List<string>();

    [Space]
    [Title("Upgrade")]
    [SerializeField]
    private WindCharacterUpgrade windCharacterUpgrade;

    [Space]
    [Title("Present")]
    [SerializeField]
    private List<PresentClass> presentList = new List<PresentClass>();

    [Space]
    [Title("Upgrade")]
    [SerializeField]
    private List<UpgradeTicketClass> upgradeTicketList = new List<UpgradeTicketClass>();

    [Space]
    [Title("ETC")]
    [SerializeField]
    private int newsAlarm = 0;
    [SerializeField]
    private int defDestroyTicket = 0;

    Dictionary<string, string> levelCustomData = new Dictionary<string, string>();

    public delegate void BoxEvent();
    public static event BoxEvent eGetSnowBox, eGetSnowBox_N, eGetSnowBox_R, eGetSnowBox_SR, eGetSnowBox_SSR, eGetSnowBox_UR, eGetSnowBox_NR, eGetSnowBox_RSR, eGetSnowBox_SRSSR,
        eGetUnderworldBox, eGetUnderworldBox_N, eGetUnderworldBox_R, eGetUnderworldBox_SR, eGetUnderworldBox_SSR, eGetUnderworldBox_UR, eGetUnderworldBox_NR,
        eGetUnderworldBox_RSR, eGetUnderworldBox_SRSSR;

    public delegate void TitleEvent();
    public static event TitleEvent eGetNormalTitle, eGetSpeicalTitle;

    #region Data

    public long Coin
    {
        get
        {
            return coin;
        }
        set
        {
            coin = value;
        }
    }

    public long CoinA
    {
        get
        {
            return coinA;
        }
        set
        {
            coinA = value;
        }
    }

    public long CoinB
    {
        get
        {
            return coinB;
        }
        set
        {
            coinB = value;
        }
    }

    public int Crystal
    {
        get
        {
            return crystal;
        }
        set
        {
            crystal = value;
        }
    }

    public int Millage
    {
        get
        {
            return millage;
        }
        set
        {
            millage = value;
        }
    }

    public int Formation
    {
        get
        {
            return formation;
        }
        set
        {
            formation = value;
        }
    }

    public int Star
    {
        get
        {
            return star;
        }
        set
        {
            star = value;
        }
    }

    public int PlayTime
    {
        get
        {
            return playTime;
        }
        set
        {
            playTime = value;
        }
    }

    public int AdCount
    {
        get
        {
            return adCount;
        }
        set
        {
            adCount = value;
        }
    }

    public int NowRank
    {
        get
        {
            return nowRank;
        }
        set
        {
            nowRank = value;
        }
    }

    public int HighRank
    {
        get
        {
            return highRank;
        }
        set
        {
            highRank = value;
        }
    }

    public int TotalRaf
    {
        get
        {
            return totalRaf;
        }
        set
        {
            totalRaf = value;
        }
    }

    public int AccessDate
    {
        get
        {
            return accessDate;
        }
        set
        {
            accessDate = value;
        }
    }

    public int ChallengeCount
    {
        get
        {
            return challengeCount;
        }
        set
        {
            challengeCount = value;
        }
    }

    public int RankUpCount
    {
        get
        {
            return rankUpCount;
        }
        set
        {
            rankUpCount = value;
        }
    }

    public int TestAccount
    {
        get
        {
            return testAccount;
        }
        set
        {
            testAccount = value;
        }
    }

    public int OS
    {
        get
        {
            return os;
        }
        set
        {
            os = value;
        }
    }

    public int DailyWin
    {
        get
        {
            return dailyWin;
        }
        set
        {
            dailyWin = value;
        }
    }

    public int DailyReward
    {
        get
        {
            return dailyReward;
        }
        set
        {
            dailyReward = value;
        }
    }

    public int DailyBuy1
    {
        get
        {
            return dailyBuy1;
        }
        set
        {
            dailyBuy1 = value;
        }
    }

    public int DailyBuy2
    {
        get
        {
            return dailyBuy2;
        }
        set
        {
            dailyBuy2 = value;
        }
    }

    public int DailyBuyCount1
    {
        get
        {
            return dailyBuyCount1;
        }
        set
        {
            dailyBuyCount1 = value;
        }
    }

    public int DailyBuyCount2
    {
        get
        {
            return dailyBuyCount2;
        }
        set
        {
            dailyBuyCount2 = value;
        }
    }

    public int DailyNormalBox
    {
        get
        {
            return dailyNormalBox;
        }
        set
        {
            dailyNormalBox = value;
        }
    }

    public int DailyEpicBox
    {
        get
        {
            return dailyEpicBox;
        }
        set
        {
            dailyEpicBox = value;
        }
    }

    public int DailyNormalBox_1
    {
        get
        {
            return dailyNormalBox_1;
        }
        set
        {
            dailyNormalBox_1 = value;
        }
    }

    public int DailyNormalBox_10
    {
        get
        {
            return dailyNormalBox_10;
        }
        set
        {
            dailyNormalBox_10 = value;
        }
    }

    public int DailyEpicBox_1
    {
        get
        {
            return dailyEpicBox_1;
        }
        set
        {
            dailyEpicBox_1 = value;
        }
    }

    public int DailyEpicBox_10
    {
        get
        {
            return dailyEpicBox_10;
        }
        set
        {
            dailyEpicBox_10 = value;
        }
    }

    public int DailyAdsReward
    {
        get
        {
            return dailyAdsReward;
        }
        set
        {
            dailyAdsReward = value;
        }
    }

    public int DailyAdsReward2
    {
        get
        {
            return dailyAdsReward2;
        }
        set
        {
            dailyAdsReward2 = value;
        }
    }

    public int DailyAdsReward3
    {
        get
        {
            return dailyAdsReward3;
        }
        set
        {
            dailyAdsReward3 = value;
        }
    }

    public int DailyGoldReward
    {
        get
        {
            return dailyGoldReward;
        }
        set
        {
            dailyGoldReward = value;
        }
    }

    public int DailyReset
    {
        get { return dailyReset ; } 
        set { dailyReset = value; }
    }

    public int Package_Daily1
    {
        get { return package_Daily1; }
        set { package_Daily1 = value; }
    }

    public int Package_Daily2
    {
        get { return package_Daily2; }
        set { package_Daily2 = value; }
    }

    public int Package_Daily3
    {
        get { return package_Daily3; }
        set { package_Daily3 = value; }
    }

    public int Package_Daily4
    {
        get { return package_Daily4; }
        set { package_Daily4 = value; }
    }

    public int Package_Daily5
    {
        get { return package_Daily5; }
        set { package_Daily5 = value; }
    }

    public int Package_Weekly1
    {
        get { return package_Weekly1; }
        set { package_Weekly1 = value; }
    }

    public int Package_Weekly2
    {
        get { return package_Weekly2; }
        set { package_Weekly2 = value; }
    }

    public int Package_Weekly3
    {
        get { return package_Weekly3; }
        set { package_Weekly3 = value; }
    }

    public int Package_Weekly4
    {
        get { return package_Weekly4; }
        set { package_Weekly4 = value; }
    }

    public int Package_Weekly5
    {
        get { return package_Weekly5; }
        set { package_Weekly5 = value; }
    }

    public int Package_Monthly1
    {
        get { return package_Monthly1; }
        set { package_Monthly1 = value; }
    }

    public int Package_Monthly2
    {
        get { return package_Monthly2; }
        set { package_Monthly2 = value; }
    }

    public int Package_Monthly3
    {
        get { return package_Monthly3; }
        set { package_Monthly3 = value; }
    }

    public int Package_Monthly4
    {
        get { return package_Monthly4; }
        set { package_Monthly4 = value; }
    }

    public int Package_Monthly5
    {
        get { return package_Monthly5; }
        set { package_Monthly5 = value; }
    }


    public int ConsumeGold
    {
        get
        {
            return consumeGold;
        }
        set
        {
            consumeGold = value;
        }
    }

    public int DestroyBlockCount
    {
        get
        {
            return destroyBlockCount;
        }
        set
        {
            destroyBlockCount = value;
        }
    }

    public int WinGetMoney
    {
        get
        {
            return winGetMoney;
        }
        set
        {
            winGetMoney = value;
        }
    }
    public int SynthesisGetBlock
    {
        get
        {
            return synthesisGetBlock;
        }
        set
        {
            synthesisGetBlock = value;
        }
    }

    public int RankDownCount
    {
        get
        {
            return rankDownCount;
        }
        set
        {
            rankDownCount = value;
        }
    }
    public int RankDownStreak
    {
        get
        {
            return rankDownStreak;
        }
        set
        {
            rankDownStreak = value;
        }
    }

    public int WinNumber
    {
        get
        {
            return winNumber;
        }
        set
        {
            winNumber = value;
        }
    }

    public int WinQueen
    {
        get
        {
            return winQueen;
        }
        set
        {
            winQueen = value;
        }
    }

    public int GoalAchieveCount
    {
        get
        {
            return goalAchieveCount;
        }
        set
        {
            goalAchieveCount = value;
        }
    }

    public int ChargingRM
    {
        get
        {
            return chargingRM;
        }
        set
        {
            chargingRM = value;
        }
    }

    public int BoxOpenCount
    {
        get
        {
            return boxOpenCount;
        }
        set
        {
            boxOpenCount = value;
        }
    }

    public int UpgradeSuccessCount
    {
        get
        {
            return upgradeSuccessCount;
        }
        set
        {
            upgradeSuccessCount = value;
        }
    }

    public int UpgradeFailCount
    {
        get
        {
            return upgradeFailCount;
        }
        set
        {
            upgradeFailCount = value;
        }
    }

    public int UseUpgradeTicket
    {
        get
        {
            return useUpgradeTicket;
        }
        set
        {
            useUpgradeTicket = value;
        }
    }

    public int RepairBlockCount
    {
        get
        {
            return repairBlockCount;
        }
        set
        {
            repairBlockCount = value;
        }
    }

    public int TitleNumber
    {
        get
        {
            return titleNumber;
        }
        set
        {
            titleNumber = value;
        }
    }

    public int ComicWorld2023
    {
        get
        {
            return comicWorld2023;
        }
        set
        {
            comicWorld2023 = value;
        }
    }

    public int IndieFestival2023
    {
        get
        {
            return indieFestival2023;
        }
        set
        {
            indieFestival2023 = value;
        }
    }

    public int NaverCafe202310
    {
        get
        {
            return naverCafe202310;
        }
        set
        {
            naverCafe202310 = value;
        }
    }

    public int NaverCafe202311
    {
        get
        {
            return naverCafe202311;
        }
        set
        {
            naverCafe202311 = value;
        }
    }

    public int NaverCafe202312
    {
        get
        {
            return naverCafe202312;
        }
        set
        {
            naverCafe202312 = value;
        }
    }

    public int NaverCafe202401
    {
        get
        {
            return naverCafe202401;
        }
        set
        {
            naverCafe202401 = value;
        }
    }

    public int NaverCafe202402
    {
        get
        {
            return naverCafe202402;
        }
        set
        {
            naverCafe202402 = value;
        }
    }

    public int NaverCafe202403
    {
        get
        {
            return naverCafe202403;
        }
        set
        {
            naverCafe202403 = value;
        }
    }

    public int NaverCafe202404
    {
        get
        {
            return naverCafe202404;
        }
        set
        {
            naverCafe202404 = value;
        }
    }

    public int NaverCafe202405
    {
        get
        {
            return naverCafe202405;
        }
        set
        {
            naverCafe202405 = value;
        }
    }

    public int NaverCafe202406
    {
        get
        {
            return naverCafe202406;
        }
        set
        {
            naverCafe202406 = value;
        }
    }

    public int NaverCafe202407
    {
        get
        {
            return naverCafe202407;
        }
        set
        {
            naverCafe202407 = value;
        }
    }

    public int NaverCafe202408
    {
        get
        {
            return naverCafe202408;
        }
        set
        {
            naverCafe202408 = value;
        }
    }

    public int NaverCafe202409
    {
        get
        {
            return naverCafe202409;
        }
        set
        {
            naverCafe202409 = value;
        }
    }

    public int NaverCafe202410
    {
        get
        {
            return naverCafe202410;
        }
        set
        {
            naverCafe202410 = value;
        }
    }

    public int NaverCafe202411
    {
        get
        {
            return naverCafe202411;
        }
        set
        {
            naverCafe202411 = value;
        }
    }

    public int NaverCafe202412
    {
        get
        {
            return naverCafe202412;
        }
        set
        {
            naverCafe202412 = value;
        }
    }

    public int Season
    {
        get
        {
            return season;
        }
        set
        {
            season = value;
        }
    }

    public string AttendanceDay
    {
        get
        {
            return attendanceDay;
        }
        set
        {
            attendanceDay = value;
        }
    }

    public int AttendanceCount
    {
        get
        {
            return attendanceCount;
        }
        set
        {
            attendanceCount = value;
        }
    }

    public bool AttendanceCheck
    {
        get
        {
            return attendanceCheck;
        }
        set
        {
            attendanceCheck = value;
        }
    }

    public string NextMonday
    {
        get
        {
            return nextMonday;
        }
        set
        {
            nextMonday = value;
        }
    }

    public string NextMonth
    {
        get
        {
            return nextMonth;
        }
        set
        {
            nextMonth = value;
        }
    }

    public int Update
    {
        get
        {
            return update;
        }
        set
        {
            update = value;
        }
    }

    public int WelcomeCount
    {
        get
        {
            return welcomeCount;
        }
        set
        {
            welcomeCount = value;
        }
    }

    public bool WelcomeCheck
    {
        get
        {
            return welcomeCheck;
        }
        set
        {
            welcomeCheck = value;
        }
    }

    public int WelcomeBoxCount
    {
        get
        {
            return welcomeBoxCount;
        }
        set
        {
            welcomeBoxCount = value;
        }
    }

    public bool WelcomeBoxCheck
    {
        get
        {
            return welcomeBoxCheck;
        }
        set
        {
            welcomeBoxCheck = value;
        }
    }

    public int NewbieWin
    {
        get
        {
            return newbieWin;
        }
        set
        {
            newbieWin = value;
        }
    }

    public int NewbieLose
    {
        get
        {
            return newbieLose;
        }
        set
        {
            newbieLose = value;
        }
    }

    public int GosuWin
    {
        get
        {
            return gosuWin;
        }
        set
        {
            gosuWin = value;
        }
    }

    public int GosuLose
    {
        get
        {
            return gosuLose;
        }
        set
        {
            gosuLose = value;
        }
    }

    public string Armor
    {
        get
        {
            return armor;
        }
        set
        {
            armor = value;
        }
    }

    public string Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
        }
    }

    public string Shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = value;
        }
    }

    public string Newbie
    {
        get
        {
            return newbie;
        }
        set
        {
            newbie = value;
        }
    }

    public int Emote1
    {
        get
        {
            return emote1;
        }
        set
        {
            emote1 = value;
        }
    }

    public int Emote2
    {
        get
        {
            return emote2;
        }
        set
        {
            emote2 = value;
        }
    }

    public int Emote3
    {
        get
        {
            return emote3;
        }
        set
        {
            emote3 = value;
        }
    }

    public int Emote4
    {
        get
        {
            return emote4;
        }
        set
        {
            emote4 = value;
        }
    }

    public int Emote5
    {
        get
        {
            return emote5;
        }
        set
        {
            emote5 = value;
        }
    }

    public int SnowBox
    {
        get
        {
            return snowBox;
        }
        set
        {
            snowBox = value;

            if (snowBox > 0)
            {
                eGetSnowBox();
            }
        }
    }

    public int SnowBox_N
    {
        get
        {
            return snowBox_N;
        }
        set
        {
            snowBox_N = value;

            if (snowBox_N > 0)
            {
                eGetSnowBox_N();
            }
        }
    }

    public int SnowBox_R
    {
        get
        {
            return snowBox_R;
        }
        set
        {
            snowBox_R = value;

            if (snowBox_R > 0)
            {
                eGetSnowBox_R();
            }
        }
    }

    public int SnowBox_SR
    {
        get
        {
            return snowBox_SR;
        }
        set
        {
            snowBox_SR = value;

            if (snowBox_SR > 0)
            {
                eGetSnowBox_SR();
            }
        }
    }

    public int SnowBox_SSR
    {
        get
        {
            return snowBox_SSR;
        }
        set
        {
            snowBox_SSR = value;

            if (snowBox_SSR > 0)
            {
                eGetSnowBox_SSR();
            }
        }
    }

    public int SnowBox_UR
    {
        get
        {
            return snowBox_UR;
        }
        set
        {
            snowBox_UR = value;

            if (snowBox_UR > 0)
            {
                eGetSnowBox_UR();
            }
        }
    }

    public int SnowBox_NR
    {
        get
        {
            return snowBox_NR;
        }
        set
        {
            snowBox_NR = value;

            if (snowBox_NR > 0)
            {
                eGetSnowBox_NR();
            }
        }
    }

    public int SnowBox_RSR
    {
        get
        {
            return snowBox_RSR;
        }
        set
        {
            snowBox_RSR = value;

            if (snowBox_RSR > 0)
            {
                eGetSnowBox_RSR();
            }
        }
    }

    public int SnowBox_SRSSR
    {
        get
        {
            return snowBox_SRSSR;
        }
        set
        {
            snowBox_SRSSR = value;

            if (snowBox_SRSSR > 0)
            {
                eGetSnowBox_SRSSR();
            }
        }
    }

    public int UnderworldBox
    {
        get
        {
            return underworldBox;
        }
        set
        {
            underworldBox = value;

            if (underworldBox > 0)
            {
                eGetUnderworldBox();
            }
        }
    }

    public int UnderworldBox_N
    {
        get
        {
            return underworldBox_N;
        }
        set
        {
            underworldBox_N = value;

            if (underworldBox_N > 0)
            {
                eGetUnderworldBox_N();
            }
        }
    }

    public int UnderworldBox_R
    {
        get
        {
            return underworldBox_R;
        }
        set
        {
            underworldBox_R = value;

            if (underworldBox_R > 0)
            {
                eGetUnderworldBox_R();
            }
        }
    }

    public int UnderworldBox_SR
    {
        get
        {
            return underworldBox_SR;
        }
        set
        {
            underworldBox_SR = value;

            if (underworldBox_SR > 0)
            {
                eGetUnderworldBox_SR();
            }
        }
    }

    public int UnderworldBox_SSR
    {
        get
        {
            return underworldBox_SSR;
        }
        set
        {
            underworldBox_SSR = value;

            if (underworldBox_SSR > 0)
            {
                eGetUnderworldBox_SSR();
            }
        }
    }

    public int UnderworldBox_UR
    {
        get
        {
            return underworldBox_UR;
        }
        set
        {
            underworldBox_UR = value;

            if (underworldBox_UR > 0)
            {
                eGetUnderworldBox_UR();
            }
        }
    }

    public int UnderworldBox_NR
    {
        get
        {
            return underworldBox_NR;
        }
        set
        {
            underworldBox_NR = value;

            if (underworldBox_NR > 0)
            {
                eGetUnderworldBox_NR();
            }
        }
    }

    public int UnderworldBox_RSR
    {
        get
        {
            return underworldBox_RSR;
        }
        set
        {
            underworldBox_RSR = value;

            if (underworldBox_RSR > 0)
            {
                eGetUnderworldBox_RSR();
            }
        }
    }

    public int UnderworldBox_SRSSR
    {
        get
        {
            return underworldBox_SRSSR;
        }
        set
        {
            underworldBox_SRSSR = value;

            if (underworldBox_SRSSR > 0)
            {
                eGetUnderworldBox_SRSSR();
            }
        }
    }


    public int BoxPiece_N
    {
        get
        {
            return boxPiece_N;
        }
        set
        {
            boxPiece_N = value;
        }
    }

    public int BoxPiece_R
    {
        get
        {
            return boxPiece_R;
        }
        set
        {
            boxPiece_R = value;
        }
    }

    public int BoxPiece_SR
    {
        get
        {
            return boxPiece_SR;
        }
        set
        {
            boxPiece_SR = value;
        }
    }

    public int BoxPiece_SSR
    {
        get
        {
            return boxPiece_SSR;
        }
        set
        {
            boxPiece_SSR = value;
        }
    }

    public int BoxPiece_UR
    {
        get
        {
            return boxPiece_UR;
        }
        set
        {
            boxPiece_UR = value;
        }
    }

    public int BuySnowBox
    {
        get
        {
            return buySnowBox;
        }
        set
        {
            buySnowBox = value;
        }
    }
    public int BuyUnderworldBox
    {
        get
        {
            return buyUnderworldBox;
        }
        set
        {
            buyUnderworldBox = value;
        }
    }

    public int BuySnowBoxSSRCount
    {
        get
        {
            return buySnowBoxSSRCount;
        }
        set
        {
            buySnowBoxSSRCount = value;
        }
    }

    public int BuyUnderworldBoxSSRCount
    {
        get
        {
            return buyUnderworldSSRCount;
        }
        set
        {
            buyUnderworldSSRCount = value;
        }
    }

    public int ShopNewbie
    {
        get
        {
            return shopNewbie;
        }
        set
        {
            shopNewbie = value;
        }
    }

    public int ShopSliver
    {
        get
        {
            return shopSliver;
        }
        set
        {
            shopSliver = value;
        }
    }

    public int ShopGold
    {
        get
        {
            return shopGold;
        }
        set
        {
            shopGold = value;
        }
    }

    public int ShopPlatinum
    {
        get
        {
            return shopPlatinum;
        }
        set
        {
            shopPlatinum = value;
        }
    }

    public int ShopDiamond
    {
        get
        {
            return shopDiamond;
        }
        set
        {
            shopDiamond = value;
        }
    }

    public int ShopLegend
    {
        get
        {
            return shopLegend;
        }
        set
        {
            shopLegend = value;
        }
    }

    public int ShopSupply
    {
        get
        {
            return shopSupply;
        }
        set
        {
            shopSupply = value;
        }
    }

    public int NewsAlarm
    {
        get
        {
            return newsAlarm;
        }
        set
        {
            newsAlarm = value;
        }
    }

    public int DefDestroyTicket
    {
        get
        {
            return defDestroyTicket;
        }
        set
        {
            defDestroyTicket = value;
        }
    }

    #endregion

    public void Initialize()
    {
        coin = 0;
        coinA = 0;
        coinB = 0;
        crystal = 0;
        millage = 0;

        formation = 0;
        star = 0;
        playTime = 0;
        adCount = 0;
        nowRank = 0;
        highRank = 0;
        challengeCount = 0;
        rankUpCount = 0;
        testAccount = 0;

        defDestroyTicket = 0;
        winGetMoney = 0;
        totalRaf = 0;
        synthesisGetBlock = 0;
        rankDownCount = 0;
        rankDownStreak = 0;
        winNumber = 0;
        winQueen = 0;
        goalAchieveCount = 0;
        chargingRM = 0;
        boxOpenCount = 0;
        accessDate = 0;
        upgradeSuccessCount = 0;
        upgradeFailCount = 0;
        useUpgradeTicket = 0;
        repairBlockCount = 0;
        consumeGold = 0;

        dailyWin = 0;
        dailyReward = 0;
        dailyBuy1 = 0;
        dailyBuy2 = 0;
        dailyBuyCount1 = 0;
        dailyBuyCount2 = 0;
        dailyNormalBox = 0;
        dailyEpicBox = 0;
        dailyNormalBox_1 = 0;
        dailyNormalBox_10 = 0;
        dailyEpicBox_1 = 0;
        dailyEpicBox_10 = 0;
        dailyAdsReward = 0;
        dailyAdsReward2 = 0;
        dailyAdsReward3 = 0;
        dailyGoldReward = 0;
        dailyReset = 0;

        package_Daily1 = 0;
        package_Daily2 = 0;
        package_Daily3 = 0;
        package_Daily4 = 0;
        package_Daily5 = 0;

        package_Weekly1 = 0;
        package_Weekly2 = 0;
        package_Weekly3 = 0;
        package_Weekly4 = 0;
        package_Weekly5 = 0;

        package_Monthly1 = 0;
        package_Monthly2 = 0;
        package_Monthly3 = 0;
        package_Monthly4 = 0;
        package_Monthly5 = 0;


        titleNumber = 0;

        titleNormalInformationList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(TitleNormalType)).Length; i++)
        {
            TitleNormalInformation content = new TitleNormalInformation();
            content.titleNormalType = TitleNormalType.Default + i;
            titleNormalInformationList.Add(content);
        }

        titleSpeicalInformationList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(TitleSpeicalType)).Length - 1; i++)
        {
            TitleSpeicalInformation content = new TitleSpeicalInformation();
            content.titleSpeicalType = TitleSpeicalType.TitleSpeical1 + i;
            titleSpeicalInformationList.Add(content);
        }

        comicWorld2023 = 0;
        indieFestival2023 = 0;
        naverCafe202310 = 0;
        naverCafe202311 = 0;
        naverCafe202312 = 0;
        naverCafe202401 = 0;
        naverCafe202402 = 0;
        naverCafe202403 = 0;
        naverCafe202404 = 0;
        naverCafe202405 = 0;
        naverCafe202406 = 0;
        naverCafe202407 = 0;
        naverCafe202408 = 0;
        naverCafe202409 = 0;
        naverCafe202410 = 0;
        naverCafe202411 = 0;
        naverCafe202412 = 0;

        achievementInfoList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(AchievementType)).Length; i++)
        {
            AchievementInfo content = new AchievementInfo();
            content.achievementType = AchievementType.AccessDate + i;
            achievementInfoList.Add(content);
        }

        season = 0;
        attendanceDay = "";
        attendanceCount = 0;
        attendanceCheck = false;

        welcomeCount = 0;
        welcomeCheck = false;

        welcomeBoxCount = 0;
        welcomeBoxCheck = false;

        nextMonday = "";
        nextMonth = "";
        update = 0;

        newbieWin = 0;
        newbieLose = 0;
        gosuWin = 0;
        gosuLose = 0;

        armor = "";
        weapon = "";
        shield = "";
        newbie = "";

        emote1 = 0;
        emote2 = 1;
        emote3 = 2;
        emote4 = 3;
        emote5 = 4;

        snowBox = 0;
        snowBox_N = 0;
        snowBox_R = 0;
        snowBox_SR = 0;
        snowBox_SSR = 0;
        snowBox_UR = 0;
        snowBox_NR = 0;
        snowBox_RSR = 0;
        snowBox_SRSSR = 0;

        underworldBox = 0;
        underworldBox_N = 0;
        underworldBox_R = 0;
        underworldBox_SR = 0;
        underworldBox_SSR = 0;
        underworldBox_UR = 0;
        underworldBox_NR = 0;
        underworldBox_RSR = 0;
        underworldBox_SRSSR = 0;

        boxPiece_N = 0;
        boxPiece_R = 0;
        boxPiece_SR = 0;
        boxPiece_SSR = 0;
        boxPiece_UR = 0;

        BuySnowBox = 0;
        BuyUnderworldBox = 0;
        BuySnowBoxSSRCount = 0;
        BuyUnderworldBoxSSRCount = 0;

        shopNewbie = 0;
        shopSliver = 0;
        shopGold = 0;
        shopPlatinum = 0;
        shopDiamond = 0;
        shopLegend = 0;
        shopSupply = 0;

        newsAlarm = 0;
        defDestroyTicket = 0;

        windCharacterList.Clear();

        for(int i = 0; i < System.Enum.GetValues(typeof(WindCharacterType)).Length; i ++)
        {
            WindCharacterClass content = new WindCharacterClass();
            content.windCharacterType = WindCharacterType.Winter + i;
            windCharacterList.Add(content);
        }

        blockList.Clear();
        sellBlockList.Clear();
        successionLevel.Clear();

        //for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length; i++)
        //{
        //    BlockClass content = new BlockClass();
        //    content.blockType = BlockType.Default + i + 1;
        //    blockList.Add(content);
        //}

        presentList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(PresentType)).Length; i++)
        {
            PresentClass content = new PresentClass();
            content.presentType = PresentType.A + i;
            presentList.Add(content);
        }

        upgradeTicketList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(RankType)).Length; i++)
        {
            UpgradeTicketClass content = new UpgradeTicketClass();
            content.rankType = RankType.N + i;
            upgradeTicketList.Add(content);
        }
    }

    public void Initialize_BlockList()
    {
        blockList = new List<BlockClass>();
    }

    public int GetBlockListNumber()
    {
        return blockList.Count;
    }

    public void SetBlock(ItemInstance item)
    {
        for(int i = 0; i < blockList.Count; i ++)
        {
            if(item.ItemInstanceId.Equals(blockList[i].instanceId))
            {
                return;
            }
        }

        //for(int i = 0; i < sellBlockList.Count; i ++)
        //{
        //    if(item.ItemInstanceId.Equals(sellBlockList[i]))
        //    {
        //        return;
        //    }
        //}

        BlockClass blockClass = new BlockClass();
        blockClass.blockType = (BlockType)Enum.Parse(typeof(BlockType), item.DisplayName.ToString());

        switch (item.ItemClass)
        {
            case "UR":
                blockClass.rankType = RankType.UR;
                break;
            case "SSR":
                blockClass.rankType = RankType.SSR;
                break;
            case "SR":
                blockClass.rankType = RankType.SR;
                break;
            case "R":
                blockClass.rankType = RankType.R;
                break;
            case "N":
                blockClass.rankType = RankType.N;
                break;
            default:
                blockClass.rankType = RankType.N;
                break;

        }

        blockClass.instanceId = item.ItemInstanceId;

        if(item.CustomData != null)
        {
            blockClass.level = int.Parse(item.CustomData["Level"]);

            if(item.CustomData.ContainsKey("SSRLevel"))
            {
                blockClass.ssrLevel = int.Parse(item.CustomData["SSRLevel"]);
            }
        }

        for (int i = 0; i < successionLevel.Count; i++) //레벨 계승
        {
            if(blockClass.level == 0 && 
                blockClass.blockType.Equals(successionLevel[i].blockType) && 
                blockClass.rankType.Equals(successionLevel[i].rankType))
            {
                levelCustomData.Clear();
                levelCustomData.Add("Level", successionLevel[i].level.ToString());
                levelCustomData.Add("SSRLevel", "0");

                PlayfabManager.instance.SetInventoryCustomData(blockClass.instanceId, levelCustomData);
                blockClass.level = successionLevel[i].level;

                if (successionLevel[i].equipInfo == 1)
                {
                    armor = blockClass.instanceId;

                    Debug.Log("아머로 장비가 계승되었습니다");
                }
                else if (successionLevel[i].equipInfo == 2)
                {
                    weapon = blockClass.instanceId;

                    Debug.Log("검으로 장비가 계승되었습니다");
                }
                else if (successionLevel[i].equipInfo == 3)
                {
                    shield = blockClass.instanceId;

                    Debug.Log("쉴드로 장비가 계승되었습니다");
                }
                else if (successionLevel[i].equipInfo == 4)
                {
                    newbie = blockClass.instanceId;

                    Debug.Log("뉴비로 장비가 계승되었습니다");
                }

                Debug.LogError(blockClass.blockType + "_" + blockClass.rankType + " 가 " + (successionLevel[i].level + 1) + " 레벨로 계승되었습니다");

                successionLevel.RemoveAt(i);
            }
        }

        //Debug.Log(blockClass.blockType + "_" + blockClass.rankType + " 블럭이 추가되었습니다");

        blockList.Add(blockClass);
    }

    public void SetBlockLevel(string id, int level)
    {
        for(int i = 0; i < blockList.Count; i ++)
        {
            if(blockList[i].instanceId.Equals(id))
            {
                blockList[i].level = level;
                break;
            }
        }
    }

    public void SetBlockSSRLevel(string id, int level)
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].instanceId.Equals(id))
            {
                blockList[i].ssrLevel = level;
                break;
            }
        }
    }

    public void SellBlock(string id)
    {
        sellBlockList.Add(id);
    }

    public bool CheckSellBlock(string id)
    {
        bool check = false;

        for(int i = 0; i < sellBlockList.Count; i ++)
        {
            if(sellBlockList[i].Equals(id))
            {
                check = true;
                break;
            }
        }

        return check;
    }

    public List<BlockClass> GetBlockClass()
    {
        return blockList;
    }

    public BlockClass GetBlockClass(string id)
    {
        BlockClass blockClass = new BlockClass();

        for(int i = 0; i < blockList.Count; i ++)
        {
            if(blockList[i].instanceId.Equals(id))
            {
                blockClass = blockList[i];
                break;
            }
        }

        return blockClass;
    }

    public int CheckEquip(string id)
    {
        int number = 0;

        if(!string.IsNullOrEmpty(armor))
        {
            if (armor.Equals(id))
            {
                number = 1;
            }
        }

        if (!string.IsNullOrEmpty(weapon))
        {
            if (weapon.Equals(id))
            {
                number = 2;
            }
        }

        if (!string.IsNullOrEmpty(shield))
        {
            if (shield.Equals(id))
            {
                number = 3;
            }
        }

        if (!string.IsNullOrEmpty(newbie))
        {
            if (newbie.Equals(id))
            {
                number = 4;
            }
        }

        return number;
    }

    public void CheckUnEquip(string id)
    {
        if(!string.IsNullOrEmpty(armor))
        {
            if (armor.Equals(id))
            {
                armor = "";

                Debug.Log("장착 중인 아머가 해제되었습니다");
            }
        }

        if (!string.IsNullOrEmpty(weapon))
        {
            if (weapon.Equals(id))
            {
                weapon = "";

                Debug.Log("장착 중인 검이 해제되었습니다");
            }
        }

        if (!string.IsNullOrEmpty(shield))
        {
            if (shield.Equals(id))
            {
                shield = "";

                Debug.Log("장착 중인 쉴드가 해제되었습니다");
            }
        }

        if (!string.IsNullOrEmpty(newbie))
        {
            if (newbie.Equals(id))
            {
                newbie = "";

                Debug.Log("장착 중인 뉴비가 해제되었습니다");
            }
        }
    }

    public int CheckOverlapBlock(BlockClass block)
    {
        int index = 0;

        if(!string.IsNullOrEmpty(armor))
        {
            if (GetBlockClass(armor).blockType.Equals(block.blockType))
            {
                index = 1;
            }
        }

        if (!string.IsNullOrEmpty(weapon))
        {
            if (GetBlockClass(weapon).blockType.Equals(block.blockType))
            {
                index = 2;
            }
        }

        if (!string.IsNullOrEmpty(shield))
        {
            if (GetBlockClass(shield).blockType.Equals(block.blockType))
            {
                index = 3;
            }
        }

        return index;
    }

    public void SetSuccessionLevel(BlockClass block)
    {
        successionLevel.Add(block);

        Debug.Log("계승 정보를 저장했습니다");
    }

    #region Ticket
    public void SetUpgradeTicket(RankType type, int number)
    {
        for(int i = 0; i < upgradeTicketList.Count; i ++)
        {
            if(upgradeTicketList[i].rankType.Equals(type))
            {
                upgradeTicketList[i].holdNumber += number;
                break;
            }
        }
    }

    public int GetUpgradeTicket(RankType type)
    {
        int ticket = 0;
        for (int i = 0; i < upgradeTicketList.Count; i++)
        {
            if (upgradeTicketList[i].rankType.Equals(type))
            {
                ticket = upgradeTicketList[i].holdNumber;
                break;
            }
        }
        return ticket;
    }

    public void UseUpgradeTicketCount(RankType type, int number)
    {
        for (int i = 0; i < upgradeTicketList.Count; i++)
        {
            if (upgradeTicketList[i].rankType.Equals(type))
            {
                upgradeTicketList[i].holdNumber -= number;
                break;
            }
        }
    }
    #endregion

    public int CheckEquipBlock_Gosu()
    {
        int index = 0;

        if (!string.IsNullOrEmpty(armor))
        {
            if(armor.Length > 0)
            {
                index++;
            }
        }

        if (!string.IsNullOrEmpty(weapon))
        {
            if (weapon.Length > 0)
            {
                index++;
            }
        }

        if (!string.IsNullOrEmpty(shield))
        {
            if (shield.Length > 0)
            {
                index++;
            }
        }

        return index;
    }

    public bool CheckEquipBlock_Newbie()
    {
        bool check = true;

        if(string.IsNullOrEmpty(newbie))
        {
            check = false;
        }
        else
        {
            if (newbie.Length == 0)
            {
                check = false;
            }
        }


        return check;
    }

    public bool CheckBlockLevel(int level)
    {
        bool check = false;

        for(int i = 0; i < blockList.Count; i ++)
        {
            if(blockList[i].level > level)
            {
                check = true;
                break;
            }
        }

        return check;
    }

    public int CheckSSRBlockCount()
    {
        int number = 0;

        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].rankType > RankType.SR)
            {
                number++;
            }
        }

        return number;
    }

    public int CheckBlockLevelCount()
    {
        int number = 0;

        if (!string.IsNullOrEmpty(armor))
        {
            if (GetBlockClass(armor).level > 0)
            {
                number++;
            }
        }

        if (!string.IsNullOrEmpty(weapon))
        {
            if (GetBlockClass(weapon).level > 0)
            {
                number++;
            }
        }

        if (!string.IsNullOrEmpty(shield))
        {
            if (GetBlockClass(shield).level > 0)
            {
                number++;
            }
        }

        return number;
    }

    public void SetAchievementInfo(AchievementType type, int count)
    {
        for(int i = 0; i < achievementInfoList.Count; i ++)
        {
            if(achievementInfoList[i].achievementType.Equals(type))
            {
                achievementInfoList[i].count = count;
                break;
            }
        }
    }

    public AchievementInfo GetAchievementInfo(AchievementType type)
    {
        AchievementInfo achievementInfo = new AchievementInfo();

        for(int i = 0; i < achievementInfoList.Count; i ++)
        {
            if(achievementInfoList[i].achievementType.Equals(type))
            {
                achievementInfo = achievementInfoList[i];
                break;
            }
        }
        return achievementInfo;
    }

    public void SetAchievementInfoCount(AchievementType type)
    {
        for (int i = 0; i < achievementInfoList.Count; i++)
        {
            if (achievementInfoList[i].achievementType.Equals(type))
            {
                achievementInfoList[i].count += 1;
                break;
            }
        }
    }

    public int GetAchievementCount(AchievementType type)
    {
        int number = 0;

        switch (type)
        {
            case AchievementType.AccessDate:
                number = accessDate;
                break;
            case AchievementType.GosuWin:
                number = gosuWin;
                break;
            case AchievementType.DestroyBlockCount:
                number = destroyBlockCount;
                break;
            case AchievementType.WinGetMoney:
                number = winGetMoney;
                break;
            case AchievementType.TotalRaf:
                number = totalRaf;
                break;
            case AchievementType.RankDownCount:
                number = rankDownCount;
                break;
            case AchievementType.WinNumber:
                number = winNumber;
                break;
            case AchievementType.WinQueen:
                number = winQueen;
                break;
            case AchievementType.ChargingRM:
                number = chargingRM;
                break;
            case AchievementType.BoxOpenCount:
                number = boxOpenCount;
                break;
            case AchievementType.UpgradeSuccessCount:
                number = upgradeSuccessCount;
                break;
            case AchievementType.UpgradeFailCount:
                number = upgradeFailCount;
                break;
            case AchievementType.UseUpgradeTicket:
                number = useUpgradeTicket;
                break;
            case AchievementType.RepairBlockCount:
                number = repairBlockCount;
                break;
        }

        return number;
    }

    public int CheckNormalTitle(TitleNormalType type)
    {
        int check = 0;

        for(int i = 0; i < titleNormalInformationList.Count; i ++)
        {
            if(titleNormalInformationList[i].titleNormalType.Equals(type))
            {
                check = titleNormalInformationList[i].check;
                break;
            }
        }

        return check;
    }

    public int CheckSpeicalTitle(TitleSpeicalType type)
    {
        int check = 0;

        for (int i = 0; i < titleNormalInformationList.Count; i++)
        {
            if (titleSpeicalInformationList[i].titleSpeicalType.Equals(type))
            {
                check = titleSpeicalInformationList[i].check;
                break;
            }
        }

        return check;
    }

    public void SetNormalTitle(TitleNormalType type)
    {
        for (int i = 0; i < titleNormalInformationList.Count; i++)
        {
            if (titleNormalInformationList[i].titleNormalType.Equals(type))
            {
                titleNormalInformationList[i].check = 1;
                break;
            }
        }

        eGetNormalTitle();
    }

    public void SetSpeicalTitle(TitleSpeicalType type)
    {
        for (int i = 0; i < titleSpeicalInformationList.Count; i++)
        {
            if (titleSpeicalInformationList[i].titleSpeicalType.Equals(type))
            {
                titleSpeicalInformationList[i].check = 1;
                break;
            }
        }

        eGetSpeicalTitle();
    }

    public string GetTitleName()
    {
        string name = "";

        switch(titleNumber)
        {
            case 0:
                name = LocalizationManager.instance.GetString("-");
                break;
            default:
                if(titleNumber < 500)
                {
                    name = LocalizationManager.instance.GetString("Title" + titleNumber);
                }
                else
                {
                    name = LocalizationManager.instance.GetString("TitleSpeical" + (titleNumber - 499));
                }
                break;
        }

        return name;
    }

    public string GetTitleName(int number)
    {
        string name = "";

        switch (number)
        {
            case 0:
                name = LocalizationManager.instance.GetString("-");
                break;
            default:
                if (number < 500)
                {
                    name = LocalizationManager.instance.GetString("Title" + number);
                }
                else
                {
                    name = LocalizationManager.instance.GetString("TitleSpeical" + (number - 499));
                }
                break;
        }

        return name;
    }

    public string GetMainTitleName()
    {
        string name = "";

        switch (titleNumber)
        {
            case 0:
                name = "-";
                break;
            default:
                if (titleNumber < 500)
                {
                    name = "Title" + titleNumber;
                }
                else
                {
                    name = "TitleSpeical" + (titleNumber - 499);
                }
                break;
        }

        return name;
    }

    public string GetMainTitleName(int number)
    {
        string name = "";

        switch (number)
        {
            case 0:
                name = "-";
                break;
            default:
                if (number < 500)
                {
                    name = "Title" + number;
                }
                else
                {
                    name = "TitleSpeical" + (number - 499);
                }
                break;
        }

        return name;
    }

    public int GetTitleHoldNumber()
    {
        int number = 0;

        for (int i = 0; i < titleNormalInformationList.Count; i++)
        {
            if (titleNormalInformationList[i].check > 0)
            {
                number++;
            }
        }

        for (int i = 0; i < titleSpeicalInformationList.Count; i++)
        {
            if (titleSpeicalInformationList[i].check > 0)
            {
                number++;
            }
        }

        return number;
    }
}