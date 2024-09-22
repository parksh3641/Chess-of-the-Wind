using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : MonoBehaviour
{

}

public enum StoreType
{
    None,
    Google,
    Apple,
    OneStore,
    Amazon,
}

public enum GameType
{
    NewBie,
    Gosu
}

public enum WindCharacterType
{
    Winter,
    UnderWorld
}

public enum BoxType
{
    Normal,
    //N,
    //R,
    //SR,
    //SSR,
    //UR,
    //NR,
    //RSR,
    //SRSSR,
    Epic,
    Speical
}

public enum PresentType
{
    A,
    B,
    C,
    D,
    E
}

public enum RankType
{
    N,
    R,
    SR,
    SSR,
    UR
}

public enum RouletteType
{
    Default = 0,
    StraightBet,
    SplitBet_Horizontal,
    SplitBet_Vertical,
    SquareBet,
}

public enum RouletteColorType
{
    White = 0,
    Black,
    Yellow
}

public enum TestMoneyType
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine
}

public enum BlockType
{
    Default,
    RightQueen_2,
    LeftQueen_2,
    RightQueen_3,
    LeftQueen_3,
    RightNight,
    LeftNight,
    RightDownNight,
    LeftDownNight,
    Rook_V2,
    Pawn_Under,
    Pawn_Snow,
    Rook_V4,
    RightNight_Mirror,
    LeftNight_Mirror,
    Rook_V4_2,
    Rook_V2_2,
    Pawn_Under_2,
    Pawn_Snow_2,
}

public enum ShopUIType
{
    Default,
    Gold1,
    Gold2,
    UpgradeTicket1,
    UpgradeTicket2
}

public enum BoxInfoType
{
    RightQueen_2_N,
    RightQueen_3_N,
    RightNight_N,
    RightNight_Mirror_N,
    Rook_V2_N,
    Rook_V2_2_N,
    Pawn_Under_N,
    Pawn_Under_2_N,

    RightQueen_2_R,
    RightQueen_3_R,
    RightNight_R,
    RightNight_Mirror_R,
    Rook_V2_R,
    Rook_V2_2_R,
    Pawn_Under_R,
    Pawn_Under_2_R,

    RightQueen_2_SR,
    RightQueen_3_SR,
    RightNight_SR,
    RightNight_Mirror_SR,
    Rook_V2_SR,
    Rook_V2_2_SR,
    Pawn_Under_SR,
    Pawn_Under_2_SR,

    RightQueen_2_SSR,
    RightQueen_3_SSR,
    RightNight_SSR,
    RightNight_Mirror_SSR,
    Rook_V2_SSR,
    Rook_V2_2_SSR,
    Pawn_Under_SSR,
    Pawn_Under_2_SSR,

    LeftQueen_2_N,
    LeftQueen_3_N,
    LeftNight_N,
    LeftNight_Mirror_N,
    Rook_V4_N,
    Rook_V4_2_N,
    Pawn_Snow_N,
    Pawn_Snow_2_N,

    LeftQueen_2_R,
    LeftQueen_3_R,
    LeftNight_R,
    LeftNight_Mirror_R,
    Rook_V4_R,
    Rook_V4_2_R,
    Pawn_Snow_R,
    Pawn_Snow_2_R,

    LeftQueen_2_SR,
    LeftQueen_3_SR,
    LeftNight_SR,
    LeftNight_Mirror_SR,
    Rook_V4_SR,
    Rook_V4_2_SR,
    Pawn_Snow_SR,
    Pawn_Snow_2_SR,

    LeftQueen_2_SSR,
    LeftQueen_3_SSR,
    LeftNight_SSR,
    LeftNight_Mirror_SSR,
    Rook_V4_SSR,
    Rook_V4_2_SSR,
    Pawn_Snow_SSR,
    Pawn_Snow_2_SSR,

    Gold_N,
    Gold_R,
    UpgradeTicket_N,
    UpgradeTicket_R
}

public enum BetOptionType
{
    Cancel,
    Double,
    Repeat
}

public enum OptionType
{
    Music,
    Sfx,
    Vibration,
    SleepMode,
    Graphics
}

public enum GameBgmType
{
    Main_Snow,
    Main_Under,
    Story_Snow,
    Story_Under,
    Game_Newbie,
    Game_Gosu
}
public enum GameSfxType
{
    Click,
    Roulette,
    BlockEquip,
    BlockUpgradeReady,
    BlockUpgradeSuccess,
    BlockUpgradeFail,
    BlockSynthesisSuccess,
    BuyShopItem,
    BoxOpen,
    GetBlock,
    PlusMoney1,
    PlusMoney2,
    MinusMoney,
    ChangeMoney,
    UseEmotion,
    SetBlock,
    TimesUp,
    GameWin,
    ResultMoney,
    GameLose,
    BounsRoulette,
    GetNumber,
    GetQueen,
    RankUp,
    ChoiceWinter,
    ChoiceUnder,
    TalkWinter,
    TalkUnder,
    TalkEnemy,
    Wrong,
    RankDown,
    Success,
    BlockSell,
    BlowWind,
    BoxOpen2,
    GetStar,
    LoseStar,
    Bomb,
    TalkMy
}

public enum LanguageType
{
    Default = 0,
    Korean,
    English,
    Japanese,
    Chinese,
    Indian,
    Portuguese,
    Russian,
    German,
    Spanish,
    Arabic,
    Bengali,
    Indonesian,
    Italian,
    Dutch,
    Vietnamese,
    Thai
}
public enum LoginType
{
    None = 0,
    Guest,
    Google,
    Facebook,
    Apple
}

public enum MoneyType
{
    CoinA = 0,
    Crystal,
    Millage,
    CoinB
}

public enum ItemType
{
    Clock = 0,
    Shield,
    Combo,
    Exp,
    Slow
}

public enum ShopType
{
    DailyReward,
    DailyReward_WatchAd,
    UpgradeTicket,

}

public enum NotionType
{
    Test,
    NotEnoughMoney,
    NotBettingLocation,
    BettingCancel,
    BettingTimesUp,
    YourTurn,
    BuyTicket,
    UpgradeSuccess,
    UpgradeKeep,
    UpgradeDown,
    UpgradeDestroy,
    SellBlock,
    MaxBlockLevel,
    DontSellEquipBlock,
    NotEnoughTicket,
    DefDestroy,
    LockedMode,
    LimitMaxBlock,
    OverBettingBlock,
    OnlyPawn,
    BettingAllin,
    CheckInternet,
    NickNameNotion1,
    NickNameNotion2,
    NickNameNotion3,
    NickNameNotion4,
    NickNameNotion5,
    NickNameNotion6,
    CopyIdNotion,
    NotSynthesisBlock,
    RestorePurchaseNotion,
    EnemyTurn,
    NeedEquipBlock,
    NotSellBlock,
    BuyShopItem,
    NotBuyDailyLimit,
    GetUpgradeTicket,
    EquipSameBlock,
    LimitRank,
    InGameBurning,
    InGameBurning2,
    GetFreeReward,
    GetWatchAdReward,
    GetReward,
    NotRewardDailyLimit,
    LowRank,
    LowPiece,
    CancelPurchase,
    CancelWatchAd,
    HighLevelLimit,
    InGameBurning3,
    CouponNotion1,
    CouponNotion2,
    CouponNotion3,
    LowAllowMoney,
    CouponNotion4,
    EquipTitle,
    NewTitleNotion,
    AlreadyLink,
    SuccessLink,
    FailLink,
    WaitTimeNotion,
    ComingSoon,
    ExchangeNotion,
    DisconnectServerNotion,
    TryAgainNotion
}

public enum GameRankType
{
    Bronze_4,
    Bronze_3,
    Bronze_2,
    Bronze_1,
    Sliver_4,
    Sliver_3,
    Sliver_2,
    Sliver_1,
    Gold_4,
    Gold_3,
    Gold_2,
    Gold_1,
    Platinum_4,
    Platinum_3,
    Platinum_2,
    Platinum_1,
    Diamond_4,
    Diamond_3,
    Diamond_2,
    Diamond_1,
    Legend_4,
    Legend_3,
    Legend_2,
    Legend_1,
    Trials_4,
    Trials_3,
}

public enum PackageType
{
    Default,
    Newbie,
    Sliver,
    Gold,
    Platinum,
    Diamond,
    Legend,
    Supply,
    Trials,
    NewRank,
    NewRank2,
    NewRank3,
    Daily1,
    Daily2,
    Daily3,
    Daily4,
    Daily5,
    Weekly1,
    Weekly2,
    Weekly3,
    Weekly4,
    Weekly5,
    Monthly1,
    Monthly2,
    Monthly3,
    Monthly4,
    Monthly5,
    ShopDaily1,
    ShopDaily2,
    ShopDaily3,
    ShopWeekly1,
    ShopWeekly2,
    ShopWeekly3,
    RemoveAds
}

public enum RewardType
{
    Gold,
    UpgradeTicket,
    Box_Normal,
    Box_Epic,
    Box_Speical,
    ExclusiveTitle,
    None,
    GoldShop1,
    GoldShop2,
    GoldShop3,
    RemoveAds
}

public enum AchievementType
{
    AccessDate,
    GosuWin,
    DestroyBlockCount,
    WinGetMoney,
    TotalRaf,
    RankDownCount,
    WinNumber,
    WinQueen,
    ChargingRM,
    BoxOpenCount,
    UpgradeSuccessCount,
    UpgradeFailCount,
    UseUpgradeTicket,
    RepairBlockCount
}

public enum TitleNormalType
{
    Default,
    Title1,
    Title2,
    Title3,
    Title4,
    Title5,
    Title6,
    Title7,
    Title8,
    Title9,
    Title10,
    Title11,
    Title12,
    Title13,
    Title14,
    Title15,
    Title16,
    Title17,
    Title18,
    Title19,
    Title20,
    Title21,
    Title22,
    Title23,
    Title24,
    Title25,
    Title26,
    Title27,
    Title28,
    Title29,
    Title30,
    Title31,
    Title32,
    Title33,
    Title34,
    Title35,
    Title36,
    Title37,
    Title38,
    Title39,
    Title40,
    Title41,
    Title42,
    Title43,
    Title44,
    Title45,
    Title46,
    Title47,
    Title48,
    Title49,
    Title50,
    Title51,
    Title52,
    Title53,
    Title54,
    Title55,
    Title56,
    Title57,
    Title58,
    Title59,
    Title60,
    Title61,
    Title62,
    Title63,
    Title64,
    Title65,
    Title66,
    Title67,
    Title68,
    Title69,
    Title70,
    Title71,
    Title72,
    Title73,
    Title74,
    Title75,
    Title76,
    Title77,
    Title78,
    Title79,
    Title80,

}

public enum TitleSpeicalType
{
    Default,
    TitleSpeical1,
    TitleSpeical2,
    TitleSpeical3,
    TitleSpeical4,
    TitleSpeical5,
    TitleSpeical6,
    TitleSpeical7,
}

public enum GameEventType
{
    GameEvent1,
    GameEvent2,
    GameEvent3,
    GameEvent4,
    GameEvent5,
    GameEvent6,
}

public enum EmoteType
{
    Emote1,
    Emote2,
    Emote3,
    Emote4,
    Emote5,
    Emote6,
}

public enum ResetType
{
    DailyWin,
    DailyStar,
    DailyReward,
    DailyBuy1,
    DailyBuy2,
    DailyBuyCount1,
    DailyBuyCount2,
    DailyNormalBox,
    DailyEpicBox,
    DailyNormalBox_1,
    DailyNormalBox_10,
    DailyEpicBox_1,
    DailyEpicBox_10,
    DailyAdsReward,
    DailyAdsReward2,
    DailyAdsReward3,
    DailyGoldReward,
    DailyReset,
    Package_Daily1,
    Package_Daily2,
    Package_Daily3,
    Package_Daily4,
    Package_Daily5,
    Package_Weekly1,
    Package_Weekly2,
    Package_Weekly3,
    Package_Weekly4,
    Package_Weekly5,
    Package_Monthly1,
    Package_Monthly2,
    Package_Monthly3,
    Package_Monthly4,
    Package_Monthly5,
    Package_ShopDaily1,
    Package_ShopDaily2,
    Package_ShopDaily3,
    Package_ShopWeekly1,
    Package_ShopWeekly2,
    Package_ShopWeekly3,
}

public enum SeasonPassType
{
    Free,
    Pass
}