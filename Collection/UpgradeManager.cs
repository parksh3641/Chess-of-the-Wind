using Firebase.Analytics;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeView;

    public BlockUIContent blockUIContent;

    public LocalizationContent titleText;
    public Text levelText;
    public Image levelFillamount;
    public LocalizationContent valueText;
    public LocalizationContent blockValueText;

    public LocalizationContent blockAbilityText1;
    public LocalizationContent blockAbilityText2;

    public LocalizationContent storyText;

    public LocalizationContent successText;
    public LocalizationContent keepText;
    public LocalizationContent downText;
    public LocalizationContent destroyText;

    public LocalizationContent valuePlusText;

    public LocalizationContent goldText;
    public Text goldNumberText;

    public LocalizationContent ticketText;
    public Text ticketNumberText;

    public Image ticketImg;
    public Sprite[] ticketImgArray;

    public LocalizationContent equipText;

    public GameObject goldObj;
    public GameObject ticketObj;
    public GameObject defDestroyObj;
    public LocalizationContent defDestroyText;
    public Text defDestroyNumberText;

    public Image defCheckMark;

    public Image upgradeButton;
    public Sprite[] upgradeButtonArray;

    public GameObject testMode;

    int gold = 0;
    int upgradeTicket = 0;
    int level = 0;
    int needTicket = 0;

    bool isWait = false;
    bool isDef = false;
    bool unEquip = false;

    [Title("Upgrade Screen")]
    public GameObject upgradeScreen;

    public GameObject upgradeScreenEffect;
    public Text upgradeScreenTitle;
    public Image upgradeScreenIcon;
    public Text upgradeScreenLevelName;
    public Text upgradeScreenLevel;
    public Text upgradeScreenValueName;
    public Text upgradeScreenValue;

    public GameObject pieceObj;
    public Image pieceImg;

    public GameObject tapToContinue;

    Sprite[] blockArray;
    Sprite[] rankBackgroundArray;

    Dictionary<string, string> customData = new Dictionary<string, string>();

    public List<string> sellBlockList = new List<string>();

    public CollectionManager collectionManager;
    public SellManager sellManager;
    public EquipManager equipManager;
    public TitleManager titleManager;
    public InventoryManager inventoryManager;

    Color gray = new Color(200 / 255f, 200 / 255f, 200 / 255f);

    BlockClass blockClass;
    UpgradeValue upgradeValue;
    UpgradeInformation upgradeInformation;

    UpgradeDataBase upgradeDataBase;
    BlockDataBase blockDataBase;
    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
        if (blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        blockArray = imageDataBase.GetBlockArray();
        rankBackgroundArray = imageDataBase.GetRankBackgroundArray();

        upgradeView.SetActive(false);
        upgradeScreen.SetActive(false);

        ticketObj.SetActive(false);
        defDestroyObj.SetActive(false);

        testMode.SetActive(false);
    }

    public void OpenUpgradeView(string id)
    {
        if (!upgradeView.activeSelf)
        {
            upgradeView.SetActive(true);

            blockClass = playerDataBase.GetBlockClass(id);

            if(blockClass.blockType != BlockType.Default)
            {
                Initialize();
            }

            if(playerDataBase.TestAccount > 0)
            {
                testMode.SetActive(true);
            }
        }
    }

    public void CloseUpgradeView()
    {
        upgradeView.SetActive(false);
    }

    void Initialize()
    {
        blockUIContent.Initialize_UI(blockClass);
        upgradeValue = upgradeDataBase.GetUpgradeValue(blockClass.rankType);
        upgradeInformation = upgradeDataBase.GetUpgradeInformation(blockClass.level + 1);

        titleText.localizationName = blockClass.blockType.ToString();
        titleText.plusText = "";
        if (blockClass.rankType >= RankType.SSR && blockClass.ssrLevel > 0)
        {
            titleText.plusText = " +" + blockClass.ssrLevel.ToString();
        }
        titleText.ReLoad();

        storyText.localizationName = blockClass.blockType + "_Story";
        storyText.ReLoad();

        levelText.text = "Lv. " + (blockClass.level + 1).ToString() + "/" + upgradeValue.maxLevel;
        levelFillamount.fillAmount = (blockClass.level + 1) * 1.0f / upgradeValue.maxLevel * 1.0f;

        valueText.localizationName = "CurrentValue";
        valueText.plusText = " : <color=#FFCA14>" + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) + "</color>";
        valueText.ReLoad();

        blockValueText.localizationName = "BlockValue";
        blockValueText.plusText = " : <color=#FFCA14>" + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level) / blockDataBase.GetSize(blockClass.blockType)) + "</color>";
        blockValueText.ReLoad();

        blockAbilityText1.localizationName = "BlockAbility1";
        blockAbilityText1.plusText = " + 1%";
        blockAbilityText1.GetComponent<Text>().color = gray;
        if (blockClass.rankType >= RankType.SSR)
        {
            blockAbilityText1.GetComponent<Text>().color = Color.white;
            if (blockClass.ssrLevel > 0)
            {
                blockAbilityText1.plusText = " + " + (blockClass.ssrLevel + 1).ToString() + "%";
            }

            if (blockClass.rankType == RankType.UR)
            {
                blockAbilityText1.plusText = " + 5%";
            }
        }
        blockAbilityText1.ReLoad();

        blockAbilityText2.localizationName = "BlockAbility2";
        blockAbilityText2.plusText = " + 1%";
        blockAbilityText2.ReLoad();
        blockAbilityText2.GetComponent<Text>().color = gray;
        if (blockClass.rankType >= RankType.UR)
        {
            blockAbilityText2.GetComponent<Text>().color = Color.white;
        }

        successText.localizationName = "SuccessPercent";
        successText.plusText = " : " + upgradeInformation.success + "%";
        successText.ReLoad();

        //keepText.localizationName = "RetentionPercent";
        //keepText.plusText = " : " + upgradeInformation.keep + "%";
        //keepText.ReLoad();

        downText.localizationName = "LowerPercent";
        downText.plusText = " : " + upgradeInformation.down + "%";
        downText.ReLoad();

        destroyText.localizationName = "DestroyPercent";
        destroyText.plusText = " : " + upgradeInformation.destroy + "%";
        destroyText.ReLoad();

        //if (upgradeInformation.destroy > 0)
        //{
        //    destroyText.localizationName = "DestroyPercent";
        //    destroyText.plusText = " : " + upgradeInformation.destroy + "%";
        //    destroyText.ReLoad();
        //}
        //else
        //{
        //    destroyText.forwardText = "";
        //    destroyText.plusText = "";
        //    destroyText.ReLoad();
        //}

        valuePlusText.localizationName = "Value";
        valuePlusText.plusText = " : " + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) +
            "   ▶   <color=#FFCA14>" + MoneyUnitString.ToCurrencyString((upgradeValue.GetValueNumber(blockClass.level + 1))) + "</color>";

        valuePlusText.ReLoad();

        goldObj.SetActive(true);

        UIManager.instance.Renewal();

        gold = (int)playerDataBase.Coin;

        goldText.localizationName = "NeedGold_Upgrade";
        goldText.ReLoad();

        goldNumberText.text = MoneyUnitString.ToCurrencyString(upgradeInformation.needGold);

        upgradeTicket = playerDataBase.GetUpgradeTicket(RankType.N);

        needTicket = upgradeDataBase.GetNeedTicket(blockClass.level + 1);

        if (blockClass.level > 0)
        {
            ticketObj.SetActive(true);

            ticketText.localizationName = "UpgradeTicket";
            ticketText.ReLoad();

            ticketNumberText.text = upgradeTicket + "/" + needTicket;

            ticketImg.sprite = ticketImgArray[(int)RankType.SR];
        }
        else
        {
            ticketObj.SetActive(false);
        }

        defDestroyObj.SetActive(false);

        if(playerDataBase.CheckEquip(blockClass.instanceId) > 0)
        {
            equipText.localizationName = "UnEquip";
            equipText.ReLoad();           

            unEquip = true;
        }
        else
        {
            equipText.localizationName = "Equip";
            equipText.ReLoad();

            unEquip = false;
        }

        if (blockClass.level + 2 > upgradeValue.maxLevel)
        {
            if(blockClass.rankType != RankType.UR)
            {
                valuePlusText.localizationName = "MaxLevel";
                valuePlusText.plusText = "";

            //    valuePlusText.localizationName = "MaxUpgradeLevel";
            //    valuePlusText.plusText = " : " + upgradeDataBase.GetUpgradeValue(blockClass.rankType).maxLevel +
            //"   ▶   <color=#FFCA14>" + upgradeDataBase.GetUpgradeValue(blockClass.rankType + 1).maxLevel + "</color>";


                //if (blockClass.rankType == RankType.SSR)
                //{
                //    if(blockClass.ssrLevel >= 4)
                //    {
                //        downText.localizationName = "NextSynthesisBlockAbility2";
                //    }
                //    else
                //    {
                //        downText.localizationName = "NextSynthesisInfoSSR";

                //        valuePlusText.localizationName = "";
                //        valuePlusText.plusText = "";
                //    }
                //}
                //else if (blockClass.rankType == RankType.SR)
                //{
                //    downText.localizationName = "NextSynthesisBlockAbility1";
                //}
                //else
                //{
                //    downText.localizationName = "NextSynthesisInfo";
                //}

                successText.localizationName = "";
                successText.plusText = "";

                //keepText.localizationName = "";
                //keepText.plusText = "";

                downText.localizationName = "";
                downText.plusText = "";

                destroyText.localizationName = "";

                goldNumberText.text = "-";
                ticketNumberText.text = "-";
            }
            else
            {
                successText.localizationName = "";
                successText.plusText = "";

                //keepText.localizationName = "";
                //keepText.plusText = "";

                downText.localizationName = "MaxLevel";
                downText.plusText = "";

                destroyText.localizationName = "";

                valuePlusText.localizationName = "";
                valuePlusText.plusText = "";

                goldNumberText.text = "-";
                ticketNumberText.text = "-";
            }

            successText.ReLoad();
            //keepText.ReLoad();
            downText.ReLoad();
            destroyText.ReLoad();
            valuePlusText.ReLoad();

            goldObj.SetActive(false);
            ticketObj.SetActive(false);

            Debug.Log("최대 레벨입니다");
        }
        else
        {
            if (blockClass.level > 0)
            {
                if(upgradeTicket >= needTicket && gold >= upgradeInformation.needGold)
                {
                    upgradeButton.sprite = upgradeButtonArray[1];

                    Debug.Log("강화 준비 완료");
                }
                else
                {
                    upgradeButton.sprite = upgradeButtonArray[0];
                }
            }
            else
            {
                if (playerDataBase.ChallengeCount < 5)
                {
                    if (blockClass.level < 1 && gold >= upgradeInformation.needGold)
                    {
                        upgradeButton.sprite = upgradeButtonArray[1];

                        Debug.Log("강화 준비 완료");
                    }
                    else
                    {
                        upgradeButton.sprite = upgradeButtonArray[0];
                    }
                }
                else
                {
                    if (gold >= upgradeInformation.needGold)
                    {
                        upgradeButton.sprite = upgradeButtonArray[1];

                        Debug.Log("강화 준비 완료");
                    }
                    else
                    {
                        upgradeButton.sprite = upgradeButtonArray[0];
                    }
                }
            }

            //if(upgradeInformation.destroy > 0f)
            //{
            //    defDestroyObj.SetActive(true);
            //    defDestroyText.localizationName = "DefDestroyTicket";
            //    defDestroyText.ReLoad();
            //    defDestroyNumberText.text = playerDataBase.DefDestroyTicket + " /1";

            //    defCheckMark.enabled = false;
            //    isDef = false;
            //}
            //else
            //{
            //    defCheckMark.enabled = false;

            //    isDef = false;
            //}
        }

        titleManager.CheckGoal();
    }

    public void UpgradeButton()
    {
        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        if(playerDataBase.ChallengeCount < 5)
        {
            if(blockClass.level + 1 >= 2)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.MaxBlockLevel);
                return;
            }
        }

        if (blockClass.level + 2 > upgradeValue.maxLevel)
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.MaxBlockLevel);
            return;
        }

        if (gold < upgradeInformation.needGold)
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
            return;
        }

        if (blockClass.level >= 6 && upgradeTicket < needTicket)
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotEnoughTicket);
            return;
        }

        PlayfabManager.instance.UpdateSubtractGold(upgradeInformation.needGold);

        playerDataBase.UseUpgradeTicketCount(RankType.N, needTicket);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));

        //playerDataBase.UseUpgradeTicket += needTicket;
        //PlayfabManager.instance.UpdatePlayerStatisticsInsert("UseUpgradeTicket", playerDataBase.UseUpgradeTicket);

        //switch (upgradeValue.rankType)
        //{
        //    case RankType.N:
        //        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket_N", upgradeTicket - 1);
        //        break;
        //    case RankType.R:
        //        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket_R", upgradeTicket - 1);
        //        break;
        //    case RankType.SR:
        //        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket_SR", upgradeTicket - 1);
        //        break;
        //    case RankType.SSR:
        //        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket_SSR", upgradeTicket - 1);
        //        break;
        //    case RankType.UR:
        //        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket_UR", upgradeTicket - 1);
        //        break;
        //}

        float random = Random.Range(0, 100.0f);

        if (random <= upgradeInformation.destroy)
        {
            if (isDef)
            {
                //NotionManager.instance.UseNotion(NotionType.DefDestroy);

                OpenUpgradeScreen(0); //실패했지만 파괴가 안됨
            }
            else
            {
                SellBlockOne(blockClass.instanceId);

                CloseUpgradeView();

                OpenUpgradeScreen(4);

                //NotionManager.instance.UseNotion(NotionType.UpgradeDestroy);
            }
        }
        else if (random <= upgradeInformation.down)
        {
            OpenUpgradeScreen(1);

            level = blockClass.level - 1;

            customData.Clear();
            customData.Add("Level", (level).ToString());
            customData.Add("SSRLevel", "0");

            playerDataBase.SetBlockLevel(blockClass.instanceId, level);
            collectionManager.SetBlockLevel(blockClass.instanceId, level);
            blockUIContent.SetLevel(level);

            PlayfabManager.instance.SetInventoryCustomData(blockClass.instanceId, customData);

            //NotionManager.instance.UseNotion(NotionType.UpgradeDown);
        }
        else if (random <= upgradeInformation.keep)
        {
            //Nothing

            //NotionManager.instance.UseNotion(NotionType.UpgradeKeep);

            OpenUpgradeScreen(2);
        }
        else
        {
            OpenUpgradeScreen(3);

            level = blockClass.level + 1;

            customData.Clear();
            customData.Add("Level", (level).ToString());

            playerDataBase.SetBlockLevel(blockClass.instanceId, level);
            collectionManager.SetBlockLevel(blockClass.instanceId, level);
            equipManager.SetBlockLevel(blockClass.instanceId, level);
            blockUIContent.SetLevel(level);

            PlayfabManager.instance.SetInventoryCustomData(blockClass.instanceId, customData);

            //NotionManager.instance.UseNotion(NotionType.UpgradeSuccess);
        }

        if (isDef)
        {
            playerDataBase.DefDestroyTicket -= 1;

            defDestroyNumberText.text = playerDataBase.DefDestroyTicket + " /1";

            PlayfabManager.instance.UpdatePlayerStatisticsInsert("DefDestroyTicket", playerDataBase.DefDestroyTicket);

            CheckDefDestroyTicket();
        }

        FirebaseAnalytics.LogEvent("Play_Upgrade : " + level);

        Initialize();

        isWait = true;
        Invoke("Delay", 0.5f);
    }

    void OpenUpgradeScreen(int number)
    {
        upgradeScreen.SetActive(true);

        tapToContinue.SetActive(false);

        pieceObj.SetActive(false);

        upgradeScreenIcon.sprite = blockArray[(int)blockUIContent.blockClass.blockType - 1];
        upgradeScreenIcon.color = new Color(1, 1, 1);

        upgradeScreenEffect.SetActive(false);

        upgradeScreenLevelName.text = LocalizationManager.instance.GetString("Level");
        upgradeScreenValueName.text = LocalizationManager.instance.GetString("Value");

        switch (number)
        {
            case 0:
                upgradeScreenTitle.text = LocalizationManager.instance.GetString("FailureDefUpgrade");
                upgradeScreenLevel.text = (blockClass.level + 1).ToString() + "   ▶   <color=#FFCA14>" + (blockClass.level + 1).ToString() + "</color>";
                upgradeScreenValue.text = MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) + "    ▶    <color=#FFCA14>"
                    + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) + "</color>";

                SoundManager.instance.PlaySFX(GameSfxType.BlockUpgradeFail);

                FirebaseAnalytics.LogEvent("Play_Upgrade_FailureDef");
                break;
            case 1:
                upgradeScreenTitle.text = LocalizationManager.instance.GetString("FailureUpgrade");
                upgradeScreenLevel.text = (blockClass.level + 1).ToString() + "   ▶   <color=#FFCA14>" + (blockClass.level).ToString() + "</color>";
                upgradeScreenValue.text = MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) + "    ▶    <color=#FFCA14>"
                    + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level - 1)) + "</color>";

                SoundManager.instance.PlaySFX(GameSfxType.BlockUpgradeFail);

                FirebaseAnalytics.LogEvent("Play_Upgrade_Failure");
                break;
            case 2:
                upgradeScreenTitle.text = LocalizationManager.instance.GetString("RetentionUpgrade");
                upgradeScreenLevel.text = (blockClass.level + 1).ToString() + "   ▶   <color=#FFCA14>" + (blockClass.level + 1).ToString() + "</color>";
                upgradeScreenValue.text = MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) + "    ▶    <color=#FFCA14>"
                    + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) + "</color>";

                SoundManager.instance.PlaySFX(GameSfxType.BlockUpgradeFail);

                FirebaseAnalytics.LogEvent("Play_Upgrade_Retention");
                break;
            case 3:
                upgradeScreenEffect.SetActive(true);

                upgradeScreenTitle.text = LocalizationManager.instance.GetString("SuccessUpgrade");
                upgradeScreenLevel.text = (blockClass.level + 1).ToString() + "   ▶   <color=#FFCA14>" + (blockClass.level + 2).ToString() + "</color>";
                upgradeScreenValue.text = MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) + "    ▶    <color=#FFCA14>"
                    + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level + 1)) + "</color>";

                playerDataBase.UpgradeSuccessCount += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeSuccessCount", playerDataBase.UpgradeSuccessCount);

                SoundManager.instance.PlaySFX(GameSfxType.BlockUpgradeSuccess);

                FirebaseAnalytics.LogEvent("Play_Upgrade_Success");
                break;
            case 4:
                upgradeScreenIcon.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);

                upgradeScreenTitle.text = LocalizationManager.instance.GetString("FailureDestroyUpgrade");

                upgradeScreenLevelName.text = "";
                upgradeScreenLevel.text = LocalizationManager.instance.GetString("DestroyBlockInfo");
                upgradeScreenValueName.text = "";
                upgradeScreenValue.text = "";

                pieceObj.SetActive(false);
                pieceImg.sprite = rankBackgroundArray[(int)blockClass.rankType];

                inventoryManager.mainAlarm.SetActive(true);

                //ItemAnimManager.instance.GetBoxPiece(blockClass.rankType, 1);

                equipManager.CheckUnEquip(blockClass.instanceId);
                collectionManager.CheckUnEquip(blockClass.instanceId);

                SellBlockOne(blockClass.instanceId);

                playerDataBase.DestroyBlockCount += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("DestroyBlockCount", playerDataBase.DestroyBlockCount);

                isWait = true;
                Invoke("Delay", 2.0f);

                FirebaseAnalytics.LogEvent("Play_Upgrade_Destroy");
                break;
        }
    }

    public void CloseUpgradeScreen()
    {
        if(!isWait)
        {
            upgradeScreen.SetActive(false);
        }
    }

    void Delay()
    {
        isWait = false;

        tapToContinue.SetActive(true);
    }

    public void SellBlock(string id)
    {
        PlayfabManager.instance.DeleteInventoryItem(id);
        playerDataBase.SellBlock(id);

        //Debug.LogError("삭제 : " + id);
    }

    public void SetSellBlock(string id)
    {
        sellBlockList.Add(id);
        playerDataBase.SellBlock(id);

        //Debug.LogError("삭제할 블럭 : " + id);
    }

    public void SellBlockAll()
    {
        PlayfabManager.instance.DeleteInventoryItems(sellBlockList);

        Debug.LogError("일괄 삭제");
    }


    public void SellBlockOne(string id)
    {
        PlayfabManager.instance.DeleteInventoryItem(id);
        playerDataBase.SellBlock(id);

        for(int i = 0; i < collectionManager.blockUIContentList.Count; i ++)
        {
            if(collectionManager.blockUIContentList[i].instanceId.Equals(id))
            {
                collectionManager.blockUIContentList[i].gameObject.SetActive(false);
            }
        }

        collectionManager.CheckTotalRaf();
    }

    public void SellButton()
    {
        if (blockClass.instanceId == null)
        {
            Debug.Log("잘못된 블럭 입니다");
            return;
        }

        //if(blockClass.rankType < RankType.R)
        //{
        //    SoundManager.instance.PlaySFX(GameSfxType.Wrong);

        //    NotionManager.instance.UseNotion(NotionType.NotSellBlock);
        //    return;
        //}

        sellManager.OpenSellView(blockClass, upgradeValue.GetValueNumber(blockClass.level));
    }

    void CheckDefDestroyTicket()
    {
        if(playerDataBase.DefDestroyTicket <= 0)
        {
            defCheckMark.enabled = false;

            isDef = false;
        }
    }

    public void UseDefDestroyTicket()
    {
        if(!isDef)
        {
            if(playerDataBase.DefDestroyTicket > 0)
            {
                defCheckMark.enabled = true;

                isDef = true;
            }
        }
        else
        {
            defCheckMark.enabled = false;

            isDef = false;

            NotionManager.instance.UseNotion(NotionType.NotBuyDailyLimit);
        }

        Debug.Log("파괴 방지권 : " + isDef);
    }

    public void EquipButton()
    {
        CloseUpgradeView();

        if(unEquip)
        {
            equipManager.CheckUnEquip(blockClass.instanceId);
            collectionManager.CheckUnEquip(blockClass.instanceId);
        }
        else
        {
            equipManager.OpenEquipView(blockClass);
        }
    }


    public void MaxLevel()
    {
        if (isWait) return;

        int level = 0;

        switch (blockClass.rankType)
        {
            case RankType.N:
                level = 4;
                break;
            case RankType.R:
                level = 9;
                break;
            case RankType.SR:
                level = 14;
                break;
            case RankType.SSR:
                level = 19;
                break;
            case RankType.UR:
                level = 24;
                break;
        }

        if (blockClass.level < level)
        {
            blockClass.level = level;

            customData.Clear();
            customData.Add("Level", (level).ToString());

            playerDataBase.SetBlockLevel(blockClass.instanceId, level);
            collectionManager.SetBlockLevel(blockClass.instanceId, level);
            equipManager.SetBlockLevel(blockClass.instanceId, level);
            blockUIContent.SetLevel(level);

            PlayfabManager.instance.SetInventoryCustomData(blockClass.instanceId, customData);

            Initialize();

            isWait = true;
            Invoke("Delay", 0.5f);
        }

        collectionManager.CheckTotalRaf();
    }

    public void ResetLevel()
    {
        if (isWait) return;

        if (blockClass.level != 0)
        {
            int level = 0;

            blockClass.level = 0;

            customData.Clear();
            customData.Add("Level", (level).ToString());

            playerDataBase.SetBlockLevel(blockClass.instanceId, level);
            collectionManager.SetBlockLevel(blockClass.instanceId, level);
            equipManager.SetBlockLevel(blockClass.instanceId, level);
            blockUIContent.SetLevel(level);

            PlayfabManager.instance.SetInventoryCustomData(blockClass.instanceId, customData);

            Initialize();

            isWait = true;
            Invoke("Delay", 0.5f);
        }
    }
}
