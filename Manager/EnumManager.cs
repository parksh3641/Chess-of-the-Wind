using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : MonoBehaviour
{
}
public enum RouletteType
{
    Default = 0,
    StraightBet,
    SplitBet,
    StreetBet,
    SquareBet,
    FiveNumberBet,
    LineBet,
    ColumnBet,
    DozenBet,
    LowNumberBet,
    HighNumberBet,
    EvenNumberBet,
    OddNumberBet,
    RedColorBet,
    BlackColorBet
}

public enum BetMoneyColorType
{
    Red = 0,
    Black
}

public enum MoneyType
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

public enum BetOptionType
{
    Double,
    Cancle,
    Repeat
}