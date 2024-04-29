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

    private bool check = false;

    int[] piece = new int[5];

    List<string> itemList = new List<string>();

    private Dictionary<string, string> playerData = new Dictionary<string, string>();

    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void Initialize(BlockType type, RankType type2)
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

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter) gameObject.SetActive(false);
                break;
            case BlockType.LeftQueen_2:
                needPiece = 4;

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.UnderWorld) gameObject.SetActive(false);
                break;
            case BlockType.RightQueen_3:
                gameObject.SetActive(false);

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter) gameObject.SetActive(false);
                break;
            case BlockType.LeftQueen_3:
                gameObject.SetActive(false);

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.UnderWorld) gameObject.SetActive(false);
                break;
            case BlockType.RightNight:
                needPiece = 3;
                break;
            case BlockType.LeftNight:
                needPiece = 3;
                break;
            case BlockType.RightDownNight:
                gameObject.SetActive(false);

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter) gameObject.SetActive(false);
                break;
            case BlockType.LeftDownNight:
                gameObject.SetActive(false);

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.UnderWorld) gameObject.SetActive(false);
                break;
            case BlockType.Rook_V2:
                needPiece = 4;

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.UnderWorld) gameObject.SetActive(false);
                break;
            case BlockType.Rook_V2H2:
                gameObject.SetActive(false);
                break;
            case BlockType.Pawn_Under:
                needPiece = 5;

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter) gameObject.SetActive(false);
                break;
            case BlockType.Pawn_Snow:
                needPiece = 5;

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.UnderWorld) gameObject.SetActive(false);
                break;
            case BlockType.Spider:
                gameObject.SetActive(false);
                break;
            case BlockType.Rook_V4:
                needPiece = 4;

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter) gameObject.SetActive(false);
                break;
            case BlockType.Tetris_I_Hor:
                gameObject.SetActive(false);
                break;
            case BlockType.Tetris_T:
                gameObject.SetActive(false);
                break;
            case BlockType.Tetris_L:
                gameObject.SetActive(false);
                break;
            case BlockType.Tetris_J:
                gameObject.SetActive(false);
                break;
            case BlockType.Tetris_S:
                gameObject.SetActive(false);
                break;
            case BlockType.Tetris_Z:
                gameObject.SetActive(false);
                break;
            case BlockType.Tetris_Speical:
                gameObject.SetActive(false);
                break;
            case BlockType.RightNight_Mirror:
                needPiece = 3;

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter) gameObject.SetActive(false);
                break;
            case BlockType.LeftNight_Mirror:
                needPiece = 3;

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.UnderWorld) gameObject.SetActive(false);
                break;
            case BlockType.Rook_V4_2:
                needPiece = 4;

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter) gameObject.SetActive(false);
                break;
            case BlockType.Rook_V2_2:
                needPiece = 4;

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.UnderWorld) gameObject.SetActive(false);
                break;
            case BlockType.Pawn_Under_2:
                needPiece = 5;

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter) gameObject.SetActive(false);
                break;
            case BlockType.Pawn_Snow_2:
                needPiece = 5;

                if (GameStateManager.instance.WindCharacterType == WindCharacterType.UnderWorld) gameObject.SetActive(false);
                break;
        }

        for(int i = 0; i < blockUIContentArray.Length; i ++)
        {
            blockUIContentArray[i].gameObject.SetActive(false);
            blockHoldNumberText[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < needPiece; i ++)
        {
            blockUIContentArray[i].gameObject.SetActive(true);
            blockUIContentArray[i].SetPieceLevel(i + 1);
            blockHoldNumberText[i].gameObject.SetActive(true);

            blockUIContentArray[i].Initialize(blockType);
            blockUIContentArray[i].SetRank(rankType);
        }

        blockUIContent_Reward.Initialize(blockType);
        blockUIContent_Reward.SetRank(rankType);

        Initialize();
    }

    public void Initialize()
    {
        piece = playerDataBase.PieceInfo.GetPiece(blockType, rankType);

        for(int i = 0; i < needPiece; i ++)
        {
            blockHoldNumberText[i].text = piece[0] + "/1";
        }

        lockedObj.SetActive(true);

        check = true;

        for (int i = 0; i < needPiece; i ++)
        {
            if(piece[i] <= 0)
            {
                check = false;
            }
        }

        if(check)
        {
            lockedObj.SetActive(false);
        }
    }

    public void Exchange()
    {
        if (lockedObj.activeInHierarchy) return;

        playerDataBase.PieceInfo.MinusPiece(blockType, rankType);

        playerData.Clear();
        playerData.Add("PieceInfo", JsonUtility.ToJson(playerDataBase.PieceInfo));
        PlayfabManager.instance.SetPlayerData(playerData);


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
                break;
            case BlockType.LeftQueen_3:
                break;
            case BlockType.RightNight:
                windCharacterType = WindCharacterType.UnderWorld;
                break;
            case BlockType.LeftNight:
                windCharacterType = WindCharacterType.Winter;
                break;
            case BlockType.RightDownNight:
                break;
            case BlockType.LeftDownNight:
                break;
            case BlockType.Rook_V2:
                windCharacterType = WindCharacterType.Winter;
                break;
            case BlockType.Rook_V2H2:
                break;
            case BlockType.Pawn_Under:
                windCharacterType = WindCharacterType.UnderWorld;
                break;
            case BlockType.Pawn_Snow:
                windCharacterType = WindCharacterType.Winter;
                break;
            case BlockType.Spider:
                break;
            case BlockType.Rook_V4:
                windCharacterType = WindCharacterType.UnderWorld;
                break;
            case BlockType.Tetris_I_Hor:
                break;
            case BlockType.Tetris_T:
                break;
            case BlockType.Tetris_L:
                break;
            case BlockType.Tetris_J:
                break;
            case BlockType.Tetris_S:
                break;
            case BlockType.Tetris_Z:
                break;
            case BlockType.Tetris_Speical:
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
        NotionManager.instance.UseNotion(NotionType.ExchangeNotion);

        Initialize(blockType, rankType);

        FirebaseAnalytics.LogEvent("Exchange : " +  blockType + "_" + rankType);
    }

}
