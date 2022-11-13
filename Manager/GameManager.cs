using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MoneyType moneyType = MoneyType.One;

    public int autoTargetNumber = 0;
    public int startMoney = 50000;

    public RouletteContent rouletteContent;
    public MoneyContent moneyContent;

    public RectTransform rouletteContentTransform;
    public RectTransform splitContentHorizontalTransform;
    public RectTransform splitContentVerticalTransform;
    public RectTransform streetContentTransform;
    public RectTransform moneyContentTransform;

    public RouletteContent[] rouletteContents;

    List<RouletteContent> rouletteContentList = new List<RouletteContent>();
    List<RouletteContent> splitHorizontalContentList = new List<RouletteContent>();
    List<RouletteContent> splitVertialContentList = new List<RouletteContent>();
    List<RouletteContent> streetContentList = new List<RouletteContent>();
    List<RouletteContent> etcContentList = new List<RouletteContent>();

    List<RouletteContent> allContentList = new List<RouletteContent>();

    List<MoneyContent> moneyContentList = new List<MoneyContent>();

    public Text timerText;
    public Image timerFillAmount;

    public Text moneyText;

    public Text targetText;

    public Text recordText;

    private int targetNumber = 0;

    public float money = 0;
    public int saveBetMoney = 0;
    private float getMoney = 0;


    private int time = 0;
    private int bettingValue = 0;

    private int[] redIndex = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
    private int[] column1Index = { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
    private int[] column2Index = { 2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35 };
    private int[] column3Index = { 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34 };
    private List<int[]> squareIndexList = new List<int[]>();
    private List<int[]> splitHorizontalIndexList = new List<int[]>();
    private List<int[]> splitVerticalIndexList = new List<int[]>();
    private List<int[]> streetIndexList = new List<int[]>();
    private List<int[]> lineIndexList = new List<int[]>();

    [Header("Betting")]
    public int[] straightBet;
    public int[] splitBet;
    public int[] streetBet;
    public int[] squareBet;
    public int fiveNumberBet;
    public int[] lineBet;
    public int[] columnBet;
    public int[] dozenBet;
    public int lowNumberBet;
    public int highNumberBet;
    public int evenNumberBet;
    public int oddNumberBet;
    public int redColorBet;
    public int blackColorBet;


    private void Awake()
    {
        instance = this;

        BetInitialize();

        moneyText.text = "₩ 0";
        targetText.text = "?";
        recordText.text = "";

        for (int i = 0; i < 37; i ++)
        {
            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = rouletteContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, RouletteType.StraightBet, i + 1);
            rouletteContentList.Add(content);
            allContentList.Add(content);
        }

        for (int i = 0; i < 36; i++)
        {
            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = splitContentHorizontalTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, RouletteType.SplitBet, i);
            splitHorizontalContentList.Add(content);
            allContentList.Add(content);
        }

        int street = 2;
        int split = 36;

        for (int i = 36; i < 72; i++)
        {
            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = splitContentVerticalTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;

            if(i % 3 == 0) //Street
            {
                content.Initialize(this, RouletteType.StreetBet, street);
                street++;
                streetContentList.Add(content);
            }
            else
            {
                content.Initialize(this, RouletteType.SplitBet, split);
                split++;
                splitVertialContentList.Add(content);
            }
            allContentList.Add(content);
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
                    content.Initialize(this, RouletteType.LineBet, 0);
                    break;
                case 4:
                    content.Initialize(this, RouletteType.SquareBet, 0);
                    break;
                case 5:
                    content.Initialize(this, RouletteType.SquareBet, 1);
                    break;
                case 6:
                    content.Initialize(this, RouletteType.LineBet, 1);
                    break;
                case 7:
                    content.Initialize(this, RouletteType.SquareBet, 2);
                    break;
                case 8:
                    content.Initialize(this, RouletteType.SquareBet, 3);
                    break;
                case 9:
                    content.Initialize(this, RouletteType.LineBet, 2);
                    break;
                case 10:
                    content.Initialize(this, RouletteType.SquareBet, 4);
                    break;
                case 11:
                    content.Initialize(this, RouletteType.SquareBet, 5);
                    break;
                case 12:
                    content.Initialize(this, RouletteType.LineBet, 3);
                    break;
                case 13:
                    content.Initialize(this, RouletteType.SquareBet, 6);
                    break;
                case 14:
                    content.Initialize(this, RouletteType.SquareBet, 7);
                    break;
                case 15:
                    content.Initialize(this, RouletteType.LineBet, 4);
                    break;
                case 16:
                    content.Initialize(this, RouletteType.SquareBet, 8);
                    break;
                case 17:
                    content.Initialize(this, RouletteType.SquareBet, 9);
                    break;
                case 18:
                    content.Initialize(this, RouletteType.LineBet, 5);
                    break;
                case 19:
                    content.Initialize(this, RouletteType.SquareBet, 10);
                    break;
                case 20:
                    content.Initialize(this, RouletteType.SquareBet, 11);
                    break;
                case 21:
                    content.Initialize(this, RouletteType.LineBet, 6);
                    break;
                case 22:
                    content.Initialize(this, RouletteType.SquareBet, 12);
                    break;
                case 23:
                    content.Initialize(this, RouletteType.SquareBet, 13);
                    break;
                case 24:
                    content.Initialize(this, RouletteType.LineBet, 7);
                    break;
                case 25:
                    content.Initialize(this, RouletteType.SquareBet, 14);
                    break;
                case 26:
                    content.Initialize(this, RouletteType.SquareBet, 15);
                    break;
                case 27:
                    content.Initialize(this, RouletteType.LineBet, 8);
                    break;
                case 28:
                    content.Initialize(this, RouletteType.SquareBet, 16);
                    break;
                case 29:
                    content.Initialize(this, RouletteType.SquareBet, 17);
                    break;
                case 30:
                    content.Initialize(this, RouletteType.LineBet, 9);
                    break;
                case 31:
                    content.Initialize(this, RouletteType.SquareBet, 18);
                    break;
                case 32:
                    content.Initialize(this, RouletteType.SquareBet, 19);
                    break;
                case 33:
                    content.Initialize(this, RouletteType.LineBet, 10);
                    break;
                case 34:
                    content.Initialize(this, RouletteType.SquareBet, 20);
                    break;
                case 35:
                    content.Initialize(this, RouletteType.SquareBet, 21);
                    break;

            }

            etcContentList.Add(content);
            allContentList.Add(content);
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

        for(int i = 0; i < rouletteContents.Length; i ++)
        {
            allContentList.Add(rouletteContents[i]);
        }

        SetSquareIndex();

        SetSplitIndex();

        SetStreetIndex();

        SetLineIndex();
    }

    void SetSquareIndex()
    {
        squareIndexList.Add(new int[4] { 1, 2, 4, 5 });
        squareIndexList.Add(new int[4] { 2, 3, 5, 6 });
        squareIndexList.Add(new int[4] { 4, 5, 7, 8 });
        squareIndexList.Add(new int[4] { 5, 6, 8, 9 });
        squareIndexList.Add(new int[4] { 7, 8, 10, 11 });
        squareIndexList.Add(new int[4] { 8, 9, 11, 12 });
        squareIndexList.Add(new int[4] { 10, 11, 13, 14 });
        squareIndexList.Add(new int[4] { 11, 12, 14, 15 });
        squareIndexList.Add(new int[4] { 13, 14, 16, 17 });
        squareIndexList.Add(new int[4] { 14, 15, 17, 18 });
        squareIndexList.Add(new int[4] { 16, 17, 19, 20 });
        squareIndexList.Add(new int[4] { 17, 18, 20, 21 });
        squareIndexList.Add(new int[4] { 19, 20, 22, 23 });
        squareIndexList.Add(new int[4] { 20, 21, 23, 24 });
        squareIndexList.Add(new int[4] { 22, 23, 25, 26 });
        squareIndexList.Add(new int[4] { 23, 24, 26, 27 });
        squareIndexList.Add(new int[4] { 25, 26, 28, 29 });
        squareIndexList.Add(new int[4] { 26, 27, 29, 30 });
        squareIndexList.Add(new int[4] { 28, 29, 31, 32 });
        squareIndexList.Add(new int[4] { 29, 30, 32, 33 });
        squareIndexList.Add(new int[4] { 31, 32, 34, 35 });
        squareIndexList.Add(new int[4] { 32, 33, 35, 36 });
    }

    void SetSplitIndex()
    {
        splitHorizontalIndexList.Add(new int[2] { 0, 1 });
        splitHorizontalIndexList.Add(new int[2] { 0, 2 });
        splitHorizontalIndexList.Add(new int[2] { 0, 3 });

        splitHorizontalIndexList.Add(new int[2] { 1, 4 });
        splitHorizontalIndexList.Add(new int[2] { 2, 5 });
        splitHorizontalIndexList.Add(new int[2] { 3, 6 });

        splitHorizontalIndexList.Add(new int[2] { 4, 7 });
        splitHorizontalIndexList.Add(new int[2] { 5, 8 });
        splitHorizontalIndexList.Add(new int[2] { 6, 9 });

        splitHorizontalIndexList.Add(new int[2] { 7, 10 });
        splitHorizontalIndexList.Add(new int[2] { 8, 11 });
        splitHorizontalIndexList.Add(new int[2] { 9, 12 });

        splitHorizontalIndexList.Add(new int[2] { 10, 13 });
        splitHorizontalIndexList.Add(new int[2] { 11, 14 });
        splitHorizontalIndexList.Add(new int[2] { 12, 15 });

        splitHorizontalIndexList.Add(new int[2] { 13, 16 });
        splitHorizontalIndexList.Add(new int[2] { 14, 17 });
        splitHorizontalIndexList.Add(new int[2] { 15, 18 });

        splitHorizontalIndexList.Add(new int[2] { 16, 19 });
        splitHorizontalIndexList.Add(new int[2] { 17, 20 });
        splitHorizontalIndexList.Add(new int[2] { 18, 21 });

        splitHorizontalIndexList.Add(new int[2] { 19, 22 });
        splitHorizontalIndexList.Add(new int[2] { 20, 23 });
        splitHorizontalIndexList.Add(new int[2] { 21, 24 });

        splitHorizontalIndexList.Add(new int[2] { 22, 25 });
        splitHorizontalIndexList.Add(new int[2] { 23, 26 });
        splitHorizontalIndexList.Add(new int[2] { 24, 27 });

        splitHorizontalIndexList.Add(new int[2] { 25, 28 });
        splitHorizontalIndexList.Add(new int[2] { 26, 29 });
        splitHorizontalIndexList.Add(new int[2] { 27, 30 });

        splitHorizontalIndexList.Add(new int[2] { 28, 31 });
        splitHorizontalIndexList.Add(new int[2] { 29, 32 });
        splitHorizontalIndexList.Add(new int[2] { 30, 33 });

        splitHorizontalIndexList.Add(new int[2] { 31, 34 });
        splitHorizontalIndexList.Add(new int[2] { 32, 35 });
        splitHorizontalIndexList.Add(new int[2] { 33, 36 });

        splitVerticalIndexList.Add(new int[2] { 1, 2 });
        splitVerticalIndexList.Add(new int[2] { 2, 3 });

        splitVerticalIndexList.Add(new int[2] { 4, 5 });
        splitVerticalIndexList.Add(new int[2] { 5, 6 });

        splitVerticalIndexList.Add(new int[2] { 7, 8 });
        splitVerticalIndexList.Add(new int[2] { 8, 9 });

        splitVerticalIndexList.Add(new int[2] { 10, 11 });
        splitVerticalIndexList.Add(new int[2] { 11, 12 });

        splitVerticalIndexList.Add(new int[2] { 13, 14 });
        splitVerticalIndexList.Add(new int[2] { 14, 15 });

        splitVerticalIndexList.Add(new int[2] { 16, 17 });
        splitVerticalIndexList.Add(new int[2] { 17, 18 });

        splitVerticalIndexList.Add(new int[2] { 19, 20 });
        splitVerticalIndexList.Add(new int[2] { 20, 21 });

        splitVerticalIndexList.Add(new int[2] { 22, 23 });
        splitVerticalIndexList.Add(new int[2] { 23, 24 });

        splitVerticalIndexList.Add(new int[2] { 25, 26 });
        splitVerticalIndexList.Add(new int[2] { 26, 27 });

        splitVerticalIndexList.Add(new int[2] { 28, 29 });
        splitVerticalIndexList.Add(new int[2] { 29, 30 });

        splitVerticalIndexList.Add(new int[2] { 31, 32 });
        splitVerticalIndexList.Add(new int[2] { 32, 33 });

        splitVerticalIndexList.Add(new int[2] { 34, 35 });
        splitVerticalIndexList.Add(new int[2] { 35, 36 });
    }

    void SetStreetIndex()
    {
        streetIndexList.Add(new int[3] { 0, 1 ,2 });
        streetIndexList.Add(new int[3] { 0, 2, 3 });

        streetIndexList.Add(new int[3] { 1, 2, 3 });
        streetIndexList.Add(new int[3] { 4, 5, 6 });
        streetIndexList.Add(new int[3] { 7, 8, 9 });
        streetIndexList.Add(new int[3] { 10, 11, 12 });
        streetIndexList.Add(new int[3] { 13, 14, 15 });
        streetIndexList.Add(new int[3] { 16, 17, 18 });
        streetIndexList.Add(new int[3] { 19, 20, 21 });
        streetIndexList.Add(new int[3] { 22, 23, 24 });
        streetIndexList.Add(new int[3] { 25, 26, 27 });
        streetIndexList.Add(new int[3] { 28, 29, 30 });
        streetIndexList.Add(new int[3] { 31, 32, 33 });
        streetIndexList.Add(new int[3] { 34, 35, 36 });
    }

    void SetLineIndex()
    {
        lineIndexList.Add(new int[6] { 1, 2, 3, 4, 5, 6 });
        lineIndexList.Add(new int[6] { 4, 5, 6, 7, 8, 9 });
        lineIndexList.Add(new int[6] { 7, 8, 9, 10, 11, 12 });
        lineIndexList.Add(new int[6] { 10, 11, 12, 13, 14, 15 });
        lineIndexList.Add(new int[6] { 13, 4, 15, 16, 17, 18 });
        lineIndexList.Add(new int[6] { 16, 17, 18, 19, 20, 21 });
        lineIndexList.Add(new int[6] { 19, 20, 21, 22, 23, 24 });
        lineIndexList.Add(new int[6] { 22, 23, 24, 25, 26, 27 });
        lineIndexList.Add(new int[6] { 25, 26, 27, 28, 29, 30 });
        lineIndexList.Add(new int[6] { 28, 29, 30, 31, 32, 33 });
        lineIndexList.Add(new int[6] { 31, 32, 33, 34, 35, 36 });
    }

    private void Start()
    {
        money = startMoney;
        moneyText.text = "₩ " + money.ToString("N2");

        ChangeBettingMoney(MoneyType.One);

        timerFillAmount.fillAmount = 1;
        time = 16;
        StartCoroutine(Timer());
    }

    void BetInitialize()
    {
        getMoney = 0;
        saveBetMoney = 0;

        for (int i = 0; i < allContentList.Count; i++)
        {
            allContentList[i].ResetBettingMoney();
        }

        straightBet = new int[36];
        splitBet = new int[60];
        streetBet = new int[14];
        squareBet = new int[22];
        fiveNumberBet = 0;
        lineBet = new int[11];
        columnBet = new int[3];
        dozenBet = new int[3];
        lowNumberBet = 0;
        highNumberBet = 0;
        evenNumberBet = 0;
        oddNumberBet = 0;
        redColorBet = 0;
        blackColorBet = 0;
    }


    public void ChangeBettingMoney(MoneyType type)
    {
        Debug.Log("Change Betting Money : " + type.ToString());
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
        if(autoTargetNumber == 0)
        {
            targetNumber = Random.Range(0, 37);
        }
        else
        {
            targetNumber = autoTargetNumber;
        }

        string str = "";

        targetText.color = new Color(0, 0, 0);
        str = "<color=#000000>" + targetNumber.ToString() + "</color>";

        for (int i = 1; i < redIndex.Length; i++)
        {
            if (targetNumber == redIndex[i])
            {
                targetText.color = new Color(1, 0, 0);
                str = "<color=#ff0000>" + targetNumber.ToString() + "</color>";
                break;
            }
        }

        recordText.text += str + ", ";
        targetText.text = targetNumber.ToString();

        CheckGame();

        timerText.text = "5초 뒤에 다시 시작합니다.";

        yield return new WaitForSeconds(5f);

        targetText.color = new Color(0, 0, 0);

        timerFillAmount.fillAmount = 1;
        time = 16;
        StartCoroutine(Timer());
    }

    public void Betting(RouletteType rouletteType, int number)
    {
        Debug.Log(rouletteType.ToString() + " : " + number);

        switch (moneyType)
        {
            case MoneyType.One:
                bettingValue = 1;
                break;
            case MoneyType.Two:
                bettingValue = 2;
                break;
            case MoneyType.Three:
                bettingValue = 5;
                break;
            case MoneyType.Four:
                bettingValue = 10;
                break;
            case MoneyType.Five:
                bettingValue = 25;
                break;
            case MoneyType.Six:
                bettingValue = 50;
                break;
            case MoneyType.Seven:
                bettingValue = 100;
                break;
            case MoneyType.Eight:
                bettingValue = 500;
                break;
            case MoneyType.Nine:
                bettingValue = 1000;
                break;
        }

        if(money - bettingValue < 0)
        {
            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
            return;
        }


        switch (rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:

                if (number == 0)
                {
                    rouletteContents[0].SetBettingMoney(moneyType);

                    straightBet[0] += bettingValue;
                }
                else
                {
                    rouletteContentList[number - 1].SetBettingMoney(moneyType);

                    straightBet[number - 1] += bettingValue;
                }

                break;
            case RouletteType.SplitBet:

                if(number < 36)
                {
                    splitHorizontalContentList[number].SetBettingMoney(moneyType);
                }
                else
                {
                    splitVertialContentList[number - 36].SetBettingMoney(moneyType);
                }

                splitBet[number] += bettingValue;

                break;
            case RouletteType.StreetBet:

                switch(number)
                {
                    case 0:
                        etcContentList[1].SetBettingMoney(moneyType);
                        break;
                    case 1:
                        etcContentList[2].SetBettingMoney(moneyType);
                        break;
                    default:
                        streetContentList[number - 2].SetBettingMoney(moneyType);
                        break;
                }    

                streetBet[number] += bettingValue;

                break;
            case RouletteType.SquareBet:

                switch (number)
                {
                    case 0:
                        etcContentList[4].SetBettingMoney(moneyType);
                        break;
                    case 1:
                        etcContentList[5].SetBettingMoney(moneyType);
                        break;
                    case 2:
                        etcContentList[7].SetBettingMoney(moneyType);
                        break;
                    case 3:
                        etcContentList[8].SetBettingMoney(moneyType);
                        break;
                    case 4:
                        etcContentList[10].SetBettingMoney(moneyType);
                        break;
                    case 5:
                        etcContentList[11].SetBettingMoney(moneyType);
                        break;
                    case 6:
                        etcContentList[13].SetBettingMoney(moneyType);
                        break;
                    case 7:
                        etcContentList[14].SetBettingMoney(moneyType);
                        break;
                    case 8:
                        etcContentList[16].SetBettingMoney(moneyType);
                        break;
                    case 9:
                        etcContentList[17].SetBettingMoney(moneyType);
                        break;
                    case 10:
                        etcContentList[19].SetBettingMoney(moneyType);
                        break;
                    case 11:
                        etcContentList[20].SetBettingMoney(moneyType);
                        break;
                    case 12:
                        etcContentList[22].SetBettingMoney(moneyType);
                        break;
                    case 13:
                        etcContentList[23].SetBettingMoney(moneyType);
                        break;
                    case 14:
                        etcContentList[25].SetBettingMoney(moneyType);
                        break;
                    case 15:
                        etcContentList[26].SetBettingMoney(moneyType);
                        break;
                    case 16:
                        etcContentList[28].SetBettingMoney(moneyType);
                        break;
                    case 17:
                        etcContentList[29].SetBettingMoney(moneyType);
                        break;
                    case 18:
                        etcContentList[31].SetBettingMoney(moneyType);
                        break;
                    case 19:
                        etcContentList[32].SetBettingMoney(moneyType);
                        break;
                    case 20:
                        etcContentList[34].SetBettingMoney(moneyType);
                        break;
                    case 21:
                        etcContentList[35].SetBettingMoney(moneyType);
                        break;
                }

                squareBet[number] += bettingValue;

                break;
            case RouletteType.FiveNumberBet:
                etcContentList[0].SetBettingMoney(moneyType);

                fiveNumberBet += bettingValue;

                break;
            case RouletteType.LineBet:

                switch (number)
                {
                    case 0:
                        etcContentList[3].SetBettingMoney(moneyType);
                        break;
                    case 1:
                        etcContentList[6].SetBettingMoney(moneyType);
                        break;
                    case 2:
                        etcContentList[9].SetBettingMoney(moneyType);
                        break;
                    case 3:
                        etcContentList[12].SetBettingMoney(moneyType);
                        break;
                    case 4:
                        etcContentList[15].SetBettingMoney(moneyType);
                        break;
                    case 5:
                        etcContentList[18].SetBettingMoney(moneyType);
                        break;
                    case 6:
                        etcContentList[21].SetBettingMoney(moneyType);
                        break;
                    case 7:
                        etcContentList[24].SetBettingMoney(moneyType);
                        break;
                    case 8:
                        etcContentList[27].SetBettingMoney(moneyType);
                        break;
                    case 9:
                        etcContentList[30].SetBettingMoney(moneyType);
                        break;
                    case 10:
                        etcContentList[33].SetBettingMoney(moneyType);
                        break;
                }

                lineBet[number] += bettingValue;

                break;
            case RouletteType.ColumnBet:
                switch (number)
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

                columnBet[number] += bettingValue;

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

                dozenBet[number] += bettingValue;

                break;
            case RouletteType.LowNumberBet:
                rouletteContents[7].SetBettingMoney(moneyType);

                lowNumberBet += bettingValue;

                break;
            case RouletteType.HighNumberBet:
                rouletteContents[12].SetBettingMoney(moneyType);

                highNumberBet += bettingValue;

                break;
            case RouletteType.EvenNumberBet:
                rouletteContents[8].SetBettingMoney(moneyType);

                evenNumberBet += bettingValue;

                break;
            case RouletteType.OddNumberBet:
                rouletteContents[11].SetBettingMoney(moneyType);

                oddNumberBet += bettingValue;

                break;
            case RouletteType.RedColorBet:
                rouletteContents[9].SetBettingMoney(moneyType);

                redColorBet += bettingValue;

                break;
            case RouletteType.BlackColorBet:
                rouletteContents[10].SetBettingMoney(moneyType);

                blackColorBet += bettingValue;

                break;
        }
    }

    public void SetBettingMoney(int number)
    {
        money -= number;
        saveBetMoney += number;
        moneyText.text = "₩ " + money.ToString("N2");
    }

    public void ReturnBettingMoney(int number)
    {
        money += number;
        saveBetMoney -= number;
        moneyText.text = "₩ " + money.ToString("N2");
    }

    public void BetOptionButton(BetOptionType type)
    {
        //Debug.Log(type.ToString());

        switch (type)
        {
            case BetOptionType.Double:

                for (int i = 0; i < allContentList.Count; i++)
                {
                    allContentList[i].DoubleBetting();
                }

                NotionManager.instance.UseNotion(NotionType.Double);

                break;
            case BetOptionType.Cancle:

                money += saveBetMoney;
                moneyText.text = "₩ " + money.ToString("N2");

                BetInitialize();

                NotionManager.instance.UseNotion(NotionType.Cancle);

                break;
            case BetOptionType.Repeat:

                NotionManager.instance.UseNotion(NotionType.Repeat);

                break;
        }
    }

    void CheckGame()
    {
        getMoney += straightBet[targetNumber - 1] * 35; //Straight 베팅

        for(int i = 0; i < column1Index.Length; i ++) //Column 베팅
        {
            if(targetNumber == column1Index[i])
            {
                getMoney += columnBet[0] * 2;
                break;
            }
        }
        for (int i = 0; i < column2Index.Length; i++)
        {
            if (targetNumber == column2Index[i])
            {
                getMoney += columnBet[1] * 2;
                break;
            }
        }
        for (int i = 0; i < column3Index.Length; i++)
        {
            if (targetNumber == column3Index[i])
            {
                getMoney += columnBet[2] * 2;
                break;
            }
        }

        if(targetNumber > 0 && targetNumber < 13) //Dozen 베팅
        {
            getMoney += dozenBet[0] * 2;
        }

        if (targetNumber > 12 && targetNumber < 25)
        {
            getMoney += dozenBet[1] * 2;
        }

        if (targetNumber > 24)
        {
            getMoney += dozenBet[2] * 2;
        }

        if (targetNumber > 0 && targetNumber < 19) //LowNumber 베팅
        {
            getMoney += lowNumberBet * 1;
        }

        if (targetNumber > 18) //HighNumber 베팅
        {
            getMoney += highNumberBet * 1;
        }

        if(targetNumber % 2 == 0 && targetNumber != 0) //Even 베팅
        {
            getMoney += evenNumberBet * 1;
        }

        if (targetNumber % 2 == 1) //Odd 베팅
        {
            getMoney += oddNumberBet * 1;
        }

        bool check = false;

        for(int i = 0; i < redIndex.Length; i ++) //RedColor 베팅
        {
            if(targetNumber == redIndex[i])
            {
                getMoney += redColorBet * 1;
                check = true;
                break;
            }
        }

        if(check)
        {
            getMoney += blackColorBet * 1; //BlackColor 베팅
        }

        if(targetNumber == 0 || targetNumber == 1 || targetNumber == 2 || targetNumber == 3)  //FiveNumber 베팅
        {
            getMoney += fiveNumberBet * 6;
        }

        for(int i = 0; i < squareIndexList.Count; i ++) //Square 베팅
        {
            for(int j = 0; j < squareIndexList[i].Length; j ++)
            {
                if(targetNumber == squareIndexList[i][j])
                {
                    getMoney += squareBet[i] * 4;
                    break;
                }
            }
        }

        for (int i = 0; i < splitHorizontalIndexList.Count; i ++) //Split 베팅
        {
            for (int j = 0; j < splitHorizontalIndexList[i].Length; j ++)
            {
                if (targetNumber == splitHorizontalIndexList[i][j])
                {
                    getMoney += splitBet[i] * 17;
                    break;
                }
            }
        }

        for (int i = 0; i < splitVerticalIndexList.Count; i++) //Split 베팅
        {
            for (int j = 0; j < splitVerticalIndexList[i].Length; j++)
            {
                if (targetNumber == splitVerticalIndexList[i][j])
                {
                    getMoney += splitBet[i + 36] * 17;
                    break;
                }
            }
        }

        for (int i = 0; i < streetIndexList.Count; i++) //Street 베팅
        {
            for (int j = 0; j < streetIndexList[i].Length; j++)
            {
                if (targetNumber == streetIndexList[i][j])
                {
                    getMoney += streetBet[i] * 11;
                    break;
                }
            }
        }

        for (int i = 0; i < lineIndexList.Count; i++) //Line 베팅
        {
            for (int j = 0; j < lineIndexList[i].Length; j++)
            {
                if (targetNumber == lineIndexList[i][j])
                {
                    getMoney += lineBet[i] * 5;
                    break;
                }
            }
        }

        //Debug.Log(getMoney + "만큼 돈을 땄습니다.");
        NotionManager.instance.UseNotion(getMoney + " 만큼 돈을 땄습니다.", ColorType.Green);

        money += getMoney;
        moneyText.text = "₩ " + money.ToString("N2");

        BetInitialize();
    }
}
