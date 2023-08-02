using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockLevelContent : MonoBehaviour
{
    public Text myLevelText;
    public Text otherLevelText;

    RankDataBase rankDataBase;

    private void Awake()
    {
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;
    }

    public void Initialize()
    {
        myLevelText.text = "";
        otherLevelText.text = "";
    }

    public void Initialize_My()
    {
        myLevelText.text = "";
    }

    public void SetMyBlock(int number)
    {
        int level = rankDataBase.GetLimitLevel(GameStateManager.instance.GameRankType) - 1;

        if (number > level)
        {
            number = level;
        }

        myLevelText.text = "Lv. " + (number + 1).ToString();
    }

    public void Initialize_Other()
    {
        otherLevelText.text = "";
    }

    public void SetOtherBlock(int number)
    {
        int level = rankDataBase.GetLimitLevel(GameStateManager.instance.GameRankType) - 1;

        if (number > level)
        {
            number = level;
        }

        otherLevelText.text = "Lv. " + (number + 1).ToString();
    }
}
