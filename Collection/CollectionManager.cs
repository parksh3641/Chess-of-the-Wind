using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public GameObject collectionView;


    public BlockUIContent blockUIContent;
    public Transform blockUITransform;

    public List<BlockClass> blockList = new List<BlockClass>();

    public List<BlockUIContent> blockUIContentList = new List<BlockUIContent>();



    bool check = false;

    BlockClass armorBlockClass;
    BlockClass weaponBlockClass;
    BlockClass shieldBlockClass;
    BlockClass newbieBlockClass;

    public EquipManager equipManager;
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
            content.Upgrade_Initialize(this);

            blockUIContentList.Add(content);
        }
    }

    public void OpenCollectionView()
    {
        if(!collectionView.activeSelf)
        {
            collectionView.SetActive(true);

            if (!check) //딱 한번만 체크함
            {
                check = true;

                Initialize();
            }
            else
            {
                if (blockList.Count != playerDataBase.GetBlockClass().Count)
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
        blockList = new List<BlockClass>(blockList.Count);

        for (int i = 0; i < playerDataBase.GetBlockClass().Count; i ++)
        {
            blockList.Add(playerDataBase.GetBlockClass()[i]);
        }

        if (blockUIContentList.Count < blockList.Count)
        {
            int number = blockList.Count - blockUIContentList.Count;

            for (int i = 0; i < number; i++)
            {
                BlockUIContent content = Instantiate(blockUIContent);
                content.transform.parent = blockUITransform;
                content.transform.localPosition = Vector3.zero;
                content.transform.localScale = Vector3.one;
                content.gameObject.SetActive(false);
                content.Upgrade_Initialize(this);

                blockUIContentList.Add(content);
            }
        }

        for (int i = 0; i < blockUIContentList.Count; i ++)
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

    public void UpdateCollection() //변경점이 생겼을 경우 업데이트
    {
        Debug.Log("컬렉션 변경 점 업데이트");

        blockList = new List<BlockClass>(blockList.Count);

        for (int i = 0; i < playerDataBase.GetBlockClass().Count; i++)
        {
            blockList.Add(playerDataBase.GetBlockClass()[i]);
        }

        if (blockUIContentList.Count < blockList.Count)
        {
            int number = blockList.Count - blockUIContentList.Count;

            for (int i = 0; i < number; i++)
            {
                BlockUIContent content = Instantiate(blockUIContent);
                content.transform.parent = blockUITransform;
                content.transform.localPosition = Vector3.zero;
                content.transform.localScale = Vector3.one;
                content.gameObject.SetActive(false);
                content.Upgrade_Initialize(this);

                blockUIContentList.Add(content);
            }
        }

        for (int i = 0; i < blockUIContentList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < blockList.Count; i++)
        {
            if (!blockList[i].instanceId.Equals(armorBlockClass.instanceId) &&
                !blockList[i].instanceId.Equals(weaponBlockClass.instanceId) &&
                !blockList[i].instanceId.Equals(shieldBlockClass.instanceId) &&
                !blockList[i].instanceId.Equals(newbieBlockClass.instanceId))
            {
                blockUIContentList[i].gameObject.SetActive(true);
                blockUIContentList[i].Collection_Initialize(blockList[i]);
            }
        }
    }
   

    public void CheckEquipArmor()
    {
        if (playerDataBase.Armor.Length <= 0) //장착한게 없을 경우 기본 세팅해주기
        {
            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].blockType == BlockType.LeftQueen_2 || blockList[i].blockType == BlockType.RightQueen_2)
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                    armorBlockClass = blockList[i];
                    equipManager.EquipArmor(blockList[i]);
                    break;
                }
            }
        }
        else //불러오기
        {
            bool equip = false;

            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].instanceId.Equals(playerDataBase.Armor))
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                    armorBlockClass = blockList[i];
                    equipManager.EquipArmor(blockList[i]);
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
                    weaponBlockClass = blockList[i];
                    equipManager.EquipWeapon(blockList[i]);
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
                    weaponBlockClass = blockList[i];
                    equipManager.EquipWeapon(blockList[i]);
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
                    shieldBlockClass = blockList[i];
                    equipManager.EquipShield(blockList[i]);
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
                    shieldBlockClass = blockList[i];
                    equipManager.EquipShield(blockList[i]);
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
                    newbieBlockClass = blockList[i];
                    equipManager.EquipNewBie(blockList[i]);
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
                    newbieBlockClass = blockList[i];
                    equipManager.EquipNewBie(blockList[i]);
                    equip = true;

                    break;
                }
            }

            if (!equip)
            {
                playerDataBase.Newbie = "";
                CheckEquipNewBie();
            }
        }
    }

    #region BlockInformation
    public void OpenBlockInformation(string id)
    {
        upgradeManager.OpenUpgradeView(id);
    }

    public void SetBlockLevel(string id, int level)
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].instanceId.Equals(id))
            {
                blockList[i].level = level;
                blockUIContentList[i].SetLevel(level);
                break;
            }
        }
    }
    #endregion
}
