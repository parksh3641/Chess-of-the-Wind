using Firebase.Analytics;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SynthesisManager : MonoBehaviour
{
    public RankType synthesisRankType = RankType.N;

    public WindCharacterType windCharacterType = WindCharacterType.Winter;

    public GameObject synthesisView;
    public GameObject synthesisResultView;
    public GameObject syntheisResultView2;
    public GameObject synthesisResultButton;

    public BlockUIContent nextBlockUIContent;
    public BlockUIContent targetBlockUIContent;
    public BlockUIContent matBlockUIContent1;
    public BlockUIContent matBlockUIContent2;

    public LocalizationContent titleText;
    public LocalizationContent upgradeLevelText;
    public LocalizationContent valueText;

    public LocalizationContent sortText;

    public GameObject plusObj;
    public GameObject matObj1;
    public GameObject matObj2;

    public GameObject goldObj;
    public Text goldText;

    public GameObject needObj;
    public LocalizationContent needText;

    public GameObject sortButton;
    public GameObject synthesisButton;

    public Text synthesisResultText;
    public GameObject synthesisResultEffect;

    [Title("All Synthesis")]
    public GameObject synthesisAllButton;
    public GameObject synthesisWarning;

    public Text synthesisRank_N_Value;
    public Text synthesisRank_R_Value;
    public Text synthesisRank_SR_Value;

    [Title("Value")]
    private int needGold = 0;
    private int updateLevel = 0;
    private int equipInfo = 0;
    private int sortCount = 0;

    private int rankNCount = 0;
    private int rankNValue = 0;

    private int rankRCount = 0;
    private int rankRValue = 0;

    private int rankSRCount = 0;
    private int rankSRValue = 0;

    private bool isStart = false; //합성 시작 여부
    private bool isMat1 = false;
    private bool isMat2 = false;
    private bool isReady = false;

    public BlockUIContent blockUIContent;
    public Transform blockUITransform;
    public Transform synthesisTransform;

    [Space]
    public List<BlockClass> blockList = new List<BlockClass>();

    [Space]
    public List<BlockUIContent> blockUIContentList = new List<BlockUIContent>();
    public List<BlockUIContent> synthesisResultContentList = new List<BlockUIContent>();

    [Space]
    public List<BlockUIContent> synthesisList_Rank_N = new List<BlockUIContent>();
    public List<BlockUIContent> synthesisList_Rank_R = new List<BlockUIContent>();
    public List<BlockUIContent> synthesisList_Rank_SR = new List<BlockUIContent>();

    [Space]
    public int[] synthesisList_Rank_N_Count;
    public int[] synthesisList_Rank_R_Count;
    public int[] synthesisList_Rank_SR_Count;

    List<string> synthesisResultList = new List<string>();
    List<BlockType> synthesisListAllResult = new List<BlockType>();

    RankType rankType = RankType.N;
    BlockClass blockClass = new BlockClass();
    BlockClass blockClassMat1 = new BlockClass();
    BlockClass blockClassMat2 = new BlockClass();

    UpgradeValue upgradeValue;
    UpgradeValue upgradeValue2;
    UpgradeInformation upgradeInformation;

    public CollectionManager collectionManager;
    public UpgradeManager upgradeManager;
    public TitleManager titleManager;

    UpgradeDataBase upgradeDataBase;
    BlockDataBase blockDataBase;
    PlayerDataBase playerDataBase;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.2f);

    private void Awake()
    {
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
        if (blockDataBase == null) blockDataBase = Resources.Load("BlockDataBase") as BlockDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        synthesisView.SetActive(false);
        synthesisResultView.SetActive(false);
        syntheisResultView2.SetActive(false);
        synthesisResultEffect.SetActive(false);
        synthesisResultText.gameObject.SetActive(false);
        synthesisWarning.SetActive(false);

        plusObj.SetActive(false);
        matObj1.SetActive(false);
        matObj2.SetActive(false);
        goldObj.SetActive(false);
        needObj.SetActive(false);

        sortButton.SetActive(true);
        synthesisButton.SetActive(false);
        synthesisAllButton.SetActive(true);

        for (int i = 0; i < 50; i++)
        {
            BlockUIContent content = Instantiate(blockUIContent);
            content.transform.SetParent(blockUITransform);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            content.Synthesis_Initialize(this);

            blockUIContentList.Add(content);
        }

        for (int i = 0; i < 100; i++)
        {
            BlockUIContent content = Instantiate(blockUIContent);
            content.transform.SetParent(synthesisTransform);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);

            synthesisResultContentList.Add(content);
        }
    }

    private void Start()
    {
        sortCount = 0;
        sortText.localizationName = "ByType";
    }

    public void OpenSynthesisView()
    {
        if (!synthesisView.activeSelf)
        {
            synthesisView.SetActive(true);
            synthesisResultView.SetActive(false);
            syntheisResultView2.SetActive(false);
            synthesisResultEffect.SetActive(false);
            synthesisResultText.gameObject.SetActive(false);
            synthesisWarning.SetActive(false);

            Initialize();
        }
    }

    public void CloseSynthesisView()
    {
        synthesisView.SetActive(false);

        synthesisResultView.SetActive(false);
        syntheisResultView2.SetActive(false);
        synthesisResultEffect.SetActive(false);
        synthesisResultText.gameObject.SetActive(false);
    }

    void Initialize()
    {
        titleText.localizationName = "";
        upgradeLevelText.localizationName = "SynthesisInfo";
        upgradeLevelText.plusText = "";
        valueText.localizationName = "";

        titleText.ReLoad();
        upgradeLevelText.ReLoad();
        valueText.ReLoad();

        nextBlockUIContent.gameObject.SetActive(false);
        targetBlockUIContent.gameObject.SetActive(false);
        matBlockUIContent1.gameObject.SetActive(false);
        matBlockUIContent2.gameObject.SetActive(false);

        plusObj.SetActive(false);
        matObj1.SetActive(false);
        matObj2.SetActive(false);
        goldObj.SetActive(false);
        needObj.SetActive(false);

        sortButton.SetActive(true);
        synthesisButton.SetActive(false);
        synthesisAllButton.SetActive(true);

        blockList.Clear();
        blockList = new List<BlockClass>(blockList.Count);

        if(blockUIContentList.Count < blockList.Count)
        {
            for (int i = 0; i < blockList.Count - blockUIContentList.Count; i++)
            {
                BlockUIContent content = Instantiate(blockUIContent);
                content.transform.parent = blockUITransform;
                content.transform.localPosition = Vector3.zero;
                content.transform.localScale = Vector3.one;
                content.gameObject.SetActive(false);
                content.Synthesis_Initialize(this);

                blockUIContentList.Add(content);
            }

            for (int i = 0; i < (blockList.Count - blockUIContentList.Count) / 3; i++)
            {
                BlockUIContent content = Instantiate(blockUIContent);
                content.transform.parent = synthesisTransform;
                content.transform.localPosition = Vector3.zero;
                content.transform.localScale = Vector3.one;
                content.gameObject.SetActive(false);

                synthesisResultContentList.Add(content);
            }
        }

        for (int i = 0; i < playerDataBase.GetBlockClass().Count; i++)
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

            if (blockList[i].rankType == RankType.SSR || blockList[i].rankType == RankType.UR || playerDataBase.CheckEquip(blockList[i].instanceId) != 0)
            {
                blockUIContentList[i].gameObject.SetActive(false);
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

        isStart = false;
        isMat1 = false;
        isMat2 = false;

        titleManager.CheckGoal();

        Debug.Log("합성 초기화");
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

            nextBlockUIContent.gameObject.SetActive(false);
            targetBlockUIContent.gameObject.SetActive(false);
            matBlockUIContent1.gameObject.SetActive(false);
            matBlockUIContent2.gameObject.SetActive(false);

            blockClass = playerDataBase.GetBlockClass(id);

            //equipInfo = playerDataBase.CheckEquipId(id);

            windCharacterType = blockDataBase.GetBlockInfomation(blockClass.blockType).windCharacterType;
            rankType = blockClass.rankType;

            upgradeValue = upgradeDataBase.GetUpgradeValue(blockClass.rankType);
            upgradeValue2 = upgradeDataBase.GetUpgradeValue(blockClass.rankType + 1);
            upgradeInformation = upgradeDataBase.GetUpgradeInformation(blockClass.level + 1);

            nextBlockUIContent.gameObject.SetActive(true);
            nextBlockUIContent.Collection_Initialize(blockClass);
            nextBlockUIContent.NextLevel_Initialize();

            targetBlockUIContent.gameObject.SetActive(true);
            targetBlockUIContent.Collection_Initialize(blockClass);

            plusObj.SetActive(true);
            matObj1.SetActive(true);
            matObj2.SetActive(true);
            goldObj.SetActive(true);
            needObj.SetActive(true);

            sortButton.SetActive(false);
            synthesisAllButton.SetActive(false);

            titleText.localizationName = blockClass.blockType.ToString();
            titleText.ReLoad();

            upgradeLevelText.localizationName = "MaxUpgradeLevel";
            upgradeLevelText.plusText = " : " + upgradeValue.maxLevel + " ▶ <color=#FF6123>" + (upgradeValue.maxLevel + 5) + "</color>";
            upgradeLevelText.ReLoad();

            valueText.localizationName = "Value";
            valueText.plusText = " : " + upgradeValue.GetValueNumber(blockClass.level) + " ▶ <color=#FF6123>" + upgradeValue2.GetValueNumber(blockClass.level) + "</color>";
            valueText.ReLoad();

            needGold = upgradeValue.GetSynthesisValue();

            goldText.text = MoneyUnitString.ToCurrencyString(needGold);

            needText.localizationName = "Required";
            needText.localizationName2 = "Grade" + blockClass.rankType.ToString();
            needText.plusText = "  x2";
            needText.ReLoad();

            for (int i = 0; i < blockUIContentList.Count; i++)
            {
                if (!blockUIContentList[i].blockClass.blockType.Equals(blockClass.blockType) ||
                    !blockUIContentList[i].blockClass.rankType.Equals(blockClass.rankType) || 
                    playerDataBase.CheckEquip(blockUIContentList[i].instanceId) != 0)
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

                playerDataBase.CheckEquip(id);

                matBlockUIContent1.gameObject.SetActive(true);
                matBlockUIContent1.Collection_Initialize(blockClassMat1);

                isMat1 = true;

                action.Invoke();

                CheckSynthesisMaterial();

                synthesisAllButton.SetActive(false);

                Debug.Log("1번째 재료 선택됨");
            }
            else if (!isMat2 && matObj2.activeSelf)
            {
                blockClassMat2 = playerDataBase.GetBlockClass(id);

                playerDataBase.CheckEquip(id);

                matBlockUIContent2.gameObject.SetActive(true);
                matBlockUIContent2.Collection_Initialize(blockClassMat2);

                isMat2 = true;

                action.Invoke();

                CheckSynthesisMaterial();

                synthesisAllButton.SetActive(false);

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
                if (isMat1 && isMat2)
                {
                    CheckSynthesis();
                }
                break;
            case RankType.UR:
                if (isMat1 && isMat2)
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
        level3 = blockClassMat2.level;

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
            titleText.localizationName = "";
            upgradeLevelText.localizationName = "SynthesisInfo";
            upgradeLevelText.plusText = "";
            valueText.localizationName = "";

            titleText.ReLoad();
            upgradeLevelText.ReLoad();
            valueText.ReLoad();

            nextBlockUIContent.levelText.text = "";

            equipInfo = 0;

            nextBlockUIContent.gameObject.SetActive(false);
            targetBlockUIContent.gameObject.SetActive(false);
            matBlockUIContent1.gameObject.SetActive(false);
            matBlockUIContent2.gameObject.SetActive(false);

            plusObj.SetActive(false);
            matObj1.SetActive(false);
            matObj2.SetActive(false);
            goldObj.SetActive(false);
            needObj.SetActive(false);

            sortButton.SetActive(true);
            synthesisButton.SetActive(false);
            synthesisAllButton.SetActive(true);

            for (int i = 0; i < blockList.Count; i++)
            {
                blockUIContentList[i].gameObject.SetActive(true);
                blockUIContentList[i].SynthesisUnSelected();
                blockUIContentList[i].Lock(false);

                if (blockList[i].rankType == RankType.SSR || blockList[i].rankType == RankType.UR || 
                    playerDataBase.CheckEquip(blockList[i].instanceId) != 0)
                {
                    blockUIContentList[i].gameObject.SetActive(false);
                }
            }

            isStart = false;
            isMat1 = false;
            isMat2 = false;
        }
        else //재료 취소
        {
            if(blockClassMat1.instanceId.Equals(id))
            {
                matBlockUIContent1.gameObject.SetActive(false);

                isMat1 = false;

                Debug.Log("1번째 재료 취소");
            }

            if (blockClassMat2.instanceId.Equals(id))
            {
                matBlockUIContent2.gameObject.SetActive(false);

                isMat2 = false;

                Debug.Log("2번째 재료 취소");
            }

            for (int i = 0; i < blockUIContentList.Count; i++)
            {
                blockUIContentList[i].Lock(false);
            }

            synthesisButton.SetActive(false);
        }
    }

    public void SynthesisButton()
    {
        if (synthesisButton.activeSelf)
        {
            if (!NetworkConnect.instance.CheckConnectInternet())
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.CheckInternet);
                return;
            }

            if (playerDataBase.Gold < needGold)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                return;
            }

            PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, needGold);

            for (int i = 0; i < synthesisResultContentList.Count; i++)
            {
                synthesisResultContentList[i].gameObject.SetActive(false);
            }

            FirebaseAnalytics.LogEvent("Synthesis");

            StartCoroutine(SynthesisCoroution());

        }
    }

    IEnumerator SynthesisCoroution()
    {
        synthesisResultView.SetActive(true);

        for (int i = 0; i < synthesisResultContentList.Count; i++)
        {
            synthesisResultContentList[i].gameObject.SetActive(false);
        }

        synthesisResultButton.SetActive(false);

        synthesisResultText.gameObject.SetActive(true);
        StartCoroutine(TextCoroution());

        upgradeManager.SellBlock(blockClass.instanceId);
        yield return waitForSeconds;

        upgradeManager.SellBlock(blockClassMat1.instanceId);
        yield return waitForSeconds;

        upgradeManager.SellBlock(blockClassMat2.instanceId);
        yield return waitForSeconds;

        //switch (rankType)
        //{
        //    case RankType.N:
        //        upgradeManager.SellBlock(blockClassMat2.instanceId);
        //        break;
        //    case RankType.R:
        //        upgradeManager.SellBlock(blockClassMat2.instanceId);
        //        break;
        //    case RankType.SR:
        //        upgradeManager.SellBlock(blockClassMat2.instanceId);
        //        break;
        //}

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
                Debug.Log(equipInfo + " 위치로 장착이 계승 될 예정입니다");
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
                PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", synthesisResultList);
                break;
        }

        yield return new WaitForSeconds(2.5f);

        syntheisResultView2.SetActive(true);

        synthesisResultText.gameObject.SetActive(false);
        synthesisResultEffect.SetActive(true);

        BlockClass block2 = new BlockClass();

        block2.blockType = blockClass.blockType;
        block2.rankType = rankType + 1;
        block2.level = updateLevel;

        for (int i = 0; i < synthesisResultList.Count; i++)
        {
            synthesisResultContentList[i].gameObject.SetActive(true);
            synthesisResultContentList[i].Collection_Initialize(block2);
        }

        collectionManager.UpdateCollection();
        Initialize();

        synthesisResultButton.SetActive(true);

        SoundManager.instance.PlaySFX(GameSfxType.BlockSynthesisSuccess);

        Debug.Log("합성 성공!");
    }

    public void ClosesSynthesisResultView()
    {
        synthesisResultView.SetActive(false);
        syntheisResultView2.SetActive(false);
        synthesisResultEffect.SetActive(false);
        synthesisResultText.gameObject.SetActive(false);
    }

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
            blockUIContentList[i].Collection_Initialize(blockList[i]);

            if (blockList[i].rankType == RankType.SSR || blockList[i].rankType == RankType.UR ||
    playerDataBase.CheckEquip(blockList[i].instanceId) != 0)
            {
                blockUIContentList[i].gameObject.SetActive(false);
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
    }

    IEnumerator TextCoroution()
    {
        if (synthesisResultText.gameObject.activeInHierarchy)
        {
            synthesisResultText.text = LocalizationManager.instance.GetString("Synthesizing");
            yield return waitForSeconds2;
            synthesisResultText.text = LocalizationManager.instance.GetString("Synthesizing") + ".";
            yield return waitForSeconds2;
            synthesisResultText.text = LocalizationManager.instance.GetString("Synthesizing") + "..";
            yield return waitForSeconds2;
            synthesisResultText.text = LocalizationManager.instance.GetString("Synthesizing") + "...";
            yield return waitForSeconds2;
            StartCoroutine(TextCoroution());
        }
        else
        {
            yield break;
        }
    }

    public void OpenSynthesisWarning()
    {
        synthesisWarning.SetActive(true);

        synthesisList_Rank_N.Clear();
        synthesisList_Rank_R.Clear();
        synthesisList_Rank_SR.Clear();

        synthesisList_Rank_N_Count = new int[(System.Enum.GetValues(typeof(BlockType)).Length - 1)];
        synthesisList_Rank_R_Count = new int[(System.Enum.GetValues(typeof(BlockType)).Length - 1)];
        synthesisList_Rank_SR_Count = new int[(System.Enum.GetValues(typeof(BlockType)).Length - 1)];

        rankNCount = 0;
        rankNValue = 0;

        rankRCount = 0;
        rankRValue = 0;

        rankSRCount = 0;
        rankSRValue = 0;

        synthesisRank_N_Value.text = "0";
        synthesisRank_R_Value.text = "0";
        synthesisRank_SR_Value.text = "0";

        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].level == 0 && playerDataBase.CheckEquip(blockList[i].instanceId) == 0 && 
                !playerDataBase.CheckSellBlock(blockList[i].instanceId))
            {
                if (blockList[i].rankType == RankType.N)
                {
                    synthesisList_Rank_N.Add(blockUIContentList[i]);
                }
                else if (blockList[i].rankType == RankType.R)
                {
                    synthesisList_Rank_R.Add(blockUIContentList[i]);
                }
                else if (blockList[i].rankType == RankType.SR)
                {
                    synthesisList_Rank_SR.Add(blockUIContentList[i]);
                }
            }
        }

        Debug.Log("N등급 개수 : " + synthesisList_Rank_N.Count);
        Debug.Log("R등급 개수 : " + synthesisList_Rank_R.Count);
        Debug.Log("SR등급 개수 : " + synthesisList_Rank_SR.Count);

        for (int i = 0; i < synthesisList_Rank_N.Count; i++)
        {
            for (int j = 0; j < System.Enum.GetValues(typeof(BlockType)).Length - 1; j++)
            {
                if (synthesisList_Rank_N[i].blockClass.blockType.Equals(BlockType.Default + 1 + j))
                {
                    synthesisList_Rank_N_Count[j] += 1;
                }
            }
        }

        for (int i = 0; i < synthesisList_Rank_N_Count.Length; i++)
        {
            rankNCount += synthesisList_Rank_N_Count[i] / 3;
        }

        for (int i = 0; i < synthesisList_Rank_R.Count; i ++)
        {
            for(int j = 0; j < System.Enum.GetValues(typeof(BlockType)).Length - 1; j ++)
            {
                if(synthesisList_Rank_R[i].blockClass.blockType.Equals(BlockType.Default + 1 + j))
                {
                    synthesisList_Rank_R_Count[j] += 1;
                }
            }
        }

        for (int i = 0; i < synthesisList_Rank_R_Count.Length; i++)
        {
            rankRCount += synthesisList_Rank_R_Count[i] / 3;
        }

        for (int i = 0; i < synthesisList_Rank_SR.Count; i++)
        {
            for (int j = 0; j < System.Enum.GetValues(typeof(BlockType)).Length - 1; j++)
            {
                if (synthesisList_Rank_SR[i].blockClass.blockType.Equals(BlockType.Default + 1 + j))
                {
                    synthesisList_Rank_SR_Count[j] += 1;
                }
            }
        }

        for (int i = 0; i < synthesisList_Rank_SR_Count.Length; i++)
        {
            rankSRCount += synthesisList_Rank_SR_Count[i] / 3;
        }

        rankNValue = rankNCount * upgradeDataBase.GetSynthesisValue(RankType.N);
        rankRValue = rankRCount * upgradeDataBase.GetSynthesisValue(RankType.R);
        rankSRValue = rankSRCount * upgradeDataBase.GetSynthesisValue(RankType.SR);


        synthesisRank_N_Value.text = rankNValue.ToString();
        synthesisRank_R_Value.text = rankRValue.ToString();
        synthesisRank_SR_Value.text = rankSRValue.ToString();

        Debug.Log("일괄 합성 준비 완료");
    }

    public void CloseSynthesisWarning()
    {
        synthesisWarning.SetActive(false);
    }

    public void SynthesisAllButton_N()
    {
        if(rankNValue == 0)
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotSynthesisBlock);
            return;
        }
        else
        {
            if(playerDataBase.Gold < rankNValue)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                return;
            }
            else
            {
                PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, rankNValue);
            }
        }

        Debug.Log("N등급 일괄 합성 시작");

        synthesisRankType = RankType.N;

        FirebaseAnalytics.LogEvent("SynthesisAll_N");

        StartCoroutine(SynthesisAllCoroution());
    }

    public void SynthesisAllButton_R()
    {
        if (rankRValue == 0)
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotSynthesisBlock);
            return;
        }
        else
        {
            if (playerDataBase.Gold < rankRValue)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                return;
            }
            else
            {
                PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, rankRValue);
            }
        }

        Debug.Log("R등급 일괄 합성 시작");

        synthesisRankType = RankType.R;

        FirebaseAnalytics.LogEvent("SynthesisAll_R");

        StartCoroutine(SynthesisAllCoroution());
    }

    public void SynthesisAllButton_SR()
    {
        if (rankSRValue == 0)
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotSynthesisBlock);
            return;
        }
        else
        {
            if (playerDataBase.Gold < rankSRValue)
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
                return;
            }
            else
            {
                PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, rankSRValue);
            }
        }

        Debug.Log("SR등급 일괄 합성 시작");

        synthesisRankType = RankType.SR;

        FirebaseAnalytics.LogEvent("SynthesisAll_SR");

        StartCoroutine(SynthesisAllCoroution());
    }

    IEnumerator SynthesisAllCoroution()
    {
        synthesisWarning.SetActive(false);

        synthesisResultView.SetActive(true);

        SoundManager.instance.PlayLoopSFX(GameSfxType.BlockUpgradeReady);

        for (int i = 0; i < synthesisResultContentList.Count; i++)
        {
            synthesisResultContentList[i].gameObject.SetActive(false);
        }

        synthesisResultButton.SetActive(false);

        synthesisResultText.gameObject.SetActive(true);
        StartCoroutine(TextCoroution());

        synthesisResultList.Clear();

        synthesisListAllResult.Clear();

        switch (synthesisRankType)
        {
            case RankType.N:
                for (int i = 0; i < synthesisList_Rank_N_Count.Length; i ++)
                {
                    int number = synthesisList_Rank_N_Count[i] - (synthesisList_Rank_N_Count[i] % 3);

                    Debug.LogError((BlockType.Default + 1 + i) + " 등급을 " + number + " 개 삭제합니다");

                    if (number == 0) continue;

                    for(int j = 0; j < synthesisList_Rank_N.Count; j ++)
                    {
                        if(number > 0)
                        {
                            if (synthesisList_Rank_N[j].blockClass.blockType.Equals(BlockType.Default + 1 + i))
                            {
                                upgradeManager.SellBlock(synthesisList_Rank_N[j].blockClass.instanceId);
                                number--;

                                yield return waitForSeconds2;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }


                for (int i = 0; i < synthesisList_Rank_N_Count.Length; i ++)
                {
                    for (int j = 0; j < synthesisList_Rank_N_Count[i] / 3; j++)
                    {
                        synthesisListAllResult.Add((BlockType.Default + 1 + i));

                        synthesisResultList.Clear();
                        synthesisResultList.Add((BlockType.Default + 1 + i) + "_" + (synthesisRankType + 1));

                        switch (GameStateManager.instance.WindCharacterType)
                        {
                            case WindCharacterType.Winter:
                                PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", synthesisResultList);
                                break;
                            case WindCharacterType.UnderWorld:
                                PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", synthesisResultList);
                                break;
                        }

                        Debug.Log("일괄 합성 : " + (BlockType.Default + 1 + i) + "_" + (synthesisRankType + 1));

                        yield return waitForSeconds2;
                    }
                }

                break;
            case RankType.R:
                for (int i = 0; i < synthesisList_Rank_R_Count.Length; i++)
                {
                    int number = synthesisList_Rank_R_Count[i] - (synthesisList_Rank_R_Count[i] % 3);

                    Debug.LogError((BlockType.Default + 1 + i) + " 등급을 " + number + " 개 삭제합니다");

                    if (number == 0) continue;

                    for (int j = 0; j < synthesisList_Rank_R.Count; j++)
                    {
                        if (number > 0)
                        {
                            if (synthesisList_Rank_R[j].blockClass.blockType.Equals(BlockType.Default + 1 + i))
                            {
                                upgradeManager.SellBlock(synthesisList_Rank_R[j].blockClass.instanceId);
                                number--;

                                yield return waitForSeconds2;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                for (int i = 0; i < synthesisList_Rank_R_Count.Length; i++)
                {
                    for (int j = 0; j < synthesisList_Rank_R_Count[i] / 3; j++)
                    {
                        synthesisListAllResult.Add((BlockType.Default + 1 + i));

                        synthesisResultList.Clear();
                        synthesisResultList.Add((BlockType.Default + 1 + i) + "_" + (synthesisRankType + 1));

                        switch (GameStateManager.instance.WindCharacterType)
                        {
                            case WindCharacterType.Winter:
                                PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", synthesisResultList);
                                break;
                            case WindCharacterType.UnderWorld:
                                PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", synthesisResultList);
                                break;
                        }

                        Debug.Log("일괄 합성 : " + (BlockType.Default + 1 + i) + "_" + (synthesisRankType + 1));

                        yield return waitForSeconds2;
                    }
                }

                break;
            case RankType.SR:
                for (int i = 0; i < synthesisList_Rank_SR_Count.Length; i++)
                {
                    int number = synthesisList_Rank_SR_Count[i] - (synthesisList_Rank_SR_Count[i] % 3);

                    Debug.LogError((BlockType.Default + 1 + i) + " 등급을 " + number + " 개 삭제합니다");

                    if (number == 0) continue;

                    for (int j = 0; j < synthesisList_Rank_SR.Count; j++)
                    {
                        if (number > 0)
                        {
                            if (synthesisList_Rank_SR[j].blockClass.blockType.Equals(BlockType.Default + 1 + i))
                            {
                                upgradeManager.SellBlock(synthesisList_Rank_SR[j].blockClass.instanceId);
                                number--;

                                yield return waitForSeconds2;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                for (int i = 0; i < synthesisList_Rank_SR_Count.Length; i++)
                {
                    for (int j = 0; j < synthesisList_Rank_SR_Count[i] / 3; j++)
                    {
                        synthesisListAllResult.Add((BlockType.Default + 1 + i));

                        synthesisResultList.Clear();
                        synthesisResultList.Add((BlockType.Default + 1 + i) + "_" + (synthesisRankType + 1));

                        switch (GameStateManager.instance.WindCharacterType)
                        {
                            case WindCharacterType.Winter:
                                PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", synthesisResultList);
                                break;
                            case WindCharacterType.UnderWorld:
                                PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", synthesisResultList);
                                break;
                        }

                        Debug.Log("일괄 합성 : " + (BlockType.Default + 1 + i) + "_" + (synthesisRankType + 1));

                        yield return waitForSeconds2;
                    }
                }

                break;
            case RankType.SSR:
                break;
            case RankType.UR:
                break;
        }

        yield return new WaitForSeconds(2.5f);

        SoundManager.instance.StopLoopSFX(GameSfxType.BlockUpgradeReady);

        syntheisResultView2.SetActive(true);

        synthesisResultText.gameObject.SetActive(false);
        synthesisResultEffect.SetActive(true);

        for (int i = 0; i < synthesisListAllResult.Count; i ++)
        {
            BlockClass block = new BlockClass();

            block.blockType = synthesisListAllResult[i];
            block.rankType = synthesisRankType + 1;
            block.level = 0;

            synthesisResultContentList[i].Collection_Initialize(block);
        }

        for (int i = 0; i < synthesisListAllResult.Count; i++)
        {
            synthesisResultContentList[i].gameObject.SetActive(true);
            SoundManager.instance.PlaySFX(GameSfxType.GetBlock);
            yield return waitForSeconds;
        }

        collectionManager.UpdateCollection();
        Initialize();

        synthesisResultButton.SetActive(true);

        Debug.Log("일괄 합성 성공!");
    }
}
