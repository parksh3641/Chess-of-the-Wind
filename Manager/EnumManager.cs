using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : MonoBehaviour
{
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
    Random,
    N,
    R,
    SR,
    SSR,
    UR,
    NR,
    RSR,
    SRSSR
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
    Rook_V2H2,
    Pawn_Under,
    Pawn_Snow,
    Spider,
    Rook_V4,
    Tetris_I_Hor,
    Tetris_T,
    Tetris_L,
    Tetris_J,
    Tetris_S,
    Tetris_Z,
    Tetris_Speical,
    RightNight_Mirror,
    LeftNight_Mirror,
    Rook_V4_2,
    Rook_V2_2,
    Pawn_Under_2,
    Pawn_Snow_2,
}

public enum BetOptionType
{
    Cancle,
    Double,
    Repeat
}

public enum OptionType
{
    Music,
    Sfx,
    Vibration,
    SleepMode
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
    Bomb
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
    Dutch
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
    Gold = 0,
    Crystal
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
    BettingCancle,
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
    GetReward
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
    Legend_1
}

public enum RewardType
{
    Gold,
    UpgradeTicket,
    Box,
    Box_N,
    Box_R,
    Box_SR,
    Box_SSR,
    Box_UR,
    Box_NR,
    Box_RSR,
    Box_SRSSR,

}