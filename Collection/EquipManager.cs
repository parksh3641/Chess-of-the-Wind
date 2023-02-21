using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public GameObject equipView;

    [Title("Equip")]
    public BlockUIContent armorBlockUI;
    public BlockUIContent weaponBlockUI;
    public BlockUIContent shieldBlockUI;
    public BlockUIContent newbieBlockUI;

    public CollectionManager collectionManager;

    Dictionary<string, string> blockData = new Dictionary<string, string>();

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        equipView.SetActive(false);

        armorBlockUI.Upgrade_Initialize(collectionManager);
        weaponBlockUI.Upgrade_Initialize(collectionManager);
        shieldBlockUI.Upgrade_Initialize(collectionManager);
        newbieBlockUI.Upgrade_Initialize(collectionManager);
    }

    public void OpenEquipView()
    {
        if(!equipView.activeSelf)
        {
            equipView.SetActive(true);
        }
    }


    public void EquipArmor(BlockClass block)
    {
        playerDataBase.Armor = block.instanceId;

        armorBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("Armor", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("°©¿Ê ÀåÂø : " + block.blockType);
    }

    public void EquipWeapon(BlockClass block)
    {
        playerDataBase.Weapon = block.instanceId;

        weaponBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("Weapon", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("°Ë ÀåÂø : " + block.blockType);
    }

    public void EquipShield(BlockClass block)
    {
        playerDataBase.Shield = block.instanceId;

        shieldBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("Shield", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("¹æÆÐ ÀåÂø : " + block.blockType);
    }

    public void EquipNewBie(BlockClass block)
    {
        playerDataBase.Newbie = block.instanceId;

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
