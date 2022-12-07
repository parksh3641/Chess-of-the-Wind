using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockDataBase", menuName = "ScriptableObjects/BlockDataBase")]
public class BlockDataBase : ScriptableObject
{
    public BlockInformation[] blockInformation;

    public BlockInformation GetBlockInfomation(BlockType type)
    {
        BlockInformation block = new BlockInformation();

        for(int i = 0; i < blockInformation.Length; i ++)
        {
            if(blockInformation[i].blockType.Equals(type))
            {
                block = blockInformation[i];
            }
        }
        return block;
    }
}

[System.Serializable]
public class BlockInformation
{
    public BlockType blockType = BlockType.Default;
    public int size = 0;
    public int bettingPrice = 0;
    public int magnification = 0;
}
