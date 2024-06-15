using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PieceInfo
{
    public List<Piece> pieceList_N = new List<Piece>();
    public List<Piece> pieceList_R = new List<Piece>();
    public List<Piece> pieceList_SR = new List<Piece>();
    public List<Piece> pieceList_SSR = new List<Piece>();
    public List<Piece> pieceList_UR = new List<Piece>();

    int[] piece = new int[5];

    public void Initialize()
    {
        //pieceList_N.Clear();
        //pieceList_R.Clear();
        //pieceList_SR.Clear();
        //pieceList_SSR.Clear();
        //pieceList_UR.Clear();

        //for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i ++)
        //{
        //    Piece piece = new Piece();
        //    piece.blockType = BlockType.Default + 1 + i;
        //    piece.rankType = RankType.N;
        //    pieceList_N.Add(piece);
        //}

        //for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        //{
        //    Piece piece = new Piece();
        //    piece.blockType = BlockType.Default + 1 + i;
        //    piece.rankType = RankType.R;
        //    pieceList_R.Add(piece);
        //}

        //for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        //{
        //    Piece piece = new Piece();
        //    piece.blockType = BlockType.Default + 1 + i;
        //    piece.rankType = RankType.SR;
        //    pieceList_SR.Add(piece);
        //}

        //for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        //{
        //    Piece piece = new Piece();
        //    piece.blockType = BlockType.Default + 1 + i;
        //    piece.rankType = RankType.UR;
        //    pieceList_UR.Add(piece);
        //}

        //for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        //{
        //    Piece piece = new Piece();
        //    piece.blockType = BlockType.Default + 1 + i;
        //    piece.rankType = RankType.SSR;
        //    pieceList_SSR.Add(piece);
        //}

        for(int i = 0; i < pieceList_N.Count; i ++)
        {
            pieceList_N[i].Initialize();
        }

        for (int i = 0; i < pieceList_R.Count; i++)
        {
            pieceList_R[i].Initialize();
        }

        for (int i = 0; i < pieceList_SR.Count; i++)
        {
            pieceList_SR[i].Initialize();
        }

        for (int i = 0; i < pieceList_SSR.Count; i++)
        {
            pieceList_SSR[i].Initialize();
        }

        for (int i = 0; i < pieceList_UR.Count; i++)
        {
            pieceList_UR[i].Initialize();
        }

        Debug.LogError("조각 데이터베이스 초기화 완료");
    }

    public void SaveServerData(PieceInfo pieceInfo)
    {
        for (int i = 0; i < pieceInfo.pieceList_N.Count; i++)
        {
            if (i >= pieceList_N.Count)
            {
                pieceList_N.Add(pieceInfo.pieceList_N[i]);
            }
            else
            {
                pieceList_N[i] = pieceInfo.pieceList_N[i];
            }
        }

        for (int i = 0; i < pieceInfo.pieceList_R.Count; i++)
        {
            if (i >= pieceList_R.Count)
            {
                pieceList_R.Add(pieceInfo.pieceList_R[i]);
            }
            else
            {
                pieceList_R[i] = pieceInfo.pieceList_R[i];
            }
        }

        for (int i = 0; i < pieceInfo.pieceList_SR.Count; i++)
        {
            if (i >= pieceList_SR.Count)
            {
                pieceList_SR.Add(pieceInfo.pieceList_SR[i]);
            }
            else
            {
                pieceList_SR[i] = pieceInfo.pieceList_SR[i];
            }
        }

        for (int i = 0; i < pieceInfo.pieceList_SSR.Count; i++)
        {
            if (i >= pieceList_SSR.Count)
            {
                pieceList_SSR.Add(pieceInfo.pieceList_SSR[i]);
            }
            else
            {
                pieceList_SSR[i] = pieceInfo.pieceList_SSR[i];
            }
        }

        for (int i = 0; i < pieceInfo.pieceList_UR.Count; i++)
        {
            if (i >= pieceList_UR.Count)
            {
                pieceList_UR.Add(pieceInfo.pieceList_UR[i]);
            }
            else
            {
                pieceList_UR[i] = pieceInfo.pieceList_UR[i];
            }
        }
    }

    public void AddPiece(BlockType blockType, RankType rankType, int number)
    {
        Debug.Log(blockType + " / " + rankType + " / " + (number + 1) + "번째 조각 획득");

        switch (rankType)
        {
            case RankType.N:
                for(int i = 0; i < pieceList_N.Count; i ++)
                {
                    if(pieceList_N[i].blockType.Equals(blockType))
                    {
                        pieceList_N[i].PlusPiece(number);
                    }
                }
                break;
            case RankType.R:
                for (int i = 0; i < pieceList_R.Count; i++)
                {
                    if (pieceList_R[i].blockType.Equals(blockType))
                    {
                        pieceList_R[i].PlusPiece(number);
                    }
                }
                break;
            case RankType.SR:
                for (int i = 0; i < pieceList_SR.Count; i++)
                {
                    if (pieceList_SR[i].blockType.Equals(blockType))
                    {
                        pieceList_SR[i].PlusPiece(number);
                    }
                }
                break;
            case RankType.SSR:
                for (int i = 0; i < pieceList_SSR.Count; i++)
                {
                    if (pieceList_SSR[i].blockType.Equals(blockType))
                    {
                        pieceList_SSR[i].PlusPiece(number);
                    }
                }
                break;
            case RankType.UR:
                for (int i = 0; i < pieceList_UR.Count; i++)
                {
                    if (pieceList_UR[i].blockType.Equals(blockType))
                    {
                        pieceList_UR[i].PlusPiece(number);
                    }
                }
                break;
        }
    }

    public void MinusPiece(BlockType blockType, RankType rankType)
    {
        switch (rankType)
        {
            case RankType.N:
                for (int i = 0; i < pieceList_N.Count; i++)
                {
                    if (pieceList_N[i].blockType.Equals(blockType))
                    {
                        pieceList_N[i].MinusPiece();
                        break;
                    }
                }
                break;
            case RankType.R:
                for (int i = 0; i < pieceList_R.Count; i++)
                {
                    if (pieceList_R[i].blockType.Equals(blockType))
                    {
                        pieceList_R[i].MinusPiece();
                        break;
                    }
                }
                break;
            case RankType.SR:
                for (int i = 0; i < pieceList_SR.Count; i++)
                {
                    if (pieceList_SR[i].blockType.Equals(blockType))
                    {
                        pieceList_SR[i].MinusPiece();
                        break;
                    }
                }
                break;
            case RankType.SSR:
                for (int i = 0; i < pieceList_SSR.Count; i++)
                {
                    if (pieceList_SSR[i].blockType.Equals(blockType))
                    {
                        pieceList_SSR[i].MinusPiece();
                        break;
                    }
                }
                break;
            case RankType.UR:
                for (int i = 0; i < pieceList_UR.Count; i++)
                {
                    if (pieceList_UR[i].blockType.Equals(blockType))
                    {
                        pieceList_UR[i].MinusPiece();
                        break;
                    }
                }
                break;
        }
    }

    public int[] GetPiece(BlockType blockType, RankType rankType)
    {
        switch (rankType)
        {
            case RankType.N:
                for (int i = 0; i < pieceList_N.Count; i++)
                {
                    if (pieceList_N[i].blockType.Equals(blockType))
                    {
                        piece = pieceList_N[i].GetPiece();
                        break;
                    }
                }
                break;
            case RankType.R:
                for (int i = 0; i < pieceList_R.Count; i++)
                {
                    if (pieceList_R[i].blockType.Equals(blockType))
                    {
                        piece = pieceList_R[i].GetPiece();
                        break;
                    }
                }
                break;
            case RankType.SR:
                for (int i = 0; i < pieceList_SR.Count; i++)
                {
                    if (pieceList_SR[i].blockType.Equals(blockType))
                    {
                        piece = pieceList_SR[i].GetPiece();
                        break;
                    }
                }
                break;
            case RankType.SSR:
                for (int i = 0; i < pieceList_SSR.Count; i++)
                {
                    if (pieceList_SSR[i].blockType.Equals(blockType))
                    {
                        piece = pieceList_SSR[i].GetPiece();
                        break;
                    }
                }
                break;
            case RankType.UR:
                for (int i = 0; i < pieceList_UR.Count; i++)
                {
                    if (pieceList_UR[i].blockType.Equals(blockType))
                    {
                        piece = pieceList_UR[i].GetPiece();
                        break;
                    }
                }
                break;
        }

        return piece;
    }
}

[System.Serializable]
public class Piece
{
    public BlockType blockType = BlockType.Default;
    public RankType rankType = RankType.N;
    public int index1 = 0;
    public int index2 = 0;
    public int index3 = 0;
    public int index4 = 0;
    public int index5 = 0;

    int[] piece = new int[5];

    public void Initialize()
    {
        index1 = 0;
        index2 = 0;
        index3 = 0;
        index4 = 0;
        index5 = 0;
    }

    public void PlusPiece(int number)
    {
        switch(number)
        {
            case 0:
                index1 += 1;
                break;
            case 1:
                index2 += 1;
                break;
            case 2:
                index3 += 1;
                break;
            case 3:
                index4 += 1;
                break;
            case 4:
                index5 += 1;
                break;
        }
    }

    public void MinusPiece()
    {
        if (index1 > 0) index1 -= 1;
        if (index2 > 0) index2 -= 1;
        if (index3 > 0) index3 -= 1;
        if (index4 > 0) index4 -= 1;
        if (index5 > 0) index5 -= 1;
    }

    public int[] GetPiece()
    {
        piece[0] = index1;
        piece[1] = index2;
        piece[2] = index3;
        piece[3] = index4;
        piece[4] = index5;

        return piece;
    }
}


public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryView;

    public GameObject changeBoxView;

    public GameObject mainAlarm;

    [Title("TopMenu")]
    public Image[] topMenuImgArray;
    public Sprite[] topMenuSpriteArray;
    public GameObject[] scrollView;

    [Title("Exchange")]
    public Image[] topMenuImgArray2;
    public Sprite[] topMenuSpriteArray2;
    public GameObject[] scrollView2;
    public RectTransform[] grid2;

    public Text[] inventoryText;

    public Image pieceImg;
    public Text pieceText;

    public ReceiveContent receiveContent;

    public ExchangeContent exchangeContent;

    private List<ExchangeContent> exchangeContentList_N = new List<ExchangeContent>();
    private List<ExchangeContent> exchangeContentList_R = new List<ExchangeContent>();
    private List<ExchangeContent> exchangeContentList_SR = new List<ExchangeContent>();
    private List<ExchangeContent> exchangeContentList_SSR = new List<ExchangeContent>();
    private List<ExchangeContent> exchangeContentList_UR = new List<ExchangeContent>();

    private int boxIndex = 0;
    private int boxCount = 0;

    private int topNumber = -1;
    private int topNumber2 = -1;

    private int changeBoxCount = 10;

    bool isDelay = false;

    public TitleManager titleManager;
    public CollectionManager collectionManager;

    Sprite[] rankBackgroundArray;


    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rankBackgroundArray = imageDataBase.GetRankBackgroundArray();

        inventoryView.SetActive(false);
        changeBoxView.SetActive(false);

        mainAlarm.SetActive(true);

        for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        {
            ExchangeContent content = Instantiate(exchangeContent);
            content.transform.SetParent(grid2[0]);
            content.transform.position = Vector3.zero;
            content.transform.rotation = Quaternion.identity;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(true);

            exchangeContentList_N.Add(content);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        {
            ExchangeContent content = Instantiate(exchangeContent);
            content.transform.SetParent(grid2[1]);
            content.transform.position = Vector3.zero;
            content.transform.rotation = Quaternion.identity;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(true);

            exchangeContentList_R.Add(content);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        {
            ExchangeContent content = Instantiate(exchangeContent);
            content.transform.SetParent(grid2[2]);
            content.transform.position = Vector3.zero;
            content.transform.rotation = Quaternion.identity;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(true);

            exchangeContentList_SR.Add(content);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        {
            ExchangeContent content = Instantiate(exchangeContent);
            content.transform.SetParent(grid2[3]);
            content.transform.position = Vector3.zero;
            content.transform.rotation = Quaternion.identity;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(true);

            exchangeContentList_SSR.Add(content);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length - 1; i++)
        {
            ExchangeContent content = Instantiate(exchangeContent);
            content.transform.SetParent(grid2[4]);
            content.transform.position = Vector3.zero;
            content.transform.rotation = Quaternion.identity;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(true);

            exchangeContentList_UR.Add(content);
        }


        for (int i = 0; i < grid2.Length; i++)
        {
            grid2[i].anchoredPosition = new Vector2(0, -9999);
        }
    }


    public void OpenInventoryView()
    {
        if (!inventoryView.activeInHierarchy)
        {
            inventoryView.SetActive(true);

            mainAlarm.SetActive(false);

            if (topNumber == -1)
            {
                ChangeTopMenu(0);
            }

            FirebaseAnalytics.LogEvent("Open_Inventory");
        }
        else
        {
            inventoryView.SetActive(false);
        }
    }

    public void ChangeTopMenu(int number)
    {
        if (topNumber == number) return;

        topNumber = number;

        for (int i = 0; i < topMenuImgArray.Length; i++)
        {
            topMenuImgArray[i].sprite = topMenuSpriteArray[0];
            scrollView[i].SetActive(false);
        }

        topMenuImgArray[number].sprite = topMenuSpriteArray[1];
        scrollView[number].SetActive(true);

        switch(number)
        {
            case 0:
                if(topNumber2 == -1)
                {
                    ChangeTopMenu2(0);
                }

                FirebaseAnalytics.LogEvent("Open_Exchange");

                break;
            case 1:
                Exchange_Speical_Initialize();

                FirebaseAnalytics.LogEvent("Open_Exchange_Speical");
                break;
        }
    }

    public void ChangeTopMenu2(int number)
    {
        if (topNumber2 == number) return;

        topNumber2 = number;

        for (int i = 0; i < topMenuImgArray2.Length; i++)
        {
            topMenuImgArray2[i].sprite = topMenuSpriteArray[0];
            scrollView2[i].SetActive(false);
        }

        topMenuImgArray2[number].sprite = topMenuSpriteArray[1];
        scrollView2[number].SetActive(true);

        switch(number)
        {
            case 0:
                Exchange_Initialize(RankType.N);
                break;
            case 1:
                Exchange_Initialize(RankType.R);
                break;
            case 2:
                Exchange_Initialize(RankType.SR);
                break;
            case 3:
                Exchange_Initialize(RankType.SSR);
                break;
            case 4:
                Exchange_Initialize(RankType.UR);
                break;

        }
    }

    void Exchange_Speical_Initialize()
    {
        inventoryText[0].text = playerDataBase.GetUpgradeTicket(RankType.N).ToString();
        inventoryText[1].text = playerDataBase.BoxPiece_N.ToString() + "/" + changeBoxCount;
        inventoryText[2].text = playerDataBase.BoxPiece_R.ToString() + "/" + changeBoxCount;
        inventoryText[3].text = playerDataBase.BoxPiece_SR.ToString() + "/" + changeBoxCount;
        inventoryText[4].text = playerDataBase.BoxPiece_SSR.ToString() + "/" + changeBoxCount;
        inventoryText[5].text = playerDataBase.BoxPiece_UR.ToString() + "/" + changeBoxCount;

        titleManager.CheckGoal();
    }

    void Exchange_Initialize(RankType rankType)
    {
        switch (rankType)
        {
            case RankType.N:
                for (int i = 0; i < exchangeContentList_N.Count; i++)
                {
                    if (exchangeContentList_N[i].gameObject.activeSelf)
                    {
                        exchangeContentList_N[i].Initialize(BlockType.Default + 1 + i, rankType);
                    }
                }
                break;
            case RankType.R:
                for (int i = 0; i < exchangeContentList_R.Count; i++)
                {
                    if (exchangeContentList_R[i].gameObject.activeSelf)
                    {
                        exchangeContentList_R[i].Initialize(BlockType.Default + 1 + i, rankType);
                    }
                }
                break;
            case RankType.SR:
                for (int i = 0; i < exchangeContentList_SR.Count; i++)
                {
                    if (exchangeContentList_SR[i].gameObject.activeSelf)
                    {
                        exchangeContentList_SR[i].Initialize(BlockType.Default + 1 + i, rankType);
                    }
                }
                break;
            case RankType.SSR:
                for (int i = 0; i < exchangeContentList_SSR.Count; i++)
                {
                    if (exchangeContentList_SSR[i].gameObject.activeSelf)
                    {
                        exchangeContentList_SSR[i].Initialize(BlockType.Default + 1 + i, rankType);
                    }
                }
                break;
            case RankType.UR:
                for (int i = 0; i < exchangeContentList_UR.Count; i++)
                {
                    if (exchangeContentList_UR[i].gameObject.activeSelf)
                    {
                        exchangeContentList_UR[i].Initialize(BlockType.Default + 1 + i, rankType);
                    }
                }
                break;
        }
    }

    public void ChangeBox(int number)
    {
        if (isDelay) return;

        switch(number)
        {
            case 0:
                if(playerDataBase.BoxPiece_N < changeBoxCount)
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.LowPiece);
                    return;
                }

                break;
            case 1:
                if (playerDataBase.BoxPiece_R < changeBoxCount)
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.LowPiece);
                    return;
                }

                break;
            case 2:
                if (playerDataBase.BoxPiece_SR < changeBoxCount)
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.LowPiece);
                    return;
                }

                break;
            case 3:
                if (playerDataBase.BoxPiece_SSR < changeBoxCount)
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.LowPiece);
                    return;
                }

                break;
            case 4:
                if (playerDataBase.BoxPiece_UR < changeBoxCount)
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.LowPiece);
                    return;
                }

                break;
        }

        ChangeBoxInitialize(number);
    }

    void ChangeBoxInitialize(int number)
    {
        boxIndex = number;

        changeBoxView.SetActive(true);

        pieceImg.sprite = rankBackgroundArray[number];

        boxCount = 0;

        CountInitialize(boxCount);

        FirebaseAnalytics.LogEvent("ExChangeBox : " + number);
    }

    public void CloseChangeBoxView()
    {
        changeBoxView.SetActive(false);
    }

    void CountInitialize(int number)
    {
        //switch (boxIndex)
        //{
        //    case 0:
        //        pieceText.text = playerDataBase.BoxPiece_N + "/" + (changeBoxCount * (number + 1));

        //        receiveContent.Initialize(RewardType.Box_N, number + 1);

        //        break;
        //    case 1:
        //        pieceText.text = playerDataBase.BoxPiece_R + "/" + (changeBoxCount * (number + 1));

        //        receiveContent.Initialize(RewardType.Box_R, number + 1);
        //        break;
        //    case 2:
        //        pieceText.text = playerDataBase.BoxPiece_SR + "/" + (changeBoxCount * (number + 1));

        //        receiveContent.Initialize(RewardType.Box_SR, number + 1);
        //        break;
        //    case 3:
        //        pieceText.text = playerDataBase.BoxPiece_SSR + "/" + (changeBoxCount * (number + 1));

        //        receiveContent.Initialize(RewardType.Box_SSR, number + 1);
        //        break;
        //    case 4:
        //        pieceText.text = playerDataBase.BoxPiece_UR + "/" + (changeBoxCount * (number + 1));

        //        receiveContent.Initialize(RewardType.Box_UR, number + 1);
        //        break;
        //}
    }


    public void CountUp()
    {
        switch (boxIndex)
        {
            case 0:
                if(playerDataBase.BoxPiece_N < changeBoxCount * (boxCount + 2))
                {
                    return;
                }

                break;
            case 1:
                if (playerDataBase.BoxPiece_R < changeBoxCount * (boxCount + 2))
                {
                    return;
                }
                break;
            case 2:
                if (playerDataBase.BoxPiece_SR < changeBoxCount * (boxCount + 2))
                {
                    return;
                }
                break;
            case 3:
                if (playerDataBase.BoxPiece_SSR < changeBoxCount * (boxCount + 2))
                {
                    return;
                }
                break;
            case 4:
                if (playerDataBase.BoxPiece_UR < changeBoxCount * (boxCount + 2))
                {
                    return;
                }
                break;
        }

        boxCount++;

        CountInitialize(boxCount);
    }

    public void CountDown()
    {
        if (boxCount <= 0) return;

        boxCount--;

        CountInitialize(boxCount);
    }


    //public void ChangeBox()
    //{
    //    if (!NetworkConnect.instance.CheckConnectInternet())
    //    {
    //        SoundManager.instance.PlaySFX(GameSfxType.Wrong);

    //        NotionManager.instance.UseNotion(NotionType.CheckInternet);
    //        return;
    //    }

    //    switch (boxIndex)
    //    {
    //        case 0:
    //            playerDataBase.BoxPiece_N -= (changeBoxCount * (boxCount + 1));

    //            PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_N", playerDataBase.BoxPiece_N);

    //            switch (GameStateManager.instance.WindCharacterType)
    //            {
    //                case WindCharacterType.Winter:
    //                    playerDataBase.SnowBox_N = boxCount + 1;
    //                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_N", boxCount + 1);
    //                    break;
    //                case WindCharacterType.UnderWorld:
    //                    playerDataBase.UnderworldBox_N = boxCount + 1;
    //                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_N", boxCount + 1);
    //                    break;
    //            }
    //            break;
    //        case 1:
    //            playerDataBase.BoxPiece_R -= (changeBoxCount * (boxCount + 1));

    //            PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_R", playerDataBase.BoxPiece_R);

    //            switch (GameStateManager.instance.WindCharacterType)
    //            {
    //                case WindCharacterType.Winter:
    //                    playerDataBase.SnowBox_R = boxCount + 1;
    //                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_R", boxCount + 1);
    //                    break;
    //                case WindCharacterType.UnderWorld:
    //                    playerDataBase.UnderworldBox_R = boxCount + 1;
    //                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_R", boxCount + 1);
    //                    break;
    //            }
    //            break;
    //        case 2:
    //            playerDataBase.BoxPiece_SR -= (changeBoxCount * (boxCount + 1));

    //            PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_SR", playerDataBase.BoxPiece_SR);

    //            switch (GameStateManager.instance.WindCharacterType)
    //            {
    //                case WindCharacterType.Winter:
    //                    playerDataBase.SnowBox_SR = boxCount + 1;
    //                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SR", boxCount + 1);
    //                    break;
    //                case WindCharacterType.UnderWorld:
    //                    playerDataBase.UnderworldBox_SR = boxCount + 1;
    //                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SR", boxCount + 1);
    //                    break;
    //            }

    //            break;
    //        case 3:
    //            playerDataBase.BoxPiece_SSR -= (changeBoxCount * (boxCount + 1));

    //            PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_SSR", playerDataBase.BoxPiece_SSR);

    //            switch (GameStateManager.instance.WindCharacterType)
    //            {
    //                case WindCharacterType.Winter:
    //                    playerDataBase.SnowBox_SSR = boxCount + 1;
    //                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SSR", boxCount + 1);
    //                    break;
    //                case WindCharacterType.UnderWorld:
    //                    playerDataBase.UnderworldBox_SSR = boxCount + 1;
    //                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SSR", boxCount + 1);
    //                    break;
    //            }

    //            break;
    //        case 4:
    //            playerDataBase.BoxPiece_UR -= (changeBoxCount * (boxCount + 1));

    //            PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_UR", playerDataBase.BoxPiece_UR);

    //            switch (GameStateManager.instance.WindCharacterType)
    //            {
    //                case WindCharacterType.Winter:
    //                    playerDataBase.SnowBox_UR = boxCount + 1;
    //                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_UR", boxCount + 1);
    //                    break;
    //                case WindCharacterType.UnderWorld:
    //                    playerDataBase.UnderworldBox_UR = boxCount + 1;
    //                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_UR", boxCount + 1);
    //                    break;
    //            }

    //            break;
    //    }

    //    SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
    //    NotionManager.instance.UseNotion(NotionType.GetReward);

    //    Exchange_Speical_Initialize();

    //    CloseChangeBoxView();

    //    playerDataBase.RepairBlockCount += boxCount + 1;
    //    PlayfabManager.instance.UpdatePlayerStatisticsInsert("RepairBlockCount", playerDataBase.RepairBlockCount);

    //    collectionManager.mainAlarm.SetActive(true);

    //    isDelay = true;
    //    Invoke("Delay", 1.0f);
    //}

    void Delay()
    {
        isDelay = false;
    }
}
