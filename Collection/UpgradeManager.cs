using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeView;

    public BlockUIContent blockUIContent;

    public Text titleText;
    public Text levelText;
    public Text valueText;

    public Text successText;
    public Text keepText;
    public Text downText;
    public Text destroyText;

    public Text valuePlusText;

    public Text goldText;
    public Text goldNumberText;

    public Text ticketText;
    public Text ticketNumberText;

    public Text equipText;
    public Text upgradeText;
    public Text sellText;

    public GameObject defDestroyObj;
    public Text defDestroyText;
    public Text defDestroyNumberText;

    public Image defCheckMark;

    int gold = 0;
    int upgradeTicket = 0;
    int level = 0;

    bool isWait = false;
    bool isDef = false;

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

    private void Awake()
    {
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
        if (blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        upgradeView.SetActive(false);

        defDestroyObj.SetActive(false);
    }

    public void OpenUpgradeView(string id)
    {
        if (!upgradeView.activeSelf)
        {
            upgradeView.SetActive(true);

            Initialize(id);
        }
    }

    public void CloseUpgradeView()
    {
        upgradeView.SetActive(false);
    }

    void Initialize(string id)
    {
        blockClass = playerDataBase.GetBlockClass(id);

        blockUIContent.Collection_Initialize(blockClass);
        upgradeValue = upgradeDataBase.GetUpgradeValue(blockClass.rankType);
        upgradeInformation = upgradeDataBase.GetUpgradeInformation(blockClass.level + 1);

        titleText.text = blockDataBase.GetBlockName(blockClass.blockType);

        levelText.text = "레벨 " + (blockClass.level + 1).ToString() + " / " + upgradeValue.maxLevel;

        valueText.text = "현재 가치 : " + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level));

        successText.text = "성공 확률 : " + upgradeInformation.success + "%";
        keepText.text = "실패(유지) 확률 : " + upgradeInformation.keep + "%";
        downText.text = "실패(하락) 확률 : " + upgradeInformation.down + "%";
        destroyText.text = "파괴 확률 : " + upgradeInformation.destroy + "%";

        valuePlusText.text = "가치 : " + MoneyUnitString.ToCurrencyString(upgradeValue.GetValueNumber(blockClass.level)) +
            " ▶ " + MoneyUnitString.ToCurrencyString((upgradeValue.GetValueNumber(blockClass.level + 1)));

        gold = playerDataBase.Gold;

        goldText.text = "필요 골드";
        goldNumberText.text = MoneyUnitString.ToCurrencyString(gold) + " / " + MoneyUnitString.ToCurrencyString(upgradeInformation.needGold);

        upgradeTicket = playerDataBase.GetUpgradeTicket(upgradeValue.rankType);

        ticketText.text = "강화권";
        ticketNumberText.text = upgradeTicket + " / 1";

        defDestroyObj.SetActive(false);

        if (blockClass.level + 2 > upgradeValue.maxLevel)
        {
            Debug.Log("최대 레벨입니다");

            successText.text = "";
            keepText.text = "합성시 강화 최대 레벨이 증가합니다";
            downText.text = "합성시 강화 레벨은 그대로 연계됩니다";
            destroyText.text = "";

            goldNumberText.text = "-";
            ticketNumberText.text = "-";
        }
        else
        {
            if (gold >= upgradeInformation.needGold && upgradeTicket >= 1)
            {
                Debug.Log("강화 준비 완료");

                //강화 버튼 색깔을 바꿔야함. 안되면 회색으로
            }

            if(upgradeInformation.destroy > 0f)
            {
                defDestroyObj.SetActive(true);
                defDestroyText.text = "파괴 방지권";
                defDestroyNumberText.text = playerDataBase.DefDestroyTicket + " /1";

                defCheckMark.enabled = false;
                isDef = false;
            }
            else
            {
                defCheckMark.enabled = false;

                isDef = false;
            }
        }

        isWait = false;
    }

    public void UpgradeButton()
    {
        if (isWait) return;

        if (blockClass.level + 2 > upgradeValue.maxLevel)
        {
            Debug.Log("더 이상 강화가 불가능합니다");

            NotionManager.instance.UseNotion(NotionType.MaxBlockLevel);
            return;
        }

        if (gold < upgradeInformation.needGold)
        {
            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
            return;
        }

        if (upgradeTicket < 1)
        {
            NotionManager.instance.UseNotion(NotionType.NotEnoughTicket);
            return;
        }


        PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, upgradeInformation.needGold);

        playerDataBase.UseUpgradeTicket(upgradeValue.rankType);

        switch (upgradeValue.rankType)
        {
            case RankType.N:
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket_N", upgradeTicket - 1);
                break;
            case RankType.R:
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket_R", upgradeTicket - 1);
                break;
            case RankType.SR:
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket_SR", upgradeTicket - 1);
                break;
            case RankType.SSR:
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket_SSR", upgradeTicket - 1);
                break;
            case RankType.UR:
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket_UR", upgradeTicket - 1);
                break;
        }

        float random = Random.Range(0, 100.0f);

        if (random <= upgradeInformation.destroy)
        {
            if (isDef)
            {
                NotionManager.instance.UseNotion(NotionType.DefDestroy);
            }
            else
            {
                SellBlockOne(blockClass.instanceId);

                CloseUpgradeView();

                NotionManager.instance.UseNotion(NotionType.UpgradeDestroy);
            }
        }
        else if (random <= upgradeInformation.down)
        {
            int level = blockClass.level - 1;

            customData.Clear();
            customData.Add("Level", (level).ToString());

            playerDataBase.SetBlockLevel(blockClass.instanceId, level);
            collectionManager.SetBlockLevel(blockClass.instanceId, level);
            blockUIContent.SetLevel(level);

            PlayfabManager.instance.SetInventoryCustomData(blockClass.instanceId, customData);

            NotionManager.instance.UseNotion(NotionType.UpgradeDown);
        }
        else if (random <= upgradeInformation.keep)
        {
            //Nothing

            NotionManager.instance.UseNotion(NotionType.UpgradeKeep);
        }
        else
        {
            int level = blockClass.level + 1;

            customData.Clear();
            customData.Add("Level", (level).ToString());

            playerDataBase.SetBlockLevel(blockClass.instanceId, level);
            collectionManager.SetBlockLevel(blockClass.instanceId, level);
            equipManager.SetBlockLevel(blockClass.instanceId, level);
            blockUIContent.SetLevel(level);

            PlayfabManager.instance.SetInventoryCustomData(blockClass.instanceId, customData);

            NotionManager.instance.UseNotion(NotionType.UpgradeSuccess);
        }

        if (isDef)
        {
            playerDataBase.DefDestroyTicket -= 1;

            defDestroyNumberText.text = playerDataBase.DefDestroyTicket + " /1";

            PlayfabManager.instance.UpdatePlayerStatisticsInsert("DefDestroyTicket", playerDataBase.DefDestroyTicket);

            CheckDefDestroyTicket();
        }

        Initialize(blockClass.instanceId);

        isWait = true;
        Invoke("Delay", 0.5f);
    }

    void Delay()
    {
        isWait = false;
    }

    public void SellBlock(string id)
    {
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
        if(!collectionManager.equipManager.CheckEquipBlock(blockClass.instanceId))
        {
            sellManager.OpenSellView(blockClass, upgradeValue.GetValueNumber(blockClass.level));
        }
        else
        {
            NotionManager.instance.UseNotion(NotionType.DontSellEquipBlock);
        }
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

        equipManager.OpenEquipView(blockClass);
    }
}
