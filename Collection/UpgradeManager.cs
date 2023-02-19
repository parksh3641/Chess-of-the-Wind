using System.Collections;
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

    int gold = 0;
    int upgradeTicket = 0;

    BlockClass blockClass;
    UpgradeValue upgradeValue;
    UpgradeInformation upgradeInformation;
    UpgradeInformation upgradeInformation2;

    UpgradeDataBase upgradeDataBase;
    BlockDataBase blockDataBase;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
        if (blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        upgradeView.SetActive(false);
    }

    public void OpenUpgradeView(string id)
    {
        if(!upgradeView.activeSelf)
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
        upgradeInformation2 = upgradeDataBase.GetUpgradeInformation(blockClass.level + 2);

        titleText.text = blockDataBase.GetBlockName(blockClass.blockType);

        levelText.text = "레벨 " + (blockClass.level + 1).ToString() + " / " + upgradeValue.maxLevel;

        valueText.text = "현재 가치 : " + upgradeInformation.value;

        successText.text = "성공 확률 : " + upgradeInformation.success + "%";
        keepText.text = "실패(유지) 확률 : " + upgradeInformation.keep + "%";
        downText.text = "실패(하락) 확률 : " + upgradeInformation.down + "%";
        destroyText.text = "파괴 확률 : " + upgradeInformation.destroy + "%";

        valuePlusText.text = "가치 : " + upgradeInformation.value + " ▶ " + upgradeInformation2.value;

        gold = playerDataBase.Gold;

        goldText.text = "필요 골드";
        goldNumberText.text = gold + " / " + upgradeInformation.needGold;

        upgradeTicket = playerDataBase.GetUpgradeTicket(upgradeValue.rankType);

        ticketText.text = "강화권";
        ticketNumberText.text = upgradeTicket + " / 1";

        if(gold >= upgradeInformation.needGold && upgradeTicket >= 1)
        {
            Debug.Log("강화 준비 완료");
        }
    }

    public void EquipButton()
    {

    }

    public void UpgradeButton()
    {
        if(gold >= upgradeInformation.needGold)
        {
            if(upgradeTicket >= 1)
            {
                if(PlayfabManager.instance.isActive)
                {
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
                }

                float random = Random.Range(0, 100.0f);

                if(random <= upgradeInformation.destroy)
                {


                    NotionManager.instance.UseNotion(NotionType.UpgradeDestroy);
                }
                else if (random <= upgradeInformation.down)
                {


                    NotionManager.instance.UseNotion(NotionType.UpgradeDown);
                }
                else if (random <= upgradeInformation.keep)
                {


                    NotionManager.instance.UseNotion(NotionType.UpgradeKeep);
                }
                else
                {


                    NotionManager.instance.UseNotion(NotionType.UpgradeSuccess);
                }

            }
            else
            {
                Debug.Log("강화권이 부족합니다");
            }
        }
        else
        {
            Debug.Log("골드가 부족합니다");
        }
    }

    public void SellButton()
    {

    }

}
