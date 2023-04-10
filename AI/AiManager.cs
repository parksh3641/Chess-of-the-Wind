using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int random = 0;
    public int limitBlock = 0; //입장 제한 블럭 가치
    public int limitBlockLevel_N = 0;
    public int limitBlockLevel_R = 0;
    public int limitBlockLevel_SR = 0;
    public int limitBlockLevel_SSR = 0;
    private int value = 0;

    public int blockIndex = 0;
    public int blockPos = 0;


    [Space]
    [Title("bool")]
    public bool isPut = false;
    public bool isMove = false;

    [Space]
    [Title("Array")]
    public BlockClass[] blockClassArray;
    public BlockType[] blockTypeArray;

    public int[] bettingValue = new int[3]; //각 블럭에 배팅 금액

    public int[] dontBettingZone1;
    public int[] dontBettingZone2;
    public int[] dontBettingZone3;


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

    public int RandomCharacter()
    {
        random = Random.Range(1, 3);

        return random;
    }

    public void RestartGame()
    {
        isPut = false;
        isMove = false;

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

        if (gameType == GameType.NewBie)
        {
            blockClassArray = new BlockClass[1];
            blockTypeArray = new BlockType[1];
            bettingValue = new int[1];

            if (random == 1)
            {
                windCharacterType = WindCharacterType.Winter;

                blockTypeArray[0] = BlockType.Pawn_Snow;

                SetBlockClass();
            }
            else
            {
                windCharacterType = WindCharacterType.UnderWorld;

                blockTypeArray[0] = BlockType.Pawn_Under;

                SetBlockClass();
            }
        }
        else
        {
            blockClassArray = new BlockClass[3];
            blockTypeArray = new BlockType[3];
            bettingValue = new int[3];

            if (random == 1)
            {
                windCharacterType = WindCharacterType.Winter;

                blockTypeArray[0] = BlockType.LeftQueen_2;
                blockTypeArray[1] = BlockType.LeftNight;
                blockTypeArray[2] = BlockType.Rook_V2;

                dontBettingZone1 = new int[] { 6, 11, 16, 21 };
                dontBettingZone2 = new int[] { 6, 7, 8, 9, 10, 11, 16, 21 };
                dontBettingZone3 = new int[] { };

                SetBlockClass();
            }
            else
            {
                windCharacterType = WindCharacterType.UnderWorld;

                blockTypeArray[0] = BlockType.RightQueen_2;
                blockTypeArray[1] = BlockType.RightNight;
                blockTypeArray[2] = BlockType.Rook_V4;

                dontBettingZone1 = new int[] { 10, 15, 20, 25 };
                dontBettingZone2 = new int[] { 6, 7, 8, 9, 10, 15, 20, 25 };
                dontBettingZone3 = new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21, 22, 23, 24, 25 };

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
            blockIndex = 0;

            blockPos = Random.Range(1, 10);

            otherBlockContentList[0].gameObject.SetActive(true);
            otherBlockContentList[0].transform.position = gameManager.rouletteContentList_Target[blockPos - 1].transform.position;
            otherBlockContentList[0].SetOtherBlock(blockClassArray[blockIndex].blockType, aiName, value.ToString());

            Debug.Log("Ai가 초보방 블럭을 " + (blockPos - 1) + "번에 놓았습니다");

            gameManager.SetBettingNumber_Ai(blockClassArray[0], blockPos - 1);
        }
        else
        {
            blockIndex = Random.Range(0, 3);

            dontBettingZone1 = new int[] { 10, 15, 20, 25 };
            dontBettingZone2 = new int[] { 6, 7, 8, 9, 10, 15, 20, 25 };
            dontBettingZone3 = new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21, 22, 23, 24, 25 };

            if (blockIndex == 0)
            {
                blockPos = GenerateRandomNumber(dontBettingZone1);
            }
            else if(blockIndex == 1)
            {
                blockPos = GenerateRandomNumber(dontBettingZone2);
            }
            else
            {
                blockPos = GenerateRandomNumber(dontBettingZone3);
            }

            otherBlockContentList[blockIndex].gameObject.SetActive(true);
            otherBlockContentList[blockIndex].transform.position = gameManager.rouletteContentList_Target[blockPos - 1].transform.position;
            otherBlockContentList[blockIndex].SetOtherBlock(blockClassArray[blockIndex].blockType, aiName, value.ToString());

            Debug.Log("Ai가 고수방 블럭을 " + (blockPos - 1) + "번에 놓았습니다");

            gameManager.SetBettingNumber_Ai(blockClassArray[blockIndex], blockPos - 1);
        }
    }

    public void MoveBlock()
    {
        if (isMove) return;

        isMove = true;

        if(gameType == GameType.NewBie)
        {
            blockPos = Random.Range(1, 10);

            otherBlockContentList[0].gameObject.SetActive(true);
            otherBlockContentList[0].transform.position = gameManager.rouletteContentList_Target[blockPos - 1].transform.position;
            otherBlockContentList[0].SetOtherBlock(blockClassArray[blockIndex].blockType, aiName, value.ToString());

            Debug.Log("Ai가 초보방 블럭을 " + (blockPos - 1) + "번으로 위치를 바꿨습니다");

            gameManager.SetBettingNumber_Ai(blockClassArray[0], blockPos - 1);
        }
        else
        {
            dontBettingZone1 = new int[] { 10, 15, 20, 25 };
            dontBettingZone2 = new int[] { 6, 7, 8, 9, 10, 15, 20, 25 };
            dontBettingZone3 = new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21, 22, 23, 24, 25 };

            if (blockIndex == 0)
            {
                blockPos = GenerateRandomNumber(dontBettingZone1);
            }
            else if (blockIndex == 1)
            {
                blockPos = GenerateRandomNumber(dontBettingZone2);
            }
            else
            {
                blockPos = GenerateRandomNumber(dontBettingZone3);
            }

            otherBlockContentList[blockIndex].gameObject.SetActive(true);
            otherBlockContentList[blockIndex].transform.position = gameManager.rouletteContentList_Target[blockPos - 1].transform.position;
            otherBlockContentList[blockIndex].SetOtherBlock(blockClassArray[blockIndex].blockType, aiName, value.ToString());

            Debug.Log("Ai가 고수방 블럭을 " + (blockPos - 1) + "번으로 위치를 바꿨습니다");

            gameManager.SetBettingNumber_Ai(blockClassArray[blockIndex], blockPos - 1);
        }
    }

    private int GenerateRandomNumber(int[] exclusionList)
    {
        int randomValue = Random.Range(6, 26);
        while (exclusionList.Contains(randomValue))
        {
            randomValue = Random.Range(6, 26);
        }
        return randomValue;
    }
}
