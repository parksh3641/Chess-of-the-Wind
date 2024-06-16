using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxInfoContent : MonoBehaviour
{
    public Text titleText;
    public Text percentText;


    public void Initialize(string title, float number)
    {
        titleText.text = title + " " + LocalizationManager.instance.GetString("Piece");
        percentText.text = number.ToString("N2") + "%";
    }

    public void SetRank(RankType type)
    {
        switch (type)
        {
            case RankType.N:
                titleText.color = new Color(100 / 255f, 142 / 255f, 244 / 255f);
                break;
            case RankType.R:
                titleText.color = new Color(124 / 255f, 206 / 255f, 93 / 255f);
                break;
            case RankType.SR:
                titleText.color = new Color(140 / 255f, 117 / 255f, 215 / 255f);
                break;
            case RankType.SSR:
                titleText.color = new Color(253 / 255f, 205 / 255f, 79 / 255f);
                break;
            case RankType.UR:
                break;
        }
    }
}
