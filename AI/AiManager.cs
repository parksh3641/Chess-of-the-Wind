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
    public RouletteType rouletteType = RouletteType.Default;

    [Space]
    [Title("Prefab")]
    public OtherBlockContent otherBlockContent;
    public Transform otherBlockContentTransform;
    public List<OtherBlockContent> otherBlockContentList = new List<OtherBlockContent>();

    [Space]
    [Title("Value")]
    public int random = 0;
    public int limitBlock = 0;
    private int value = 0;

    public int blockIndex = 0;
    public int rouletteIndex = 0;
    public int blockPos = 0;


    [Space]
    [Title("bool")]
    public bool isPut = false;
    public bool isMove = false;

    [Space]
    [Title("Array")]
    public BlockClass[] blockClassArray;
    public BlockType[] blockTypeArray;

    public int[] bettingValue = new int[3];

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
            content.transform.SetParent(otherBlockContentTransform);
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
        gameType = GameStateManager.instance.GameType;
        gameRankType = GameStateManager.instance.GameRankType;

        rankInformation = rankDataBase.GetRankInformation(gameRankType);

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

                SetBlockClass();
            }
            else
            {
                windCharacterType = WindCharacterType.UnderWorld;

                blockTypeArray[0] = BlockType.RightQueen_2;
                blockTypeArray[1] = BlockType.RightNight;
                blockTypeArray[2] = BlockType.Rook_V4;

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

            blockClass.level = rankDataBase.GetLimitLevel(GameStateManager.instance.GameRankType) - 1;

            if (blockClass.level < 5)
            {
                blockClass.rankType = RankType.N;
            }
            else if(blockClass.level < 10)
            {
                blockClass.rankType = RankType.R;
            }
            else if(blockClass.level < 15)
            {
                blockClass.rankType = RankType.SR;
            }
            else if(blockClass.level < 20)
            {
                blockClass.rankType = RankType.SSR;
            }
            else
            {
                blockClass.rankType = RankType.UR;
            }

            value = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);

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

            blockPos = Random.Range(0, 9);

            Debug.LogError(blockPos);

            otherBlockContentList[0].gameObject.SetActive(true);
            otherBlockContentList[0].transform.position = gameManager.rouletteContentList_Target[blockPos].transform.position;
            otherBlockContentList[0].SetOtherBlock(blockClassArray[blockIndex].blockType, aiName, value.ToString());

            gameManager.otherBlockType = blockClassArray[blockIndex].blockType;

            gameManager.SetBettingNumber_Ai(blockClassArray[0], blockPos, RouletteType.StraightBet);

            Debug.Log("Ai가 연습방 " + blockPos + " 위치에 배팅했습니다");
        }
        else
        {
            blockIndex = Random.Range(0, 3);
            rouletteType = RouletteType.Default + Random.Range(1, 4);

            switch (rouletteType)
            {
                case RouletteType.Default:
                    break;
                case RouletteType.StraightBet:
                    blockPos = Random.Range(0, 25);
                    break;
                case RouletteType.SplitBet_Horizontal:
                    blockPos = Random.Range(0, 19);
                    break;
                case RouletteType.SplitBet_Vertical:
                    blockPos = Random.Range(0, 19);
                    break;
                case RouletteType.SquareBet:
                    blockPos = Random.Range(0, 15);
                    break;
            }

            //Debug.LogError(blockPos);

            for(int i = 0; i < otherBlockContentList.Count; i ++)
            {
                otherBlockContentList[i].gameObject.SetActive(false);
            }

            if (gameManager.CheckAiBetting(blockClassArray[blockIndex], blockPos, rouletteType))
            {
                otherBlockContentList[blockIndex].gameObject.SetActive(true);

                switch (rouletteType)
                {
                    case RouletteType.Default:
                        break;
                    case RouletteType.StraightBet:
                        otherBlockContentList[blockIndex].transform.position = gameManager.rouletteContentList_Target[blockPos].transform.position;
                        break;
                    case RouletteType.SplitBet_Horizontal:
                        otherBlockContentList[blockIndex].transform.position = gameManager.rouletteContentList_Split_Horizontal[blockPos].transform.position;
                        break;
                    case RouletteType.SplitBet_Vertical:
                        otherBlockContentList[blockIndex].transform.position = gameManager.rouletteContentList_Split_Vertical[blockPos].transform.position;
                        break;
                    case RouletteType.SquareBet:
                        break;
                }

                gameManager.SetBettingNumber_Ai(blockClassArray[blockIndex], blockPos, rouletteType);

                //Debug.Log("Ai가 고수방 " + rouletteType.ToString() + " " + blockPos + " 위치에 배팅했습니다");
            }
            else
            {
                //Debug.Log("고수방 " + rouletteType.ToString() + " 블록을 놓을 수 없는 위치라서 다시 설정합니다");

                otherBlockContentList[blockIndex].gameObject.SetActive(false);

                isPut = false;
                PutBlock();
            }

            otherBlockContentList[blockIndex].SetOtherBlock(blockClassArray[blockIndex].blockType, aiName, value.ToString());
            gameManager.otherBlockType = blockClassArray[blockIndex].blockType;
        }
    }

    public void MoveBlock()
    {
        if (isMove) return;

        isMove = true;

        isPut = false;

        PutBlock();
    }

    private int GenerateRandomNumber(int[] exclusionList)
    {
        int randomValue = Random.Range(0, 26);
        while (exclusionList.Contains(randomValue))
        {
            randomValue = Random.Range(0, 26);
        }
        return randomValue;
    }

    public int GetValue(BlockType type)
    {
        int number = 0;

        for(int i = 0; i< otherBlockContentList.Count; i ++)
        {
            if(otherBlockContentList[i].Equals(type))
            {
                number = int.Parse(otherBlockContentList[i].value);
                break;
            }
        }

        return number;
    }
}
