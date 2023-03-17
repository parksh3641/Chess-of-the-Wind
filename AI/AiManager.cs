using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AiManager : MonoBehaviour
{
    [Title("Player Information")]
    public string aiName = "인공지능";
    public GameType gameType = GameType.NewBie;
    public WindCharacterType windCharacterType = WindCharacterType.Winter;
    public GameRankType gameRankType = GameRankType.Bronze_4;

    [Space]
    [Title("Prefab")]
    public OtherBlockContent otherBlockContent;
    public Transform otherBlockContentTransform;
    public List<OtherBlockContent> otherBlockContentList = new List<OtherBlockContent>();

    [Space]
    [Title("Value")]
    public int limitBlock = 0; //입장 제한 블럭 가치
    public int limitBlockLevel_N = 0;
    public int limitBlockLevel_R = 0;
    public int limitBlockLevel_SR = 0;
    public int limitBlockLevel_SSR = 0;
    private int value = 0;

    public int index = 0;


    [Space]
    [Title("bool")]
    public bool isPut = false;

    [Space]
    [Title("Array")]
    public BlockClass[] blockClassArray;
    public BlockType[] blockTypeArray;

    public int[] bettingValue = new int[3]; //각 블럭에 배팅 금액

    RankInformation rankInformation = new RankInformation();

    public GameManager gameManager;
    public UpgradeDataBase upgradeDataBase;
    public RankDataBase rankDataBase;

    private void Awake()
    {
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;

        for(int i = 0; i < 3; i ++)
        {
            OtherBlockContent content = Instantiate(otherBlockContent);
            content.transform.parent = otherBlockContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            otherBlockContentList.Add(content);
        }
    }
    public void RestartGame()
    {
        isPut = false;

        for (int i = 0; i < otherBlockContentList.Count; i++)
        {
            otherBlockContentList[i].gameObject.SetActive(false);
        }
    }

    public void Initialize()
    {
        RestartGame();

        gameType = GameStateManager.instance.GameType;
        gameRankType = GameStateManager.instance.GameRankType;

        rankInformation = rankDataBase.GetRankInformation(gameRankType);
        limitBlock = rankInformation.limitBlockValue;

        limitBlockLevel_N = upgradeDataBase.CheckUpgradeValue(RankType.N, limitBlock);
        limitBlockLevel_R = upgradeDataBase.CheckUpgradeValue(RankType.R, limitBlock);
        limitBlockLevel_SR = upgradeDataBase.CheckUpgradeValue(RankType.SR, limitBlock);
        limitBlockLevel_SSR = upgradeDataBase.CheckUpgradeValue(RankType.SSR, limitBlock);

        int random = Random.Range(0, 2);

        if (gameType == GameType.NewBie)
        {
            blockClassArray = new BlockClass[1];
            blockTypeArray = new BlockType[1];
            bettingValue = new int[1];

            if (random == 0)
            {
                windCharacterType = WindCharacterType.Winter;

                blockTypeArray[0] = BlockType.Pawn;

                SetBlockClass();
            }
            else
            {
                windCharacterType = WindCharacterType.UnderWorld;

                blockTypeArray[0] = BlockType.Pawn;

                SetBlockClass();
            }
        }
        else
        {
            blockClassArray = new BlockClass[3];
            blockTypeArray = new BlockType[3];
            bettingValue = new int[3];

            if (random == 0)
            {
                windCharacterType = WindCharacterType.Winter;

                blockTypeArray[0] = BlockType.LeftQueen_2;
                blockTypeArray[1] = BlockType.LeftNight;
                blockTypeArray[2] = BlockType.Rook_V2;

                SetBlockClass();
            }
            else
            {
                windCharacterType = WindCharacterType.UnderWorld;

                blockTypeArray[0] = BlockType.RightQueen_2;
                blockTypeArray[1] = BlockType.RightNight;
                blockTypeArray[2] = BlockType.Rook_V2H2;

                SetBlockClass();
            }
        }
    }

    void SetBlockClass()
    {
        for (int i = 0; i < blockClassArray.Length; i++)
        {
            BlockClass blockClass = new BlockClass();

            blockClass.blockType = blockTypeArray[i];

            if(upgradeDataBase.GetUpgradeValue(RankType.SSR).GetValueNumber(limitBlockLevel_SSR) <= limitBlock)
            {
                blockClass.rankType = RankType.SSR;
                blockClass.level = limitBlockLevel_SSR;
            }
            else if (upgradeDataBase.GetUpgradeValue(RankType.SR).GetValueNumber(limitBlockLevel_SR) <= limitBlock)
            {
                blockClass.rankType = RankType.SR;
                blockClass.level = limitBlockLevel_SR;
            }
            else if (upgradeDataBase.GetUpgradeValue(RankType.R).GetValueNumber(limitBlockLevel_R) <= limitBlock)
            {
                blockClass.rankType = RankType.R;
                blockClass.level = limitBlockLevel_R;
            }
            else
            {
                blockClass.rankType = RankType.N;
                blockClass.level = limitBlockLevel_N;
            }

            value = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level - Random.Range(0, 2));
            otherBlockContentList[i].SetOtherBlock(blockClass.blockType, aiName, value.ToString());

            blockClassArray[i] = blockClass;
            bettingValue[i] = value;
        }
    }

    public void PutBlock()
    {
        if (isPut) return;

        isPut = true;

        if (gameType == GameType.NewBie)
        {
            otherBlockContentList[0].gameObject.SetActive(true);

            index = Random.Range(1, 26);

            otherBlockContentList[0].transform.position = gameManager.rouletteContentList_Target[index - 1].transform.position;

            Debug.Log("Ai가 초보방 블럭을 " + index + "번에 놓았습니다");
        }
        else
        {
            Debug.Log("Ai가 고수방 블럭을 놓았습니다");
        }
    }
}
