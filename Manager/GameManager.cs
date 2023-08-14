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

    [Space]
    [Title("Setting")]
    public GridLayoutGroup gridLayoutGroup;

    [Space]
    [Title("Timer")]
    private int timer = 0;
    public Text timerText;
    public Image timerFillAmount;
    private int timerAi = 0;

    public ButtonScaleAnimation timerAnimation;
    private bool isTimesUp = false;

    [Space]
    [Title("Text")]
    public Text roomText;
    public Text targetText;
    public Text recordText;

    [Space]
    [Title("Tip")]
    public GameObject tipObj;
    public Text tipText;

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

    public int turn = 0;
    public bool inGameBurning = false;
    public bool inGameBurning2 = false;

    public GameObject burningObj;
    public Text turnText;

    public int money = 0; //보유 코인
    public int otherMoney = 0; //상대방 보유 코인
    private int stakes = 0; //판돈

    private int bettingMoney = 0; //배치한 블럭 크기
    private float plusMoney = 0; //획득한 돈

    private int bettingAiMoney = 0;
    private float plusAiMoney = 0; //Ai가 획득한 돈
    private bool aiMoveBlock = false; //확률적으로 Ai가 2초 남기고 위치를 바꿉니다

    private int[] compareMoney = new int[2];

    private string otherBettingList = "";

    private int targetNumber = 0;
    private int targetQueenNumber = 0;

    private int gridConstraintCount = 0;

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
    List<RouletteContent> rouletteContentList_Split_Vertical = new List<RouletteContent>();
    List<RouletteContent> rouletteContentList_Split_Horizontal = new List<RouletteContent>();
    List<RouletteContent> rouletteContentList_Square = new List<RouletteContent>();
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
    public List<BlockLevelContent> blockLevelContentList_Target = new List<BlockLevelContent>();


    BlockClass blockClassArmor = new BlockClass();
    BlockClass blockClassWeapon = new BlockClass();
    BlockClass blockClassShield = new BlockClass();
    BlockClass blockClassNewbie = new BlockClass();

    [Space]
    [Title("Manager")]
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

    public PhotonView PV;

    int[] index0, index1, index2, index3, index4, index5, index6, index7, index8 = new int[2];
    int[] index0_Ai, index1_Ai, index2_Ai, index3_Ai, index4_Ai, index5_Ai, index6_Ai, index7_Ai, index8_Ai = new int[2];

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;

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
            numberContentList.Add(numContent);

            BlockLevelContent levelContent = Instantiate(blockLevelContent);
            levelContent.transform.parent = blockLevelContentTransform;
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
            content.transform.parent = rouletteContentTransform_NewBie;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize_NewBie(this, blockParent.transform, RouletteType.StraightBet, setIndex, i);
            rouletteContentList_NewBie.Add(content);

            NumberContent numContent = Instantiate(numberContent);
            numContent.transform.parent = numberContentTransform_NewBie;
            numContent.transform.localPosition = Vector3.zero;
            numContent.transform.localScale = Vector3.one;
            numContent.Initialize_NewBie(i);
            numberContentList_NewBie.Add(numContent);

            BlockLevelContent levelContent = Instantiate(blockLevelContent);
            levelContent.transform.parent = blockLevelContentTransform_NewBie;
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
            content.transform.parent = rouletteContentTransformSplitBet_Vertical;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockParent.transform, RouletteType.SplitBet_Vertical, setIndex, i);
            rouletteContentList_Split_Vertical.Add(content);

            BlockLevelContent levelContent = Instantiate(blockLevelContent);
            levelContent.transform.parent = blockLevelContentTransform_Vertical;
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
            content.transform.parent = rouletteContentTransformSplitBet_Horizontal;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(this, blockParent.transform, RouletteType.SplitBet_Horizontal, setIndex, i);
            rouletteContentList_Split_Horizontal.Add(content);


            BlockLevelContent levelContent = Instantiate(blockLevelContent);
            levelContent.transform.parent = blockLevelContentTransform_Horizontal;
            levelContent.transform.localPosition = Vector3.zero;
            levelContent.transform.localScale = Vector3.one;
            levelContent.Initialize();
            blockLevelContentList_Split_Horizontal.Add(levelContent);

            index++;

            allContentList.Add(content);
            allBlockLevelContentList.Add(levelContent);
        }

        index = 0;
        count = 0;

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

        index0_Ai = new int[2];
        index1_Ai = new int[2];
        index2_Ai = new int[2];
        index3_Ai = new int[2];
        index4_Ai = new int[2];
        index5_Ai = new int[2];
        index6_Ai = new int[2];
        index7_Ai = new int[2];
        index8_Ai = new int[2];
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

        turn = 0;
        turnText.text = LocalizationManager.instance.GetString("Turn") + " : " + turn;

        burningObj.SetActive(false);

        inGameBurning = false;
        inGameBurning2 = false;

        targetObj.SetActive(false);
        targetQueenObj.SetActive(false);

        targetText.text = "-";

        tipObj.SetActive(false);

        timer = bettingWaitTime;
        timerText.text = timer.ToString();
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
                blockContentList[i].TimeOver();
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

    public void GameStart_Newbie()
    {
        //roomText.text = "초보방";

        //developerInfo.text = "0 = 퀸 당첨\n1 ~ 8 = 해당 숫자 당첨\n9 = 보너스 룰렛 실행\n빈칸 = 정상 진행";

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

        limitLevel = rankDataBase.GetLimitLevel(GameStateManager.instance.GameRankType) - 1;
        int myLevel = 0;

        myLevel = blockClassNewbie.level;

        if (myLevel > limitLevel)
        {
            myLevel = limitLevel;
        }

        int value = upgradeDataBase.GetUpgradeValue(blockClassNewbie.rankType).GetValueNumber(myLevel);

        newbieBlockContent.InGame_Initialize(blockClassNewbie, 3, value);
        newbieBlockContent.InGame_SetLevel(myLevel);
        bettingValue[3] = upgradeDataBase.GetUpgradeValue(blockClassNewbie.rankType).GetValueNumber(myLevel);

        bettingSizeList[3] = blockDataBase.GetBlockInfomation(blockClassNewbie.blockType).GetSize();

        GameStart();
    }

    public void GameStart_Gosu()
    {
        //roomText.text = "고수방";

        //developerInfo.text = "0 = 퀸 당첨\n1 ~24 = 해당 숫자 당첨\n25 = 보너스 룰렛 실행\n빈칸 = 정상 진행";

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
        numberContentTransform.gameObject.SetActive(true);

        rouletteContentList_Target = rouletteContentList;
        blockLevelContentList_Target = blockLevelContentList;

        //for (int i = 0; i < 3; i ++)
        //{
        //    blockContentList[i].gameObject.SetActive(true);
        //}

        limitLevel = rankDataBase.GetLimitLevel(GameStateManager.instance.GameRankType) - 1;
        int myLevel = 0;

        if (playerDataBase.Armor != null)
        {
            if(playerDataBase.Armor.Length > 0)
            {
                blockClassArmor = playerDataBase.GetBlockClass(playerDataBase.Armor);

                myLevel = blockClassArmor.level;

                blockContentList[0].gameObject.SetActive(true);

                if (myLevel > limitLevel)
                {
                    myLevel = limitLevel;

                    NotionManager.instance.UseNotion(NotionType.HighLevelLimit);
                }

                int value = upgradeDataBase.GetUpgradeValue(blockClassArmor.rankType).GetValueNumber(myLevel);

                blockContentList[0].InGame_Initialize(blockClassArmor, 0, value);
                blockContentList[0].InGame_SetLevel(myLevel);
                bettingValue[0] = upgradeDataBase.GetUpgradeValue(blockClassArmor.rankType).GetValueNumber(myLevel);
                bettingSizeList[0] = blockDataBase.GetBlockInfomation(blockClassArmor.blockType).GetSize();

                //Debug.Log("아머를 장착했습니다");
            }
            else
            {
                //Debug.Log("아머가 장착되지 않았습니다");
            }
        }

        if(playerDataBase.Weapon != null)
        {
            if (playerDataBase.Weapon.Length > 0)
            {
                blockClassWeapon = playerDataBase.GetBlockClass(playerDataBase.Weapon);

                myLevel = blockClassWeapon.level;

                blockContentList[1].gameObject.SetActive(true);

                if (myLevel > limitLevel)
                {
                    myLevel = limitLevel;

                    NotionManager.instance.UseNotion(NotionType.HighLevelLimit);
                }

                int value2 = upgradeDataBase.GetUpgradeValue(blockClassWeapon.rankType).GetValueNumber(myLevel);

                blockContentList[1].InGame_Initialize(blockClassWeapon, 1, value2);
                blockContentList[1].InGame_SetLevel(myLevel);
                bettingValue[1] = upgradeDataBase.GetUpgradeValue(blockClassWeapon.rankType).GetValueNumber(myLevel);
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

                myLevel = blockClassShield.level;

                blockContentList[2].gameObject.SetActive(true);

                if (myLevel > limitLevel)
                {
                    myLevel = limitLevel;

                    NotionManager.instance.UseNotion(NotionType.HighLevelLimit);
                }

                int value3 = upgradeDataBase.GetUpgradeValue(blockClassShield.rankType).GetValueNumber(myLevel);

                blockContentList[2].InGame_Initialize(blockClassShield, 2, value3);
                blockContentList[2].InGame_SetLevel(myLevel);
                bettingValue[2] = upgradeDataBase.GetUpgradeValue(blockClassShield.rankType).GetValueNumber(myLevel);
                bettingSizeList[2] = blockDataBase.GetBlockInfomation(blockClassShield.blockType).GetSize();

                //Debug.Log("쉴드를 장착했습니다");
            }
            else
            {
                //Debug.Log("쉴드가 장착되지 않았습니다");
            }
        }

        GameStart();
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

    void SetStakes() //판돈 설정
    {
        stakes = GameStateManager.instance.Stakes;

        money = stakes;
        otherMoney = stakes;

        moneyText.text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
        otherMoneyText.text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";
    }

    public void GameStart()
    {
        GameReset();

        ResetBettingMoney();

        SetStakes();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Ready" } });

            rouletteManager.CreateObj();
            StartCoroutine(WaitTimerCoroution());
        }

        if(GameStateManager.instance.ReEnter)
        {
            Invoke("CheckPlayerState", 1.0f);

            GameStateManager.instance.ReEnter = false;
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

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

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
            //PV.RPC("GameEnd", RpcTarget.Others, 3);
        }
        else if(money <= 0) //내돈이 다 떨어졌을 때
        {
            GameStateManager.instance.Playing = false;
            PV.RPC("SetGameEnd", RpcTarget.Others);

            GameEnd(1);
            //PV.RPC("GameEnd", RpcTarget.Others, 0);
        }
        else if(otherMoney <= 0) //상대방 돈이 다 떨어졌을때
        {
            GameStateManager.instance.Playing = false;
            PV.RPC("SetGameEnd", RpcTarget.Others);

            GameEnd(0);
            //PV.RPC("GameEnd", RpcTarget.Others, 1);
        }
        else
        {
            Debug.Log("승패가 나지 않았습니다.");
        }
    }

    public void CheckGameState()
    {
        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

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
        GameStateManager.instance.Room = "";

        uIManager.dontTouchObj.SetActive(false);

        if (number == 0)
        {
            if (money < GameStateManager.instance.Stakes)
            {
                money = GameStateManager.instance.Stakes + (int)(GameStateManager.instance.Stakes * 0.1f);

                Debug.Log("리타이어 승리");
            }
            else
            {
                Debug.Log("승리");
            }

        }
        else if(number == 1)
        {
            money -= stakes;

            Debug.Log("패배");
        }
        else if (number == 2)
        {
            money = GameStateManager.instance.Stakes + (int)(GameStateManager.instance.Stakes * 0.1f);

            Debug.Log("상대방 항복으로 승리");
        }
        else
        {
            Debug.Log("무승부");
        }

        SoundManager.instance.StopAllSFX();
        SoundManager.instance.StopLoopSFX(GameSfxType.Roulette);

        StopAllCoroutines();
        timerAnimation.StopAnim();

        uIManager.OpenResultView(number, money);

        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
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
            PV.RPC("RestartGame", RpcTarget.All);

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

        if(timer <= 0)
        {
            SoundManager.instance.StopSFX(GameSfxType.TimesUp);
        }
        else
        {
            if (timer <= 5)
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

            for (int i = 0; i < blockLevelContentList_NewBie.Count; i++)
            {
                blockLevelContentList_NewBie[i].Initialize();
            }
        }
        else
        {
            for (int i = 0; i < blockContentList.Count; i++)
            {
                if (blockContentList[i].gameObject.activeInHierarchy) blockContentList[i].ResetPos();
            }

            for (int i = 0; i < allBlockLevelContentList.Count; i++)
            {
                allBlockLevelContentList[i].Initialize();
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
        ClearOtherPlayerBlock();

        moneyAnimation.Initialize();

        if (aiMode)
        {
            aiManager.RestartGame();

            timerAi = Random.Range(5, bettingTime - 2);
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

    [PunRPC]
    void ChangeTurn(int number)
    {
        turn = number;

        if (turn >= 2)
        {
            if (!inGameBurning)
            {
                inGameBurning = true;

                money = money / 2;
                otherMoney = otherMoney / 2;

                moneyAnimation.MinusMoneyAnimationMid(money + money / 2, money / 2, moneyText);
                moneyAnimation.MinusMoneyAnimationMidEnemy(otherMoney + otherMoney / 2, otherMoney / 2, otherMoneyText);

                burningObj.SetActive(true);

                NotionManager.instance.UseNotion(NotionType.InGameBurning);
            }
            else
            {
                NotionManager.instance.UseNotion(NotionType.BettingTimesUp);
            }
        }
        else if (turn >= 11)
        {
            if (!inGameBurning2)
            {
                inGameBurning2 = true;

                money = money / 2;
                otherMoney = otherMoney / 2;

                moneyAnimation.MinusMoneyAnimationMid(money + money / 2, money / 2, moneyText);
                moneyAnimation.MinusMoneyAnimationMidEnemy(otherMoney + otherMoney / 2, otherMoney / 2, otherMoneyText);

                NotionManager.instance.UseNotion(NotionType.InGameBurning2);
            }
            else
            {
                NotionManager.instance.UseNotion(NotionType.BettingTimesUp);
            }
        }
        else
        {
            NotionManager.instance.UseNotion(NotionType.BettingTimesUp);
        }

        turnText.text = LocalizationManager.instance.GetString("Turn") + " : " + turn;

        Debug.Log("현재 턴 : " + turn);

        UpdateMoney();
    }

    void CheckTip()
    {
        switch (GameStateManager.instance.GameType)
        {
            case GameType.NewBie:

                tipObj.SetActive(true);
                tipText.text = LocalizationManager.instance.GetString("Tip_" + (Random.Range(0, 6).ToString()));

                Invoke("CloseTip", 9f);

                break;
            case GameType.Gosu:
                if(GameStateManager.instance.GameRankType < GameRankType.Sliver_1)
                {
                    tipObj.SetActive(true);
                    tipText.text = LocalizationManager.instance.GetString("Tip_" + (Random.Range(6, 13).ToString()));

                    Invoke("CloseTip", 9f);
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

            int random = Random.Range(0, 2);

            if (random == 0) aiMoveBlock = true;
        }

        while (timer > 0)
        {
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
            timerText.text = timer.ToString();

            if (aiMode)
            {
                if (timer <= timerAi)
                {
                    aiManager.PutBlock();
                }

                if(aiMoveBlock)
                {
                    if(timer <= 1)
                    {
                        aiManager.MoveBlock();
                    }
                }
            }

            yield return waitForSeconds;
        }

        SoundManager.instance.StopSFX(GameSfxType.TimesUp);

        yield return new WaitForSeconds(0.5f);

        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("DelayRoulette", RpcTarget.All);
        }

        yield return new WaitForSeconds(1f);

        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("OpenRouletteView", RpcTarget.All);
        }
    }

    [PunRPC]
    void DelayRoulette()
    {
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

            if(GameStateManager.instance.GameType == GameType.NewBie)
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
                    GameStateManager.instance.CheckBouns = true;

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
                    GameStateManager.instance.CheckBouns = true;

                    check = false;
                }
            }
        }

        return check;
    }

    [PunRPC]
    void OpenRouletteView()
    {
        uIManager.dontTouchObj.SetActive(false);

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
                if (blockContentList[i].gameObject.activeInHierarchy) blockContentList[i].TimeOver();
                break;
            }
        }

        timer = bettingWaitTime;
        timerFillAmount.fillAmount = 1;

        UpdateMoney();

        if (PhotonNetwork.IsMasterClient)
        {
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

        int number = (int)plusMoney - compare[1];

        if (number > 0)
        {
            moneyAnimation.AddMoneyAnimation(money, otherMoney, number);

            money += number;
            otherMoney -= number;

            RecordManager.instance.SetRecord(number.ToString());
        }
        else if (number < 0)
        {
            moneyAnimation.MinusMoneyAnimation(money, otherMoney, Mathf.Abs(number));

            money -= Mathf.Abs(number);
            otherMoney += Mathf.Abs(number);

            RecordManager.instance.SetRecord(number.ToString());
        }
        else
        {
            if (bettingMoney > 0)
            {
                moneyAnimation.MinusMoneyAnimationMid(money + bettingMoney, bettingMoney, moneyText);
            }

            if (compare[0] > 0)
            {
                moneyAnimation.MinusMoneyAnimationMidEnemy(otherMoney + compare[0], compare[0], otherMoneyText);
            }

            RecordManager.instance.SetRecord((-bettingMoney).ToString());
        }

        UpdateMoney();
    }

    public void ChangeGetMoney(BlockClass block, RouletteType type, bool queen)
    {
        float value = 0;
        int level = 0;

        level = block.level;

        if (level > limitLevel)
        {
            level = limitLevel;
        }

        if (!allIn)
        {
            value = upgradeDataBase.GetUpgradeValue(block.rankType).GetValueNumber(level) * 1.0f / blockDataBase.GetSize(block.blockType) * 1.0f;
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
        float value = 0;
        value = upgradeDataBase.GetUpgradeValue(block.rankType).GetValueNumber(block.level) * 1.0f / blockDataBase.GetSize(otherBlockType) * 1.0f;

        switch (type)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queenStraightBet * value;
                }
                else
                {
                    plusAiMoney += blockMotherInformation.straightBet * value;
                }

                break;
            case RouletteType.SplitBet_Horizontal:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queensplitBet * (value / 2);
                }
                else
                {
                    plusAiMoney += blockMotherInformation.splitBet * (value / 2);
                }

                break;
            case RouletteType.SplitBet_Vertical:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queensplitBet * (value / 2);
                }
                else
                {
                    plusAiMoney += blockMotherInformation.splitBet * (value / 2);
                }

                break;
            case RouletteType.SquareBet:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queensquareBet * (value / 4);
                }
                else
                {
                    plusAiMoney += blockMotherInformation.squareBet * (value / 4);
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

        targetBlockContent = blockContent.transform;

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            for (int i = 0; i < rouletteContentList_NewBie.Count; i++)
            {
                rouletteContentList_NewBie[i].SetActiveFalse(blockContent.blockClass);
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
                    for (int i = 0; i < rouletteContentList_Split_Horizontal.Count; i++)
                    {
                        if (rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index0) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index1)
                            || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index2) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index3)
                            || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index4) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index5)
                            || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index6) || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index7)
                            || rouletteContentList_Split_Horizontal[i].index.SequenceEqual(index8))
                        {
                            if (rouletteContentList_Split_Horizontal[i].isActive) blockOverlap = true;
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
                            if (rouletteContentList_Split_Vertical[i].isActive) blockOverlap = true;
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
                            if (rouletteContentList_Square[i].isActive) blockOverlap = true;
                        }
                    }
                    break;
            }
        }
    }

    public void ExitBlock(BlockContent blockContent)
    {
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

        if(bettingList.Contains(1) && bettingList[blockContent.index] == 0)
        {
            blockContent.CancleBetting();

            NotionManager.instance.UseNotion(NotionType.OverBettingBlock);

            Debug.Log("1개 이상 배팅할 수 없습니다");

            return;
        }

        if(!allIn)
        {
            if (bettingList[blockContent.index] == 0)
            {
                bettingList[blockContent.index] = 1;
                bettingMinusList[blockContent.index] = blockDataBase.GetBlockInfomation(blockContent.blockClass.blockType).GetSize();
                bettingMoney += bettingValue[blockContent.index];
                ChangeBettingMoney();
            }
        }
        else
        {
            if (bettingList[blockContent.index] == 0)
            {
                bettingList[blockContent.index] = 1;
                bettingMinusList[blockContent.index] = blockDataBase.GetBlockInfomation(blockContent.blockClass.blockType).GetSize();
                bettingMoney += money;
                ChangeBettingMoney();
            }
        }

        string notion = LocalizationManager.instance.GetString("CurrentValue") + " : " + MoneyUnitString.ToCurrencyString(bettingMoney) + " " +
    LocalizationManager.instance.GetString("PowerBetting");

        NotionManager.instance.UseNotion(notion, ColorType.Green);

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
    }


    public void ResetPosBlock(int number)
    {
        blockType = BlockType.Default;

        bettingList[number] = 0;

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

    public void SetBettingNumber_Ai(BlockClass block, int number)
    {
        if (otherMoney - upgradeDataBase.GetUpgradeValue(block.rankType).GetValueNumber(block.level) < 0)
        {
            GameStateManager.instance.Playing = false;

            GameEnd(2);

            Debug.Log("Ai가 돈이 부족하여 항복하였습니다.");
            return;
        }

        for (int i = 0; i < rouletteContentList_Target.Count; i++)
        {
            rouletteContentList_Target[i].isActive_Ai = false;
        }

        for (int i = 0; i < blockLevelContentList_Target.Count; i++)
        {
            blockLevelContentList_Target[i].Initialize_Other();
        }

        index0_Ai[0] = rouletteContentList_Target[number].index[0] + blockDataBase.GetIndex0(block.blockType, 0);
        index0_Ai[1] = rouletteContentList_Target[number].index[1] + blockDataBase.GetIndex0(block.blockType, 1);

        index1_Ai[0] = rouletteContentList_Target[number].index[0] + blockDataBase.GetIndex1(block.blockType, 0);
        index1_Ai[1] = rouletteContentList_Target[number].index[1] + blockDataBase.GetIndex1(block.blockType, 1);

        index2_Ai[0] = rouletteContentList_Target[number].index[0] + blockDataBase.GetIndex2(block.blockType, 0);
        index2_Ai[1] = rouletteContentList_Target[number].index[1] + blockDataBase.GetIndex2(block.blockType, 1);

        index3_Ai[0] = rouletteContentList_Target[number].index[0] + blockDataBase.GetIndex3(block.blockType, 0);
        index3_Ai[1] = rouletteContentList_Target[number].index[1] + blockDataBase.GetIndex3(block.blockType, 1);

        index4_Ai[0] = rouletteContentList_Target[number].index[0] + blockDataBase.GetIndex4(block.blockType, 0);
        index4_Ai[1] = rouletteContentList_Target[number].index[1] + blockDataBase.GetIndex4(block.blockType, 1);

        index5_Ai[0] = rouletteContentList_Target[number].index[0] + blockDataBase.GetIndex5(block.blockType, 0);
        index5_Ai[1] = rouletteContentList_Target[number].index[1] + blockDataBase.GetIndex5(block.blockType, 1);

        index6_Ai[0] = rouletteContentList_Target[number].index[0] + blockDataBase.GetIndex6(block.blockType, 0);
        index6_Ai[1] = rouletteContentList_Target[number].index[1] + blockDataBase.GetIndex6(block.blockType, 1);

        index7_Ai[0] = rouletteContentList_Target[number].index[0] + blockDataBase.GetIndex7(block.blockType, 0);
        index7_Ai[1] = rouletteContentList_Target[number].index[1] + blockDataBase.GetIndex7(block.blockType, 1);

        index8_Ai[0] = rouletteContentList_Target[number].index[0] + blockDataBase.GetIndex8(block.blockType, 0);
        index8_Ai[1] = rouletteContentList_Target[number].index[1] + blockDataBase.GetIndex8(block.blockType, 1);

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
    }

    void ShowBettingNumber()
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
                            break; ;
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
                                break; ;
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
                            break; ;
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
                                break; ;
                        }
                    }
                }
            }
        }

        bettingNumberList = bettingNumberList.Distinct().ToList();
        bettingNumberList.Sort();

        otherBettingList = "";

        for (int i = 0; i < bettingNumberList.Count; i++)
        {
            otherBettingList += bettingNumberList[i].ToString() + "/";
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2 && otherBettingList.Length > 0)
        {
            PV.RPC("ShowOtherBetting", RpcTarget.Others, otherBettingList.TrimEnd('/'));
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
            if (newbieBlockContent.gameObject.activeInHierarchy)
            {
                newbieBlockContent.ResetPos();
            }
        }
        else
        {
            for (int i = 0; i < blockContentList.Count; i++)
            {
                if (blockContentList[i].gameObject.activeInHierarchy)
                {
                    blockContentList[i].ResetPos();
                }
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

        index0[0] = rouletteContent.index[0] + blockDataBase.GetIndex0(blockType, 0);
        index0[1] = rouletteContent.index[1] + blockDataBase.GetIndex0(blockType, 1);

        index1[0] = rouletteContent.index[0] + blockDataBase.GetIndex1(blockType, 0);
        index1[1] = rouletteContent.index[1] + blockDataBase.GetIndex1(blockType, 1);

        index2[0] = rouletteContent.index[0] + blockDataBase.GetIndex2(blockType, 0);
        index2[1] = rouletteContent.index[1] + blockDataBase.GetIndex2(blockType, 1);

        index3[0] = rouletteContent.index[0] + blockDataBase.GetIndex3(blockType, 0);
        index3[1] = rouletteContent.index[1] + blockDataBase.GetIndex3(blockType, 1);

        index4[0] = rouletteContent.index[0] + blockDataBase.GetIndex4(blockType, 0);
        index4[1] = rouletteContent.index[1] + blockDataBase.GetIndex4(blockType, 1);

        index5[0] = rouletteContent.index[0] + blockDataBase.GetIndex5(blockType, 0);
        index5[1] = rouletteContent.index[1] + blockDataBase.GetIndex5(blockType, 1);

        index6[0] = rouletteContent.index[0] + blockDataBase.GetIndex6(blockType, 0);
        index6[1] = rouletteContent.index[1] + blockDataBase.GetIndex6(blockType, 1);

        index7[0] = rouletteContent.index[0] + blockDataBase.GetIndex7(blockType, 0);
        index7[1] = rouletteContent.index[1] + blockDataBase.GetIndex7(blockType, 1);

        index8[0] = rouletteContent.index[0] + blockDataBase.GetIndex8(blockType, 0);
        index8[1] = rouletteContent.index[1] + blockDataBase.GetIndex8(blockType, 1);

        switch (rouletteContent.rouletteType)
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
                        blockLevelContentList_Target[i].SetOtherBlock(int.Parse(block[5]));
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
                        blockLevelContentList_Split_Horizontal[i].SetOtherBlock(int.Parse(block[5]));
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
                        blockLevelContentList_Split_Vertical[i].SetOtherBlock(int.Parse(block[5]));
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
                        blockLevelContentList_Square[i].SetOtherBlock(int.Parse(block[5]));
                    }
                }
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
    void ShowOtherBetting(string str)
    {
        string[] list = str.Split("/");

        for(int i = 0; i < list.Length; i ++)
        {
            otherBettingNumberList.Add(int.Parse(list[i]));
            list[i].Replace("/", "");
        }

        otherBettingNumberList = otherBettingNumberList.Distinct().ToList();
        otherBettingNumberList.Sort();

        Debug.Log("상대 배팅 위치 값을 받아왔습니다");
    }

    #endregion


    #region InGame

    public void UpdateMoney()
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
        }

        Debug.Log("현재 돈 서버에 업데이트");
    }

    public void LoadMoney()
    {
        int myNumber = 0;

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
        StopAllCoroutines();

        uIManager.CloseSurrenderView();

        money = 0;

        GameStateManager.instance.Playing = false;

        GameEnd(1);

        PV.RPC("Surrender", RpcTarget.Others);

        Debug.Log("기권");
    }

    [PunRPC]
    public void Surrender()
    {
        StopAllCoroutines();

        GameStateManager.instance.Playing = false;
        GameEnd(2);

        Debug.Log("상대방이 기권하여 승리하였습니다");
    }

    [Button]
    public void Winner()
    {
        StopAllCoroutines();

        GameStateManager.instance.Playing = false;
        GameEnd(2);

        Debug.Log("상대방이 방에서 튕겼습니다");
    }

    public void Draw()
    {
        GameStateManager.instance.Playing = false;
        GameEnd(3);

        Debug.Log("튕겨서 재접속 했으나 방이 사라져서 무승부 처리되었습니다");
    }

    #endregion
}
