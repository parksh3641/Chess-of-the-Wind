using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeContent : MonoBehaviour
{
    public BlockType blockType = BlockType.Default;
    public RankType rankType = RankType.N;
    public WindCharacterType windCharacterType = WindCharacterType.Winter;

    public Text titleText;

    public BlockUIContent[] blockUIContentArray;

    public Text[] blockHoldNumberText;

    public BlockUIContent blockUIContent_Reward;

    public GameObject lockedObj;

    private int needPiece = 0; //블럭마다 교환시 필요한 조각 개수가 3개 ~ 5개까지 차이가 있음

    public bool isActive = false;
    public bool isNone = false;

    public int[] piece = new int[5];

    List<string> itemList = new List<string>();

    private Dictionary<string, string> playerData = new Dictionary<string, string>();

    InventoryManager inventoryManager;

    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void Initialize(BlockType type, RankType type2, InventoryManager manager)
    {
        blockType = type;
        rankType = type2;
        inventoryManager = manager;

        titleText.text = LocalizationManager.instance.GetString(type.ToString());

        switch (blockType)
        {
            case BlockType.Default:
                break;
            case BlockType.RightQueen_2:
                windCharacterType = WindCharacterType.UnderWorld;
                break;
            case BlockType.LeftQueen_2:
                windCharacterType = WindCharacterType.Winter;
                break;
            case BlockType.RightQueen_3:
                windCharacterType = WindCharacterType.UnderWorld;
                break;
            case BlockType.LeftQueen_3:
                windCharacterType = WindCharacterType.Winter;
                break;
            case BlockType.RightNight:
                windCharacterType = WindCharacterType.UnderWorld;
                break;
            case BlockType.LeftNight:
                windCharacterType = WindCharacterType.Winter;
                break;
            case BlockType.RightDownNight:
                isNone = true;
                break;
            case BlockType.LeftDownNight:
                isNone = true;
                break;
            case BlockType.Rook_V2:
                windCharacterType = WindCharacterType.Winter;
                break;
            case BlockType.Pawn_Under:
                windCharacterType = WindCharacterType.UnderWorld;
                break;
            case BlockType.Pawn_Snow:
                windCharacterType = WindCharacterType.Winter;
                break;
            case BlockType.Rook_V4:
                windCharacterType = WindCharacterType.UnderWorld;
                break;
            case BlockType.RightNight_Mirror:
                windCharacterType = WindCharacterType.UnderWorld;
                break;
            case BlockType.LeftNight_Mirror:
                windCharacterType = WindCharacterType.Winter;
                break;
            case BlockType.Rook_V4_2:
                windCharacterType = WindCharacterType.UnderWorld;
                break;
            case BlockType.Rook_V2_2:
                windCharacterType = WindCharacterType.Winter;
                break;
            case BlockType.Pawn_Under_2:
                windCharacterType = WindCharacterType.UnderWorld;
                break;
            case BlockType.Pawn_Snow_2:
                windCharacterType = WindCharacterType.Winter;
                break;
        }

        Initialize();
    }

    public void Initialize()
    {
        Initialize_UI(blockType, rankType);

        piece = playerDataBase.PieceInfo.GetPiece(blockType, rankType);

        for (int i = 0; i < needPiece; i++)
        {
            blockHoldNumberText[i].text = piece[i] + "/1";

            if (piece[i] == 0)
            {
                blockUIContentArray[i].lockedObj.SetActive(true);
            }
            else
            {
                blockUIContentArray[i].lockedObj.SetActive(false);
            }
        }

        lockedObj.SetActive(true);

        isActive = true;

        for (int i = 0; i < needPiece; i++)
        {
            if (piece[i] <= 0)
            {
                isActive = false;
            }
        }

        if (isActive)
        {
            lockedObj.SetActive(false);
        }
    }

    public void Initialize_UI(BlockType type, RankType type2)
    {
        blockType = type;
        rankType = type2;

        titleText.text = LocalizationManager.instance.GetString(type.ToString());

        switch (blockType)
        {
            case BlockType.Default:
                break;
            case BlockType.RightQueen_2:
                needPiece = 4;
                break;
            case BlockType.LeftQueen_2:
                needPiece = 4;
                break;
            case BlockType.RightQueen_3:
                needPiece = 3;
                break;
            case BlockType.LeftQueen_3:
                needPiece = 3;
                break;
            case BlockType.RightNight:
                needPiece = 3;
                break;
            case BlockType.LeftNight:
                needPiece = 3;
                break;
            case BlockType.RightDownNight:
                needPiece = 3;
                break;
            case BlockType.LeftDownNight:
                needPiece = 3;
                break;
            case BlockType.Rook_V2:
                needPiece = 4;
                break;
            case BlockType.Pawn_Under:
                needPiece = 5;
                break;
            case BlockType.Pawn_Snow:
                needPiece = 5;
                break;
            case BlockType.Rook_V4:
                needPiece = 4;
                break;
            case BlockType.RightNight_Mirror:
                needPiece = 3;
                break;
            case BlockType.LeftNight_Mirror:
                needPiece = 3;
                break;
            case BlockType.Rook_V4_2:
                needPiece = 4;
                break;
            case BlockType.Rook_V2_2:
                needPiece = 4;
                break;
            case BlockType.Pawn_Under_2:
                needPiece = 5;
                break;
            case BlockType.Pawn_Snow_2:
                needPiece = 5;
                break;
        }

        for (int i = 0; i < blockUIContentArray.Length; i++)
        {
            blockUIContentArray[i].gameObject.SetActive(false);
            blockHoldNumberText[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < needPiece; i++)
        {
            blockUIContentArray[i].gameObject.SetActive(true);
            blockUIContentArray[i].SetPieceLevel(i);
            blockHoldNumberText[i].gameObject.SetActive(true);

            blockUIContentArray[i].Initialize(blockType);
            blockUIContentArray[i].Initialize_Rank(rankType);
        }

        blockUIContent_Reward.Initialize(blockType);
        blockUIContent_Reward.Initialize_Rank(rankType);
    }

    public void Exchange()
    {
        if (lockedObj.activeInHierarchy) return;

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            NotionManager.instance.UseNotion(NotionType.CheckInternet);

            return;
        }

        playerDataBase.PieceInfo.MinusPiece(blockType, rankType);

        playerData.Clear();
        playerData.Add("PieceInfo", JsonUtility.ToJson(playerDataBase.PieceInfo));
        PlayfabManager.instance.SetPlayerData(playerData);

        itemList.Clear();
        itemList.Add(blockType + "_" + rankType);

        switch (windCharacterType)
        {
            case WindCharacterType.Winter:
                PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", itemList);
                break;
            case WindCharacterType.UnderWorld:
                PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", itemList);
                break;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        //NotionManager.instance.UseNotion(NotionType.ExchangeNotion);

        inventoryManager.SuccessFusion(blockType, rankType);

        Initialize(blockType, rankType, inventoryManager);

        Debug.Log("Exchange : " + blockType + "_" + rankType);

        FirebaseAnalytics.LogEvent("Exchange : " +  blockType + "_" + rankType);

        playerDataBase.SynthesisGetBlock += 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SynthesisGetBlock", playerDataBase.SynthesisGetBlock);
    }
}
