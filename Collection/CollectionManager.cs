using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    public GameObject collectionView;

    public GameObject mainAlarm;

    public LocalizationContent totalRafText;
    public Image characterImg;

    Sprite[] characterArray;

    public LocalizationContent sortText;

    [Space]
    [Title("Value")]
    public int sortCount = 0;

    private int totalRaf = 0;
    private int saveTotalRaf = 0;
    private int number = 0;

    public bool change = false;

    private bool sortDelay = false;

    bool first = false;


    [Space]
    [Title("Prefab")]
    public BlockUIContent blockUIContent;
    public Transform blockUITransform;

    public List<BlockClass> blockList = new List<BlockClass>();
    public List<string> alarmList = new List<string>();
    public List<BlockUIContent> blockUIContentList = new List<BlockUIContent>();

    List<string> itemList = new List<string>();

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);

    public EquipManager equipManager;
    public UpgradeManager upgradeManager;
    public PresentManager presentManager;


    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;
    UpgradeDataBase upgradeDataBase;


    void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;

        characterArray = imageDataBase.GetCharacterArray();

        collectionView.SetActive(false);

        for (int i = 0; i < 50; i++)
        {
            BlockUIContent content = Instantiate(blockUIContent);
            content.transform.SetParent(blockUITransform);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            content.Upgrade_Initialize(this);

            blockUIContentList.Add(content);
        }

        mainAlarm.SetActive(true);
    }

    [Button]
    public void Checking()
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].instanceId.Equals(playerDataBase.Armor) ||
                blockList[i].instanceId.Equals(playerDataBase.Weapon) ||
                blockList[i].instanceId.Equals(playerDataBase.Shield) ||
                blockList[i].instanceId.Equals(playerDataBase.Newbie))
            {
                blockUIContentList[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenCollectionView()
    {
        if(!collectionView.activeSelf)
        {
            collectionView.SetActive(true);

            mainAlarm.SetActive(false);

            if (playerDataBase.Formation == 2)
            {
                characterImg.sprite = characterArray[1];
            }
            else
            {
                characterImg.sprite = characterArray[0];
            }

            if (!first) //딱 한번만 체크함
            {
                first = true;

                sortCount = 0;
                sortText.localizationName = "ByType";
                sortText.ReLoad();

                Initialize();

                CheckEquipArmor();
                CheckEquipWeapon();
                CheckEquipShield();
                CheckEquipNewBie();

                CheckTotalRaf();
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

    public void CheckTotalRaf()
    {
        totalRaf = 0;

        if (!string.IsNullOrEmpty(playerDataBase.Armor))
        {
            totalRaf += upgradeDataBase.GetUpgradeValue(playerDataBase.GetBlockClass(playerDataBase.Armor).rankType).GetValueNumber(
                playerDataBase.GetBlockClass(playerDataBase.Armor).level);
        }

        if (!string.IsNullOrEmpty(playerDataBase.Weapon))
        {
            totalRaf += upgradeDataBase.GetUpgradeValue(playerDataBase.GetBlockClass(playerDataBase.Weapon).rankType).GetValueNumber(
    playerDataBase.GetBlockClass(playerDataBase.Weapon).level);
        }

        if (!string.IsNullOrEmpty(playerDataBase.Shield))
        {
            totalRaf += upgradeDataBase.GetUpgradeValue(playerDataBase.GetBlockClass(playerDataBase.Shield).rankType).GetValueNumber(
    playerDataBase.GetBlockClass(playerDataBase.Shield).level);
        }

        if (!string.IsNullOrEmpty(playerDataBase.Newbie))
        {
            totalRaf += upgradeDataBase.GetUpgradeValue(playerDataBase.GetBlockClass(playerDataBase.Newbie).rankType).GetValueNumber(
    playerDataBase.GetBlockClass(playerDataBase.Newbie).level);
        }

        totalRafText.localizationName = "CollectionTotal";
        totalRafText.plusText = "\n" + MoneyUnitString.ToCurrencyString(totalRaf);
        totalRafText.ReLoad();

        if(saveTotalRaf == 0)
        {
            saveTotalRaf = totalRaf;
        }
        else
        {
            if(saveTotalRaf > totalRaf)
            {
                number = saveTotalRaf - totalRaf;

                saveTotalRaf = totalRaf;

                NotionManager.instance.UseNotion(Color.red, LocalizationManager.instance.GetString("CollectionTotal") + " -" + number);
            }
            else if(saveTotalRaf < totalRaf)
            {
                number = totalRaf - saveTotalRaf;

                saveTotalRaf = totalRaf;

                NotionManager.instance.UseNotion(Color.green, LocalizationManager.instance.GetString("CollectionTotal") + " +" + number);
            }
        }

        if(totalRaf != playerDataBase.TotalRaf)
        {
            if(playerDataBase.TestAccount > 0)
            {
                playerDataBase.TotalRaf = 0;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("TotalRaf", 0);
            }
            else
            {
                playerDataBase.TotalRaf = totalRaf;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("TotalRaf", totalRaf);
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

        if(blockList.Count == 0)
        {
            if(GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
            {
                itemList.Clear();
                itemList.Add("Pawn_Snow_N");

                PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", itemList);
            }
            else
            {
                itemList.Clear();
                itemList.Add("Pawn_Under_N");

                PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", itemList);
            }
        }

        for (int i = 0; i < playerDataBase.GetBlockClass().Count; i ++)
        {
            blockList.Add(playerDataBase.GetBlockClass()[i]);
        }

        blockList = blockList.OrderByDescending(x => x.blockType).OrderByDescending(x => x.rankType).ToList();

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
            blockUIContentList[i].Initialize_UI(blockList[i]);
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
                blockUIContentList[i].Initialize_UI(blockList[i]);
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


        //CheckEquipArmor();
        //CheckEquipWeapon();
        //CheckEquipShield();
        //CheckEquipNewBie();
    }

    //public void FirstEquipCheck()
    //{
    //    StartCoroutine(FirstEquipCoroution());
    //}

    //IEnumerator FirstEquipCoroution()
    //{
    //    blockList = new List<BlockClass>(blockList.Count);

    //    for (int i = 0; i < playerDataBase.GetBlockClass().Count; i++)
    //    {
    //        blockList.Add(playerDataBase.GetBlockClass()[i]);
    //    }

    //    for (int i = 0; i < blockList.Count; i++)
    //    {
    //        if (blockList[i].blockType == BlockType.LeftQueen_2 || blockList[i].blockType == BlockType.RightQueen_2)
    //        {
    //            equipManager.EquipArmor(blockList[i], true);
    //            break;
    //        }
    //    }

    //    yield return waitForSeconds;

    //    for (int i = 0; i < blockList.Count; i++)
    //    {
    //        if (blockList[i].blockType == BlockType.LeftNight || blockList[i].blockType == BlockType.RightNight)
    //        {
    //            equipManager.EquipWeapon(blockList[i], true);
    //            break;
    //        }
    //    }

    //    yield return waitForSeconds;

    //    for (int i = 0; i < blockList.Count; i++)
    //    {
    //        if (blockList[i].blockType == BlockType.Rook_V2 || blockList[i].blockType == BlockType.Rook_V4)
    //        {
    //            equipManager.EquipShield(blockList[i], true);
    //            break;
    //        }
    //    }

    //    yield return waitForSeconds;

    //    for (int i = 0; i < blockList.Count; i++)
    //    {
    //        if (blockList[i].blockType == BlockType.Pawn_Snow || blockList[i].blockType == BlockType.Pawn_Under)
    //        {
    //            equipManager.EquipNewBie(blockList[i], true);
    //            break;
    //        }
    //    }
    //}

    public void CheckEquipArmor()
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].instanceId.Equals(playerDataBase.Armor))
            {
                blockUIContentList[i].gameObject.SetActive(false);
                equipManager.EquipArmor(blockList[i]);
                break;
            }
        }
    }

    public void CheckEquipWeapon()
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].instanceId.Equals(playerDataBase.Weapon))
            {
                blockUIContentList[i].gameObject.SetActive(false);
                equipManager.EquipWeapon(blockList[i]);
                break;
            }
        }
    }

    public void CheckEquipShield()
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].instanceId.Equals(playerDataBase.Shield))
            {
                blockUIContentList[i].gameObject.SetActive(false);
                equipManager.EquipShield(blockList[i]);
                break;
            }
        }
    }

    public void CheckEquipNewBie()
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].instanceId.Equals(playerDataBase.Newbie))
            {
                blockUIContentList[i].gameObject.SetActive(false);
                equipManager.EquipNewBie(blockList[i]);

                break;

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

        CheckTotalRaf();
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

        CheckTotalRaf();
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

    public void SetBlockSSRLevel(string id, int level)
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].instanceId.Equals(id))
            {
                blockList[i].ssrLevel = level;
                blockUIContentList[i].SetPieceLevel(level);
                break;
            }
        }
    }

    #endregion

    #region Sort

    public void SortButton()
    {
        if (sortDelay) return;
        sortDelay = true;

        blockList = new List<BlockClass>(blockList.Count);

        for (int i = 0; i < playerDataBase.GetBlockClass().Count; i++)
        {
            blockList.Add(playerDataBase.GetBlockClass()[i]);
        }

        if (sortCount == 0)
        {
            blockList = blockList.OrderByDescending(x => x.blockType).OrderByDescending(x => x.rankType).OrderByDescending(x => x.level).ToList();

            sortText.localizationName = "ByLevel";
            sortText.ReLoad();

            sortCount = 1;
        }
        else if (sortCount == 1)
        {
            blockList = blockList.OrderByDescending(x => x.blockType).OrderByDescending(x => x.rankType).ToList();

            sortText.localizationName = "ByType";
            sortText.ReLoad();

            sortCount = 0;
        }

        for (int i = 0; i < blockUIContentList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < blockList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(true);
            blockUIContentList[i].Initialize_UI(blockList[i]);
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

        CheckEquip(playerDataBase.Armor);
        CheckEquip(playerDataBase.Weapon);
        CheckEquip(playerDataBase.Shield);
        CheckEquip(playerDataBase.Newbie);

        Invoke("Delay", 0.5f);
    }

    void Delay()
    {
        sortDelay = false;
    }

    #endregion
}
