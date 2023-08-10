using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject shopView;

    public RectTransform shopRectTransform;

    public ContentSizeFitter contentSizeFitter;

    public GameObject[] boxArray;
    public LocalizationContent[] boxSSRTextArray;

    public Text[] dailyCountText;

    public Text dailyShopCountText;

    public ShopContent shopContent;

    public Transform shopContentGoldTransform;
    public Transform shopContentTransform;

    List<ShopContent> shopContentGoldList = new List<ShopContent>();
    List<ShopContent> shopContentList = new List<ShopContent>();

    string localization_Reset = "";
    string localization_Days = "";
    string localization_Hours = "";
    string localization_Minutes = "";

    bool isDelay = false;

    public UIManager uIManager;

    PlayerDataBase playerDataBase;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        shopView.SetActive(false);

        //for (int i = 0; i < 6; i++)
        //{
        //    ShopContent monster = Instantiate(shopContent);
        //    monster.transform.parent = shopContentGoldTransform;
        //    monster.transform.position = Vector3.zero;
        //    monster.transform.rotation = Quaternion.identity;
        //    monster.transform.localScale = Vector3.one;
        //    monster.Initialize(ShopType.UpgradeTicket_N + i, MoneyType.Gold, this);
        //    monster.gameObject.SetActive(true);

        //    if (i == 4) monster.gameObject.SetActive(false);

        //    shopContentGoldList.Add(monster);
        //}

        //shopRectTransform.sizeDelta = new Vector2(0, -999);
    }

    private void Start()
    {
        StartCoroutine(DailyShopTimer());
    }

    public void OpenShopView()
    {
        if (!shopView.activeSelf)
        {
            shopView.SetActive(true);

            boxArray[0].SetActive(false);
            boxArray[1].SetActive(false);

            localization_Reset = LocalizationManager.instance.GetString("Reset");
            localization_Days = LocalizationManager.instance.GetString("Days");
            localization_Hours = LocalizationManager.instance.GetString("Hours");
            localization_Minutes = LocalizationManager.instance.GetString("Minutes");

            shopRectTransform.offsetMax = Vector3.zero;

            if (playerDataBase.Formation == 2)
            {
                boxArray[1].SetActive(true);

                boxSSRTextArray[1].forwardText = (100 - playerDataBase.BuyUnderworldBoxSSRCount).ToString();
                boxSSRTextArray[1].localizationName = "BoxSSRInfo";
                boxSSRTextArray[1].ReLoad();
            }
            else
            {
                boxArray[0].SetActive(true);

                boxSSRTextArray[0].forwardText = (100 - playerDataBase.BuySnowBoxSSRCount).ToString();
                boxSSRTextArray[0].localizationName = "BoxSSRInfo";
                boxSSRTextArray[0].ReLoad();
            }

            Initialize_Count();
        }
    }

    public void Change()
    {
        if (shopView.activeSelf)
        {
            if (playerDataBase.Formation == 2)
            {
                boxArray[1].SetActive(true);

                boxSSRTextArray[1].forwardText = (100 - playerDataBase.BuyUnderworldBoxSSRCount).ToString();
                boxSSRTextArray[1].localizationName = "BoxSSRInfo";
                boxSSRTextArray[1].ReLoad();
            }
            else
            {
                boxArray[0].SetActive(true);

                boxSSRTextArray[0].forwardText = (100 - playerDataBase.BuySnowBoxSSRCount).ToString();
                boxSSRTextArray[0].localizationName = "BoxSSRInfo";
                boxSSRTextArray[0].ReLoad();
            }
        }
    }

    void Initialize_Count()
    {
        dailyCountText[0].text = GameStateManager.instance.DailyNormalBox_1 + "/3";
        dailyCountText[1].text = GameStateManager.instance.DailyNormalBox_10 + "/1";
        dailyCountText[2].text = GameStateManager.instance.DailyEpicBox_1 + "/3";
        dailyCountText[3].text = GameStateManager.instance.DailyEpicBox_10 + "/1";
    }

    public void CloseShopView()
    {
        shopView.SetActive(false);
    }

    #region RandomBox
    public void BuySnowBox(int number)
    {
        playerDataBase.SnowBox = number;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox", number);

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);


        //기록
        playerDataBase.BuySnowBox += number;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuySnowBox", playerDataBase.BuySnowBox);
    }

    public void GetSnowBox(BoxType type, int number)
    {
        switch (type)
        {
            case BoxType.Random:
                break;
            case BoxType.N:
                playerDataBase.SnowBox_N = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_N", number);
                break;
            case BoxType.R:
                playerDataBase.SnowBox_R = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_R", number);
                break;
            case BoxType.SR:
                playerDataBase.SnowBox_SR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SR", number);
                break;
            case BoxType.SSR:
                playerDataBase.SnowBox_SSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SSR", number);
                break;
            case BoxType.UR:
                playerDataBase.SnowBox_UR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_UR", number);
                break;
            case BoxType.NR:
                playerDataBase.SnowBox_NR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_NR", number);
                break;
            case BoxType.RSR:
                playerDataBase.SnowBox_RSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_RSR", number);
                break;
            case BoxType.SRSSR:
                playerDataBase.SnowBox_SRSSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SRSSR", number);
                break;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
    }

    public void BuyUnderworldBox(int number)
    {
        playerDataBase.UnderworldBox = number;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", number);

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);


        //기록
        playerDataBase.BuyUnderworldBox += number;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuyUnderworldBox", playerDataBase.BuyUnderworldBox);
    }

    public void GetUnderworld(BoxType type, int number)
    {
        switch (type)
        {
            case BoxType.Random:
                break;
            case BoxType.N:
                playerDataBase.UnderworldBox_N = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_N", number);
                break;
            case BoxType.R:
                playerDataBase.UnderworldBox_R = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_R", number);
                break;
            case BoxType.SR:
                playerDataBase.UnderworldBox_SR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SR", number);
                break;
            case BoxType.SSR:
                playerDataBase.UnderworldBox_SSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SSR", number);
                break;
            case BoxType.UR:
                playerDataBase.UnderworldBox_UR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_UR", number);
                break;
            case BoxType.NR:
                playerDataBase.UnderworldBox_NR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_NR", number);
                break;
            case BoxType.RSR:
                playerDataBase.UnderworldBox_RSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_RSR", number);
                break;
            case BoxType.SRSSR:
                playerDataBase.UnderworldBox_SRSSR = number;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SRSSR", number);
                break;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
    }

    public void Buy_NR(int number)
    {
        int price = 15000 * number;

        if (number >= 10)
        {
            if(GameStateManager.instance.DailyNormalBox_10 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                
                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }

            price -= 15000;
        }
        else
        {
            if (GameStateManager.instance.DailyNormalBox_1 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }
        }

        if (playerDataBase.Gold >= price)
        {
            PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, price);

            switch (GameStateManager.instance.WindCharacterType)
            {
                case WindCharacterType.Winter:
                    GetSnowBox(BoxType.NR, number);
                    break;
                case WindCharacterType.UnderWorld:
                    GetUnderworld(BoxType.NR, number);
                    break;
            }

            if(number >= 10)
            {
                GameStateManager.instance.DailyNormalBox_10 -= 1;
            }
            else
            {
                GameStateManager.instance.DailyNormalBox_1 -= 1;
            }

            Initialize_Count();

            SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
        }
    }

    public void Buy_RSR(int number)
    {
        int price = 100000 * number;

        if (number >= 10)
        {
            if (GameStateManager.instance.DailyEpicBox_10 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }

            price -= 100000;
        }
        else
        {
            if (GameStateManager.instance.DailyEpicBox_1 <= 0)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
                return;
            }
        }

        if (playerDataBase.Gold >= price)
        {
            PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, price);

            switch (GameStateManager.instance.WindCharacterType)
            {
                case WindCharacterType.Winter:
                    GetSnowBox(BoxType.RSR, number);
                    break;
                case WindCharacterType.UnderWorld:
                    GetUnderworld(BoxType.RSR, number);
                    break;
            }

            if (number >= 10)
            {
                GameStateManager.instance.DailyEpicBox_10 -= 1;
            }
            else
            {
                GameStateManager.instance.DailyEpicBox_1 -= 1;
            }

            Initialize_Count();

            SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
        }
    }

    public void BuyItem(ShopType type, int price, int number)
    {
        if (isDelay) return;

        switch (type)
        {
            case ShopType.DailyReward:
                break;
            case ShopType.DailyReward_WatchAd:
                break;
            case ShopType.UpgradeTicket:
                if (playerDataBase.Gold >= price)
                {
                    PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, price);
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                    NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                    return;
                }

                playerDataBase.SetUpgradeTicket(RankType.N, number);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert(type.ToString(), playerDataBase.GetUpgradeTicket(RankType.N));

                NotionManager.instance.UseNotion(NotionType.GetUpgradeTicket);

                break;
        }

        uIManager.Renewal();

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        //NotionManager.instance.UseNotion(NotionType.BuyShopItem);

        isDelay = true;
        Invoke("Delay", 0.5f);
    }

    void Delay()
    {
        isDelay = false;
    }

    IEnumerator DailyShopTimer()
    {
        if (dailyShopCountText.gameObject.activeInHierarchy)
        {
            System.DateTime f = System.DateTime.Now;
            System.DateTime g = System.DateTime.Today.AddDays(1);
            System.TimeSpan h = g - f;

            dailyShopCountText.text = localization_Reset + " : " + h.Hours.ToString("D2") + localization_Hours + " " + h.Minutes.ToString("D2") + localization_Minutes;
        }

        yield return waitForSeconds;
        StartCoroutine(DailyShopTimer());
    }

    #endregion

    #region Developer

    [Button]
    public void PlusMoney()
    {
        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 10000000);
    }

    [Button]
    public void OpenSnowBox_N()
    {
        GetSnowBox(BoxType.N, 10);
    }

    [Button]
    public void OpenSnowBox_R()
    {
        GetSnowBox(BoxType.R, 10);
    }

    [Button]
    public void OpenSnowBox_SR()
    {
        GetSnowBox(BoxType.SR, 10);
    }

    [Button]
    public void OpenSnowBox_SSR()
    {
        GetSnowBox(BoxType.SSR, 10);
    }

    [Button]
    public void OpenSnowBox_UR()
    {
        GetSnowBox(BoxType.UR, 10);
    }

    [Button]
    public void OpenSnowBox_NR()
    {
        GetSnowBox(BoxType.NR, 10);
    }

    [Button]
    public void OpenSnowBox_RSR()
    {
        GetSnowBox(BoxType.RSR, 10);
    }


    [Button]
    public void OpenSnowBox_SRSSR()
    {
        GetSnowBox(BoxType.SRSSR, 10);
    }

    [Button]
    public void OpenUnderworld_N()
    {
        GetUnderworld(BoxType.N, 10);
    }

    [Button]
    public void OpenUnderworld_R()
    {
        GetUnderworld(BoxType.R, 10);
    }

    [Button]
    public void OpenUnderworld_SR()
    {
        GetUnderworld(BoxType.SR, 10);
    }

    [Button]
    public void OpenUnderworld_SSR()
    {
        GetUnderworld(BoxType.SSR, 10);
    }

    [Button]
    public void OpenUnderworld_UR()
    {
        GetUnderworld(BoxType.UR, 10);
    }

    [Button]
    public void OpenUnderworld_NR()
    {
        GetUnderworld(BoxType.NR, 10);
    }

    [Button]
    public void OpenUnderworld_RSR()
    {
        GetUnderworld(BoxType.RSR, 10);
    }

    [Button]
    public void OpenUnderworld_SRSSR()
    {
        GetUnderworld(BoxType.SRSSR, 10);
    }

    #endregion
}
