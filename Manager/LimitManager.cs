using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitManager : MonoBehaviour
{
    GameRankType gameRankType = GameRankType.Bronze_4;
    RankInformation rankInformation = new RankInformation();

    public GameObject limitView;

    public Text rankText;
    public Text newbieEnterText;
    public Text newbieMaxBlockText;
    public Text gosuEnterText;
    public Text gosuMaxBlockText;



    int limitValue = 0;

    BlockClass blockClass;
    BlockClass blockClass2;
    BlockClass blockClass3;


    public NetworkManager networkManager;

    PlayerDataBase playerDataBase;
    RankDataBase rankDataBase;
    UpgradeDataBase upgradeDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;

        rankText.text = "";

        limitView.SetActive(false);
    }

    public void Initialize()
    {
        int rank = rankDataBase.GetRank(playerDataBase.Gold);
        gameRankType = GameRankType.Bronze_4 + rank;

        rankText.text = "���� ��ũ : ";

        switch (gameRankType)
        {
            case GameRankType.Bronze_4:
                rankText.text += "����� 4";
                break;
            case GameRankType.Bronze_3:
                rankText.text += "����� 3";
                break;
            case GameRankType.Bronze_2:
                rankText.text += "����� 2";
                break;
            case GameRankType.Bronze_1:
                rankText.text += "����� 1";
                break;
            case GameRankType.Sliver_4:
                rankText.text += "�ǹ� 4";
                break;
            case GameRankType.Sliver_3:
                rankText.text += "�ǹ� 3";
                break;
            case GameRankType.Sliver_2:
                rankText.text += "�ǹ� 2";
                break;
            case GameRankType.Sliver_1:
                rankText.text += "�ǹ� 1";
                break;
            case GameRankType.Gold_4:
                rankText.text += "��� 4";
                break;
            case GameRankType.Gold_3:
                rankText.text += "��� 3";
                break;
            case GameRankType.Gold_2:
                rankText.text += "��� 2";
                break;
            case GameRankType.Gold_1:
                rankText.text += "��� 1";
                break;
            case GameRankType.Platinum_4:
                rankText.text += "�÷�Ƽ�� 4";
                break;
            case GameRankType.Platinum_3:
                rankText.text += "�÷�Ƽ�� 3";
                break;
            case GameRankType.Platinum_2:
                rankText.text += "�÷�Ƽ�� 2";
                break;
            case GameRankType.Platinum_1:
                rankText.text += "�÷�Ƽ�� 1";
                break;
            case GameRankType.Diamond_4:
                rankText.text += "���̾� 4";
                break;
            case GameRankType.Diamond_3:
                rankText.text += "���̾� 3";
                break;
            case GameRankType.Diamond_2:
                rankText.text += "���̾� 2";
                break;
            case GameRankType.Diamond_1:
                rankText.text += "���̾� 1";
                break;
            case GameRankType.Legend_4:
                rankText.text += "����";
                break;
            case GameRankType.Legend_3:
                rankText.text += "����";
                break;
            case GameRankType.Legend_2:
                rankText.text += "����";
                break;
            case GameRankType.Legend_1:
                rankText.text += "����";
                break;
        }

        rankInformation = rankDataBase.GetRankInformation(gameRankType);

        limitValue = rankInformation.limitBlockValue;

        newbieEnterText.text = "����� : " + rankInformation.stakes;
        newbieMaxBlockText.text = "�ִ� �� �� : " + rankInformation.limitBlockValue;

        gosuEnterText.text = "����� : " + rankInformation.stakes;
        gosuMaxBlockText.text = "�ִ� �� �� : " + rankInformation.limitBlockValue;

        GameStateManager.instance.GameRankType = gameRankType;
    }

    public void GameStartButton_Newbie()
    {
        blockClass = playerDataBase.GetBlockClass(playerDataBase.Newbie);

        int number = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);

        if(number > limitValue)
        {
            NotionManager.instance.UseNotion(NotionType.LimitMaxBlock);
        }
        else
        {
            networkManager.JoinOrCreateRoom_Newbie();
        }
    }

    public void GameStartButton_Gosu()
    {
        blockClass = playerDataBase.GetBlockClass(playerDataBase.Armor);
        blockClass2 = playerDataBase.GetBlockClass(playerDataBase.Weapon);
        blockClass3 = playerDataBase.GetBlockClass(playerDataBase.Shield);

        int number = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);
        int number2 = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);
        int number3 = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);

        if (number > limitValue || number2 > limitValue || number3 > limitValue)
        {
            NotionManager.instance.UseNotion(NotionType.LimitMaxBlock);
        }
        else
        {
            networkManager.JoinOrCreateRoom_Gosu();
        }
    }
}
