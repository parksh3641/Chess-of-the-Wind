using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RankInformation
{
    public GameRankType gameRankType = GameRankType.Bronze_4;
    public int value = 0;
    public int stakes = 0;
    public int limitBlockValue = 0;
}

[CreateAssetMenu(fileName = "RankDataBase", menuName = "ScriptableObjects/RankDataBase")]
public class RankDataBase : ScriptableObject
{
    public RankInformation[] rankInformationArray;


    public RankInformation GetRankInformation(GameRankType type)
    {
        RankInformation rank = new RankInformation();

        for (int i = 0; i < rankInformationArray.Length; i++)
        {
            if (rankInformationArray[i].gameRankType.Equals(type))
            {
                rank = rankInformationArray[i];
                break;
            }
        }
        return rank;
    }

    public int GetRank(int money)
    {
        int count = 0;

        for(int i = 0; i < rankInformationArray.Length; i ++)
        {
            if(rankInformationArray[i].value <= money)
            {
                count++;
            }
        }
        return count;
    }
}
