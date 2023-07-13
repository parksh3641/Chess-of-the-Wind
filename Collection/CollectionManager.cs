using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    public GameObject collectionView;

    public Image characterImg;

    Sprite[] characterArray;

    public Text sortText;

    [Space]
    [Title("Value")]
    public int sortCount;

    public bool change = false;
    

    [Space]
    [Title("Prefab")]
    public BlockUIContent blockUIContent;
    public Transform blockUITransform;

    public List<BlockClass> blockList = new List<BlockClass>();
    public List<string> alarmList = new List<string>();
    public List<BlockUIContent> blockUIContentList = new List<BlockUIContent>();


    WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);

    bool check = false;

    public EquipManager equipManager;
    public UpgradeManager upgradeManager;
    public PresentManager presentManager;


    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;


    void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        characterArray = imageDataBase.GetCharacterArray();

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

        sortCount = 0;

        sortText.text = "종류 순";
    }

    public void OpenCollectionView()
    {
        if(!collectionView.activeSelf)
        {
            collectionView.SetActive(true);

            if(playerDataBase.Formation == 2)
            {
                characterImg.sprite = characterArray[1];
            }
            else
            {
                characterImg.sprite = characterArray[0];
            }


            if (!check) //딱 한번만 체크함
            {
                check = true;

                Initialize();
            }
            else
            {
                //if (blockList.Count != playerDataBase.GetBlockClass().Count)
                //{
                //    UpdateCollection();
                //}

                if(change)
                {
                    change = false;

                    UpdateCollection();
                }
            }
        }
    }

    public void CloseCollectionView()
    {
        for (int i = 0; i < blockList.Count; i++) //알람 설정
        {
            blockUIContentList[i].alarm.SetActive(false);
        }

        collectionView.SetActive(false);

        upgradeManager.CloseUpgradeView();
    }

    public void Initialize()
    {
        blockList = new List<BlockClass>(blockList.Count);

        for (int i = 0; i < playerDataBase.GetBlockClass().Count; i ++)
        {
            blockList.Add(playerDataBase.GetBlockClass()[i]);
        }

        blockList = blockList.OrderByDescending(x => x.rankType).ToList();

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
            alarmList.Add(blockList[i].instanceId);
        }

        for (int i = 0; i < blockList.Count; i++)
        {
            for (int j = 0; j < playerDataBase.sellBlockList.Count; j++)
            {
                if (blockList[i].instanceId.Equals(playerDataBase.sellBlockList[j]))
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                }
            }
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

        blockList = blockList.OrderByDescending(x => x.rankType).ToList();

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
            if (!blockList[i].instanceId.Equals(playerDataBase.Armor) &&
                !blockList[i].instanceId.Equals(playerDataBase.Weapon) &&
                !blockList[i].instanceId.Equals(playerDataBase.Shield) &&
                !blockList[i].instanceId.Equals(playerDataBase.Newbie))
            {
                blockUIContentList[i].gameObject.SetActive(true);
                blockUIContentList[i].Collection_Initialize(blockList[i]);
            }
        }

        for (int i = 0; i < blockList.Count; i++)
        {
            for (int j = 0; j < playerDataBase.sellBlockList.Count; j++)
            {
                if (blockList[i].instanceId.Equals(playerDataBase.sellBlockList[j]))
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                }
            }
        }

        for (int i = 0; i < blockList.Count; i++) //알람 설정
        {
            blockUIContentList[i].alarm.SetActive(true);

            for (int j = 0; j < alarmList.Count; j++)
            {
                if(blockList[i].instanceId.Equals(alarmList[j]))
                {
                    blockUIContentList[i].alarm.SetActive(false);
                }
            }
        }

        alarmList.Clear();

        for (int i = 0; i < blockList.Count; i++)
        {
            alarmList.Add(blockList[i].instanceId);
        }


        CheckEquipArmor();
        CheckEquipWeapon();
        CheckEquipShield();
        CheckEquipNewBie();
    }

    public void FirstEquipCheck()
    {
        StartCoroutine(FirstEquipCoroution());
    }

    IEnumerator FirstEquipCoroution()
    {
        blockList = new List<BlockClass>(blockList.Count);

        for (int i = 0; i < playerDataBase.GetBlockClass().Count; i++)
        {
            blockList.Add(playerDataBase.GetBlockClass()[i]);
        }

        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].blockType == BlockType.LeftQueen_2 || blockList[i].blockType == BlockType.RightQueen_2)
            {
                equipManager.EquipArmor(blockList[i], true);
                break;
            }
        }

        yield return waitForSeconds;

        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].blockType == BlockType.LeftNight || blockList[i].blockType == BlockType.RightNight)
            {
                equipManager.EquipWeapon(blockList[i], true);
                break;
            }
        }

        yield return waitForSeconds;

        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].blockType == BlockType.Rook_V2 || blockList[i].blockType == BlockType.Rook_V4)
            {
                equipManager.EquipShield(blockList[i], true);
                break;
            }
        }

        yield return waitForSeconds;

        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].blockType == BlockType.Pawn_Snow || blockList[i].blockType == BlockType.Pawn_Under)
            {
                equipManager.EquipNewBie(blockList[i], true);
                break;
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
                    equipManager.EquipArmor(blockList[i], false);
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
                    equipManager.EquipArmor(blockList[i], false);
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
                    equipManager.EquipWeapon(blockList[i], false);
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
                    equipManager.EquipWeapon(blockList[i], false);
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
                    equipManager.EquipShield(blockList[i], false);
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
                    equipManager.EquipShield(blockList[i], false);
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
                if (blockList[i].blockType == BlockType.Pawn_Snow || blockList[i].blockType == BlockType.Pawn_Under)
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                    equipManager.EquipNewBie(blockList[i], false);
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
                    equipManager.EquipNewBie(blockList[i], false);
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


    public void CheckEquip(string id)
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].instanceId.Equals(id))
            {
                blockUIContentList[i].gameObject.SetActive(false);
                break;
            }
        }
    }

    public void CheckUnEquip(string id)
    {
        for(int i = 0; i < blockList.Count; i ++)
        {
            if(blockList[i].instanceId.Equals(id))
            {
                blockUIContentList[i].gameObject.SetActive(true);
                break;
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

    #region Sort

    public void SortButton()
    {
        blockList = new List<BlockClass>(blockList.Count);

        for (int i = 0; i < playerDataBase.GetBlockClass().Count; i++)
        {
            blockList.Add(playerDataBase.GetBlockClass()[i]);
        }

        if (sortCount == 0)
        {
            blockList = blockList.OrderByDescending(x => x.blockType).OrderByDescending(x => x.rankType).OrderByDescending(x => x.level).ToList();

            sortText.text = "레벨 순";

            sortCount = 1;
        }
        else if (sortCount == 1)
        {
            blockList = blockList.OrderByDescending(x => x.blockType).OrderByDescending(x => x.rankType).ToList();

            sortText.text = "종류 순";

            sortCount = 0;
        }

        for (int i = 0; i < blockUIContentList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < blockList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(true);
            blockUIContentList[i].Collection_Initialize(blockList[i]);
        }

        CheckEquip(playerDataBase.Armor);
        CheckEquip(playerDataBase.Weapon);
        CheckEquip(playerDataBase.Shield);
        CheckEquip(playerDataBase.Newbie);
    }

    #endregion
}
