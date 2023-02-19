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

            if (!check) //딱 한번만 체크함
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
            //생성한 것보다 보유한 게 많을 경우
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

    public void UpdateCollection() //변경점이 생겼을 경우 업데이트
    {
        Debug.Log("컬렉션 변경 점 업데이트");

        for (int i = 0; i < playerDataBase.GetBlockClass().Count - blockList.Count; i++)
        {
            blockList.Add(playerDataBase.GetBlockClass()[playerDataBase.GetBlockClass().Count - i - 1]);
            blockUIContentList[playerDataBase.GetBlockClass().Count - i - 1].gameObject.SetActive(true);
            blockUIContentList[playerDataBase.GetBlockClass().Count - i - 1].Collection_Initialize(blockList[i]);
        }

        if (blockUIContentList.Count < blockList.Count)
        {
            //생성한 것보다 보유한 게 많을 경우
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

        Debug.Log("갑옷 장착 : " + block.blockType);
    }

    public void EquipWeapon(BlockClass block)
    {
        playerDataBase.Weapon = block.instanceId;

        weaponBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("Weapon", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("검 장착 : " + block.blockType);
    }

    public void EquipShield(BlockClass block)
    {
        playerDataBase.Shield = block.instanceId;

        shieldBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("Shield", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("방패 장착 : " + block.blockType);
    }

    public void EquipNewBie(BlockClass block)
    {
        playerDataBase.Newbie = block.instanceId;

        newbieBlockUI.Collection_Initialize(block);

        blockData.Clear();
        blockData.Add("NewBie", block.instanceId);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(blockData);

        Debug.Log("뉴비 장착 : " + block.blockType);
    }

    #region BlockInformation
    public void OpenBlockInformation(string id)
    {
        upgradeManager.OpenUpgradeView(id);
    }
    #endregion
}
