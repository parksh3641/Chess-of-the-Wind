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

    public FadeInOut mainFadeInOut;
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

        rankText.text = "현재 랭크 : ";

        switch (gameRankType)
        {
            case GameRankType.Bronze_4:
                rankText.text += "브론즈 4";
                break;
            case GameRankType.Bronze_3:
                rankText.text += "브론즈 3";
                break;
            case GameRankType.Bronze_2:
                rankText.text += "브론즈 2";
                break;
            case GameRankType.Bronze_1:
                rankText.text += "브론즈 1";
                break;
            case GameRankType.Sliver_4:
                rankText.text += "실버 4";
                break;
            case GameRankType.Sliver_3:
                rankText.text += "실버 3";
                break;
            case GameRankType.Sliver_2:
                rankText.text += "실버 2";
                break;
            case GameRankType.Sliver_1:
                rankText.text += "실버 1";
                break;
            case GameRankType.Gold_4:
                rankText.text += "골드 4";
                break;
            case GameRankType.Gold_3:
                rankText.text += "골드 3";
                break;
            case GameRankType.Gold_2:
                rankText.text += "골드 2";
                break;
            case GameRankType.Gold_1:
                rankText.text += "골드 1";
                break;
            case GameRankType.Platinum_4:
                rankText.text += "플래티넘 4";
                break;
            case GameRankType.Platinum_3:
                rankText.text += "플래티넘 3";
                break;
            case GameRankType.Platinum_2:
                rankText.text += "플래티넘 2";
                break;
            case GameRankType.Platinum_1:
                rankText.text += "플래티넘 1";
                break;
            case GameRankType.Diamond_4:
                rankText.text += "다이아 4";
                break;
            case GameRankType.Diamond_3:
                rankText.text += "다이아 3";
                break;
            case GameRankType.Diamond_2:
                rankText.text += "다이아 2";
                break;
            case GameRankType.Diamond_1:
                rankText.text += "다이아 1";
                break;
            case GameRankType.Legend_4:
                rankText.text += "전설";
                break;
            case GameRankType.Legend_3:
                rankText.text += "전설";
                break;
            case GameRankType.Legend_2:
                rankText.text += "전설";
                break;
            case GameRankType.Legend_1:
                rankText.text += "전설";
                break;
        }

        rankInformation = rankDataBase.GetRankInformation(gameRankType);

        stakes = rankInformation.stakes;
        limitBlock = rankInformation.limitBlockValue;

        newbieEnterText.text = "입장료 : " + rankInformation.stakes;
        newbieMaxBlockText.text = "최대 블럭 값 : " + rankInformation.limitBlockValue;

        gosuEnterText.text = "입장료 : " + rankInformation.stakes;
        gosuMaxBlockText.text = "최대 블럭 값 : " + rankInformation.limitBlockValue;

        GameStateManager.instance.GameRankType = gameRankType;
    }

    public void GameStartButton_Newbie()
    {
        blockClass = playerDataBase.GetBlockClass(playerDataBase.Newbie);

        int number = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);

        if(playerDataBase.Gold < stakes) //입장료를 가지고 있는지?
        {
            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);

            return;
        }

        if(number > limitBlock) //블럭 제한을 넘지 않는지?
        {
            NotionManager.instance.UseNotion(NotionType.LimitMaxBlock);

            return;
        }


        OpenMacthingView();

        networkManager.JoinRandomRoom_Newbie();

        Debug.Log("초보방 매칭중입니다.");
    }

    public void GameStartButton_Gosu()
    {
        blockClass = playerDataBase.GetBlockClass(playerDataBase.Armor);
        blockClass2 = playerDataBase.GetBlockClass(playerDataBase.Weapon);
        blockClass3 = playerDataBase.GetBlockClass(playerDataBase.Shield);

        int number = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);
        int number2 = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);
        int number3 = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);

        if (playerDataBase.Gold < stakes) //입장료를 가지고 있는지?
        {
            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);

            return;
        }

        if (number > limitBlock || number2 > limitBlock || number3 > limitBlock) //블럭 제한을 넘지 않는지?
        {
            NotionManager.instance.UseNotion(NotionType.LimitMaxBlock);

            return;
        }

        OpenMacthingView();

        networkManager.JoinRandomRoom_Gosu();

        Debug.Log("고수방 매칭중입니다.");
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
            matchingText.text = "상대방을 찾고 있습니다...\n예상 대기 시간 : " + matchingWaitTime + "초";

            yield return new WaitForSeconds(1);
        }

        AlMatching();
    }

    public void PlayerMatching(string player1, string player2)
    {
        StopAllCoroutines();

        matchingView.SetActive(false);

        uIManager.OnMatchingSuccess(player1, player2);

        mainFadeInOut.FadeOutToIn();

        Debug.Log("플레이어와 매칭됩니다.");
    }

    public void AlMatching()
    {
        StopAllCoroutines();

        matchingView.SetActive(false);

        uIManager.OnMatchingSuccess(GameStateManager.instance.NickName, "인공지능");

        mainFadeInOut.FadeOutToIn();

        Debug.Log("사람이 없는 관계로 인공지능과 매칭됩니다.");
    }
}
