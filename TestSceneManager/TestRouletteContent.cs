using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestRouletteContent : MonoBehaviour
{
    public RouletteType rouletteType = RouletteType.Default;
    public int index = 0;

    private int money = 0;
    private int bettingValue = 0;

    private bool maxBetting = false;

    public GameObject main;
    public Text nameText;
    public Image backgroundImg;

    public TestMoneyContent moneyContent;

    public TestGameManager gameManager;

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

    public void Initialize(TestGameManager manager, RouletteType type, int number)
    {
        gameManager = manager;
        rouletteType = type;
        index = number;

        nameText.text = index.ToString();

        SetStraightBet(RouletteColorType.Black);

        for (int i = 1; i < redIndex.Length; i++)
        {
            if (index == redIndex[i])
            {
                SetStraightBet(RouletteColorType.Red);
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
                SetStraightBet(RouletteColorType.Black);

                for (int i = 1; i < redIndex.Length; i ++)
                {
                    if(index == redIndex[i])
                    {
                        SetStraightBet(RouletteColorType.Red);
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
            case RouletteType.LineBet:
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
                SetStraightBet(RouletteColorType.Red);
                break;
            case RouletteType.BlackColorBet:
                nameText.text = "";
                SetStraightBet(RouletteColorType.Black);
                break;
        }
    }

    void SetStraightBet(RouletteColorType type)
    {
        switch (type)
        {
            case RouletteColorType.Red:
                backgroundImg.color = new Color(1, 0, 0);
                break;
            case RouletteColorType.Black:
                backgroundImg.color = new Color(0, 0, 0);
                break;
        }
    }

    public void OnClick()
    {
        gameManager.Betting(rouletteType, index);
    }

    public void ResetBettingMoney()
    {
        moneyContent.gameObject.SetActive(false);

        money = 0;
        maxBetting = false;
    }

    public void SetBettingMoney(TestMoneyType type)
    {
        bettingValue = 0;

        switch (type)
        {
            case TestMoneyType.One:
                bettingValue = 1;
                break;
            case TestMoneyType.Two:
                bettingValue = 2;
                break;
            case TestMoneyType.Three:
                bettingValue = 5;
                break;
            case TestMoneyType.Four:
                bettingValue = 10;
                break;
            case TestMoneyType.Five:
                bettingValue = 25;
                break;
            case TestMoneyType.Six:
                bettingValue = 50;
                break;
            case TestMoneyType.Seven:
                bettingValue = 100;
                break;
            case TestMoneyType.Eight:
                bettingValue = 500;
                break;
            case TestMoneyType.Nine:
                bettingValue = 1000;
                break;
        }

        if (maxBetting)
        {
            NotionManager.instance.UseNotion(NotionType.MaxBetting);
            return;
        }
        else
        {
            moneyContent.gameObject.SetActive(true);

            gameManager.SetBettingMoney(bettingValue);

            if (money + bettingValue > 1000)
            {
                gameManager.ReturnBettingMoney((money + bettingValue) - 1000);

                money = 1000;

                maxBetting = true;
            }
            else
            {
                money += bettingValue;
            }

            moneyContent.SetBettingMoney(money);
        }
    }

    public void DoubleBetting()
    {
        if(money <= 0 || money >= 1000 || money + money >= 1000)
        {
            return;
        }

        if(TestGameManager.instance.saveBetMoney + money > 5000)
        {
            NotionManager.instance.UseNotion(NotionType.MaxBetting);
            return;
        }

        if (TestGameManager.instance.money - money < 0)
        {
            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
            return;
        }

        gameManager.SetBettingMoney(money);

        money = money * 2;

        moneyContent.SetBettingMoney(money);
    }
}
