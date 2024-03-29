using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeValue
{
    public RankType rankType = RankType.N;
    public int maxLevel = 5;
    public int synthesisValue = 0;

    public List<string> valueList = new List<string>();

    public List<string> GetValueList()
    {
        return valueList;
    }

    public int GetValueNumber(int number)
    {
        return int.Parse(valueList[number]);
    }

    public int GetSynthesisValue()
    {
        return synthesisValue;
    }
}

[System.Serializable]
public class UpgradeInformation
{
    [Space]
    public int level = 0;
    [Title("Percent")]
    public float success = 0;
    public float keep = 0;
    public float down = 0;
    public float destroy = 0;
    [Title("Cost")]
    public int needGold = 0;
    public int needTicket = 0;
}

[CreateAssetMenu(fileName = "UpgradeDataBase", menuName = "ScriptableObjects/UpgradeDataBase")]
public class UpgradeDataBase : ScriptableObject
{
    public List<UpgradeValue> upgradeValueList = new List<UpgradeValue>();

    public List<UpgradeInformation> upgradeInformationList = new List<UpgradeInformation>();

    public UpgradeValue GetUpgradeValue(RankType type)
    {
        UpgradeValue upgradeValue = new UpgradeValue();

        for(int i = 0; i < upgradeValueList.Count; i ++)
        {
            if(upgradeValueList[i].rankType.Equals(type))
            {
                upgradeValue = upgradeValueList[i];
                break;
            }
        }
        return upgradeValue;
    }

    public UpgradeInformation GetUpgradeInformation(int level)
    {
        UpgradeInformation upgradeInformation = new UpgradeInformation();
        for(int i = 0; i < upgradeInformationList.Count; i ++)
        {
            if(upgradeInformationList[i].level.Equals(level))
            {
                upgradeInformation = upgradeInformationList[i];
                break;
            }
        }

        return upgradeInformation;
    }

    public int CheckUpgradeValue(RankType type, int value)
    {
        int level = 0;

        for (int i = 0; i < upgradeValueList.Count; i++)
        {
            if (upgradeValueList[i].rankType.Equals(type))
            {
                for(int j = 0; j < upgradeValueList[i].valueList.Count - 1; j ++)
                {
                    if(int.Parse(upgradeValueList[i].valueList[j + 1]) <= value)
                    {
                        level++;
                    }
                }
                break;
            }
        }

        return level;
    }

    public int GetSynthesisValue(RankType type)
    {
        int number = 0;

        for(int i = 0; i < upgradeValueList.Count; i ++)
        {
            if(upgradeValueList[i].rankType.Equals(type))
            {
                number = upgradeValueList[i].GetSynthesisValue();
                break;
            }
        }

        return number;
    }

    public int GetNeedTicket(int level)
    {
        int need = 0;
        for (int i = 0; i < upgradeInformationList.Count; i++)
        {
            if (upgradeInformationList[i].level.Equals(level))
            {
                need = upgradeInformationList[i].needTicket;
                break;
            }
        }

        return need;
    }
}
