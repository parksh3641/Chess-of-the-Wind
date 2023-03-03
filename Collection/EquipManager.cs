using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public GameObject equipView;

    [Title("Colletion")]
    public BlockUIContent armorBlockUI;
    public BlockUIContent weaponBlockUI;
    public BlockUIContent shieldBlockUI;
    public BlockUIContent newbieBlockUI;

    [Title("Equip")]
    public BlockUIContent armorBlockEquip;
    public BlockUIContent weaponBlockEquip;
    public BlockUIContent shieldBlockEquip;
    public BlockUIContent targetBlockEquip;

    public CollectionManager collectionManager;
    public UpgradeManager upgradeManager;

    BlockClass blockClass;

    Dictionary<string, string> blockData = new Dictionary<string, string>();

    BlockDataBase blockDataBase;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        equipView.SetActive(false);

        armorBlockUI.Upgrade_Initialize(collectionManager);
        weaponBlockUI.Upgrade_Initialize(collectionManager);
        shieldBlockUI.Upgrade_Initialize(collectionManager);
        newbieBlockUI.Upgrade_Initialize(collectionManager);
    }

    public void OpenEquipView(BlockClass block)
    {
        if(!equipView.activeSelf)
        {
            equipView.SetActive(true);

            Initialize(block);
        }
    }

    public void CloseEquipView()
    {
        equipView.SetActive(false);
    }


    void Initialize(BlockClass block)
    {
        blockClass = block;

        armorBlockEquip.Collection_Initialize(playerDataBase.GetBlockClass(playerDataBase.Armor));
        weaponBlockEquip.Collection_Initialize(playerDataBase.GetBlockClass(playerDataBase.Weapon));
        shieldBlockEquip.Collection_Initialize(playerDataBase.GetBlockClass(playerDataBase.Shield));

        targetBlockEquip.Collection_Initialize(block);

    }

    public void ChangeArmor()
    {
        if (playerDataBase.CheckOverlapBlock(blockClass, 0))
        {
            NotionManager.instance.UseNotion(NotionType.SameEquipBlock);
            return;
        }

        EquipArmor(blockClass, false);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();
    }

    public void ChangeWeapon()
    {
        if (playerDataBase.CheckOverlapBlock(blockClass, 1))
        {
            NotionManager.instance.UseNotion(NotionType.SameEquipBlock);
            return;
        }

        EquipWeapon(blockClass, false);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();
    }

    public void ChangeShield()
    {
        if (playerDataBase.CheckOverlapBlock(blockClass, 2))
        {
            NotionManager.instance.UseNotion(NotionType.SameEquipBlock);
            return;
        }

        EquipShield(blockClass, false);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();
    }

    public void ChangeNewbie(BlockClass block)
    {
        EquipNewBie(block, false);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();
    }


    public void EquipArmor(BlockClass block, bool first)
    {
        if(!first)
        {
            collectionManager.CheckUnEquip(playerDataBase.Armor);
        }

        playerDataBase.Armor = block.instanceId;

        if (!first)
        {
            collectionManager.CheckEquip(playerDataBase.Armor);
            armorBlockUI.Collection_Initialize(block);
        }

        blockData.Clear();
        blockData.Add("Armor", block.instanceId);

        PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("∞©ø  ¿Â¬¯ : " + block.blockType);
    }

    public void EquipWeapon(BlockClass block, bool first)
    {
        if(!first)
        {
            collectionManager.CheckUnEquip(playerDataBase.Weapon);
        }

        playerDataBase.Weapon = block.instanceId;

        if (!first)
        {
            collectionManager.CheckEquip(playerDataBase.Weapon);
            weaponBlockUI.Collection_Initialize(block);
        }

        blockData.Clear();
        blockData.Add("Weapon", block.instanceId);

        PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("∞À ¿Â¬¯ : " + block.blockType);
    }

    public void EquipShield(BlockClass block, bool first)
    {
        if (!first)
        {
            collectionManager.CheckUnEquip(playerDataBase.Shield);
        }

        playerDataBase.Shield = block.instanceId;

        if (!first)
        {
            collectionManager.CheckEquip(playerDataBase.Shield);
            shieldBlockUI.Collection_Initialize(block);
        }

        blockData.Clear();
        blockData.Add("Shield", block.instanceId);

        PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("πÊ∆– ¿Â¬¯ : " + block.blockType);
    }

    public void EquipNewBie(BlockClass block, bool first)
    {
        if (!first)
        {
            collectionManager.CheckUnEquip(playerDataBase.Newbie);
        }

        playerDataBase.Newbie = block.instanceId;

        if (!first)
        {
            collectionManager.CheckEquip(playerDataBase.Newbie);
            newbieBlockUI.Collection_Initialize(block);
        }

        blockData.Clear();
        blockData.Add("NewBie", block.instanceId);

        PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("¥∫∫Ò ¿Â¬¯ : " + block.blockType);
    }

    public bool CheckEquipBlock(string id)
    {
        bool check = false;

        if (armorBlockUI.instanceId.Equals(id) ||
            weaponBlockUI.instanceId.Equals(id) ||
            shieldBlockUI.instanceId.Equals(id) ||
            newbieBlockUI.instanceId.Equals(id))
        {
            check = true;
        }
        return check;
    }

    public void SetBlockLevel(string id, int level)
    {
        if(armorBlockUI.instanceId.Equals(id))
        {
            armorBlockUI.SetLevel(level);
        }

        if (weaponBlockUI.instanceId.Equals(id))
        {
            weaponBlockUI.SetLevel(level);
        }

        if (shieldBlockUI.instanceId.Equals(id))
        {
            shieldBlockUI.SetLevel(level);
        }

        if (newbieBlockUI.instanceId.Equals(id))
        {
            newbieBlockUI.SetLevel(level);
        }
    }
}
