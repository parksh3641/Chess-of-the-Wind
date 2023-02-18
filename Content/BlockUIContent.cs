using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockUIContent : MonoBehaviour
{
    public BlockType blockType = BlockType.Default;

    public Image backgroundImg;
    public Text levelText;
    private int level = 0;

    public BlockChildContent[] blockUIArray;

    ImageDataBase imageDataBase;

    void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

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

    public void Collection_Initialize(BlockClass blockClass)
    {
        blockType = blockClass.blockType;

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
        }

        level = blockClass.level;

        if(level > 0)
        {
            levelText.text = level.ToString();
        }
        else
        {
            levelText.text = "";
        }
    }

    public void SetRankBackground()
    {

    }
}
