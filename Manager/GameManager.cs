using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public MoneyType moneyType = MoneyType.One;

    public RouletteContent rouletteContent;
    public MoneyContent moneyContent;

    public RectTransform rouletteContentTransform;
    public RectTransform splitContentTransform;
    public RectTransform streetContentTransform;
    public RectTransform moneyContentTransform;

    public RouletteContent[] rouletteContents;

    List<RouletteContent> rouletteContentList = new List<RouletteContent>();
    List<RouletteContent> splitContentList = new List<RouletteContent>();
    List<RouletteContent> streetContentList = new List<RouletteContent>();
    List<MoneyContent> moneyContentList = new List<MoneyContent>();

    public Text timerText;
    public Image timerFillAmount;

    public Text moneyText;

    public Text targetText;

    public Text recordText;

    private int time = 0;
    private float money = 0;

    private int[] redIndex = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };


    private void Awake()
    {
        moneyText.text = "₩ 0";
        targetText.text = "?";
        recordText.text = "";

        for(int i = 0; i < 37; i ++)
        {
            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = rouletteContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, RouletteType.StraightBet, i + 1);
            rouletteContentList.Add(content);
        }

        for (int i = 0; i < 33; i++)
        {
            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = splitContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, RouletteType.SplitBet, i);
            splitContentList.Add(content);
        }

        for (int i = 0; i < 36; i++)
        {
            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = streetContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;

            switch(i)
            {
                case 0:
                    content.Initialize(this, RouletteType.FiveNumberBet, 0);
                    break;
                case 1:
                    content.Initialize(this, RouletteType.StreetBet, 0);
                    break;
                case 2:
                    content.Initialize(this, RouletteType.StreetBet, 1);
                    break;
                case 3:
                    content.Initialize(this, RouletteType.SixNumberBet, 0);
                    break;
                case 4:
                    content.Initialize(this, RouletteType.SquareBet, 0);
                    break;
                case 5:
                    content.Initialize(this, RouletteType.SquareBet, 1);
                    break;
                case 6:
                    content.Initialize(this, RouletteType.SixNumberBet, 1);
                    break;
                case 7:
                    content.Initialize(this, RouletteType.SquareBet, 2);
                    break;
                case 8:
                    content.Initialize(this, RouletteType.SquareBet, 3);
                    break;
                case 9:
                    content.Initialize(this, RouletteType.SixNumberBet, 2);
                    break;
                case 10:
                    content.Initialize(this, RouletteType.SquareBet, 4);
                    break;
                case 11:
                    content.Initialize(this, RouletteType.SquareBet, 5);
                    break;
                case 12:
                    content.Initialize(this, RouletteType.SixNumberBet, 3);
                    break;
                case 13:
                    content.Initialize(this, RouletteType.SquareBet, 6);
                    break;
                case 14:
                    content.Initialize(this, RouletteType.SquareBet, 7);
                    break;
                case 15:
                    content.Initialize(this, RouletteType.SixNumberBet, 4);
                    break;
                case 16:
                    content.Initialize(this, RouletteType.SquareBet, 8);
                    break;
                case 17:
                    content.Initialize(this, RouletteType.SquareBet, 9);
                    break;
                case 18:
                    content.Initialize(this, RouletteType.SixNumberBet, 5);
                    break;
                case 19:
                    content.Initialize(this, RouletteType.SquareBet, 10);
                    break;
                case 20:
                    content.Initialize(this, RouletteType.SquareBet, 11);
                    break;
                case 21:
                    content.Initialize(this, RouletteType.SixNumberBet, 6);
                    break;
                case 22:
                    content.Initialize(this, RouletteType.SquareBet, 12);
                    break;
                case 23:
                    content.Initialize(this, RouletteType.SquareBet, 13);
                    break;
                case 24:
                    content.Initialize(this, RouletteType.SixNumberBet, 7);
                    break;
                case 25:
                    content.Initialize(this, RouletteType.SquareBet, 14);
                    break;
                case 26:
                    content.Initialize(this, RouletteType.SquareBet, 15);
                    break;
                case 27:
                    content.Initialize(this, RouletteType.SixNumberBet, 8);
                    break;
                case 28:
                    content.Initialize(this, RouletteType.SquareBet, 16);
                    break;
                case 29:
                    content.Initialize(this, RouletteType.SquareBet, 17);
                    break;
                case 30:
                    content.Initialize(this, RouletteType.SixNumberBet, 9);
                    break;
                case 31:
                    content.Initialize(this, RouletteType.SquareBet, 18);
                    break;
                case 32:
                    content.Initialize(this, RouletteType.SquareBet, 19);
                    break;
                case 33:
                    content.Initialize(this, RouletteType.SixNumberBet, 10);
                    break;
                case 34:
                    content.Initialize(this, RouletteType.SquareBet, 20);
                    break;
                case 35:
                    content.Initialize(this, RouletteType.SquareBet, 21);
                    break;

            }
            streetContentList.Add(content);
        }

        for (int i = 0; i < 9; i ++)
        {
            MoneyContent content = Instantiate(moneyContent);
            content.transform.parent = moneyContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, moneyType + i);
            moneyContentList.Add(content);
        }
    }

    private void Start()
    {
        money = 50000;
        moneyText.text = "₩ " + money.ToString("N2");

        ChangeBettingMoney(MoneyType.One);

        timerFillAmount.fillAmount = 1;
        time = 16;
        StartCoroutine(Timer());
    }


    public void ChangeBettingMoney(MoneyType type)
    {
        Debug.Log("ChangeBettingMoney : " + type.ToString());
        moneyType = type;

        for (int i = 0; i < moneyContentList.Count; i ++)
        {
            moneyContentList[i].ChangeBetAnimation(false);
        }

        moneyContentList[(int)type].ChangeBetAnimation(true);
    }

    IEnumerator Timer()
    {
        if(time <= 0)
        {
            StartCoroutine(RandomTargetNumber());
            yield break;
        }

        targetText.text = "?";

        time -= 1;

        timerFillAmount.fillAmount = time / 15.0f;
        timerText.text = time.ToString();

        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer());
    }

    IEnumerator RandomTargetNumber()
    {
        int random = Random.Range(0, 37);
        string target = "";

        targetText.color = new Color(0, 0, 0);
        target = "<color=#000000>" + random.ToString() + "</color>";

        for (int i = 1; i < redIndex.Length; i++)
        {
            if (random == redIndex[i])
            {
                targetText.color = new Color(1, 0, 0);
                target = "<color=#ff0000>" + random.ToString() + "</color>";
                break;
            }
        }

        recordText.text += target + ", ";
        targetText.text = random.ToString();

        yield return new WaitForSeconds(3f);

        targetText.color = new Color(0, 0, 0);

        timerFillAmount.fillAmount = 1;
        time = 16;
        StartCoroutine(Timer());
    }

    public void Betting(RouletteType rouletteType, int number)
    {
        Debug.Log(rouletteType.ToString() + " : " + number);

        switch (rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:

                if(number == 0)
                {
                    rouletteContents[0].SetBettingMoney(moneyType);
                }
                else
                {
                    rouletteContentList[number - 1].SetBettingMoney(moneyType);
                }

                break;
            case RouletteType.SplitBet:
                splitContentList[number].SetBettingMoney(moneyType);
                break;
            case RouletteType.StreetBet:
                switch(number)
                {
                    case 0:
                        streetContentList[1].SetBettingMoney(moneyType);
                        break;
                    case 1:
                        streetContentList[2].SetBettingMoney(moneyType);
                        break;
                }
                break;
            case RouletteType.SquareBet:
                switch(number)
                {
                    case 0:
                        streetContentList[4].SetBettingMoney(moneyType);
                        break;
                    case 1:
                        streetContentList[5].SetBettingMoney(moneyType);
                        break;
                    case 2:
                        streetContentList[7].SetBettingMoney(moneyType);
                        break;
                    case 3:
                        streetContentList[8].SetBettingMoney(moneyType);
                        break;
                    case 4:
                        streetContentList[10].SetBettingMoney(moneyType);
                        break;
                    case 5:
                        streetContentList[11].SetBettingMoney(moneyType);
                        break;
                    case 6:
                        streetContentList[13].SetBettingMoney(moneyType);
                        break;
                    case 7:
                        streetContentList[14].SetBettingMoney(moneyType);
                        break;
                    case 8:
                        streetContentList[16].SetBettingMoney(moneyType);
                        break;
                    case 9:
                        streetContentList[17].SetBettingMoney(moneyType);
                        break;
                    case 10:
                        streetContentList[19].SetBettingMoney(moneyType);
                        break;
                    case 11:
                        streetContentList[20].SetBettingMoney(moneyType);
                        break;
                    case 12:
                        streetContentList[22].SetBettingMoney(moneyType);
                        break;
                    case 13:
                        streetContentList[23].SetBettingMoney(moneyType);
                        break;
                    case 14:
                        streetContentList[25].SetBettingMoney(moneyType);
                        break;
                    case 15:
                        streetContentList[26].SetBettingMoney(moneyType);
                        break;
                    case 16:
                        streetContentList[28].SetBettingMoney(moneyType);
                        break;
                    case 17:
                        streetContentList[29].SetBettingMoney(moneyType);
                        break;
                    case 18:
                        streetContentList[31].SetBettingMoney(moneyType);
                        break;
                    case 19:
                        streetContentList[32].SetBettingMoney(moneyType);
                        break;
                    case 20:
                        streetContentList[34].SetBettingMoney(moneyType);
                        break;
                    case 21:
                        streetContentList[35].SetBettingMoney(moneyType);
                        break;
                }
                break;
            case RouletteType.FiveNumberBet:
                streetContentList[0].SetBettingMoney(moneyType);
                break;
            case RouletteType.SixNumberBet:
                switch(number)
                {
                    case 0:
                        streetContentList[3].SetBettingMoney(moneyType);
                        break;
                    case 1:
                        streetContentList[6].SetBettingMoney(moneyType);
                        break;
                    case 2:
                        streetContentList[9].SetBettingMoney(moneyType);
                        break;
                    case 3:
                        streetContentList[12].SetBettingMoney(moneyType);
                        break;
                    case 4:
                        streetContentList[15].SetBettingMoney(moneyType);
                        break;
                    case 5:
                        streetContentList[18].SetBettingMoney(moneyType);
                        break;
                    case 6:
                        streetContentList[21].SetBettingMoney(moneyType);
                        break;
                    case 7:
                        streetContentList[24].SetBettingMoney(moneyType);
                        break;
                    case 8:
                        streetContentList[27].SetBettingMoney(moneyType);
                        break;
                    case 9:
                        streetContentList[30].SetBettingMoney(moneyType);
                        break;
                    case 10:
                        streetContentList[33].SetBettingMoney(moneyType);
                        break;
                }
                break;
            case RouletteType.ColumnBet:
                switch(number)
                {
                    case 0:
                        rouletteContents[1].SetBettingMoney(moneyType);
                        break;
                    case 1:
                        rouletteContents[2].SetBettingMoney(moneyType);
                        break;
                    case 2:
                        rouletteContents[3].SetBettingMoney(moneyType);
                        break;
                }
                break;
            case RouletteType.DozenBet:
                switch (number)
                {
                    case 0:
                        rouletteContents[4].SetBettingMoney(moneyType);
                        break;
                    case 1:
                        rouletteContents[5].SetBettingMoney(moneyType);
                        break;
                    case 2:
                        rouletteContents[6].SetBettingMoney(moneyType);
                        break;
                }
                break;
            case RouletteType.LowNumberBet:
                rouletteContents[7].SetBettingMoney(moneyType);
                break;
            case RouletteType.HighNumberBet:
                rouletteContents[12].SetBettingMoney(moneyType);
                break;
            case RouletteType.EvenNumberBet:
                rouletteContents[8].SetBettingMoney(moneyType);
                break;
            case RouletteType.OddNumberBet:
                rouletteContents[11].SetBettingMoney(moneyType);
                break;
            case RouletteType.RedColorBet:
                rouletteContents[9].SetBettingMoney(moneyType);
                break;
            case RouletteType.BlackColorBet:
                rouletteContents[10].SetBettingMoney(moneyType);
                break;
        }
    }

    public void BetOptionButton(BetOptionType type)
    {
        switch (type)
        {
            case BetOptionType.Double:
                break;
            case BetOptionType.Cancle:
                break;
            case BetOptionType.Repeat:
                break;
        }
    }
}
