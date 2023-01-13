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
    Transform targetBlockContent;

    public BlockType blockType = BlockType.Default;
    public BlockInformation blockInformation = new BlockInformation();
    BlockMotherInformation blockMotherInformation;

    public int[] bettingValue;
    public List<int> bettingNumberList = new List<int>();

    private int bettingTime = 0;
    private int bettingWaitTime = 0;

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
    private int targetQueenNumber = 0;
    private int gridConstraintCount = 0;

    string notion, notion2 = "";

    string[] insertBlock = new string[4];
    string[] deleteBlock = new string[2];

    [Title("Drag")]
    private Transform dragPos;
    private bool checkDrag = false;

    [Title("Bool")]
    public bool blockDrag = false;
    public bool blockDrop = false;
    public bool blockOverlap = false;

    bool straightReward = false;
    bool horizontalReward = false;
    bool verticalReward = false;
    bool squareReward = false;

    public GameObject blockRootParent;
    public GameObject blockParent;
    public GameObject blockGridParent;
    public Transform otherBlockParent;

    public GameObject targetObj;
    public GameObject targetQueenObj;

    [Title("Prefab")]
    public RouletteContent rouletteContent;
    public RectTransform rouletteContentTransform;

    public RectTransform rouletteContentTransformSplitBet_Vertical;
    public RectTransform rouletteContentTransformSplitBet_Horizontal;

    public RectTransform rouletteContentTransformSquareBet;

    public NumberContent numberContent;
    public RectTransform numberContentTransform;

    public BlockContent blockContent;
    public OtherBlockContent otherBlockContent;
    public RectTransform blockContentTransform;

    List<RouletteContent> rouletteContentList = new List<RouletteContent>();
    List<RouletteContent> rouletteSplitContentList_Vertical = new List<RouletteContent>();
    List<RouletteContent> rouletteSplitContentList_Horizontal = new List<RouletteContent>();
    List<RouletteContent> rouletteSquareContentList = new List<RouletteContent>();

    List<RouletteContent> allContentList = new List<RouletteContent>();

    List<RouletteContent> numberContentList = new List<RouletteContent>();
    List<BlockContent> blockContentList = new List<BlockContent>();
    public List<OtherBlockContent> otherBlockContentList = new List<OtherBlockContent>();

    [Header("Betting")]
    private List<int[]> splitHorizontalIndexList = new List<int[]>();
    private List<int[]> splitVerticalIndexList = new List<int[]>();
    private List<int[]> squareIndexList = new List<int[]>();

    [Header("Manager")]
    public UIManager uIManager;
    public SoundManager soundManager;
    public RouletteManager rouletteManager;
    public TalkManager talkManager;
    public CharacterManager characterManager;

    PlayerDataBase playerDataBase;
    BlockDataBase blockDataBase;

    public PhotonView PV;

    int[] index0, index1, index2, index3, index4, index5, index6, index7, index8 = new int[2];

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;

        instance = this;

        targetObj.SetActive(false);
        targetQueenObj.SetActive(false);

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

        //for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        //{
        //    BlockContent content = Instantiate(blockContent);
        //    content.transform.parent = blockContentTransform;
        //    content.transform.localPosition = Vector3.zero;
        //    content.transform.localScale = Vector3.one;
        //    content.Initialize(this, blockRootParent.transform, blockGridParent.transform, BlockType.Default + i + 1);
        //    blockContentList.Add(content);
        //}

        BlockContent content2 = Instantiate(blockContent);
        content2.transform.parent = blockContentTransform;
        content2.transform.localPosition = Vector3.zero;
        content2.transform.localScale = Vector3.one;
        content2.Initialize(this, blockRootParent.transform, blockGridParent.transform, BlockType.Default + 2);
        blockContentList.Add(content2);
    }

    public void Initialize()
    {
        bettingTime = GameStateManager.instance.BettingTime;
        bettingWaitTime = GameStateManager.instance.BettingWaitTime;

        blockMotherInformation = blockDataBase.blockMotherInformation;

        ChangeMoney(playerDataBase.Coin);
    }

    public void GameStart()
    {
        SetTotalMoney();

        uIManager.OnGameStart();

        talkManager.Initialize();
        recordText.text = "";

        targetObj.SetActive(false);
        targetQueenObj.SetActive(false);

        ClearOtherPlayerBlock();

        timer = bettingWaitTime;
        timerFillAmount.fillAmount = 1;

        if (PhotonNetwork.IsMasterClient)
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
                uIManager.SetWaiting(true);
                break;
            case "Roulette":
                uIManager.SetWaiting(true);
                OpenRouletteView();
                rouletteManager.SpectatorRoulette();
                break;
            case "Bouns":
                uIManager.SetWaiting(true);
                OpenRouletteView();
                rouletteManager.SpectatorRoulette();
                break;
        }
    }

    void ClearOtherPlayerBlock()
    {
        for (int i = 0; i < otherBlockContentList.Count; i++)
        {
            Destroy(otherBlockContentList[i].gameObject);
        }
        otherBlockContentList.Clear();
    }

    IEnumerator TimerCoroution()
    {
        if (timer <= 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("DelayRoulette", RpcTarget.All);
            }

            yield return new WaitForSeconds(1f);

            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("OpenRouletteView", RpcTarget.All);
            }

            yield break;
        }

        timer -= 1;

        PV.RPC("ChangeTimer", RpcTarget.All, timer);

        timerFillAmount.fillAmount = timer / ((bettingTime - 1) * 1.0f);
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

        timerFillAmount.fillAmount = timer / ((bettingWaitTime - 1) * 1.0f);
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

        for(int i = 0; i < bettingValue.Length; i ++)
        {
            bettingValue[i] = 0;
        }

        ClearOtherPlayerBlock();

        uIManager.OnRestartGame();
        targetObj.SetActive(false);
        targetQueenObj.SetActive(false);

        timer = bettingTime;
        timerFillAmount.fillAmount = 1;

        NotionManager.instance.UseNotion(NotionType.GoBetting);

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        switch (ht["Status"])
        {
            case "Waiting":
                uIManager.SetWaiting(false);
                break;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerCoroution());
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Betting" } });
        }
    }

    [PunRPC]
    void ChangeTimer(int number)
    {
        timer = number;

        timerFillAmount.fillAmount = timer / ((bettingTime - 1) * 1.0f);
        timerText.text = timer.ToString();
    }

    [PunRPC]
    void ChangeWaitTimer(int number)
    {
        timer = number;

        timerFillAmount.fillAmount = timer / ((bettingWaitTime - 1) * 1.0f);
        timerText.text = timer + "초 뒤에 게임을 시작합니다";
    }

    [PunRPC]
    void DelayRoulette()
    {
        uIManager.dontTouchObj.SetActive(true);

        for (int i = 0; i < blockContentList.Count; i++)
        {
            if (blockContentList[i].isDrag)
            {
                blockContentList[i].TimeOver();
                break;
            }
        }

        ResetRouletteBackgroundColor();
    }

    [PunRPC]
    void OpenRouletteView()
    {
        uIManager.dontTouchObj.SetActive(false);

        ShowBettingNumber();

        if (!GameStateManager.instance.AutoTarget)
        {
            rouletteManager.Initialize();
        }
        else
        {
            string[] init = new string[2];

            CloseRouletteView(init);
        }
    }

    public void CloseRouletteView(string[] target)
    {
        uIManager.CloseRouletteView();

        if (GameStateManager.instance.AutoTarget)
        {
            targetNumber = GameStateManager.instance.AutoTargetNumber;
            targetQueenNumber = 1;
        }
        else
        {
            targetNumber = int.Parse(target[0]);
            targetQueenNumber = int.Parse(target[1]);
        }

        straightReward = false;
        horizontalReward = false;
        verticalReward = false;
        squareReward = false;

        Transform trans;

        if (targetQueenNumber == 1)
        {
            targetQueenObj.SetActive(true);
            targetQueenObj.transform.SetAsLastSibling();
            trans = rouletteContentList[12].transform;
            targetQueenObj.transform.position = trans.position;

            targetText.text = "퀸";
        }

        targetObj.SetActive(true);
        targetObj.transform.SetAsLastSibling();

        if (targetNumber <= 12)
        {
            trans = rouletteContentList[targetNumber - 1].transform;
            targetObj.transform.position = trans.position;
        }
        else if (targetNumber >= 13)
        {
            trans = rouletteContentList[targetNumber].transform;
            targetObj.transform.position = trans.position;
        }

        targetText.text = targetNumber.ToString();
        recordText.text += targetNumber + ", ";

        getMoney = 0;

        CheckTargetNumber(targetNumber);

        if (targetQueenNumber == 1)
        {
            Debug.Log("퀸 당첨");
            CheckQueenNumber();
        }

        ChangeMoney((int)getMoney);

        if (bettingMoney == 0)
        {
            notion2 = "변동이 없습니다";
            NotionManager.instance.UseNotion(notion2, ColorType.Green);
        }
        else
        {
            if (getMoney > bettingMoney)
            {
                notion = "<color=#00FF00>+" + (int)(getMoney - bettingMoney) + "   " + GameStateManager.instance.NickName + "</color>";
                notion2 = "+" + (int)(getMoney - bettingMoney);

                NotionManager.instance.UseNotion(notion2, ColorType.Green);
            }
            else
            {
                notion = "<color=#FF0000>-" + (int)Mathf.Abs((bettingMoney - getMoney)) + "   " + GameStateManager.instance.NickName + "</color>";
                notion2 = "-" + (int)Mathf.Abs((bettingMoney - getMoney));

                NotionManager.instance.UseNotion(notion2, ColorType.Red);

                SetMinusMoney((int)Mathf.Abs((bettingMoney - getMoney)));
            }

            PV.RPC("ChatRPC", RpcTarget.All, notion);
        }

        ResetRouletteBackgroundColor();

        ChangeResetBettingMoney();

        for (int i = 0; i < blockContentList.Count; i++)
        {
            if (blockContentList[i].isDrag)
            {
                blockContentList[i].TimeOver();
                break;
            }
        }

        timer = bettingWaitTime;
        timerFillAmount.fillAmount = 1;

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(WaitTimerCoroution());
        }
    }

    public void ChangeGetMoney(BlockInformation block, RouletteType type, bool queen)
    {
        switch (type)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                if(queen)
                {
                    getMoney += ((blockMotherInformation.straightBet + blockMotherInformation.queenBet) / block.size) * block.bettingPrice * block.magnification;
                }
                else
                {
                    getMoney += (blockMotherInformation.straightBet / block.size) * block.bettingPrice * block.magnification;
                }

                straightReward = true;

                break;
            case RouletteType.SplitBet_Horizontal:
                if (queen)
                {
                    getMoney += ((blockMotherInformation.splitBet + blockMotherInformation.queenBet) / block.size) * block.bettingPrice * block.magnification;
                }
                else
                {
                    getMoney += (blockMotherInformation.splitBet / block.size) * block.bettingPrice * block.magnification;
                }

                horizontalReward = true;

                break;
            case RouletteType.SplitBet_Vertical:
                if (queen)
                {
                    getMoney += ((blockMotherInformation.splitBet + blockMotherInformation.queenBet) / block.size) * block.bettingPrice * block.magnification;
                }
                else
                {
                    getMoney += (blockMotherInformation.splitBet / block.size) * block.bettingPrice * block.magnification;
                }

                verticalReward = true;

                break;
            case RouletteType.SquareBet:
                if (queen)
                {
                    getMoney += ((blockMotherInformation.squareBet + blockMotherInformation.queenBet) / block.size) * block.bettingPrice * block.magnification;
                }
                else
                {
                    getMoney += (blockMotherInformation.squareBet / block.size) * block.bettingPrice * block.magnification;
                }

                squareReward = true;

                break;
        }

        Debug.Log(type + " / " + getMoney + " / " + queen);
    }

    public void ChangeMoney(float plus)
    {
        money += plus;
        moneyText.text = money.ToString();
    }

    void ChangeBettingMoney(float plus)
    {
        bettingMoney += plus;
        bettingMoneyText.text = bettingMoney.ToString();
    }

    void ChangeResetBettingMoney()
    {
        bettingMoney = 0;
        bettingMoneyText.text = bettingMoney.ToString();
    }

    private void CheckTargetNumber(int target)
    {
        for (int i = 0; i < rouletteContentList.Count; i++)
        {
            if (rouletteContentList[i].isActive)
            {
                if (straightReward) return;

                if (i < 12)
                {
                    if (rouletteContentList[i].number == target)
                    {
                        ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteContentList[i].blockType), RouletteType.StraightBet, false);
                    }
                }
                else if(i >= 13)
                {
                    if (rouletteContentList[i].number == target + 1)
                    {
                        ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteContentList[i].blockType), RouletteType.StraightBet, false);
                    }
                }
            }
        }

        for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++) // 1 2 / 3 4
        {
            if (rouletteSplitContentList_Horizontal[i].isActive)
            {
                if (horizontalReward) return;

                switch (i)
                {
                    case 0:
                        if (target == 1 || target == 6)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 1:
                        if (target == 2 || target == 7)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 2:
                        if (target == 3 || target == 8)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 3:
                        if (target == 4 || target == 9)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 4:
                        if (target == 5 || target == 10)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 5:
                        if (target == 6 || target == 11)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 6:
                        if (target == 7 || target == 12)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 7:
                        if (target == 8)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 8:
                        if (target == 9 || target == 13)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 9:
                        if (target == 10 || target == 14)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 10:
                        if (target == 11 || target == 15)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 11:
                        if (target == 12 || target == 16)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 12:
                        if (target == 17)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 13:
                        if (target == 13 || target == 18)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 14:
                        if (target == 14 || target == 19)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 15:
                        if (target == 15 || target == 20)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 16:
                        if (target == 16 || target == 21)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 17:
                        if (target == 17 || target == 22)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 18:
                        if (target == 18 || target == 23)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 19:
                        if (target == 19 || target == 24)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[i].blockType), RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                }
            }
        }

        for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++) // 1 6 / 2 7
        {
            if (rouletteSplitContentList_Vertical[i].isActive)
            {
                if (verticalReward) return;

                switch (i)
                {
                    case 0:
                        if (target == 1 || target == 2)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 1:
                        if (target == 2 || target == 3)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 2:
                        if (target == 3 || target == 4)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 3:
                        if (target == 4 || target == 5)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 4:
                        if (target == 6 || target == 7)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 5:
                        if (target == 7 || target == 8)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 6:
                        if (target == 8 || target == 9)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 7:
                        if (target == 9 || target == 10)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 8:
                        if (target == 11 || target == 12)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 9:
                        if (target == 12)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 10:
                        if (target == 13)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 11:
                        if (target == 13 || target == 14)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 12:
                        if (target == 15 || target == 16)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 13:
                        if (target == 16 || target == 17)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 14:
                        if (target == 17 || target == 18)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 15:
                        if (target == 18 || target == 19)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 16:
                        if (target == 20 || target == 21)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 17:
                        if (target == 21 || target == 22)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 18:
                        if (target == 22 || target == 23)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 19:
                        if (target == 23 || target == 24)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[i].blockType), RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                }
            }
        }

        for (int i = 0; i < rouletteSquareContentList.Count; i++) //1 2 6 7
        {
            if (rouletteSquareContentList[i].isActive)
            {
                if (squareReward) return;

                switch (i)
                {
                    case 0:
                        if (target == 1 || target == 2 || target == 6 || target == 7)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 1:
                        if (target == 2 || target == 3 || target == 7 || target == 8)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 2:
                        if (target == 3 || target == 4 || target == 8 || target == 9)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 3:
                        if (target == 4 || target == 5 || target == 9 || target == 10)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 4:
                        if (target == 6 || target == 7 || target == 11 || target == 12)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 5:
                        if (target == 7 || target == 8 || target == 12)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 6:
                        if (target == 8 || target == 9 || target == 13)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 7:
                        if (target == 9 || target == 10 || target == 14 || target == 15)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 8:
                        if (target == 11 || target == 12 || target == 15 || target == 16)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 9:
                        if (target == 12 || target == 16 || target == 17)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 10:
                        if (target == 13 || target == 17 || target == 18)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 11:
                        if (target == 13 || target == 14 || target == 18 || target == 19)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 12:
                        if (target == 15 || target == 16 || target == 20 || target == 21)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 13:
                        if (target == 16 || target == 17 || target == 21 || target == 22)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 14:
                        if (target == 17 || target == 18 || target == 22 || target == 23)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                    case 15:
                        if (target == 18 || target == 19 || target == 23 || target == 24)
                        {
                            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[i].blockType), RouletteType.SquareBet, false);
                        }
                        break;
                }
            }
        }
    }

    private void CheckQueenNumber()
    {
        if (rouletteContentList[12].isActive)
        {
            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteContentList[12].blockType), RouletteType.StraightBet, true);
        }

        if (rouletteSplitContentList_Horizontal[7].isActive)
        {
            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[7].blockType), RouletteType.SplitBet_Horizontal, true);
        }

        if (rouletteSplitContentList_Horizontal[12].isActive)
        {
            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Horizontal[12].blockType), RouletteType.SplitBet_Horizontal, true);
        }

        if (rouletteSplitContentList_Vertical[9].isActive)
        {
            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[9].blockType), RouletteType.SplitBet_Vertical, true);
        }

        if (rouletteSplitContentList_Vertical[10].isActive)
        {
            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSplitContentList_Vertical[10].blockType), RouletteType.SplitBet_Vertical, true);
        }

        if (rouletteSquareContentList[5].isActive)
        {
            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[5].blockType), RouletteType.SquareBet, true);
        }

        if (rouletteSquareContentList[6].isActive)
        {
            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[6].blockType), RouletteType.SquareBet, true);
        }

        if (rouletteSquareContentList[9].isActive)
        {
            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[9].blockType), RouletteType.SquareBet, true);
        }

        if (rouletteSquareContentList[10].isActive)
        {
            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteSquareContentList[10].blockType), RouletteType.SquareBet, true);
        }
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
                    ResetRouletteBackgroundColor();
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
                    ResetRouletteBackgroundColor();
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

            bettingValue[(int)blockContent.blockType] = 0;
        }

        for (int i = 0; i < allContentList.Count; i++)
        {
            allContentList[i].SetActiveFalse(blockContent.blockType);
        }

        //switch (mainRouletteContent.rouletteType)
        //{
        //    case RouletteType.Default:
        //        break;
        //    case RouletteType.StraightBet:
        //        for (int i = 0; i < rouletteContentList.Count; i++)
        //        {
        //            rouletteContentList[i].SetActiveFalse(blockContent.blockType);
        //        }
        //        break;
        //    case RouletteType.SplitBet_Horizontal:
        //        for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++)
        //        {
        //            rouletteSplitContentList_Horizontal[i].SetActiveFalse(blockContent.blockType);
        //        }
        //        break;
        //    case RouletteType.SplitBet_Vertical:
        //        for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++)
        //        {
        //            rouletteSplitContentList_Vertical[i].SetActiveFalse(blockContent.blockType);
        //        }
        //        break;
        //    case RouletteType.SquareBet:
        //        for (int i = 0; i < rouletteSquareContentList.Count; i++)
        //        {
        //            rouletteSquareContentList[i].SetActiveFalse(blockContent.blockType);
        //        }
        //        break;
        //}

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

        ResetRouletteBackgroundColor();

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

        if (GameStateManager.instance.BlockOverlap)
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

        ResetRouletteBackgroundColor();

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

        string notion = blockInformation.bettingPrice.ToString() + " x " + blockInformation.size + " 블록 배치";

        NotionManager.instance.UseNotion(notion, ColorType.Green);

        insertBlock = new string[4];

        insertBlock[0] = mainRouletteContent.rouletteType.ToString();
        insertBlock[1] = blockContent.blockType.ToString();
        insertBlock[2] = mainRouletteContent.number.ToString();
        insertBlock[3] = GameStateManager.instance.NickName;

        PV.RPC("ShowOtherPlayerBlock", RpcTarget.Others, insertBlock);
    }

    void ShowBettingNumber()
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

    public void ResetRouletteBackgroundColor()
    {
        for (int i = 0; i < allContentList.Count; i++)
        {
            allContentList[i].ResetBackgroundColor();
        }
    }

    public void CancleBetting(BlockType type)
    {
        ResetRouletteBackgroundColor();

        deleteBlock = new string[2];

        deleteBlock[0] = type.ToString();
        deleteBlock[1] = GameStateManager.instance.NickName;

        PV.RPC("HideOtherPlayerBlock", RpcTarget.Others, deleteBlock);
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

    public void BetOptionLeaveGame()
    {
        StopAllCoroutines();

        rouletteManager.CloseRouletteView();
        characterManager.DeleteAllPlayer();
        soundManager.StopAllSFX();

        ResetRouletteBackgroundColor();

        for (int i = 0; i < blockContentList.Count; i++)
        {
            if (blockContentList[i].isDrag)
            {
                blockContentList[i].TimeOver();
                break;
            }
        }

        for (int i = 0; i < bettingValue.Length; i++)
        {
            bettingValue[i] = 0;
        }

        BetOptionCancleButton();

        uIManager.OnGameStop();
    }

    [PunRPC]
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

        OtherBlockContent content = Instantiate(otherBlockContent);
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



    public void SetTotalMoney() //베팅 시작 전 나의 금액 저장
    {
        int myNumber = 0;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName.Equals(GameStateManager.instance.NickName))
            {
                myNumber = i;
            }
        }

        switch (myNumber)
        {
            case 0:
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Total", money } });
                break;
            case 1:
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Total", money } });
                break;
            case 2:
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player3_Total", money } });
                break;
            case 3:
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player4_Total", money } });
                break;
        }
    }

    void SetMinusMoney(int number) //잃었을 경우 나의 금액 저장
    {
        int myNumber = 0;
        int minus = 0;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName.Equals(GameStateManager.instance.NickName))
            {
                myNumber = i;
            }
        }

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        switch (myNumber)
        {
            case 0:
                minus = int.Parse(ht["Player1_Minus"].ToString());
                break;
            case 1:
                minus = int.Parse(ht["Player2_Minus"].ToString());
                break;
            case 2:
                minus = int.Parse(ht["Player3_Minus"].ToString());
                break;
            case 3:
                minus = int.Parse(ht["Player4_Minus"].ToString());
                break;
        }

        minus += number;

        switch (myNumber)
        {
            case 0:
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Minus", minus } });
                break;
            case 1:
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Minus", minus } });
                break;
            case 2:
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player3_Minus", minus } });
                break;
            case 3:
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player4_Minus", minus } });
                break;
        }
    }

}
