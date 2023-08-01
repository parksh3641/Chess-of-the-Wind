using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBoxManager : MonoBehaviour
{
    public WindCharacterType windCharacterType = WindCharacterType.Winter;
    public BoxType boxType = BoxType.Random;

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
    public Image boxGradient;
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

    [Title("Value")]
    public int boxCount = 0;
    public int boxCountSave = 0;
    public int boxIndex = 0;
    public float[] percentBlock;
    public List<string> allowSnowBlockList = new List<string>();
    public List<string> allowUnderworldBlockList = new List<string>();
    public List<BlockClass> prizeBlockList = new List<BlockClass>();
    public List<string> prizeBlockStringList = new List<string>();

    public bool isWait = false;
    public bool isStart = false;
    public bool isDelay = false;

    private float random = 0;
    private string block = "";

    private bool confirmationSR = false;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        for (int i = 0; i < 20; i++)
        {
            BlockUIContent monster = Instantiate(blockUIContent);
            monster.transform.parent = blockUIContentTransform;
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
        percentBlock = new float[4];

        allowSnowBlockList.Clear();
        allowUnderworldBlockList.Clear();
        prizeBlockList.Clear();
        prizeBlockStringList.Clear();

        isWait = false;
        isStart = false;

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
        PlayerDataBase.eGetSnowBox += OpenSnowBox;
        PlayerDataBase.eGetSnowBox_N += OpenSnowBox_N;
        PlayerDataBase.eGetSnowBox_R += OpenSnowBox_R;
        PlayerDataBase.eGetSnowBox_SR += OpenSnowBox_SR;
        PlayerDataBase.eGetSnowBox_SSR += OpenSnowBox_SSR;
        PlayerDataBase.eGetSnowBox_UR += OpenSnowBox_UR;


        PlayerDataBase.eGetUnderworldBox += OpenUnderworldBox;
        PlayerDataBase.eGetUnderworldBox_N += OpenUnderworldBox_N;
        PlayerDataBase.eGetUnderworldBox_R += OpenUnderworldBox_R;
        PlayerDataBase.eGetUnderworldBox_SR += OpenUnderworldBox_SR;
        PlayerDataBase.eGetUnderworldBox_SSR += OpenUnderworldBox_SSR;
        PlayerDataBase.eGetUnderworldBox_UR += OpenUnderworldBox_UR;
    }

    private void OnDisable()
    {
        PlayerDataBase.eGetSnowBox -= OpenSnowBox;
        PlayerDataBase.eGetSnowBox_N -= OpenSnowBox_N;
        PlayerDataBase.eGetSnowBox_R -= OpenSnowBox_R;
        PlayerDataBase.eGetSnowBox_SR -= OpenSnowBox_SR;
        PlayerDataBase.eGetSnowBox_SSR -= OpenSnowBox_SSR;
        PlayerDataBase.eGetSnowBox_UR -= OpenSnowBox_UR;

        PlayerDataBase.eGetUnderworldBox -= OpenUnderworldBox;
        PlayerDataBase.eGetUnderworldBox_N -= OpenUnderworldBox_N;
        PlayerDataBase.eGetUnderworldBox_R -= OpenUnderworldBox_R;
        PlayerDataBase.eGetUnderworldBox_SR -= OpenUnderworldBox_SR;
        PlayerDataBase.eGetUnderworldBox_SSR -= OpenUnderworldBox_SSR;
        PlayerDataBase.eGetUnderworldBox_UR -= OpenUnderworldBox_UR;
    }

    private void OnApplicationQuit()
    {
        PlayerDataBase.eGetSnowBox -= OpenSnowBox;
        PlayerDataBase.eGetSnowBox_N -= OpenSnowBox_N;
        PlayerDataBase.eGetSnowBox_R -= OpenSnowBox_R;
        PlayerDataBase.eGetSnowBox_SR -= OpenSnowBox_SR;
        PlayerDataBase.eGetSnowBox_SSR -= OpenSnowBox_SSR;
        PlayerDataBase.eGetSnowBox_UR -= OpenSnowBox_UR;

        PlayerDataBase.eGetUnderworldBox -= OpenUnderworldBox;
        PlayerDataBase.eGetUnderworldBox_N -= OpenUnderworldBox_N;
        PlayerDataBase.eGetUnderworldBox_R -= OpenUnderworldBox_R;
        PlayerDataBase.eGetUnderworldBox_SR -= OpenUnderworldBox_SR;
        PlayerDataBase.eGetUnderworldBox_SSR -= OpenUnderworldBox_SSR;
        PlayerDataBase.eGetUnderworldBox_UR -= OpenUnderworldBox_UR;
    }

    public void OpenSnowBox()
    {
        boxType = BoxType.Random;

        boxCount = playerDataBase.SnowBox;

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

    public void OpenUnderworldBox()
    {
        boxType = BoxType.Random;

        boxCount = playerDataBase.UnderworldBox;

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

    public void OpenSnowBox_Initialize()
    {
        ResetView();

        boxIcon.sprite = boxInitIcon[(int)boxType];
        boxGradient.sprite = boxInitIcon[(int)boxType];

        boxCountText.text = boxCount.ToString();
        boxCountSave = boxCount;

        windCharacterType = WindCharacterType.Winter;

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData("RandomBox", InitializePercent);
    }

    public void OpenUnderworldBox_Initialize()
    {
        ResetView();

        boxIcon.sprite = boxInitIcon[(int)boxType];
        boxGradient.sprite = boxInitIcon[(int)boxType];

        boxCountText.text = boxCount.ToString();
        boxCountSave = boxCount;

        windCharacterType = WindCharacterType.UnderWorld;

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData("RandomBox", InitializePercent);
    }

    public void CloseBoxView()
    {
        boxView.SetActive(false);
    }

    void InitializePercent(string check)
    {
        string[] temp = check.Split(",");

        percentBlock[0] = float.Parse(temp[0]);
        percentBlock[1] = float.Parse(temp[1]);
        percentBlock[2] = float.Parse(temp[2]);
        percentBlock[3] = float.Parse(temp[3]);

        switch (windCharacterType)
        {
            case WindCharacterType.Winter:
                PlayfabManager.instance.GetTitleInternalData("AllowSnowBlock", InitializeAllowSnowBlock);
                break;
            case WindCharacterType.UnderWorld:
                PlayfabManager.instance.GetTitleInternalData("AllowUnderworldBlock", InitializeAllowUnderworldBlock);
                break;
        }
    }

    void InitializeAllowSnowBlock(string check)
    {
        string[] temp = check.Split(",");

        for(int i = 0; i < temp.Length; i ++)
        {
            allowSnowBlockList.Add(temp[i]);
        }

        isStart = true;

        boxView.SetActive(true);

        boxAnim.PlayAnim();

        StartCoroutine(RandomBoxCoroution());
    }

    void InitializeAllowUnderworldBlock(string check)
    {
        string[] temp = check.Split(",");

        for (int i = 0; i < temp.Length; i++)
        {
            allowUnderworldBlockList.Add(temp[i]);
        }

        isStart = true;

        boxView.SetActive(true);

        boxAnim.PlayAnim();

        StartCoroutine(RandomBoxCoroution());
    }

    public void OpenBox()
    {
        if (!isStart) return;

        isStart = false;

        boxIcon.sprite = boxOpenIcon[(int)boxType];
        boxGradient.sprite = boxOpenIcon[(int)boxType];

        boxOpenEffect.SetActive(true);

        boxAnim.StopAnim();

        SoundManager.instance.PlaySFX(GameSfxType.BoxOpen);

        //StartCoroutine(OpenBoxCoroution());

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

    public void NextButton()
    {
        if (isDelay) return;

        if (boxIndex < boxCountSave)
        {
            boxOpenView.SetActive(true);

            blockUIEffect.SetActive(false);

            if (prizeBlockList[boxIndex].rankType >= RankType.SSR)
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

            blockUIContent_Detail.Collection_Initialize(prizeBlockList[boxIndex]);

            blockTitleText.text = LocalizationManager.instance.GetString(prizeBlockList[boxIndex].blockType.ToString());
            nextText.text = (boxIndex + 1) + "/" + boxCountSave;

            boxIndex++;
        }
        else
        {
            boxOpenView.SetActive(false);

            StartCoroutine(OpenBoxCoroution());
        }

        isDelay = true;
        Invoke("Delay2", 0.3f);
    }

    IEnumerator OpenBoxCoroution()
    {
        boxCountText.text = "0";

        boxPanel.SetActive(true);

        for (int i = 0; i < blockUIContentList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < prizeBlockList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(true);
            blockUIContentList[i].Collection_Initialize(prizeBlockList[i]);

            //boxCountSave -= 1;
            //boxCountText.text = boxCountSave.ToString();

            SoundManager.instance.PlaySFX(GameSfxType.GetBlock);

            yield return waitForSeconds;
        }

        yield return new WaitForSeconds(0.5f);

        tapObj.SetActive(true);

        closePanel.SetActive(true);
    }

    IEnumerator RandomBoxCoroution()
    {
        if (boxCount > 0)
        {
            boxCount -= 1;

            while (isWait)
            {
                yield return null;
            }

            random = Random.Range(0, 100.0f);
            block = "";

            switch (windCharacterType)
            {
                case WindCharacterType.Winter:
                    int snow = Random.Range(0, allowSnowBlockList.Count);

                    block = allowSnowBlockList[snow];
                    break;
                case WindCharacterType.UnderWorld:
                    int underworld = Random.Range(0, allowUnderworldBlockList.Count);

                    block = allowUnderworldBlockList[underworld];
                    break;
            }

            BlockClass blockClass = new BlockClass();
            blockClass.blockType = (BlockType)System.Enum.Parse(typeof(BlockType), block);

            if (boxCount == 0 && confirmationSR)
            {
                confirmationSR = false;

                blockClass.rankType = RankType.SR;
                block += "_SR";

                Debug.Log("SR 확정");
            }
            else
            {
                switch (boxType)
                {
                    case BoxType.Random:
                        if (random <= percentBlock[3])
                        {
                            blockClass.rankType = RankType.SSR;
                            block += "_SSR";

                            gradient.SetActive(true);
                        }
                        else if (random <= percentBlock[2])
                        {
                            blockClass.rankType = RankType.SR;
                            block += "_SR";
                        }
                        else if (random <= percentBlock[1])
                        {
                            blockClass.rankType = RankType.R;
                            block += "_R";

                        }
                        else
                        {
                            blockClass.rankType = RankType.N;
                            block += "_N";

                        }
                        break;
                    case BoxType.N:
                        blockClass.rankType = RankType.N;
                        block += "_N";

                        break;
                    case BoxType.R:
                        blockClass.rankType = RankType.R;
                        block += "_R";

                        break;
                    case BoxType.SR:
                        blockClass.rankType = RankType.SR;
                        block += "_SR";
                        break;
                    case BoxType.SSR:
                        blockClass.rankType = RankType.SSR;
                        block += "_SSR";

                        gradient.SetActive(true);
                        break;
                    case BoxType.UR:
                        blockClass.rankType = RankType.UR;
                        block += "_UR";

                        gradient.SetActive(true);
                        break;
                    case BoxType.Choice_N:
                        break;
                    case BoxType.Choice_R:
                        break;
                    case BoxType.Choice_SR:
                        break;
                    case BoxType.Choice_SSR:
                        break;
                    case BoxType.Choice_UR:
                        break;
                }
            }

            prizeBlockList.Add(blockClass);
            prizeBlockStringList.Add(block);

            isWait = true;
            Invoke("Delay", 0.1f);

            StartCoroutine(RandomBoxCoroution());
        }
        else
        {
            while (isWait)
            {
                yield return null;
            }

            StartCoroutine(SetUserPriceBlockCoroution());
        }
    }

    IEnumerator SetUserPriceBlockCoroution()
    {
        while (isWait)
        {
            yield return null;
        }

        switch (windCharacterType)
        {
            case WindCharacterType.Winter:
                PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", prizeBlockStringList);
                break;
            case WindCharacterType.UnderWorld:
                PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", prizeBlockStringList);
                break;
        }

        yield return new WaitForSeconds(0.5f);

        switch (windCharacterType)
        {
            case WindCharacterType.Winter:

                switch (boxType)
                {
                    case BoxType.Random:
                        playerDataBase.SnowBox = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox", 0);
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
                    case BoxType.Choice_N:
                        break;
                    case BoxType.Choice_R:
                        break;
                    case BoxType.Choice_SR:
                        break;
                    case BoxType.Choice_SSR:
                        break;
                    case BoxType.Choice_UR:
                        break;
                }

                break;
            case WindCharacterType.UnderWorld:

                switch (boxType)
                {
                    case BoxType.Random:
                        playerDataBase.UnderworldBox = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", 0);
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
                    case BoxType.Choice_N:
                        break;
                    case BoxType.Choice_R:
                        break;
                    case BoxType.Choice_SR:
                        break;
                    case BoxType.Choice_SSR:
                        break;
                    case BoxType.Choice_UR:
                        break;
                }

                break;
        }
    }

    void Delay()
    {
        isWait = false;
    }

    void Delay2()
    {
        isDelay = false;
    }

    public void GameReward()
    {
        int random = Random.Range(0, 4);

        string prize = "";

        prizeBlockStringList.Clear();

        switch (GameStateManager.instance.WindCharacterType)
        {
            case WindCharacterType.Winter:
                switch(random)
                {
                    case 0:
                        prize = "LeftQueen_2";
                        break;
                    case 1:
                        prize = "LeftNight";
                        break;
                    case 2:
                        prize = "Rook_V2";
                        break;
                    case 3:
                        prize = "Pawn_Snow";
                        break;
                }

                prize += "_N";

                prizeBlockStringList.Add(prize);

                PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", prizeBlockStringList);
                break;
            case WindCharacterType.UnderWorld:
                switch (random)
                {
                    case 0:
                        prize = "RightQueen_2";
                        break;
                    case 1:
                        prize = "RightNight";
                        break;
                    case 2:
                        prize = "Rook_V4";
                        break;
                    case 3:
                        prize = "Pawn_Under";
                        break;
                }

                prize += "_N";

                prizeBlockStringList.Add(prize);

                PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", prizeBlockStringList);

                break;
        }
    }
}
