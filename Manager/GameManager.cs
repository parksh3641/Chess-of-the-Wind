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
    public RouletteContent mainRouletteContent;
    Transform targetBlockContent;

    public BlockType blockType = BlockType.Default;
    public BlockInformation blockInformation = new BlockInformation();
    BlockMotherInformation blockMotherInformation;

    [Title("Developer")]
    public InputField inputTargetNumber;
    public Text developerInfo;

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
    public Text targetText;
    public Text recordText;

    [Space]
    [Title("MainText")]
    public Text moneyText;
    public Text bettingMoneyText;
    public Text otherMoneyText;

    [Space]
    [Title("Betting")]
    public int[] bettingValue = new int[4]; //각 블럭에 배팅 금액
    public int[] bettingSizeList = new int[4]; //각 블럭에 사이즈
    public int[] bettingList = new int[4];//블럭이 배팅했는지 여부
    public int[] bettingMinusList = new int[4]; //당첨 안된 블럭 빼기

    public int[] bettingNumberList_NewBie = new int[3]; //어디 블럭에 배팅했는지 (나중에 룰렛에 표시할 용도)
    public List<int> bettingNumberList_Gosu = new List<int>();

    public int[] otherBettingNumberList_Newbie = new int[3];
    public List<int> otherBettingNumberList_Gosu = new List<int>();

    [Space]
    [Title("Value")]
    private int bettingTime = 0;
    private int bettingWaitTime = 0;

    public int money = 0; //보유 코인
    public int otherMoney = 0; //상대방 보유 코인
    private int stakes = 0; //판돈

    private int bettingMoney = 0; //배치한 블럭 크기
    private float plusMoney = 0; //획득한 돈
    private int tempMoney = 0;

    private int bettingAiMoney = 0;
    private float plusAiMoney = 0; //Ai가 획득한 돈
    private int tempAiMoney = 0;

    private int[] compareMoney = new int[2];

    private string otherBettingList = "";

    private int targetNumber = 0;
    private int targetQueenNumber = 0;
    private int blockCount = 0;

    private int gridConstraintCount = 0;

    string worldNotion, localNotion = "";

    string[] insertBlock = new string[5];
    string[] deleteBlock = new string[2];

    [Space]
    [Title("Drag")]
    private Transform dragPos;
    private bool checkDrag = false;

    [Space]
    [Title("Bool")]
    public bool aiMode = false;
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

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

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
    public BlockContent newbieBlockContent;
    List<BlockContent> blockContentList = new List<BlockContent>();
    List<OtherBlockContent> otherBlockContentList = new List<OtherBlockContent>();

    [Space]
    [Title("Target")]
    public List<RouletteContent> rouletteContentList_Target = new List<RouletteContent>();


    BlockClass blockClassArmor = new BlockClass();
    BlockClass blockClassWeapon = new BlockClass();
    BlockClass blockClassShield = new BlockClass();
    BlockClass blockClassNewbie = new BlockClass();

    [Space]
    [Title("Manager")]
    public NetworkManager networkManager;
    public UIManager uIManager;
    public SoundManager soundManager;
    public RouletteManager rouletteManager;
    public AiManager aiManager;
    public CharacterManager characterManager;
    public RandomBoxManager randomBoxManager;

    UpgradeDataBase upgradeDataBase;
    PlayerDataBase playerDataBase;
    BlockDataBase blockDataBase;

    public PhotonView PV;

    int[] index0, index1, index2, index3, index4, index5, index6, index7, index8 = new int[2];

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;

        bettingValue = new int[4];
        bettingSizeList = new int[4];
        bettingList = new int[4];
        bettingMinusList = new int[4];
        bettingNumberList_NewBie = new int[3];

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
            rouletteContentList_Gosu.Add(content);

            NumberContent numContent = Instantiate(numberContent);
            numContent.transform.parent = numberContentTransform;
            numContent.transform.localPosition = Vector3.zero;
            numContent.transform.localScale = Vector3.one;
            numContent.Initialize(i);
            numberContentList.Add(content);

            index++;

            allContentList.Add(content);
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

        //for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        //{
        //    BlockContent content = Instantiate(blockContent);
        //    content.transform.parent = blockContentTransform_Developer;
        //    content.transform.localPosition = Vector3.zero;
        //    content.transform.localScale = Vector3.one;
        //    content.Initialize(this, blockRootParent.transform, blockGridParent_Developer.transform, BlockType.Default + i + 1);
        //    blockContentList.Add(content);
        //}

        blockContentList.Clear();

        newbieBlockContent = Instantiate(blockContent);
        newbieBlockContent.transform.parent = blockContentTransform_NewBie;
        newbieBlockContent.transform.localPosition = Vector3.zero;
        newbieBlockContent.transform.localScale = Vector3.one;
        newbieBlockContent.Initialize(this, blockRootParent.transform, blockGridParent_NewBie.transform);
        newbieBlockContent.gameObject.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            BlockContent content = Instantiate(blockContent);
            content.transform.parent = blockContentTransform_Gosu;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockRootParent.transform, blockGridParent_Gosu.transform);
            content.gameObject.SetActive(false);
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

    private void Start()
    {
        GameReset();
    }

    private void OnApplicationQuit()
    {
        if(GameStateManager.instance.Playing)
        {
            int number = (int)(money * 0.1f);

            GameStateManager.instance.Penalty = number;
        }
    }

    public void Initialize()
    {
        bettingTime = GameStateManager.instance.BettingTime;
        bettingWaitTime = GameStateManager.instance.BettingWaitTime;

        blockMotherInformation = blockDataBase.blockMotherInformation;
    }

    void GameReset()
    {
        StopAllCoroutines();

        targetObj.SetActive(false);
        targetQueenObj.SetActive(false);

        targetText.text = "-";

        timer = bettingWaitTime;
        timerText.text = timer.ToString();
        timerFillAmount.fillAmount = 1;

        moneyText.text = moneyText.text = "GOLD <size=25>0</size>";
        bettingMoneyText.text = "0";
        otherMoneyText.text = moneyText.text = "GOLD <size=25>0</size>";

        RecordManager.instance.Initialize();
        recordText.text = "";

        inputTargetNumber.text = "";

        ClearOtherPlayerBlock();

        for (int i = 0; i < otherBettingNumberList_Newbie.Length; i++)
        {
            otherBettingNumberList_Newbie[i] = 0;
        }

        otherBettingNumberList_Gosu.Clear();
    }

    public void Penalty()
    {
        int number = (int)(money * 0.1f);

        GameStateManager.instance.Penalty = number;

        uIManager.OpenDisconnectedView();
    }

    public void ExitRoom()
    {
        StopAllCoroutines();

        GameReset();

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

        BetOptionCancleButton();

        uIManager.GameEnd();
    }

    public void GameStart_Newbie()
    {
        roomText.text = "초보방";

        developerInfo.text = "0 = 퀸 당첨\n1 = 검은색, 2 = 흰색 당첨\n25 = 보너스 룰렛 실행\n빈칸 = 정상 진행";

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

        newbieBlockContent.gameObject.SetActive(true);

        blockClassNewbie = playerDataBase.GetBlockClass(playerDataBase.Newbie);

        int value = upgradeDataBase.GetUpgradeValue(blockClassNewbie.rankType).GetValueNumber(blockClassNewbie.level);

        newbieBlockContent.InGame_Initialize(blockClassNewbie, 3, value);

        bettingValue[3] = upgradeDataBase.GetUpgradeValue(blockClassNewbie.rankType).GetValueNumber(blockClassNewbie.level);
        bettingSizeList[3] = blockDataBase.GetBlockInfomation(blockClassNewbie.blockType).GetSize();

        GameStart();
    }

    public void GameStart_Gosu()
    {
        roomText.text = "고수방";

        developerInfo.text = "0 = 퀸 당첨\n1 ~24 = 해당 숫자 당첨\n25 = 보너스 룰렛 실행\n빈칸 = 정상 진행";

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

        for(int i = 0; i < 3; i ++)
        {
            blockContentList[i].gameObject.SetActive(true);
        }

        blockClassArmor = playerDataBase.GetBlockClass(playerDataBase.Armor);
        blockClassWeapon = playerDataBase.GetBlockClass(playerDataBase.Weapon);
        blockClassShield = playerDataBase.GetBlockClass(playerDataBase.Shield);
        blockClassNewbie = playerDataBase.GetBlockClass(playerDataBase.Newbie);

        int value = upgradeDataBase.GetUpgradeValue(blockClassArmor.rankType).GetValueNumber(blockClassArmor.level);
        int value2 = upgradeDataBase.GetUpgradeValue(blockClassWeapon.rankType).GetValueNumber(blockClassWeapon.level);
        int value3 = upgradeDataBase.GetUpgradeValue(blockClassShield.rankType).GetValueNumber(blockClassShield.level);

        blockContentList[0].InGame_Initialize(blockClassArmor, 0, value);
        blockContentList[1].InGame_Initialize(blockClassWeapon, 1, value2);
        blockContentList[2].InGame_Initialize(blockClassShield, 2, value3);

        bettingValue[0] = upgradeDataBase.GetUpgradeValue(blockClassArmor.rankType).GetValueNumber(blockClassArmor.level);
        bettingValue[1] = upgradeDataBase.GetUpgradeValue(blockClassWeapon.rankType).GetValueNumber(blockClassWeapon.level);
        bettingValue[2] = upgradeDataBase.GetUpgradeValue(blockClassShield.rankType).GetValueNumber(blockClassShield.level);

        bettingSizeList[0] = blockDataBase.GetBlockInfomation(blockClassArmor.blockType).GetSize();
        bettingSizeList[1] = blockDataBase.GetBlockInfomation(blockClassWeapon.blockType).GetSize();
        bettingSizeList[2] = blockDataBase.GetBlockInfomation(blockClassShield.blockType).GetSize();

        GameStart();
    }

    public void GameStart_Newbie_Ai()
    {
        aiManager.Initialize();

        aiMode = true;

        characterManager.AddPlayer("인공지능");

        GameStart_Newbie();

    }

    public void GameStart_Gosu_Ai()
    {
        aiManager.Initialize();

        aiMode = true;

        characterManager.AddPlayer("인공지능");

        GameStart_Gosu();
    }

    void SetStakes() //판돈 설정
    {
        stakes = GameStateManager.instance.Stakes;

        money = stakes;
        otherMoney = stakes;

        moneyText.text = "GOLD <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
        otherMoneyText.text = "GOLD <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";
    }

    public void GameStart()
    {
        GameReset();

        ResetBettingMoney();
        SetStakes();
        SetTotalMoney();

        timer = bettingWaitTime;
        timerFillAmount.fillAmount = 1;

        if (PhotonNetwork.IsMasterClient)
        {
            rouletteManager.CreateObj();
            StartCoroutine(WaitTimerCoroution());
        }

        //Invoke("CheckPlayerState", 0.5f);
    }

    //private void CheckPlayerState() //게임이 이미 시작중일 경우 대기상태
    //{
    //    Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

    //    switch (ht["Status"])
    //    {
    //        case "Betting":
    //            uIManager.SetWaitingView(true);
    //            break;
    //        case "Roulette":
    //            uIManager.SetWaitingView(true);
    //            OpenRouletteView();
    //            rouletteManager.SpectatorRoulette();
    //            break;
    //        case "Bouns":
    //            uIManager.SetWaitingView(true);
    //            OpenRouletteView();
    //            rouletteManager.SpectatorRoulette();
    //            break;
    //    }
    //}

    void CheckWinnerPlayer() //게임 누가 승리했는지 체크
    {
        if(money <= 0 && otherMoney <= 0) //둘다 0원일때
        {
            GameEnd(2);
            PV.RPC("GameEnd", RpcTarget.Others, 2);
        }
        else if(money <= 0) //내돈이 다 떨어졌을 때
        {
            GameEnd(1);
            PV.RPC("GameEnd", RpcTarget.Others, 1);
        }
        else if(otherMoney <= 0) //상대방 돈이 다 떨어졌을때
        {
            GameEnd(0);
            PV.RPC("GameEnd", RpcTarget.Others, 0);
        }
        else
        {
            Debug.Log("승패가 나지 않았습니다.");
        }
    }

    [PunRPC]
    void GameEnd(int number)
    {
        uIManager.dontTouchObj.SetActive(false);

        networkManager.LeaveRoom();

        if (number == 0)
        {
            Debug.Log("승리");
        }
        else if(number == 1)
        {
            Debug.Log("패배");
        }
        else
        {
            Debug.Log("무승부");
        }

        GameStateManager.instance.Playing = false;

        soundManager.StopLoopSFX(GameSfxType.Roulette);

        uIManager.OpenResultView(number, money - stakes);
    }

    private void ClearOtherPlayerBlock()
    {
        for (int i = 0; i < otherBlockContentList.Count; i++)
        {
            Destroy(otherBlockContentList[i].gameObject);
        }

        otherBlockContentList.Clear();
    }

    IEnumerator WaitTimerCoroution()
    {
        uIManager.dontTouchObj.SetActive(true);

        while(timer > 0)
        {
            timer -= 1;
            PV.RPC("ChangeWaitTimer", RpcTarget.Others, timer);

            timerFillAmount.fillAmount = timer / ((bettingWaitTime - 1) * 1.0f);
            timerText.text = timer + "초 뒤에 게임을 시작합니다";
            yield return waitForSeconds;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("RestartGame", RpcTarget.All);
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
    void RestartGame()
    {
        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            newbieBlockContent.ResetPos();
        }
        else
        {
            for (int i = 0; i < blockContentList.Count; i++)
            {
                blockContentList[i].ResetPos();
            }
        }

        for (int i = 0; i < allContentList.Count; i++)
        {
            allContentList[i].SetActiveFalseAll();
        }

        for (int i = 0; i < rouletteContentList_Target.Count; i++)
        {
            rouletteContentList_Target[i].SetActiveFalseAll();
        }

        for (int i = 0; i < bettingList.Length; i++)
        {
            bettingList[i] = 0;
        }

        ClearOtherPlayerBlock();

        if(aiMode) aiManager.RestartGame();

        targetObj.SetActive(false);
        targetQueenObj.SetActive(false);

        timer = bettingTime;
        timerFillAmount.fillAmount = 1;

        NotionManager.instance.UseNotion(NotionType.GoBetting);

        uIManager.SetWaitingView(false);

        //Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        //switch (ht["Status"])
        //{
        //    case "Waiting":
        //        uIManager.SetWaiting(false);
        //        break;
        //}

        uIManager.dontTouchObj.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerCoroution());
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Betting" } });
        }
    }

    IEnumerator TimerCoroution()
    {
        if (timer <= 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("DelayRoulette", RpcTarget.All);
            }

            yield return new WaitForSeconds(1.5f);

            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("OpenRouletteView", RpcTarget.All);
            }

            yield break;
        }

        if (aiMode)
        {
            if (timer <= 5)
            {
                aiManager.PutBlock();
            }
        }

        timer -= 1;

        PV.RPC("ChangeTimer", RpcTarget.Others, timer);

        timerFillAmount.fillAmount = timer / ((bettingTime - 1) * 1.0f);
        timerText.text = timer.ToString();

        yield return new WaitForSeconds(1f);
        StartCoroutine(TimerCoroution());
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

        ShowBettingNumber();
    }

    bool CheckDeveloper()
    {
        bool check = false;

        if(inputTargetNumber.text.Length > 0)
        {
            int number = int.Parse(inputTargetNumber.text.ToString());

            targetQueenNumber = 0;

            if (number <= 0)
            {
                targetNumber = 1;
                targetQueenNumber = 1;

                check = true;
            }
            else if(number <= 24)
            {
                targetNumber = number;

                check = true;
            }
            else
            {
                GameStateManager.instance.CheckBouns = true;

                check = false;
            }
        }

        return check;
    }

    [PunRPC]
    void OpenRouletteView()
    {
        uIManager.dontTouchObj.SetActive(false);

        money -= bettingMoney;

        if(bettingMoney > 0)
        {
            SetMinusMoney(bettingMoney);

            PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, bettingMoney);

            Debug.LogError(bettingMoney + "만큼 배팅했습니다.");
        }

        RecordManager.instance.GameRecordInitialize();

        if (!CheckDeveloper())
        {
            rouletteManager.Initialize();
        }
        else
        {
            string[] init = new string[2];

            init[0] = targetNumber.ToString();
            init[1] = targetQueenNumber.ToString();

            GameResult(init);
        }
    }

    public void GameResult(string[] target) //게임이 한판 끝났을 경우
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
            targetObj.SetActive(false);
            targetQueenObj.SetActive(true);
            targetQueenObj.transform.SetAsLastSibling();
            trans = rouletteContentList_Target[12].transform;
            targetQueenObj.transform.position = trans.position;

            targetText.text = "퀸";

            Debug.Log("퀸 당첨!");
        }
        else
        {
            targetObj.SetActive(true);
            targetObj.transform.SetAsLastSibling();
        }

        if (targetNumber > 12)
        {
            trans = rouletteContentList_Target[targetNumber].transform;
            targetObj.transform.position = trans.position;
        }
        else
        {
            trans = rouletteContentList_Target[targetNumber - 1].transform;
            targetObj.transform.position = trans.position;
        }

        if (GameStateManager.instance.GameType == GameType.NewBie)
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

        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            recordText.text += targetText.text + ", ";
        }
        else
        {
            recordText.text += targetNumber + ", ";
        }

        plusMoney = 0; //획득한 돈
        plusAiMoney = 0;

        tempMoney = 0;
        tempAiMoney = 0;

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            if(aiMode)
            {
                if(otherBettingNumberList_Newbie[2] == 1 && targetQueenNumber == 1)
                {
                    plusAiMoney += blockMotherInformation.straightBet_NewBie_Queen * aiManager.bettingValue[0] 
                        + aiManager.bettingValue[0];

                    Debug.Log("Ai 초보방 퀸 당첨");
                }
                else
                {
                    if (targetNumber % 2 == 0 && aiManager.blockPos % 2 == 0)
                    {
                        plusAiMoney += blockMotherInformation.straightBet_NewBie * aiManager.bettingValue[0] 
                            + aiManager.bettingValue[0];

                        Debug.Log("Ai 초보방 흰색 당첨");
                    }
                    else if (targetNumber % 2 != 0 && aiManager.blockPos % 2 != 0)
                    {
                        plusAiMoney += blockMotherInformation.straightBet_NewBie * aiManager.bettingValue[0] 
                            + aiManager.bettingValue[0];

                        Debug.Log("Ai 초보방 검은색 당첨");
                    }
                }
            }


            if (targetQueenNumber == 1)
            {
                if(rouletteContentList_Target[12].isActive)
                {
                    plusMoney += blockMotherInformation.straightBet_NewBie_Queen * bettingValue[3]
                        + bettingValue[3];

                    Debug.Log("초보방 퀸 당첨");
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
                            plusMoney += blockMotherInformation.straightBet_NewBie * bettingValue[3]
                        + bettingValue[3];

                            Debug.Log("초보방 흰색 당첨");
                        }
                        else if (targetNumber % 2 != 0 && rouletteContentList_Target[i].number % 2 != 0)
                        {
                            plusMoney += blockMotherInformation.straightBet_NewBie * bettingValue[3]
                        + bettingValue[3];

                            Debug.Log("초보방 검은색 당첨");
                        }
                    }
                }
            }
        }
        else
        {
            if (targetQueenNumber == 1)
            {
                CheckQueenNumber();

                Debug.Log("고수방 퀸 당첨");
            }
            else
            {
                Debug.Log("고수방 " + targetNumber + "번 당첨");

                if (targetNumber > 12)
                {
                    targetNumber += 1;
                }

                CheckTargetNumber(targetNumber);
            }

            for (int i = 0; i < bettingMinusList.Length; i++) //마지막에 당첨 안 된거 만큼 빼기
            {
                plusMoney -= (bettingList[i] / (bettingSizeList[i] + 1)) * bettingMinusList[i];
            }
        }

        if (bettingMoney == 0)
        {
            localNotion = "변동이 없습니다";

            NotionManager.instance.UseNotion(localNotion, ColorType.Green);
            RecordManager.instance.SetRecord("+0");
        }
        else
        {
            if ((int)(plusMoney) - bettingMoney > 0) //배팅한 것보다 딴 돈이 많을 경우
            {
                tempMoney = (int)(plusMoney) - bettingMoney;

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, tempMoney);

                worldNotion = "<color=#00FF00>+" + MoneyUnitString.ToCurrencyString(tempMoney) + "   " + GameStateManager.instance.NickName + "</color>";
                localNotion = "+" + MoneyUnitString.ToCurrencyString(tempMoney);

                NotionManager.instance.UseNotion(localNotion, ColorType.Green);
                RecordManager.instance.SetRecord(localNotion);
            }
            else
            {
                tempMoney = (int)(plusMoney) - bettingMoney;

                worldNotion = "<color=#FF0000>" + MoneyUnitString.ToCurrencyString(tempMoney) + "   " + GameStateManager.instance.NickName + "</color>";
                localNotion = MoneyUnitString.ToCurrencyString(tempMoney);

                NotionManager.instance.UseNotion(localNotion, ColorType.Red);
                RecordManager.instance.SetRecord(localNotion);
            }

            PV.RPC("ChatRPC", RpcTarget.All, worldNotion);

            //randomBoxManager.GameReward();
        }

        Debug.LogError(MoneyUnitString.ToCurrencyString(tempMoney) + " 만큼 돈을 획득하였습니다");

        if(PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            if (!PhotonNetwork.IsMasterClient) //플레이어2가 자신이 얻은 돈을 알려줍니다.
            {
                int[] compare = new int[2];
                compare[0] = bettingMoney;
                compare[1] = (int)(plusMoney);
                PV.RPC("CompareMoney", RpcTarget.MasterClient, compare);
            }
        }
        else
        {
            bettingAiMoney = aiManager.bettingValue[aiManager.blockIndex];

            if ((int)(plusAiMoney) - bettingAiMoney > 0) //인공지능 결과 기록하기
            {
                tempAiMoney = (int)(plusAiMoney) - bettingAiMoney;

                worldNotion = "<color=#00FF00>" + MoneyUnitString.ToCurrencyString(tempAiMoney) + "   " + "인공지능</color>";
            }
            else
            {
                tempAiMoney = (int)(plusAiMoney) - bettingAiMoney;

                worldNotion = "<color=#FF0000>" + MoneyUnitString.ToCurrencyString(tempAiMoney) + "   " + "인공지능</color>";
            }

            RecordManager.instance.SetGameRecord(worldNotion);

            int[] compare = new int[2];
            compare[0] = bettingAiMoney;
            compare[1] = tempAiMoney;

            CompareMoney(compare);
        }

        ResetRouletteBackgroundColor();
        ResetBettingMoney();

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

        StartCoroutine(WaitTimerCoroution());
    }

    public void ChangeGetMoney(BlockClass block, RouletteType type, bool queen)
    {
        int money = upgradeDataBase.GetUpgradeValue(block.rankType).GetValueNumber(block.level);

        for(int i = 0; i < bettingValue.Length; i ++) //당첨된 블럭 빼주기
        {
            if(money.Equals(bettingValue))
            {
                bettingMinusList[i] -= 1;
            }
        }

        switch (type)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                if(queen)
                {
                    plusMoney += blockMotherInformation.queenStraightBet * money + money;
                }
                else
                {
                    plusMoney += blockMotherInformation.straightBet * money + money;
                }

                break;
            case RouletteType.SplitBet_Horizontal:
                if (queen)
                {
                    plusMoney += blockMotherInformation.queensplitBet * money + money;
                }
                else
                {
                    plusMoney += blockMotherInformation.splitBet * money + money;
                }

                break;
            case RouletteType.SplitBet_Vertical:
                if (queen)
                {
                    plusMoney += blockMotherInformation.queensplitBet * money + money;
                }
                else
                {
                    plusMoney += blockMotherInformation.splitBet * money + money;
                }

                break;
            case RouletteType.SquareBet:
                if (queen)
                {
                    plusMoney += blockMotherInformation.queensquareBet * money + money;
                }
                else
                {
                    plusMoney += blockMotherInformation.squareBet * money + money;
                }

                break;
        }
    }

    void ChangeBettingMoney()
    {
        moneyText.text = "GOLD <size=25>" + MoneyUnitString.ToCurrencyString(money - bettingMoney) + "</size>"; ;
        bettingMoneyText.text = MoneyUnitString.ToCurrencyString(bettingMoney);
    }

    void ResetBettingMoney()
    {
        for (int i = 0; i < bettingList.Length; i++)
        {
            bettingList[i] = 0;
        }

        bettingMoney = 0;
        bettingMoneyText.text = MoneyUnitString.ToCurrencyString(bettingMoney);

        ChangeBettingMoney();
    }

    private void CheckTargetNumber(int target)
    {
        for (int i = 0; i < rouletteContentList_Target.Count; i++)
        {
            if (rouletteContentList_Target[i].isActive && target == rouletteContentList_Target[i].number)
            {
                ChangeGetMoney(rouletteContentList_Target[i].blockClass, RouletteType.StraightBet, false);
                break;
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
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 1:
                        if (target == 2 || target == 7)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 2:
                        if (target == 3 || target == 8)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 3:
                        if (target == 4 || target == 9)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 4:
                        if (target == 5 || target == 10)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 5:
                        if (target == 6 || target == 11)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 6:
                        if (target == 7 || target == 12)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 7:
                        if (target == 8)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 8:
                        if (target == 9 || target == 14)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 9:
                        if (target == 10 || target == 15)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 10:
                        if (target == 11 || target == 16)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 11:
                        if (target == 12 || target == 17)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 12:
                        if (target == 18)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 13:
                        if (target == 14 || target == 19)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 14:
                        if (target == 15 || target == 20)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 15:
                        if (target == 16 || target == 21)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 16:
                        if (target == 17 || target == 22)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 17:
                        if (target == 18 || target == 23)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 18:
                        if (target == 19 || target == 24)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 19:
                        if (target == 20 || target == 25)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
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
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 1:
                        if (target == 2 || target == 3)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 2:
                        if (target == 3 || target == 4)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 3:
                        if (target == 4 || target == 5)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 4:
                        if (target == 6 || target == 7)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 5:
                        if (target == 7 || target == 8)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 6:
                        if (target == 8 || target == 9)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 7:
                        if (target == 9 || target == 10)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 8:
                        if (target == 11 || target == 12)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 9:
                        if (target == 12)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 10:
                        if (target == 13)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 11:
                        if (target == 14 || target == 15)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 12:
                        if (target == 16 || target == 17)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 13:
                        if (target == 17 || target == 18)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 14:
                        if (target == 18 || target == 19)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 15:
                        if (target == 19 || target == 20)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 16:
                        if (target == 21 || target == 22)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 17:
                        if (target == 22 || target == 23)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 18:
                        if (target == 23 || target == 24)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 19:
                        if (target == 24 || target == 25)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
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
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 1:
                        if (target == 2 || target == 3 || target == 7 || target == 8)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 2:
                        if (target == 3 || target == 4 || target == 8 || target == 9)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 3:
                        if (target == 4 || target == 5 || target == 9 || target == 10)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 4:
                        if (target == 6 || target == 7 || target == 11 || target == 12)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 5:
                        if (target == 7 || target == 8 || target == 12)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 6:
                        if (target == 8 || target == 9 || target == 14)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 7:
                        if (target == 9 || target == 10 || target == 14 || target == 15)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 8:
                        if (target == 11 || target == 12 || target == 16 || target == 17)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 9:
                        if (target == 12 || target == 17 || target == 18)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 10:
                        if (target == 14 || target == 18 || target == 19)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 11:
                        if (target == 14 || target == 15 || target == 19 || target == 20)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 12:
                        if (target == 16 || target == 17 || target == 21 || target == 22)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 13:
                        if (target == 17 || target == 18 || target == 22 || target == 23)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 14:
                        if (target == 18 || target == 19 || target == 23 || target == 24)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 15:
                        if (target == 19 || target == 20 || target == 24 || target == 25)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
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
            ChangeGetMoney(rouletteContentList_Target[12].blockClass, RouletteType.StraightBet, true);
        }

        if (rouletteSplitContentList_Horizontal[7].isActive)
        {
            ChangeGetMoney(rouletteSplitContentList_Horizontal[7].blockClass, RouletteType.SplitBet_Horizontal, true);
        }

        if (rouletteSplitContentList_Horizontal[12].isActive)
        {
            ChangeGetMoney(rouletteSplitContentList_Horizontal[12].blockClass, RouletteType.SplitBet_Horizontal, true);
        }

        if (rouletteSplitContentList_Vertical[9].isActive)
        {
            ChangeGetMoney(rouletteSplitContentList_Vertical[9].blockClass, RouletteType.SplitBet_Vertical, true);
        }

        if (rouletteSplitContentList_Vertical[10].isActive)
        {
            ChangeGetMoney(rouletteSplitContentList_Vertical[10].blockClass, RouletteType.SplitBet_Vertical, true);
        }

        if (rouletteSquareContentList[5].isActive)
        {
            ChangeGetMoney(rouletteSquareContentList[5].blockClass, RouletteType.SquareBet, true);
        }

        if (rouletteSquareContentList[6].isActive)
        {
            ChangeGetMoney(rouletteSquareContentList[6].blockClass, RouletteType.SquareBet, true);
        }

        if (rouletteSquareContentList[9].isActive)
        {
            ChangeGetMoney(rouletteSquareContentList[9].blockClass, RouletteType.SquareBet, true);
        }

        if (rouletteSquareContentList[10].isActive)
        {
            ChangeGetMoney(rouletteSquareContentList[10].blockClass, RouletteType.SquareBet, true);
        }
    }

    //void Update()
    //{
    //    if(blockDrag && targetBlockContent != null)
    //    {
    //        if(targetBlockContent.position.y > Screen.height * 0.5f + 450 || targetBlockContent.position.y < Screen.height * 0.5f - 650)
    //        {
    //            if (checkDrag)
    //            {
    //                checkDrag = false;
    //                ResetRouletteBackgroundColor();
    //            }
    //        }
    //        else
    //        {
    //            if(!checkDrag) checkDrag = true;
    //        }

    //        if (targetBlockContent.position.x > Screen.width * 0.5f + 520 || targetBlockContent.position.x < Screen.width * 0.5f - 520)
    //        {
    //            if(checkDrag)
    //            {
    //                checkDrag = false;
    //                ResetRouletteBackgroundColor();
    //            }
    //        }
    //        else
    //        {
    //            if (!checkDrag) checkDrag = true;
    //        }
    //    }
    //}

    public void EnterBlock(RouletteContent rouletteContent, BlockContent blockContent)
    {
        mainRouletteContent = rouletteContent;

        blockInformation = blockDataBase.GetBlockInfomation(blockContent.blockClass.blockType);

        targetBlockContent = blockContent.transform;

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < rouletteContentList_NewBie.Count; i++)
            {
                rouletteContentList_NewBie[i].SetActiveFalse(blockContent.blockClass);
            }
        }
        else
        {
            for (int i = 0; i < allContentList.Count; i++)
            {
                allContentList[i].SetActiveFalse(blockContent.blockClass);
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

        index0[0] = rouletteContent.index[0] + blockDataBase.GetIndex0(blockContent.blockClass.blockType, 0);
        index0[1] = rouletteContent.index[1] + blockDataBase.GetIndex0(blockContent.blockClass.blockType, 1);

        index1[0] = rouletteContent.index[0] + blockDataBase.GetIndex1(blockContent.blockClass.blockType, 0);
        index1[1] = rouletteContent.index[1] + blockDataBase.GetIndex1(blockContent.blockClass.blockType, 1);

        index2[0] = rouletteContent.index[0] + blockDataBase.GetIndex2(blockContent.blockClass.blockType, 0);
        index2[1] = rouletteContent.index[1] + blockDataBase.GetIndex2(blockContent.blockClass.blockType, 1);

        index3[0] = rouletteContent.index[0] + blockDataBase.GetIndex3(blockContent.blockClass.blockType, 0);
        index3[1] = rouletteContent.index[1] + blockDataBase.GetIndex3(blockContent.blockClass.blockType, 1);

        index4[0] = rouletteContent.index[0] + blockDataBase.GetIndex4(blockContent.blockClass.blockType, 0);
        index4[1] = rouletteContent.index[1] + blockDataBase.GetIndex4(blockContent.blockClass.blockType, 1);

        index5[0] = rouletteContent.index[0] + blockDataBase.GetIndex5(blockContent.blockClass.blockType, 0);
        index5[1] = rouletteContent.index[1] + blockDataBase.GetIndex5(blockContent.blockClass.blockType, 1);

        index6[0] = rouletteContent.index[0] + blockDataBase.GetIndex6(blockContent.blockClass.blockType, 0);
        index6[1] = rouletteContent.index[1] + blockDataBase.GetIndex6(blockContent.blockClass.blockType, 1);

        index7[0] = rouletteContent.index[0] + blockDataBase.GetIndex7(blockContent.blockClass.blockType, 0);
        index7[1] = rouletteContent.index[1] + blockDataBase.GetIndex7(blockContent.blockClass.blockType, 1);

        index8[0] = rouletteContent.index[0] + blockDataBase.GetIndex8(blockContent.blockClass.blockType, 0);
        index8[1] = rouletteContent.index[1] + blockDataBase.GetIndex8(blockContent.blockClass.blockType, 1);

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

        if (playerDataBase.Gold < bettingValue[blockContent.index])
        {
            blockContent.ResetPos();

            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);

            return;
        }

        if(bettingList.Contains(1) && bettingList[blockContent.index] == 0)
        {
            blockContent.ResetPos();

            NotionManager.instance.UseNotion(NotionType.OverBettingBlock);

            Debug.Log("1개 이상 배팅할 수 없습니다");

            return;
        }

        if (bettingList[blockContent.index] == 0)
        {
            bettingList[blockContent.index] = 1;
            bettingMinusList[blockContent.index] = blockDataBase.GetBlockInfomation(blockContent.blockClass.blockType).GetSize();
            bettingMoney += bettingValue[blockContent.index];
            ChangeBettingMoney();
        }

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
                        rouletteContentList_Target[i].SetActiveTrue(blockContent.blockClass);
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
                        rouletteSplitContentList_Horizontal[i].SetActiveTrue(blockContent.blockClass);
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
                        rouletteSplitContentList_Vertical[i].SetActiveTrue(blockContent.blockClass);
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
                        rouletteSquareContentList[i].SetActiveTrue(blockContent.blockClass);
                    }
                }
                break;
        }

        string notion = MoneyUnitString.ToCurrencyString(bettingValue[blockContent.index]) + " 값의 블록 배치";

        NotionManager.instance.UseNotion(notion, ColorType.Green);

        insertBlock = new string[5];

        insertBlock[0] = mainRouletteContent.rouletteType.ToString();
        insertBlock[1] = blockContent.blockClass.blockType.ToString();
        insertBlock[2] = mainRouletteContent.number.ToString();
        insertBlock[3] = GameStateManager.instance.NickName;
        insertBlock[4] = bettingValue[blockContent.index].ToString();

        PV.RPC("ShowOtherPlayerBlock", RpcTarget.Others, insertBlock);
    }


    public void ResetPosBlock(int number)
    {
        bettingList[number] = 0;
        bettingMoney -= bettingValue[number];

        if (bettingMoney < 0)
        {
            bettingMoney = 0;
        }

        ChangeBettingMoney();
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

            otherBettingList = "";

            for (int i = 0; i < bettingNumberList_NewBie.Length; i++)
            {
                otherBettingList += bettingNumberList_NewBie[i].ToString() + "/";
            }


            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2 && otherBettingList.Length > 0)
            {
                PV.RPC("ShowOtherBetting_Newbie", RpcTarget.Others, otherBettingList.TrimEnd('/'));
            }
            else
            {
                for (int i = 0; i < otherBettingNumberList_Newbie.Length; i++)
                {
                    otherBettingNumberList_Newbie[i] = 0;
                }

                if (aiManager.blockPos == 13)
                {
                    otherBettingNumberList_Newbie[2] = 1;
                }
                else
                {
                    if (aiManager.blockPos % 2 == 0)
                    {
                        otherBettingNumberList_Newbie[0] = 1;
                    }
                    else
                    {
                        otherBettingNumberList_Newbie[1] = 1;
                    }
                }

                Debug.Log("초보방 Ai 배팅한 위치값을 받아왔습니다");
            }
        }
        else
        {
            bettingNumberList_Gosu.Clear();

            for (int i = 0; i < allContentList.Count; i++)
            {
                if (allContentList[i].isActive)
                {
                    switch (allContentList[i].rouletteType)
                    {
                        case RouletteType.StraightBet:
                            bettingNumberList_Gosu.Add(allContentList[i].number);
                            break;
                        default:
                            for(int j = 0; j < allContentList[i].numberList.Length; j ++)
                            {
                                bettingNumberList_Gosu.Add(allContentList[i].numberList[j]);
                            }
                            break;;
                    }
                }
            }

            bettingNumberList_Gosu = bettingNumberList_Gosu.Distinct().ToList();
            bettingNumberList_Gosu.Sort();

            otherBettingList = "";

            for (int i = 0; i < bettingNumberList_Gosu.Count; i ++)
            {
                otherBettingList += bettingNumberList_Gosu[i].ToString() + "/";
            }

            otherBettingNumberList_Gosu.Clear();

            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2 && otherBettingList.Length > 0)
            {
                PV.RPC("ShowOtherBetting_Gosu", RpcTarget.Others, otherBettingList.TrimEnd('/'));
            }
            else
            {
                //아직 미구현


                Debug.Log("고수방 Ai 배팅한 위치값을 받아왔습니다");
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

    public void BetOptionCancleButton() //배팅 취소
    {
        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            newbieBlockContent.ResetPos();
        }
        else
        {
            for (int i = 0; i < blockContentList.Count; i++)
            {
                blockContentList[i].ResetPos();
            }
        }

        for (int i = 0; i < allContentList.Count; i++)
        {
            allContentList[i].SetActiveFalseAll();
        }

        for (int i = 0; i < bettingList.Length; i++)
        {
            bettingList[i] = 0;
        }

        ResetBettingMoney();

        //NotionManager.instance.UseNotion(NotionType.Cancle);
    }

    [PunRPC]
    void ChatRPC(string msg)
    {
        RecordManager.instance.SetGameRecord(msg);
    }


    #region ShowBetting

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
        content.SetOtherBlock(blockType, block[3], block[4]);
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

    [PunRPC]
    void ShowOtherBetting_Newbie(string str)
    {
        for(int i = 0; i < otherBettingNumberList_Newbie.Length; i ++)
        {
            otherBettingNumberList_Newbie[i] = 0;
        }

        string[] list = str.Split("/");

        otherBettingNumberList_Newbie[0] = int.Parse(list[0]);
        otherBettingNumberList_Newbie[1] = int.Parse(list[1]);
        otherBettingNumberList_Newbie[2] = int.Parse(list[2]);

        Debug.Log("초보방 상대 배팅 위치값을 받아왔습니다");
    }

    [PunRPC]
    void ShowOtherBetting_Gosu(string str)
    {
        string[] list = str.Split("/");

        for(int i = 0; i < list.Length; i ++)
        {
            otherBettingNumberList_Gosu.Add(int.Parse(list[i]));
            list[i].Replace("/", "");
        }

        otherBettingNumberList_Gosu = otherBettingNumberList_Gosu.Distinct().ToList();
        otherBettingNumberList_Gosu.Sort();

        Debug.Log("고수방 상대 배팅 위치값을 받아왔습니다");
    }

    #endregion


    #region InGame

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
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Total", GameStateManager.instance.Stakes } });
                break;
            case 1:
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Total", GameStateManager.instance.Stakes } });
                break;
        }
    }

    public void SetMinusMoney(int number) //배팅 후 결과값에 따라 잃은 돈 저장하기
    {
        int myNumber = 0;

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

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
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Minus", int.Parse(ht["Player1_Minus"].ToString()) + number } });
                break;
            case 1:
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Minus", int.Parse(ht["Player2_Minus"].ToString()) + number } });
                break;
        }
    }

    [PunRPC]
    public void CompareMoney(int[] other)
    {
        int number = 0;

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        otherMoney -= other[0]; //상대방의 배팅한 금액만큼 일단 빼기

        if (tempMoney > 0 && other[1] > 0) //둘다 땃을 경우
        {
            if (tempMoney > other[1]) //내가 더 많이 땃을 경우
            {
                number = tempMoney - other[1];

                money += number;
                otherMoney -= number;
            }
            else //상대방이 더 많이 땃을 경우
            {
                number = other[1] - tempMoney;

                money -= number;
                otherMoney += number;
            }
        }
        else if (tempMoney > 0 && other[1] <= 0) //상대방은 잃었는데 나만 땃을 경우 
        {
            money += tempMoney;
            otherMoney -= tempMoney + Mathf.Abs(other[1]);
        }
        else if (tempMoney <= 0 && other[1] > 0) //난 잃었는데 상대방만 땃을 경우
        {
            money -= other[1] + Mathf.Abs(tempMoney);
            otherMoney += other[1];
        }
        else if (tempMoney < 0 && other[1] < 0) //둘다 못 땃을 경우
        {
            money += Mathf.Abs(tempMoney);
            otherMoney += Mathf.Abs(other[1]);
        }
        else if (tempMoney > 0 && other[1] == 0) //상대방이 배팅 안 했을 경우
        {
            money += tempMoney;
            otherMoney -= tempMoney;
        }
        else if (tempMoney == 0 && other[1] > 0) //내가 배팅 안 했을 경우 
        {
            money -= other[1];
            otherMoney += other[1];
        }

        moneyText.text = "GOLD <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
        otherMoneyText.text = "GOLD <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            compareMoney[0] = otherMoney;
            compareMoney[1] = money;

            PV.RPC("SyncMoney", RpcTarget.Others, compareMoney);

            Debug.Log("서버 돈 동기화 갱신완료");
        }

        CheckWinnerPlayer();
    }

    [PunRPC]
    void SyncMoney(int[] compare)
    {
        money = compare[0];
        moneyText.text = "GOLD <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";

        otherMoney = compare[1];
        otherMoneyText.text = "GOLD <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";
    }

    public void SurrenderButton() //기권하기
    {
        StopAllCoroutines();

        uIManager.CloseSurrenderView();

        money -= (int)(money * 0.1f);

        PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, (int)(money * 0.1f)); //내 보유 금액의 10% 잃기

        if (PhotonNetwork.IsMasterClient)
        {
            GameEnd(1);
            PV.RPC("Surrender", RpcTarget.Others);

            Debug.Log("플레이어 1 기권");
        }
        else
        {
            GameEnd(1);
            PV.RPC("Surrender", RpcTarget.MasterClient);

            Debug.Log("플레이어 2 기권");
        }
    }

    [PunRPC]
    public void Surrender()
    {
        StopAllCoroutines();

        money += (int)(otherMoney * 0.1f);

        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, (int)(otherMoney * 0.1f)); //상대방 보유 금액의 10% 가져옴

        GameEnd(0);

        Debug.Log("상대방이 기권하여 승리하였습니다");
    }

    public void Winner()
    {
        StopAllCoroutines();

        money += (int)(otherMoney * 0.1f);

        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, (int)(otherMoney * 0.1f)); //상대방 보유 금액의 10% 가져옴

        GameEnd(0);

        Debug.Log("상대방이 방에서 튕겼습니다");
    }

    #endregion
}
