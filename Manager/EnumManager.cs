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
    Pawn,
    Spider,
    Tetris_I,
    Tetris_I_Hor,
    Tetris_T,
    Tetris_L,
    Tetris_J,
    Tetris_S,
    Tetris_Z,
    Tetris_Speical,
}

public enum BetOptionType
{
    Cancle,
    Double,
    Repeat
}

public enum GameSfxType
{
    Click,
    Roulette
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
    Cancle,
    GoBetting,
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
    LimitMaxBlock
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
}