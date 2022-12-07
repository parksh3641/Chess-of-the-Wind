using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public RouletteContent mainRouletteContent;
    public BlockType blockType = BlockType.Default;
    public BlockInformation blockInformation = new BlockInformation();
    Transform targetBlockContent;

    [Title("Developer")]
    public int autoTargetNumber = -1;
    public int setMoney = 50000;
    public int setTimer = 16;
    public bool checkOverlap = false;

    [Title("BettingView")]
    public GameObject rouletteCamera;
    public GameObject mainView;
    public GameObject rouletteView;

    [Title("Setting")]
    public GridLayoutGroup gridLayoutGroup;

    [Title("Timer")]
    public Text timerText;
    public Image timerFillAmount;
    private int timer = 0;

    [Title("Text")]
    public Text moneyText;
    public Text targetText;
    public Text recordText;

    [Title("Value")]
    public float money = 0;
    private float bettingMoney = 0;
    private float getMoney = 0;

    private int targetNumber = 0;
    private int gridConstraintCount = 0;

    [Title("Drag")]
    private Transform dragPos;
    private bool checkDrag = false;

    [Title("Bool")]
    public bool blockDrag = false;
    public bool blockDrop = false;
    public bool blockOverlap = false;


    public GameObject blockRootParent;
    public GameObject blockParent;
    public GameObject blockGridParent;

    public GameObject dontTouchObj;
    public GameObject targetObj;

    [Title("Prefab")]
    public RouletteContent rouletteContent;
    public RectTransform rouletteContentTransform;

    public RectTransform rouletteContentTransformSplitBet_Vertical;
    public RectTransform rouletteContentTransformSplitBet_Horizontal;

    public RectTransform rouletteContentTransformSquareBet;

    public NumberContent numberContent;
    public RectTransform numberContentTransform;

    public BlockContent blockContent;
    public RectTransform blockContentTransform;

    List<RouletteContent> rouletteContentList = new List<RouletteContent>();
    List<RouletteContent> rouletteSplitContentList_Vertical = new List<RouletteContent>();
    List<RouletteContent> rouletteSplitContentList_Horizontal = new List<RouletteContent>();
    List<RouletteContent> rouletteSquareContentList = new List<RouletteContent>();

    List<RouletteContent> numberContentList = new List<RouletteContent>();
    List<BlockContent> blockContentList = new List<BlockContent>();

    [Header("Betting")]
    private List<int[]> splitHorizontalIndexList = new List<int[]>();
    private List<int[]> splitVerticalIndexList = new List<int[]>();
    private List<int[]> squareIndexList = new List<int[]>();

    public RouletteManager rouletteManager;
    BlockDataBase blockDataBase;

    private void Awake()
    {
        if(blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;

        instance = this;

        Application.targetFrameRate = 60;

        dontTouchObj.SetActive(false);
        targetObj.SetActive(false);

        rouletteCamera.SetActive(false);
        mainView.SetActive(true);
        rouletteView.SetActive(false);

        ChangeMoney(setMoney);

        targetText.text = "?";
        recordText.text = "";

        gridConstraintCount = gridLayoutGroup.constraintCount;

        int index = 0;
        int count = 0;

        for (int i = 0; i < 25; i++)
        {
            int[] setIndex = new int[2];

            if (index >= gridConstraintCount)
            {
                index = 0;
                count++;
            }

            setIndex[0] = index;
            setIndex[1] = count;

            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = rouletteContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockParent.transform, RouletteType.StraightBet, setIndex, i);
            rouletteContentList.Add(content);

            NumberContent numContent = Instantiate(numberContent);
            numContent.transform.parent = numberContentTransform;
            numContent.transform.localPosition = Vector3.zero;
            numContent.transform.localScale = Vector3.one;
            numContent.Initialize(i);
            numberContentList.Add(content);

            index++;
        }

        index = 0;
        count = 0;

        for (int i = 0; i < 20; i ++)
        {
            int[] setIndex = new int[2];

            if (index >= gridConstraintCount - 1)
            {
                index = 0;
                count++;
            }

            setIndex[0] = index;
            setIndex[1] = count;

            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = rouletteContentTransformSplitBet_Vertical;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockParent.transform, RouletteType.SplitBet_Vertical, setIndex, i);
            rouletteSplitContentList_Vertical.Add(content);

            index++;
        }

        index = 0;
        count = 0;

        for (int i = 0; i < 20; i++)
        {
            int[] setIndex = new int[2];

            if (index >= gridConstraintCount)
            {
                index = 0;
                count++;
            }

            setIndex[0] = index;
            setIndex[1] = count;

            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = rouletteContentTransformSplitBet_Horizontal;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockParent.transform, RouletteType.SplitBet_Horizontal, setIndex, i);
            rouletteSplitContentList_Horizontal.Add(content);

            index++;
        }

        index = 0;
        count = 0;

        for (int i = 0; i < 16; i++)
        {
            int[] setIndex = new int[2];

            if (index >= gridConstraintCount - 1)
            {
                index = 0;
                count++;
            }

            setIndex[0] = index;
            setIndex[1] = count;

            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = rouletteContentTransformSquareBet;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockParent.transform, RouletteType.SquareBet, setIndex, i);
            rouletteSquareContentList.Add(content);

            index++;
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        {
            BlockContent content = Instantiate(blockContent);
            content.transform.parent = blockContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockRootParent.transform, blockGridParent.transform, BlockType.Default + i + 1);
            blockContentList.Add(content);
        }
    }

    private void Start()
    {
        timer = setTimer;
        timerFillAmount.fillAmount = 1;
        StartCoroutine(TimerCoroution());

        SetSplitIndex();
        SetSquareIndex();
    }

    void SetSplitIndex()
    {
        int index = 0;
        int count = 0;
        int number = 0;

        for (int i = 0; i < 20; i ++)
        {
            if (index >= gridConstraintCount - 1)
            {
                index = 0;
                count += gridConstraintCount;
            }

            number = index + 1 + count;

            splitHorizontalIndexList.Add(new int[2] { number, number + 1 });

            index++;
        }

        index = 0;
        count = 0;
        number = 0;

        for (int i = 0; i < 20; i++)
        {
            if (index >= gridConstraintCount)
            {
                index = 0;
                count += gridConstraintCount;
            }

            number = index + 1 + count;

            splitVerticalIndexList.Add(new int[2] { number, number + 5 });

            index++;
        }
    }

    void SetSquareIndex()
    {
        int index = 0;
        int count = 0;
        int number = 0;

        for (int i = 0; i < 16; i ++)
        {
            if (index >= gridConstraintCount - 1)
            {
                index = 0;
                count += gridConstraintCount;
            }

            number = index + 1 + count;

            squareIndexList.Add(new int[4] { number, number + 1, number + 5, number + 6 });

            index++;
        }
    }

    void ChangeMoney(float plus)
    {
        money += plus;
        moneyText.text = "₩ " + money.ToString("N2");
    }

    IEnumerator TimerCoroution()
    {
        if (timer <= 0)
        {
            //StartCoroutine(RandomTargetNumber());
            OpenRouletteView();
            yield break;
        }

        targetText.text = "?";

        timer -= 1;

        timerFillAmount.fillAmount = timer / ((setTimer - 1) * 1.0f);
        timerText.text = timer.ToString();

        yield return new WaitForSeconds(1f);
        StartCoroutine(TimerCoroution());
    }

    void OpenRouletteView()
    {
        if (autoTargetNumber == -1)
        {
            rouletteCamera.SetActive(true);
            mainView.SetActive(false);
            rouletteView.SetActive(true);

            rouletteManager.Initialize(rouletteContentList.Count + 1);
        }
        else
        {
            targetNumber = autoTargetNumber;
        }
    }

    public void CloseRouletteView(int number)
    {
        rouletteCamera.SetActive(false);
        mainView.SetActive(true);
        rouletteView.SetActive(false);

        targetNumber = number;

        StartCoroutine(RandomTargetNumber());
    }

    IEnumerator RandomTargetNumber()
    {
        //string str = "";

        //targetText.color = new Color(0, 0, 0);
        //str = "<color=#000000>" + targetNumber.ToString() + "</color>";

        //if (targetNumber % 2 == 0)
        //{
        //    targetText.color = new Color(1, 0, 0);
        //    str = "<color=#ff0000>" + targetNumber.ToString() + "</color>";
        //}

        recordText.text += targetNumber + ", ";
        targetText.text = targetNumber.ToString();

        CheckTargetNumber();

        dontTouchObj.SetActive(true);

        ResetRouletteContent();

        for(int i = 0; i < blockContentList.Count; i ++)
        {
            if(blockContentList[i].isDrag)
            {
                blockContentList[i].TimeOver();
                break;
            }
        }

        timerText.text = "5초 뒤에 다시 시작합니다.";

        yield return new WaitForSeconds(5f);

        for (int i = 0; i < blockContentList.Count; i++)
        {
            blockContentList[i].ResetPos();
        }

        for (int i = 0; i < rouletteContentList.Count; i++)
        {
            rouletteContentList[i].SetActiveFalseAll();
        }

        for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++)
        {
            rouletteSplitContentList_Vertical[i].SetActiveFalseAll();
        }

        for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++)
        {
            rouletteSplitContentList_Horizontal[i].SetActiveFalseAll();
        }

        for (int i = 0; i < rouletteSquareContentList.Count; i++)
        {
            rouletteSquareContentList[i].SetActiveFalseAll();
        }

        dontTouchObj.SetActive(false);
        targetObj.SetActive(false);

        timerFillAmount.fillAmount = 1;
        timer = setTimer;
        StartCoroutine(TimerCoroution());
    }

    private void CheckTargetNumber()
    {
        getMoney = 0;

        targetObj.SetActive(true);
        targetObj.transform.SetAsLastSibling();

        Transform trans = rouletteContentList[targetNumber - 1].transform;

        targetObj.transform.position = trans.position;

        BlockInformation block = new BlockInformation();

        for (int i = 0; i < rouletteContentList.Count; i ++)
        {
            if (rouletteContentList[i].number == targetNumber && rouletteContentList[i].isActive)
            {
                for (int j = 0; j < rouletteContentList[i].blockType.Length; j++)
                {
                    if (rouletteContentList[i].blockType[j] != BlockType.Default)
                    {
                        block = blockDataBase.GetBlockInfomation(rouletteContentList[i].blockType[j]);
                        getMoney += (36.0f / block.size) * block.bettingPrice * block.magnification;
                    }
                }
            }
        }

        for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++)
        {
            if (rouletteSplitContentList_Vertical[i].isActive)
            {
                for (int j = 0; j < rouletteSplitContentList_Vertical[i].blockType.Length; j++)
                {
                    if (rouletteSplitContentList_Vertical[i].blockType[j] != BlockType.Default)
                    {
                        for (int k = 0; k < splitHorizontalIndexList[rouletteContentList[i].number - 1].Length; k++)
                        {
                            if (targetNumber == splitHorizontalIndexList[rouletteContentList[i].number - 1][k])
                            {
                                block = blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType[j]);
                                getMoney += (18.0f / block.size) * block.bettingPrice * block.magnification;
                                break;
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++)
        {
            if (rouletteSplitContentList_Horizontal[i].isActive)
            {
                for (int j = 0; j < rouletteSplitContentList_Horizontal[i].blockType.Length; j++)
                {
                    if (rouletteSplitContentList_Horizontal[i].blockType[j] != BlockType.Default)
                    {
                        for (int k = 0; k < splitVerticalIndexList[rouletteContentList[i].number - 1].Length; k++)
                        {
                            if (targetNumber == splitVerticalIndexList[rouletteContentList[i].number - 1][k])
                            {
                                block = blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType[j]);
                                getMoney += (18.0f / block.size) * block.bettingPrice * block.magnification;
                                break;
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < rouletteSquareContentList.Count; i++)
        {
            if (rouletteSquareContentList[i].isActive)
            {
                for (int j = 0; j < rouletteSquareContentList[i].blockType.Length; j++)
                {
                    if (rouletteSquareContentList[i].blockType[j] != BlockType.Default)
                    {
                        for (int k = 0; k < squareIndexList[rouletteContentList[i].number - 1].Length; k++)
                        {
                            if (targetNumber == squareIndexList[rouletteContentList[i].number - 1][k])
                            {
                                block = blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType[j]);
                                getMoney += (6.0f / block.size) * block.bettingPrice * block.magnification;
                                break;
                            }
                        }
                    }
                }
            }
        }

        ChangeMoney((int)getMoney);

        if (getMoney > 0)
        {
            NotionManager.instance.UseNotion((int)getMoney + " 만큼 돈을 땄어요 !", ColorType.Green);
        }
        else
        {
            if(bettingMoney == 0)
            {
                NotionManager.instance.UseNotion("시간 초과 !", ColorType.Red);
            }
            else
            {
                NotionManager.instance.UseNotion(bettingMoney + " 만큼 돈을 잃었어요 ㅠㅠ", ColorType.Red);
            }
        }

        bettingMoney = 0;
    }

    void Update()
    {
        if(blockDrag && targetBlockContent != null)
        {
            if(targetBlockContent.position.y > Screen.height * 0.5f + 450 || targetBlockContent.position.y < Screen.height * 0.5f - 650)
            {
                if (checkDrag)
                {
                    checkDrag = false;
                    ResetRouletteContent();
                }
            }
            else
            {
                if(!checkDrag) checkDrag = true;
            }

            if (targetBlockContent.position.x > Screen.width * 0.5f + 520 || targetBlockContent.position.x < Screen.width * 0.5f - 520)
            {
                if(checkDrag)
                {
                    checkDrag = false;
                    ResetRouletteContent();
                }
            }
            else
            {
                if (!checkDrag) checkDrag = true;
            }
        }
    }

    int[] index0 = new int[2];
    int[] index1 = new int[2];
    int[] index2 = new int[2];
    int[] index3 = new int[2];
    int[] index4 = new int[2];
    int[] index5 = new int[2];
    int[] index6 = new int[2];
    int[] index7 = new int[2];
    int[] index8 = new int[2];

    public void EnterBlock(RouletteContent rouletteContent, BlockContent blockContent)
    {
        mainRouletteContent = rouletteContent;

        blockInformation = blockDataBase.GetBlockInfomation(blockContent.blockType);

        targetBlockContent = blockContent.transform;

        switch (mainRouletteContent.rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                for (int i = 0; i < rouletteContentList.Count; i++)
                {
                    for(int j = 0; j < rouletteContentList[i].blockType.Length; j ++)
                    {
                        if (rouletteContentList[i].blockType[j] == blockContent.blockType)
                        {
                            rouletteContentList[i].SetActiveFalse(blockContent.blockType);

                            bettingMoney -= blockInformation.bettingPrice;
                            ChangeMoney(blockInformation.bettingPrice);
                        }
                    }
                }
                break;
            case RouletteType.SplitBet_Horizontal:
                for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++)
                {
                    for (int j = 0; j < rouletteSplitContentList_Horizontal[i].blockType.Length; j++)
                    {
                        if (rouletteSplitContentList_Horizontal[i].blockType[j] == blockContent.blockType)
                        {
                            rouletteSplitContentList_Horizontal[i].SetActiveFalse(blockContent.blockType);

                            bettingMoney -= blockInformation.bettingPrice;
                            ChangeMoney(blockInformation.bettingPrice);
                        }
                    }
                }
                break;
            case RouletteType.SplitBet_Vertical:
                for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++)
                {
                    for (int j = 0; j < rouletteSplitContentList_Vertical[i].blockType.Length; j++)
                    {
                        if (rouletteSplitContentList_Vertical[i].blockType[j] == blockContent.blockType)
                        {
                            rouletteSplitContentList_Vertical[i].SetActiveFalse(blockContent.blockType);

                            bettingMoney -= blockInformation.bettingPrice;
                            ChangeMoney(blockInformation.bettingPrice);
                        }
                    }
                }
                break;
            case RouletteType.SquareBet:
                for (int i = 0; i < rouletteSquareContentList.Count; i++)
                {
                    for (int j = 0; j < rouletteSquareContentList[i].blockType.Length; j++)
                    {
                        if (rouletteSquareContentList[i].blockType[j] == blockContent.blockType)
                        {
                            rouletteSquareContentList[i].SetActiveFalse(blockContent.blockType);

                            bettingMoney -= blockInformation.bettingPrice;
                            ChangeMoney(blockInformation.bettingPrice);
                        }
                    }
                }
                break;
        }

        index0 = new int[2];
        index1 = new int[2];
        index2 = new int[2];
        index3 = new int[2];
        index4 = new int[2];
        index5 = new int[2];
        index6 = new int[2];
        index7 = new int[2];
        index8 = new int[2];

        switch (blockContent.blockType)
        {
            case BlockType.Default:
                break;
            case BlockType.I:
                index0[0] = rouletteContent.index[0];
                index0[1] = rouletteContent.index[1] - 2;

                index1[0] = rouletteContent.index[0];
                index1[1] = rouletteContent.index[1] - 1;

                index2[0] = rouletteContent.index[0]; //마우스 포인터 기준점
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0];
                index3[1] = rouletteContent.index[1] + 1;

                index4[0] = rouletteContent.index[0];
                index4[1] = rouletteContent.index[1];

                index5[0] = rouletteContent.index[0];
                index5[1] = rouletteContent.index[1];

                index6[0] = rouletteContent.index[0];
                index6[1] = rouletteContent.index[1];

                index7[0] = rouletteContent.index[0];
                index7[1] = rouletteContent.index[1];

                index8[0] = rouletteContent.index[0];
                index8[1] = rouletteContent.index[1];
                break;
            case BlockType.O:
                index0[0] = rouletteContent.index[0];
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0] + 1;
                index1[1] = rouletteContent.index[1] - 1;

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] + 1;
                index3[1] = rouletteContent.index[1];

                index4[0] = rouletteContent.index[0];
                index4[1] = rouletteContent.index[1];

                index5[0] = rouletteContent.index[0];
                index5[1] = rouletteContent.index[1];

                index6[0] = rouletteContent.index[0];
                index6[1] = rouletteContent.index[1];

                index7[0] = rouletteContent.index[0];
                index7[1] = rouletteContent.index[1];

                index8[0] = rouletteContent.index[0];
                index8[1] = rouletteContent.index[1];
                break;
            case BlockType.T:
                index0[0] = rouletteContent.index[0];
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0] - 1;
                index1[1] = rouletteContent.index[1];

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] + 1;
                index3[1] = rouletteContent.index[1];

                index4[0] = rouletteContent.index[0];
                index4[1] = rouletteContent.index[1];

                index5[0] = rouletteContent.index[0];
                index5[1] = rouletteContent.index[1];

                index6[0] = rouletteContent.index[0];
                index6[1] = rouletteContent.index[1];

                index7[0] = rouletteContent.index[0];
                index7[1] = rouletteContent.index[1];

                index8[0] = rouletteContent.index[0];
                index8[1] = rouletteContent.index[1];
                break;
            case BlockType.L:
                index0[0] = rouletteContent.index[0] + 1;
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0] - 1;
                index1[1] = rouletteContent.index[1];

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] + 1;
                index3[1] = rouletteContent.index[1];

                index4[0] = rouletteContent.index[0];
                index4[1] = rouletteContent.index[1];

                index5[0] = rouletteContent.index[0];
                index5[1] = rouletteContent.index[1];

                index6[0] = rouletteContent.index[0];
                index6[1] = rouletteContent.index[1];

                index7[0] = rouletteContent.index[0];
                index7[1] = rouletteContent.index[1];

                index8[0] = rouletteContent.index[0];
                index8[1] = rouletteContent.index[1];
                break;
            case BlockType.J:
                index0[0] = rouletteContent.index[0] - 1;
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0] - 1;
                index1[1] = rouletteContent.index[1];

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] + 1;
                index3[1] = rouletteContent.index[1];

                index4[0] = rouletteContent.index[0];
                index4[1] = rouletteContent.index[1];

                index5[0] = rouletteContent.index[0];
                index5[1] = rouletteContent.index[1];

                index6[0] = rouletteContent.index[0];
                index6[1] = rouletteContent.index[1];

                index7[0] = rouletteContent.index[0];
                index7[1] = rouletteContent.index[1];

                index8[0] = rouletteContent.index[0];
                index8[1] = rouletteContent.index[1];
                break;
            case BlockType.S:
                index0[0] = rouletteContent.index[0];
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0] + 1;
                index1[1] = rouletteContent.index[1] - 1;

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] - 1;
                index3[1] = rouletteContent.index[1];

                index4[0] = rouletteContent.index[0];
                index4[1] = rouletteContent.index[1];

                index5[0] = rouletteContent.index[0];
                index5[1] = rouletteContent.index[1];

                index6[0] = rouletteContent.index[0];
                index6[1] = rouletteContent.index[1];

                index7[0] = rouletteContent.index[0];
                index7[1] = rouletteContent.index[1];

                index8[0] = rouletteContent.index[0];
                index8[1] = rouletteContent.index[1];
                break;
            case BlockType.Z:
                index0[0] = rouletteContent.index[0] - 1;
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0];
                index1[1] = rouletteContent.index[1] - 1;

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] + 1;
                index3[1] = rouletteContent.index[1];

                index4[0] = rouletteContent.index[0];
                index4[1] = rouletteContent.index[1];

                index5[0] = rouletteContent.index[0];
                index5[1] = rouletteContent.index[1];

                index6[0] = rouletteContent.index[0];
                index6[1] = rouletteContent.index[1];

                index7[0] = rouletteContent.index[0];
                index7[1] = rouletteContent.index[1];

                index8[0] = rouletteContent.index[0];
                index8[1] = rouletteContent.index[1];
                break;
            case BlockType.BigO:
                index0[0] = rouletteContent.index[0] - 1;
                index0[1] = rouletteContent.index[1] - 1;

                index1[0] = rouletteContent.index[0];
                index1[1] = rouletteContent.index[1] - 1;

                index2[0] = rouletteContent.index[0] + 1;
                index2[1] = rouletteContent.index[1] - 1;

                index3[0] = rouletteContent.index[0] - 1;
                index3[1] = rouletteContent.index[1];

                index4[0] = rouletteContent.index[0];
                index4[1] = rouletteContent.index[1];

                index5[0] = rouletteContent.index[0] + 1;
                index5[1] = rouletteContent.index[1];

                index6[0] = rouletteContent.index[0] - 1;
                index6[1] = rouletteContent.index[1] + 1;

                index7[0] = rouletteContent.index[0];
                index7[1] = rouletteContent.index[1] + 1;

                index8[0] = rouletteContent.index[0] + 1;
                index8[1] = rouletteContent.index[1] + 1;
                break;
            case BlockType.I_Horizontal:
                index0[0] = rouletteContent.index[0] - 1;
                index0[1] = rouletteContent.index[1];

                index1[0] = rouletteContent.index[0];
                index1[1] = rouletteContent.index[1];

                index2[0] = rouletteContent.index[0] + 1;
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0] + 2;
                index3[1] = rouletteContent.index[1];

                index4[0] = rouletteContent.index[0];
                index4[1] = rouletteContent.index[1];

                index5[0] = rouletteContent.index[0];
                index5[1] = rouletteContent.index[1];

                index6[0] = rouletteContent.index[0];
                index6[1] = rouletteContent.index[1];

                index7[0] = rouletteContent.index[0];
                index7[1] = rouletteContent.index[1];

                index8[0] = rouletteContent.index[0];
                index8[1] = rouletteContent.index[1];
                break;
        }

        ResetRouletteContent();

        switch (mainRouletteContent.rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                for (int i = 0; i < rouletteContentList.Count; i++)
                {
                    if (rouletteContentList[i].index.SequenceEqual(index0) || rouletteContentList[i].index.SequenceEqual(index1)
                        || rouletteContentList[i].index.SequenceEqual(index2) || rouletteContentList[i].index.SequenceEqual(index3)
                        || rouletteContentList[i].index.SequenceEqual(index4) || rouletteContentList[i].index.SequenceEqual(index5)
                        || rouletteContentList[i].index.SequenceEqual(index6) || rouletteContentList[i].index.SequenceEqual(index7)
                        || rouletteContentList[i].index.SequenceEqual(index8))
                    {
                        rouletteContentList[i].SetBackgroundColor(RouletteColorType.Yellow);
                    }
                }
                break;
            case RouletteType.SplitBet_Horizontal:
                for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++)
                {
                    if (rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index0) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index1)
                        || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index2) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index3)
                        || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index4) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index5)
                        || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index6) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index7)
                        || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index8))
                    {
                        rouletteSplitContentList_Horizontal[i].SetBackgroundColor(RouletteColorType.Yellow);
                    }
                }
                break;
            case RouletteType.SplitBet_Vertical:
                for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++)
                {
                    if (rouletteSplitContentList_Vertical[i].index.SequenceEqual(index0) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index1)
                        || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index2) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index3)
                        || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index4) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index5)
                        || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index6) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index7)
                        || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index8))
                    {
                        rouletteSplitContentList_Vertical[i].SetBackgroundColor(RouletteColorType.Yellow);
                    }
                }
                break;
            case RouletteType.SquareBet:
                for (int i = 0; i < rouletteSquareContentList.Count; i++)
                {
                    if (rouletteSquareContentList[i].index.SequenceEqual(index0) || rouletteSquareContentList[i].index.SequenceEqual(index1)
                        || rouletteSquareContentList[i].index.SequenceEqual(index2) || rouletteSquareContentList[i].index.SequenceEqual(index3)
                        || rouletteSquareContentList[i].index.SequenceEqual(index4) || rouletteSquareContentList[i].index.SequenceEqual(index5)
                        || rouletteSquareContentList[i].index.SequenceEqual(index6) || rouletteSquareContentList[i].index.SequenceEqual(index7)
                        || rouletteSquareContentList[i].index.SequenceEqual(index8))
                    {
                        rouletteSquareContentList[i].SetBackgroundColor(RouletteColorType.Yellow);
                    }
                }
                break;
        }

        //범위 밖 넘어갔는지 체크

        switch (mainRouletteContent.rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                if (index0[0] < 0 || index0[1] < 0 || index1[0] < 0 || index1[1] < 0 || index2[0] < 0 || index2[1] < 0 || index3[0] < 0 || index3[1] < 0
                    || index4[0] < 0 || index4[1] < 0 || index5[0] < 0 || index5[1] < 0 || index6[0] < 0 || index6[1] < 0 || index7[0] < 0 || index7[1] < 0
                    || index8[0] < 0 || index8[1] < 0
    || index0[0] >= gridConstraintCount || index0[1] >= gridConstraintCount || index1[0] >= gridConstraintCount || index1[1] >= gridConstraintCount
    || index2[0] >= gridConstraintCount || index2[1] >= gridConstraintCount || index3[0] >= gridConstraintCount || index3[1] >= gridConstraintCount
    || index4[0] >= gridConstraintCount || index4[1] >= gridConstraintCount || index5[0] >= gridConstraintCount || index5[1] >= gridConstraintCount
    || index6[0] >= gridConstraintCount || index6[1] >= gridConstraintCount || index7[0] >= gridConstraintCount || index7[1] >= gridConstraintCount
    || index8[0] >= gridConstraintCount || index8[1] >= gridConstraintCount)
                {
                    blockDrop = false;
                }
                else
                {
                    blockDrop = true;
                }
                break;
            case RouletteType.SplitBet_Horizontal:
                if (index0[0] < 0 || index0[1] < 0 || index1[0] < 0 || index1[1] < 0 || index2[0] < 0 || index2[1] < 0 || index3[0] < 0 || index3[1] < 0
                    || index4[0] < 0 || index4[1] < 0 || index5[0] < 0 || index5[1] < 0 || index6[0] < 0 || index6[1] < 0 || index7[0] < 0 || index7[1] < 0
                    || index8[0] < 0 || index8[1] < 0
|| index0[0] >= gridConstraintCount || index0[1] >= gridConstraintCount - 1 || index1[0] >= gridConstraintCount || index1[1] >= gridConstraintCount - 1
|| index2[0] >= gridConstraintCount || index2[1] >= gridConstraintCount - 1 || index3[0] >= gridConstraintCount || index3[1] >= gridConstraintCount - 1
|| index4[0] >= gridConstraintCount || index4[1] >= gridConstraintCount - 1 || index5[0] >= gridConstraintCount || index5[1] >= gridConstraintCount - 1
|| index6[0] >= gridConstraintCount || index6[1] >= gridConstraintCount - 1 || index7[0] >= gridConstraintCount || index7[1] >= gridConstraintCount - 1
|| index8[0] >= gridConstraintCount || index8[1] >= gridConstraintCount - 1)
                {
                    blockDrop = false;
                }
                else
                {
                    blockDrop = true;
                }
                break;
            case RouletteType.SplitBet_Vertical:
                if (index0[0] < 0 || index0[1] < 0 || index1[0] < 0 || index1[1] < 0 || index2[0] < 0 || index2[1] < 0 || index3[0] < 0 || index3[1] < 0
                    || index4[0] < 0 || index4[1] < 0 || index5[0] < 0 || index5[1] < 0 || index6[0] < 0 || index6[1] < 0 || index7[0] < 0 || index7[1] < 0
                    || index8[0] < 0 || index8[1] < 0
|| index0[0] >= gridConstraintCount - 1 || index0[1] >= gridConstraintCount || index1[0] >= gridConstraintCount - 1 || index1[1] >= gridConstraintCount
|| index2[0] >= gridConstraintCount - 1 || index2[1] >= gridConstraintCount || index3[0] >= gridConstraintCount - 1 || index3[1] >= gridConstraintCount
|| index4[0] >= gridConstraintCount - 1 || index4[1] >= gridConstraintCount || index5[0] >= gridConstraintCount - 1 || index5[1] >= gridConstraintCount
|| index6[0] >= gridConstraintCount - 1 || index6[1] >= gridConstraintCount || index7[0] >= gridConstraintCount - 1 || index7[1] >= gridConstraintCount
|| index8[0] >= gridConstraintCount - 1 || index8[1] >= gridConstraintCount)
                {
                    blockDrop = false;
                }
                else
                {
                    blockDrop = true;
                }
                break;
            case RouletteType.SquareBet:
                if (index0[0] < 0 || index0[1] < 0 || index1[0] < 0 || index1[1] < 0 || index2[0] < 0 || index2[1] < 0 || index3[0] < 0 || index3[1] < 0
                    || index4[0] < 0 || index4[1] < 0 || index5[0] < 0 || index5[1] < 0 || index6[0] < 0 || index6[1] < 0 || index7[0] < 0 || index7[1] < 0
                    || index8[0] < 0 || index8[1] < 0
    || index0[0] >= gridConstraintCount - 1 || index0[1] >= gridConstraintCount - 1 || index1[0] >= gridConstraintCount - 1 || index1[1] >= gridConstraintCount - 1
    || index2[0] >= gridConstraintCount - 1 || index2[1] >= gridConstraintCount - 1 || index3[0] >= gridConstraintCount - 1 || index3[1] >= gridConstraintCount - 1
    || index4[0] >= gridConstraintCount - 1 || index4[1] >= gridConstraintCount - 1 || index5[0] >= gridConstraintCount - 1 || index5[1] >= gridConstraintCount - 1
    || index6[0] >= gridConstraintCount - 1 || index6[1] >= gridConstraintCount - 1 || index7[0] >= gridConstraintCount - 1 || index7[1] >= gridConstraintCount - 1
    || index8[0] >= gridConstraintCount - 1 || index8[1] >= gridConstraintCount - 1)
                {
                    blockDrop = false;
                }
                else
                {
                    blockDrop = true;
                }
                break;
        }

        //겹치는거 체크

        blockOverlap = false;

        if (checkOverlap)
        {
            switch (mainRouletteContent.rouletteType)
            {
                case RouletteType.Default:
                    break;
                case RouletteType.StraightBet:
                    for (int i = 0; i < rouletteContentList.Count; i++)
                    {
                        if (rouletteContentList[i].index.SequenceEqual(index0) || rouletteContentList[i].index.SequenceEqual(index1)
                            || rouletteContentList[i].index.SequenceEqual(index2) || rouletteContentList[i].index.SequenceEqual(index3)
                            || rouletteContentList[i].index.SequenceEqual(index4) || rouletteContentList[i].index.SequenceEqual(index5)
                            || rouletteContentList[i].index.SequenceEqual(index6) || rouletteContentList[i].index.SequenceEqual(index7)
                            || rouletteContentList[i].index.SequenceEqual(index8))
                        {
                            if (rouletteContentList[i].isActive) blockOverlap = true;
                        }
                    }
                    break;
                case RouletteType.SplitBet_Horizontal:
                    for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++)
                    {
                        if (rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index0) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index1)
                            || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index2) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index3)
                            || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index4) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index5)
                            || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index6) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index7)
                            || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index8))
                        {
                            if (rouletteSplitContentList_Horizontal[i].isActive) blockOverlap = true;
                        }
                    }
                    break;
                case RouletteType.SplitBet_Vertical:
                    for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++)
                    {
                        if (rouletteSplitContentList_Vertical[i].index.SequenceEqual(index0) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index1)
                            || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index2) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index3)
                            || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index4) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index5)
                            || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index6) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index7)
                            || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index8))
                        {
                            if (rouletteSplitContentList_Vertical[i].isActive) blockOverlap = true;
                        }
                    }
                    break;
                case RouletteType.SquareBet:
                    for (int i = 0; i < rouletteSquareContentList.Count; i++)
                    {
                        if (rouletteSquareContentList[i].index.SequenceEqual(index0) || rouletteSquareContentList[i].index.SequenceEqual(index1)
                            || rouletteSquareContentList[i].index.SequenceEqual(index2) || rouletteSquareContentList[i].index.SequenceEqual(index3)
                            || rouletteSquareContentList[i].index.SequenceEqual(index4) || rouletteSquareContentList[i].index.SequenceEqual(index5)
                            || rouletteSquareContentList[i].index.SequenceEqual(index6) || rouletteSquareContentList[i].index.SequenceEqual(index7)
                            || rouletteSquareContentList[i].index.SequenceEqual(index8))
                        {
                            if (rouletteSquareContentList[i].isActive) blockOverlap = true;
                        }
                    }
                    break;
            }
        }
    }

    public void ExitBlock(BlockContent content)
    {
        ResetRouletteContent();

        switch (mainRouletteContent.rouletteType) //블럭 범위에 있는 모든 컨텐츠에 isActive 켜기
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                for (int i = 0; i < rouletteContentList.Count; i++)
                {
                    if (rouletteContentList[i].index.SequenceEqual(index0) || rouletteContentList[i].index.SequenceEqual(index1)
                        || rouletteContentList[i].index.SequenceEqual(index2) || rouletteContentList[i].index.SequenceEqual(index3)
                        || rouletteContentList[i].index.SequenceEqual(index4) || rouletteContentList[i].index.SequenceEqual(index5)
                        || rouletteContentList[i].index.SequenceEqual(index6) || rouletteContentList[i].index.SequenceEqual(index7)
                        || rouletteContentList[i].index.SequenceEqual(index8))
                    {
                        rouletteContentList[i].SetActiveTrue(content.blockType);
                    }
                }
                break;
            case RouletteType.SplitBet_Horizontal:
                for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++)
                {
                    if (rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index0) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index1)
                        || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index2) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index3)
                        || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index4) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index5)
                        || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index6) || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index7)
                        || rouletteSplitContentList_Horizontal[i].index.SequenceEqual(index8))
                    {
                        rouletteSplitContentList_Horizontal[i].SetActiveTrue(content.blockType);
                    }
                }
                break;
            case RouletteType.SplitBet_Vertical:
                for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++)
                {
                    if (rouletteSplitContentList_Vertical[i].index.SequenceEqual(index0) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index1)
                        || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index2) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index3)
                        || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index4) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index5)
                        || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index6) || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index7)
                        || rouletteSplitContentList_Vertical[i].index.SequenceEqual(index8))
                    {
                        rouletteSplitContentList_Vertical[i].SetActiveTrue(content.blockType);
                    }
                }
                break;
            case RouletteType.SquareBet:
                for (int i = 0; i < rouletteSquareContentList.Count; i++)
                {
                    if (rouletteSquareContentList[i].index.SequenceEqual(index0) || rouletteSquareContentList[i].index.SequenceEqual(index1)
                        || rouletteSquareContentList[i].index.SequenceEqual(index2) || rouletteSquareContentList[i].index.SequenceEqual(index3)
                        || rouletteSquareContentList[i].index.SequenceEqual(index4) || rouletteSquareContentList[i].index.SequenceEqual(index5)
                        || rouletteSquareContentList[i].index.SequenceEqual(index6) || rouletteSquareContentList[i].index.SequenceEqual(index7)
                        || rouletteSquareContentList[i].index.SequenceEqual(index8))
                    {
                        rouletteSquareContentList[i].SetActiveTrue(content.blockType);
                    }
                }
                break;
        }

        bettingMoney += (blockInformation.bettingPrice * blockInformation.size);
        ChangeMoney(-(blockInformation.bettingPrice * blockInformation.size));
    }


    public void ResetRouletteContent()
    {
        for (int i = 0; i < rouletteContentList.Count; i++)
        {
            rouletteContentList[i].ResetBackgroundColor();
        }

        for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++)
        {
            rouletteSplitContentList_Vertical[i].ResetBackgroundColor();
        }

        for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++)
        {
            rouletteSplitContentList_Horizontal[i].ResetBackgroundColor();
        }

        for (int i = 0; i < rouletteSquareContentList.Count; i++)
        {
            rouletteSquareContentList[i].ResetBackgroundColor();
        }
    }

    public void BetOptionCancleButton()
    {
        for (int i = 0; i < blockContentList.Count; i++)
        {
            blockContentList[i].ResetPos();
        }

        for (int i = 0; i < rouletteContentList.Count; i++)
        {
            rouletteContentList[i].SetActiveFalseAll();
        }

        for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++)
        {
            rouletteSplitContentList_Vertical[i].SetActiveFalseAll();
        }

        for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++)
        {
            rouletteSplitContentList_Horizontal[i].SetActiveFalseAll();
        }

        for (int i = 0; i < rouletteSquareContentList.Count; i++)
        {
            rouletteSquareContentList[i].SetActiveFalseAll();
        }

        ChangeMoney(bettingMoney);

        bettingMoney = 0;

        NotionManager.instance.UseNotion(NotionType.Cancle);
    }
}
