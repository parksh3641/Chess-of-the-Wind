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
    Choice_N,
    Choice_R,
    Choice_SR,
    Choice_SSR,
    Choice_UR
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
    Stroy_Under,
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
    GetBlock, //???? ??????
    PlusMoney1,
    PlusMoney2,
    MinusMoney,
    ChangeMoney, //?? ???? ????
    UseEmotion,
    SetBlock, //???? ????
    TimesUp,
    GameWin,
    ResultMoney, //???? ?????????? ?? ???????? ????
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
    Wrong
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
    RemoveAds,
    WatchAd,
    DailyReward,
    UpgradeTicket_N,
    UpgradeTicket_R,
    UpgradeTicket_SR,
    UpgradeTicket_SSR,
    UpgradeTicket_UR,
    DefDestroyTicket,
    PresentA,
    PresentB,
    PresentC,
    PresentD,
    PresentE,
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
    SameEquipBlock,
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
    NotEnoughDefTicket,
    GetGradeNTicket,
    GetGradeRTicket,
    GetGradeSRTicket,
    GetGradeSSRTicket,
    GetGradeURTicket,
    GetGradeDefTicket,
    EquipSameBlock,
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