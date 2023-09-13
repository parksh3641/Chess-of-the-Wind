using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AchievementInformation //설정용
{
    public AchievementType achievementType = AchievementType.AccessDate;
    public int startGoal = 0;
    public int reward = 0;
}

[System.Serializable]
public class AchievementInfo //서버 저장용
{
    public AchievementType achievementType = AchievementType.AccessDate;
    public int count = 0;
}


[CreateAssetMenu(fileName = "AchievementDataBase", menuName = "ScriptableObjects/AchievementDataBase")]
public class AchievementDataBase : ScriptableObject
{
    public List<AchievementInformation> achievementInfomationList = new List<AchievementInformation>();

    public AchievementInformation GetAchievementInfomation(AchievementType type)
    {
        AchievementInformation info = new AchievementInformation();

        for(int i = 0; i < achievementInfomationList.Count; i ++)
        {
            if(achievementInfomationList[i].achievementType.Equals(type))
            {
                info = achievementInfomationList[i];
                break;
            }
        }

        return info;
    }
}
