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

    public int GetRank(int money) //???΄μ ?¬μ©?μ? ?μ 
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

    public int GetNeedStar(int level) //?€μ ??¬ ?±κΈ ?μΉ???ν λ³μ΄ λͺκ° ?μ?κ???
    {
        return rankInformationArray[level].star;
    }

    public int GetLimitLevel(GameRankType type) //????¬?μ μ΅λ?λ‘??μ©?λ ?λ²¨??λͺμΈκ°??
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
