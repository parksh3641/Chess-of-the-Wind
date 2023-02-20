using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockUIContent : MonoBehaviour
{
    public BlockType blockType = BlockType.Default;
    public RankType rankType = RankType.N;
    public string instanceId = "";

    public Image backgroundImg;
    public Text levelText;
    private int level = 0;

    public GameObject selectedObj;
    public GameObject lockedObj;

    public BlockChildContent[] blockUIArray;

    ImageDataBase imageDataBase;
    CollectionManager collectionManager;
    SynthesisManager synthesisManager;

    void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        levelText.text = "";

        selectedObj.SetActive(false);
        lockedObj.SetActive(false);
    }

    public void Reset_Initalize()
    {
        backgroundImg.color = Color.white;

        for (int i = 0; i < blockUIArray.Length; i++)
        {
            blockUIArray[i].gameObject.SetActive(false);
        }

        selectedObj.SetActive(false);
        lockedObj.SetActive(false);

        levelText.text = "";
    }

    public void Initialize(BlockType type)
    {
        blockType = type;

        for (int i = 0; i < blockUIArray.Length; i++)
        {
            blockUIArray[i].gameObject.SetActive(false);
        }

        blockUIArray[(int)blockType - 1].gameObject.SetActive(true);
    }

    public void Initialize(string name)
    {
        string block = name.Substring(0, name.Length - 2);
        string rank = name.Substring(name.Length - 1);

        blockType = (BlockType)Enum.Parse(typeof(BlockType), block);

        Initialize(blockType);

        switch (rank)
        {
            case "D":
                backgroundImg.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
                break;
            case "C":
                backgroundImg.color = Color.green;
                break;
            case "B":
                backgroundImg.color = Color.blue;
                break;
            case "A":
                backgroundImg.color = new Color(1, 0, 1);
                break;
            case "S":
                backgroundImg.color = Color.yellow;
                break;
            default:
                backgroundImg.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
                break;

        }
    }


    public void Collection_Initialize(BlockClass blockClass)
    {
        blockType = blockClass.blockType;
        rankType = blockClass.rankType;
        instanceId = blockClass.instanceId;

        Initialize(blockType);

        switch (blockClass.rankType)
        {
            case RankType.N:
                backgroundImg.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
                break;
            case RankType.R:
                backgroundImg.color = Color.green;
                break;
            case RankType.SR:
                backgroundImg.color = Color.blue;
                break;
            case RankType.SSR:
                backgroundImg.color = new Color(1, 0, 1);
                break;
            case RankType.UR:
                backgroundImg.color = Color.yellow;
                break;
            default:
                backgroundImg.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
                break;
        }

        SetLevel(blockClass.level);
    }

    public void NextLevel_Initialize()
    {
        switch (rankType)
        {
            case RankType.N:
                backgroundImg.color = Color.green;
                break;
            case RankType.R:
                backgroundImg.color = Color.blue;
                break;
            case RankType.SR:
                backgroundImg.color = new Color(1, 0, 1);
                break;
            case RankType.SSR:
                backgroundImg.color = Color.yellow;
                break;
            case RankType.UR:
                backgroundImg.color = Color.yellow;
                break;
        }

        levelText.text = "";
    }

    public void SetLevel(int number)
    {
        level = number;

        if (level > 0)
        {
            levelText.text = (level + 1).ToString();
        }
        else
        {
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
        }

        if(synthesisManager != null)
        {
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
