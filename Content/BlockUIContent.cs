using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockUIContent : MonoBehaviour
{
    public BlockClass blockClass;
    public string instanceId = "";

    public Image backgroundImg;

    public Image rankImg;
    public Text rankText;

    public Image rankSSRImg;
    public Text rankSSRText;

    public Text levelText;
    private int level = 0;

    private bool boxInfo = false;

    public GameObject gradient;

    public GameObject alarm;
    public GameObject selectedObj;
    public GameObject lockedObj;

    public GameObject[] blockUIArray;

    Sprite[] rankBackgroundArray;
    Sprite[] rankBannerArray;

    ImageDataBase imageDataBase;
    CollectionManager collectionManager;
    SynthesisManager synthesisManager;

    public int Value { get; internal set; }

    void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rankBackgroundArray = imageDataBase.GetRankBackgroundArray();
        rankBannerArray = imageDataBase.GetRankBannerArray();

        levelText.text = "";

        gradient.SetActive(false);

        alarm.SetActive(false);
        selectedObj.SetActive(false);
        lockedObj.SetActive(false);

        rankSSRImg.gameObject.SetActive(false);
    }

    public void Reset_Initalize()
    {
        for (int i = 0; i < blockUIArray.Length; i++)
        {
            blockUIArray[i].SetActive(false);
        }

        selectedObj.SetActive(false);
        lockedObj.SetActive(false);
    }

    public void Initialize(BlockType type)
    {
        blockClass.blockType = type;

        for (int i = 0; i < blockUIArray.Length; i++)
        {
            blockUIArray[i].SetActive(false);
        }

        blockUIArray[(int)blockClass.blockType - 1].SetActive(true);
    }

    public void Collection_Initialize(BlockClass block)
    {
        blockClass = block;
        instanceId = blockClass.instanceId;

        Initialize(blockClass.blockType);

        backgroundImg.sprite = rankBackgroundArray[(int)blockClass.rankType];
        rankImg.sprite = rankBannerArray[(int)blockClass.rankType];
        rankSSRImg.sprite = rankBannerArray[(int)blockClass.rankType];
        rankText.text = blockClass.rankType.ToString();

        if(block.rankType > RankType.SR)
        {
            gradient.SetActive(true);
        }
        else
        {
            gradient.SetActive(false);
        }

        SetLevel(blockClass.level);

        if(block.ssrLevel > 0)
        {
            rankSSRImg.gameObject.SetActive(true);
            rankSSRText.text = block.ssrLevel.ToString();
        }
        else
        {
            rankSSRImg.gameObject.SetActive(false);
        }
    }

    public void SetRank(RankType type)
    {
        boxInfo = true;

        blockClass.rankType = type;

        backgroundImg.sprite = rankBackgroundArray[(int)type];
        rankImg.sprite = rankBannerArray[(int)type];
        rankSSRImg.sprite = rankBannerArray[(int)type];
        rankText.text = type.ToString();

        if (type > RankType.SR)
        {
            gradient.SetActive(true);
        }
        else
        {
            gradient.SetActive(false);
        }
    }

    public void NextLevel_Initialize()
    {
        if(blockClass.rankType == RankType.SSR && blockClass.ssrLevel < 4)
        {
            backgroundImg.sprite = rankBackgroundArray[(int)blockClass.rankType];
            rankImg.sprite = rankBannerArray[(int)blockClass.rankType];
            rankSSRImg.sprite = rankBannerArray[(int)blockClass.rankType];
            rankText.text = (blockClass.rankType).ToString();
        }
        else
        {
            backgroundImg.sprite = rankBackgroundArray[(int)blockClass.rankType + 1];
            rankImg.sprite = rankBannerArray[(int)blockClass.rankType + 1];
            rankSSRImg.sprite = rankBannerArray[(int)blockClass.rankType + 1];
            rankText.text = (blockClass.rankType + 1).ToString();
        }

        if (blockClass.rankType + 1 > RankType.SR)
        {
            gradient.SetActive(true);
        }
        else
        {
            gradient.SetActive(false);
        }

        //levelText.text = "";
    }

    public void SetLevel(int number)
    {
        level = number;

        levelText.text = "Lv." + (level + 1).ToString();
    }

    public void SetPieceLevel(int number)
    {
        rankSSRImg.gameObject.SetActive(true);
        rankSSRText.text = number.ToString();
    }

    public void Upgrade_Initialize(CollectionManager manager)
    {
        collectionManager = manager;
    }

    public void BlockInfomationButton()
    {
        if(boxInfo)
        {
            ReceiveInfoManager.instance.OpenBlockInfo(blockClass);

            return;
        }

        if(collectionManager != null)
        {
            collectionManager.OpenBlockInformation(instanceId);
        }

        if(synthesisManager != null)
        {
            if(blockClass.rankType != RankType.UR)
                synthesisManager.OpenSynthesisView(instanceId, SynthesisSelected);
        }
    }

    #region Synthesis
    public void SynthesisSelected()
    {
        selectedObj.SetActive(true);
    }

    public void SynthesisUnSelected()
    {
        selectedObj.SetActive(false);
    }

    public void CancleSynthesisButton()
    {
        if (synthesisManager != null)
        {
            synthesisManager.CancleSynthesis(instanceId);

            selectedObj.SetActive(false);
        }
    }

    public void Synthesis_Initialize(SynthesisManager manager)
    {
        synthesisManager = manager;
    }

    public void Lock(bool check)
    {
        if (selectedObj.activeSelf) return;

        lockedObj.SetActive(check);
    }

    #endregion
}
