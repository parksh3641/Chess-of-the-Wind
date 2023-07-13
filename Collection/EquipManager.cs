using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public GameObject[] equipView;

    [Title("Colletion")]
    public BlockUIContent armorBlockUI;
    public BlockUIContent weaponBlockUI;
    public BlockUIContent shieldBlockUI;
    public BlockUIContent newbieBlockUI;

    [Title("Equip")]
    public BlockEquipUIContent blockEquipUIContent;

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

        equipView[0].SetActive(false);
        equipView[1].SetActive(false);

        armorBlockUI.Upgrade_Initialize(collectionManager);
        weaponBlockUI.Upgrade_Initialize(collectionManager);
        shieldBlockUI.Upgrade_Initialize(collectionManager);
        newbieBlockUI.Upgrade_Initialize(collectionManager);
    }

    public void OpenEquipView(BlockClass block)
    {
        if(!equipView[0].activeSelf)
        {
            equipView[0].SetActive(true);
            equipView[1].SetActive(true);

            blockClass = block;

            blockEquipUIContent.Initialize(block);
        }
    }

    public void CloseEquipView()
    {
        equipView[0].SetActive(false);
        equipView[1].SetActive(false);
    }


    public void ChangeArmor()
    {
        switch (playerDataBase.CheckOverlapBlock(blockClass, 0))
        {
            case 1:

                break;
            case 2:
                EquipWeapon(armorBlockUI.blockClass, false);
                break;
            case 3:
                EquipShield(armorBlockUI.blockClass, false);
                break;
        }

        EquipArmor(blockClass, false);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();

        SoundManager.instance.PlaySFX(GameSfxType.BlockEquip);
    }

    public void ChangeWeapon()
    {
        switch (playerDataBase.CheckOverlapBlock(blockClass, 0))
        {
            case 1:
                EquipArmor(weaponBlockUI.blockClass, false);
                break;
            case 2:

                break;
            case 3:
                EquipShield(weaponBlockUI.blockClass, false);
                break;
        }

        EquipWeapon(blockClass, false);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();

        SoundManager.instance.PlaySFX(GameSfxType.BlockEquip);
    }

    public void ChangeShield()
    {
        switch (playerDataBase.CheckOverlapBlock(blockClass, 0))
        {
            case 1:
                EquipArmor(shieldBlockUI.blockClass, false);
                break;
            case 2:
                EquipWeapon(shieldBlockUI.blockClass, false);
                break;
            case 3:

                break;
        }

        EquipShield(blockClass, false);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();

        SoundManager.instance.PlaySFX(GameSfxType.BlockEquip);
    }

    public void ChangeNewbie()
    {
        if(blockClass.blockType == BlockType.Pawn_Under || blockClass.blockType == BlockType.Pawn_Snow)
        {
            EquipNewBie(blockClass, false);

            upgradeManager.CloseUpgradeView();

            CloseEquipView();

            SoundManager.instance.PlaySFX(GameSfxType.BlockEquip);
        }
        else
        {
            NotionManager.instance.UseNotion(NotionType.OnlyPawn);
            return;
        }
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

        //Debug.Log("∞©ø  ¿Â¬¯ : " + block.blockType);
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

        //Debug.Log("∞À ¿Â¬¯ : " + block.blockType);
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

        //Debug.Log("πÊ∆– ¿Â¬¯ : " + block.blockType);
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

        //Debug.Log("¥∫∫Ò ¿Â¬¯ : " + block.blockType);
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
