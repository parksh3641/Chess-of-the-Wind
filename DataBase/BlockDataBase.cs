using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockMotherInformation
{
    [Title("NewBie")]
    public float straightBet_NewBie = 2;
    public float straightBet_NewBie_Queen = 3;

    [Space]
    [Title("Gosu")]
    public float straightBet = 26;
    public float splitBet = 13;
    public float squareBet = 6;

    [Space]
    [Title("Queen")]
    public float queenStraightBet = 28;
    public float queensplitBet = 14;
    public float queensquareBet = 7;
}

[System.Serializable]
public class BlockInformation
{
    public BlockType blockType = BlockType.Default;
    public int size = 0;
    public int bettingPrice = 0;

    public int[] index0 = new int[2];
    public int[] index1 = new int[2];
    public int[] index2 = new int[2];
    public int[] index3 = new int[2];
    public int[] index4 = new int[2];
    public int[] index5 = new int[2];
    public int[] index6 = new int[2];
    public int[] index7 = new int[2];
    public int[] index8 = new int[2];
}

[CreateAssetMenu(fileName = "BlockDataBase", menuName = "ScriptableObjects/BlockDataBase")]
public class BlockDataBase : ScriptableObject
{
    public BlockMotherInformation blockMotherInformation;

    [Space]
    public BlockInformation[] blockInformation;

    int index = 0;

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

    public int GetIndex0(BlockType type, int number)
    {
        return index = blockInformation[(int)type - 1].index0[number];
    }

    public int GetIndex1(BlockType type, int number)
    {
        return index = blockInformation[(int)type - 1].index1[number];
    }

    public int GetIndex2(BlockType type, int number)
    {
        return index = blockInformation[(int)type - 1].index2[number];
    }

    public int GetIndex3(BlockType type, int number)
    {
        return index = blockInformation[(int)type - 1].index3[number];
    }

    public int GetIndex4(BlockType type, int number)
    {
        return index = blockInformation[(int)type - 1].index4[number];
    }

    public int GetIndex5(BlockType type, int number)
    {
        return index = blockInformation[(int)type - 1].index5[number];
    }

    public int GetIndex6(BlockType type, int number)
    {
        return index = blockInformation[(int)type - 1].index6[number];
    }

    public int GetIndex7(BlockType type, int number)
    {
        return index = blockInformation[(int)type - 1].index7[number];
    }

    public int GetIndex8(BlockType type, int number)
    {
        return index = blockInformation[(int)type - 1].index8[number];
    }
}

