using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChildContent : MonoBehaviour
{
    public BlockChild[] blockChildArray;

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
        if (blockChildArray.Length <= 0)
        {
            blockChildArray = gameObject.GetComponentsInChildren<BlockChild>();
        }

        for (int i = 0; i < blockChildArray.Length; i++)
        {
            blockChildArray[i].SetBlock(name, value);
        }
    }
}
