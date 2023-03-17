using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    public GameObject collectionView;

    public Text sortText;

    [Space]
    [Title("Value")]
    public int sortCount;
    

    [Space]
    [Title("Prefab")]
    public BlockUIContent blockUIContent;
    public Transform blockUITransform;

    public List<BlockClass> blockList = new List<BlockClass>();
    public List<BlockUIContent> blockUIContentList = new List<BlockUIContent>();


    WaitForSeconds waitForSeconds = new WaitForSeconds(0.03f);

    bool check = false;

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

        sortCount = 0;

        sortText.text = "등급 순 ▲";
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
                //if (blockList.Count != playerDataBase.GetBlockClass().Count)
                //{
                //    UpdateCollection();
                //}

                UpdateCollection();
            }
        }
    }

    public void CloseCollectionView()
    {
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
        }

        StartCoroutine(DelayCheckEquip());
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

        if(playerDataBase.EquipBlock > 0)
        {
            StartCoroutine(DelayCheckEquip());

            playerDataBase.EquipBlock = 0;
        }
    }

    IEnumerator DelayCheckEquip()
    {
        CheckEquipArmor();
        yield return waitForSeconds;
        CheckEquipWeapon();
        yield return waitForSeconds;
        CheckEquipShield();
        yield return waitForSeconds;
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
                yield return waitForSeconds;
            }

            if (blockList[i].blockType == BlockType.LeftNight || blockList[i].blockType == BlockType.RightNight)
            {
                equipManager.EquipWeapon(blockList[i], true);
                yield return waitForSeconds;
            }

            if (blockList[i].blockType == BlockType.Rook_V2 || blockList[i].blockType == BlockType.Rook_V2H2)
            {
                equipManager.EquipShield(blockList[i], true);
                yield return waitForSeconds;
            }

            if (blockList[i].blockType == BlockType.Pawn)
            {
                equipManager.EquipNewBie(blockList[i], false);
                yield return waitForSeconds;
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
                if (blockList[i].blockType == BlockType.Pawn)
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
        if(sortCount == 0)
        {
            blockList = blockList.OrderBy(x => x.rankType).ToList();

            sortText.text = "등급 순 ▼";

            sortCount = 1;
        }
        else if(sortCount == 1)
        {
            blockList = blockList.OrderByDescending(x => x.blockType).ToList();

            sortText.text = "종류 순 ▲";

            sortCount = 2;
        }
        else if (sortCount == 2)
        {
            blockList = blockList.OrderBy(x => x.blockType).ToList();

            sortText.text = "종류 순 ▼";

            sortCount = 3;
        }
        else
        {
            blockList = blockList.OrderByDescending(x => x.rankType).ToList();

            sortText.text = "등급 순 ▲";

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
    }

    #endregion
}
