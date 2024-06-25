using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int needPiece = 0;
    public int index1 = 0;
    public int index2 = 0;
    public int index3 = 0;
    public int index4 = 0;
    public int index5 = 0;

    int[] piece = new int[5];
    bool check = false;

    public void Initialize()
    {
        index1 = 0;
        index2 = 0;
        index3 = 0;
        index4 = 0;
        index5 = 0;

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

    public bool CheckingFusion()
    {
        check = false;

        switch(needPiece)
        {
            case 3:
                if(index1 > 0 && index2 > 0 && index3 > 0)
                {
                    check = true;
                }
                break;
            case 4:
                if (index1 > 0 && index2 > 0 && index3 > 0 && index4 > 0)
                {
                    check = true;
                }
                break;
            case 5:
                if (index1 > 0 && index2 > 0 && index3 > 0 && index4 > 0 && index5 > 0)
                {
                    check = true;
                }
                break;
        }

        return check;
    }
}


public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryView;

    public GameObject changeBoxView;

    public GameObject successView;
    public BlockUIContent successBlockUIContent;
    public GameObject successContinue;

    public GameObject mainAlarm;
    public GameObject[] topAlarm;

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

    private List<ExchangeContent> exchangeContentList_N_Sort = new List<ExchangeContent>();
    private List<ExchangeContent> exchangeContentList_R_Sort = new List<ExchangeContent>();
    private List<ExchangeContent> exchangeContentList_SR_Sort = new List<ExchangeContent>();
    private List<ExchangeContent> exchangeContentList_SSR_Sort = new List<ExchangeContent>();
    private List<ExchangeContent> exchangeContentList_UR_Sort = new List<ExchangeContent>();


    private List<int> exchangeContentList_N_List = new List<int>();
    private List<int> exchangeContentList_R_List = new List<int>();
    private List<int> exchangeContentList_SR_List = new List<int>();
    private List<int> exchangeContentList_SSR_List = new List<int>();
    private List<int> exchangeContentList_UR_List = new List<int>();

    private int boxIndex = 0;
    private int boxCount = 0;

    private int topNumber = -1;
    private int topNumber2 = -1;

    private int changeBoxCount = 10;

    bool isDelay = false;
    private bool isSuccessDelay = false;

    private bool init1 = false;
    private bool init2 = false;
    private bool init3 = false;
    private bool init4 = false;
    private bool init5 = false;

    public TitleManager titleManager;
    public CollectionManager collectionManager;


    WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);

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
        successView.SetActive(false);

        mainAlarm.SetActive(false);

        for(int i = 0; i < topAlarm.Length; i ++)
        {
            topAlarm[i].SetActive(false);
        }

        init1 = false;
        init2 = false;
        init3 = false;
        init4 = false;
        init5 = false;

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

            CheckingFusion();

            if (topNumber == -1)
            {
                ChangeTopMenu(0);
            }
            else
            {
                grid2[topNumber].anchoredPosition = new Vector2(0, -9999);
            }

            FirebaseAnalytics.LogEvent("Open_Inventory");
        }
        else
        {
            inventoryView.SetActive(false);
        }
    }

    public void CloseInventoryView()
    {
        inventoryView.SetActive(false);
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

        topAlarm[number].SetActive(false);

        switch (number)
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

        switch (number)
        {
            case 0:
                if (!init1)
                {
                    init1 = true;

                    for (int i = 0; i < exchangeContentList_N.Count; i++)
                    {
                        exchangeContentList_N[i].Initialize(BlockType.Default + 1 + i, RankType.N, this);

                        if (exchangeContentList_N[i].windCharacterType != GameStateManager.instance.WindCharacterType || exchangeContentList_N[i].isNone)
                        {
                            exchangeContentList_N[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            exchangeContentList_N_Sort.Add(exchangeContentList_N[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < exchangeContentList_N_Sort.Count; i++)
                    {
                        exchangeContentList_N_Sort[i].Initialize();
                    }
                }

                //exchangeContentList_N_Sort.Clear();
                //exchangeContentList_N_List.Clear();

                //for (int i = 0; i < exchangeContentList_N.Count; i++)
                //{
                //    if (exchangeContentList_N[i].windCharacterType == GameStateManager.instance.WindCharacterType && !exchangeContentList_N[i].isNone)
                //    {
                //        exchangeContentList_N_Sort.Add(exchangeContentList_N[i]);
                //        exchangeContentList_N_List.Add(i);
                //    }
                //}

                //exchangeContentList_N_Sort = exchangeContentList_N_Sort.OrderByDescending(x => x.blockType).OrderByDescending(x => x.isActive).ToList();

                //for (int i = 0; i < exchangeContentList_N_Sort.Count; i++)
                //{
                //    exchangeContentList_N[exchangeContentList_N_List[i]].Initialize_Sort(exchangeContentList_N_Sort[i].blockType, RankType.N);
                //}
                break;
            case 1:
                if (!init2)
                {
                    init2 = true;

                    for (int i = 0; i < exchangeContentList_R.Count; i++)
                    {
                        exchangeContentList_R[i].Initialize(BlockType.Default + 1 + i, RankType.R, this);

                        if (exchangeContentList_R[i].windCharacterType != GameStateManager.instance.WindCharacterType || exchangeContentList_R[i].isNone)
                        {
                            exchangeContentList_R[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            exchangeContentList_R_Sort.Add(exchangeContentList_R[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < exchangeContentList_R_Sort.Count; i++)
                    {
                        exchangeContentList_R_Sort[i].Initialize();
                    }
                }

                break;
            case 2:
                if (!init3)
                {
                    init3 = true;

                    for (int i = 0; i < exchangeContentList_SR.Count; i++)
                    {
                        exchangeContentList_SR[i].Initialize(BlockType.Default + 1 + i, RankType.SR, this);

                        if (exchangeContentList_SR[i].windCharacterType != GameStateManager.instance.WindCharacterType || exchangeContentList_SR[i].isNone)
                        {
                            exchangeContentList_SR[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            exchangeContentList_SR_Sort.Add(exchangeContentList_SR[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < exchangeContentList_SR_Sort.Count; i++)
                    {
                        exchangeContentList_SR_Sort[i].Initialize();
                    }
                }

                break;
            case 3:
                if (!init4)
                {
                    init4 = true;

                    for (int i = 0; i < exchangeContentList_SSR.Count; i++)
                    {
                        exchangeContentList_SSR[i].Initialize(BlockType.Default + 1 + i, RankType.SSR, this);

                        if (exchangeContentList_SSR[i].windCharacterType != GameStateManager.instance.WindCharacterType || exchangeContentList_SSR[i].isNone)
                        {
                            exchangeContentList_SSR[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            exchangeContentList_SSR_Sort.Add(exchangeContentList_SSR[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < exchangeContentList_SSR_Sort.Count; i++)
                    {
                        exchangeContentList_SSR_Sort[i].Initialize();
                    }
                }

                break;
            case 4:
                if (!init5)
                {
                    init5 = true;

                    for (int i = 0; i < exchangeContentList_UR.Count; i++)
                    {
                        exchangeContentList_UR[i].Initialize(BlockType.Default + 1 + i, RankType.UR, this);

                        if (exchangeContentList_UR[i].windCharacterType != GameStateManager.instance.WindCharacterType || exchangeContentList_UR[i].isNone)
                        {
                            exchangeContentList_UR[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            exchangeContentList_UR_Sort.Add(exchangeContentList_UR[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < exchangeContentList_UR_Sort.Count; i++)
                    {
                        exchangeContentList_UR_Sort[i].Initialize();
                    }
                }

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

    public void SuccessFusion(BlockType blockType,RankType rankType)
    {
        isSuccessDelay = true;

        successView.SetActive(true);

        successContinue.SetActive(false);

        successBlockUIContent.Initialize(blockType);
        successBlockUIContent.Initialize_Rank(rankType);

        Invoke("SuccessDelay", 1.0f);
    }

    void SuccessDelay()
    {
        isSuccessDelay = false;

        successContinue.SetActive(true);
    }

    public void CloseFusion()
    {
        if (isSuccessDelay) return;

        successView.SetActive(false);
    }
    
    public void CheckingFusion() //합성 가능한게 있는지 확인하기
    {
        for(int i = 0; i < playerDataBase.PieceInfo.pieceList_N.Count; i ++)
        {
            if(playerDataBase.PieceInfo.pieceList_N[i].CheckingFusion())
            {
                mainAlarm.SetActive(true);
                topAlarm[0].SetActive(true);

                Debug.Log("합성 가능한 N 등급이 있습니다");
                break;
            }
        }

        for (int i = 0; i < playerDataBase.PieceInfo.pieceList_R.Count; i++)
        {
            if (playerDataBase.PieceInfo.pieceList_R[i].CheckingFusion())
            {
                mainAlarm.SetActive(true);
                topAlarm[1].SetActive(true);

                Debug.Log("합성 가능한 R 등급이 있습니다");
                break;
            }
        }

        for (int i = 0; i < playerDataBase.PieceInfo.pieceList_SR.Count; i++)
        {
            if (playerDataBase.PieceInfo.pieceList_SR[i].CheckingFusion())
            {
                mainAlarm.SetActive(true);
                topAlarm[2].SetActive(true);

                Debug.Log("합성 가능한 SR 등급이 있습니다");
                break;
            }
        }

        for (int i = 0; i < playerDataBase.PieceInfo.pieceList_SSR.Count; i++)
        {
            if (playerDataBase.PieceInfo.pieceList_SSR[i].CheckingFusion())
            {
                mainAlarm.SetActive(true);
                topAlarm[3].SetActive(true);

                Debug.Log("합성 가능한 SSR 등급이 있습니다");
                break;
            }
        }
    }
}
