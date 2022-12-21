using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public RouletteContent mainRouletteContent;
    public Transform targetBlockContent;

    public BlockType blockType = BlockType.Default;
    public BlockInformation blockInformation = new BlockInformation();

    public int[] bettingValue;
    public List<int> bettingNumberList = new List<int>();

    [Title("Developer")]
    public int autoTargetNumber = -1;
    public int setMoney = 50000;
    public int setTimer = 16;
    public int waitTimer = 10;
    public int bounsCount = 3;
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

    string[] insertBlock = new string[4];
    string[] deleteBlock = new string[2];

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
    public Transform otherBlockParent;

    public GameObject dontTouchObj;
    public GameObject targetObj;
    public GameObject waitingObj;

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

    List<RouletteContent> allContentList = new List<RouletteContent>();

    List<RouletteContent> numberContentList = new List<RouletteContent>();
    List<BlockContent> blockContentList = new List<BlockContent>();
    public List<BlockContent> otherBlockContentList = new List<BlockContent>();

    [Header("Betting")]
    private List<int[]> splitHorizontalIndexList = new List<int[]>();
    private List<int[]> splitVerticalIndexList = new List<int[]>();
    private List<int[]> squareIndexList = new List<int[]>();

    [Header("Manager")]
    public SoundManager soundManager;
    public RouletteManager rouletteManager;
    public TalkManager talkManager;
    public CharacterManager characterManager;

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
        waitingObj.SetActive(false);

        loginView.SetActive(true);
        mainView.SetActive(false);

        ChangeMoney(setMoney);
        ChangeResetBettingMoney();

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

            allContentList.Add(content);
            bettingNumberList.Add(0);
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

            allContentList.Add(content);
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

            allContentList.Add(content);
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

            allContentList.Add(content);
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

    private void SetSplitIndex()
    {
        int index = 0;
        int count = 0;
        int number = 0;

        for (int i = 0; i < 20; i++)
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

    private void SetSquareIndex()
    {
        int index = 0;
        int count = 0;
        int number = 0;

        for (int i = 0; i < 16; i++)
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

    public void GameStart()
    {
        loginView.SetActive(false);
        mainView.SetActive(true);

        talkManager.Initialize();
        recordText.text = "";

        dontTouchObj.SetActive(true);
        targetObj.SetActive(false);

        ClearOtherPlayerBlock();

        timer = waitTimer;
        timerFillAmount.fillAmount = 1;

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(WaitTimerCoroution());
        }

        Invoke("CheckPlayer", 0.5f);
    }

    private void CheckPlayer()
    {
        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        switch (ht["Status"])
        {
            case "Betting":
                Waiting(true);
                break;
            case "Roulette":
                OpenRouletteView();
                Waiting(true);
                break;
            case "Bouns":
                OpenRouletteView();
                Waiting(true);
                break;
        }
    }

    public void GameStop()
    {
        StopAllCoroutines();

        dontTouchObj.SetActive(false);
        waitingObj.SetActive(false);

        rouletteManager.CloseRouletteView();
        characterManager.DeleteAllPlayer();
        soundManager.StopAllSFX();

        ResetRouletteContent();

        for (int i = 0; i < blockContentList.Count; i++)
        {
            if (blockContentList[i].isDrag)
            {
                blockContentList[i].TimeOver();
                break;
            }
        }

        BetOptionCancleButton();

        loginView.SetActive(true);
        mainView.SetActive(false);
    }

    void ClearOtherPlayerBlock()
    {
        for (int i = 0; i < otherBlockContentList.Count; i++)
        {
            Destroy(otherBlockContentList[i].gameObject);
        }
        otherBlockContentList.Clear();
    }

    public void Waiting(bool check)
    {
        waitingObj.SetActive(check);
    }

    IEnumerator TimerCoroution()
    {
        if (timer <= 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("OpenRouletteView", RpcTarget.All);
            }
            yield break;
        }

        timer -= 1;

        PV.RPC("ChangeTimer", RpcTarget.All, timer);

        timerFillAmount.fillAmount = timer / ((setTimer - 1) * 1.0f);
        timerText.text = timer.ToString();

        yield return new WaitForSeconds(1f);
        StartCoroutine(TimerCoroution());
    }

    IEnumerator WaitTimerCoroution()
    {
        if (timer <= 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("RestartGame", RpcTarget.All);
            }
            yield break;
        }

        timer -= 1;

        PV.RPC("ChangeWaitTimer", RpcTarget.All, timer);

        timerFillAmount.fillAmount = timer / ((waitTimer - 1) * 1.0f);
        timerText.text = timer + "초 뒤에 게임을 시작합니다";

        yield return new WaitForSeconds(1f);
        StartCoroutine(WaitTimerCoroution());
    }

    [PunRPC]
    void RestartGame()
    {
        for (int i = 0; i < blockContentList.Count; i++)
        {
            blockContentList[i].ResetPos();
        }

        for (int i = 0; i < allContentList.Count; i++)
        {
            allContentList[i].SetActiveFalseAll();
        }

        dontTouchObj.SetActive(false);
        targetObj.SetActive(false);

        timer = setTimer;
        timerFillAmount.fillAmount = 1;

        NotionManager.instance.UseNotion(NotionType.GoBetting);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Betting" } });
        }

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        switch (ht["Status"])
        {
            case "Waiting":
                Waiting(false);
                break;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerCoroution());
        }
    }

    [PunRPC]
    void ChangeTimer(int number)
    {
        timer = number;

        timerFillAmount.fillAmount = timer / ((setTimer - 1) * 1.0f);
        timerText.text = timer.ToString();
    }

    [PunRPC]
    void ChangeWaitTimer(int number)
    {
        timer = number;

        timerFillAmount.fillAmount = timer / ((waitTimer - 1) * 1.0f);
        timerText.text = timer + "초 뒤에 게임을 시작합니다";
    }

    [PunRPC]
    void OpenRouletteView()
    {
        CheckBettingNumber();

        if (autoTargetNumber == -1)
        {
            mainView.SetActive(false);

            rouletteManager.Initialize(rouletteContentList.Count + 1);
            rouletteManager.CheckBettingNumber();
        }
        else
        {
            targetNumber = autoTargetNumber;
        }
    }

    public void CloseRouletteView(int number)
    {
        mainView.SetActive(true);
        dontTouchObj.SetActive(true);

        targetNumber = number;

        recordText.text += targetNumber + ", ";
        targetText.text = targetNumber.ToString();

        CheckTargetNumber();

        ResetRouletteContent();

        ClearOtherPlayerBlock();

        for (int i = 0; i < blockContentList.Count; i++)
        {
            if (blockContentList[i].isDrag)
            {
                blockContentList[i].TimeOver();
                break;
            }
        }

        timer = waitTimer;
        timerFillAmount.fillAmount = 1;

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(WaitTimerCoroution());
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
            ChangeMoney((int)getMoney);

            string notion, notion2 = "";

            if (getMoney > bettingMoney)
            {
                notion = "<color=#00C8FF>" + PlayerPrefs.GetString("NickName") + " 님 +" + (int)(getMoney - bettingMoney) + "만큼 증가\n남은 금액 : " + money + "</color>";
                notion2 = (int)(getMoney - bettingMoney) + " 만큼 획득";

                NotionManager.instance.UseNotion(notion2, ColorType.Green);
            }
            else
            {
                notion = "<color=#FF0000>" + PlayerPrefs.GetString("NickName") + " 님 " + (int)Mathf.Abs((bettingMoney - getMoney)) + "만큼 감소\n남은 금액 : " + money + "</color>";
                notion2 = (int)Mathf.Abs((bettingMoney - getMoney)) + " 만큼 감소";

                NotionManager.instance.UseNotion(notion2, ColorType.Red);
            }

            PV.RPC("ChatRPC", RpcTarget.All, notion);
        }

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

        string notion2 = "₩ " + blockInformation.bettingPrice.ToString() + " x " + blockInformation.size + " 베팅";

        NotionManager.instance.UseNotion(notion2, ColorType.Green);

        PV.RPC("ChatRPC", RpcTarget.All, notion);

        insertBlock = new string[4];

        insertBlock[0] = mainRouletteContent.rouletteType.ToString();
        insertBlock[1] = blockContent.blockType.ToString();
        insertBlock[2] = mainRouletteContent.number.ToString();
        insertBlock[3] = PlayerPrefs.GetString("NickName");

        PV.RPC("ShowOtherPlayerBlock", RpcTarget.All, insertBlock);
    }

    void CheckBettingNumber()
    {
        for (int i = 0; i < bettingNumberList.Count; i++)
        {
            bettingNumberList[i] = 0;
        }

        for (int i = 0; i < allContentList.Count; i ++)
        {
            if(allContentList[i].isActive)
            {
                switch (allContentList[i].rouletteType)
                {
                    case RouletteType.Default:
                        break;
                    case RouletteType.StraightBet:
                        bettingNumberList[allContentList[i].number - 1] = 1;
                        break;
                    case RouletteType.SplitBet_Horizontal:
                        bettingNumberList[allContentList[i].number - 1] = 1;

                        if(allContentList[i].number - 1 + 5 < allContentList.Count)
                        {
                            bettingNumberList[allContentList[i].number - 1 + 5] = 1;
                        }
                        break;
                    case RouletteType.SplitBet_Vertical:
                        int revision = (allContentList[i].number - 1) / 4;

                        bettingNumberList[allContentList[i].number - 1 + revision] = 1;
                        bettingNumberList[allContentList[i].number - 1 + revision + 1] = 1;

                        break;
                    case RouletteType.SquareBet:
                        int revision2 = (allContentList[i].number - 1) / 4;

                        bettingNumberList[allContentList[i].number - 1 + revision2] = 1;
                        bettingNumberList[allContentList[i].number - 1 + revision2 + 1] = 1;
                        bettingNumberList[allContentList[i].number - 1 + revision2 + 5] = 1;
                        bettingNumberList[allContentList[i].number - 1 + revision2 + 6] = 1;

                        break;
                }
            }
        }
    }

    public void ResetRouletteContent()
    {
        for (int i = 0; i < allContentList.Count; i++)
        {
            allContentList[i].ResetBackgroundColor();
        }
    }

    public void CancleBetting(BlockType type)
    {
        ResetRouletteContent();

        deleteBlock = new string[2];

        deleteBlock[0] = type.ToString();
        deleteBlock[1] = PlayerPrefs.GetString("NickName");

        PV.RPC("HideOtherPlayerBlock", RpcTarget.All, deleteBlock);
    }

    public void BetOptionCancleButton()
    {
        for (int i = 0; i < blockContentList.Count; i++)
        {
            blockContentList[i].ResetPos();
        }

        for (int i = 0; i < allContentList.Count; i++)
        {
            allContentList[i].SetActiveFalseAll();
        }

        ChangeMoney(bettingMoney);
        ChangeResetBettingMoney();

        NotionManager.instance.UseNotion(NotionType.Cancle);
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        talkManager.UseNotion(msg);
    }

    [PunRPC]
    void ShowOtherPlayerBlock(string[] block)
    {
        RouletteType rouletteType = (RouletteType)System.Enum.Parse(typeof(RouletteType), block[0]);
        BlockType blockType = (BlockType)System.Enum.Parse(typeof(BlockType), block[1]);

        for (int i = 0; i < otherBlockContentList.Count; i++)
        {
            if (otherBlockContentList[i].blockType == blockType)
            {
                Destroy(otherBlockContentList[i].gameObject);
                otherBlockContentList.Remove(otherBlockContentList[i]);
            }
        }

        BlockContent content = Instantiate(blockContent);
        content.transform.parent = otherBlockParent.transform;
        content.transform.localPosition = Vector3.zero;
        content.transform.localScale = Vector3.one;
        content.ShowInitialize(blockType, block[3]);
        otherBlockContentList.Add(content);


        switch (rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                content.transform.position = rouletteContentList[int.Parse(block[2]) - 1].transform.position;
                break;
            case RouletteType.SplitBet_Horizontal:
                content.transform.position = rouletteSplitContentList_Horizontal[int.Parse(block[2]) - 1].transform.position;
                break;
            case RouletteType.SplitBet_Vertical:
                content.transform.position = rouletteSplitContentList_Vertical[int.Parse(block[2]) - 1].transform.position;
                break;
            case RouletteType.SquareBet:
                content.transform.position = rouletteSquareContentList[int.Parse(block[2]) - 1].transform.position;
                break;
        }
    }

    [PunRPC]
    void HideOtherPlayerBlock(string[] block)
    {
        BlockType blockType = (BlockType)System.Enum.Parse(typeof(BlockType), block[0]);

        for (int i = 0; i < otherBlockContentList.Count; i ++)
        {
            if(otherBlockContentList[i].nickName.Equals(block[1]) && otherBlockContentList[i].blockType == blockType)
            {
                Destroy(otherBlockContentList[i].gameObject);
                otherBlockContentList.Remove(otherBlockContentList[i]);
            }
        }
    }
}
