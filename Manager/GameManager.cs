using Firebase.Analytics;
using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    public RouletteContent mainRouletteContent;
    Transform targetBlockContent;

    public BlockType dragBlockType = BlockType.Default;
    public BlockType blockType = BlockType.Default;
    public BlockType otherBlockType = BlockType.Default;
    BlockMotherInformation blockMotherInformation;

    [Space]
    [Title("Developer")]
    public InputField inputTargetNumber;
    public Text developerInfo;
    public GameObject nextButton;

    [Space]
    [Title("Setting")]
    public GridLayoutGroup gridLayoutGroup;

    [Space]
    [Title("Timer")]
    private int timer = 0;
    public Text timerText;
    public Image timerFillAmount;
    private int timerAi = 0;
    private int timerEmoteAi = 0;

    public ButtonScaleAnimation timerAnimation;
    private bool isTimesUp = false;

    public FadeInOut fadeInOut;

    [Space]
    [Title("Text")]
    public Text roomText;
    public Text targetText;
    public Text recordText;

    [Space]
    [Title("Tip")]
    public GameObject tipObj;
    public LocalizationContent tipText;
    public GameObject tutorialArrow; //블럭 배치하라는 화살표

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
    public int[] bettingPlusList = new int[4]; //당첨 안된 블럭 빼기
    public int[] bettingMinusList = new int[4]; //당첨 안된 블럭 빼기

    [Space]
    [Title("Ai")]
    public int[] bettingAiPlusList = new int[4]; //당첨 안된 블럭 빼기
    public int[] bettingAiMinusList = new int[4]; //당첨 안된 블럭 빼기

    public List<int> bettingNumberList = new List<int>();
    public List<int> otherBettingNumberList = new List<int>();

    [Space]
    [Title("Value")]
    private int limitLevel = 0;
    private int bettingTime = 0;
    private int bettingWaitTime = 0;
    public int keepCount = 0;

    public int windMax = 0;
    public int windGranularity = 0;

    public int turn = 0;
    public int turnCount = 0;
    public bool inGameBurning = false;
    public bool inGameBurning2 = false;

    public GameObject burningObj;
    public LocalizationContent turnText;

    public int money = 0; //보유 코인
    public int otherMoney = 0; //상대방 보유 코인
    private int stakes = 0; //판돈

    private int bettingMoney = 0; //배치한 블럭 크기
    private float plusMoney = 0; //획득한 돈

    private int bettingAiMoney = 0;
    private float plusAiMoney = 0; //Ai가 획득한 돈
    private bool aiMoveBlock = false; //확률적으로 Ai가 0초 남기고 위치를 바꿉니다
    private bool aiEmote = false;

    private float value; //당첨된 금액
    private float value_Ai;
    private int blockLevel, compareValue, myNumber, myBlockLevel, myBlockLevel2;

    private int halfMoney = 0;
    private int halfOtherMoney = 0;

    private int[] compareMoney = new int[2];

    private string otherBettingList = "";

    private int targetNumber = 0;
    private int targetQueenNumber = 0;

    private int gridConstraintCount = 0;

    string[] insertBlock = new string[5];
    string[] deleteBlock = new string[2];

    [Space]
    [Title("Bool")]
    public bool aiMode = false;
    public bool blockDrag = false;
    public bool blockDrop = false;
    public bool blockOverlap = false;
    public bool allIn = false;

    [Space]
    [Title("Parent")]
    public GameObject blockRootParent;
    public GameObject blockParent;
    public GameObject blockGridParent_NewBie;
    public GameObject blockGridParent_Gosu;
    public Transform otherBlockParent;

    public GameObject targetObj;
    public GameObject targetQueenObj;

    [Space]
    [Title("Prefab")]
    public RouletteContent rouletteContent;
    public NumberContent numberContent;
    public BlockLevelContent blockLevelContent;
    public BlockContent blockContent;
    public OtherBlockContent otherBlockContent;

    [Space]
    [Title("Grid")]
    public RectTransform rouletteContentTransform_NewBie;
    public RectTransform rouletteContentTransform;

    [Space]
    public RectTransform rouletteContentTransformSplitBet_Vertical;
    public RectTransform rouletteContentTransformSplitBet_Horizontal;
    public RectTransform rouletteContentTransformSquareBet;

    [Space]
    [Title("Block")]
    public RectTransform blockContentTransform_NewBie;
    public RectTransform blockContentTransform_Gosu;

    [Space]
    [Title("Number")]
    public RectTransform numberContentTransform_NewBie;
    public RectTransform numberContentTransform;

    [Space]
    [Title("Level")]
    public RectTransform blockLevelContentTransform_NewBie;
    public RectTransform blockLevelContentTransform;

    [Space]
    public RectTransform blockLevelContentTransform_Vertical;
    public RectTransform blockLevelContentTransform_Horizontal;
    public RectTransform blockLevelContentTransform_Square;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    [Space]
    [Title("Roulette")]
    List<RouletteContent> rouletteContentList = new List<RouletteContent>();
    public List<RouletteContent> rouletteContentList_Split_Vertical = new List<RouletteContent>();
    public List<RouletteContent> rouletteContentList_Split_Horizontal = new List<RouletteContent>();
    public List<RouletteContent> rouletteContentList_Square = new List<RouletteContent>();
    List<RouletteContent> rouletteContentList_NewBie = new List<RouletteContent>();

    [Space]
    List<RouletteContent> allContentList = new List<RouletteContent>();

    [Space]
    List<NumberContent> numberContentList_NewBie = new List<NumberContent>();
    List<NumberContent> numberContentList = new List<NumberContent>();

    [Space]
    List<BlockLevelContent> blockLevelContentList = new List<BlockLevelContent>();
    List<BlockLevelContent> blockLevelContentList_Split_Vertical = new List<BlockLevelContent>();
    List<BlockLevelContent> blockLevelContentList_Split_Horizontal = new List<BlockLevelContent>();
    List<BlockLevelContent> blockLevelContentList_Square = new List<BlockLevelContent>();
    List<BlockLevelContent> blockLevelContentList_NewBie = new List<BlockLevelContent>();

    [Space]
    List<BlockLevelContent> allBlockLevelContentList = new List<BlockLevelContent>();

    [Space]
    [Title("Other")]
    public BlockContent newbieBlockContent;
    List<BlockContent> blockContentList = new List<BlockContent>();
    List<OtherBlockContent> otherBlockContentList = new List<OtherBlockContent>();

    [Space]
    [Title("Target")]
    public List<RouletteContent> rouletteContentList_Target = new List<RouletteContent>();
    public List<BlockLevelContent> blockLevelContentList_Target = new List<BlockLevelContent>(); //상대방 블럭 값 저장


    BlockClass blockClassArmor = new BlockClass();
    BlockClass blockClassWeapon = new BlockClass();
    BlockClass blockClassShield = new BlockClass();
    BlockClass blockClassNewbie = new BlockClass();

    [Space]
    [Title("Manager")]
    public GameEventManager gameEventManager;
    public NetworkManager networkManager;
    public UIManager uIManager;
    public RouletteManager rouletteManager;
    public AiManager aiManager;
    public RandomBoxManager randomBoxManager;
    public EmoteManager emoteManager;
    public MoneyAnimation moneyAnimation;

    UpgradeDataBase upgradeDataBase;
    PlayerDataBase playerDataBase;
    BlockDataBase blockDataBase;
    RankDataBase rankDataBase;

    Hashtable ht;

    public PhotonView PV;

    int[] index0, index1, index2, index3, index4, index5, index6, index7, index8 = new int[2];
    int[] index0_Ai, index1_Ai, index2_Ai, index3_Ai, index4_Ai, index5_Ai, index6_Ai, index7_Ai, index8_Ai = new int[2];
    int[] index0_Enemy, index1_Enemy, index2_Enemy, index3_Enemy, index4_Enemy, index5_Enemy, index6_Enemy, index7_Enemy, index8_Enemy = new int[2];

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;

        inputTargetNumber.gameObject.SetActive(false);
        nextButton.SetActive(false);

        bettingValue = new int[4];
        bettingSizeList = new int[4];
        bettingList = new int[4];
        bettingPlusList = new int[4];
        bettingMinusList = new int[4];

        bettingAiPlusList = new int[4];
        bettingAiMinusList = new int[4];

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
            content.transform.SetParent(rouletteContentTransform);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockParent.transform, RouletteType.StraightBet, setIndex, i);
            rouletteContentList.Add(content);

            NumberContent numContent = Instantiate(numberContent);
            numContent.transform.SetParent(numberContentTransform);
            numContent.transform.localPosition = Vector3.zero;
            numContent.transform.localScale = Vector3.one;
            numContent.Initialize(i);
            numberContentList.Add(numContent);

            BlockLevelContent levelContent = Instantiate(blockLevelContent);
            levelContent.transform.SetParent(blockLevelContentTransform);
            levelContent.transform.localPosition = Vector3.zero;
            levelContent.transform.localScale = Vector3.one;
            levelContent.Initialize();
            blockLevelContentList.Add(levelContent);

            index++;

            allContentList.Add(content);
            allBlockLevelContentList.Add(levelContent);
        }

        index = 0;
        count = 0;

        for (int i = 0; i < 9; i++) //NewBie
        {
            int[] setIndex = new int[2];

            if (index >= 3)
            {
                index = 0;
                count++;
            }

            setIndex[0] = index;
            setIndex[1] = count;

            RouletteContent content = Instantiate(rouletteContent);
            content.transform.SetParent(rouletteContentTransform_NewBie);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize_NewBie(this, blockParent.transform, RouletteType.StraightBet, setIndex, i);
            rouletteContentList_NewBie.Add(content);

            NumberContent numContent = Instantiate(numberContent);
            numContent.transform.SetParent(numberContentTransform_NewBie);
            numContent.transform.localPosition = Vector3.zero;
            numContent.transform.localScale = Vector3.one;
            numContent.Initialize_NewBie(i);
            numberContentList_NewBie.Add(numContent);

            BlockLevelContent levelContent = Instantiate(blockLevelContent);
            levelContent.transform.SetParent(blockLevelContentTransform_NewBie);
            levelContent.transform.localPosition = Vector3.zero;
            levelContent.transform.localScale = Vector3.one;
            levelContent.Initialize();
            blockLevelContentList_NewBie.Add(levelContent);

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
            content.transform.SetParent(rouletteContentTransformSplitBet_Vertical);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockParent.transform, RouletteType.SplitBet_Vertical, setIndex, i);
            rouletteContentList_Split_Vertical.Add(content);

            BlockLevelContent levelContent = Instantiate(blockLevelContent);
            levelContent.transform.SetParent(blockLevelContentTransform_Vertical);
            levelContent.transform.localPosition = Vector3.zero;
            levelContent.transform.localScale = Vector3.one;
            levelContent.Initialize();
            blockLevelContentList_Split_Vertical.Add(levelContent);

            index++;

            allContentList.Add(content);
            allBlockLevelContentList.Add(levelContent);
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
            content.transform.SetParent(rouletteContentTransformSplitBet_Horizontal);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockParent.transform, RouletteType.SplitBet_Horizontal, setIndex, i);
            rouletteContentList_Split_Horizontal.Add(content);


            BlockLevelContent levelContent = Instantiate(blockLevelContent);
            levelContent.transform.SetParent(blockLevelContentTransform_Horizontal);
            levelContent.transform.localPosition = Vector3.zero;
            levelContent.transform.localScale = Vector3.one;
            levelContent.Initialize();
            blockLevelContentList_Split_Horizontal.Add(levelContent);

            index++;

            allContentList.Add(content);
            allBlockLevelContentList.Add(levelContent);
        }

        //index = 0;
        //count = 0;

        //for (int i = 0; i < 16; i++)
        //{
        //    int[] setIndex = new int[2];

        //    if (index >= gridConstraintCount - 1)
        //    {
        //        index = 0;
        //        count++;
        //    }

        //    setIndex[0] = index;
        //    setIndex[1] = count;

        //    RouletteContent content = Instantiate(rouletteContent);
        //    content.transform.parent = rouletteContentTransformSquareBet;
        //    content.transform.localPosition = Vector3.zero;
        //    content.transform.localScale = Vector3.one;
        //    content.Initialize(this, blockParent.transform, RouletteType.SquareBet, setIndex, i);
        //    rouletteSquareContentList.Add(content);

        //    index++;

        //    allContentList.Add(content);
        //}

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
        newbieBlockContent.transform.SetParent(blockContentTransform_NewBie);
        newbieBlockContent.transform.localPosition = Vector3.zero;
        newbieBlockContent.transform.localScale = Vector3.one;
        newbieBlockContent.Initialize(this, blockRootParent.transform, blockGridParent_NewBie.transform);
        newbieBlockContent.gameObject.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            BlockContent content = Instantiate(blockContent);
            content.transform.SetParent(blockContentTransform_Gosu);
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

        index0_Ai = new int[2];
        index1_Ai = new int[2];
        index2_Ai = new int[2];
        index3_Ai = new int[2];
        index4_Ai = new int[2];
        index5_Ai = new int[2];
        index6_Ai = new int[2];
        index7_Ai = new int[2];
        index8_Ai = new int[2];

        index0_Enemy = new int[2];
        index1_Enemy = new int[2];
        index2_Enemy = new int[2];
        index3_Enemy = new int[2];
        index4_Enemy = new int[2];
        index5_Enemy = new int[2];
        index6_Enemy = new int[2];
        index7_Enemy = new int[2];
        index8_Enemy = new int[2];
    }

    private void Start()
    {
        GameReset();
    }

    public void Initialize()
    {
        bettingTime = GameStateManager.instance.BettingTime;
        bettingWaitTime = GameStateManager.instance.BettingWaitTime;

        blockMotherInformation = blockDataBase.blockMotherInformation;
    }

    private void GameReset()
    {
        StopAllCoroutines();

        keepCount = 0;

        turn = 0;

        turnCount = 0;

        burningObj.SetActive(false);

        inGameBurning = false;
        inGameBurning2 = false;

        targetObj.SetActive(false);
        targetQueenObj.SetActive(false);
        gameEventManager.eventInfoView.SetActive(false);

        targetText.text = "-";

        tipObj.SetActive(false);
        tutorialArrow.SetActive(false);

        timer = bettingWaitTime;
        timerText.text = LocalizationManager.instance.GetString("ReadyToGame") + " : " + timer;
        timerFillAmount.fillAmount = 1;

        moneyText.text = moneyText.text = "Raf  <size=25>0</size>";
        bettingMoneyText.text = "0";
        otherMoneyText.text = moneyText.text = "Raf  <size=25>0</size>";

        RecordManager.instance.Initialize();
        recordText.text = "";

        inputTargetNumber.text = "";

        otherBlockType = BlockType.Default;

        ClearOtherPlayerBlock();

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < blockLevelContentList_Target.Count; i++)
            {
                blockLevelContentList_Target[i].Initialize();
            }
        }
        else
        {
            for (int i = 0; i < allBlockLevelContentList.Count; i++)
            {
                allBlockLevelContentList[i].Initialize();
            }
        }
    }

    public void ExitRoom()
    {
        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);

            return;
        }

        StopAllCoroutines();

        GameReset();

        rouletteManager.CloseRouletteView();
        SoundManager.instance.StopAllSFX();
        SoundManager.instance.Initialize();

        ResetRouletteBackgroundColor();

        for (int i = 0; i < blockContentList.Count; i++)
        {
            if (blockContentList[i].isDrag)
            {
                if (blockContentList[i].gameObject.activeInHierarchy)
                {
                    blockContentList[i].TimeOver();
                }
            }
        }

        BetOptionCancleButton();

        uIManager.GameEnd();

        networkManager.LeaveRoom();
    }

    public void GameStart_Initialize()
    {
        emoteManager.Initialize();
        moneyAnimation.Initialize();

        rouletteContentTransform_NewBie.gameObject.SetActive(false);
        rouletteContentTransform.gameObject.SetActive(false);
        rouletteContentTransformSplitBet_Vertical.gameObject.SetActive(false);
        rouletteContentTransformSplitBet_Horizontal.gameObject.SetActive(false);
        rouletteContentTransformSquareBet.gameObject.SetActive(false);

        blockLevelContentTransform_NewBie.gameObject.SetActive(false);
        blockLevelContentTransform.gameObject.SetActive(false);
        blockLevelContentTransform_Vertical.gameObject.SetActive(false);
        blockLevelContentTransform_Horizontal.gameObject.SetActive(false);
        blockLevelContentTransform_Square.gameObject.SetActive(false);

        blockGridParent_NewBie.SetActive(false);
        blockGridParent_Gosu.SetActive(false);

        blockContentTransform_NewBie.gameObject.SetActive(false);
        blockContentTransform_Gosu.gameObject.SetActive(false);

        numberContentTransform_NewBie.gameObject.SetActive(false);
        numberContentTransform.gameObject.SetActive(false);

        gameEventManager.eventView.SetActive(false);

        aiMode = false;
        aiManager.RestartGame();

        timer = bettingWaitTime;
        timerFillAmount.fillAmount = 1;

        isTimesUp = false;
        timerText.text = "";
        timerText.color = new Color(7 / 255f, 80 / 255f, 93 / 255f);
        timerAnimation.StopAnim();

        for (int i = 0; i < allBlockLevelContentList.Count; i++)
        {
            allBlockLevelContentList[i].Initialize();
        }

        BetOptionCancleButton();
    }

    public void GameStart_Newbie_Ai()
    {
        aiManager.Initialize();

        aiMode = true;

        GameStart_Newbie();

    }

    public void GameStart_Gosu_Ai()
    {
        aiManager.Initialize();

        aiMode = true;

        GameStart_Gosu();
    }

    public void GameStart_Newbie()
    {
        //roomText.text = "초보방";

        developerInfo.text = "0 = 퀸 당첨\n1 ~ 8 = 해당 숫자 당첨\n빈칸 = 정상 진행";

        for (int i = 0; i < numberContentList_NewBie.Count; i++)
        {
            numberContentList_NewBie[i].Betting_Initialize();
        }

        rouletteContentTransform_NewBie.gameObject.SetActive(true);
        rouletteContentTransform.gameObject.SetActive(false);
        rouletteContentTransformSplitBet_Vertical.gameObject.SetActive(false);
        rouletteContentTransformSplitBet_Horizontal.gameObject.SetActive(false);
        rouletteContentTransformSquareBet.gameObject.SetActive(false);

        blockLevelContentTransform_NewBie.gameObject.SetActive(true);
        blockLevelContentTransform.gameObject.SetActive(false);
        blockLevelContentTransform_Vertical.gameObject.SetActive(false);
        blockLevelContentTransform_Horizontal.gameObject.SetActive(false);
        blockLevelContentTransform_Square.gameObject.SetActive(false);


        blockGridParent_NewBie.SetActive(true);
        blockContentTransform_NewBie.gameObject.SetActive(true);
        numberContentTransform_NewBie.gameObject.SetActive(true);

        rouletteContentList_Target = rouletteContentList_NewBie;
        blockLevelContentList_Target = blockLevelContentList_NewBie;

        newbieBlockContent.gameObject.SetActive(true);

        blockClassNewbie = playerDataBase.GetBlockClass(playerDataBase.Newbie);

        limitLevel = rankDataBase.GetLimitLevel(GameStateManager.instance.PlayRankType) - 1;

        myBlockLevel = blockClassNewbie.level;

        if (myBlockLevel > limitLevel)
        {
            myBlockLevel = limitLevel;
            blockClassNewbie.level = limitLevel;
        }

        if (blockClassNewbie.rankType == RankType.SSR)
        {
            windMax += blockClassNewbie.ssrLevel + 1;
        }
        else if (blockClassNewbie.rankType == RankType.UR)
        {
            windGranularity += 1;
        }

        myBlockLevel2 = upgradeDataBase.GetUpgradeValue(blockClassNewbie.rankType).GetValueNumber(myBlockLevel);

        newbieBlockContent.InGame_Initialize(blockClassNewbie, 3, myBlockLevel2);
        newbieBlockContent.InGame_SetLevel(myBlockLevel);
        bettingValue[3] = upgradeDataBase.GetUpgradeValue(blockClassNewbie.rankType).GetValueNumber(myBlockLevel);

        bettingSizeList[3] = blockDataBase.GetBlockInfomation(blockClassNewbie.blockType).GetSize();

        GameEvent();
    }

    public void GameStart_Gosu()
    {
        //roomText.text = "고수방";

        developerInfo.text = "0 = 퀸 당첨\n1 ~ 24 = 해당 숫자 당첨\n빈칸 = 정상 진행";

        for (int i = 0; i < numberContentList.Count; i++)
        {
            numberContentList[i].Betting_Initialize();
        }

        blockLevelContentTransform_NewBie.gameObject.SetActive(false);
        rouletteContentTransform.gameObject.SetActive(true);
        rouletteContentTransformSplitBet_Vertical.gameObject.SetActive(true);
        rouletteContentTransformSplitBet_Horizontal.gameObject.SetActive(true);
        rouletteContentTransformSquareBet.gameObject.SetActive(true);

        blockLevelContentTransform_NewBie.gameObject.SetActive(false);
        blockLevelContentTransform.gameObject.SetActive(true);
        blockLevelContentTransform_Vertical.gameObject.SetActive(true);
        blockLevelContentTransform_Horizontal.gameObject.SetActive(true);
        blockLevelContentTransform_Square.gameObject.SetActive(true);


        blockGridParent_Gosu.SetActive(true);
        blockContentTransform_Gosu.gameObject.SetActive(true);
        blockContentTransform_Gosu.GetComponent<GridLayoutGroup>().enabled = true;

        numberContentTransform.gameObject.SetActive(true);

        rouletteContentList_Target = rouletteContentList;
        blockLevelContentList_Target = blockLevelContentList;

        //for (int i = 0; i < 3; i ++)
        //{
        //    blockContentList[i].gameObject.SetActive(true);
        //}

        limitLevel = rankDataBase.GetLimitLevel(GameStateManager.instance.PlayRankType) - 1;

        windMax = 0;
        windGranularity = 0;

        if (playerDataBase.Armor != null)
        {
            if(playerDataBase.Armor.Length > 0)
            {
                blockClassArmor = playerDataBase.GetBlockClass(playerDataBase.Armor);

                myBlockLevel = blockClassArmor.level;

                blockContentList[0].gameObject.SetActive(true);

                if (myBlockLevel > limitLevel)
                {
                    myBlockLevel = limitLevel;
                    blockClassArmor.level = limitLevel;

                    NotionManager.instance.UseNotion(NotionType.HighLevelLimit);
                }

                if(blockClassArmor.rankType == RankType.SSR)
                {
                    windMax += blockClassArmor.ssrLevel + 1;
                }
                else if (blockClassArmor.rankType == RankType.UR)
                {
                    windMax += 5;
                    windGranularity += 1;
                }

                myBlockLevel2 = upgradeDataBase.GetUpgradeValue(blockClassArmor.rankType).GetValueNumber(myBlockLevel);

                blockContentList[0].InGame_Initialize(blockClassArmor, 0, myBlockLevel2);
                blockContentList[0].InGame_SetLevel(myBlockLevel);
                bettingValue[0] = upgradeDataBase.GetUpgradeValue(blockClassArmor.rankType).GetValueNumber(myBlockLevel);
                bettingSizeList[0] = blockDataBase.GetBlockInfomation(blockClassArmor.blockType).GetSize();

                //Debug.Log("아머를 장착했습니다");
            }
            else
            {
                //Debug.Log("아머가 장착되지 않았습니다");
            }
        }

        if (playerDataBase.Weapon != null)
        {
            if (playerDataBase.Weapon.Length > 0)
            {
                blockClassWeapon = playerDataBase.GetBlockClass(playerDataBase.Weapon);

                myBlockLevel = blockClassWeapon.level;

                blockContentList[1].gameObject.SetActive(true);

                if (myBlockLevel > limitLevel)
                {
                    myBlockLevel = limitLevel;
                    blockClassWeapon.level = limitLevel;

                    NotionManager.instance.UseNotion(NotionType.HighLevelLimit);
                }

                if (blockClassWeapon.rankType == RankType.SSR)
                {
                    windMax += blockClassWeapon.ssrLevel + 1;
                }
                else if (blockClassWeapon.rankType == RankType.UR)
                {
                    windMax += 5;
                    windGranularity += 1;
                }

                myBlockLevel2 = upgradeDataBase.GetUpgradeValue(blockClassWeapon.rankType).GetValueNumber(myBlockLevel);

                blockContentList[1].InGame_Initialize(blockClassWeapon, 1, myBlockLevel2);
                blockContentList[1].InGame_SetLevel(myBlockLevel);
                bettingValue[1] = upgradeDataBase.GetUpgradeValue(blockClassWeapon.rankType).GetValueNumber(myBlockLevel);
                bettingSizeList[1] = blockDataBase.GetBlockInfomation(blockClassWeapon.blockType).GetSize();

                //Debug.Log("검을 장착했습니다");
            }
            else
            {
                //Debug.Log("검이 장착되지 않았습니다");
            }
        }

        if (playerDataBase.Shield != null)
        {
            if (playerDataBase.Shield.Length > 0)
            {
                blockClassShield = playerDataBase.GetBlockClass(playerDataBase.Shield);

                myBlockLevel = blockClassShield.level;

                blockContentList[2].gameObject.SetActive(true);

                if (myBlockLevel > limitLevel)
                {
                    myBlockLevel = limitLevel;
                    blockClassShield.level = limitLevel;

                    NotionManager.instance.UseNotion(NotionType.HighLevelLimit);
                }

                if (blockClassShield.rankType == RankType.SSR)
                {
                    windMax += blockClassShield.ssrLevel + 1;
                }
                else if (blockClassShield.rankType == RankType.UR)
                {
                    windMax += 5;
                    windGranularity += 1;
                }

                myBlockLevel2 = upgradeDataBase.GetUpgradeValue(blockClassShield.rankType).GetValueNumber(myBlockLevel);

                blockContentList[2].InGame_Initialize(blockClassShield, 2, myBlockLevel2);
                blockContentList[2].InGame_SetLevel(myBlockLevel);
                bettingValue[2] = upgradeDataBase.GetUpgradeValue(blockClassShield.rankType).GetValueNumber(myBlockLevel);
                bettingSizeList[2] = blockDataBase.GetBlockInfomation(blockClassShield.blockType).GetSize();

                //Debug.Log("쉴드를 장착했습니다");
            }
            else
            {
                //Debug.Log("쉴드가 장착되지 않았습니다");
            }
        }

        Invoke("GridDelay", 0.5f);

        GameEvent();
    }

    void GridDelay()
    {
        for(int i = 0; i < blockContentList.Count; i ++)
        {
            blockContentList[i].SetPos();
        }

        blockContentTransform_Gosu.GetComponent<GridLayoutGroup>().enabled = false;
    }

    void SetStakes() //판돈 설정
    {
        stakes = GameStateManager.instance.Stakes;

        money = stakes;
        otherMoney = stakes;

        moneyText.text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
        otherMoneyText.text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";
    }

    public void GameEvent()
    {
        GameReset();

        ResetBettingMoney();

        SetStakes();

        turnText.localizationName = "Turn";
        turnText.plusText = ": " + turn;
        turnText.ReLoad();

        GameStateManager.instance.GameEventType = GameEventType.GameEvent1;

        if (GameStateManager.instance.ReEnter)
        {
            Invoke("CheckPlayerState", 1.0f);

            GameStateManager.instance.ReEnter = false;
        }
        else
        {
            ht = PhotonNetwork.CurrentRoom.CustomProperties;

            if(GameStateManager.instance.GameType == GameType.Gosu)
            {
                gameEventManager.OnEventStart(ht["Event"].ToString());
            }
            else if (GameStateManager.instance.GameType == GameType.NewBie && GameStateManager.instance.PlayRankType > GameRankType.Bronze_1)
            {
                gameEventManager.OnEventStart(ht["Event"].ToString());
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Ready" } });

            rouletteManager.CreateObj();
            timer += 3;
            bettingWaitTime += 3;
            StartCoroutine(WaitTimerCoroution());
        }
    }

    public void GameStart()
    {
        switch (GameStateManager.instance.GameEventType)
        {
            case GameEventType.GameEvent1:
                break;
            case GameEventType.GameEvent2:
                ChangeTurn(7);
                break;
            case GameEventType.GameEvent3:
                break;
            case GameEventType.GameEvent4:
                break;
            case GameEventType.GameEvent5:
                break;
        }
    }

    public void CheckPlayerState()
    {
        GameStart_Initialize();

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            GameStart_Newbie();
        }
        else
        {
            GameStart_Gosu();
        }

        ht = PhotonNetwork.CurrentRoom.CustomProperties;

        switch (ht["Status"])
        {
            case "Betting":
                RestartGame();
                break;
            case "Roulette":
                OpenRouletteView();
                rouletteManager.CheckRouletteState();
                break;
            //case "Bouns":
            //    OpenRouletteView();
            //    rouletteManager.SpectatorRoulette();
            //    break;
        }

        GameStateManager.instance.GameEventType = GameEventType.GameEvent1 + int.Parse(ht["Event"].ToString());

        Debug.Log("현재 게임 상태를 불러옵니다");

        LoadMoney();
    }

    public void CheckWinnerPlayer() //게임 누가 승리했는지 체크
    {
        //if (!PhotonNetwork.IsMasterClient) return;

        Debug.Log("누가 이겼는지 체크중입니다");

        if(money <= 0 && otherMoney <= 0) //둘다 0원일때
        {
            GameStateManager.instance.Playing = false;
            PV.RPC("SetGameEnd", RpcTarget.Others);

            GameEnd(3);
        }
        else if(money <= 0) //내돈이 다 떨어졌을 때
        {
            GameStateManager.instance.Playing = false;
            PV.RPC("SetGameEnd", RpcTarget.Others);

            GameEnd(1);
        }
        else if(otherMoney <= 0) //상대방 돈이 다 떨어졌을때
        {
            GameStateManager.instance.Playing = false;
            PV.RPC("SetGameEnd", RpcTarget.Others);

            GameEnd(0);
        }
        else
        {
            Debug.Log("승패가 나지 않았습니다.");
        }
    }

    public void CheckGameState()
    {
        ht = PhotonNetwork.CurrentRoom.CustomProperties;

        StopAllCoroutines();

        switch (ht["Status"])
        {
            case "Ready":
                StartCoroutine(WaitTimerCoroution());
                break;
            case "Betting":
                rouletteManager.CreateObj();
                StartCoroutine(TimerCoroution());
                break;
            case "Roulette":
                GameStateManager.instance.Playing = false;
                GameEnd(0);
                break;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Pinball", GameStateManager.instance.NickName } });

        Debug.Log("게임을 이어서 진행합니다");
    }

    [PunRPC]

    void SetGameEnd()
    {
        GameStateManager.instance.Playing = false;
    }

    [PunRPC]
    void GameEnd(int number)
    {
        StopAllCoroutines();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        GameStateManager.instance.Playing = false;
        GameStateManager.instance.Room = "";

        SoundManager.instance.StopLoopSFX(GameSfxType.Roulette);
        SoundManager.instance.StopBGM();

        uIManager.dontTouchObj.SetActive(false);

        if (number == 0)
        {
            if (money < GameStateManager.instance.Stakes)
            {
                money = GameStateManager.instance.Stakes + (int)(GameStateManager.instance.Stakes * 0.1f);

                Debug.Log("리타이어 승리");

                FirebaseAnalytics.LogEvent("InGame_Win_Retire_" + GameStateManager.instance.GameType.ToString());
            }
            else
            {
                Debug.Log("승리");

                FirebaseAnalytics.LogEvent("InGame_Win_" + GameStateManager.instance.GameType.ToString());
            }

        }
        else if(number == 1)
        {
            money = -stakes;

            Debug.Log("패배");

            FirebaseAnalytics.LogEvent("InGame_Lose_" + GameStateManager.instance.GameType.ToString());
        }
        else if (number == 2)
        {
            money = GameStateManager.instance.Stakes + (int)(GameStateManager.instance.Stakes * 0.1f);

            Debug.Log("상대방 항복으로 승리");

            FirebaseAnalytics.LogEvent("InGame_Win_Surrender_" + GameStateManager.instance.GameType.ToString());
        }
        else
        {
            money = 0;

            Debug.Log("무승부");

            FirebaseAnalytics.LogEvent("InGame_Tie_" + GameStateManager.instance.GameType.ToString());
        }

        timerAnimation.StopAnim();

        rouletteManager.CloseRouletteView();
        uIManager.OpenResultView(number, money);
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

        for (int i = 0; i < rouletteContentList_Target.Count; i++)
        {
            rouletteContentList_Target[i].SetActiveFalseAll();
        }

        while (timer > 0)
        {
            timer -= 1;
            PV.RPC("ChangeWaitTimer", RpcTarget.Others, timer);

            timerFillAmount.fillAmount = timer / ((bettingWaitTime - 1) * 1.0f);
            timerText.text = LocalizationManager.instance.GetString("ReadyToGame") + " : " + timer;
            yield return waitForSeconds;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            bettingWaitTime = GameStateManager.instance.BettingWaitTime;

            PV.RPC("RestartGame", RpcTarget.All);

            StartCoroutine(TimerCoroution());
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Betting" } });
        }
    }

    [PunRPC]
    void ChangeTimer(int number)
    {
        if (!GameStateManager.instance.Playing) return;

        timer = number;

        timerFillAmount.fillAmount = timer / ((bettingTime - 1) * 1.0f);
        timerText.text = LocalizationManager.instance.GetString("ReadyToGame") + " : " + timer;

        if (timer <= 0)
        {
            SoundManager.instance.StopSFX(GameSfxType.TimesUp);
        }
        else
        {
            if (timer <= 3)
            {
                if (!isTimesUp)
                {
                    isTimesUp = true;

                    timerText.color = Color.red;
                    timerAnimation.PlayAnim();

                    SoundManager.instance.PlaySFX(GameSfxType.TimesUp);
                }
                else
                {
                    if (blockType == BlockType.Default)
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.TimesUp);
                    }
                    else
                    {
                        SoundManager.instance.StopSFX(GameSfxType.TimesUp);
                    }
                }
            }
        }
    }

    [PunRPC]
    void ChangeWaitTimer(int number)
    {
        timer = number;

        timerFillAmount.fillAmount = timer / ((bettingWaitTime - 1) * 1.0f);
        timerText.text = LocalizationManager.instance.GetString("ReadyToGame") + " : " + timer;
    }

    [PunRPC]
    void RestartGame()
    {
        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            newbieBlockContent.ResetPos();

            for (int i = 0; i < rouletteContentList_Target.Count; i++)
            {
                rouletteContentList_Target[i].SetActiveFalseAll();
            }

            for (int i = 0; i < blockLevelContentList_NewBie.Count; i++)
            {
                blockLevelContentList_NewBie[i].Initialize();
            }

            for (int i = 0; i < numberContentList_NewBie.Count; i++)
            {
                numberContentList_NewBie[i].Betting_Initialize();
            }
        }
        else
        {
            for (int i = 0; i < blockContentList.Count; i++)
            {
                if (blockContentList[i].gameObject.activeInHierarchy) blockContentList[i].ResetPos();
            }

            for (int i = 0; i < allContentList.Count; i++)
            {
                allContentList[i].SetActiveFalseAll();
            }

            for (int i = 0; i < allBlockLevelContentList.Count; i++)
            {
                allBlockLevelContentList[i].Initialize();
            }

            for (int i = 0; i < numberContentList.Count; i++)
            {
                numberContentList[i].Betting_Initialize();
            }
        }

        bettingNumberList.Clear();
        otherBettingNumberList.Clear();

        for (int i = 0; i < bettingList.Length; i++)
        {
            bettingList[i] = 0;
        }

        ResetBettingMoney();
        ClearOtherPlayerBlock();

        //moneyAnimation.Initialize();

        if (aiMode)
        {
            aiManager.RestartGame();

            timerAi = Random.Range(5, bettingTime - 2);
            timerEmoteAi = Random.Range(4, bettingTime - 1);
        }

        targetObj.SetActive(false);
        targetQueenObj.SetActive(false);

        timer = bettingTime;
        timerFillAmount.fillAmount = 1;

        uIManager.SetWaitingView(false);
        uIManager.dontTouchObj.SetActive(false);

        if(PhotonNetwork.IsMasterClient)
        {
            turn += 1;

            PV.RPC("ChangeTurn", RpcTarget.All, turn);

            if (PhotonNetwork.CurrentRoom.PlayerCount < 2 && !aiMode)
            {
                Winner();
            }
        }

        CheckTip();
    }

    [Button]
    void TurnUp()
    {
        turn += 1;
        ChangeTurn(turn);
    }

    [PunRPC]
    void ChangeTurn(int number)
    {
        turn = number;

        if (!inGameBurning)
        {
            if (turn >= 7)
            {
                inGameBurning = true;

                burningObj.SetActive(true);

                HalfMoney();

                NotionManager.instance.UseNotion2(NotionType.InGameBurning);
            }
            else
            {
                SoundManager.instance.PlaySFX(GameSfxType.Click);
                NotionManager.instance.UseNotion(NotionType.BettingTimesUp);
            }
        }
        else
        {
            if (!inGameBurning2)
            {
                if (turn >= 11)
                {
                    inGameBurning2 = true;

                    turnCount = 1;

                    HalfMoney();

                    NotionManager.instance.UseNotion2(NotionType.InGameBurning2);
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Click);
                    NotionManager.instance.UseNotion(NotionType.BettingTimesUp);
                }
            }
            else
            {
                if (turnCount >= 3)
                {
                    turnCount = 1;

                    HalfMoney();

                    NotionManager.instance.UseNotion2(NotionType.InGameBurning3);
                }
                else
                {
                    turnCount += 1;

                    NotionManager.instance.UseNotion(NotionType.BettingTimesUp);
                }
            }
        }

        turnText.localizationName = "Turn";
        turnText.plusText = ": " + turn;
        turnText.ReLoad();

        //Debug.Log("현재 턴 : " + turn);

        UpdateMoney();
    }

    void HalfMoney()
    {
        halfMoney = (int)(money * 0.5f);
        halfOtherMoney = (int)(otherMoney * 0.5f);

        money = money - halfMoney;
        otherMoney = otherMoney - halfOtherMoney;

        moneyAnimation.MinusMoneyAnimationMid(money + halfMoney, halfMoney);
        moneyAnimation.MinusMoneyAnimationMidEnemy(otherMoney + halfOtherMoney, halfOtherMoney);

        fadeInOut.InGameFadeIn(Color.red);
    }

    void CheckTip()
    {
        tutorialArrow.SetActive(true);

        switch (GameStateManager.instance.GameType)
        {
            case GameType.NewBie:

                tipObj.SetActive(true);
                tipText.localizationName = "Tip_" + (Random.Range(0, 6).ToString());
                tipText.ReLoad();

                Invoke("CloseTip", 8f);

                break;
            case GameType.Gosu:
                if(GameStateManager.instance.GameRankType < GameRankType.Sliver_1)
                {
                    tipObj.SetActive(true);
                    tipText.localizationName = "Tip_" + (Random.Range(6, 13).ToString());
                    tipText.ReLoad();

                    Invoke("CloseTip", 8f);
                }

                break;
        }
    }

    void CloseTip()
    {
        tipObj.SetActive(false);
    }


    IEnumerator TimerCoroution()
    {
        if(aiMode)
        {
            aiMoveBlock = false;
            aiEmote = false;

            if (Random.Range(0, 10) < 7)
            {
                aiMoveBlock = true;

                Debug.Log("Ai가 블록을 0초 남기고 이동시킬 예정입니다");
            }

            if (Random.Range(0, 10) < 2)
            {
                aiEmote = true;

                Debug.Log("Ai가 이모티콘을 사용할 예정입니다");
            }
        }

        while (timer > 0)
        {
            if (!GameStateManager.instance.Playing) break;

            timer -= 1;

            PV.RPC("ChangeTimer", RpcTarget.Others, timer);

            if (timer <= 5)
            {
                if (!isTimesUp)
                {
                    isTimesUp = true;

                    timerText.color = Color.red;
                    timerAnimation.PlayAnim();

                    if(blockType == BlockType.Default)
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.TimesUp);
                    }
                }
                else
                {
                    if(blockType == BlockType.Default)
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.TimesUp);
                    }
                    else
                    {
                        SoundManager.instance.StopSFX(GameSfxType.TimesUp);
                    }
                }
                
            }

            timerFillAmount.fillAmount = timer / ((bettingTime - 1) * 1.0f);
            timerText.text = LocalizationManager.instance.GetString("ReadyToGame") + " : " + timer;

            if (aiMode)
            {
                if (timer <= timerAi)
                {
                    aiManager.PutBlock();
                }

                if(aiMoveBlock)
                {
                    if(timer <= 0)
                    {
                        aiManager.MoveBlock();
                    }
                }

                if(aiEmote)
                {
                    if (timer <= timerEmoteAi)
                    {
                        emoteManager.UseEmote_Ai();

                        aiEmote = false;
                    }
                }
            }

            yield return waitForSeconds;
        }

        SoundManager.instance.StopSFX(GameSfxType.TimesUp);

        yield return new WaitForSeconds(1.0f);

        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("DelayRoulette", RpcTarget.All);
        }

        yield return new WaitForSeconds(1.5f);

        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("OpenRouletteView", RpcTarget.All);
        }
    }

    [PunRPC]
    void DelayRoulette()
    {
        if (!GameStateManager.instance.Playing) return;

        isTimesUp = false;
        timerText.color = new Color(7 / 255f, 80 / 255f, 93 / 255f);
        timerAnimation.StopAnim();

        uIManager.dontTouchObj.SetActive(true);

        for (int i = 0; i < blockContentList.Count; i++)
        {
            if (blockContentList[i].isDrag)
            {
                if (blockContentList[i].gameObject.activeInHierarchy)
                {
                    blockContentList[i].TimeOver();
                }
            }
        }

        ResetRouletteBackgroundColor();

        ShowBettingNumber_Total();
    }

    bool CheckDeveloper()
    {
        bool check = false;

        if (inputTargetNumber.gameObject.activeInHierarchy)
        {
            if (inputTargetNumber.text.Length > 0)
            {
                int number = int.Parse(inputTargetNumber.text.ToString());

                targetQueenNumber = 0;

                if (GameStateManager.instance.GameType == GameType.NewBie)
                {
                    if (number <= 0)
                    {
                        targetNumber = 1;
                        targetQueenNumber = 1;

                        check = true;
                    }
                    else if (number <= 8)
                    {
                        targetNumber = number;

                        check = true;
                    }
                    else
                    {
                        //GameStateManager.instance.CheckBouns = true;

                        check = false;
                    }
                }
                else
                {
                    if (number <= 0)
                    {
                        targetNumber = 1;
                        targetQueenNumber = 1;

                        check = true;
                    }
                    else if (number <= 24)
                    {
                        targetNumber = number;

                        check = true;
                    }
                    else
                    {
                        //GameStateManager.instance.CheckBouns = true;

                        check = false;
                    }
                }
            }
        }

        return check;
    }

    [PunRPC]
    void OpenRouletteView()
    {
        if (!GameStateManager.instance.Playing) return;

        uIManager.dontTouchObj.SetActive(false);
        gameEventManager.eventInfoView.SetActive(false);

        SoundManager.instance.StopSFX(GameSfxType.TimesUp);

        //money -= bettingMoney;

        if(bettingMoney > 0)
        {
            //SetMinusMoney(bettingMoney);

            //PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, bettingMoney);

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
        emoteManager.Initialize();

        isTimesUp = false;
        timerText.color = new Color(7 / 255f, 80 / 255f, 93 / 255f);
        timerAnimation.StopAnim();

        uIManager.CloseRouletteView();
        uIManager.CloseSurrenderView();

        tutorialArrow.SetActive(false);

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

            if(GameStateManager.instance.GameType == GameType.NewBie)
            {
                trans = rouletteContentList_Target[4].transform;
            }
            else
            {
                trans = rouletteContentList_Target[12].transform;
            }
            targetQueenObj.transform.position = trans.position;

            targetText.text = LocalizationManager.instance.GetString("Queen");
        }
        else
        {
            targetObj.SetActive(true);
            targetObj.transform.SetAsLastSibling();
        }

        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            if (targetNumber > 4)
            {
                trans = rouletteContentList_Target[targetNumber].transform;
                targetObj.transform.position = trans.position;
            }
            else
            {
                trans = rouletteContentList_Target[targetNumber - 1].transform;
                targetObj.transform.position = trans.position;
            }
        }
        else
        {
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
        }

        targetText.text = targetNumber.ToString();
        recordText.text += targetNumber + ", ";

        plusMoney = 0; //획득한 돈
        plusAiMoney = 0;

        if (targetQueenNumber == 1)
        {
            if(GameStateManager.instance.GameType == GameType.NewBie)
            {
                if(rouletteContentList_Target[4].isActive)
                {
                    ChangeGetMoney(rouletteContentList_Target[4].blockClass, RouletteType.StraightBet, true);
                }

                if(aiMode)
                {
                    if (rouletteContentList_Target[4].isActive_Ai)
                    {
                        ChangeGetMoney_Ai(rouletteContentList_Target[4].blockClass_Ai, RouletteType.StraightBet, true);
                    }
                }
            }
            else
            {
                CheckQueenNumber();

                if (aiMode)
                {
                    CheckQueenNumber_Ai();
                }
            }

            Debug.Log("퀸 당첨");
        }
        else
        {
            Debug.Log(targetNumber + "번 당첨");

            if(GameStateManager.instance.GameType == GameType.NewBie)
            {
                if (targetNumber > 4)
                {
                    targetNumber += 1;
                }

                for (int i = 0; i < rouletteContentList_Target.Count; i++)
                {
                    if (rouletteContentList_Target[i].isActive && targetNumber == rouletteContentList_Target[i].number)
                    {
                        ChangeGetMoney(rouletteContentList_Target[i].blockClass, RouletteType.StraightBet, false);
                        break;
                    }
                }

                if(aiMode)
                {
                    for (int i = 0; i < rouletteContentList_Target.Count; i++)
                    {
                        if (rouletteContentList_Target[i].isActive_Ai && targetNumber == rouletteContentList_Target[i].number)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Target[i].blockClass_Ai, RouletteType.StraightBet, false);
                            break;
                        }
                    }
                }
            }
            else
            {
                if (targetNumber > 12)
                {
                    targetNumber += 1;
                }

                CheckTargetNumber(targetNumber);

                if (aiMode)
                {
                    CheckTargetNumber_Ai(targetNumber);
                }
            }
        }

        if (plusMoney > 0)
        {
            if(inGameBurning)
            {
                plusMoney *= 2;
            }

            for (int i = 0; i < bettingMinusList.Length; i++) //마지막에 당첨 안 된거 만큼 빼기
            {
                if (bettingMinusList[i] > 0)
                {
                    plusMoney = plusMoney + ((bettingValue[i] * 1.0f / bettingSizeList[i] * 1.0f) * bettingPlusList[i]) -
                        ((bettingValue[i] * 1.0f / bettingSizeList[i] * 1.0f) * bettingMinusList[i]);
                    break;
                }
            }
        }

        for (int i = 0; i < bettingMinusList.Length; i ++)
        {
            bettingMinusList[i] = 0;
        }

        for(int i = 0; i < bettingPlusList.Length; i ++)
        {
            bettingPlusList[i] = 0;
        }

        if (aiMode)
        {
            if (plusAiMoney > 0)
            {
                switch (blockDataBase.GetSize(otherBlockType))
                {
                    case 1:
                        plusAiMoney = plusAiMoney + (aiManager.GetValue(otherBlockType) * 1.0f / blockDataBase.GetSize(otherBlockType) * 1.0f);
                        break;
                    case 2:
                        break;
                    case 3:
                        plusAiMoney = plusAiMoney - aiManager.GetValue(otherBlockType) * 1.0f / blockDataBase.GetSize(otherBlockType) * 1.0f * 2;
                        break;
                }
            }
        }

        Debug.LogError("My : " + plusMoney);
        Debug.LogError("Ai : " + plusAiMoney);

        if(GameStateManager.instance.Newbie)
        {
            plusAiMoney *= 0.1f;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            Debug.Log("상대방한테 값을 전달했습니다");

            int[] compare = new int[2];
            compare[0] = bettingMoney;
            compare[1] = (int)(plusMoney);
            PV.RPC("CompareMoney", RpcTarget.Others, compare);
        }
        else
        {
            Debug.Log("Ai 결과값 비교중입니다");

            bettingAiMoney = aiManager.bettingValue[aiManager.blockIndex];

            int[] compare = new int[2];
            compare[0] = bettingAiMoney;
            compare[1] = (int)plusAiMoney;

            CompareMoney(compare);
        }

        ResetRouletteBackgroundColor();

        for (int i = 0; i < blockContentList.Count; i++)
        {
            if (blockContentList[i].isDrag)
            {
                if (blockContentList[i].gameObject.activeInHierarchy)
                {
                    blockContentList[i].TimeOver();
                }
                break;
            }
        }

        timer = bettingWaitTime;
        timerFillAmount.fillAmount = 1;

        if (inputTargetNumber.gameObject.activeInHierarchy)
        {
            nextButton.SetActive(true);
            uIManager.dontTouchObj.SetActive(false);

            return;
        }

        if (bettingMoney == 0)
        {
            keepCount += 1;

            if(keepCount >= 3)
            {
                Debug.Log("3턴 동안 움직이지 않아 패배합니다");

                SurrenderButton();
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            blockType = BlockType.Default;
            otherBlockType = BlockType.Default;

            Debug.Log("타이머를 다시 시작합니다");

            StartCoroutine(WaitTimerCoroution());
        }
    }

    [PunRPC]
    public void CompareMoney(int[] compare)
    {
        Debug.LogError("내가 배팅 한 돈 : " + bettingMoney + " / 내가 딴 돈 : " + (int)plusMoney);
        Debug.LogError("상대방이 배팅 돈 : " + compare[0] + " / 상대방이 딴 돈 : " + compare[1]);

        if(inGameBurning)
        {
            compare[1] *= 2;
        }

        money -= bettingMoney; //내 배팅한 금액만큼 일단 빼기
        otherMoney -= compare[0]; //상대방 배팅한 금액만큼 일단 빼기

        this.compareValue = (int)plusMoney - compare[1];

        if (this.compareValue > 0)
        {
            moneyAnimation.AddMoneyAnimation(money, otherMoney, this.compareValue);

            fadeInOut.InGameFadeIn(Color.blue);

            money += this.compareValue;
            otherMoney -= this.compareValue;

            RecordManager.instance.SetRecord(this.compareValue.ToString());
        }
        else if (this.compareValue < 0)
        {
            moneyAnimation.MinusMoneyAnimation(money, otherMoney, Mathf.Abs(this.compareValue));

            fadeInOut.InGameFadeIn(Color.red);

            money -= Mathf.Abs(this.compareValue);
            otherMoney += Mathf.Abs(this.compareValue);

            RecordManager.instance.SetRecord(this.compareValue.ToString());
        }
        else
        {
            if ((int)plusMoney > 0 && (int)plusMoney > 0 && (int)plusMoney == compare[1])
            {

            }
            else
            {
                if (bettingMoney > 0)
                {
                    moneyAnimation.MinusMoneyAnimationMid(money + bettingMoney, bettingMoney);

                    fadeInOut.InGameFadeIn(Color.red);
                }

                if (compare[0] > 0)
                {
                    moneyAnimation.MinusMoneyAnimationMidEnemy(otherMoney + compare[0], compare[0]);

                    fadeInOut.InGameFadeIn(Color.red);
                }
            }

            RecordManager.instance.SetRecord((-bettingMoney).ToString());
        }

        UpdateMoney();
    }

    public void ChangeGetMoney(BlockClass block, RouletteType type, bool queen)
    {
        value = 0;
        blockLevel = 0;

        blockLevel = block.level;

        if (blockLevel > limitLevel)
        {
            blockLevel = limitLevel;
        }

        if (!allIn)
        {
            value = upgradeDataBase.GetUpgradeValue(block.rankType).GetValueNumber(blockLevel) * 1.0f / blockDataBase.GetSize(block.blockType) * 1.0f;
        }
        else
        {
            value = bettingMoney * 1.0f / blockDataBase.GetSize(block.blockType) * 1.0f;
        }

        for(int i = 0; i < bettingValue.Length; i ++) //당첨된 블럭 빼주기
        {
            if (bettingList[i] > 0)
            {
                if (value.ToString() == (bettingValue[i] * 1.0f / blockDataBase.GetSize(block.blockType) * 1.0f).ToString())
                {
                    bettingMinusList[i] -= 1;
                    bettingPlusList[i] += 1;
                }
            }
        }

        switch (type)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                if(queen)
                {
                    plusMoney += blockMotherInformation.queenStraightBet * value;
                }
                else
                {
                    plusMoney += blockMotherInformation.straightBet * value;
                }

                break;
            case RouletteType.SplitBet_Horizontal:
                if (queen)
                {
                    plusMoney += blockMotherInformation.queensplitBet * (value / 2);
                }
                else
                {
                    plusMoney += blockMotherInformation.splitBet * (value / 2);
                }

                break;
            case RouletteType.SplitBet_Vertical:
                if (queen)
                {
                    plusMoney += blockMotherInformation.queensplitBet * (value / 2);
                }
                else
                {
                    plusMoney += blockMotherInformation.splitBet * (value / 2);
                }

                break;
            case RouletteType.SquareBet:
                if (queen)
                {
                    plusMoney += blockMotherInformation.queensquareBet * (value / 4);
                }
                else
                {
                    plusMoney += blockMotherInformation.squareBet * (value / 4);
                }

                break;
        }
    }

    public void ChangeGetMoney_Ai(BlockClass block, RouletteType type, bool queen)
    {
        value_Ai = 0;
        value_Ai = upgradeDataBase.GetUpgradeValue(block.rankType).GetValueNumber(block.level) * 1.0f / blockDataBase.GetSize(otherBlockType) * 1.0f;

        switch (type)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queenStraightBet * value_Ai;
                }
                else
                {
                    plusAiMoney += blockMotherInformation.straightBet * value_Ai;
                }

                break;
            case RouletteType.SplitBet_Horizontal:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queensplitBet * (value_Ai / 2);
                }
                else
                {
                    plusAiMoney += blockMotherInformation.splitBet * (value_Ai / 2);
                }

                break;
            case RouletteType.SplitBet_Vertical:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queensplitBet * (value_Ai / 2);
                }
                else
                {
                    plusAiMoney += blockMotherInformation.splitBet * (value_Ai / 2);
                }

                break;
            case RouletteType.SquareBet:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queensquareBet * (value_Ai / 4);
                }
                else
                {
                    plusAiMoney += blockMotherInformation.squareBet * (value_Ai / 4);
                }

                break;
        }
    }

    void ChangeBettingMoney()
    {
        //moneyText.text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(money - bettingMoney) + "</size>";
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

    private void CheckTargetNumber(int number)
    {
        for (int i = 0; i < rouletteContentList_Target.Count; i++)
        {
            if (rouletteContentList_Target[i].isActive && number == rouletteContentList_Target[i].number)
            {
                ChangeGetMoney(rouletteContentList_Target[i].blockClass, RouletteType.StraightBet, false);
                break;
            }
        }

        for (int i = 0; i < rouletteContentList_Split_Horizontal.Count; i++) // 1 2 / 3 4
        {
            if (rouletteContentList_Split_Horizontal[i].isActive)
            {
                switch (i)
                {
                    case 0:
                        if (number == 1 || number == 6)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 7)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 8)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 9)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 4:
                        if (number == 5 || number == 10)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 5:
                        if (number == 6 || number == 11)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 6:
                        if (number == 7 || number == 12)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 7:
                        if (number == 8)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 8:
                        if (number == 9 || number == 14)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 9:
                        if (number == 10 || number == 15)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 10:
                        if (number == 11 || number == 16)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 11:
                        if (number == 12 || number == 17)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 12:
                        if (number == 18)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 13:
                        if (number == 14 || number == 19)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 14:
                        if (number == 15 || number == 20)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 15:
                        if (number == 16 || number == 21)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 16:
                        if (number == 17 || number == 22)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 17:
                        if (number == 18 || number == 23)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 18:
                        if (number == 19 || number == 24)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 19:
                        if (number == 20 || number == 25)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                }
            }
        }

        for (int i = 0; i < rouletteContentList_Split_Vertical.Count; i++) // 1 6 / 2 7
        {
            if (rouletteContentList_Split_Vertical[i].isActive)
            {
                switch (i)
                {
                    case 0:
                        if (number == 1 || number == 2)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 3)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 4)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 5)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 4:
                        if (number == 6 || number == 7)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 5:
                        if (number == 7 || number == 8)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 6:
                        if (number == 8 || number == 9)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 7:
                        if (number == 9 || number == 10)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 8:
                        if (number == 11 || number == 12)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 9:
                        if (number == 12)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 10:
                        if (number == 13)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 11:
                        if (number == 14 || number == 15)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 12:
                        if (number == 16 || number == 17)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 13:
                        if (number == 17 || number == 18)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 14:
                        if (number == 18 || number == 19)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 15:
                        if (number == 19 || number == 20)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 16:
                        if (number == 21 || number == 22)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 17:
                        if (number == 22 || number == 23)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 18:
                        if (number == 23 || number == 24)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 19:
                        if (number == 24 || number == 25)
                        {
                            ChangeGetMoney(rouletteContentList_Split_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                }
            }
        }

        for (int i = 0; i < rouletteContentList_Square.Count; i++) //1 2 6 7
        {
            if (rouletteContentList_Square[i].isActive)
            {
                switch (i)
                {
                    case 0:
                        if (number == 1 || number == 2 || number == 6 || number == 7)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 3 || number == 7 || number == 8)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 4 || number == 8 || number == 9)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 5 || number == 9 || number == 10)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 4:
                        if (number == 6 || number == 7 || number == 11 || number == 12)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 5:
                        if (number == 7 || number == 8 || number == 12)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 6:
                        if (number == 8 || number == 9 || number == 14)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 7:
                        if (number == 9 || number == 10 || number == 14 || number == 15)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 8:
                        if (number == 11 || number == 12 || number == 16 || number == 17)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 9:
                        if (number == 12 || number == 17 || number == 18)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 10:
                        if (number == 14 || number == 18 || number == 19)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 11:
                        if (number == 14 || number == 15 || number == 19 || number == 20)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 12:
                        if (number == 16 || number == 17 || number == 21 || number == 22)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 13:
                        if (number == 17 || number == 18 || number == 22 || number == 23)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 14:
                        if (number == 18 || number == 19 || number == 23 || number == 24)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 15:
                        if (number == 19 || number == 20 || number == 24 || number == 25)
                        {
                            ChangeGetMoney(rouletteContentList_Square[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                }
            }
        }
    }

    private void CheckTargetNumber_Ai(int number)
    {
        for (int i = 0; i < rouletteContentList_Target.Count; i++)
        {
            if (rouletteContentList_Target[i].isActive_Ai && number == rouletteContentList_Target[i].number)
            {
                ChangeGetMoney_Ai(rouletteContentList_Target[i].blockClass_Ai, RouletteType.StraightBet, false);
                break;
            }
        }

        for (int i = 0; i < rouletteContentList_Split_Horizontal.Count; i++) // 1 2 / 3 4
        {
            if (rouletteContentList_Split_Horizontal[i].isActive_Ai)
            {
                switch (i)
                {
                    case 0:
                        if (number == 1 || number == 6)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 7)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 8)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 9)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 4:
                        if (number == 5 || number == 10)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 5:
                        if (number == 6 || number == 11)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 6:
                        if (number == 7 || number == 12)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 7:
                        if (number == 8)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 8:
                        if (number == 9 || number == 14)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 9:
                        if (number == 10 || number == 15)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 10:
                        if (number == 11 || number == 16)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 11:
                        if (number == 12 || number == 17)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 12:
                        if (number == 18)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 13:
                        if (number == 14 || number == 19)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 14:
                        if (number == 15 || number == 20)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 15:
                        if (number == 16 || number == 21)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 16:
                        if (number == 17 || number == 22)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 17:
                        if (number == 18 || number == 23)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 18:
                        if (number == 19 || number == 24)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 19:
                        if (number == 20 || number == 25)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                }
            }
        }

        for (int i = 0; i < rouletteContentList_Split_Vertical.Count; i++) // 1 6 / 2 7
        {
            if (rouletteContentList_Split_Vertical[i].isActive_Ai)
            {
                switch (i)
                {
                    case 0:
                        if (number == 1 || number == 2)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 3)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 4)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 5)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 4:
                        if (number == 6 || number == 7)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 5:
                        if (number == 7 || number == 8)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 6:
                        if (number == 8 || number == 9)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 7:
                        if (number == 9 || number == 10)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 8:
                        if (number == 11 || number == 12)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 9:
                        if (number == 12)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 10:
                        if (number == 13)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 11:
                        if (number == 14 || number == 15)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 12:
                        if (number == 16 || number == 17)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 13:
                        if (number == 17 || number == 18)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 14:
                        if (number == 18 || number == 19)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 15:
                        if (number == 19 || number == 20)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 16:
                        if (number == 21 || number == 22)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 17:
                        if (number == 22 || number == 23)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 18:
                        if (number == 23 || number == 24)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 19:
                        if (number == 24 || number == 25)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                }
            }
        }

        for (int i = 0; i < rouletteContentList_Square.Count; i++) //1 2 6 7
        {
            if (rouletteContentList_Square[i].isActive_Ai)
            {
                switch (i)
                {
                    case 0:
                        if (number == 1 || number == 2 || number == 6 || number == 7)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 3 || number == 7 || number == 8)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 4 || number == 8 || number == 9)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 5 || number == 9 || number == 10)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 4:
                        if (number == 6 || number == 7 || number == 11 || number == 12)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 5:
                        if (number == 7 || number == 8 || number == 12)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 6:
                        if (number == 8 || number == 9 || number == 14)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 7:
                        if (number == 9 || number == 10 || number == 14 || number == 15)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 8:
                        if (number == 11 || number == 12 || number == 16 || number == 17)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 9:
                        if (number == 12 || number == 17 || number == 18)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 10:
                        if (number == 14 || number == 18 || number == 19)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 11:
                        if (number == 14 || number == 15 || number == 19 || number == 20)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 12:
                        if (number == 16 || number == 17 || number == 21 || number == 22)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 13:
                        if (number == 17 || number == 18 || number == 22 || number == 23)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 14:
                        if (number == 18 || number == 19 || number == 23 || number == 24)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 15:
                        if (number == 19 || number == 20 || number == 24 || number == 25)
                        {
                            ChangeGetMoney_Ai(rouletteContentList_Square[i].blockClass_Ai, RouletteType.SquareBet, false);
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

        if (rouletteContentList_Split_Horizontal[7].isActive)
        {
            ChangeGetMoney(rouletteContentList_Split_Horizontal[7].blockClass, RouletteType.SplitBet_Horizontal, true);
        }

        if (rouletteContentList_Split_Horizontal[12].isActive)
        {
            ChangeGetMoney(rouletteContentList_Split_Horizontal[12].blockClass, RouletteType.SplitBet_Horizontal, true);
        }

        if (rouletteContentList_Split_Vertical[9].isActive)
        {
            ChangeGetMoney(rouletteContentList_Split_Vertical[9].blockClass, RouletteType.SplitBet_Vertical, true);
        }

        if (rouletteContentList_Split_Vertical[10].isActive)
        {
            ChangeGetMoney(rouletteContentList_Split_Vertical[10].blockClass, RouletteType.SplitBet_Vertical, true);
        }

        //if (rouletteContentList_Square[5].isActive)
        //{
        //    ChangeGetMoney(rouletteContentList_Square[5].blockClass, RouletteType.SquareBet, true);
        //}

        //if (rouletteContentList_Square[6].isActive)
        //{
        //    ChangeGetMoney(rouletteContentList_Square[6].blockClass, RouletteType.SquareBet, true);
        //}

        //if (rouletteContentList_Square[9].isActive)
        //{
        //    ChangeGetMoney(rouletteContentList_Square[9].blockClass, RouletteType.SquareBet, true);
        //}

        //if (rouletteContentList_Square[10].isActive)
        //{
        //    ChangeGetMoney(rouletteContentList_Square[10].blockClass, RouletteType.SquareBet, true);
        //}
    }

    private void CheckQueenNumber_Ai()
    {
        if (rouletteContentList_Target[12].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteContentList_Target[12].blockClass_Ai, RouletteType.StraightBet, true);
        }

        if (rouletteContentList_Split_Horizontal[7].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[7].blockClass_Ai, RouletteType.SplitBet_Horizontal, true);
        }

        if (rouletteContentList_Split_Horizontal[12].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteContentList_Split_Horizontal[12].blockClass_Ai, RouletteType.SplitBet_Horizontal, true);
        }

        if (rouletteContentList_Split_Vertical[9].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[9].blockClass_Ai, RouletteType.SplitBet_Vertical, true);
        }

        if (rouletteContentList_Split_Vertical[10].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteContentList_Split_Vertical[10].blockClass_Ai, RouletteType.SplitBet_Vertical, true);
        }

        //if (rouletteContentList_Square[5].isActive_Ai)
        //{
        //    ChangeGetMoney_Ai(rouletteContentList_Square[5].blockClass_Ai, RouletteType.SquareBet, true);
        //}

        //if (rouletteContentList_Square[6].isActive_Ai)
        //{
        //    ChangeGetMoney_Ai(rouletteContentList_Square[6].blockClass_Ai, RouletteType.SquareBet, true);
        //}

        //if (rouletteContentList_Square[9].isActive_Ai)
        //{
        //    ChangeGetMoney_Ai(rouletteContentList_Square[9].blockClass_Ai, RouletteType.SquareBet, true);
        //}

        //if (rouletteContentList_Square[10].isActive_Ai)
        //{
        //    ChangeGetMoney_Ai(rouletteContentList_Square[10].blockClass_Ai, RouletteType.SquareBet, true);
        //}
    }


    public void EnterBlock(RouletteContent rouletteContent, BlockContent blockContent)
    {
        keepCount = 0;

        mainRouletteContent = rouletteContent;

        targetBlockContent = blockContent.transform;

        if (bettingList.Contains(1) && bettingList[blockContent.index] != 0)
        {
            if (GameStateManager.instance.GameType == GameType.NewBie)
            {
                for (int i = 0; i < rouletteContentList_Target.Count; i++)
                {
                    rouletteContentList_Target[i].SetActiveFalse(blockContent.blockClass);
                }

                for (int i = 0; i < blockLevelContentList_NewBie.Count; i++)
                {
                    blockLevelContentList_NewBie[i].Initialize_My();
                }
            }
            else
            {
                for (int i = 0; i < allContentList.Count; i++)
                {
                    allContentList[i].SetActiveFalse(blockContent.blockClass);
                }

                for (int i = 0; i < allBlockLevelContentList.Count; i++)
                {
                    allBlockLevelContentList[i].Initialize_My();
                }
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
                for (int i = 0; i < rouletteContentList_Split_Horizontal.Count; i++)
                {
                    if (rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index0) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index1)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index2) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index3)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index4) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index5)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index6) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index7)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index8))
                    {
                        rouletteContentList_Split_Horizontal[i].SetBackgroundColor(RouletteColorType.Yellow);
                    }
                }
                break;
            case RouletteType.SplitBet_Vertical:
                for (int i = 0; i < rouletteContentList_Split_Vertical.Count; i++)
                {
                    if (rouletteContentList_Split_Vertical[i].index.SequenceEqual(index0) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index1)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index2) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index3)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index4) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index5)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index6) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index7)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index8))
                    {
                        rouletteContentList_Split_Vertical[i].SetBackgroundColor(RouletteColorType.Yellow);
                    }
                }
                break;
            case RouletteType.SquareBet:
                for (int i = 0; i < rouletteContentList_Square.Count; i++)
                {
                    if (rouletteContentList_Square[i].index.SequenceEqual(index0) || rouletteContentList_Square[i].index.SequenceEqual(index1)
                        || rouletteContentList_Square[i].index.SequenceEqual(index2) || rouletteContentList_Square[i].index.SequenceEqual(index3)
                        || rouletteContentList_Square[i].index.SequenceEqual(index4) || rouletteContentList_Square[i].index.SequenceEqual(index5)
                        || rouletteContentList_Square[i].index.SequenceEqual(index6) || rouletteContentList_Square[i].index.SequenceEqual(index7)
                        || rouletteContentList_Square[i].index.SequenceEqual(index8))
                    {
                        rouletteContentList_Square[i].SetBackgroundColor(RouletteColorType.Yellow);
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

        //if (GameStateManager.instance.BlockOverlap)
        //{
        //    switch (mainRouletteContent.rouletteType)
        //    {
        //        case RouletteType.Default:
        //            break;
        //        case RouletteType.StraightBet:
        //            for (int i = 0; i < rouletteContentList_Target.Count; i++)
        //            {
        //                if (rouletteContentList_Target[i].index.SequenceEqual(index0) || rouletteContentList_Target[i].index.SequenceEqual(index1)
        //                    || rouletteContentList_Target[i].index.SequenceEqual(index2) || rouletteContentList_Target[i].index.SequenceEqual(index3)
        //                    || rouletteContentList_Target[i].index.SequenceEqual(index4) || rouletteContentList_Target[i].index.SequenceEqual(index5)
        //                    || rouletteContentList_Target[i].index.SequenceEqual(index6) || rouletteContentList_Target[i].index.SequenceEqual(index7)
        //                    || rouletteContentList_Target[i].index.SequenceEqual(index8))
        //                {
        //                    if (rouletteContentList_Target[i].isActive) blockOverlap = true;
        //                }
        //            }
        //            break;
        //        case RouletteType.SplitBet_Horizontal:
        //            for (int i = 0; i < rouletteContentList_Split_Horizontal.Count; i++)
        //            {
        //                if (rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index0) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index1)
        //                    || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index2) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index3)
        //                    || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index4) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index5)
        //                    || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index6) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index7)
        //                    || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index8))
        //                {
        //                    if (rouletteContentList_Split_Horizontal[i].isActive) blockOverlap = true;
        //                }
        //            }
        //            break;
        //        case RouletteType.SplitBet_Vertical:
        //            for (int i = 0; i < rouletteContentList_Split_Vertical.Count; i++)
        //            {
        //                if (rouletteContentList_Split_Vertical[i].index.SequenceEqual(index0) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index1)
        //                    || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index2) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index3)
        //                    || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index4) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index5)
        //                    || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index6) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index7)
        //                    || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index8))
        //                {
        //                    if (rouletteContentList_Split_Vertical[i].isActive) blockOverlap = true;
        //                }
        //            }
        //            break;
        //        case RouletteType.SquareBet:
        //            for (int i = 0; i < rouletteContentList_Square.Count; i++)
        //            {
        //                if (rouletteContentList_Square[i].index.SequenceEqual(index0) || rouletteContentList_Square[i].index.SequenceEqual(index1)
        //                    || rouletteContentList_Square[i].index.SequenceEqual(index2) || rouletteContentList_Square[i].index.SequenceEqual(index3)
        //                    || rouletteContentList_Square[i].index.SequenceEqual(index4) || rouletteContentList_Square[i].index.SequenceEqual(index5)
        //                    || rouletteContentList_Square[i].index.SequenceEqual(index6) || rouletteContentList_Square[i].index.SequenceEqual(index7)
        //                    || rouletteContentList_Square[i].index.SequenceEqual(index8))
        //                {
        //                    if (rouletteContentList_Square[i].isActive) blockOverlap = true;
        //                }
        //            }
        //            break;
        //    }
        //}
    }

    public void ExitBlock(BlockContent blockContent) //내 블럭 배치 및 상대방한테 내 블럭 위치 전송
    {
        tutorialArrow.SetActive(false);

        SoundManager.instance.PlaySFX(GameSfxType.Click);

        ResetRouletteBackgroundColor();

        if (money < bettingValue[blockContent.index])
        {
            NotionManager.instance.UseNotion(NotionType.BettingAllin);

            Debug.Log("남은 골드를 모두 사용하여 배팅합니다.");

            allIn = true;
        }
        else
        {
            allIn = false;
        }

        if (bettingList.Contains(1) && bettingList[blockContent.index] == 0)
        {
            for(int i = 0; i < bettingList.Length; i ++)
            {
                if(bettingList[i] == 1)
                {
                    blockContentList[i].CancleBetting();
                }
            }

            //SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            //NotionManager.instance.UseNotion(NotionType.OverBettingBlock);

            Debug.Log("기존에 있던 블럭을 해제합니다");
        }

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < rouletteContentList_Target.Count; i++)
            {
                rouletteContentList_Target[i].SetActiveFalse();
            }

            for (int i = 0; i < blockLevelContentList_Target.Count; i++)
            {
                blockLevelContentList_Target[i].Initialize_My();
            }
        }
        else
        {
            for (int i = 0; i < allContentList.Count; i++)
            {
                allContentList[i].SetActiveFalse();
            }

            for (int i = 0; i < allBlockLevelContentList.Count; i++)
            {
                allBlockLevelContentList[i].Initialize_My();
            }
        }

        if (!allIn)
        {
            if (bettingList[blockContent.index] == 0)
            {
                bettingList[blockContent.index] = 1;
                bettingMinusList[blockContent.index] = blockDataBase.GetBlockInfomation(blockContent.blockClass.blockType).GetSize();
                ChangeBettingMoney();
            }

            bettingMoney = bettingValue[blockContent.index];
        }
        else
        {
            if (bettingList[blockContent.index] == 0)
            {
                bettingList[blockContent.index] = 1;
                bettingMinusList[blockContent.index] = blockDataBase.GetBlockInfomation(blockContent.blockClass.blockType).GetSize();
                ChangeBettingMoney();
            }

            bettingMoney = money;
        }

        NotionManager.instance.UseNotion(LocalizationManager.instance.GetString("CurrentValue") + " : " + MoneyUnitString.ToCurrencyString(bettingMoney) + " " +
    LocalizationManager.instance.GetString("PowerBetting"), ColorType.White);

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
                        blockLevelContentList_Target[i].SetMyBlock(blockContent.blockClass.level);
                    }
                }
                break;
            case RouletteType.SplitBet_Horizontal:
                for (int i = 0; i < rouletteContentList_Split_Horizontal.Count; i++)
                {
                    if (rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index0) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index1)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index2) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index3)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index4) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index5)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index6) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index7)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index8))
                    {
                        rouletteContentList_Split_Horizontal[i].SetActiveTrue(blockContent.blockClass);
                        blockLevelContentList_Split_Horizontal[i].SetMyBlock(blockContent.blockClass.level);
                    }
                }
                break;
            case RouletteType.SplitBet_Vertical:
                for (int i = 0; i < rouletteContentList_Split_Vertical.Count; i++)
                {
                    if (rouletteContentList_Split_Vertical[i].index.SequenceEqual(index0) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index1)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index2) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index3)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index4) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index5)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index6) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index7)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index8))
                    {
                        rouletteContentList_Split_Vertical[i].SetActiveTrue(blockContent.blockClass);
                        blockLevelContentList_Split_Vertical[i].SetMyBlock(blockContent.blockClass.level);
                    }
                }
                break;
            case RouletteType.SquareBet:
                for (int i = 0; i < rouletteContentList_Square.Count; i++)
                {
                    if (rouletteContentList_Square[i].index.SequenceEqual(index0) || rouletteContentList_Square[i].index.SequenceEqual(index1)
                        || rouletteContentList_Square[i].index.SequenceEqual(index2) || rouletteContentList_Square[i].index.SequenceEqual(index3)
                        || rouletteContentList_Square[i].index.SequenceEqual(index4) || rouletteContentList_Square[i].index.SequenceEqual(index5)
                        || rouletteContentList_Square[i].index.SequenceEqual(index6) || rouletteContentList_Square[i].index.SequenceEqual(index7)
                        || rouletteContentList_Square[i].index.SequenceEqual(index8))
                    {
                        rouletteContentList_Square[i].SetActiveTrue(blockContent.blockClass);
                        blockLevelContentList_Square[i].SetMyBlock(blockContent.blockClass.level);
                    }
                }
                break;
        }

        insertBlock = new string[6];

        insertBlock[0] = mainRouletteContent.rouletteType.ToString();
        insertBlock[1] = blockContent.blockClass.blockType.ToString();
        insertBlock[2] = mainRouletteContent.number.ToString();
        insertBlock[3] = GameStateManager.instance.NickName;
        insertBlock[4] = bettingValue[blockContent.index].ToString();
        insertBlock[5] = blockContent.blockClass.level.ToString();

        PV.RPC("ShowOtherPlayerBlock", RpcTarget.Others, insertBlock);

        ShowBettingNumber_My();
    }


    public void ResetBlockPos(int number)
    {
        bettingList[number] = 0;

        if(!bettingList.Contains(1))
        {
            blockType = BlockType.Default;
        }

        if(!allIn)
        {
            bettingMoney -= bettingValue[number];
        }
        else
        {
            bettingMoney -= money;
        }

        if (bettingMoney < 0)
        {
            bettingMoney = 0;
        }

        ChangeBettingMoney();
    }

    public bool CheckBetting_Ai(BlockClass block, int number, RouletteType rouletteType) //Ai가 배팅을 했는지?
    {
        bool check = false;

        RouletteContent rouletteContent = new RouletteContent();

        switch (rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                rouletteContent = rouletteContentList_Target[number];

                break;
            case RouletteType.SplitBet_Horizontal:
                rouletteContent = rouletteContentList_Split_Horizontal[number];

                break;
            case RouletteType.SplitBet_Vertical:
                rouletteContent = rouletteContentList_Split_Vertical[number];

                break;
            case RouletteType.SquareBet:
                rouletteContent = rouletteContentList_Square[number];

                break;
        }

        index0_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex0(block.blockType, 0);
        index0_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex0(block.blockType, 1);

        index1_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex1(block.blockType, 0);
        index1_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex1(block.blockType, 1);

        index2_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex2(block.blockType, 0);
        index2_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex2(block.blockType, 1);

        index3_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex3(block.blockType, 0);
        index3_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex3(block.blockType, 1);

        index4_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex4(block.blockType, 0);
        index4_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex4(block.blockType, 1);

        index5_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex5(block.blockType, 0);
        index5_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex5(block.blockType, 1);

        index6_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex6(block.blockType, 0);
        index6_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex6(block.blockType, 1);

        index7_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex7(block.blockType, 0);
        index7_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex7(block.blockType, 1);

        index8_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex8(block.blockType, 0);
        index8_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex8(block.blockType, 1);

        switch (rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                if (index0_Ai[0] < 0 || index0_Ai[1] < 0 || index1_Ai[0] < 0 || index1_Ai[1] < 0 || index2_Ai[0] < 0 || index2_Ai[1] < 0 || index3_Ai[0] < 0 || index3_Ai[1] < 0
                    || index4_Ai[0] < 0 || index4_Ai[1] < 0 || index5_Ai[0] < 0 || index5_Ai[1] < 0 || index6_Ai[0] < 0 || index6_Ai[1] < 0 || index7_Ai[0] < 0 || index7_Ai[1] < 0
                    || index8_Ai[0] < 0 || index8_Ai[1] < 0
    || index0_Ai[0] >= gridConstraintCount || index0_Ai[1] >= gridConstraintCount || index1_Ai[0] >= gridConstraintCount || index1_Ai[1] >= gridConstraintCount
    || index2_Ai[0] >= gridConstraintCount || index2_Ai[1] >= gridConstraintCount || index3_Ai[0] >= gridConstraintCount || index3_Ai[1] >= gridConstraintCount
    || index4_Ai[0] >= gridConstraintCount || index4_Ai[1] >= gridConstraintCount || index5_Ai[0] >= gridConstraintCount || index5_Ai[1] >= gridConstraintCount
    || index6_Ai[0] >= gridConstraintCount || index6_Ai[1] >= gridConstraintCount || index7_Ai[0] >= gridConstraintCount || index7_Ai[1] >= gridConstraintCount
    || index8_Ai[0] >= gridConstraintCount || index8_Ai[1] >= gridConstraintCount)
                {
                    check = false;
                }
                else
                {
                    check = true;
                    break;
                }
                break;
            case RouletteType.SplitBet_Horizontal:
                if (index0_Ai[0] < 0 || index0_Ai[1] < 0 || index1_Ai[0] < 0 || index1_Ai[1] < 0 || index2_Ai[0] < 0 || index2_Ai[1] < 0 || index3_Ai[0] < 0 || index3_Ai[1] < 0
                    || index4_Ai[0] < 0 || index4_Ai[1] < 0 || index5_Ai[0] < 0 || index5_Ai[1] < 0 || index6_Ai[0] < 0 || index6_Ai[1] < 0 || index7_Ai[0] < 0 || index7_Ai[1] < 0
                    || index8_Ai[0] < 0 || index8_Ai[1] < 0
|| index0_Ai[0] >= gridConstraintCount || index0_Ai[1] >= gridConstraintCount - 1 || index1_Ai[0] >= gridConstraintCount || index1_Ai[1] >= gridConstraintCount - 1
|| index2_Ai[0] >= gridConstraintCount || index2_Ai[1] >= gridConstraintCount - 1 || index3_Ai[0] >= gridConstraintCount || index3_Ai[1] >= gridConstraintCount - 1
|| index4_Ai[0] >= gridConstraintCount || index4_Ai[1] >= gridConstraintCount - 1 || index5_Ai[0] >= gridConstraintCount || index5_Ai[1] >= gridConstraintCount - 1
|| index6_Ai[0] >= gridConstraintCount || index6_Ai[1] >= gridConstraintCount - 1 || index7_Ai[0] >= gridConstraintCount || index7_Ai[1] >= gridConstraintCount - 1
|| index8_Ai[0] >= gridConstraintCount || index8_Ai[1] >= gridConstraintCount - 1)
                {
                    check = false;
                }
                else
                {
                    check = true;
                    break;
                }
                break;
            case RouletteType.SplitBet_Vertical:
                if (index0_Ai[0] < 0 || index0_Ai[1] < 0 || index1_Ai[0] < 0 || index1_Ai[1] < 0 || index2_Ai[0] < 0 || index2_Ai[1] < 0 || index3_Ai[0] < 0 || index3_Ai[1] < 0
                    || index4_Ai[0] < 0 || index4_Ai[1] < 0 || index5_Ai[0] < 0 || index5_Ai[1] < 0 || index6_Ai[0] < 0 || index6_Ai[1] < 0 || index7_Ai[0] < 0 || index7_Ai[1] < 0
                    || index8_Ai[0] < 0 || index8_Ai[1] < 0
|| index0_Ai[0] >= gridConstraintCount - 1 || index0_Ai[1] >= gridConstraintCount || index1_Ai[0] >= gridConstraintCount - 1 || index1_Ai[1] >= gridConstraintCount
|| index2_Ai[0] >= gridConstraintCount - 1 || index2_Ai[1] >= gridConstraintCount || index3_Ai[0] >= gridConstraintCount - 1 || index3_Ai[1] >= gridConstraintCount
|| index4_Ai[0] >= gridConstraintCount - 1 || index4_Ai[1] >= gridConstraintCount || index5_Ai[0] >= gridConstraintCount - 1 || index5_Ai[1] >= gridConstraintCount
|| index6_Ai[0] >= gridConstraintCount - 1 || index6_Ai[1] >= gridConstraintCount || index7_Ai[0] >= gridConstraintCount - 1 || index7_Ai[1] >= gridConstraintCount
|| index8_Ai[0] >= gridConstraintCount - 1 || index8_Ai[1] >= gridConstraintCount)
                {
                    check = false;
                }
                else
                {
                    check = true;
                    break;
                }
                break;
            case RouletteType.SquareBet:
                if (index0_Ai[0] < 0 || index0_Ai[1] < 0 || index1_Ai[0] < 0 || index1_Ai[1] < 0 || index2_Ai[0] < 0 || index2_Ai[1] < 0 || index3_Ai[0] < 0 || index3_Ai[1] < 0
                    || index4_Ai[0] < 0 || index4_Ai[1] < 0 || index5_Ai[0] < 0 || index5_Ai[1] < 0 || index6_Ai[0] < 0 || index6_Ai[1] < 0 || index7_Ai[0] < 0 || index7_Ai[1] < 0
                    || index8_Ai[0] < 0 || index8_Ai[1] < 0
    || index0_Ai[0] >= gridConstraintCount - 1 || index0_Ai[1] >= gridConstraintCount - 1 || index1_Ai[0] >= gridConstraintCount - 1 || index1_Ai[1] >= gridConstraintCount - 1
    || index2_Ai[0] >= gridConstraintCount - 1 || index2_Ai[1] >= gridConstraintCount - 1 || index3_Ai[0] >= gridConstraintCount - 1 || index3_Ai[1] >= gridConstraintCount - 1
    || index4_Ai[0] >= gridConstraintCount - 1 || index4_Ai[1] >= gridConstraintCount - 1 || index5_Ai[0] >= gridConstraintCount - 1 || index5_Ai[1] >= gridConstraintCount - 1
    || index6_Ai[0] >= gridConstraintCount - 1 || index6_Ai[1] >= gridConstraintCount - 1 || index7_Ai[0] >= gridConstraintCount - 1 || index7_Ai[1] >= gridConstraintCount - 1
    || index8_Ai[0] >= gridConstraintCount - 1 || index8_Ai[1] >= gridConstraintCount - 1)
                {
                    check = false;
                }
                else
                {
                    check = true;
                    break;
                }
                break;
        }

        return check;
    }

    public void SetBettingNumber_Ai(BlockClass block, int number, RouletteType rouletteType) //Ai가 배팅한 숫자 저장
    {
        if (otherMoney - upgradeDataBase.GetUpgradeValue(block.rankType).GetValueNumber(block.level) < 0)
        {
            Debug.Log("Ai가 돈이 부족하여 항복하였습니다.");

            GameStateManager.instance.Playing = false;
            GameEnd(2);

            return;
        }

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            for(int i = 0; i < rouletteContentList_Target.Count; i ++)
            {
                rouletteContentList_Target[i].SetActiveFalseAi();
            }

            for (int i = 0; i < blockLevelContentList_Target.Count; i++)
            {
                blockLevelContentList_Target[i].Initialize_Other();
            }
        }
        else
        {
            for (int i = 0; i < allContentList.Count; i++)
            {
                allContentList[i].SetActiveFalseAi();
            }

            for (int i = 0; i < allBlockLevelContentList.Count; i++)
            {
                allBlockLevelContentList[i].Initialize_Other();
            }
        }

        RouletteContent rouletteContent = new RouletteContent();

        switch (rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                rouletteContent = rouletteContentList_Target[number];

                break;
            case RouletteType.SplitBet_Horizontal:
                rouletteContent = rouletteContentList_Split_Horizontal[number];

                break;
            case RouletteType.SplitBet_Vertical:
                rouletteContent = rouletteContentList_Split_Vertical[number];

                break;
            case RouletteType.SquareBet:
                rouletteContent = rouletteContentList_Square[number];

                break;
        }

        index0_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex0(block.blockType, 0);
        index0_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex0(block.blockType, 1);

        index1_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex1(block.blockType, 0);
        index1_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex1(block.blockType, 1);

        index2_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex2(block.blockType, 0);
        index2_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex2(block.blockType, 1);

        index3_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex3(block.blockType, 0);
        index3_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex3(block.blockType, 1);

        index4_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex4(block.blockType, 0);
        index4_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex4(block.blockType, 1);

        index5_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex5(block.blockType, 0);
        index5_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex5(block.blockType, 1);

        index6_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex6(block.blockType, 0);
        index6_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex6(block.blockType, 1);

        index7_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex7(block.blockType, 0);
        index7_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex7(block.blockType, 1);

        index8_Ai[0] = rouletteContent.index[0] + blockDataBase.GetIndex8(block.blockType, 0);
        index8_Ai[1] = rouletteContent.index[1] + blockDataBase.GetIndex8(block.blockType, 1);


        switch (rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                for (int i = 0; i < rouletteContentList_Target.Count; i++)
                {
                    if (rouletteContentList_Target[i].index.SequenceEqual(index0_Ai) || rouletteContentList_Target[i].index.SequenceEqual(index1_Ai)
                        || rouletteContentList_Target[i].index.SequenceEqual(index2_Ai) || rouletteContentList_Target[i].index.SequenceEqual(index3_Ai)
                        || rouletteContentList_Target[i].index.SequenceEqual(index4_Ai) || rouletteContentList_Target[i].index.SequenceEqual(index5_Ai)
                        || rouletteContentList_Target[i].index.SequenceEqual(index6_Ai) || rouletteContentList_Target[i].index.SequenceEqual(index7_Ai)
                        || rouletteContentList_Target[i].index.SequenceEqual(index8_Ai))
                    {
                        rouletteContentList_Target[i].SetAciveTrue_Ai(block);
                        blockLevelContentList_Target[i].SetOtherBlock(block.level);
                    }
                }
                break;
            case RouletteType.SplitBet_Horizontal:
                for (int i = 0; i < rouletteContentList_Split_Horizontal.Count; i++)
                {
                    if (rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index0_Ai) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index1_Ai)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index2_Ai) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index3_Ai)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index4_Ai) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index5_Ai)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index6_Ai) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index7_Ai)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index8_Ai))
                    {
                        rouletteContentList_Split_Horizontal[i].SetAciveTrue_Ai(block);
                        blockLevelContentList_Split_Horizontal[i].SetOtherBlock(block.level);
                    }
                }
                break;
            case RouletteType.SplitBet_Vertical:
                for (int i = 0; i < rouletteContentList_Split_Vertical.Count; i++)
                {
                    if (rouletteContentList_Split_Vertical[i].index.SequenceEqual(index0_Ai) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index1_Ai)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index2_Ai) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index3_Ai)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index4_Ai) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index5_Ai)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index6_Ai) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index7_Ai)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index8_Ai))
                    {
                        rouletteContentList_Split_Vertical[i].SetAciveTrue_Ai(block);
                        blockLevelContentList_Split_Vertical[i].SetOtherBlock(block.level);
                    }
                }
                break;
            case RouletteType.SquareBet:
                for (int i = 0; i < rouletteContentList_Square.Count; i++)
                {
                    if (rouletteContentList_Square[i].index.SequenceEqual(index0_Ai) || rouletteContentList_Square[i].index.SequenceEqual(index1_Ai)
                        || rouletteContentList_Square[i].index.SequenceEqual(index2_Ai) || rouletteContentList_Square[i].index.SequenceEqual(index3_Ai)
                        || rouletteContentList_Square[i].index.SequenceEqual(index4_Ai) || rouletteContentList_Square[i].index.SequenceEqual(index5_Ai)
                        || rouletteContentList_Square[i].index.SequenceEqual(index6_Ai) || rouletteContentList_Square[i].index.SequenceEqual(index7_Ai)
                        || rouletteContentList_Square[i].index.SequenceEqual(index8_Ai))
                    {
                        rouletteContentList_Square[i].SetAciveTrue_Ai(block);
                        blockLevelContentList_Square[i].SetOtherBlock(block.level);
                    }
                }
                break;
        }

        ShowBettingNumber_Ai();
    }

    public void ResetRouletteBackgroundColor()
    {
        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < rouletteContentList_Target.Count; i++)
            {
                rouletteContentList_Target[i].ResetBackgroundColor();
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

    public void CancelBetting(BlockType type) //배팅 취소
    {
        ResetRouletteBackgroundColor();

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < numberContentList_NewBie.Count; i++)
            {
                numberContentList_NewBie[i].Betting_My_Initialize();
            }

            for (int i = 0; i < bettingNumberList.Count; i++)
            {
                if (bettingNumberList[i] > 0)
                {
                    numberContentList_NewBie[bettingNumberList[i] - 1].Betting();
                }
            }
        }
        else
        {
            for (int i = 0; i < numberContentList.Count; i++)
            {
                numberContentList[i].Betting_My_Initialize();
            }

            for (int i = 0; i < bettingNumberList.Count; i++)
            {
                if (bettingNumberList[i] > 0)
                {
                    numberContentList[bettingNumberList[i] - 1].Betting();
                }
            }
        }


        if (blockType.Equals(type))
        {
            deleteBlock = new string[2];

            deleteBlock[0] = type.ToString();
            deleteBlock[1] = GameStateManager.instance.NickName;

            PV.RPC("HideOtherPlayerBlock", RpcTarget.Others, deleteBlock);

            Debug.Log("상대방한테 필드에 배치한 내 블럭을 지워달라고 요청했습니다");
        }
    }

    public void BetOptionCancleButton() //배팅 취소 버튼 누르기
    {
        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < rouletteContentList_Target.Count; i++)
            {
                rouletteContentList_Target[i].SetActiveFalse();
            }

            if (newbieBlockContent.gameObject.activeInHierarchy)
            {
                newbieBlockContent.ResetPos();
            }
        }
        else
        {
            for (int i = 0; i < allContentList.Count; i++)
            {
                allContentList[i].SetActiveFalse();
            }

            for (int i = 0; i < blockContentList.Count; i++)
            {
                if (blockContentList[i].gameObject.activeInHierarchy)
                {
                    blockContentList[i].ResetPos();
                }
            }
        }

        for (int i = 0; i < bettingList.Length; i++)
        {
            bettingList[i] = 0;
        }

        ResetBettingMoney();

        //NotionManager.instance.UseNotion(NotionType.Cancle);
    }


    #region ShowBetting
    void ShowBettingNumber_Total() //나, 상대방 배팅한 위치 값 정리
    {
        bettingNumberList.Clear();
        otherBettingNumberList.Clear();

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < rouletteContentList_Target.Count; i++)
            {
                if (rouletteContentList_Target[i].isActive)
                {
                    switch (rouletteContentList_Target[i].rouletteType)
                    {
                        case RouletteType.StraightBet:
                            bettingNumberList.Add(rouletteContentList_Target[i].number);
                            break;
                        default:
                            for (int j = 0; j < rouletteContentList_Target[i].numberList.Length; j++)
                            {
                                bettingNumberList.Add(rouletteContentList_Target[i].numberList[j]);
                            }
                            break;
                    }
                }
            }

            if (aiMode)
            {
                for (int i = 0; i < rouletteContentList_Target.Count; i++)
                {
                    if (rouletteContentList_Target[i].isActive_Ai)
                    {
                        switch (rouletteContentList_Target[i].rouletteType)
                        {
                            case RouletteType.StraightBet:
                                otherBettingNumberList.Add(rouletteContentList_Target[i].number);
                                break;
                            default:
                                for (int j = 0; j < rouletteContentList_Target[i].numberList.Length; j++)
                                {
                                    otherBettingNumberList.Add(rouletteContentList_Target[i].numberList[j]);
                                }
                                break;
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < allContentList.Count; i++)
            {
                if (allContentList[i].isActive)
                {
                    switch (allContentList[i].rouletteType)
                    {
                        case RouletteType.StraightBet:
                            bettingNumberList.Add(allContentList[i].number);
                            break;
                        default:
                            for (int j = 0; j < allContentList[i].numberList.Length; j++)
                            {
                                bettingNumberList.Add(allContentList[i].numberList[j]);
                            }
                            break;
                    }
                }
            }

            if (aiMode)
            {
                for (int i = 0; i < allContentList.Count; i++)
                {
                    if (allContentList[i].isActive_Ai)
                    {
                        switch (allContentList[i].rouletteType)
                        {
                            case RouletteType.StraightBet:
                                otherBettingNumberList.Add(allContentList[i].number);
                                break;
                            default:
                                for (int j = 0; j < allContentList[i].numberList.Length; j++)
                                {
                                    otherBettingNumberList.Add(allContentList[i].numberList[j]);
                                }
                                break;
                        }
                    }
                }
            }
        }

        bettingNumberList = bettingNumberList.Distinct().ToList();
        bettingNumberList.Sort();

        otherBettingNumberList = otherBettingNumberList.Distinct().ToList();
        otherBettingNumberList.Sort();

        if (!aiMode)
        {
            otherBettingList = "";

            for (int i = 0; i < bettingNumberList.Count; i++)
            {
                otherBettingList += bettingNumberList[i].ToString() + "/";
            }

            if (otherBettingList.Length > 0)
            {
                PV.RPC("ShowOtherBetting", RpcTarget.Others, otherBettingList.TrimEnd('/'));
            }
        }
    }

    void ShowBettingNumber_My() //내가 어느 숫자에 배치했는지 정리
    {
        bettingNumberList.Clear();

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < rouletteContentList_Target.Count; i++)
            {
                if (rouletteContentList_Target[i].isActive)
                {
                    switch (rouletteContentList_Target[i].rouletteType)
                    {
                        case RouletteType.StraightBet:
                            bettingNumberList.Add(rouletteContentList_Target[i].number);
                            break;
                        default:
                            for (int j = 0; j < rouletteContentList_Target[i].numberList.Length; j++)
                            {
                                bettingNumberList.Add(rouletteContentList_Target[i].numberList[j]);
                            }
                            break;
                    }
                }
            }

            for (int i = 0; i < numberContentList_NewBie.Count; i++)
            {
                numberContentList_NewBie[i].Betting_My_Initialize();
            }

            for (int i = 0; i < bettingNumberList.Count; i++)
            {
                if (bettingNumberList[i] > 0)
                {
                    numberContentList_NewBie[bettingNumberList[i] - 1].Betting();
                }
            }
        }
        else
        {
            for (int i = 0; i < allContentList.Count; i++)
            {
                if (allContentList[i].isActive)
                {
                    switch (allContentList[i].rouletteType)
                    {
                        case RouletteType.StraightBet:
                            bettingNumberList.Add(allContentList[i].number);
                            break;
                        default:
                            for (int j = 0; j < allContentList[i].numberList.Length; j++)
                            {
                                bettingNumberList.Add(allContentList[i].numberList[j]);
                            }
                            break;
                    }
                }
            }

            for (int i = 0; i < numberContentList.Count; i++)
            {
                numberContentList[i].Betting_My_Initialize();
            }

            for (int i = 0; i < bettingNumberList.Count; i++)
            {
                if (bettingNumberList[i] > 0)
                {
                    numberContentList[bettingNumberList[i] - 1].Betting();
                }
            }
        }

        if (!aiMode)
        {
            otherBettingList = "";

            for (int i = 0; i < bettingNumberList.Count; i++)
            {
                otherBettingList += bettingNumberList[i].ToString() + "/";
            }

            if (otherBettingList.Length > 0)
            {
                PV.RPC("ShowOtherBetting", RpcTarget.Others, otherBettingList.TrimEnd('/'));
            }
        }
    }

    void ShowBettingNumber_Ai() //Ai가 어느 숫자에 배치했는지 정리
    {
        otherBettingNumberList.Clear();

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            if (aiMode)
            {
                for (int i = 0; i < rouletteContentList_Target.Count; i++)
                {
                    if (rouletteContentList_Target[i].isActive_Ai)
                    {
                        switch (rouletteContentList_Target[i].rouletteType)
                        {
                            case RouletteType.StraightBet:
                                otherBettingNumberList.Add(rouletteContentList_Target[i].number);
                                break;
                            default:
                                for (int j = 0; j < rouletteContentList_Target[i].numberList.Length; j++)
                                {
                                    otherBettingNumberList.Add(rouletteContentList_Target[i].numberList[j]);
                                }
                                break;
                        }
                    }
                }
            }

            for (int i = 0; i < numberContentList_NewBie.Count; i++)
            {
                numberContentList_NewBie[i].Betting_Other_Initialize();
            }

            for (int i = 0; i < otherBettingNumberList.Count; i++)
            {
                if (otherBettingNumberList[i] > 0)
                {
                    numberContentList_NewBie[otherBettingNumberList[i] - 1].Betting_Other();
                }
            }
        }
        else
        {
            if (aiMode)
            {
                for (int i = 0; i < allContentList.Count; i++)
                {
                    if (allContentList[i].isActive_Ai)
                    {
                        switch (allContentList[i].rouletteType)
                        {
                            case RouletteType.StraightBet:
                                otherBettingNumberList.Add(allContentList[i].number);
                                break;
                            default:
                                for (int j = 0; j < allContentList[i].numberList.Length; j++)
                                {
                                    otherBettingNumberList.Add(allContentList[i].numberList[j]);
                                }
                                break;
                        }
                    }
                }
            }

            for (int i = 0; i < numberContentList.Count; i++)
            {
                numberContentList[i].Betting_Other_Initialize();
            }

            for (int i = 0; i < otherBettingNumberList.Count; i++)
            {
                if (otherBettingNumberList[i] > 0)
                {
                    numberContentList[otherBettingNumberList[i] - 1].Betting_Other();
                }
            }
        }
    }

    [PunRPC]
    void ShowOtherPlayerBlock(string[] block) //상대방 블럭 배치
    {
        RouletteType rouletteType = (RouletteType)System.Enum.Parse(typeof(RouletteType), block[0]);
        BlockType blockType = (BlockType)System.Enum.Parse(typeof(BlockType), block[1]);

        otherBlockType = blockType;

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

        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < blockLevelContentList_Target.Count; i++)
            {
                blockLevelContentList_Target[i].Initialize_Other();
            }
        }
        else
        {
            for (int i = 0; i < allBlockLevelContentList.Count; i++)
            {
                allBlockLevelContentList[i].Initialize_Other();
            }
        }

        RouletteContent rouletteContent = new RouletteContent();

        switch (rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                content.transform.position = rouletteContentList_Target[int.Parse(block[2]) - 1].transform.position;

                rouletteContent = rouletteContentList_Target[int.Parse(block[2]) - 1];

                break;
            case RouletteType.SplitBet_Horizontal:
                content.transform.position = rouletteContentList_Split_Horizontal[int.Parse(block[2]) - 1].transform.position;

                rouletteContent = rouletteContentList_Split_Horizontal[int.Parse(block[2]) - 1];

                break;
            case RouletteType.SplitBet_Vertical:
                content.transform.position = rouletteContentList_Split_Vertical[int.Parse(block[2]) - 1].transform.position;

                rouletteContent = rouletteContentList_Split_Vertical[int.Parse(block[2]) - 1];

                break;
            case RouletteType.SquareBet:
                content.transform.position = rouletteContentList_Square[int.Parse(block[2]) - 1].transform.position;

                rouletteContent = rouletteContentList_Square[int.Parse(block[2]) - 1];

                break;
        }

        index0_Enemy[0] = rouletteContent.index[0] + blockDataBase.GetIndex0(blockType, 0);
        index0_Enemy[1] = rouletteContent.index[1] + blockDataBase.GetIndex0(blockType, 1);

        index1_Enemy[0] = rouletteContent.index[0] + blockDataBase.GetIndex1(blockType, 0);
        index1_Enemy[1] = rouletteContent.index[1] + blockDataBase.GetIndex1(blockType, 1);

        index2_Enemy[0] = rouletteContent.index[0] + blockDataBase.GetIndex2(blockType, 0);
        index2_Enemy[1] = rouletteContent.index[1] + blockDataBase.GetIndex2(blockType, 1);

        index3_Enemy[0] = rouletteContent.index[0] + blockDataBase.GetIndex3(blockType, 0);
        index3_Enemy[1] = rouletteContent.index[1] + blockDataBase.GetIndex3(blockType, 1);

        index4_Enemy[0] = rouletteContent.index[0] + blockDataBase.GetIndex4(blockType, 0);
        index4_Enemy[1] = rouletteContent.index[1] + blockDataBase.GetIndex4(blockType, 1);

        index5_Enemy[0] = rouletteContent.index[0] + blockDataBase.GetIndex5(blockType, 0);
        index5_Enemy[1] = rouletteContent.index[1] + blockDataBase.GetIndex5(blockType, 1);

        index6_Enemy[0] = rouletteContent.index[0] + blockDataBase.GetIndex6(blockType, 0);
        index6_Enemy[1] = rouletteContent.index[1] + blockDataBase.GetIndex6(blockType, 1);

        index7_Enemy[0] = rouletteContent.index[0] + blockDataBase.GetIndex7(blockType, 0);
        index7_Enemy[1] = rouletteContent.index[1] + blockDataBase.GetIndex7(blockType, 1);

        index8_Enemy[0] = rouletteContent.index[0] + blockDataBase.GetIndex8(blockType, 0);
        index8_Enemy[1] = rouletteContent.index[1] + blockDataBase.GetIndex8(blockType, 1);

        switch (rouletteContent.rouletteType)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                for (int i = 0; i < rouletteContentList_Target.Count; i++)
                {
                    if (rouletteContentList_Target[i].index.SequenceEqual(index0_Enemy) || rouletteContentList_Target[i].index.SequenceEqual(index1_Enemy)
                        || rouletteContentList_Target[i].index.SequenceEqual(index2_Enemy) || rouletteContentList_Target[i].index.SequenceEqual(index3_Enemy)
                        || rouletteContentList_Target[i].index.SequenceEqual(index4_Enemy) || rouletteContentList_Target[i].index.SequenceEqual(index5_Enemy)
                        || rouletteContentList_Target[i].index.SequenceEqual(index6_Enemy) || rouletteContentList_Target[i].index.SequenceEqual(index7_Enemy)
                        || rouletteContentList_Target[i].index.SequenceEqual(index8_Enemy))
                    {
                        blockLevelContentList_Target[i].SetOtherBlock(int.Parse(block[5]));
                    }
                }
                break;
            case RouletteType.SplitBet_Horizontal:
                for (int i = 0; i < rouletteContentList_Split_Horizontal.Count; i++)
                {
                    if (rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index0_Enemy) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index1_Enemy)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index2_Enemy) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index3_Enemy)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index4_Enemy) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index5_Enemy)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index6_Enemy) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index7_Enemy)
                        || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index8_Enemy))
                    {
                        blockLevelContentList_Split_Horizontal[i].SetOtherBlock(int.Parse(block[5]));
                    }
                }
                break;
            case RouletteType.SplitBet_Vertical:
                for (int i = 0; i < rouletteContentList_Split_Vertical.Count; i++)
                {
                    if (rouletteContentList_Split_Vertical[i].index.SequenceEqual(index0_Enemy) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index1_Enemy)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index2_Enemy) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index3_Enemy)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index4_Enemy) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index5_Enemy)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index6_Enemy) || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index7_Enemy)
                        || rouletteContentList_Split_Vertical[i].index.SequenceEqual(index8_Enemy))
                    {
                        blockLevelContentList_Split_Vertical[i].SetOtherBlock(int.Parse(block[5]));
                    }
                }
                break;
            case RouletteType.SquareBet:
                for (int i = 0; i < rouletteContentList_Square.Count; i++)
                {
                    if (rouletteContentList_Square[i].index.SequenceEqual(index0_Enemy) || rouletteContentList_Square[i].index.SequenceEqual(index1_Enemy)
                        || rouletteContentList_Square[i].index.SequenceEqual(index2_Enemy) || rouletteContentList_Square[i].index.SequenceEqual(index3_Enemy)
                        || rouletteContentList_Square[i].index.SequenceEqual(index4_Enemy) || rouletteContentList_Square[i].index.SequenceEqual(index5_Enemy)
                        || rouletteContentList_Square[i].index.SequenceEqual(index6_Enemy) || rouletteContentList_Square[i].index.SequenceEqual(index7_Enemy)
                        || rouletteContentList_Square[i].index.SequenceEqual(index8_Enemy))
                    {
                        blockLevelContentList_Square[i].SetOtherBlock(int.Parse(block[5]));
                    }
                }
                break;
        }
    }

    [PunRPC]
    void HideOtherPlayerBlock(string[] block) //상대방이 블럭을 취소했습니다
    {
        BlockType blockType = (BlockType)System.Enum.Parse(typeof(BlockType), block[0]);

        for (int i = 0; i < otherBlockContentList.Count; i++)
        {
            if (otherBlockContentList[i].nickName.Equals(block[1]) && otherBlockContentList[i].blockType == blockType)
            {
                Destroy(otherBlockContentList[i].gameObject);
                otherBlockContentList.Remove(otherBlockContentList[i]);

                otherBlockType = BlockType.Default;

                if (GameStateManager.instance.GameType == GameType.NewBie)
                {
                    for (int j = 0; j < blockLevelContentList_Target.Count; j++)
                    {
                        blockLevelContentList_Target[j].Initialize_Other();
                    }
                }
                else
                {
                    for (int j = 0; j < allBlockLevelContentList.Count; j++)
                    {
                        allBlockLevelContentList[j].Initialize_Other();
                    }
                }

                break;
            }
        }

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < numberContentList_NewBie.Count; i++)
            {
                numberContentList_NewBie[i].Betting_Other_Initialize();
            }
        }
        else
        {
            for (int i = 0; i < numberContentList.Count; i++)
            {
                numberContentList[i].Betting_Other_Initialize();
            }
        }
    }

    [PunRPC]
    void ShowOtherBetting(string str) //상대방 블럭 위치 값 받기
    {
        Debug.Log("상대방 배팅 위치 값 : " + str);

        otherBettingNumberList.Clear();

        string[] list = str.Split("/");

        for(int i = 0; i < list.Length; i ++)
        {
            otherBettingNumberList.Add(int.Parse(list[i]));
            list[i].Replace("/", "");
        }

        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            for(int i = 0; i < numberContentList_NewBie.Count; i ++)
            {
                numberContentList_NewBie[i].Betting_Other_Initialize();
            }

            for (int i = 0; i < otherBettingNumberList.Count; i++)
            {
                if (otherBettingNumberList[i] > 0)
                {
                    numberContentList_NewBie[otherBettingNumberList[i] - 1].Betting_Other();
                }
            }
        }
        else
        {
            for (int i = 0; i < numberContentList.Count; i++)
            {
                numberContentList[i].Betting_Other_Initialize();
            }

            for (int i = 0; i < otherBettingNumberList.Count; i++)
            {
                if (otherBettingNumberList[i] > 0)
                {
                    numberContentList[otherBettingNumberList[i] - 1].Betting_Other();
                }
            }
        }

        otherBettingNumberList = otherBettingNumberList.Distinct().ToList();
        otherBettingNumberList.Sort();

        Debug.Log("상대방의 배팅 위치 값을 받아서 저장했습니다");
    }

    #endregion

    [PunRPC]
    void ChatRPC(string msg) //전체 메세지
    {
        RecordManager.instance.SetGameRecord(msg);
    }


    #region InGame

    public void UpdateMoney()
    {
        myNumber = 0;

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
        }

        Debug.Log("현재 돈 서버에 업데이트");
    }

    public void LoadMoney()
    {
        myNumber = 0;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName.Equals(GameStateManager.instance.NickName))
            {
                myNumber = i;
            }
        }

        ht = PhotonNetwork.CurrentRoom.CustomProperties;

        switch (myNumber)
        {
            case 0:
                money = int.Parse(ht["Player1_Total"].ToString());
                otherMoney = int.Parse(ht["Player2_Total"].ToString());
                break;
            case 1:
                money = int.Parse(ht["Player2_Total"].ToString());
                otherMoney = int.Parse(ht["Player1_Total"].ToString());
                break;
        }

        moneyText.text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
        otherMoneyText.text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";

        Debug.Log("저장된 돈을 불러옵니다");
    }

    //public void SetMinusMoney(int number) //배팅 후 결과값에 따라 잃은 돈 저장하기
    //{
    //    int myNumber = 0;

    //    Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

    //    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //    {
    //        if (PhotonNetwork.PlayerList[i].NickName.Equals(GameStateManager.instance.NickName))
    //        {
    //            myNumber = i;
    //        }
    //    }

    //    switch (myNumber)
    //    {
    //        case 0:
    //            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Minus", int.Parse(ht["Player1_Minus"].ToString()) + number } });
    //            break;
    //        case 1:
    //            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Minus", int.Parse(ht["Player2_Minus"].ToString()) + number } });
    //            break;
    //    }
    //}

    public void SurrenderButton() //기권하기
    {
        uIManager.CloseSurrenderView();

        money = 0;

        Debug.Log("기권");

        GameStateManager.instance.Playing = false;
        GameEnd(1);

        PV.RPC("Surrender", RpcTarget.Others);
    }

    [PunRPC]
    public void Surrender()
    {
        Debug.Log("상대방이 기권");

        GameStateManager.instance.Playing = false;
        GameEnd(2);
    }

    [Button]
    public void Winner()
    {
        Debug.Log("상대방이 방에서 튕겼습니다");

        GameStateManager.instance.Playing = false;
        GameEnd(2);
    }

    public void Lose()
    {
        Debug.Log("튕겨서 재접속 했으나 방이 사라져서 패배 처리되었습니다");

        GameStateManager.instance.Playing = false;
        GameEnd(1);
    }

    public void Draw()
    {
        Debug.Log("튕겨서 재접속 했으나 방이 사라져서 무승부 처리되었습니다");

        GameStateManager.instance.Playing = false;
        GameEnd(3);
    }

    #endregion

    public void OpenDeveloperInput()
    {
        if(!inputTargetNumber.gameObject.activeInHierarchy)
        {
            inputTargetNumber.gameObject.SetActive(true);
        }
        else
        {
            inputTargetNumber.gameObject.SetActive(false);
        }
    }

    public void DeveloperContinue()
    {
        nextButton.SetActive(false);

        StartCoroutine(WaitTimerCoroution());
    }

    public void ChangeFormation()
    {
        if (playerDataBase.Formation == 1)
        {
            playerDataBase.Formation = 2;
        }
        else
        {
            playerDataBase.Formation = 1;
        }

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Formation", playerDataBase.Formation);

        SceneManager.LoadScene("LoginScene");
    }
}
