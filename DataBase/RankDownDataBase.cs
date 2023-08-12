using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[System.Serializable]
public class ReceiveInformation
{
    public RewardType rewardType;
    public int count = 0;
}


[System.Serializable]
public class RankDownInfomation
{
    [Title("Rank")]
    public GameRankType gameRankType = GameRankType.Sliver_4;

    [Title("Reward")]
    public List<ReceiveInformation> receiveInformationList = new List<ReceiveInformation>();
}


[CreateAssetMenu(fileName = "RankDownDataBase", menuName = "ScriptableObjects/RankDataDownBase")]
public class RankDownDataBase : ScriptableObject
{
    public List<RankDownInfomation> rankDownInfomationList = new List<RankDownInfomation>();


    public RankDownInfomation GetRankDownInfomation(GameRankType type)
    {
        RankDownInfomation rank = new RankDownInfomation();

        for(int i = 0; i < rankDownInfomationList.Count; i ++)
        {
            if(rankDownInfomationList[i].gameRankType.Equals(type))
            {
                rank = rankDownInfomationList[i];
                break;
            }
        }

        return rank;
    }
 
}
