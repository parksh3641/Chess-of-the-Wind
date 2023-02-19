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
    public Sprite[] boxInitIcon;
    public Sprite[] boxOpenIcon;
    public GameObject boxOpenEffect;

    public GameObject boxPanel;
    public GameObject closePanel;
    public ButtonScaleAnimation boxAnim;


    [Title("Value")]
    public int boxCount = 0;
    public int boxCountSave = 0;
    public int[] percentBlock;
    public List<string> allowSnowBlockList = new List<string>();
    public List<string> allowUnderworldBlockList = new List<string>();
    public List<string> prizeBlockList = new List<string>(); //당첨된 것

    public bool isWait = false; //상자 뽑기 딜레이
    public bool isStart = false; //준비완료

    private int random = 0;
    private string rank = "";

    public SoundManager soundManager;
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
        percentBlock = new int[3];

        allowSnowBlockList.Clear();
        allowUnderworldBlockList.Clear();
        prizeBlockList.Clear();

        isWait = false;
        isStart = false;

        boxView.SetActive(false);
        boxPanel.SetActive(false);
        closePanel.SetActive(false);

        boxOpenEffect.SetActive(false);
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

        percentBlock[0] = int.Parse(temp[0]);
        percentBlock[1] = int.Parse(temp[1]);
        percentBlock[2] = int.Parse(temp[2]);

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

    public void OpenBox() //상자가 준비되면 열기
    {
        if (!isStart) return;

        switch (windCharacterType)
        {
            case WindCharacterType.Winter:
                boxIcon.sprite = boxOpenIcon[0];
                break;
            case WindCharacterType.UnderWorld:
                boxIcon.sprite = boxOpenIcon[1];
                break;
        }

        boxPanel.SetActive(true);

        boxAnim.StopAnim();

        isStart = false;

        StartCoroutine(OpenBoxCoroution());
    }

    IEnumerator OpenBoxCoroution()
    {
        for (int i = 0; i < blockUIContentList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < prizeBlockList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(true);
            blockUIContentList[i].Initialize(prizeBlockList[i]);

            boxCountSave -= 1;
            boxCountText.text = boxCountSave.ToString();

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2f);

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

            random = Random.Range(0, 100);
            rank = "";

            switch (windCharacterType)
            {
                case WindCharacterType.Winter:
                    int snow = Random.Range(0, allowSnowBlockList.Count);

                    rank = allowSnowBlockList[snow];
                    break;
                case WindCharacterType.UnderWorld:
                    int underworld = Random.Range(0, allowUnderworldBlockList.Count);

                    rank = allowUnderworldBlockList[underworld];
                    break;
            }


            if (random <= percentBlock[2])
            {
                Debug.Log("A 당첨");
                rank += "_A";
            }
            else if (random <= percentBlock[1])
            {
                Debug.Log("B 당첨");
                rank += "_B";
            }
            else
            {
                Debug.Log("C 당첨");
                rank += "_C";
            }

            prizeBlockList.Add(rank);

            isWait = true;
            Invoke("Delay", 0.1f);

            StartCoroutine(RandomBoxCoroution());
        }
        else
        {
            Debug.Log("상자 종료 후 유저 인벤토리 전송 시작");

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
                PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", prizeBlockList);
                break;
            case WindCharacterType.UnderWorld:
                PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", prizeBlockList);
                break;
        }

        yield return new WaitForSeconds(0.5f);

        Debug.Log("유저 인벤토리 전송 완료");

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

        isStart = true;
    }

    void Delay()
    {
        isWait = false;
    }
}
