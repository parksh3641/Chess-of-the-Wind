using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUIContent : MonoBehaviour
{
    public BlockType blockType = BlockType.Default;

    public BlockChildContent[] blockUIArray;

    public void Initialize(BlockType type)
    {
        blockType = type;

        for (int i = 0; i < blockUIArray.Length; i++)
        {
            blockUIArray[i].gameObject.SetActive(false);
        }

        blockUIArray[(int)blockType - 1].gameObject.SetActive(true);
    }
}
