using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class RankDownManager : MonoBehaviour
{
    public GameObject rankDownView;

    public GameObject alarm;

    public Text rankDownInfo;

    public ReceiveContent[] receiveContents;

    string[] strArray = new string[2];
    string[] strArray2 = new string[2];


    public MatchingManager matchingManager;
    public TitleManager titleManager;

    RankDownInfomation rankDownInfomation;

    PlayerDataBase playerDataBase;
    RankDataBase rankDataBase;
    RankDownDataBase rankDownDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;
        if (rankDownDataBase == null) rankDownDataBase = Resources.Load("RankDownDataBase") as RankDownDataBase;

        rankDownView.SetActive(false);
    }

    private void Start()
    {
        alarm.SetActive(false);

        if (GameStateManager.instance.GameRankType > GameRankType.Sliver_2)
        {
            alarm.SetActive(true);
        }
    }


    public void OpenRankDownView()
    {
        if (!rankDownView.activeInHierarchy)
        {
            if(GameStateManager.instance.GameRankType < GameRankType.Sliver_3)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                NotionManager.instance.UseNotion(NotionType.LowRank);

                return;
            }

            rankDownView.SetActive(true);
            alarm.SetActive(false);

            Initialize();

            FirebaseAnalytics.LogEvent("RankDown");
        }
        else
        {
            rankDownView.SetActive(false);

            if (playerDataBase.RankDownStreak > 0)
            {
                playerDataBase.RankDownStreak = 0;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("RankDownStreak", playerDataBase.RankDownStreak);
            }
        }
    }

    void Initialize()
    {
        rankDownInfomation = rankDownDataBase.GetRankDownInfomation(GameStateManager.instance.GameRankType - 1);

        for(int i = 0; i < receiveContents.Length; i ++)
        {
            receiveContents[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < rankDownInfomation.receiveInformationList.Count; i ++)
        {
            receiveContents[i].gameObject.SetActive(true);
            receiveContents[i].Initialize(rankDownInfomation.receiveInformationList[i].rewardType, rankDownInfomation.receiveInformationList[i].count);
        }

        strArray = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType].gameRankType.ToString().Split("_");
        strArray2 = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType - 1].gameRankType.ToString().Split("_");

        rankDownInfo.text = LocalizationManager.instance.GetString(strArray[0]) + " <color=#FFC032>" + strArray[1] + "</color>     ▶     " +
LocalizationManager.instance.GetString(strArray2[0]) + " <color=#FFC032>" + strArray2[1] + "</color>";

        titleManager.CheckGoal();
    }

    public void ReceiveveButton()
    {
        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        for(int i = 0; i < rankDownInfomation.receiveInformationList.Count; i ++)
        {
            switch (rankDownInfomation.receiveInformationList[i].rewardType)
            {
                case RewardType.Gold:
                    PlayfabManager.instance.UpdateAddGold(rankDownInfomation.receiveInformationList[i].count);
                    break;
                case RewardType.UpgradeTicket:
                    ItemAnimManager.instance.GetUpgradeTicket(rankDownInfomation.receiveInformationList[i].count);
                    break;
                case RewardType.Box:
                    switch (GameStateManager.instance.WindCharacterType)
                    {
                        case WindCharacterType.Winter:
                            playerDataBase.SnowBox = rankDownInfomation.receiveInformationList[i].count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox", rankDownInfomation.receiveInformationList[i].count);
                            break;
                        case WindCharacterType.UnderWorld:
                            playerDataBase.UnderworldBox = rankDownInfomation.receiveInformationList[i].count;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox", rankDownInfomation.receiveInformationList[i].count);
                            break;
                    }
                    break;
                case RewardType.Box_N:
                    break;
                case RewardType.Box_R:
                    break;
                case RewardType.Box_SR:
                    break;
                case RewardType.Box_SSR:
                    break;
                case RewardType.Box_UR:
                    break;
                case RewardType.Box_NR:
                    break;
                case RewardType.Box_RSR:
                    break;
                case RewardType.Box_SRSSR:
                    break;
            }
        }

        playerDataBase.NowRank -= 1;
        playerDataBase.Star = 1;

        GameStateManager.instance.GameRankType -= 1;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("NowRank", playerDataBase.NowRank);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Star", playerDataBase.Star);

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        NotionManager.instance.UseNotion(NotionType.GetReward);

        rankDownView.SetActive(false);

        matchingManager.Initialize();

        playerDataBase.RankDownCount += 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("RankDownCount", playerDataBase.RankDownCount);

        playerDataBase.RankDownStreak += 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("RankDownStreak", playerDataBase.RankDownStreak);
    }
}
