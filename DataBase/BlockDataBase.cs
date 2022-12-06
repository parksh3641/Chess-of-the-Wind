using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockDataBase", menuName = "ScriptableObjects/BlockDataBase")]
public class BlockDataBase : ScriptableObject
{
    public BlockInformation[] blockInformation;
}

[System.Serializable]
public class BlockInformation
{
    public BlockType blockType = BlockType.Default;
    public int size = 0;
    public int bettingPrice = 0;
    public int magnification = 0;
}

