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

        armorBlockUI.gameObject.SetActive(false);
        weaponBlockUI.gameObject.SetActive(false);
        shieldBlockUI.gameObject.SetActive(false);
        newbieBlockUI.gameObject.SetActive(false);
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
        if (!armorBlockUI.gameObject.activeInHierarchy)
        {
            UnEquipSameType(blockClass);
        }
        else
        {
            switch (playerDataBase.CheckOverlapBlock(blockClass))
            {
                case 1:

                    break;
                case 2:
                    EquipWeapon(armorBlockUI.blockClass);
                    break;
                case 3:
                    EquipShield(armorBlockUI.blockClass);
                    break;
            }
        }

        EquipArmor(blockClass);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();

        SoundManager.instance.PlaySFX(GameSfxType.BlockEquip);
    }

    public void ChangeWeapon()
    {
        if (!weaponBlockUI.gameObject.activeInHierarchy)
        {
            UnEquipSameType(blockClass);
        }
        else
        {
            switch (playerDataBase.CheckOverlapBlock(blockClass))
            {
                case 1:
                    EquipArmor(weaponBlockUI.blockClass);
                    break;
                case 2:

                    break;
                case 3:
                    EquipShield(weaponBlockUI.blockClass);
                    break;
            }
        }

        EquipWeapon(blockClass);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();

        SoundManager.instance.PlaySFX(GameSfxType.BlockEquip);
    }

    public void ChangeShield()
    {
        if (!shieldBlockUI.gameObject.activeInHierarchy)
        {
            UnEquipSameType(blockClass);
        }
        else
        {
            switch (playerDataBase.CheckOverlapBlock(blockClass))
            {
                case 1:
                    EquipArmor(shieldBlockUI.blockClass);
                    break;
                case 2:
                    EquipWeapon(shieldBlockUI.blockClass);
                    break;
                case 3:

                    break;
            }
        }

        EquipShield(blockClass);

        upgradeManager.CloseUpgradeView();

        CloseEquipView();

        SoundManager.instance.PlaySFX(GameSfxType.BlockEquip);
    }

    public void ChangeNewbie()
    {
        if(blockClass.blockType == BlockType.Pawn_Under || blockClass.blockType == BlockType.Pawn_Snow || blockClass.blockType == BlockType.Pawn_Under_2
            || blockClass.blockType == BlockType.Pawn_Snow_2)
        {
            EquipNewBie(blockClass);

            upgradeManager.CloseUpgradeView();

            CloseEquipView();

            SoundManager.instance.PlaySFX(GameSfxType.BlockEquip);
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.OnlyPawn);
            return;
        }
    }

    public void UnEquipSameType(BlockClass block)
    {
        if(armorBlockUI.gameObject.activeInHierarchy)
        {
            if (playerDataBase.GetBlockClass(playerDataBase.Armor).blockType.Equals(block.blockType))
            {
                CheckUnEquip(armorBlockUI.blockClass.instanceId);

                collectionManager.CheckUnEquip(armorBlockUI.blockClass.instanceId);

                NotionManager.instance.UseNotion(NotionType.EquipSameBlock);
            }
        }

        if (weaponBlockUI.gameObject.activeInHierarchy)
        {
            if (playerDataBase.GetBlockClass(playerDataBase.Weapon).blockType.Equals(block.blockType))
            {
                CheckUnEquip(weaponBlockUI.blockClass.instanceId);

                collectionManager.CheckUnEquip(weaponBlockUI.blockClass.instanceId);

                NotionManager.instance.UseNotion(NotionType.EquipSameBlock);
            }
        }

        if (shieldBlockUI.gameObject.activeInHierarchy)
        {
            if (playerDataBase.GetBlockClass(playerDataBase.Shield).blockType.Equals(block.blockType))
            {
                CheckUnEquip(shieldBlockUI.blockClass.instanceId);

                collectionManager.CheckUnEquip(shieldBlockUI.blockClass.instanceId);

                NotionManager.instance.UseNotion(NotionType.EquipSameBlock);
            }
        }
    }


    public void EquipArmor(BlockClass block)
    {
        collectionManager.CheckUnEquip(playerDataBase.Armor);

        armorBlockUI.gameObject.SetActive(true);

        playerDataBase.Armor = block.instanceId;

        armorBlockUI.Initialize_UI(block);
        collectionManager.CheckEquip(playerDataBase.Armor);

        collectionManager.Checking();

        blockData.Clear();
        blockData.Add("Armor", block.instanceId);

        PlayfabManager.instance.SetPlayerData(blockData);
    }

    public void EquipWeapon(BlockClass block)
    {
        collectionManager.CheckUnEquip(playerDataBase.Weapon);

        weaponBlockUI.gameObject.SetActive(true);

        playerDataBase.Weapon = block.instanceId;

        weaponBlockUI.Initialize_UI(block);
        collectionManager.CheckEquip(playerDataBase.Weapon);

        collectionManager.Checking();

        blockData.Clear();
        blockData.Add("Weapon", block.instanceId);

        PlayfabManager.instance.SetPlayerData(blockData);
    }

    public void EquipShield(BlockClass block)
    {
        collectionManager.CheckUnEquip(playerDataBase.Shield);

        shieldBlockUI.gameObject.SetActive(true);

        playerDataBase.Shield = block.instanceId;

        shieldBlockUI.Initialize_UI(block);
        collectionManager.CheckEquip(playerDataBase.Shield);

        collectionManager.Checking();

        blockData.Clear();
        blockData.Add("Shield", block.instanceId);

        PlayfabManager.instance.SetPlayerData(blockData);
    }

    public void EquipNewBie(BlockClass block)
    {
        collectionManager.CheckUnEquip(playerDataBase.Newbie);

        newbieBlockUI.gameObject.SetActive(true);

        playerDataBase.Newbie = block.instanceId;

        newbieBlockUI.Initialize_UI(block);
        collectionManager.CheckEquip(playerDataBase.Newbie);

        collectionManager.Checking();

        blockData.Clear();
        blockData.Add("NewBie", block.instanceId);

        PlayfabManager.instance.SetPlayerData(blockData);
    }

    public void CheckUnEquip(string id)
    {
        if(armorBlockUI.instanceId.Equals(id))
        {
            armorBlockUI.gameObject.SetActive(false);

            blockData.Clear();
            blockData.Add("Armor", "");

            PlayfabManager.instance.SetPlayerData(blockData);
        }

        if (weaponBlockUI.instanceId.Equals(id))
        {
            weaponBlockUI.gameObject.SetActive(false);

            blockData.Clear();
            blockData.Add("Weapon", "");

            PlayfabManager.instance.SetPlayerData(blockData);
        }

        if (shieldBlockUI.instanceId.Equals(id))
        {
            shieldBlockUI.gameObject.SetActive(false);

            blockData.Clear();
            blockData.Add("Shield", "");

            PlayfabManager.instance.SetPlayerData(blockData);
        }

        if (newbieBlockUI.instanceId.Equals(id))
        {
            newbieBlockUI.gameObject.SetActive(false);

            blockData.Clear();
            blockData.Add("NewBie", "");

            PlayfabManager.instance.SetPlayerData(blockData);
        }

        playerDataBase.CheckUnEquip(id);
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
