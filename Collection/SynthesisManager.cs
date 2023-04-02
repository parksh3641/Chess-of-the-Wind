using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SynthesisManager : MonoBehaviour
{
    public GameObject synthesisView;
    public GameObject synthesisResultView;
    public GameObject synthesisResultButton;

    public BlockUIContent nextBlockUIContent;
    public BlockUIContent targetBlockUIContent;
    public BlockUIContent matBlockUIContent1;
    public BlockUIContent matBlockUIContent2;

    public Text titleText;
    public Text upgradeLevelText;
    public Text valueText;

    public Text sortText;

    public GameObject plusObj;
    public GameObject matObj1;
    public GameObject matObj2;

    public GameObject goldObj;
    public Text goldText;

    public GameObject needObj;
    public Text needText;

    public GameObject synthesisButton;

    [Title("Value")]
    private int needGold = 0;
    private int updateLevel = 0;
    private int equipInfo = 0;
    private int sortCount = 0;

    private bool isStart = false; //합성 시작 여부
    private bool isMat1 = false;
    private bool isMat2 = false;
    private bool isReady = false;

    public BlockUIContent blockUIContent;
    public Transform blockUITransform;
    public Transform synthesisTransform;

    public List<BlockClass> blockList = new List<BlockClass>();

    public List<BlockUIContent> blockUIContentList = new List<BlockUIContent>();
    public List<BlockUIContent> synthesisResultContentList = new List<BlockUIContent>();

    List<string> synthesisResultList = new List<string>();

    WindCharacterType windCharacterType = WindCharacterType.Winter;
    RankType rankType = RankType.N;
    BlockClass blockClass;
    BlockClass blockClassMat1;
    BlockClass blockClassMat2;

    UpgradeValue upgradeValue;
    UpgradeValue upgradeValue2;
    UpgradeInformation upgradeInformation;

    public CollectionManager collectionManager;
    public UpgradeManager upgradeManager;

    UpgradeDataBase upgradeDataBase;
    BlockDataBase blockDataBase;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
        if (blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        synthesisView.SetActive(false);

        plusObj.SetActive(false);
        matObj1.SetActive(false);
        matObj2.SetActive(false);
        goldObj.SetActive(false);
        needObj.SetActive(false);
        synthesisButton.SetActive(false);

        for (int i = 0; i < 100; i++)
        {
            BlockUIContent content = Instantiate(blockUIContent);
            content.transform.parent = blockUITransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            content.Synthesis_Initialize(this);

            blockUIContentList.Add(content);
        }

        for (int i = 0; i < 20; i++)
        {
            BlockUIContent content = Instantiate(blockUIContent);
            content.transform.parent = synthesisTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);

            synthesisResultContentList.Add(content);
        }

        sortText.text = "레벨 순";
    }

    public void OpenSynthesisView()
    {
        if (!synthesisView.activeSelf)
        {
            synthesisView.SetActive(true);

            Initialize();
        }
    }

    public void CloseSynthesisView()
    {
        synthesisView.SetActive(false);

        synthesisResultView.SetActive(false);
    }

    void Initialize()
    {
        titleText.text = "";
        upgradeLevelText.text = "합성을 원하는 블록을 선택하세요!";
        valueText.text = "";

        nextBlockUIContent.Reset_Initalize();
        targetBlockUIContent.Reset_Initalize();
        matBlockUIContent1.Reset_Initalize();
        matBlockUIContent2.Reset_Initalize();

        blockList = new List<BlockClass>(blockList.Count);

        for (int i = 0; i < playerDataBase.GetBlockClass().Count; i++)
        {
            blockList.Add(playerDataBase.GetBlockClass()[i]);
        }

        blockList = blockList.OrderByDescending(x => x.blockType).OrderByDescending(x => x.level).OrderByDescending(x => x.rankType).ToList();

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
                content.Synthesis_Initialize(this);

                blockUIContentList.Add(content);
            }
        }

        for (int i = 0; i < blockUIContentList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(false);
            blockUIContentList[i].Reset_Initalize();
        }

        for (int i = 0; i < blockList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(true);
            blockUIContentList[i].Collection_Initialize(blockList[i]);
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

        isStart = false;
        isMat1 = false;
        isMat2 = false;
    }

    public void OpenSynthesisView(string id, Action action)
    {
        if (!isStart)
        {
            for (int i = 0; i < blockUIContentList.Count; i++)
            {
                blockUIContentList[i].SynthesisUnSelected();
            }

            action.Invoke();

            equipInfo = 0;

            nextBlockUIContent.Reset_Initalize();
            targetBlockUIContent.Reset_Initalize();
            matBlockUIContent1.Reset_Initalize();
            matBlockUIContent2.Reset_Initalize();

            blockClass = playerDataBase.GetBlockClass(id);

            equipInfo = playerDataBase.CheckEquipId(id);

            windCharacterType = blockDataBase.GetBlockInfomation(blockClass.blockType).windCharacterType;
            rankType = blockClass.rankType;

            upgradeValue = upgradeDataBase.GetUpgradeValue(blockClass.rankType);
            upgradeValue2 = upgradeDataBase.GetUpgradeValue(blockClass.rankType + 1);
            upgradeInformation = upgradeDataBase.GetUpgradeInformation(blockClass.level + 1);

            nextBlockUIContent.Collection_Initialize(blockClass);
            nextBlockUIContent.NextLevel_Initialize();
            targetBlockUIContent.Collection_Initialize(blockClass);

            plusObj.SetActive(true);
            matObj1.SetActive(true);
            matObj2.SetActive(true);
            goldObj.SetActive(true);

            needObj.SetActive(true);

            titleText.text = blockDataBase.GetBlockName(blockClass.blockType);
            upgradeLevelText.text = "최대 강화 레벨 : " + upgradeValue.maxLevel + " ▶ " + (upgradeValue.maxLevel + 5);
            valueText.text = "가치 " + upgradeValue.GetValueNumber(blockClass.level) + " ▶ " + upgradeValue2.GetValueNumber(blockClass.level);

            needGold = upgradeValue.GetSynthesisValue();

            goldText.text = needGold.ToString();

            switch (blockClass.rankType)
            {
                case RankType.N:
                    needText.text = "필수 재료 : 2x N등급 " + titleText.text;
                    break;
                case RankType.R:
                    needText.text = "필수 재료 : 2x R등급 " + titleText.text;
                    break;
                case RankType.SR:
                    needText.text = "필수 재료 : 2x SR등급 " + titleText.text;
                    break;
                case RankType.SSR:
                    matObj2.SetActive(false);
                    needText.text = "필수 재료 : 1x SSR등급 " + titleText.text;
                    break;
                case RankType.UR:
                    matObj2.SetActive(false);
                    break;
            }

            for (int i = 0; i < blockUIContentList.Count; i++)
            {
                if (!blockUIContentList[i].blockClass.blockType.Equals(blockClass.blockType) ||
                    !blockUIContentList[i].blockClass.rankType.Equals(blockClass.rankType))
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                }
            }

            isStart = true;
        }
        else //2번째 재료 선택
        {
            if (!isMat1 && matObj1.activeSelf)
            {
                blockClassMat1 = playerDataBase.GetBlockClass(id);

                playerDataBase.CheckEquipId(id);

                matBlockUIContent1.Collection_Initialize(blockClassMat1);

                isMat1 = true;

                action.Invoke();

                CheckSynthesisMaterial();

                Debug.Log("1번째 재료 선택됨");
            }
            else if (!isMat2 && matObj2.activeSelf)
            {
                blockClassMat2 = playerDataBase.GetBlockClass(id);

                playerDataBase.CheckEquipId(id);

                matBlockUIContent2.Collection_Initialize(blockClassMat2);

                isMat2 = true;

                action.Invoke();

                CheckSynthesisMaterial();

                Debug.Log("2번째 재료 선택됨");
            }
        }
    }

    void CheckSynthesisMaterial()
    {
        switch (blockClass.rankType)
        {
            case RankType.N:
                if (isMat1 && isMat2)
                {
                    CheckSynthesis();
                }
                break;
            case RankType.R:
                if (isMat1 && isMat2)
                {
                    CheckSynthesis();
                }
                break;
            case RankType.SR:
                if (isMat1 && isMat2)
                {
                    CheckSynthesis();
                }
                break;
            case RankType.SSR:
                if (isMat1)
                {
                    CheckSynthesis();
                }
                break;
            case RankType.UR:
                if (isMat1)
                {
                    CheckSynthesis();
                }
                break;
        }
    }

    void CheckSynthesis()
    {
        int level1, level2, level3 = 0;

        level1 = blockClass.level;
        level2 = blockClassMat1.level;

        switch (blockClass.rankType)
        {
            case RankType.N:
                level3 = blockClassMat2.level;
                break;
            case RankType.R:
                level3 = blockClassMat2.level;
                break;
            case RankType.SR:
                level3 = blockClassMat2.level;
                break;
            case RankType.SSR:
                break;
            case RankType.UR:
                break;
        }

        List<int> list = new List<int>();
        list.Add(level1);
        list.Add(level2);
        list.Add(level3);

        int max = list.Max();

        updateLevel = max;

        nextBlockUIContent.SetLevel(max);

        for(int i = 0; i < blockUIContentList.Count; i ++)
        {
            blockUIContentList[i].Lock(true);
        }

        synthesisButton.SetActive(true);

        Debug.Log("합성 준비 완료");
    }

    public void CancleSynthesis(string id)
    {
        if (id.Equals(blockClass.instanceId)) //합성 취소
        {
            titleText.text = "";
            upgradeLevelText.text = "합성을 원하는 블록을 선택하세요!";
            valueText.text = "";

            equipInfo = 0;

            nextBlockUIContent.Reset_Initalize();
            targetBlockUIContent.Reset_Initalize();
            matBlockUIContent1.Reset_Initalize();
            matBlockUIContent2.Reset_Initalize();

            plusObj.SetActive(false);
            matObj1.SetActive(false);
            matObj2.SetActive(false);
            goldObj.SetActive(false);
            needObj.SetActive(false);

            for (int i = 0; i < blockList.Count; i++)
            {
                blockUIContentList[i].gameObject.SetActive(true);
                blockUIContentList[i].SynthesisUnSelected();
                blockUIContentList[i].Lock(false);
            }

            isStart = false;
            isMat1 = false;
            isMat2 = false;
        }
        else //재료 취소
        {
            if(blockClassMat1.instanceId.Equals(id))
            {
                matBlockUIContent1.Reset_Initalize();

                isMat1 = false;

                Debug.Log("1번째 재료 취소");
            }

            if(blockClassMat2.instanceId.Equals(id))
            {
                matBlockUIContent2.Reset_Initalize();

                isMat2 = false;

                Debug.Log("2번째 재료 취소");
            }

            for (int i = 0; i < blockUIContentList.Count; i++)
            {
                blockUIContentList[i].Lock(false);
            }

            nextBlockUIContent.levelText.text = "";

            synthesisButton.SetActive(false);
        }
    }

    public void SynthesisButton()
    {
        synthesisResultButton.SetActive(false);

        if (synthesisButton.activeSelf)
        {
            if (playerDataBase.Gold < needGold)
            {
                NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                return;
            }

            PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, needGold);


            for (int i = 0; i < synthesisResultContentList.Count; i++)
            {
                synthesisResultContentList[i].gameObject.SetActive(false);
            }

            StartCoroutine(SynthesisCoroution());

        }
    }

    IEnumerator SynthesisCoroution()
    {
        synthesisResultView.SetActive(true);

        upgradeManager.SellBlock(blockClass.instanceId);
        yield return new WaitForSeconds(0.1f);

        upgradeManager.SellBlock(blockClassMat1.instanceId);
        yield return new WaitForSeconds(0.1f);

        switch (rankType)
        {
            case RankType.N:
                upgradeManager.SellBlock(blockClassMat2.instanceId);
                break;
            case RankType.R:
                upgradeManager.SellBlock(blockClassMat2.instanceId);
                break;
            case RankType.SR:
                upgradeManager.SellBlock(blockClassMat2.instanceId);
                break;
        }

        yield return new WaitForSeconds(0.1f);

        if (updateLevel > 0)
        {
            BlockClass block = new BlockClass();
            block.blockType = blockClass.blockType;
            block.rankType = rankType + 1;
            block.level = updateLevel;
            block.equipInfo = equipInfo;

            Debug.Log(updateLevel + 1 + " 레벨로 계승 될 예정입니다");

            if(equipInfo > 0)
            {
                Debug.Log(equipInfo + " 로 장착이 계승 될 예정입니다");
            }

            playerDataBase.SetSuccessionLevel(block);
        }

        synthesisResultList.Clear();
        synthesisResultList.Add(blockClass.blockType + "_" + (rankType + 1));

        Debug.LogError("합성 결과 : " + blockClass.blockType + "_" + (rankType + 1));

        switch (windCharacterType)
        {
            case WindCharacterType.Winter:
                PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", synthesisResultList);
                break;
            case WindCharacterType.UnderWorld:
                PlayfabManager.instance.GrantItemsToUser("Kingdom of The Underworld", synthesisResultList);
                break;
        }

        yield return new WaitForSeconds(2.5f);

        BlockClass block2 = new BlockClass();

        block2.blockType = blockClass.blockType;
        block2.rankType = rankType + 1;
        block2.level = updateLevel;

        for (int i = 0; i < synthesisResultList.Count; i++)
        {
            synthesisResultContentList[i].gameObject.SetActive(true);
            synthesisResultContentList[i].Collection_Initialize(block2);
        }

        synthesisResultButton.SetActive(true);

        Debug.Log("합성 성공!");

        collectionManager.UpdateCollection();
        Initialize();
    }

    public void ClosesSynthesisResultView()
    {
        synthesisResultView.SetActive(false);
    }

    public void SortButton()
    {
        blockList = blockList.OrderByDescending(x => x.blockType).OrderByDescending(x => x.level).OrderByDescending(x => x.rankType).ToList();

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
}
