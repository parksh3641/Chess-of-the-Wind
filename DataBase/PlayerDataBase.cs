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
    public int equipInfo = 0;
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
    [SerializeField]
    private int defDestroyTicket = 0;

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
    public List<BlockClass> successionLevel = new List<BlockClass>();
    public List<BlockClass> equipBlockInfo = new List<BlockClass>();
    public List<string> sellBlockList = new List<string>();

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


    Dictionary<string, string> levelCustomData = new Dictionary<string, string>();

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

    public int DefDestroyTicket
    {
        get
        {
            return defDestroyTicket;
        }
        set
        {
            defDestroyTicket = value;
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
        defDestroyTicket = 0;

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
        sellBlockList.Clear();
        successionLevel.Clear();
        equipBlockInfo.Clear();

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

    public void Initialize_BlockList()
    {
        blockList = new List<BlockClass>();
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

        for(int i = 0; i < sellBlockList.Count; i ++)
        {
            if(item.ItemInstanceId.Equals(sellBlockList[i]))
            {
                return;
            }
        }

        BlockClass blockClass = new BlockClass();

        blockClass.blockType = (BlockType)Enum.Parse(typeof(BlockType), item.DisplayName.ToString());

        switch (item.ItemClass)
        {
            case "UR":
                blockClass.rankType = RankType.UR;
                break;
            case "SSR":
                blockClass.rankType = RankType.SSR;
                break;
            case "SR":
                blockClass.rankType = RankType.SR;
                break;
            case "R":
                blockClass.rankType = RankType.R;
                break;
            case "N":
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

        for (int i = 0; i < successionLevel.Count; i++) //레벨 계승
        {
            if(blockClass.level == 0 && 
                blockClass.blockType.Equals(successionLevel[i].blockType) && 
                blockClass.rankType.Equals(successionLevel[i].rankType))
            {
                levelCustomData.Clear();
                levelCustomData.Add("Level", successionLevel[i].level.ToString());

                PlayfabManager.instance.SetInventoryCustomData(blockClass.instanceId, levelCustomData);
                blockClass.level = successionLevel[i].level;

                Debug.LogError(blockClass.blockType + "_" + blockClass.rankType + " 가 " + (successionLevel[i].level + 1) + " 레벨로 계승되었습니다");

                successionLevel.RemoveAt(i);
            }
        }

        for (int i = 0; i < equipBlockInfo.Count; i++) //장착 계승
        {
            if (blockClass.level == equipBlockInfo[i].level &&
                blockClass.blockType.Equals(equipBlockInfo[i].blockType) &&
                blockClass.rankType.Equals(equipBlockInfo[i].rankType))
            {
                if (equipBlockInfo[i].equipInfo == 1)
                {
                    armor = blockClass.instanceId;

                    Debug.Log("아머로 장비가 계승되었습니다");
                }
                else if (equipBlockInfo[i].equipInfo == 2)
                {
                    weapon = blockClass.instanceId;

                    Debug.Log("검으로 장비가 계승되었습니다");
                }
                else if (equipBlockInfo[i].equipInfo == 3)
                {
                    shield = blockClass.instanceId;

                    Debug.Log("쉴드로 장비가 계승되었습니다");
                }
                else if (equipBlockInfo[i].equipInfo == 4)
                {
                    newbie = blockClass.instanceId;

                    Debug.Log("뉴비로 장비가 계승되었습니다");
                }

                equipBlockInfo.RemoveAt(i);
            }
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
        sellBlockList.Add(id);
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

    public int CheckEquipId(string id)
    {
        int number = 0;

        if(armor.Equals(id))
        {
            number = 1;

            Debug.Log("장착 중인 아머가 선택되었습니다");
        }

        if (weapon.Equals(id))
        {
            number = 2;

            Debug.Log("장착 중인 검이 선택되었습니다");
        }

        if (shield.Equals(id))
        {
            number = 3;

            Debug.Log("장착 중인 쉴드가 선택되었습니다");
        }

        if (newbie.Equals(id))
        {
            number = 4;

            Debug.Log("장착 중인 뉴비가 선택되었습니다");
        }

        return number;
    }

    public bool CheckEquipId2(string id)
    {
        bool check = false;

        if (armor.Equals(id))
        {
            check = true;
        }

        if (weapon.Equals(id))
        {
            check = true;
        }

        if (shield.Equals(id))
        {
            check = true;
        }

        if (newbie.Equals(id))
        {
            check = true;
        }

        return check;
    }

    public bool CheckOverlapBlock(BlockClass block, int number)
    {
        bool check = false;

        switch (number)
        {
            case 0:
                if(GetBlockClass(weapon).blockType.Equals(block.blockType) || GetBlockClass(shield).blockType.Equals(block.blockType))
                {
                    check = true;
                }

                break;
            case 1:
                if (GetBlockClass(armor).blockType.Equals(block.blockType) || GetBlockClass(shield).blockType.Equals(block.blockType))
                {
                    check = true;
                }

                break;
            case 2:
                if (GetBlockClass(armor).blockType.Equals(block.blockType) || GetBlockClass(weapon).blockType.Equals(block.blockType))
                {
                    check = true;
                }

                break;
        }

        return check;
    }

    public void SetSuccessionLevel(BlockClass block)
    {
        successionLevel.Add(block);

        Debug.Log("계승 정보를 저장했습니다");
    }

    public void SetEquipInfo(BlockClass block)
    {
        equipBlockInfo.Add(block);

        Debug.Log("장착할 정보를 저장했습니다");
    }

    #region Ticket
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
    #endregion
}