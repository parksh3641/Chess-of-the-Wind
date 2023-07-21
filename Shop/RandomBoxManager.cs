using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBoxManager : MonoBehaviour
{
    public WindCharacterType windCharacterType = WindCharacterType.Winter;

    public GameObject boxView;

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

    [Title("Value")]
    public int boxCount = 0;
    public int boxCountSave = 0;
    public float[] percentBlock;
    public List<string> allowSnowBlockList = new List<string>();
    public List<string> allowUnderworldBlockList = new List<string>();
    public List<BlockClass> prizeBlockList = new List<BlockClass>();
    public List<string> prizeBlockStringList = new List<string>();

    public bool isWait = false;
    public bool isStart = false;

    private float random = 0;
    private string block = "";

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        for (int i = 0; i < 10; i++)
        {
            BlockUIContent monster = Instantiate(blockUIContent);
            monster.transform.parent = blockUIContentTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            monster.gameObject.SetActive(false);

            blockUIContentList.Add(monster);
        }

        PlayerDataBase.eGetSnowBox += OpenSnowBoxView;
        PlayerDataBase.eGetUnderworldBox += OpenUnderworldBoxView;

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
    }

    private void OnApplicationQuit()
    {
        PlayerDataBase.eGetSnowBox -= OpenSnowBoxView;
        PlayerDataBase.eGetUnderworldBox -= OpenUnderworldBoxView;
    }

    public void OpenSnowBoxView()
    {
        if (!boxView.activeInHierarchy)
        {
            ResetView();

            boxIcon.sprite = boxInitIcon[0];
            boxGradient.sprite = boxInitIcon[0];

            boxCount = playerDataBase.SnowBox;
            boxCountText.text = boxCount.ToString();

            boxCountSave = boxCount;

            windCharacterType = WindCharacterType.Winter;

            if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData("RandomBox", InitializePercent);
        }
    }

    public void OpenUnderworldBoxView()
    {
        if (!boxView.activeInHierarchy)
        {
            ResetView();

            boxIcon.sprite = boxInitIcon[1];
            boxGradient.sprite = boxInitIcon[1];

            boxCount = playerDataBase.UnderworldBox;
            boxCountText.text = boxCount.ToString();

            boxCountSave = boxCount;

            windCharacterType = WindCharacterType.UnderWorld;

            if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData("RandomBox", InitializePercent);
        }
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

        StartCoroutine(RandomBoxCoroution());
    }

    public void OpenBox()
    {
        if (!isStart) return;

        switch (windCharacterType)
        {
            case WindCharacterType.Winter:
                boxIcon.sprite = boxOpenIcon[0];
                boxGradient.sprite = boxOpenIcon[1];
                break;
            case WindCharacterType.UnderWorld:
                boxIcon.sprite = boxOpenIcon[1];
                boxGradient.sprite = boxOpenIcon[1];
                break;
        }

        boxPanel.SetActive(true);

        boxOpenEffect.SetActive(true);

        boxAnim.StopAnim();

        isStart = false;

        SoundManager.instance.PlaySFX(GameSfxType.BoxOpen);

        StartCoroutine(OpenBoxCoroution());
    }

    IEnumerator OpenBoxCoroution()
    {
        for (int i = 0; i < blockUIContentList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < prizeBlockList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(true);
            blockUIContentList[i].Collection_Initialize(prizeBlockList[i]);

            boxCountSave -= 1;
            boxCountText.text = boxCountSave.ToString();

            SoundManager.instance.PlaySFX(GameSfxType.GetBlock);

            yield return waitForSeconds;
        }

        yield return new WaitForSeconds(1.0f);

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

            if (random <= percentBlock[3])
            {
                blockClass.rankType = RankType.SSR;
                block += "_SSR";
                Debug.Log("SSR 당첨");

                gradient.SetActive(true);
            }
            else if (random <= percentBlock[2])
            {
                blockClass.rankType = RankType.SR;
                block += "_SR";
                Debug.Log("SR 당첨");

                gradient.SetActive(true);
            }
            else if (random <= percentBlock[1])
            {
                blockClass.rankType = RankType.R;
                block += "_R";
                Debug.Log("R 당첨");
            }
            else
            {
                blockClass.rankType = RankType.N;
                block += "_N";
                Debug.Log("N 당첨");
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
                playerDataBase.SnowBox = 0;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox", 0);

                break;
            case WindCharacterType.UnderWorld:
                playerDataBase.UnderworldBox = 0;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", 0);

                break;
        }
    }

    void Delay()
    {
        isWait = false;
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
