using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChildContent : MonoBehaviour
{
    public BlockChild[] blockChildArray;

    private void Start()
    {
        blockChildArray = gameObject.GetComponentsInChildren<BlockChild>();
    }
}
