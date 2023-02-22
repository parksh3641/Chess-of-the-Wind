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

        EquipArmor(blockClass);

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

        EquipWeapon(blockClass);

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

        EquipShield(blockClass);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();
    }

    public void ChangeNewbie(BlockClass block)
    {
        EquipNewBie(block);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();
    }


    public void EquipArmor(BlockClass block)
    {
        if(playerDataBase.Armor.Length > 0)
        {
            collectionManager.CheckUnEquip(playerDataBase.Armor);
        }

        playerDataBase.Armor = block.instanceId;

        collectionManager.CheckEquip(playerDataBase.Armor);

        armorBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("Armor", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("°©¿Ê ÀåÂø : " + block.blockType);
    }

    public void EquipWeapon(BlockClass block)
    {
        if (playerDataBase.Weapon.Length > 0)
        {
            collectionManager.CheckUnEquip(playerDataBase.Weapon);
        }

        playerDataBase.Weapon = block.instanceId;

        collectionManager.CheckEquip(playerDataBase.Weapon);

        weaponBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("Weapon", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("°Ë ÀåÂø : " + block.blockType);
    }

    public void EquipShield(BlockClass block)
    {
        if (playerDataBase.Shield.Length > 0)
        {
            collectionManager.CheckUnEquip(playerDataBase.Shield);
        }

        playerDataBase.Shield = block.instanceId;

        collectionManager.CheckEquip(playerDataBase.Shield);

        shieldBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("Shield", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("¹æÆÐ ÀåÂø : " + block.blockType);
    }

    public void EquipNewBie(BlockClass block)
    {
        if (playerDataBase.Newbie.Length > 0)
        {
            collectionManager.CheckUnEquip(playerDataBase.Newbie);
        }

        playerDataBase.Newbie = block.instanceId;

        collectionManager.CheckEquip(playerDataBase.Newbie);

        newbieBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("NewBie", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("´ººñ ÀåÂø : " + block.blockType);
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
}
