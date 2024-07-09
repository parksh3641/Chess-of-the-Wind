using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankContent : MonoBehaviour
{
    public GameObject frame;
    public GameObject effect;

    public Text indexText;
    public Image indexRankImg;
    public Sprite[] rankIconList;
    public Text nickNameText;
    public Text titleText;
    public Image countryImg;
    public Image rankImg;
    public Text nowRankText;
    public Text nowScoreText;

    private Sprite[] rankIconArray;

    string[] strArray = new string[2];

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;
    RankDataBase rankDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;

        rankIconArray = imageDataBase.GetRankIconArray();

        indexText.text = "";
        nickNameText.text = "";
    }

    public void SetIndex(int index)
    {
        if (index <= 3)
        {
            indexRankImg.enabled = true;
            indexRankImg.sprite = rankIconList[index - 1];
            effect.SetActive(true);
        }
        else
        {
            indexRankImg.enabled = false;
            indexText.text = index.ToString();
            effect.SetActive(false);
        }
    }

    public void InitState(int index, string country, string nickName, string score, bool checkMy, int type)
    {
        titleText.text = "-";

        if (index <= 3)
        {
            indexRankImg.enabled = true;
            indexRankImg.sprite = rankIconList[index - 1];
            effect.SetActive(true);
        }
        else
        {
            indexRankImg.enabled = false;
            indexText.text = index.ToString();
            effect.SetActive(false);
        }

        nickNameText.text = nickName;

        if(type == 0)
        {
            countryImg.enabled = true;
            rankImg.enabled = true;
            nowScoreText.text = "";

            countryImg.sprite = Resources.Load<Sprite>("Country/" + country);
            rankImg.sprite = rankIconArray[int.Parse(score)];
            strArray = rankDataBase.rankInformationArray[int.Parse(score)].gameRankType.ToString().Split("_");
            nowRankText.text = strArray[1];
        }
        else
        {
            countryImg.enabled = false;
            rankImg.enabled = false;
            nowRankText.text = "";

            nowScoreText.text = MoneyUnitString.ToCurrencyString(int.Parse(score));
        }

        if (index == 999)
        {
            indexText.text = "-";
        }

        frame.SetActive(checkMy);
    }

    public void TitleState(int number)
    {
        titleText.text = LocalizationManager.instance.GetString(playerDataBase.GetMainTitleName(number));
    }
}
