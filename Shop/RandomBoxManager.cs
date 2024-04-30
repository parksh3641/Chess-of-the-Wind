using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RandomBoxInfo
{
    [Title("Normal")]
    public List<RandomBox> randomBox_Winter_Normal_List = new List<RandomBox>();
    public List<RandomBox> randomBox_Underworld_Normal_List = new List<RandomBox>();

    [Space]
    [Title("Epic")]
    public List<RandomBox> randomBox_Winter_Epic_List = new List<RandomBox>();
    public List<RandomBox> randomBox_Underworld_Epic_List = new List<RandomBox>();

    [Space]
    [Title("Speical")]
    public List<RandomBox> randomBox_Winter_Speical_List = new List<RandomBox>();
    public List<RandomBox> randomBox_Underworld_Speical_List = new List<RandomBox>();

    List<float> percent = new List<float>();
    List<int> confirm = new List<int>();

    private RandomBox_Block randomBox_Block = new RandomBox_Block();

    public List<float> GetPercent(BoxType boxType)
    {
        percent.Clear();

        switch (boxType)
        {
            case BoxType.Normal:
                if(GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    for (int i = 0; i < randomBox_Winter_Normal_List.Count; i ++)
                    {
                        percent.Add(randomBox_Underworld_Normal_List[i].percent);
                    }
                }
                else
                {
                    for (int i = 0; i < randomBox_Underworld_Normal_List.Count; i++)
                    {
                        percent.Add(randomBox_Underworld_Normal_List[i].percent);
                    }
                }
                break;
            case BoxType.N:
                break;
            case BoxType.R:
                break;
            case BoxType.SR:
                break;
            case BoxType.SSR:
                break;
            case BoxType.UR:
                break;
            case BoxType.NR:
                break;
            case BoxType.RSR:
                break;
            case BoxType.SRSSR:
                break;
            case BoxType.Epic:
                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    for (int i = 0; i < randomBox_Winter_Epic_List.Count; i++)
                    {
                        percent.Add(randomBox_Winter_Epic_List[i].percent);
                    }
                }
                else
                {
                    for (int i = 0; i < randomBox_Underworld_Epic_List.Count; i++)
                    {
                        percent.Add(randomBox_Underworld_Epic_List[i].percent);
                    }
                }
                break;
            case BoxType.Speical:
                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    for (int i = 0; i < randomBox_Winter_Speical_List.Count; i++)
                    {
                        percent.Add(randomBox_Winter_Speical_List[i].percent);
                    }
                }
                else
                {
                    for (int i = 0; i < randomBox_Underworld_Speical_List.Count; i++)
                    {
                        percent.Add(randomBox_Underworld_Speical_List[i].percent);
                    }
                }
                break;
        }

        return percent;
    }

    public List<int> GetConfrim(BoxType boxType, RankType rankType) //확정 조각
    {
        confirm.Clear();

        switch (boxType)
        {
            case BoxType.Normal:
                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    for (int i = 0; i < randomBox_Winter_Normal_List.Count; i++)
                    {
                        if(randomBox_Winter_Normal_List[i].boxInfoType.ToString().Contains(rankType.ToString()))
                        {
                            confirm.Add((int)randomBox_Winter_Normal_List[i].boxInfoType);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < randomBox_Underworld_Normal_List.Count; i++)
                    {
                        if (randomBox_Underworld_Normal_List[i].boxInfoType.ToString().Contains(rankType.ToString()))
                        {
                            confirm.Add((int)randomBox_Underworld_Normal_List[i].boxInfoType);
                        }
                    }
                }
                break;
            case BoxType.N:
                break;
            case BoxType.R:
                break;
            case BoxType.SR:
                break;
            case BoxType.SSR:
                break;
            case BoxType.UR:
                break;
            case BoxType.NR:
                break;
            case BoxType.RSR:
                break;
            case BoxType.SRSSR:
                break;
            case BoxType.Epic:
                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    for (int i = 0; i < randomBox_Winter_Epic_List.Count; i++)
                    {
                        if (randomBox_Winter_Epic_List[i].boxInfoType.ToString().Contains(rankType.ToString()))
                        {
                            confirm.Add((int)randomBox_Winter_Epic_List[i].boxInfoType);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < randomBox_Underworld_Epic_List.Count; i++)
                    {
                        if (randomBox_Underworld_Epic_List[i].boxInfoType.ToString().Contains(rankType.ToString()))
                        {
                            confirm.Add((int)randomBox_Underworld_Epic_List[i].boxInfoType);
                        }
                    }
                }
                break;
            case BoxType.Speical:
                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    for (int i = 0; i < randomBox_Winter_Speical_List.Count; i++)
                    {
                        if (randomBox_Winter_Speical_List[i].boxInfoType.ToString().Contains(rankType.ToString()))
                        {
                            confirm.Add((int)randomBox_Winter_Speical_List[i].boxInfoType);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < randomBox_Underworld_Speical_List.Count; i++)
                    {
                        if (randomBox_Underworld_Speical_List[i].boxInfoType.ToString().Contains(rankType.ToString()))
                        {
                            confirm.Add((int)randomBox_Underworld_Speical_List[i].boxInfoType);
                        }
                    }
                }
                break;
        }

        return confirm;
    }

    public void Initialize()
    {
        randomBox_Winter_Normal_List.Clear();
        randomBox_Underworld_Normal_List.Clear();
        randomBox_Winter_Epic_List.Clear();
        randomBox_Underworld_Epic_List.Clear();
        randomBox_Winter_Speical_List.Clear();
        randomBox_Underworld_Speical_List.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(BoxInfoType)).Length - 1; i ++)
        {
            RandomBox randomBox1 = new RandomBox();
            RandomBox randomBox2 = new RandomBox();
            RandomBox randomBox3 = new RandomBox();
            RandomBox randomBox4 = new RandomBox();
            RandomBox randomBox5 = new RandomBox();
            RandomBox randomBox6 = new RandomBox();

            randomBox_Winter_Normal_List.Add(randomBox1);
            randomBox_Underworld_Normal_List.Add(randomBox2);
            randomBox_Winter_Epic_List.Add(randomBox3);
            randomBox_Underworld_Epic_List.Add(randomBox4);
            randomBox_Winter_Speical_List.Add(randomBox5);
            randomBox_Underworld_Speical_List.Add(randomBox6);
        }

        if(GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
        {
            randomBox_Winter_Normal_List[0].boxInfoType = BoxInfoType.RightQueen_2_N;
            randomBox_Winter_Normal_List[1].boxInfoType = BoxInfoType.RightQueen_2_R;
            randomBox_Winter_Normal_List[2].boxInfoType = BoxInfoType.RightQueen_2_SR;
            randomBox_Winter_Normal_List[3].boxInfoType = BoxInfoType.RightQueen_2_SSR;

            randomBox_Winter_Normal_List[0].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 });
            randomBox_Winter_Normal_List[1].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 });
            randomBox_Winter_Normal_List[2].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 });
            randomBox_Winter_Normal_List[3].SetPercent(new float[] { 0.75f, 0.75f, 0.1f, 0.75f, 0 });

            randomBox_Winter_Normal_List[4].boxInfoType = BoxInfoType.RightQueen_3_N;
            randomBox_Winter_Normal_List[5].boxInfoType = BoxInfoType.RightQueen_3_R;
            randomBox_Winter_Normal_List[6].boxInfoType = BoxInfoType.RightQueen_3_SR;
            randomBox_Winter_Normal_List[7].boxInfoType = BoxInfoType.RightQueen_3_SSR;

            randomBox_Winter_Normal_List[4].SetPercent(new float[] { 1, 0.1f, 1f, 0, 0 });
            randomBox_Winter_Normal_List[5].SetPercent(new float[] { 1, 0.1f, 1f, 0, 0 });
            randomBox_Winter_Normal_List[6].SetPercent(new float[] { 1, 0.1f, 1f, 0, 0 });
            randomBox_Winter_Normal_List[7].SetPercent(new float[] { 1, 0.1f, 1f, 0, 0 });

        }
        else
        {

        }
    }

    public RandomBox_Block GetRandom(BoxType boxType, int index)
    {
        switch (boxType)
        {
            case BoxType.Normal:
                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    randomBox_Block = randomBox_Underworld_Normal_List[index].GetRandom();
                }
                else
                {
                    randomBox_Block = randomBox_Underworld_Normal_List[index].GetRandom();
                }
                break;
            case BoxType.N:
                break;
            case BoxType.R:
                break;
            case BoxType.SR:
                break;
            case BoxType.SSR:
                break;
            case BoxType.UR:
                break;
            case BoxType.NR:
                break;
            case BoxType.RSR:
                break;
            case BoxType.SRSSR:
                break;
            case BoxType.Epic:
                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    randomBox_Block = randomBox_Winter_Epic_List[index].GetRandom();
                }
                else
                {
                    randomBox_Block = randomBox_Underworld_Epic_List[index].GetRandom();
                }
                break;
            case BoxType.Speical:
                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    randomBox_Block = randomBox_Winter_Speical_List[index].GetRandom();
                }
                else
                {
                    randomBox_Block = randomBox_Underworld_Speical_List[index].GetRandom();
                }
                break;
        }

        return randomBox_Block;
    }

}


[System.Serializable]
public class RandomBox
{
    public BoxInfoType boxInfoType = BoxInfoType.RightQueen_2_N;

    public float percent = 0; //전체 비율에서

    public float[] index = new float[5]; //걸렸을 경우에서 또 나누기

    float random = 0;
    RandomBox_Block randomBox_Block = new RandomBox_Block();

    public void SetPercent(float[] value)
    {
        index = value;

        percent = 0;

        for (int i = 0; i < index.Length; i ++)
        {
            percent += index[i];
        }
    }

    public RandomBox_Block GetRandom() //몇번째 조각이 당첨되었는지 알려주기
    {
        random = Random.Range(0f, percent);

        for(int i = 0; i < index.Length; i ++)
        {
            if (random <= index[i])
            {
                randomBox_Block.boxInfoType = boxInfoType;
                randomBox_Block.number = i;
                break;
            }
        }

        return randomBox_Block;
    }
}

[System.Serializable]
public class RandomBox_Block //당첨된 블럭 정보
{
    public BoxInfoType boxInfoType = BoxInfoType.RightQueen_2_N;
    public int number = 0;
}


public class RandomBoxManager : MonoBehaviour
{
    public WindCharacterType windCharacterType = WindCharacterType.Winter;
    public BoxType boxType = BoxType.Normal;

    [SerializeField]
    public RandomBoxInfo randomBoxInfo;

    public GameObject boxView;

    public GameObject boxOpenView;

    public FadeInOut fadeInOut;

    public Text boxCountText;

    [Title("Content")]
    public BlockUIContent blockUIContent;

    public Transform blockUIContentTransform;

    public List<BlockUIContent> blockUIContentList = new List<BlockUIContent>();

    [Title("Box")]
    public Image boxIcon;
    public Sprite[] boxInitIcon;
    public Sprite[] boxOpenIcon;
    public GameObject boxOpenEffect;
    public GameObject gradient;

    public GameObject boxPanel;
    public GameObject closePanel;
    public ButtonScaleAnimation boxAnim;
    public GameObject tapObj;

    [Title("Detail")]
    public BlockUIContent blockUIContent_Detail;
    public Text blockTitleText;
    public Text nextText;
    public GameObject blockUIEffect;
    public Text nextBoxTapObj;

    [Title("Value")]
    public int boxCount = 0;
    public int boxCountSave = 0;
    public int boxIndex = 0;

    private bool isStart = false;
    private bool isDelay = false;

    private float random = 0;
    private int gold = 0;

    private bool confirmationSR = false;
    private bool confirmationSSR = false;

    private List<float> percent = new List<float>(); //확률
    private List<int> confirm_N = new List<int>(); //확정 조각
    private List<int> confirm_R = new List<int>();
    private List<int> confirm_SR = new List<int>();
    private List<int> confirm_SSR = new List<int>();
    private List<int> confirm_UR = new List<int>();



    private List<int> prize = new List<int>(); //당첨된 것 (숫자)
    private List<RandomBox_Block> prize_Block = new List<RandomBox_Block>(); //당첨된 블럭 조각

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

    public ShopManager shopManager;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        for (int i = 0; i < 100; i++)
        {
            BlockUIContent monster = Instantiate(blockUIContent);
            monster.transform.SetParent(blockUIContentTransform);
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            monster.gameObject.SetActive(false);

            blockUIContentList.Add(monster);
        }

        ResetView();
    }

    void ResetView()
    {
        isStart = false;
        isDelay = false;

        boxView.SetActive(false);
        boxPanel.SetActive(false);
        closePanel.SetActive(false);

        boxOpenEffect.SetActive(false);

        tapObj.SetActive(false);

        gradient.SetActive(false);

        boxOpenView.SetActive(false);
        blockUIEffect.SetActive(false);

        boxIndex = 0;
    }

    private void OnEnable()
    {
        PlayerDataBase.eGetSnowBox_Normal += OpenSnowBox_Normal;
        PlayerDataBase.eGetSnowBox_Epic += OpenSnowBox_Epic;
        PlayerDataBase.eGetSnowBox_Speical += OpenSnowBox_Speical;
        PlayerDataBase.eGetSnowBox_N += OpenSnowBox_N;
        PlayerDataBase.eGetSnowBox_R += OpenSnowBox_R;
        PlayerDataBase.eGetSnowBox_SR += OpenSnowBox_SR;
        PlayerDataBase.eGetSnowBox_SSR += OpenSnowBox_SSR;
        PlayerDataBase.eGetSnowBox_UR += OpenSnowBox_UR;
        PlayerDataBase.eGetSnowBox_NR += OpenSnowBox_NR;
        PlayerDataBase.eGetSnowBox_RSR += OpenSnowBox_RSR;
        PlayerDataBase.eGetSnowBox_SRSSR += OpenSnowBox_SRSSR;

        PlayerDataBase.eGetUnderworldBox_Normal += OpenUnderworldBox_Normal;
        PlayerDataBase.eGetUnderworldBox_Epic += OpenUnderworldBox_Epic;
        PlayerDataBase.eGetUnderworldBox_Speical += OpenUnderworldBox_Speical;
        PlayerDataBase.eGetUnderworldBox_N += OpenUnderworldBox_N;
        PlayerDataBase.eGetUnderworldBox_R += OpenUnderworldBox_R;
        PlayerDataBase.eGetUnderworldBox_SR += OpenUnderworldBox_SR;
        PlayerDataBase.eGetUnderworldBox_SSR += OpenUnderworldBox_SSR;
        PlayerDataBase.eGetUnderworldBox_UR += OpenUnderworldBox_UR;
        PlayerDataBase.eGetUnderworldBox_NR += OpenUnderworldBox_NR;
        PlayerDataBase.eGetUnderworldBox_RSR += OpenUnderworldBox_RSR;
        PlayerDataBase.eGetUnderworldBox_SRSSR += OpenUnderworldBox_SRSSR;
    }

    private void OnDisable()
    {
        PlayerDataBase.eGetSnowBox_Normal -= OpenSnowBox_Normal;
        PlayerDataBase.eGetSnowBox_Epic -= OpenSnowBox_Epic;
        PlayerDataBase.eGetSnowBox_Speical -= OpenSnowBox_Speical;
        PlayerDataBase.eGetSnowBox_N -= OpenSnowBox_N;
        PlayerDataBase.eGetSnowBox_R -= OpenSnowBox_R;
        PlayerDataBase.eGetSnowBox_SR -= OpenSnowBox_SR;
        PlayerDataBase.eGetSnowBox_SSR -= OpenSnowBox_SSR;
        PlayerDataBase.eGetSnowBox_UR -= OpenSnowBox_UR;
        PlayerDataBase.eGetSnowBox_NR -= OpenSnowBox_NR;
        PlayerDataBase.eGetSnowBox_RSR -= OpenSnowBox_RSR;
        PlayerDataBase.eGetSnowBox_SRSSR -= OpenSnowBox_SRSSR;

        PlayerDataBase.eGetUnderworldBox_Normal -= OpenUnderworldBox_Normal;
        PlayerDataBase.eGetUnderworldBox_Epic -= OpenUnderworldBox_Epic;
        PlayerDataBase.eGetUnderworldBox_Speical -= OpenUnderworldBox_Speical;
        PlayerDataBase.eGetUnderworldBox_N -= OpenUnderworldBox_N;
        PlayerDataBase.eGetUnderworldBox_R -= OpenUnderworldBox_R;
        PlayerDataBase.eGetUnderworldBox_SR -= OpenUnderworldBox_SR;
        PlayerDataBase.eGetUnderworldBox_SSR -= OpenUnderworldBox_SSR;
        PlayerDataBase.eGetUnderworldBox_UR -= OpenUnderworldBox_UR;
        PlayerDataBase.eGetUnderworldBox_NR -= OpenUnderworldBox_NR;
        PlayerDataBase.eGetUnderworldBox_RSR -= OpenUnderworldBox_RSR;
        PlayerDataBase.eGetUnderworldBox_SRSSR -= OpenUnderworldBox_SRSSR;
    }

    private void OnApplicationQuit()
    {
        PlayerDataBase.eGetSnowBox_Normal -= OpenSnowBox_Normal;
        PlayerDataBase.eGetSnowBox_Epic -= OpenSnowBox_Epic;
        PlayerDataBase.eGetSnowBox_Speical -= OpenSnowBox_Speical;
        PlayerDataBase.eGetSnowBox_N -= OpenSnowBox_N;
        PlayerDataBase.eGetSnowBox_R -= OpenSnowBox_R;
        PlayerDataBase.eGetSnowBox_SR -= OpenSnowBox_SR;
        PlayerDataBase.eGetSnowBox_SSR -= OpenSnowBox_SSR;
        PlayerDataBase.eGetSnowBox_UR -= OpenSnowBox_UR;
        PlayerDataBase.eGetSnowBox_NR -= OpenSnowBox_NR;
        PlayerDataBase.eGetSnowBox_RSR -= OpenSnowBox_RSR;
        PlayerDataBase.eGetSnowBox_SRSSR -= OpenSnowBox_SRSSR;

        PlayerDataBase.eGetUnderworldBox_Normal -= OpenUnderworldBox_Normal;
        PlayerDataBase.eGetUnderworldBox_Epic -= OpenUnderworldBox_Epic;
        PlayerDataBase.eGetUnderworldBox_Speical -= OpenUnderworldBox_Speical;
        PlayerDataBase.eGetUnderworldBox_N -= OpenUnderworldBox_N;
        PlayerDataBase.eGetUnderworldBox_R -= OpenUnderworldBox_R;
        PlayerDataBase.eGetUnderworldBox_SR -= OpenUnderworldBox_SR;
        PlayerDataBase.eGetUnderworldBox_SSR -= OpenUnderworldBox_SSR;
        PlayerDataBase.eGetUnderworldBox_UR -= OpenUnderworldBox_UR;
        PlayerDataBase.eGetUnderworldBox_NR -= OpenUnderworldBox_NR;
        PlayerDataBase.eGetUnderworldBox_RSR -= OpenUnderworldBox_RSR;
        PlayerDataBase.eGetUnderworldBox_SRSSR -= OpenUnderworldBox_SRSSR;
    }

    public void Initialize()
    {
        randomBoxInfo = new RandomBoxInfo();
        randomBoxInfo.Initialize();
    }

    public void OpenSnowBox_Normal()
    {
        boxType = BoxType.Normal;

        boxCount = playerDataBase.SnowBox_Normal;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public void OpenSnowBox_Epic()
    {
        boxType = BoxType.Epic;

        boxCount = playerDataBase.SnowBox_Epic;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public void OpenSnowBox_Speical()
    {
        boxType = BoxType.Speical;

        boxCount = playerDataBase.SnowBox_Speical;

        if (playerDataBase.BuySnowBoxSSRCount >= 50)
        {
            playerDataBase.BuySnowBoxSSRCount -= 50;

            confirmationSSR = true;
        }
        else
        {
            confirmationSSR = false;
        }

        playerDataBase.BuySnowBoxSSRCount += boxCount;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuySnowBoxSSRCount", playerDataBase.BuySnowBoxSSRCount);

        if (boxCount >= 10)
        {
            confirmationSR = true;
        }
        else
        {
            confirmationSR = false;
        }

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public void OpenSnowBox_N()
    {
        boxType = BoxType.N;

        boxCount = playerDataBase.SnowBox_N;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public void OpenSnowBox_R()
    {
        boxType = BoxType.R;

        boxCount = playerDataBase.SnowBox_R;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public void OpenSnowBox_SR()
    {
        boxType = BoxType.SR;

        boxCount = playerDataBase.SnowBox_SR;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public void OpenSnowBox_SSR()
    {
        boxType = BoxType.SSR;

        boxCount = playerDataBase.SnowBox_SSR;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public void OpenSnowBox_UR()
    {
        boxType = BoxType.UR;

        boxCount = playerDataBase.SnowBox_UR;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public void OpenSnowBox_NR()
    {
        boxType = BoxType.NR;

        boxCount = playerDataBase.SnowBox_NR;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public void OpenSnowBox_RSR()
    {
        boxType = BoxType.RSR;

        boxCount = playerDataBase.SnowBox_RSR;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public void OpenSnowBox_SRSSR()
    {
        boxType = BoxType.SRSSR;

        boxCount = playerDataBase.SnowBox_SRSSR;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public void OpenUnderworldBox_Normal()
    {
        boxType = BoxType.Normal;

        boxCount = playerDataBase.UnderworldBox_Normal;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenUnderworldBox_Epic()
    {
        boxType = BoxType.Epic;

        boxCount = playerDataBase.UnderworldBox_Epic;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenUnderworldBox_Speical()
    {
        boxType = BoxType.Speical;

        boxCount = playerDataBase.UnderworldBox_Speical;

        if (playerDataBase.BuyUnderworldBoxSSRCount >= 50)
        {
            playerDataBase.BuyUnderworldBoxSSRCount -= 50;

            confirmationSSR = true;
        }
        else
        {
            confirmationSSR = false;
        }

        playerDataBase.BuyUnderworldBoxSSRCount += boxCount;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuyUnderworldBoxSSRCount", playerDataBase.BuyUnderworldBoxSSRCount);

        if (boxCount >= 10)
        {
            confirmationSR = true;
        }
        else
        {
            confirmationSR = false;
        }

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenUnderworldBox_N()
    {
        boxType = BoxType.N;

        boxCount = playerDataBase.UnderworldBox_N;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenUnderworldBox_R()
    {
        boxType = BoxType.R;

        boxCount = playerDataBase.UnderworldBox_R;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenUnderworldBox_SR()
    {
        boxType = BoxType.SR;

        boxCount = playerDataBase.UnderworldBox_SR;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenUnderworldBox_SSR()
    {
        boxType = BoxType.SSR;

        boxCount = playerDataBase.UnderworldBox_SSR;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenUnderworldBox_UR()
    {
        boxType = BoxType.UR;

        boxCount = playerDataBase.UnderworldBox_UR;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenUnderworldBox_NR()
    {
        boxType = BoxType.NR;

        boxCount = playerDataBase.UnderworldBox_NR;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenUnderworldBox_RSR()
    {
        boxType = BoxType.RSR;

        boxCount = playerDataBase.UnderworldBox_RSR;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenUnderworldBox_SRSSR()
    {
        boxType = BoxType.SRSSR;

        boxCount = playerDataBase.UnderworldBox_SRSSR;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenSnowBox_Initialize()
    {
        ResetView();

        boxIcon.sprite = boxInitIcon[(int)boxType];

        boxCountText.text = boxCount.ToString();
        boxCountSave = boxCount;

        playerDataBase.BoxOpenCount += boxCount;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxOpenCount", playerDataBase.BoxOpenCount);

        windCharacterType = WindCharacterType.Winter;

        percent = randomBoxInfo.GetPercent(boxType);
        confirm_N = randomBoxInfo.GetConfrim(boxType, RankType.N);
        confirm_R = randomBoxInfo.GetConfrim(boxType, RankType.R);
        confirm_SR = randomBoxInfo.GetConfrim(boxType, RankType.SR);
        confirm_SSR = randomBoxInfo.GetConfrim(boxType, RankType.SSR);
        confirm_UR = randomBoxInfo.GetConfrim(boxType, RankType.UR);

        prize.Clear();
        prize_Block.Clear();

        boxView.SetActive(true);
        boxAnim.PlayAnim();

        RandomBox();
    }

    public void OpenUnderworldBox_Initialize()
    {
        ResetView();

        boxIcon.sprite = boxInitIcon[(int)boxType];

        boxCountText.text = boxCount.ToString();
        boxCountSave = boxCount;

        playerDataBase.BoxOpenCount += boxCount;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxOpenCount", playerDataBase.BoxOpenCount);

        windCharacterType = WindCharacterType.UnderWorld;

        percent = randomBoxInfo.GetPercent(boxType);
        confirm_N = randomBoxInfo.GetConfrim(boxType, RankType.N);
        confirm_R = randomBoxInfo.GetConfrim(boxType, RankType.R);
        confirm_SR = randomBoxInfo.GetConfrim(boxType, RankType.SR);
        confirm_SSR = randomBoxInfo.GetConfrim(boxType, RankType.SSR);
        confirm_UR = randomBoxInfo.GetConfrim(boxType, RankType.UR);

        prize.Clear();
        prize_Block.Clear();

        boxView.SetActive(true);
        boxAnim.PlayAnim();

        RandomBox();
    }

    public void CloseBoxView()
    {
        boxView.SetActive(false);
    }

    public void OpenBox()
    {
        if(!isStart)
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            NotionManager.instance.UseNotion(NotionType.WaitTimeNotion);
            return;
        }

        isStart = false;

        boxIcon.sprite = boxOpenIcon[(int)boxType];
        gradient.SetActive(false);
        boxOpenEffect.SetActive(true);
        boxAnim.StopAnim();

        SoundManager.instance.PlaySFX(GameSfxType.BoxOpen);
        SoundManager.instance.PlaySFX(GameSfxType.BoxOpen2);

        StartCoroutine(NextButtonCoroution());
    }

    IEnumerator NextButtonCoroution()
    {
        yield return new WaitForSeconds(0.3f);

        fadeInOut.gameObject.SetActive(true);
        fadeInOut.FadeOutToIn();

        yield return new WaitForSeconds(1.5f);

        NextButton();
    }

    void RandomBox()
    {
        while (boxCount > 0)
        {
            boxCount -= 1;

            random = Random.Range(0f, 100.0f);

            if (confirmationSSR)
            {
                confirmationSSR = false;

                prize.Add(confirm_SSR[Random.Range(0, confirm_SSR.Count)]);

                gradient.SetActive(true);

                Debug.Log("SSR 확정");
            }
            else
            {
                if (confirmationSR)
                {
                    confirmationSR = false;

                    prize.Add(confirm_SR[Random.Range(0, confirm_SR.Count)]);

                    Debug.Log("SR 확정");
                }
                else
                {
                    for (int i = 0; i < percent.Count; i++)
                    {
                        if (random <= percent[i])
                        {
                            prize.Add(i);
                            break;
                        }
                    }
                }
            }
        }

        isStart = true;

        BoxOff();
    }

    void BoxOff()
    {
        //Prize에 걸린 숫자에 따라 이제 조각 획득하기

        gold = 0;

        for (int i = 0; i < prize.Count; i ++)
        {
            prize_Block.Add(randomBoxInfo.GetRandom(boxType ,prize[i]));
        }

        for(int i = 0; i < prize_Block.Count; i ++)
        {
            switch (prize_Block[i].boxInfoType)
            {
                case BoxInfoType.RightQueen_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_2, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.RightQueen_3_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_3, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_Mirror_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight_Mirror, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2_2, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under_2, RankType.N, prize_Block[i].number);
                    break;


                case BoxInfoType.RightQueen_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_2, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.RightQueen_3_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_3, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_Mirror_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight_Mirror, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2_2, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under_2, RankType.R, prize_Block[i].number);
                    break;


                case BoxInfoType.RightQueen_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_2, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightQueen_3_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_3, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_Mirror_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight_Mirror, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2_2, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under_2, RankType.SR, prize_Block[i].number);
                    break;


                case BoxInfoType.RightQueen_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_2, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightQueen_3_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_3, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_Mirror_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight_Mirror, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2_2, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under_2, RankType.SSR, prize_Block[i].number);
                    break;


                case BoxInfoType.LeftQueen_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_2, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftQueen_3_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_3, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_Mirror_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight_Mirror, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4_2, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow_2, RankType.N, prize_Block[i].number);
                    break;


                case BoxInfoType.LeftQueen_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_2, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftQueen_3_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_3, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_Mirror_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight_Mirror, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4_2, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow_2, RankType.R, prize_Block[i].number);
                    break;


                case BoxInfoType.LeftQueen_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_2, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftQueen_3_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_3, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_Mirror_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight_Mirror, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4_2, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow_2, RankType.SR, prize_Block[i].number);
                    break;


                case BoxInfoType.LeftQueen_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_2, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftQueen_3_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_3, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_Mirror_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight_Mirror, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4_2, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow_2, RankType.SSR, prize_Block[i].number);
                    break;


                case BoxInfoType.Gold1:
                    switch(prize_Block[i].number)
                    {
                        case 0:
                            gold += Random.Range(5000, 10001);

                            break;
                        case 1:
                            gold += Random.Range(10000, 20001);

                            break;
                        case 2:
                            gold += Random.Range(20000, 30001);

                            break;
                        case 3:
                            gold += Random.Range(30000, 40001);

                            break;
                        case 4:
                            gold += Random.Range(40000, 50001);

                            break;
                    }
                    break;
                case BoxInfoType.Gold2:
                    switch (prize_Block[i].number)
                    {
                        case 0:
                            gold += Random.Range(50000, 60001);

                            break;
                        case 1:
                            gold += Random.Range(60000, 70001);

                            break;
                        case 2:
                            gold += Random.Range(70000, 80001);

                            break;
                        case 3:
                            gold += Random.Range(80000, 90001);

                            break;
                        case 4:
                            gold += Random.Range(90000, 100001);

                            break;
                    }
                    break;
                case BoxInfoType.UpgradeTicket1:
                    switch (prize_Block[i].number)
                    {
                        case 0:


                            break;
                        case 1:


                            break;
                        case 2:


                            break;
                        case 3:


                            break;
                        case 4:


                            break;
                    }
                    break;
                case BoxInfoType.UpgradeTicket2:
                    switch (prize_Block[i].number)
                    {
                        case 0:


                            break;
                        case 1:


                            break;
                        case 2:


                            break;
                        case 3:


                            break;
                        case 4:


                            break;
                    }
                    break;
            }
        }


        if(gold > 0)
        {
            PlayfabManager.instance.UpdateAddGold(gold);
        }


        switch (windCharacterType)
        {
            case WindCharacterType.Winter:

                switch (boxType)
                {
                    case BoxType.Normal:
                        playerDataBase.SnowBox_Normal = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", 0);
                        break;
                    case BoxType.N:
                        playerDataBase.SnowBox_N = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_N", 0);
                        break;
                    case BoxType.R:
                        playerDataBase.SnowBox_R = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_R", 0);
                        break;
                    case BoxType.SR:
                        playerDataBase.SnowBox_SR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SR", 0);
                        break;
                    case BoxType.SSR:
                        playerDataBase.SnowBox_SSR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SSR", 0);
                        break;
                    case BoxType.UR:
                        playerDataBase.SnowBox_UR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_UR", 0);
                        break;
                    case BoxType.NR:
                        playerDataBase.SnowBox_NR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_NR", 0);
                        break;
                    case BoxType.RSR:
                        playerDataBase.SnowBox_RSR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_RSR", 0);
                        break;
                    case BoxType.SRSSR:
                        playerDataBase.SnowBox_SRSSR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SRSSR", 0);
                        break;
                    case BoxType.Epic:
                        playerDataBase.SnowBox_Epic = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Epic", 0);
                        break;
                    case BoxType.Speical:
                        playerDataBase.SnowBox_Speical = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Speical", 0);
                        break;
                }

                break;
            case WindCharacterType.UnderWorld:

                switch (boxType)
                {
                    case BoxType.Normal:
                        playerDataBase.UnderworldBox_Normal = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", 0);
                        break;
                    case BoxType.N:
                        playerDataBase.UnderworldBox_N = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_N", 0);
                        break;
                    case BoxType.R:
                        playerDataBase.UnderworldBox_R = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_R", 0);
                        break;
                    case BoxType.SR:
                        playerDataBase.UnderworldBox_SR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SR", 0);
                        break;
                    case BoxType.SSR:
                        playerDataBase.UnderworldBox_SSR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SSR", 0);
                        break;
                    case BoxType.UR:
                        playerDataBase.UnderworldBox_UR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_UR", 0);
                        break;
                    case BoxType.NR:
                        playerDataBase.UnderworldBox_NR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_NR", 0);
                        break;
                    case BoxType.RSR:
                        playerDataBase.UnderworldBox_RSR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_RSR", 0);
                        break;
                    case BoxType.SRSSR:
                        playerDataBase.UnderworldBox_SRSSR = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SRSSR", 0);
                        break;
                    case BoxType.Epic:
                        playerDataBase.UnderworldBox_Epic = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Epic", 0);
                        break;
                    case BoxType.Speical:
                        playerDataBase.UnderworldBox_Speical = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Speical", 0);
                        break;
                }

                break;
        }

        shopManager.Change();
    }

    public void NextButton()
    {
        if (isDelay) return;

        if (boxIndex < boxCountSave)
        {
            boxOpenView.SetActive(true);

            blockUIEffect.SetActive(false);

            if (prize_Block[boxIndex].boxInfoType.ToString().Contains(RankType.SSR.ToString()))
            {
                fadeInOut.gameObject.SetActive(true);
                fadeInOut.FadeOut();

                blockUIEffect.SetActive(true);

                SoundManager.instance.PlaySFX(GameSfxType.BoxOpen);
            }
            else
            {
                SoundManager.instance.PlaySFX(GameSfxType.GetBlock);
            }

            blockUIContent_Detail.Initialize_RandomBox(prize_Block[boxIndex]);

            blockTitleText.text = "";
            //blockTitleText.text = LocalizationManager.instance.GetString(prize_Block[boxIndex].boxInfoType.ToString());
            nextText.text = (boxIndex + 1) + "/" + boxCountSave;

            boxIndex++;
        }
        else
        {
            boxOpenView.SetActive(false);

            StartCoroutine(OpenBoxCoroution());
        }

        if (boxIndex == boxCountSave)
        {
            nextBoxTapObj.text = LocalizationManager.instance.GetString("EndBox");
        }
        else
        {
            nextBoxTapObj.text = LocalizationManager.instance.GetString("NextBox");
        }

        //Debug.Log(boxIndex + " / " + boxCountSave);

        isDelay = true;
        Invoke("Delay", 0.1f);
    }

    IEnumerator OpenBoxCoroution() //마지막에 전체 보여주기
    {
        boxCountText.text = "0";

        boxPanel.SetActive(true);

        for (int i = 0; i < blockUIContentList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < prize_Block.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(true);
            blockUIContentList[i].Initialize_RandomBox(prize_Block[i]);

            SoundManager.instance.PlaySFX(GameSfxType.GetBlock);

            yield return waitForSeconds;
        }

        yield return new WaitForSeconds(0.5f);

        tapObj.SetActive(true);

        closePanel.SetActive(true);
    }


    void Delay()
    {
        isDelay = false;
    }
}
