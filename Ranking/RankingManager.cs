using Firebase.Analytics;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager instance;

    public GameObject rankingView;

    public GameObject alarm;

    public RankContent rankContentPrefab;
    public RankContent myRankContent;
    public RectTransform rankContentParent;

    public bool isDelay = false;
    public bool isDelay2 = false;

    private string recordStr = "";
    private string country = "";

    [Space]
    List<RankContent> rankContentList = new List<RankContent>();

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        instance = this;

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        for (int i = 0; i < 100; i++)
        {
            RankContent monster = Instantiate(rankContentPrefab) as RankContent;
            monster.name = "RankContent_" + i;
            monster.transform.position = Vector3.zero;
            monster.transform.localScale = Vector3.one;
            monster.transform.SetParent(rankContentParent);
            monster.gameObject.SetActive(false);

            rankContentList.Add(monster);
        }

        rankingView.SetActive(false);

        rankContentParent.anchoredPosition = new Vector2(0, -9999);

        alarm.SetActive(true);
    }

    public void OpenRanking()
    {
        if (!rankingView.activeSelf)
        {
            rankingView.SetActive(true);

            alarm.SetActive(false);

            Initialize();

            FirebaseAnalytics.LogEvent("Open_Ranking");
        }
        else
        {
            if (!isDelay)
            {
                rankingView.SetActive(false);
            }
        }
    }

    void Initialize()
    {
        isDelay = true;
        PlayfabManager.instance.GetLeaderboarder("NowRank", 0, SetRanking);
    }

    public void SetRanking(GetLeaderboardResult result)
    {
        int index = 1;
        bool isMine = false;
        bool isCheck = false;
        string nickName = "";

        var curBoard = result.Leaderboard;
        int num = 0;

        for (int i = 0; i < rankContentList.Count; i++)
        {
            rankContentList[i].transform.localScale = Vector3.one;
            rankContentList[i].gameObject.SetActive(false);
        }

        foreach (PlayerLeaderboardEntry player in curBoard)
        {
            var location = curBoard[num].Profile.Locations[0].CountryCode.Value.ToString().ToLower();

            isMine = false;

            if (player.StatValue != 0)
            {
                recordStr = player.StatValue.ToString();
            }
            else
            {
                recordStr = "0";
            }

            if (player.DisplayName == null)
            {
                nickName = player.PlayFabId;
            }
            else
            {
                nickName = player.DisplayName;
            }

            if (player.PlayFabId.Equals(GameStateManager.instance.PlayfabId))
            {
                isMine = true;
                isCheck = true;

                country = location;

                myRankContent.InitState(index, location, nickName, recordStr, false);
            }
            else if (player.DisplayName != null)
            {
                if (player.DisplayName.Equals(GameStateManager.instance.NickName))
                {
                    isMine = true;
                    isCheck = true;

                    country = location;

                    myRankContent.InitState(index, location, nickName, recordStr, false);
                }
            }

            rankContentList[num].InitState(index, location, nickName, recordStr, isMine);

            if (!isDelay2)
            {
                rankContentList[num].gameObject.SetActive(true);
            }

            index++;
            num++;
        }

        if (!isCheck)
        {
            PlayfabManager.instance.GetPlayerProfile(GameStateManager.instance.PlayfabId, CheckCountry);
        }

        isDelay = false;

        PlayfabManager.instance.GetLeaderboarder("TitleNumber", 0, SetTitle);

        rankContentParent.anchoredPosition = new Vector2(0, -9999);
    }

    void CheckCountry(string code)
    {
        myRankContent.InitState(999, code, GameStateManager.instance.NickName, playerDataBase.NowRank.ToString(), false);
    }

    void SetTitle(GetLeaderboardResult result)
    {
        var curBoard = result.Leaderboard;

        for (int i = 0; i < rankContentList.Count; i++)
        {
            rankContentList[i].TitleState(0);
        }

        foreach (PlayerLeaderboardEntry player in curBoard)
        {
            for (int i = 0; i < rankContentList.Count; i++)
            {
                //rankContentList[i].TitleState(0);

                if (rankContentList[i].nickNameText.text.Equals(player.DisplayName) ||
                    rankContentList[i].nickNameText.text.Equals(player.PlayFabId))
                {
                    if (player.StatValue > 0)
                    {
                        rankContentList[i].TitleState(player.StatValue);
                    }

                    continue;
                }
            }
        }

        myRankContent.TitleState(playerDataBase.TitleNumber);
    }
}
