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
    public UpgradeTicketType upgradeTicketType = UpgradeTicketType.Queen;

    public int holdNumber = 0;
}


[CreateAssetMenu(fileName = "PlayerDataBase", menuName = "ScriptableObjects/PlayerDataBase")]
public class PlayerDataBase : ScriptableObject
{
    [Title("Money")]
    [SerializeField]
    private int coin = 0;
    [SerializeField]
    private int crystal = 0;

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

    public int Coin
    {
        get
        {
            return coin;
        }
        set
        {
            coin = value;
        }
    }

    public int Crystal
    {
        get
        {
            return crystal;
        }
        set
        {
            crystal = value;
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

    #endregion

    public void Initialize()
    {
        coin = 0;
        crystal = 0;
        formation = 0;

        armor = "";
        weapon = "";
        shield = "";
        newbie = "";

        snowBox = 0;
        underworldBox = 0;

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

        for (int i = 0; i < System.Enum.GetValues(typeof(UpgradeTicketType)).Length; i++)
        {
            UpgradeTicketClass content = new UpgradeTicketClass();
            content.upgradeTicketType = UpgradeTicketType.Queen + i;
            upgradeTicketList.Add(content);
        }
    }

    public void SetBlock(ItemInstance item)
    {
        BlockClass blockClass = new BlockClass();

        blockClass.blockType = (BlockType)Enum.Parse(typeof(BlockType), item.DisplayName.ToString());

        string rank = item.ItemId.Substring(item.ItemId.Length);

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

    public List<BlockClass> GetBlockClass()
    {
        return blockList;
    }
}