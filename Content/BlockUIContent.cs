using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockUIContent : MonoBehaviour
{
    public BlockClass blockClass;
    public string instanceId = "";

    public ShopUIType shopUIType = ShopUIType.Default;

    public Image backgroundImg;

    public Image rankImg;
    public Text rankText;

    public Image rankSSRImg;
    public Text rankSSRText;

    public Text levelText;
    private int level = 0;

    public Text valueText;

    private bool boxInfo = false;

    public GameObject gradient;

    public GameObject alarm;
    public GameObject selectedObj;
    public GameObject lockedObj;

    public GameObject[] blockUIArray;
    public GameObject[] shopUIArray;

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
        valueText.text = "";

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

        for (int i = 0; i < shopUIArray.Length; i++)
        {
            shopUIArray[i].SetActive(false);
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

        for (int i = 0; i < shopUIArray.Length; i++)
        {
            shopUIArray[i].SetActive(false);
        }

        if (shopUIType == ShopUIType.Default)
        {
            blockUIArray[(int)blockClass.blockType - 1].SetActive(true);
        }
        else
        {
            shopUIArray[(int)shopUIType - 1].SetActive(true);
        }
    }

    public void Initialize_RandomBox(RandomBox_Block randomBox_Block)
    {
        shopUIType = ShopUIType.Default;

        valueText.text = "";

        switch (randomBox_Block.boxInfoType)
        {
            case BoxInfoType.RightQueen_2_N:
                blockClass.blockType = BlockType.RightQueen_2;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.RightQueen_3_N:
                blockClass.blockType = BlockType.RightQueen_3;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.RightNight_N:
                blockClass.blockType = BlockType.RightNight;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.RightNight_Mirror_N:
                blockClass.blockType = BlockType.RightNight_Mirror;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.Rook_V2_N:
                blockClass.blockType = BlockType.Rook_V2;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.Rook_V2_2_N:
                blockClass.blockType = BlockType.Rook_V2_2;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.Pawn_Under_N:
                blockClass.blockType = BlockType.Pawn_Under;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.Pawn_Under_2_N:
                blockClass.blockType = BlockType.Pawn_Under_2;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.RightQueen_2_R:
                blockClass.blockType = BlockType.RightQueen_2;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.RightQueen_3_R:
                blockClass.blockType = BlockType.RightQueen_3;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.RightNight_R:
                blockClass.blockType = BlockType.RightNight;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.RightNight_Mirror_R:
                blockClass.blockType = BlockType.RightNight_Mirror;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.Rook_V2_R:
                blockClass.blockType = BlockType.Rook_V2;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.Rook_V2_2_R:
                blockClass.blockType = BlockType.Rook_V2_2;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.Pawn_Under_R:
                blockClass.blockType = BlockType.Pawn_Under;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.Pawn_Under_2_R:
                blockClass.blockType = BlockType.Pawn_Under_2;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.RightQueen_2_SR:
                blockClass.blockType = BlockType.RightQueen_2;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.RightQueen_3_SR:
                blockClass.blockType = BlockType.RightQueen_3;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.RightNight_SR:
                blockClass.blockType = BlockType.RightNight;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.RightNight_Mirror_SR:
                blockClass.blockType = BlockType.RightNight_Mirror;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.Rook_V2_SR:
                blockClass.blockType = BlockType.Rook_V2;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.Rook_V2_2_SR:
                blockClass.blockType = BlockType.Rook_V2_2;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.Pawn_Under_SR:
                blockClass.blockType = BlockType.Pawn_Under;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.Pawn_Under_2_SR:
                blockClass.blockType = BlockType.Pawn_Under_2;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.RightQueen_2_SSR:
                blockClass.blockType = BlockType.RightQueen_2;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.RightQueen_3_SSR:
                blockClass.blockType = BlockType.RightQueen_3;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.RightNight_SSR:
                blockClass.blockType = BlockType.RightNight;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.RightNight_Mirror_SSR:
                blockClass.blockType = BlockType.RightNight_Mirror;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.Rook_V2_SSR:
                blockClass.blockType = BlockType.Rook_V2;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.Rook_V2_2_SSR:
                blockClass.blockType = BlockType.Rook_V2_2;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.Pawn_Under_SSR:
                blockClass.blockType = BlockType.Pawn_Under;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.Pawn_Under_2_SSR:
                blockClass.blockType = BlockType.Pawn_Under_2;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.LeftQueen_2_N:
                blockClass.blockType = BlockType.LeftQueen_2;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.LeftQueen_3_N:
                blockClass.blockType = BlockType.LeftQueen_3;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.LeftNight_N:
                blockClass.blockType = BlockType.LeftNight;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.LeftNight_Mirror_N:
                blockClass.blockType = BlockType.LeftNight_Mirror;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.Rook_V4_N:
                blockClass.blockType = BlockType.Rook_V4;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.Rook_V4_2_N:
                blockClass.blockType = BlockType.Rook_V4_2;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.Pawn_Snow_N:
                blockClass.blockType = BlockType.Pawn_Snow;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.Pawn_Snow_2_N:
                blockClass.blockType = BlockType.Pawn_Snow_2;
                blockClass.rankType = RankType.N;
                break;
            case BoxInfoType.LeftQueen_2_R:
                blockClass.blockType = BlockType.LeftQueen_2;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.LeftQueen_3_R:
                blockClass.blockType = BlockType.LeftQueen_3;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.LeftNight_R:
                blockClass.blockType = BlockType.LeftNight;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.LeftNight_Mirror_R:
                blockClass.blockType = BlockType.LeftNight_Mirror;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.Rook_V4_R:
                blockClass.blockType = BlockType.Rook_V4;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.Rook_V4_2_R:
                blockClass.blockType = BlockType.Rook_V4_2;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.Pawn_Snow_R:
                blockClass.blockType = BlockType.Pawn_Snow;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.Pawn_Snow_2_R:
                blockClass.blockType = BlockType.Pawn_Snow_2;
                blockClass.rankType = RankType.R;
                break;
            case BoxInfoType.LeftQueen_2_SR:
                blockClass.blockType = BlockType.LeftQueen_2;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.LeftQueen_3_SR:
                blockClass.blockType = BlockType.LeftQueen_3;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.LeftNight_SR:
                blockClass.blockType = BlockType.LeftNight;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.LeftNight_Mirror_SR:
                blockClass.blockType = BlockType.LeftNight_Mirror;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.Rook_V4_SR:
                blockClass.blockType = BlockType.Rook_V4;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.Rook_V4_2_SR:
                blockClass.blockType = BlockType.Rook_V4_2;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.Pawn_Snow_SR:
                blockClass.blockType = BlockType.Pawn_Snow;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.Pawn_Snow_2_SR:
                blockClass.blockType = BlockType.Pawn_Snow_2;
                blockClass.rankType = RankType.SR;
                break;
            case BoxInfoType.LeftQueen_2_SSR:
                blockClass.blockType = BlockType.LeftQueen_2;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.LeftQueen_3_SSR:
                blockClass.blockType = BlockType.LeftQueen_3;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.LeftNight_SSR:
                blockClass.blockType = BlockType.LeftNight;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.LeftNight_Mirror_SSR:
                blockClass.blockType = BlockType.LeftNight_Mirror;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.Rook_V4_SSR:
                blockClass.blockType = BlockType.Rook_V4;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.Rook_V4_2_SSR:
                blockClass.blockType = BlockType.Rook_V4_2;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.Pawn_Snow_SSR:
                blockClass.blockType = BlockType.Pawn_Snow;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.Pawn_Snow_2_SSR:
                blockClass.blockType = BlockType.Pawn_Snow_2;
                blockClass.rankType = RankType.SSR;
                break;
            case BoxInfoType.Gold_N:
                shopUIType = ShopUIType.Gold1;
                blockClass.rankType = RankType.N;
                valueText.text = randomBox_Block.value.ToString();
                break;
            case BoxInfoType.Gold_R:
                shopUIType = ShopUIType.Gold2;
                blockClass.rankType = RankType.R;
                valueText.text = randomBox_Block.value.ToString();
                break;
            case BoxInfoType.UpgradeTicket_N:
                shopUIType = ShopUIType.UpgradeTicket1;
                blockClass.rankType = RankType.N;
                valueText.text = randomBox_Block.value.ToString();
                break;
            case BoxInfoType.UpgradeTicket_R:
                shopUIType = ShopUIType.UpgradeTicket2;
                blockClass.rankType = RankType.R;
                valueText.text = randomBox_Block.value.ToString();
                break;
        }

        Initialize_UI(blockClass);

        SetPieceLevel(randomBox_Block.number);
    }


    public void Initialize_UI(BlockClass block)
    {
        boxInfo = true;

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

        if (block.ssrLevel > 0)
        {
            rankSSRImg.gameObject.SetActive(true);
            rankSSRText.text = block.ssrLevel.ToString();
        }
        else
        {
            rankSSRImg.gameObject.SetActive(false);
        }
    }

    public void Initialize_Rank(RankType type)
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
        if (shopUIType == ShopUIType.Default)
        {
            rankSSRImg.gameObject.SetActive(true);
            rankSSRText.text = (number + 1).ToString();
            levelText.text = "";
        }
        else
        {
            rankSSRImg.gameObject.SetActive(false);
            levelText.text = "";
        }
    }

    public void Upgrade_Initialize(CollectionManager manager)
    {
        collectionManager = manager;
    }

    public void BlockInfomationButton()
    {
        if(collectionManager != null)
        {
            collectionManager.OpenBlockInformation(instanceId);

            return;
        }

        if(synthesisManager != null)
        {
            if(blockClass.rankType != RankType.UR)
            {
                synthesisManager.OpenSynthesisView(instanceId, SynthesisSelected);
            }

            return;
        }

        if (boxInfo)
        {
            if (shopUIType == ShopUIType.Default)
            {
                ReceiveInfoManager.instance.OpenBlockInfo(blockClass);
            }

            return;
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
