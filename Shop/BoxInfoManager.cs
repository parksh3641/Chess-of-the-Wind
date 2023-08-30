using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxInfoManager : MonoBehaviour
{
    RankType rankType = RankType.N;

    public GameObject boxInfoView;

    public Text titleText;

    public BoxInfoContent boxInfoContent;

    public RectTransform boxInfoContentTransform;

    public List<BoxInfoContent> boxInfoContentList = new List<BoxInfoContent>();

    public BlockUIContent blockUIContent;

    public RectTransform blockUITransform;

    public List<BlockUIContent> blockUIContentList = new List<BlockUIContent>();


    public float[] percentBlock = new float[4];

    private int index = 0;

    private int average = 0;

    private void Awake()
    {
        boxInfoView.SetActive(false);

        for (int i = 0; i < 200; i++)
        {
            BoxInfoContent monster = Instantiate(boxInfoContent);
            monster.transform.SetParent(boxInfoContentTransform);
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            monster.gameObject.SetActive(false);

            boxInfoContentList.Add(monster);
        }

        for (int i = 0; i < 20; i++)
        {
            BlockUIContent content = Instantiate(blockUIContent);
            content.transform.SetParent(blockUITransform);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);

            blockUIContentList.Add(content);
        }
    }

    public void OpenBoxInfo(int number)
    {
        boxInfoView.SetActive(true);

        for (int i = 0; i < boxInfoContentList.Count; i++)
        {
            boxInfoContentList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < blockUIContentList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(false);
        }

        average = 0;

        switch (number)
        {
            case 0:
                rankType = RankType.SSR;

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        titleText.text = LocalizationManager.instance.GetString("Box_Winter");
                        break;
                    case WindCharacterType.UnderWorld:
                        titleText.text = LocalizationManager.instance.GetString("Box_Under");
                        break;
                }

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData("RandomBox", InitializePercent);
                break;
            case 1:
                rankType = RankType.R;

                titleText.text = LocalizationManager.instance.GetString("Box_Normal");

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData("NRBox", InitializePercent);
                break;
            case 2:
                rankType = RankType.SR;

                titleText.text = LocalizationManager.instance.GetString("Box_Epic");

                average = 1;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData("RSRBox", InitializePercent);
                break;
            case 3:
                rankType = RankType.SSR;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData("SRSSRBox", InitializePercent);
                break;
        }
    }

    public void CloseBoxInfoView()
    {
        boxInfoView.SetActive(false);
    }

    void InitializePercent(string check)
    {
        string[] temp = check.Split(",");

        percentBlock[0] = float.Parse(temp[0]);
        percentBlock[1] = float.Parse(temp[1]);
        percentBlock[2] = float.Parse(temp[2]);
        percentBlock[3] = float.Parse(temp[3]);

        switch (GameStateManager.instance.WindCharacterType)
        {
            case WindCharacterType.Winter:
                PlayfabManager.instance.GetTitleInternalData("AllowSnowBlock", InitializeBlock);
                break;
            case WindCharacterType.UnderWorld:
                PlayfabManager.instance.GetTitleInternalData("AllowUnderworldBlock", InitializeBlock);
                break;
        }
    }

    void InitializeBlock(string check)
    {
        string[] temp = check.Split(",");

        index = 0;

        if (percentBlock[0] > 0)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                boxInfoContentList[index].gameObject.SetActive(true);
                boxInfoContentList[index].Initialize(LocalizationManager.instance.GetString(temp[i]), percentBlock[0] / temp.Length * 1.0f);
                boxInfoContentList[index].SetRank(RankType.N + average);

                index++;
            }
        }

        if (percentBlock[1] > 0)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                boxInfoContentList[index].gameObject.SetActive(true);
                boxInfoContentList[index].Initialize(LocalizationManager.instance.GetString(temp[i]), percentBlock[1] / temp.Length * 1.0f);
                boxInfoContentList[index].SetRank(RankType.R + average);
                index++;
            }
        }

        if (percentBlock[2] > 0)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                boxInfoContentList[index].gameObject.SetActive(true);
                boxInfoContentList[index].Initialize(LocalizationManager.instance.GetString(temp[i]), percentBlock[2] / temp.Length * 1.0f);
                boxInfoContentList[index].SetRank(RankType.SR + average);

                index++;
            }
        }

        if (percentBlock[3] > 0)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                boxInfoContentList[index].gameObject.SetActive(true);
                boxInfoContentList[index].Initialize(LocalizationManager.instance.GetString(temp[i]), percentBlock[3] / temp.Length * 1.0f);
                boxInfoContentList[index].SetRank(RankType.SSR);

                index++;
            }
        }

        for (int i = 0; i < temp.Length; i++)
        {
            blockUIContentList[i].gameObject.SetActive(true);
            blockUIContentList[i].Initialize((BlockType)Enum.Parse(typeof(BlockType),temp[i]));
            blockUIContentList[i].SetRank(rankType);
        }

        boxInfoContentTransform.offsetMax = new Vector2(0, -9999);
    }
}
