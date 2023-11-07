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

    public int GetRank(int money) //???´ìƒ ?¬ìš©?˜ì? ?ŠìŒ 
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

    public int GetNeedStar(int level) //?¤ìŒ ??¬ ?±ê¸‰ ?ìŠ¹???„í•œ ë³„ì´ ëª‡ê°œ ?„ìš”?œê???
    {
        return rankInformationArray[level].star;
    }

    public int GetLimitLevel(GameRankType type) //????¬?ì„œ ìµœë?ë¡??ˆìš©?˜ëŠ” ?ˆë²¨??ëª‡ì¸ê°€??
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
