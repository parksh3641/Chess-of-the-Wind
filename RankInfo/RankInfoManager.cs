using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;

public class RankInfoManager : MonoBehaviour
{
    public GameObject rankInfoView;

    public RankInfoContent rankInfoContent;

    public RectTransform rankInfoContentTransform;

    public List<RankInfoContent> rankInfoContentList = new List<RankInfoContent>();

    Sprite[] rankIconArray;

    string[] strArray = new string[2];

    PlayerDataBase playerDataBase;
    RankDataBase rankDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rankIconArray = imageDataBase.GetRankIconArray();

        rankInfoView.SetActive(false);
    }

    public void Initialize()
    {
        for (int i = 0; i < rankDataBase.rankInformationArray.Length; i++)
        {
            RankInfoContent monster = Instantiate(rankInfoContent);
            monster.transform.SetParent(rankInfoContentTransform);
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;

            if(i % 2 == 0)
            {
                monster.SetBackground(false);
            }

            strArray = rankDataBase.rankInformationArray[i].gameRankType.ToString().Split("_");

            monster.Initialize(rankIconArray[i], int.Parse(strArray[1]), strArray[0], strArray[1],
                rankDataBase.rankInformationArray[i].star);
            monster.gameObject.SetActive(true);

            rankInfoContentList.Add(monster);
        }
    }

    public void OpenRankInfo()
    {
        if(!rankInfoView.activeInHierarchy)
        {
            rankInfoView.SetActive(true);

            CheckMy();

            FirebaseAnalytics.LogEvent("RankInfo");
        }
        else
        {
            rankInfoView.SetActive(false);
        }
    }

    public void CheckMy()
    {
        //int rank = rankDataBase.GetRank(playerDataBase.Gold) - 1;

        for(int i = 0; i < rankInfoContentList.Count; i ++)
        {
            rankInfoContentList[i].CheckMy(false);
        }

        rankInfoContentList[playerDataBase.NowRank].CheckMy(true);
    }

}
