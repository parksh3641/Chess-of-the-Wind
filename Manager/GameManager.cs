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

    public BlockType blockType = BlockType.Default;
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

    public List<int> bettingNumberList = new List<int>();
    public List<int> otherBettingNumberList = new List<int>();

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
    private bool aiMoveBlock = false; //확률적으로 Ai가 2초 남기고 위치를 바꿉니다

    private int[] compareMoney = new int[2];

    private string otherBettingList = "";

    private int targetNumber = 0;
    private int targetQueenNumber = 0;

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

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    [Space]
    [Title("Roulette")]
    List<RouletteContent> rouletteContentList_Gosu = new List<RouletteContent>();
    List<RouletteContent> rouletteSplitContentList_Vertical = new List<RouletteContent>();
    List<RouletteContent> rouletteSplitContentList_Horizontal = new List<RouletteContent>();
    List<RouletteContent> rouletteSquareContentList = new List<RouletteContent>();

    List<RouletteContent> rouletteContentList_NewBie = new List<RouletteContent>();

    List<RouletteContent> allContentList = new List<RouletteContent>();

    List<RouletteContent> numberContentList = new List<RouletteContent>();
    List<RouletteContent> numberContentList_NewBie = new List<RouletteContent>();

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
    public RandomBoxManager randomBoxManager;
    public EmoteManager emoteManager;
    public MoneyAnimation moneyAnimation;

    UpgradeDataBase upgradeDataBase;
    PlayerDataBase playerDataBase;
    BlockDataBase blockDataBase;

    public PhotonView PV;

    int[] index0, index1, index2, index3, index4, index5, index6, index7, index8 = new int[2];
    int[] index0_Ai, index1_Ai, index2_Ai, index3_Ai, index4_Ai, index5_Ai, index6_Ai, index7_Ai, index8_Ai = new int[2];

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;

        bettingValue = new int[4];
        bettingSizeList = new int[4];
        bettingList = new int[4];
        bettingMinusList = new int[4];

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

    void OnApplicationQuit()
    {
        if(GameStateManager.instance.Playing)
        {
            int number = (int)(money * 0.1f);

            GameStateManager.instance.Penalty = number;
        }
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

        targetObj.SetActive(false);
        targetQueenObj.SetActive(false);

        targetText.text = "-";

        timer = bettingWaitTime;
        timerText.text = timer.ToString();
        timerFillAmount.fillAmount = 1;

        moneyText.text = moneyText.text = "LP  <size=25>0</size>";
        bettingMoneyText.text = "0";
        otherMoneyText.text = moneyText.text = "LP  <size=25>0</size>";

        RecordManager.instance.Initialize();
        recordText.text = "";

        inputTargetNumber.text = "";

        ClearOtherPlayerBlock();
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
        soundManager.StopAllSFX();
        soundManager.Initialize();

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

    public void GameStart_Initialize()
    {
        emoteManager.Initialize();
        moneyAnimation.Initialize();

        rouletteContentTransform_NewBie.gameObject.SetActive(false);
        rouletteContentTransform.gameObject.SetActive(false);
        rouletteContentTransformSplitBet_Vertical.gameObject.SetActive(false);
        rouletteContentTransformSplitBet_Horizontal.gameObject.SetActive(false);
        rouletteContentTransformSquareBet.gameObject.SetActive(false);

        blockGridParent_NewBie.SetActive(false);
        blockGridParent_Gosu.SetActive(false);

        blockContentTransform_NewBie.gameObject.SetActive(false);
        blockContentTransform_Gosu.gameObject.SetActive(false);

        numberContentTransform_NewBie.gameObject.SetActive(false);
        numberContentTransform.gameObject.SetActive(false);
    }

    public void GameStart_Newbie()
    {
        roomText.text = "초보방";

        developerInfo.text = "0 = 퀸 당첨\n1 ~ 8 = 해당 숫자 당첨\n9 = 보너스 룰렛 실행\n빈칸 = 정상 진행";

        rouletteContentTransform.gameObject.SetActive(false);
        rouletteContentTransformSplitBet_Vertical.gameObject.SetActive(false);
        rouletteContentTransformSplitBet_Horizontal.gameObject.SetActive(false);
        rouletteContentTransformSquareBet.gameObject.SetActive(false);

        rouletteContentTransform_NewBie.gameObject.SetActive(true);
        blockGridParent_NewBie.SetActive(true);
        blockContentTransform_NewBie.gameObject.SetActive(true);
        numberContentTransform_NewBie.gameObject.SetActive(true);

        rouletteContentList_Target = rouletteContentList_NewBie;

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

        blockGridParent_Gosu.SetActive(true);
        blockContentTransform_Gosu.gameObject.SetActive(true);
        numberContentTransform.gameObject.SetActive(true);

        rouletteContentList_Target = rouletteContentList_Gosu;

        for (int i = 0; i < 3; i ++)
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
        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            stakes = GameStateManager.instance.Stakes / 2;
        }
        else
        {
            stakes = GameStateManager.instance.Stakes;
        }

        money = stakes;
        otherMoney = stakes;

        moneyText.text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
        otherMoneyText.text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";
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

    public void CheckWinnerPlayer() //게임 누가 승리했는지 체크
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if(money <= 0 && otherMoney <= 0) //둘다 0원일때
        {
            GameEnd(3);
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
        else if (number == 2)
        {
            Debug.Log("상대방 항복으로 승리");
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

        NotionManager.instance.UseNotion(NotionType.BettingTimesUp);
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
        emoteManager.Initialize();

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

            if(GameStateManager.instance.GameType == GameType.NewBie)
            {
                trans = rouletteContentList_Target[4].transform;
            }
            else
            {
                trans = rouletteContentList_Target[12].transform;
            }
            targetQueenObj.transform.position = trans.position;

            targetText.text = "퀸";
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
        tempMoney = 0;

        plusAiMoney = 0;
        tempAiMoney = 0;

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

                if(aiMode)
                {
                    CheckTargetNumber_Ai(targetNumber);
                }
            }
        }

        for (int i = 0; i < bettingMinusList.Length; i++) //마지막에 당첨 안 된거 만큼 빼기
        {
            plusMoney -= (bettingList[i] / (bettingSizeList[i] + 1)) * bettingMinusList[i];
        }

        if (bettingMoney == 0)
        {
            localNotion = "변동이 없습니다";

            NotionManager.instance.UseNotion(localNotion, ColorType.Green);
            RecordManager.instance.SetRecord("0");
        }
        else
        {
            if ((int)(plusMoney) - bettingMoney > 0) //배팅한 것보다 딴 돈이 많을 경우
            {
                tempMoney = (int)(plusMoney) - bettingMoney;

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, tempMoney);

                //worldNotion = "<color=#00FF00>+" + MoneyUnitString.ToCurrencyString(tempMoney) + "   " + GameStateManager.instance.NickName + "</color>";
                localNotion = "+" + MoneyUnitString.ToCurrencyString(tempMoney);

                NotionManager.instance.UseNotion(localNotion, ColorType.Green);
                //RecordManager.instance.SetRecord(localNotion);
            }
            else
            {
                tempMoney = (int)(plusMoney) - bettingMoney;

                //worldNotion = "<color=#FF0000>" + MoneyUnitString.ToCurrencyString(tempMoney) + "   " + GameStateManager.instance.NickName + "</color>";
                localNotion = MoneyUnitString.ToCurrencyString(tempMoney);

                NotionManager.instance.UseNotion(localNotion, ColorType.Red);
                //RecordManager.instance.SetRecord(localNotion);
            }

            //PV.RPC("ChatRPC", RpcTarget.All, worldNotion);

            //randomBoxManager.GameReward();
        }

        Debug.LogError(MoneyUnitString.ToCurrencyString(tempMoney) + " 만큼 내가 돈을 획득");

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
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
            }
            else
            {
                tempAiMoney = (int)(plusAiMoney) - bettingAiMoney;
            }

            //Debug.LogError(MoneyUnitString.ToCurrencyString(tempAiMoney) + " 만큼 Ai가 돈을 획득");

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

    public void ChangeGetMoney(BlockClass block, RouletteType type, bool queen)
    {
        int value = 0;

        if(!allIn)
        {
            value = upgradeDataBase.GetUpgradeValue(block.rankType).GetValueNumber(block.level);
        }
        else
        {
            value = bettingMoney;
        }

        for(int i = 0; i < bettingValue.Length; i ++) //당첨된 블럭 빼주기
        {
            if(value.Equals(bettingValue))
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
                    plusMoney += blockMotherInformation.queenStraightBet * value + value;
                }
                else
                {
                    plusMoney += blockMotherInformation.straightBet * value + value;
                }

                break;
            case RouletteType.SplitBet_Horizontal:
                if (queen)
                {
                    plusMoney += blockMotherInformation.queensplitBet * value + value;
                }
                else
                {
                    plusMoney += blockMotherInformation.splitBet * value + value;
                }

                break;
            case RouletteType.SplitBet_Vertical:
                if (queen)
                {
                    plusMoney += blockMotherInformation.queensplitBet * value + value;
                }
                else
                {
                    plusMoney += blockMotherInformation.splitBet * value + value;
                }

                break;
            case RouletteType.SquareBet:
                if (queen)
                {
                    plusMoney += blockMotherInformation.queensquareBet * value + value;
                }
                else
                {
                    plusMoney += blockMotherInformation.squareBet * value + value;
                }

                break;
        }
    }

    public void ChangeGetMoney_Ai(BlockClass block, RouletteType type, bool queen)
    {
        int value = upgradeDataBase.GetUpgradeValue(block.rankType).GetValueNumber(block.level);

        //for (int i = 0; i < bettingValue.Length; i++) //당첨된 블럭 빼주기
        //{
        //    if (value.Equals(bettingValue))
        //    {
        //        bettingMinusList[i] -= 1;
        //    }
        //}

        switch (type)
        {
            case RouletteType.Default:
                break;
            case RouletteType.StraightBet:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queenStraightBet * value + value;
                }
                else
                {
                    plusAiMoney += blockMotherInformation.straightBet * value + value;
                }

                break;
            case RouletteType.SplitBet_Horizontal:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queensplitBet * value + value;
                }
                else
                {
                    plusAiMoney += blockMotherInformation.splitBet * value + value;
                }

                break;
            case RouletteType.SplitBet_Vertical:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queensplitBet * value + value;
                }
                else
                {
                    plusAiMoney += blockMotherInformation.splitBet * value + value;
                }

                break;
            case RouletteType.SquareBet:
                if (queen)
                {
                    plusAiMoney += blockMotherInformation.queensquareBet * value + value;
                }
                else
                {
                    plusAiMoney += blockMotherInformation.squareBet * value + value;
                }

                break;
        }
    }

    void ChangeBettingMoney()
    {
        moneyText.text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(money - bettingMoney) + "</size>";
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

        for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++) // 1 2 / 3 4
        {
            if (rouletteSplitContentList_Horizontal[i].isActive)
            {
                switch (i)
                {
                    case 0:
                        if (number == 1 || number == 6)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 7)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 8)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 9)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 4:
                        if (number == 5 || number == 10)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 5:
                        if (number == 6 || number == 11)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 6:
                        if (number == 7 || number == 12)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 7:
                        if (number == 8)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 8:
                        if (number == 9 || number == 14)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 9:
                        if (number == 10 || number == 15)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 10:
                        if (number == 11 || number == 16)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 11:
                        if (number == 12 || number == 17)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 12:
                        if (number == 18)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 13:
                        if (number == 14 || number == 19)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 14:
                        if (number == 15 || number == 20)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 15:
                        if (number == 16 || number == 21)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 16:
                        if (number == 17 || number == 22)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 17:
                        if (number == 18 || number == 23)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 18:
                        if (number == 19 || number == 24)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Horizontal[i].blockClass, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 19:
                        if (number == 20 || number == 25)
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
                        if (number == 1 || number == 2)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 3)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 4)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 5)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 4:
                        if (number == 6 || number == 7)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 5:
                        if (number == 7 || number == 8)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 6:
                        if (number == 8 || number == 9)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 7:
                        if (number == 9 || number == 10)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 8:
                        if (number == 11 || number == 12)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 9:
                        if (number == 12)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 10:
                        if (number == 13)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 11:
                        if (number == 14 || number == 15)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 12:
                        if (number == 16 || number == 17)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 13:
                        if (number == 17 || number == 18)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 14:
                        if (number == 18 || number == 19)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 15:
                        if (number == 19 || number == 20)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 16:
                        if (number == 21 || number == 22)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 17:
                        if (number == 22 || number == 23)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 18:
                        if (number == 23 || number == 24)
                        {
                            ChangeGetMoney(rouletteSplitContentList_Vertical[i].blockClass, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 19:
                        if (number == 24 || number == 25)
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
                        if (number == 1 || number == 2 || number == 6 || number == 7)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 3 || number == 7 || number == 8)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 4 || number == 8 || number == 9)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 5 || number == 9 || number == 10)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 4:
                        if (number == 6 || number == 7 || number == 11 || number == 12)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 5:
                        if (number == 7 || number == 8 || number == 12)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 6:
                        if (number == 8 || number == 9 || number == 14)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 7:
                        if (number == 9 || number == 10 || number == 14 || number == 15)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 8:
                        if (number == 11 || number == 12 || number == 16 || number == 17)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 9:
                        if (number == 12 || number == 17 || number == 18)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 10:
                        if (number == 14 || number == 18 || number == 19)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 11:
                        if (number == 14 || number == 15 || number == 19 || number == 20)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 12:
                        if (number == 16 || number == 17 || number == 21 || number == 22)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 13:
                        if (number == 17 || number == 18 || number == 22 || number == 23)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 14:
                        if (number == 18 || number == 19 || number == 23 || number == 24)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
                        }
                        break;
                    case 15:
                        if (number == 19 || number == 20 || number == 24 || number == 25)
                        {
                            ChangeGetMoney(rouletteSquareContentList[i].blockClass, RouletteType.SquareBet, false);
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

        for (int i = 0; i < rouletteSplitContentList_Horizontal.Count; i++) // 1 2 / 3 4
        {
            if (rouletteSplitContentList_Horizontal[i].isActive_Ai)
            {
                switch (i)
                {
                    case 0:
                        if (number == 1 || number == 6)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 7)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 8)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 9)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 4:
                        if (number == 5 || number == 10)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 5:
                        if (number == 6 || number == 11)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 6:
                        if (number == 7 || number == 12)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 7:
                        if (number == 8)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 8:
                        if (number == 9 || number == 14)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 9:
                        if (number == 10 || number == 15)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 10:
                        if (number == 11 || number == 16)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 11:
                        if (number == 12 || number == 17)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 12:
                        if (number == 18)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 13:
                        if (number == 14 || number == 19)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 14:
                        if (number == 15 || number == 20)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 15:
                        if (number == 16 || number == 21)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 16:
                        if (number == 17 || number == 22)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 17:
                        if (number == 18 || number == 23)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 18:
                        if (number == 19 || number == 24)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                    case 19:
                        if (number == 20 || number == 25)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[i].blockClass_Ai, RouletteType.SplitBet_Horizontal, false);
                        }
                        break;
                }
            }
        }

        for (int i = 0; i < rouletteSplitContentList_Vertical.Count; i++) // 1 6 / 2 7
        {
            if (rouletteSplitContentList_Vertical[i].isActive_Ai)
            {
                switch (i)
                {
                    case 0:
                        if (number == 1 || number == 2)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 3)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 4)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 5)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 4:
                        if (number == 6 || number == 7)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 5:
                        if (number == 7 || number == 8)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 6:
                        if (number == 8 || number == 9)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 7:
                        if (number == 9 || number == 10)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 8:
                        if (number == 11 || number == 12)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 9:
                        if (number == 12)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 10:
                        if (number == 13)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 11:
                        if (number == 14 || number == 15)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 12:
                        if (number == 16 || number == 17)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 13:
                        if (number == 17 || number == 18)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 14:
                        if (number == 18 || number == 19)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 15:
                        if (number == 19 || number == 20)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 16:
                        if (number == 21 || number == 22)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 17:
                        if (number == 22 || number == 23)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 18:
                        if (number == 23 || number == 24)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                    case 19:
                        if (number == 24 || number == 25)
                        {
                            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[i].blockClass_Ai, RouletteType.SplitBet_Vertical, false);
                        }
                        break;
                }
            }
        }

        for (int i = 0; i < rouletteSquareContentList.Count; i++) //1 2 6 7
        {
            if (rouletteSquareContentList[i].isActive_Ai)
            {
                switch (i)
                {
                    case 0:
                        if (number == 1 || number == 2 || number == 6 || number == 7)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 1:
                        if (number == 2 || number == 3 || number == 7 || number == 8)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 2:
                        if (number == 3 || number == 4 || number == 8 || number == 9)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 3:
                        if (number == 4 || number == 5 || number == 9 || number == 10)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 4:
                        if (number == 6 || number == 7 || number == 11 || number == 12)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 5:
                        if (number == 7 || number == 8 || number == 12)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 6:
                        if (number == 8 || number == 9 || number == 14)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 7:
                        if (number == 9 || number == 10 || number == 14 || number == 15)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 8:
                        if (number == 11 || number == 12 || number == 16 || number == 17)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 9:
                        if (number == 12 || number == 17 || number == 18)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 10:
                        if (number == 14 || number == 18 || number == 19)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 11:
                        if (number == 14 || number == 15 || number == 19 || number == 20)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 12:
                        if (number == 16 || number == 17 || number == 21 || number == 22)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 13:
                        if (number == 17 || number == 18 || number == 22 || number == 23)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 14:
                        if (number == 18 || number == 19 || number == 23 || number == 24)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
                        }
                        break;
                    case 15:
                        if (number == 19 || number == 20 || number == 24 || number == 25)
                        {
                            ChangeGetMoney_Ai(rouletteSquareContentList[i].blockClass_Ai, RouletteType.SquareBet, false);
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

    private void CheckQueenNumber_Ai()
    {
        if (rouletteContentList_Target[12].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteContentList_Target[12].blockClass_Ai, RouletteType.StraightBet, true);
        }

        if (rouletteSplitContentList_Horizontal[7].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[7].blockClass_Ai, RouletteType.SplitBet_Horizontal, true);
        }

        if (rouletteSplitContentList_Horizontal[12].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteSplitContentList_Horizontal[12].blockClass_Ai, RouletteType.SplitBet_Horizontal, true);
        }

        if (rouletteSplitContentList_Vertical[9].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[9].blockClass_Ai, RouletteType.SplitBet_Vertical, true);
        }

        if (rouletteSplitContentList_Vertical[10].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteSplitContentList_Vertical[10].blockClass_Ai, RouletteType.SplitBet_Vertical, true);
        }

        if (rouletteSquareContentList[5].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteSquareContentList[5].blockClass_Ai, RouletteType.SquareBet, true);
        }

        if (rouletteSquareContentList[6].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteSquareContentList[6].blockClass_Ai, RouletteType.SquareBet, true);
        }

        if (rouletteSquareContentList[9].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteSquareContentList[9].blockClass_Ai, RouletteType.SquareBet, true);
        }

        if (rouletteSquareContentList[10].isActive_Ai)
        {
            ChangeGetMoney_Ai(rouletteSquareContentList[10].blockClass_Ai, RouletteType.SquareBet, true);
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

        if (money < bettingValue[blockContent.index])
        {
            NotionManager.instance.UseNotion(NotionType.BettingAllin);

            Debug.Log("남은 골드를 모두 사용하여 배팅합니다.");

            allIn = true;
        }
        else
        {
            string notion = MoneyUnitString.ToCurrencyString(bettingValue[blockContent.index]) + " 값의 블록 배치";

            NotionManager.instance.UseNotion(notion, ColorType.Green);

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
        if(otherMoney < upgradeDataBase.GetUpgradeValue(block.rankType).GetValueNumber(block.level))
        {
            GameEnd(0);

            Debug.Log("Ai가 돈이 부족하여 항복하였습니다.");
            return;
        }

        for (int i = 0; i < rouletteContentList_Target.Count; i++)
        {
            rouletteContentList_Target[i].isActive_Ai = false;
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
                if(GameStateManager.instance.GameType == GameType.NewBie)
                {
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Total", GameStateManager.instance.Stakes / 2 } });
                }
                else
                {
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Total", GameStateManager.instance.Stakes } });
                }
                break;
            case 1:
                if (GameStateManager.instance.GameType == GameType.NewBie)
                {
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Total", GameStateManager.instance.Stakes / 2 } });
                }
                else
                {
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Total", GameStateManager.instance.Stakes } });
                }
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

        otherMoney -= other[0]; //상대방의 배팅한 금액만큼 일단 빼기

        if (tempMoney > 0 && other[1] > 0) //둘다 땃을 경우
        {
            if (tempMoney > other[1]) //내가 더 많이 땃을 경우
            {
                number = tempMoney - other[1];

                moneyAnimation.AddMoneyAnimation(money, otherMoney, number);

                money += number;
                otherMoney -= number;
            }
            else //상대방이 더 많이 땃을 경우
            {
                number = other[1] - tempMoney;

                moneyAnimation.MinusMoneyAnimation(money, otherMoney, number);

                money -= number;
                otherMoney += number;
            }
        }
        else if (tempMoney > 0 && other[1] < 0) //상대방은 잃고 난 땃을 경우 
        {
            moneyAnimation.AddMoneyAnimation(money, otherMoney + Mathf.Abs(other[1]), Mathf.Abs(tempMoney));

            money += Mathf.Abs(tempMoney);
            otherMoney -= Mathf.Abs(tempMoney) + Mathf.Abs(other[1]);
        }
        else if (tempMoney < 0 && other[1] > 0) //난 잃고 상대방만 땃을 경우
        {
            moneyAnimation.MinusMoneyAnimation(money, otherMoney, other[1] + Mathf.Abs(tempMoney));

            money -= other[1] + Mathf.Abs(tempMoney);
            otherMoney += other[1];
        }
        else if (tempMoney > 0 && other[1] == 0) //상대방이 배팅 안 했을 경우
        {
            moneyAnimation.AddMoneyAnimation(money, otherMoney, Mathf.Abs(tempMoney));

            money += Mathf.Abs(tempMoney);
            otherMoney -= Mathf.Abs(tempMoney);
        }
        else if (tempMoney == 0 && other[1] > 0) //내가 배팅 안 했을 경우
        {
            moneyAnimation.MinusMoneyAnimation(money, otherMoney, other[1]);

            money -= other[1];
            otherMoney += other[1];
        }
        else
        {
            moneyText.text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
            otherMoneyText.text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";
        }


        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            compareMoney[0] = otherMoney;
            compareMoney[1] = money;

            PV.RPC("SyncMoney", RpcTarget.Others, compareMoney);

            Debug.Log("서버 돈 동기화 갱신완료");
        }
    }

    [PunRPC]
    void SyncMoney(int[] compare)
    {
        if(money > compare[0]) //돈을 잃은 경우
        {
            moneyAnimation.correction = compare[1];

            moneyAnimation.MinusMoneyAnimation(money, otherMoney, money - compare[0]);

            money = compare[0];
            otherMoney = compare[1];
        }
        else if(money < compare[0]) //돈을 얻었을 경우
        {
            moneyAnimation.correction = compare[1];

            moneyAnimation.AddMoneyAnimation(money, otherMoney, compare[0] - money);

            money = compare[0];
            otherMoney = compare[1];
        }
        else //변동이 없을 경우
        {
            money = compare[0];
            otherMoney = compare[1];

            moneyText.text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
            otherMoneyText.text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";
        }
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

        GameEnd(2);

        Debug.Log("상대방이 기권하여 승리하였습니다");
    }

    public void Winner()
    {
        StopAllCoroutines();

        money += (int)(otherMoney * 0.1f);

        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, (int)(otherMoney * 0.1f)); //상대방 보유 금액의 10% 가져옴

        GameEnd(2);

        Debug.Log("상대방이 방에서 튕겼습니다");
    }

    #endregion
}
