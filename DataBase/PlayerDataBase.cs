using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WindCharacterClass
{
    public WindCharacterType windCharacterType = WindCharacterType.Winter;
    public PresentType presentType = PresentType.A; //좋아하는 선물

    public int friendship_Level = 0; //호감도 레벨
    public int friendship_Exp = 0; //호감도 경험치

    public int storyProgress = 0; //스토리 진행도

    public int windlevel = 0; //레벨
    public int windPower = 0; //바람 세기

    public bool hidden = false;
}

[System.Serializable]
public class BlockClass
{
    public BlockType blockType = BlockType.Default;
    public RankType rankType = RankType.N;
    public string instanceId = "";

    public int level = 0;
}

[System.Serializable]
public class WindCharacterUpgrade
{
    public int max = 99;

    public int gold = 0;
    public int addGold = 0;

    public int exp = 0; //레벨 업에 필요한 경험치
    public int addExp = 0;

    public int friendshipUnlock = 0; //다이아로 스토리 해금
    public int addFriendshipUnlock = 0;

    public float saleCardUpgradecost = 0; //카드 강화 비용 감소
}

[System.Serializable]
public class BlockUpgrade
{
    public int max = 99;

    public int gold = 0;
    public int addGold = 0;

    public int ticket = 0; //필요한 강화권 개수
    public int addTicket = 0;
}

[System.Serializable]
public class PresentClass
{
    public PresentType presentType = PresentType.A;

    public int holdNumber = 0;
}

[System.Serializable]
public class UpgradeTicketClass
{
    public RankType rankType = RankType.N;

    public int holdNumber = 0;
}


[CreateAssetMenu(fileName = "PlayerDataBase", menuName = "ScriptableObjects/PlayerDataBase")]
public class PlayerDataBase : ScriptableObject
{
    [Title("Money")]
    [SerializeField]
    private int gold = 0;

    [Title("User")]
    [SerializeField]
    private int formation = 0;

    [Title("Equip")]
    [SerializeField]
    private string armor = "";
    [SerializeField]
    private string weapon = "";
    [SerializeField]
    private string shield = "";
    [SerializeField]
    private string newbie = "";

    [Title("Item")]
    [SerializeField]
    private int snowBox = 0;
    [SerializeField]
    private int underworldBox = 0;

    [Title("BuyCount")]
    [SerializeField]
    private int buySnowBox = 0;
    [SerializeField]
    private int buyUnderworldBox = 0;

    [Title("Wind Character")]
    [SerializeField]
    private List<WindCharacterClass> windCharacterList = new List<WindCharacterClass>();

    [Title("Block")]
    [SerializeField]
    private List<BlockClass> blockList = new List<BlockClass>();

    [Title("Upgrade")]
    [SerializeField]
    private WindCharacterUpgrade windCharacterUpgrade;
    [SerializeField]
    private BlockUpgrade blockUpgrade;

    [Title("Present")]
    [SerializeField]
    private List<PresentClass> presentList = new List<PresentClass>();

    [Title("UpgradeTicket")]
    [SerializeField]
    private List<UpgradeTicketClass> upgradeTicketList = new List<UpgradeTicketClass>();


    public delegate void BoxEvent();
    public static event BoxEvent eGetSnowBox, eGetUnderworldBox;

    #region Data

    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
        }
    }

    public int Formation
    {
        get
        {
            return formation;
        }
        set
        {
            formation = value;
        }
    }

    public string Armor
    {
        get
        {
            return armor;
        }
        set
        {
            armor = value;
        }
    }

    public string Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
        }
    }

    public string Shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = value;
        }
    }

    public string Newbie
    {
        get
        {
            return newbie;
        }
        set
        {
            newbie = value;
        }
    }

    public int SnowBox
    {
        get
        {
            return snowBox;
        }
        set
        {
            snowBox = value;

            if (snowBox > 0)
            {
                eGetSnowBox();
            }
        }
    }

    public int UnderworldBox
    {
        get
        {
            return underworldBox;
        }
        set
        {
            underworldBox = value;

            if (underworldBox > 0)
            {
                eGetUnderworldBox();
            }
        }
    }

    public int BuySnowBox
    {
        get
        {
            return buySnowBox;
        }
        set
        {
            buySnowBox = value;
        }
    }
    public int BuyUnderworldBox
    {
        get
        {
            return buyUnderworldBox;
        }
        set
        {
            buyUnderworldBox = value;
        }
    }

    #endregion

    public void Initialize()
    {
        gold = 0;
        formation = 0;

        armor = "";
        weapon = "";
        shield = "";
        newbie = "";

        snowBox = 0;
        underworldBox = 0;

        BuySnowBox = 0;
        BuyUnderworldBox = 0;

        windCharacterList.Clear();

        for(int i = 0; i < System.Enum.GetValues(typeof(WindCharacterType)).Length; i ++)
        {
            WindCharacterClass content = new WindCharacterClass();
            content.windCharacterType = WindCharacterType.Winter + i;
            windCharacterList.Add(content);
        }

        blockList.Clear();

        //for (int i = 0; i < System.Enum.GetValues(typeof(BlockType)).Length; i++)
        //{
        //    BlockClass content = new BlockClass();
        //    content.blockType = BlockType.Default + i + 1;
        //    blockList.Add(content);
        //}

        presentList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(PresentType)).Length; i++)
        {
            PresentClass content = new PresentClass();
            content.presentType = PresentType.A + i;
            presentList.Add(content);
        }

        upgradeTicketList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(RankType)).Length; i++)
        {
            UpgradeTicketClass content = new UpgradeTicketClass();
            content.rankType = RankType.N + i;
            upgradeTicketList.Add(content);
        }
    }

    public void SetBlock(ItemInstance item)
    {
        for(int i = 0; i < blockList.Count; i ++)
        {
            if(item.ItemInstanceId.Equals(blockList[i].instanceId))
            {
                return;
            }
        }

        Debug.Log(item.DisplayName + "가 추가됨");

        BlockClass blockClass = new BlockClass();

        blockClass.blockType = (BlockType)Enum.Parse(typeof(BlockType), item.DisplayName.ToString());

        string rank = item.ItemId.Substring(item.ItemId.Length - 1);

        switch (rank)
        {
            case "S":
                blockClass.rankType = RankType.UR;
                break;
            case "A":
                blockClass.rankType = RankType.SSR;
                break;
            case "B":
                blockClass.rankType = RankType.SR;
                break;
            case "C":
                blockClass.rankType = RankType.R;
                break;
            case "D":
                blockClass.rankType = RankType.N;
                break;
            default:
                blockClass.rankType = RankType.N;
                break;

        }

        blockClass.instanceId = item.ItemInstanceId;

        if(item.CustomData != null)
        {
            blockClass.level = int.Parse(item.CustomData["Level"]);
        }

        blockList.Add(blockClass);
    }

    public void SetBlockLevel(string id, int level)
    {
        for(int i = 0; i < blockList.Count; i ++)
        {
            if(blockList[i].instanceId.Equals(id))
            {
                blockList[i].level = level;
                break;
            }
        }
    }

    public void SellBlock(string id)
    {
        for (int i = blockList.Count - 1; i > 0; i--)
        {
            if (blockList[i].instanceId.Equals(id))
            {
                Debug.Log(blockList[i].blockType + " 제거됨");
                blockList.RemoveAt(i);
                break;
            }
        }
    }

    public List<BlockClass> GetBlockClass()
    {
        return blockList;
    }

    public BlockClass GetBlockClass(string id)
    {
        BlockClass blockClass = new BlockClass();

        for(int i = 0; i < blockList.Count; i ++)
        {
            if(blockList[i].instanceId.Equals(id))
            {
                blockClass = blockList[i];
                break;
            }
        }

        return blockClass;
    }

    public void SetUpgradeTicket(RankType type, int number)
    {
        for(int i = 0; i < upgradeTicketList.Count; i ++)
        {
            if(upgradeTicketList[i].rankType.Equals(type))
            {
                upgradeTicketList[i].holdNumber += number;
                break;
            }
        }
    }

    public int GetUpgradeTicket(RankType type)
    {
        int ticket = 0;
        for (int i = 0; i < upgradeTicketList.Count; i++)
        {
            if (upgradeTicketList[i].rankType.Equals(type))
            {
                ticket = upgradeTicketList[i].holdNumber;
                break;
            }
        }
        return ticket;
    }

    public void UseUpgradeTicket(RankType type)
    {
        for (int i = 0; i < upgradeTicketList.Count; i++)
        {
            if (upgradeTicketList[i].rankType.Equals(type))
            {
                upgradeTicketList[i].holdNumber -= 1;
                break;
            }
        }
    }
}