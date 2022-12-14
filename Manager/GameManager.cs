using Photon.Pun;
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
    public Transform targetBlockContent;

    public BlockType blockType = BlockType.Default;
    public BlockInformation blockInformation = new BlockInformation();

    public int[] bettingValue;

    [Title("Developer")]
    public int autoTargetNumber = -1;
    public int setMoney = 50000;
    public int setTimer = 16;
    public bool checkOverlap = false;

    [Title("View")]
    public GameObject loginView;
    public GameObject mainView;

    [Title("Setting")]
    public GridLayoutGroup gridLayoutGroup;

    [Title("Timer")]
    private int timer = 0;
    public Text timerText;
    public Image timerFillAmount;

    [Title("Text")]
    public Text moneyText;
    public Text bettingMoneyText;
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

    public SoundManager soundManager;
    public RouletteManager rouletteManager;
    public PointerManager pointerManager;
    BlockDataBase blockDataBase;

    public PhotonView PV;

    int[] index0, index1, index2, index3, index4, index5, index6, index7, index8 = new int[2];

    private void Awake()
    {
        if(blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;

        instance = this;

        Application.targetFrameRate = 60;

        dontTouchObj.SetActive(false);
        targetObj.SetActive(false);

        loginView.SetActive(true);
        mainView.SetActive(false);

        ChangeMoney(setMoney);
        ChangeBettingMoney(0);

        timer = 0;
        timerFillAmount.fillAmount = 0;

        targetText.text = "-";
        recordText.text = "";

        gridConstraintCount = gridLayoutGroup.constraintCount;

        bettingValue = new int[System.Enum.GetValues(typeof(BlockType)).Length];

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

        SetSplitIndex();
        SetSquareIndex();
    }

    [PunRPC]
    public void GameStart()
    {
        loginView.SetActive(false);
        mainView.SetActive(true);

        timer = setTimer;
        timerFillAmount.fillAmount = 1;

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerCoroution());
        }

        pointerManager.Initialize();
    }

    public void GameStop()
    {
        StopAllCoroutines();

        loginView.SetActive(true);
        mainView.SetActive(false);

        rouletteManager.CloseRouletteView();
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

    public void ChangeMoney(float plus)
    {
        money += plus;
        moneyText.text = "현재 돈 : ₩ " + money.ToString();
    }

    void ChangeBettingMoney(float plus)
    {
        bettingMoney += plus;
        bettingMoneyText.text = "베팅 금액 : ₩ " + bettingMoney.ToString();
    }

    void ChangeResetBettingMoney()
    {
        bettingMoney = 0;
        bettingMoneyText.text = "베팅 금액 : ₩ " + bettingMoney.ToString();
    }

    [PunRPC]
    void ChangeTimer(int number)
    {
        timer = number;

        timerFillAmount.fillAmount = timer / ((setTimer - 1) * 1.0f);
        timerText.text = timer.ToString();
    }

    IEnumerator TimerCoroution()
    {
        if (timer <= 0)
        {
            //OpenRouletteView();
            PV.RPC("OpenRouletteView", RpcTarget.All);
            yield break;
        }

        timer -= 1;

        PV.RPC("ChangeTimer", RpcTarget.All, timer);

        //timerFillAmount.fillAmount = timer / ((setTimer - 1) * 1.0f);
        //timerText.text = timer.ToString();

        yield return new WaitForSeconds(1f);
        StartCoroutine(TimerCoroution());
    }

    [PunRPC]
    void OpenRouletteView()
    {
        if (autoTargetNumber == -1)
        {
            mainView.SetActive(false);

            rouletteManager.Initialize(rouletteContentList.Count + 1);
        }
        else
        {
            targetNumber = autoTargetNumber;
        }
    }

    public void CloseRouletteView(int number)
    {
        mainView.SetActive(true);

        targetNumber = number;

        recordText.text += targetNumber + ", ";
        targetText.text = targetNumber.ToString();

        CheckTargetNumber();

        dontTouchObj.SetActive(true);

        ResetRouletteContent();

        for (int i = 0; i < blockContentList.Count; i++)
        {
            if (blockContentList[i].isDrag)
            {
                blockContentList[i].TimeOver();
                break;
            }
        }

        timerText.text = "5초 뒤에 다시 시작합니다.";

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(RandomTargetNumber());
        }
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

        yield return new WaitForSeconds(5f);

        PV.RPC("RestartGame", RpcTarget.All);
    }

    [PunRPC]
    void RestartGame()
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

        timer = setTimer;
        timerFillAmount.fillAmount = 1;

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerCoroution());
        }
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

        if (bettingMoney == 0)
        {
            NotionManager.instance.UseNotion("시간 초과 !", ColorType.Red);
        }
        else
        {
            if (getMoney > bettingMoney)
            {
                NotionManager.instance.UseNotion(((int)getMoney - bettingMoney) + " 만큼 돈을 땄어요 !", ColorType.Green);
            }
            else
            {
                NotionManager.instance.UseNotion(Mathf.Abs((bettingMoney - getMoney)) + " 만큼 돈을 잃었어요 ㅠㅠ", ColorType.Red);
            }
        }

        ChangeMoney((int)getMoney);
        ChangeResetBettingMoney();
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

    public void EnterBlock(RouletteContent rouletteContent, BlockContent blockContent)
    {
        mainRouletteContent = rouletteContent;

        blockInformation = blockDataBase.GetBlockInfomation(blockContent.blockType);

        targetBlockContent = blockContent.transform;

        if(bettingValue[(int)blockContent.blockType] > 0)
        {
            ChangeMoney(bettingValue[(int)blockContent.blockType]);
            ChangeBettingMoney(-bettingValue[(int)blockContent.blockType]);

            //string notion = bettingValue[(int)blockContent.blockType] + "만큼 배팅을 취소했습니다.\n총 배팅 금액은 " + bettingMoney + "입니다.";

            //TalkManager.instance.UseNotion(notion, Color.red);

            bettingValue[(int)blockContent.blockType] = 0;
        }

        switch (mainRouletteContent.rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                for (int i = 0; i < rouletteContentList.Count; i++)
                {
                    for (int j = 0; j < rouletteContentList[i].blockType.Length; j++)
                    {
                        if (rouletteContentList[i].blockType[j] == blockContent.blockType)
                        {
                            rouletteContentList[i].SetActiveFalse(blockContent.blockType);
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
            case BlockType.One:
                index0[0] = rouletteContent.index[0];
                index0[1] = rouletteContent.index[1];

                index1[0] = rouletteContent.index[0];
                index1[1] = rouletteContent.index[1];

                index2[0] = rouletteContent.index[0];
                index2[1] = rouletteContent.index[1];

                index3[0] = rouletteContent.index[0];
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

    public void ExitBlock(BlockContent blockContent)
    {
        soundManager.PlaySFX(GameSfxType.Click);

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
                        rouletteContentList[i].SetActiveTrue(blockContent.blockType);
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
                        rouletteSplitContentList_Horizontal[i].SetActiveTrue(blockContent.blockType);
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
                        rouletteSplitContentList_Vertical[i].SetActiveTrue(blockContent.blockType);
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
                        rouletteSquareContentList[i].SetActiveTrue(blockContent.blockType);
                    }
                }
                break;
        }

        bettingValue[(int)blockContent.blockType] = blockInformation.bettingPrice * blockInformation.size;

        ChangeMoney(-blockInformation.bettingPrice * blockInformation.size);
        ChangeBettingMoney(blockInformation.bettingPrice * blockInformation.size);

        string notion = "<color=#00FF00>" + PlayerPrefs.GetString("NickName") + " 님이 ₩ " + blockInformation.bettingPrice.ToString()
            + " x " + blockInformation.size + " 베팅\n총 배팅 : " + bettingMoney + "</color>";

        NotionManager.instance.UseNotion(notion, ColorType.Green);

        PV.RPC("ChatRPC", RpcTarget.All, notion);
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
        ChangeResetBettingMoney();

        NotionManager.instance.UseNotion(NotionType.Cancle);
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        TalkManager.instance.UseNotion(msg);
    }
}
