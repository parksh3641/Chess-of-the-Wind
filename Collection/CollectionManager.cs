using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public GameObject collectionView;


    [Title("Equip")]
    public BlockUIContent armorBlockUI;
    public BlockUIContent weaponBlockUI;
    public BlockUIContent shieldBlockUI;
    public BlockUIContent newbieBlockUI;

    public BlockUIContent blockUIContent;
    public Transform blockUITransform;

    public List<BlockUIContent> blockUIContentList = new List<BlockUIContent>();

    public List<BlockClass> blockList = new List<BlockClass>();

    Dictionary<string, string> blockData = new Dictionary<string, string>();

    bool check = false;

    public UpgradeManager upgradeManager;
    public PresentManager presentManager;
    PlayerDataBase playerDataBase;

    void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        collectionView.SetActive(false);

        for (int i = 0; i < 100; i++)
        {
            BlockUIContent content = Instantiate(blockUIContent);
            content.transform.parent = blockUITransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);

            blockUIContentList.Add(content);
        }
    }

    public void OpenCollectionView()
    {
        if(!collectionView.activeSelf)
        {
            collectionView.SetActive(true);

            if (!check) //�� �ѹ��� üũ��
            {
                check = true;

                Initialize();
            }
            else
            {
                if (playerDataBase.GetBlockClass().Count > blockList.Count)
                {
                    UpdateCollection();
                }
            }
        }
    }

    public void CloseCollectionView()
    {
        collectionView.SetActive(false);
    }

    public void Initialize()
    {
        for(int i = 0; i < playerDataBase.GetBlockClass().Count; i ++)
        {
            blockList.Add(playerDataBase.GetBlockClass()[i]);
        }

        if(blockUIContentList.Count < blockList.Count)
        {
            //������ �ͺ��� ������ �� ���� ���
        }

        for(int i = 0; i < blockUIContentList.Count; i ++)
        {
            blockUIContentList[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < blockList.Count; i ++)
        {
            blockUIContentList[i].gameObject.SetActive(true);
            blockUIContentList[i].Collection_Initialize(blockList[i]);
        }

        CheckEquipArmor();
        CheckEquipWeapon();
        CheckEquipShield();
        CheckEquipNewBie();
    }

    public void UpdateCollection() //�������� ������ ��� ������Ʈ
    {
        Debug.Log("�÷��� ���� �� ������Ʈ");

        for (int i = 0; i < playerDataBase.GetBlockClass().Count - blockList.Count; i++)
        {
            blockList.Add(playerDataBase.GetBlockClass()[playerDataBase.GetBlockClass().Count - i - 1]);
            blockUIContentList[playerDataBase.GetBlockClass().Count - i - 1].gameObject.SetActive(true);
            blockUIContentList[playerDataBase.GetBlockClass().Count - i - 1].Collection_Initialize(blockList[i]);
        }

        if (blockUIContentList.Count < blockList.Count)
        {
            //������ �ͺ��� ������ �� ���� ���
        }
    }

    public void CheckEquipArmor()
    {
        if (playerDataBase.Armor.Length <= 0) //�����Ѱ� ���� ��� �⺻ �������ֱ�
        {
            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].blockType == BlockType.LeftQueen_2 || blockList[i].blockType == BlockType.RightQueen_2)
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                    EquipArmor(blockList[i]);
                    break;
                }
            }
        }
        else
        {
            bool equip = false;

            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].instanceId.Equals(playerDataBase.Armor))
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                    EquipArmor(blockList[i]);
                    equip = true;
                    break;
                }
            }

            if (!equip)
            {
                playerDataBase.Armor = "";
                CheckEquipArmor();
            }
        }
    }

    public void CheckEquipWeapon()
    {
        if (playerDataBase.Weapon.Length <= 0)
        {
            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].blockType == BlockType.LeftNight || blockList[i].blockType == BlockType.RightNight)
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                    EquipWeapon(blockList[i]);
                    break;
                }
            }
        }
        else
        {
            bool equip = false;

            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].instanceId.Equals(playerDataBase.Weapon))
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                    EquipWeapon(blockList[i]);
                    equip = true;
                    break;
                }
            }

            if (!equip)
            {
                playerDataBase.Weapon = "";
                CheckEquipWeapon();
            }
        }
    }

    public void CheckEquipShield()
    {
        if (playerDataBase.Shield.Length <= 0)
        {
            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].blockType == BlockType.Rook_V2 || blockList[i].blockType == BlockType.Rook_V2H2)
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                    EquipShield(blockList[i]);
                    break;
                }
            }
        }
        else
        {
            bool equip = false;

            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].instanceId.Equals(playerDataBase.Shield))
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                    EquipShield(blockList[i]);
                    equip = true;
                    break;
                }
            }

            if (!equip)
            {
                playerDataBase.Shield = "";
                CheckEquipShield();
            }
        }
    }

    public void CheckEquipNewBie()
    {
        if (playerDataBase.Newbie.Length <= 0)
        {
            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].blockType == BlockType.Pawn)
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                    EquipNewBie(blockList[i]);
                    break;
                }
            }
        }
        else
        {
            bool equip = false;

            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].instanceId.Equals(playerDataBase.Newbie))
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                    EquipNewBie(blockList[i]);
                    equip = true;

                    break;
                }
            }

            if(!equip)
            {
                playerDataBase.Newbie = "";
                CheckEquipNewBie();
            }
        }
    }

    public void EquipArmor(BlockClass block)
    {
        playerDataBase.Armor = block.instanceId;

        armorBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("Armor", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("���� ���� : " + block.blockType);
    }

    public void EquipWeapon(BlockClass block)
    {
        playerDataBase.Weapon = block.instanceId;

        weaponBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("Weapon", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("�� ���� : " + block.blockType);
    }

    public void EquipShield(BlockClass block)
    {
        playerDataBase.Shield = block.instanceId;

        shieldBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("Shield", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("���� ���� : " + block.blockType);
    }

    public void EquipNewBie(BlockClass block)
    {
        playerDataBase.Newbie = block.instanceId;

        newbieBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("NewBie", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("���� ���� : " + block.blockType);
    }

    #region BlockInformation
    public void OpenBlockInformation(string id)
    {
        upgradeManager.OpenUpgradeView(id);
    }
    #endregion
}
