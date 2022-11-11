using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteContent : MonoBehaviour
{
    public RouletteType rouletteType = RouletteType.Default;
    public int index = 0;

    private int bettingValue = 0;

    public GameObject main;
    public Text nameText;
    public Image backgroundImg;

    public MoneyContent moneyContent;

    public GameManager gameManager;

    private int[] redIndex = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };

    private void Awake()
    {
        moneyContent.gameObject.SetActive(false);
    }

    private void Start()
    {
        nameText.color = new Color(1, 1, 1);
        Initialize();
    }

    public void Initialize(GameManager manager, RouletteType type, int number)
    {
        gameManager = manager;
        rouletteType = type;
        index = number;

        nameText.text = index.ToString();

        SetStraightBet(ColorType.Black);

        for (int i = 1; i < redIndex.Length; i++)
        {
            if (index == redIndex[i])
            {
                SetStraightBet(ColorType.Red);
                break;
            }
        }
    }

    void Initialize()
    {
        switch (rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                nameText.text = index.ToString();
                SetStraightBet(ColorType.Black);

                for (int i = 1; i < redIndex.Length; i ++)
                {
                    if(index == redIndex[i])
                    {
                        SetStraightBet(ColorType.Red);
                        break;
                    }
                }
                break;
            case RouletteType.SplitBet:
                backgroundImg.color = new Color(0, 0, 0, 1 / 255f);
                main.SetActive(false);
                break;
            case RouletteType.StreetBet:
                backgroundImg.color = new Color(0, 0, 0, 1 / 255f);
                main.SetActive(false);
                break;
            case RouletteType.SquareBet:
                backgroundImg.color = new Color(0, 0, 0, 1 / 255f);
                main.SetActive(false);
                break;
            case RouletteType.FiveNumberBet:
                backgroundImg.color = new Color(0, 0, 0, 1 / 255f);
                main.SetActive(false);
                break;
            case RouletteType.SixNumberBet:
                backgroundImg.color = new Color(0, 0, 0, 1 / 255f);
                main.SetActive(false);
                break;
            case RouletteType.ColumnBet:
                nameText.text = "2:1";
                break;
            case RouletteType.DozenBet:
                switch(index)
                {
                    case 0:
                        nameText.text = "1st - 12";
                        break;
                    case 1:
                        nameText.text = "2st - 12";
                        break;
                    case 2:
                        nameText.text = "3st - 12";
                        break;
                }
                break;
            case RouletteType.LowNumberBet:
                nameText.text = "1 - 18";
                break;
            case RouletteType.HighNumberBet:
                nameText.text = "19 - 36";
                break;
            case RouletteType.EvenNumberBet:
                nameText.text = "EVEN";
                break;
            case RouletteType.OddNumberBet:
                nameText.text = "ODD";
                break;
            case RouletteType.RedColorBet:
                nameText.text = "";
                SetStraightBet(ColorType.Red);
                break;
            case RouletteType.BlackColorBet:
                nameText.text = "";
                SetStraightBet(ColorType.Black);
                break;
        }
    }

    void SetStraightBet(ColorType type)
    {
        switch (type)
        {
            case ColorType.Red:
                backgroundImg.color = new Color(1, 0, 0);
                break;
            case ColorType.Black:
                backgroundImg.color = new Color(0, 0, 0);
                break;
        }
    }

    public void OnClick()
    {
        gameManager.Betting(rouletteType, index);
    }

    public void SetBettingMoney(MoneyType type)
    {
        moneyContent.gameObject.SetActive(true);

        switch (type)
        {
            case MoneyType.One:
                bettingValue += 1;
                break;
            case MoneyType.Two:
                bettingValue += 2;
                break;
            case MoneyType.Three:
                bettingValue += 5;
                break;
            case MoneyType.Four:
                bettingValue += 10;
                break;
            case MoneyType.Five:
                bettingValue += 25;
                break;
            case MoneyType.Six:
                bettingValue += 50;
                break;
            case MoneyType.Seven:
                bettingValue += 100;
                break;
            case MoneyType.Eight:
                bettingValue += 500;
                break;
            case MoneyType.Nine:
                bettingValue += 1000;
                break;
        }

        if (bettingValue >= 5000)
        {
            bettingValue = 5000;
        }

        moneyContent.SetBettingMoney(bettingValue);
    }
}
