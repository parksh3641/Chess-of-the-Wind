using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[System.Serializable]
public class RankUpInfomation
{
    [Title("Rank")]
    public GameRankType gameRankType = GameRankType.Sliver_4;

    [Title("Reward")]
    public List<ReceiveInformation> receiveInformationList = new List<ReceiveInformation>();
}


[CreateAssetMenu(fileName = "RankUpDataBase", menuName = "ScriptableObjects/RankUpBase")]
public class RankUpDataBase : ScriptableObject
{
    public List<RankUpInfomation> rankUpInfomationList = new List<RankUpInfomation>();


    public RankUpInfomation GetRankUpInfomation(GameRankType type)
    {
        RankUpInfomation rank = new RankUpInfomation();

        for (int i = 0; i < rankUpInfomationList.Count; i++)
        {
            if (rankUpInfomationList[i].gameRankType.Equals(type))
            {
                rank = rankUpInfomationList[i];
                break;
            }
        }

        return rank;
    }

}
