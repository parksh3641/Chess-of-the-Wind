using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingManager : MonoBehaviour
{
    GameRankType gameRankType = GameRankType.Bronze_4;
    RankInformation rankInformation = new RankInformation();

    [Title("Limit")]
    public GameObject limitView;
    public Text rankText;
    public Text newbieEnterText;
    public Text newbieMaxBlockText;
    public Text gosuEnterText;
    public Text gosuMaxBlockText;


    [Title("Matching")]
    public GameObject matchingView;
    public Text matchingText;


    private int stakes = 0;
    private int limitBlock = 0;
    private int matchingWaitTime = 0;

    BlockClass blockClass;
    BlockClass blockClass2;
    BlockClass blockClass3;

    public FadeInOut fadeInOut;
    public NetworkManager networkManager;
    public UIManager uIManager;

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
        matchingView.SetActive(false);
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

        stakes = rankInformation.stakes;
        limitBlock = rankInformation.limitBlockValue;

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

        if(playerDataBase.Gold < stakes) //����Ḧ ������ �ִ���?
        {
            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);

            return;
        }

        if(number > limitBlock) //�� ������ ���� �ʴ���?
        {
            NotionManager.instance.UseNotion(NotionType.LimitMaxBlock);

            return;
        }


        OpenMacthingView();

        networkManager.JoinRandomRoom_Newbie();

        Debug.Log("�ʺ��� ��Ī���Դϴ�.");
    }

    public void GameStartButton_Gosu()
    {
        blockClass = playerDataBase.GetBlockClass(playerDataBase.Armor);
        blockClass2 = playerDataBase.GetBlockClass(playerDataBase.Weapon);
        blockClass3 = playerDataBase.GetBlockClass(playerDataBase.Shield);

        int number = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);
        int number2 = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);
        int number3 = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);

        if (playerDataBase.Gold < stakes) //����Ḧ ������ �ִ���?
        {
            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);

            return;
        }

        if (number > limitBlock || number2 > limitBlock || number3 > limitBlock) //�� ������ ���� �ʴ���?
        {
            NotionManager.instance.UseNotion(NotionType.LimitMaxBlock);

            return;
        }

        OpenMacthingView();

        networkManager.JoinRandomRoom_Gosu();

        Debug.Log("����� ��Ī���Դϴ�.");
    }

    public void OpenMacthingView()
    {
        if (!matchingView.activeSelf)
        {
            matchingView.SetActive(true);

            GameStateManager.instance.Stakes = stakes;

            matchingWaitTime = 5;
            StartCoroutine(WaitingPlayer());
        }
        else
        {
            StopAllCoroutines();

            networkManager.LeaveRoom();

            matchingView.SetActive(false);
        }
    }

    IEnumerator WaitingPlayer()
    {
        while(matchingWaitTime > 0)
        {
            matchingWaitTime -= 1;
            matchingText.text = "������ ã�� �ֽ��ϴ�...\n���� ��� �ð� : " + matchingWaitTime + "��";

            yield return new WaitForSeconds(1);
        }

        AlMatching();
    }

    public void PlayerMatching(string player1, string player2)
    {
        StopAllCoroutines();

        matchingView.SetActive(false);

        uIManager.OnMatchingSuccess(player1, player2);

        fadeInOut.FadeOutToIn();

        Debug.Log("�÷��̾�� ��Ī�˴ϴ�.");
    }

    public void AlMatching()
    {
        StopAllCoroutines();

        matchingView.SetActive(false);

        uIManager.OnMatchingSuccess(GameStateManager.instance.NickName, "�ΰ�����");

        fadeInOut.FadeOutToIn();

        Debug.Log("����� ���� ����� �ΰ����ɰ� ��Ī�˴ϴ�.");
    }
}
