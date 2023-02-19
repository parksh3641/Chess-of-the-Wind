﻿using Photon.Pun;
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

    [Title("Developer")]
    public InputField inputTargetNumber;
    public Text developerInfo;

    [Space]
    [Title("Betting")]
    public int[] bettingValue;
    public int[] bettingSizeValue;
    public int[] bettingNumberList_NewBie = new int[3];
    public List<int> bettingNumberList_Gosu = new List<int>();

    [Space]
    [Title("Setting")]
    public GridLayoutGroup gridLayoutGroup;

    [Space]
    [Title("Timer")]
    private int timer = 0;
    public Text timerText;
    public Image timerFillAmount;

    [Space]
    [Title("Text")]
    public Text roomText;
    public Text moneyText;
    public Text bettingMoneyText;
    public Text targetText;
    public Text recordText;

    [Space]
    [Title("Value")]
    private int bettingTime = 0;
    private int bettingWaitTime = 0;

    public float money = 0;
    private float bettingMoney = 0;
    private float getMoney = 0;

    private int targetNumber = 0;
    private int targetQueenNumber = 0;
    private int blockCount = 0;

    private int gridConstraintCount = 0;

    string notion, notion2 = "";

    string[] insertBlock = new string[4];
    string[] deleteBlock = new string[2];

    [Space]
    [Title("Drag")]
    private Transform dragPos;
    private bool checkDrag = false;

    [Space]
    [Title("Bool")]
    public bool blockDrag = false;
    public bool blockDrop = false;
    public bool blockOverlap = false;

    [Space]
    [Title("Parent")]
    public GameObject blockRootParent;
    public GameObject blockParent;
    public GameObject blockGridParent_Developer;
    public GameObject blockGridParent_NewBie;
    public GameObject blockGridParent_Gosu;
    public Transform otherBlockParent;

    public GameObject targetObj;
    public GameObject targetQueenObj;

    [Space]
    [Title("Prefab")]
    public RouletteContent rouletteContent;
    public NumberContent numberContent;
    public BlockContent blockContent;
    public OtherBlockContent otherBlockContent;

    [Space]
    [Title("Grid")]
    public RectTransform rouletteContentTransform;
    public RectTransform rouletteContentTransformSplitBet_Vertical;
    public RectTransform rouletteContentTransformSplitBet_Horizontal;
    public RectTransform rouletteContentTransformSquareBet;
    public RectTransform numberContentTransform;

    public RectTransform blockContentTransform_Developer;
    public RectTransform blockContentTransform_NewBie;
    public RectTransform blockContentTransform_Gosu;

    public RectTransform rouletteContentTransform_NewBie;
    public RectTransform numberContentTransform_NewBie;

    [Space]
    [Title("NewBie")]
    List<RouletteContent> rouletteContentList_NewBie = new List<RouletteContent>();
    List<RouletteContent> numberContentList_NewBie = new List<RouletteContent>();

    [Space]
    [Title("Gosu")]
    List<RouletteContent> rouletteContentList_Gosu = new List<RouletteContent>();
    List<RouletteContent> rouletteSplitContentList_Vertical = new List<RouletteContent>();
    List<RouletteContent> rouletteSplitContentList_Horizontal = new List<RouletteContent>();
    List<RouletteContent> rouletteSquareContentList = new List<RouletteContent>();

    List<RouletteContent> allContentList = new List<RouletteContent>();

    List<RouletteContent> numberContentList = new List<RouletteContent>();

    [Space]
    [Title("Other")]
    List<BlockContent> blockContentList = new List<BlockContent>();
    List<OtherBlockContent> otherBlockContentList = new List<OtherBlockContent>();

    [Space]
    [Title("Target")]
    public List<RouletteContent> rouletteContentList_Target = new List<RouletteContent>();

    [Space]
    [Title("Manager")]
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

        inputTargetNumber.text = "";

        targetObj.SetActive(false);
        targetQueenObj.SetActive(false);

        ChangeResetBettingMoney();

        timer = 0;
        timerFillAmount.fillAmount = 0;

        targetText.text = "-";
        recordText.text = "";

        gridConstraintCount = gridLayoutGroup.constraintCount;

        bettingValue = new int[System.Enum.GetValues(typeof(BlockType)).Length];
        bettingSizeValue = new int[System.Enum.GetValues(typeof(BlockType)).Length];

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
            rouletteContentList_Gosu.Add(content);

            NumberContent numContent = Instantiate(numberContent);
            numContent.transform.parent = numberContentTransform;
            numContent.transform.localPosition = Vector3.zero;
            numContent.transform.localScale = Vector3.one;
            numContent.Initialize(i);
            numberContentList.Add(content);

            index++;

            allContentList.Add(content);
            bettingNumberList_Gosu.Add(0);
        }

        index = 0;
        count = 0;

        for (int i = 0; i < 25; i++) //NewBie
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
            content.transform.parent = rouletteContentTransform_NewBie;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockParent.transform, RouletteType.StraightBet, setIndex, i);
            rouletteContentList_NewBie.Add(content);

            NumberContent numContent = Instantiate(numberContent);
            numContent.transform.parent = numberContentTransform_NewBie;
            numContent.transform.localPosition = Vector3.zero;
            numContent.transform.localScale = Vector3.one;
            numContent.Initialize_NewBie(i);
            numberContentList_NewBie.Add(content);

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
            content.transform.parent = blockContentTransform_Developer;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockRootParent.transform, blockGridParent_Developer.transform, BlockType.Default + i + 1);
            blockContentList.Add(content);
        }

        BlockContent content2 = Instantiate(blockContent);
        content2.transform.parent = blockContentTransform_NewBie;
        content2.transform.localPosition = Vector3.zero;
        content2.transform.localScale = Vector3.one;
        content2.Initialize(this, blockRootParent.transform, blockGridParent_NewBie.transform, BlockType.Pawn);
        content2.gameObject.SetActive(true);
        blockContentList.Add(content2);

        int[] temp = new int[3];
        temp[0] = 1;
        temp[1] = 5;
        temp[2] = 9;
        for (int i = 0; i < 3; i++)
        {
            BlockContent content = Instantiate(blockContent);
            content.transform.parent = blockContentTransform_Gosu;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockRootParent.transform, blockGridParent_Gosu.transform, BlockType.Default + temp[i]);
            content.gameObject.SetActive(true);
            blockContentList.Add(content);
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
    }

    public void Initialize()
    {
        bettingTime = GameStateManager.instance.BettingTime;
        bettingWaitTime = GameStateManager.instance.BettingWaitTime;

        blockMotherInformation = blockDataBase.blockMotherInformation;

        ChangeMoney(playerDataBase.Gold);
    }

    public void GameStart_NewBie()
    {
        roomText.text = "초보방";

        developerInfo.text = "개발자 모드 (초보방)\n0 = 퀸 당첨\n1 = 검은색, 2 = 흰색 당첨\n빈칸 = 정상 진행";

        rouletteContentTransform.gameObject.SetActive(false);
        rouletteContentTransformSplitBet_Vertical.gameObject.SetActive(false);
        rouletteContentTransformSplitBet_Horizontal.gameObject.SetActive(false);
        rouletteContentTransformSquareBet.gameObject.SetActive(false);
        numberContentTransform.gameObject.SetActive(false);

        rouletteContentTransform_NewBie.gameObject.SetActive(true);
        numberContentTransform_NewBie.gameObject.SetActive(true);

        rouletteContentList_Target = rouletteContentList_NewBie;

        blockContentTransform_NewBie.gameObject.SetActive(true);
        blockContentTransform_Gosu.gameObject.SetActive(false);

        GameStart();
    }

    public void GameStart_Gosu()
    {
        roomText.text = "고수방";

        developerInfo.text = "개발자 모드 (고수방)\n0 = 퀸 당첨\n1 ~24 해당 숫자 당첨\n빈칸 = 정상 진행";

        rouletteContentTransform.gameObject.SetActive(true);
        rouletteContentTransformSplitBet_Vertical.gameObject.SetActive(true);
        rouletteContentTransformSplitBet_Horizontal.gameObject.SetActive(true);
        rouletteContentTransformSquareBet.gameObject.SetActive(true);
        numberContentTransform.gameObject.SetActive(true);

        rouletteContentTransform_NewBie.gameObject.SetActive(false);
        numberContentTransform_NewBie.gameObject.SetActive(false);

        rouletteContentList_Target = rouletteContentList_Gosu;

        blockContentTransform_NewBie.gameObject.SetActive(false);
        blockContentTransform_Gosu.gameObject.SetActive(true);

        GameStart();
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

        for (int i = 0; i < rouletteContentList_Target.Count; i++)
        {
            rouletteContentList_Target[i].SetActiveFalseAll();
        }

        for (int i = 0; i < bettingValue.Length; i ++)
        {
            bettingValue[i] = 0;
            bettingSizeValue[i] = 0;
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

    bool CheckDeveloper()
    {
        bool check = false;

        if(inputTargetNumber.text.Length > 0)
        {
            int number = int.Parse(inputTargetNumber.text.ToString());

            if(number <= 0)
            {
                targetNumber = 1;
                targetQueenNumber = 1;
            }
            else
            {
                targetQueenNumber = 0;

                if (number > 24)
                {
                    targetNumber = 24;
                }
                else
                {
                    targetNumber = number;
                }
            }

            check = true;
        }

        return check;
    }

    [PunRPC]
    void OpenRouletteView()
    {
        uIManager.dontTouchObj.SetActive(false);

        ShowBettingNumber();

        if (!CheckDeveloper())
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

        if (!CheckDeveloper())
        {
            targetNumber = int.Parse(target[0]);
            targetQueenNumber = int.Parse(target[1]);
        }

        Transform trans;

        if (targetQueenNumber == 1)
        {
            targetQueenObj.SetActive(true);
            targetQueenObj.transform.SetAsLastSibling();
            trans = rouletteContentList_Target[12].transform;
            targetQueenObj.transform.position = trans.position;

            targetText.text = "퀸";
        }

        targetObj.SetActive(true);
        targetObj.transform.SetAsLastSibling();

        if (targetNumber <= 12)
        {
            trans = rouletteContentList_Target[targetNumber - 1].transform;
            targetObj.transform.position = trans.position;
        }
        else if (targetNumber >= 13)
        {
            trans = rouletteContentList_Target[targetNumber].transform;
            targetObj.transform.position = trans.position;
        }

        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            if(targetNumber % 2 == 0)
            {
                targetText.text = "흰";
            }
            else
            {
                targetText.text = "검";
            }
        }
        else
        {
            targetText.text = targetNumber.ToString();
        }

        recordText.text += targetNumber + ", ";

        getMoney = 0;

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            if (targetQueenNumber == 1)
            {
                if(rouletteContentList_Target[12].isActive)
                {
                    Debug.Log("초보방 퀸 당첨");
                    getMoney += blockMotherInformation.straightBet_NewBie_Queen * blockDataBase.GetBlockInfomation(BlockType.Pawn).bettingPrice
                        + blockDataBase.GetBlockInfomation(BlockType.Pawn).bettingPrice;
                }
            }
            else
            {
                for (int i = 0; i < rouletteContentList_Target.Count; i++)
                {
                    if (rouletteContentList_Target[i].isActive)
                    {
                        if(targetNumber % 2 == 0 && rouletteContentList_Target[i].number % 2 == 0)
                        {
                            Debug.Log("초보방 흰색 당첨");
                            getMoney += blockMotherInformation.straightBet_NewBie * blockDataBase.GetBlockInfomation(BlockType.Pawn).bettingPrice
                                + blockDataBase.GetBlockInfomation(BlockType.Pawn).bettingPrice;
                        }
                        else if (targetNumber % 2 != 0 && rouletteContentList_Target[i].number % 2 != 0)
                        {
                            Debug.Log("초보방 검은색 당첨");
                            getMoney += blockMotherInformation.straightBet_NewBie * blockDataBase.GetBlockInfomation(BlockType.Pawn).bettingPrice
                                + blockDataBase.GetBlockInfomation(BlockType.Pawn).bettingPrice;
                        }
                    }
                }
            }
        }
        else
        {
            if (targetQueenNumber == 1)
            {
                Debug.Log("고수방 퀸 당첨");
                CheckQueenNumber();
            }
            else
            {
                Debug.Log("고수방 " + targetNumber + "번 당첨");
                CheckTargetNumber(targetNumber);
            }

            for (int i = 0; i < bettingSizeValue.Length; i++) //마지막에 당첨 안 된거 만큼 빼기
            {
                if (bettingSizeValue[i] > 0)
                {
                    int minus = blockDataBase.GetBlockInfomation(BlockType.Default + i).bettingPrice * i;
                    ChangeMoney(-minus);
                }
            }
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
        bettingSizeValue[(int)block.blockType] -= 1;

        switch (type)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                if(queen)
                {
                    getMoney += blockMotherInformation.queenStraightBet * block.bettingPrice + block.bettingPrice;
                }
                else
                {
                    getMoney += blockMotherInformation.straightBet * block.bettingPrice + block.bettingPrice;
                }

                break;
            case RouletteType.SplitBet_Horizontal:
                if (queen)
                {
                    getMoney += blockMotherInformation.queensplitBet * block.bettingPrice + block.bettingPrice;
                }
                else
                {
                    getMoney += blockMotherInformation.splitBet * block.bettingPrice + block.bettingPrice;
                }

                break;
            case RouletteType.SplitBet_Vertical:
                if (queen)
                {
                    getMoney += blockMotherInformation.queensplitBet * block.bettingPrice + block.bettingPrice;
                }
                else
                {
                    getMoney += blockMotherInformation.splitBet * block.bettingPrice + block.bettingPrice;
                }

                break;
            case RouletteType.SquareBet:
                if (queen)
                {
                    getMoney += blockMotherInformation.queensquareBet * block.bettingPrice + block.bettingPrice;
                }
                else
                {
                    getMoney += blockMotherInformation.squareBet * block.bettingPrice + block.bettingPrice;
                }

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
        for (int i = 0; i < rouletteContentList_Target.Count; i++)
        {
            if (rouletteContentList_Target[i].isActive)
            {
                if (i < 12)
                {
                    if (rouletteContentList_Target[i].number == target)
                    {
                        ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteContentList_Target[i].blockType), RouletteType.StraightBet, false);
                    }
                }
                else if(i >= 13)
                {
                    if (rouletteContentList_Target[i].number == target + 1)
                    {
                        ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteContentList_Target[i].blockType), RouletteType.StraightBet, false);
                    }
                }
            }
        }

        for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++) // 1 2 / 3 4
        {
            if (rouletteSplitContentList_Horizontal[i].isActive)
            {
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
        if (rouletteContentList_Target[12].isActive)
        {
            ChangeGetMoney(blockDataBase.GetBlockInfomation(rouletteContentList_Target[12].blockType), RouletteType.StraightBet, true);
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
            bettingSizeValue[(int)blockContent.blockType] = 0;
        }

        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < rouletteContentList_NewBie.Count; i++)
            {
                rouletteContentList_NewBie[i].SetActiveFalse(blockContent.blockType);
            }
        }
        else
        {
            for (int i = 0; i < allContentList.Count; i++)
            {
                allContentList[i].SetActiveFalse(blockContent.blockType);
            }
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

        index0[0] = rouletteContent.index[0] + blockDataBase.GetIndex0(blockContent.blockType, 0);
        index0[1] = rouletteContent.index[1] + blockDataBase.GetIndex0(blockContent.blockType, 1);

        index1[0] = rouletteContent.index[0] + blockDataBase.GetIndex1(blockContent.blockType, 0);
        index1[1] = rouletteContent.index[1] + blockDataBase.GetIndex1(blockContent.blockType, 1);

        index2[0] = rouletteContent.index[0] + blockDataBase.GetIndex2(blockContent.blockType, 0);
        index2[1] = rouletteContent.index[1] + blockDataBase.GetIndex2(blockContent.blockType, 1);

        index3[0] = rouletteContent.index[0] + blockDataBase.GetIndex3(blockContent.blockType, 0);
        index3[1] = rouletteContent.index[1] + blockDataBase.GetIndex3(blockContent.blockType, 1);

        index4[0] = rouletteContent.index[0] + blockDataBase.GetIndex4(blockContent.blockType, 0);
        index4[1] = rouletteContent.index[1] + blockDataBase.GetIndex4(blockContent.blockType, 1);

        index5[0] = rouletteContent.index[0] + blockDataBase.GetIndex5(blockContent.blockType, 0);
        index5[1] = rouletteContent.index[1] + blockDataBase.GetIndex5(blockContent.blockType, 1);

        index6[0] = rouletteContent.index[0] + blockDataBase.GetIndex6(blockContent.blockType, 0);
        index6[1] = rouletteContent.index[1] + blockDataBase.GetIndex6(blockContent.blockType, 1);

        index7[0] = rouletteContent.index[0] + blockDataBase.GetIndex7(blockContent.blockType, 0);
        index7[1] = rouletteContent.index[1] + blockDataBase.GetIndex7(blockContent.blockType, 1);

        index8[0] = rouletteContent.index[0] + blockDataBase.GetIndex8(blockContent.blockType, 0);
        index8[1] = rouletteContent.index[1] + blockDataBase.GetIndex8(blockContent.blockType, 1);

        ResetRouletteBackgroundColor();

        switch (mainRouletteContent.rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                for (int i = 0; i < rouletteContentList_Target.Count; i++)
                {
                    if (rouletteContentList_Target[i].index.SequenceEqual(index0) || rouletteContentList_Target[i].index.SequenceEqual(index1)
                        || rouletteContentList_Target[i].index.SequenceEqual(index2) || rouletteContentList_Target[i].index.SequenceEqual(index3)
                        || rouletteContentList_Target[i].index.SequenceEqual(index4) || rouletteContentList_Target[i].index.SequenceEqual(index5)
                        || rouletteContentList_Target[i].index.SequenceEqual(index6) || rouletteContentList_Target[i].index.SequenceEqual(index7)
                        || rouletteContentList_Target[i].index.SequenceEqual(index8))
                    {
                        rouletteContentList_Target[i].SetBackgroundColor(RouletteColorType.Yellow);
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
                    for (int i = 0; i < rouletteContentList_Target.Count; i++)
                    {
                        if (rouletteContentList_Target[i].index.SequenceEqual(index0) || rouletteContentList_Target[i].index.SequenceEqual(index1)
                            || rouletteContentList_Target[i].index.SequenceEqual(index2) || rouletteContentList_Target[i].index.SequenceEqual(index3)
                            || rouletteContentList_Target[i].index.SequenceEqual(index4) || rouletteContentList_Target[i].index.SequenceEqual(index5)
                            || rouletteContentList_Target[i].index.SequenceEqual(index6) || rouletteContentList_Target[i].index.SequenceEqual(index7)
                            || rouletteContentList_Target[i].index.SequenceEqual(index8))
                        {
                            if (rouletteContentList_Target[i].isActive) blockOverlap = true;
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
                for (int i = 0; i < rouletteContentList_Target.Count; i++)
                {
                    if (rouletteContentList_Target[i].index.SequenceEqual(index0) || rouletteContentList_Target[i].index.SequenceEqual(index1)
                        || rouletteContentList_Target[i].index.SequenceEqual(index2) || rouletteContentList_Target[i].index.SequenceEqual(index3)
                        || rouletteContentList_Target[i].index.SequenceEqual(index4) || rouletteContentList_Target[i].index.SequenceEqual(index5)
                        || rouletteContentList_Target[i].index.SequenceEqual(index6) || rouletteContentList_Target[i].index.SequenceEqual(index7)
                        || rouletteContentList_Target[i].index.SequenceEqual(index8))
                    {
                        rouletteContentList_Target[i].SetActiveTrue(blockContent.blockType);
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
        bettingSizeValue[(int)blockContent.blockType] = blockInformation.size;

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
        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < bettingNumberList_NewBie.Length; i++)
            {
                bettingNumberList_NewBie[i] = 0;
            }

            for (int i = 0; i < rouletteContentList_NewBie.Count; i ++)
            {
                if(rouletteContentList_NewBie[i].isActive)
                {
                    if(rouletteContentList_NewBie[i].number == 13) //퀸일때
                    {
                        bettingNumberList_NewBie[2] = 1;
                    }
                    else
                    {
                        if (rouletteContentList_NewBie[i].number % 2 == 0) //흰
                        {
                            bettingNumberList_NewBie[0] = 1;
                        }
                        else
                        {
                            bettingNumberList_NewBie[1] = 1; //검
                        }
                    }

                    break;
                }
            }

        }
        else
        {
            for (int i = 0; i < bettingNumberList_Gosu.Count; i++)
            {
                bettingNumberList_Gosu[i] = 0;
            }

            for (int i = 0; i < allContentList.Count; i++)
            {
                if (allContentList[i].isActive)
                {
                    switch (allContentList[i].rouletteType)
                    {
                        case RouletteType.Default:
                            break;
                        case RouletteType.StraightBet:
                            bettingNumberList_Gosu[allContentList[i].number - 1] = 1;
                            break;
                        case RouletteType.SplitBet_Horizontal:
                            bettingNumberList_Gosu[allContentList[i].number - 1] = 1;

                            if (allContentList[i].number - 1 + 5 < allContentList.Count)
                            {
                                bettingNumberList_Gosu[allContentList[i].number - 1 + 5] = 1;
                            }
                            break;
                        case RouletteType.SplitBet_Vertical:
                            int revision = (allContentList[i].number - 1) / 4;

                            bettingNumberList_Gosu[allContentList[i].number - 1 + revision] = 1;
                            bettingNumberList_Gosu[allContentList[i].number - 1 + revision + 1] = 1;

                            break;
                        case RouletteType.SquareBet:
                            int revision2 = (allContentList[i].number - 1) / 4;

                            bettingNumberList_Gosu[allContentList[i].number - 1 + revision2] = 1;
                            bettingNumberList_Gosu[allContentList[i].number - 1 + revision2 + 1] = 1;
                            bettingNumberList_Gosu[allContentList[i].number - 1 + revision2 + 5] = 1;
                            bettingNumberList_Gosu[allContentList[i].number - 1 + revision2 + 6] = 1;

                            break;
                    }
                }
            }
        }
    }

    public void ResetRouletteBackgroundColor()
    {
        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < rouletteContentList_NewBie.Count; i++)
            {
                rouletteContentList_NewBie[i].ResetBackgroundColor();
            }
        }
        else
        {
            for (int i = 0; i < allContentList.Count; i++)
            {
                allContentList[i].ResetBackgroundColor();
            }
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
            bettingSizeValue[i] = 0;
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
                content.transform.position = rouletteContentList_Target[int.Parse(block[2]) - 1].transform.position;
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
