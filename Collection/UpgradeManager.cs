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
    public LocalizationContent storyText;
    public Text levelText;
    public Image levelFillamount;
    public LocalizationContent valueText;
    public LocalizationContent blockValueText;

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

    public CollectionManager collectionManager;
    public SellManager sellManager;
    public EquipManager equipManager;

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
        }
    }

    public void CloseUpgradeView()
    {
        upgradeView.SetActive(false);
    }

    void Initialize()
    {
        blockUIContent.Collection_Initialize(blockClass);
        upgradeValue = upgradeDataBase.GetUpgradeValue(blockClass.rankType);
        upgradeInformation = upgradeDataBase.GetUpgradeInformation(blockClass.level + 1);

        titleText.localizationName = blockClass.blockType.ToString();
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

        successText.localizationName = "SuccessPercent";
        successText.plusText = " : " + upgradeInformation.success + "%";
        successText.ReLoad();

        keepText.localizationName = "RetentionPercent";
        keepText.plusText = " : " + upgradeInformation.keep + "%";
        keepText.ReLoad();

        downText.localizationName = "LowerPercent";
        downText.plusText = " : " + upgradeInformation.down + "%";
        downText.ReLoad();

        destroyText.localizationName = "DestroyPercent";
        destroyText.plusText = " : " + upgradeInformation.destroy + "%";
        destroyText.ReLoad();

        valuePlusText.localizationName = "Value";
        valuePlusText.plusText = " : " + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) +
            "   ▶   <color=#FFCA14>" + MoneyUnitString.ToCurrencyString((upgradeValue.GetValueNumber(blockClass.level + 1))) + "</color>";

        valuePlusText.ReLoad();

        goldObj.SetActive(true);

        gold = playerDataBase.Gold;

        goldText.localizationName = "NeedGold_Upgrade";
        goldText.ReLoad();

        goldNumberText.text = MoneyUnitString.ToCurrencyString(upgradeInformation.needGold);

        upgradeTicket = playerDataBase.GetUpgradeTicket(RankType.N);

        needTicket = upgradeDataBase.GetNeedTicket(blockClass.level + 1);

        if (blockClass.level >= 6)
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
            if(blockClass.rankType != RankType.SSR)
            {
                //successText.localizationName = "NextSynthesisInfo";
                successText.localizationName = "";
                successText.plusText = "";
                keepText.localizationName = "NextSynthesisInfo";
                keepText.plusText = "";
                downText.localizationName = "";
                destroyText.localizationName = "";

                goldNumberText.text = "-";
                ticketNumberText.text = "-";
            }
            else
            {
                //successText.localizationName = "MaxLevel";
                successText.localizationName = "";
                successText.plusText = "";
                keepText.localizationName = "";
                keepText.plusText = "";
                downText.localizationName = "MaxLevel";
                downText.plusText = "";
                destroyText.localizationName = "";
                valuePlusText.localizationName = "";

                goldNumberText.text = "-";
                ticketNumberText.text = "-";
            }

            successText.ReLoad();
            keepText.ReLoad();
            downText.ReLoad();
            destroyText.ReLoad();
            valuePlusText.ReLoad();

            goldObj.SetActive(false);
            ticketObj.SetActive(false);

            Debug.Log("최대 레벨입니다");
        }
        else
        {
            if (blockClass.level >= 6)
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


        PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, upgradeInformation.needGold);

        playerDataBase.UseUpgradeTicket(RankType.N, needTicket);

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));

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

            int level = blockClass.level - 1;

            customData.Clear();
            customData.Add("Level", (level).ToString());

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

            int level = blockClass.level + 1;

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

        FirebaseAnalytics.LogEvent("Upgrade");

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
                break;
            case 1:
                upgradeScreenTitle.text = LocalizationManager.instance.GetString("FailureUpgrade");
                upgradeScreenLevel.text = (blockClass.level + 1).ToString() + "   ▶   <color=#FFCA14>" + (blockClass.level).ToString() + "</color>";
                upgradeScreenValue.text = MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) + "    ▶    <color=#FFCA14>"
                    + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level - 1)) + "</color>";

                SoundManager.instance.PlaySFX(GameSfxType.BlockUpgradeFail);
                break;
            case 2:
                upgradeScreenTitle.text = LocalizationManager.instance.GetString("RetentionUpgrade");
                upgradeScreenLevel.text = (blockClass.level + 1).ToString() + "   ▶   <color=#FFCA14>" + (blockClass.level + 1).ToString() + "</color>";
                upgradeScreenValue.text = MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) + "    ▶    <color=#FFCA14>"
                    + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) + "</color>";

                SoundManager.instance.PlaySFX(GameSfxType.BlockUpgradeFail);
                break;
            case 3:
                upgradeScreenEffect.SetActive(true);

                upgradeScreenTitle.text = LocalizationManager.instance.GetString("SuccessUpgrade");
                upgradeScreenLevel.text = (blockClass.level + 1).ToString() + "   ▶   <color=#FFCA14>" + (blockClass.level + 2).ToString() + "</color>";
                upgradeScreenValue.text = MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) + "    ▶    <color=#FFCA14>"
                    + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level + 1)) + "</color>";

                SoundManager.instance.PlaySFX(GameSfxType.BlockUpgradeSuccess);
                break;
            case 4:
                upgradeScreenIcon.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);

                upgradeScreenTitle.text = LocalizationManager.instance.GetString("FailureDestroyUpgrade");

                upgradeScreenLevelName.text = "";
                upgradeScreenLevel.text = LocalizationManager.instance.GetString("DestroyBlockInfo");
                upgradeScreenValueName.text = "";
                upgradeScreenValue.text = "";

                pieceObj.SetActive(true);
                pieceImg.sprite = rankBackgroundArray[(int)blockClass.rankType];

                switch (blockClass.rankType)
                {
                    case RankType.N:
                        playerDataBase.BoxPiece_N += 1;

                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_N", playerDataBase.BoxPiece_N);
                        break;
                    case RankType.R:
                        playerDataBase.BoxPiece_R += 1;

                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_R", playerDataBase.BoxPiece_R);
                        break;
                    case RankType.SR:
                        playerDataBase.BoxPiece_SR += 1;

                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_SR", playerDataBase.BoxPiece_SR);
                        break;
                    case RankType.SSR:
                        playerDataBase.BoxPiece_SSR += 1;

                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_SSR", playerDataBase.BoxPiece_SSR);
                        break;
                    case RankType.UR:
                        playerDataBase.BoxPiece_UR += 1;

                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_UR", playerDataBase.BoxPiece_UR);
                        break;
                }

                equipManager.CheckUnEquip(blockClass.instanceId);
                collectionManager.CheckUnEquip(blockClass.instanceId);

                SellBlockOne(blockClass.instanceId);
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
        Debug.LogError("삭제 : " + id);

        PlayfabManager.instance.RevokeConsumeItem(id);
        playerDataBase.SellBlock(id);
    }

    public void SellBlockOne(string id)
    {
        PlayfabManager.instance.RevokeConsumeItem(id);
        playerDataBase.SellBlock(id);

        for(int i = 0; i < collectionManager.blockUIContentList.Count; i ++)
        {
            if(collectionManager.blockUIContentList[i].instanceId.Equals(id))
            {
                collectionManager.blockUIContentList[i].gameObject.SetActive(false);
            }
        }
    }

    public void SellButton()
    {
        if (blockClass.instanceId == null)
        {
            Debug.Log("잘못된 블럭 입니다");
            return;
        }

        if(blockClass.rankType < RankType.SR)
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotSellBlock);
            return;
        }

        //switch(playerDataBase.CheckEquip(blockClass.instanceId))
        //{
        //    case 0:
        //        break;
        //    default:
        //        equipManager.CheckUnEquip(blockClass.instanceId);
        //        break;
        //}

        sellManager.OpenSellView(blockClass, upgradeValue.GetValueNumber(blockClass.level) * 100);

        //if(!collectionManager.equipManager.CheckEquip(blockClass.instanceId))
        //{
        //    sellManager.OpenSellView(blockClass, upgradeValue.GetValueNumber(blockClass.level));
        //}
        //else
        //{
        //    NotionManager.instance.UseNotion(NotionType.DontSellEquipBlock);
        //}
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
        //if(blockClass.blockType != BlockType.Pawn_Under)
        //{
        //    equipManager.OpenEquipView(blockClass);
        //}
        //else
        //{
        //    equipManager.ChangeNewbie(blockClass);
        //}

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
}
