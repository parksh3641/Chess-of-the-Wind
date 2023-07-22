using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RankInformation
{
    public GameRankType gameRankType = GameRankType.Bronze_4;
    public int star = 0;
    public int stakes = 0;
    public int limitBlockLevel = 0;
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

    public int GetRank(int money) //더 이상 사용되지 않음 
    {
        int count = 0;

        for(int i = 0; i < rankInformationArray.Length; i ++)
        {
            if(rankInformationArray[i].star <= money)
            {
                count++;
            }
        }

        return count;
    }

    public int GetNeedStar(int level) //다음 랭크 등급 상승을 위한 별이 몇개 필요한가요?
    {
        return rankInformationArray[level].star;
    }

    public int GetLimitLevel(GameRankType type) //제 랭크에서 최대로 허용되는 레벨이 몇인가요?
    {
        int level = 0;

        for(int i = 0; i < rankInformationArray.Length; i ++)
        {
            if(rankInformationArray[i].gameRankType.Equals(type))
            {
                level = rankInformationArray[i].limitBlockLevel;
                break;
            }
        }

        return level;
    }
}
