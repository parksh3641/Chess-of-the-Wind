using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockChildContent : MonoBehaviour
{
    public BlockChild[] blockChildArray;

    public Image blockIcon;

    public string nickName = "";
    public string value = "";

    public Text blockInformation;

    public void Betting(bool check)
    {
        if (blockChildArray.Length <= 0)
        {
            blockChildArray = gameObject.GetComponentsInChildren<BlockChild>();
        }

        for (int i = 0; i < blockChildArray.Length; i ++)
        {
            blockChildArray[i].SetBettingMark(check);
        }
    }

    public void SetBlock(string name,string value)
    {
        blockInformation.text = name + " / " + MoneyUnitString.ToCurrencyString(int.Parse(value));

        if (blockChildArray.Length <= 0)
        {
            blockChildArray = gameObject.GetComponentsInChildren<BlockChild>();
        }
    }

    public void SetEnemy()
    {
        if (blockChildArray.Length <= 0)
        {
            blockChildArray = gameObject.GetComponentsInChildren<BlockChild>();
        }

        for (int i = 0; i < blockChildArray.Length; i++)
        {
            blockChildArray[i].SetEnemy();
        }
    }
}
