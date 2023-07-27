using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public GameObject profileView;

    public Image characterImg;

    public Text nickNameText;

    public Image nowRankImg;
    public Text nowRankText;

    public Image highRankImg;
    public Text highRankText;

    public Text newbieWinTitleText;
    public Text newbieWinText;

    public Text gosuWinTitleText;
    public Text gosuWinText;

    Sprite[] characterArray;
    Sprite[] rankIconArray;

    string[] strArray = new string[2];
    string[] strArray2 = new string[2];

    PlayerDataBase playerDataBase;
    RankDataBase rankDataBase;
    ImageDataBase imageDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        characterArray = imageDataBase.GetCharacterArray();
        rankIconArray = imageDataBase.GetRankIconArray();

        profileView.SetActive(false);
    }

    public void OpenProfileView()
    {
        if (!profileView.activeInHierarchy)
        {
            profileView.SetActive(true);

            Initialize();
        }
        else
        {
            profileView.SetActive(false);
        }
    }

    void Initialize()
    {
        nickNameText.text = LocalizationManager.instance.GetString("Nickname") + " : " + GameStateManager.instance.NickName;

        if (playerDataBase.Formation == 2)
        {
            characterImg.sprite = characterArray[1];
        }
        else
        {
            characterImg.sprite = characterArray[0];
        }

        //int rank = rankDataBase.GetRank(playerDataBase.Gold) - 1;

        nowRankImg.sprite = rankIconArray[playerDataBase.NowRank];
        strArray = rankDataBase.rankInformationArray[playerDataBase.NowRank].gameRankType.ToString().Split("_");
        nowRankText.text = strArray[1];

        highRankImg.sprite = rankIconArray[playerDataBase.HighRank];
        strArray2 = rankDataBase.rankInformationArray[playerDataBase.HighRank].gameRankType.ToString().Split("_");
        highRankText.text = strArray2[1];

        newbieWinTitleText.text = LocalizationManager.instance.GetString("Newbie") + " " + LocalizationManager.instance.GetString("Win");
        newbieWinText.text = playerDataBase.NewbieWin.ToString();

        gosuWinTitleText.text = LocalizationManager.instance.GetString("Gosu") + " " + LocalizationManager.instance.GetString("Win");
        gosuWinText.text = playerDataBase.GosuWin.ToString();


    }

    public void CopyId()
    {
        GUIUtility.systemCopyBuffer = nickNameText.text;

        NotionManager.instance.UseNotion(NotionType.CopyIdNotion);
    }
}
