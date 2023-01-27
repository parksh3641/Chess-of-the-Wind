using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChildContent : MonoBehaviour
{
    public BlockChild[] blockChildArray;

    private void OnEnable()
    {
        if(blockChildArray.Length <= 0)
        {
            blockChildArray = gameObject.GetComponentsInChildren<BlockChild>();
        }
    }
}
