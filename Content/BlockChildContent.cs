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

    public BlockContent blockContent;

    public void Betting(bool check, BlockContent block)
    {
        blockContent = block;

        if (blockChildArray.Length <= 0)
        {
            blockChildArray = gameObject.GetComponentsInChildren<BlockChild>();

            for (int i = 0; i < blockChildArray.Length; i++)
            {
                blockChildArray[i].Initialize(blockContent);
            }
        }

        //for (int i = 0; i < blockChildArray.Length; i ++)
        //{
        //    blockChildArray[i].SetBettingMark(check);
        //}
    }

    public void SetBlock()
    {
        if (blockChildArray.Length <= 0)
        {
            blockChildArray = gameObject.GetComponentsInChildren<BlockChild>();

            for (int i = 0; i < blockChildArray.Length; i++)
            {
                blockChildArray[i].Initialize(blockContent);
            }
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
