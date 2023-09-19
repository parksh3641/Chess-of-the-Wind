using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public Text millageText;
    public Image millageFillamount;

    public Image boxIcon;
    public ButtonScaleAnimation boxAnim;

    public GameObject alarmObj;
    public GameObject achievementAlarmObj;


    public RectTransform achievementRectTransform;


    int goal = 0;
    int count = 0;

    public List<AchievementContent> achievementContentList = new List<AchievementContent>();

    Dictionary<string, string> infoData = new Dictionary<string, string>();

    AchievementInformation achievementInformation = new AchievementInformation();
    AchievementInfo achievementInfo = new AchievementInfo();

    public ShopManager shopManager;

    PlayerDataBase playerDataBase;
    AchievementDataBase achievementDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (achievementDataBase == null) achievementDataBase = Resources.Load("AchievementDataBase") as AchievementDataBase;

        alarmObj.SetActive(false);
        achievementAlarmObj.SetActive(false);

        achievementRectTransform.offsetMax = Vector3.zero;
    }

    private void Start()
    {
        for (int i = 0; i < achievementContentList.Count; i++)
        {
            achievementContentList[i].achievementType = AchievementType.AccessDate + i;

            if (achievementContentList[i].achievementType.Equals(AchievementType.ChargingRM) ||
                achievementContentList[i].achievementType.Equals(AchievementType.UpgradeFailCount) ||
                achievementContentList[i].achievementType.Equals(AchievementType.UseUpgradeTicket))
            {
                achievementContentList[i].gameObject.SetActive(false);
            }
        }
    }

    public void Initialize()
    {
        for(int i = 0; i < achievementContentList.Count; i ++)
        {
            if (!achievementContentList[i].achievementType.Equals(AchievementType.ChargingRM) &&
                !achievementContentList[i].achievementType.Equals(AchievementType.UpgradeFailCount) &&
                !achievementContentList[i].achievementType.Equals(AchievementType.UseUpgradeTicket))
            {
                achievementContentList[i].Initialize(this);
            }
        }

        achievementContentList = achievementContentList.OrderByDescending(x => x.isActive).ToList();

        for (int i = 0; i < achievementContentList.Count; i++)
        {
            achievementContentList[i].transform.SetSiblingIndex(i);
        }

        millageText.text = playerDataBase.Millage + "/100";

        if(playerDataBase.Millage > 0)
        {
            millageFillamount.fillAmount = playerDataBase.Millage * 1.0f / 100f;
        }
        else
        {
            millageFillamount.fillAmount = 0;
        }

        if(playerDataBase.Millage >= 100)
        {
            boxIcon.color = Color.white;
            boxAnim.PlayAnim();
        }
        else
        {
            boxIcon.color = Color.gray;
            boxAnim.StopAnim();
        }
    }

    public void CheckGoal()
    {
        for(int i = 0; i < achievementContentList.Count; i ++)
        {
            achievementInformation = achievementDataBase.GetAchievementInfomation(AchievementType.AccessDate + i);
            achievementInfo = playerDataBase.GetAchievementInfo(AchievementType.AccessDate + i);

            goal = (achievementInformation.startGoal * (achievementInfo.count + 1));
            count = playerDataBase.GetAchievementCount(AchievementType.AccessDate + i);

            if(count >= goal)
            {
                alarmObj.SetActive(true);
                achievementAlarmObj.SetActive(false);
            }
        }
    }

    public void GetBox()
    {
        if(playerDataBase.Millage >= 100)
        {
            if (!NetworkConnect.instance.CheckConnectInternet())
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                NotionManager.instance.UseNotion(NotionType.CheckInternet);
                return;
            }

            PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Millage, 100);

            shopManager.OpenRandomBox(1);

            SoundManager.instance.PlaySFX(GameSfxType.Success);
            NotionManager.instance.UseNotion(NotionType.GetReward);

            Initialize();
        }
        else
        {
            ReceiveInfoManager.instance.OpenReceiveInfo(RewardType.Box);
        }
    }

    public void GetReward(AchievementType type)
    {
        achievementInformation = achievementDataBase.GetAchievementInfomation(type);
        achievementInfo = playerDataBase.GetAchievementInfo(type);

        playerDataBase.SetAchievementInfoCount(type);

        infoData.Clear();
        infoData.Add(type.ToString(), (achievementInfo.count).ToString());

        PlayfabManager.instance.SetPlayerData(infoData);

        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Millage, achievementInformation.reward);

        SoundManager.instance.PlaySFX(GameSfxType.Success);
        NotionManager.instance.UseNotion(NotionType.GetReward);

        Initialize();

        alarmObj.SetActive(false);
        achievementAlarmObj.SetActive(false);
    }


    #region Developer

    [Button]
    public void AccessDate()
    {
        playerDataBase.AccessDate += 1;

        Initialize();
    }

    [Button]
    public void GosuWin()
    {
        playerDataBase.GosuWin += 3;

        Initialize();
    }

    [Button]
    public void DestroyBlockCount()
    {
        playerDataBase.DestroyBlockCount += 1;

        Initialize();
    }

    [Button]
    public void WinGetMoney()
    {
        playerDataBase.WinGetMoney += 10000;

        Initialize();
    }

    [Button]
    public void TotalRaf()
    {
        playerDataBase.TotalRaf += 1000;

        Initialize();
    }

    [Button]
    public void RankDownCount()
    {
        playerDataBase.RankDownCount += 1;

        Initialize();
    }

    [Button]
    public void WinNumber()
    {
        playerDataBase.WinNumber += 10;

        Initialize();
    }

    [Button]
    public void WinQueen()
    {
        playerDataBase.WinQueen += 1;

        Initialize();
    }

    [Button]
    public void ChargingRM()
    {
        playerDataBase.ChargingRM += 1000;

        Initialize();
    }

    [Button]
    public void BoxOpenCount()
    {
        playerDataBase.BoxOpenCount += 10;

        Initialize();
    }

    [Button]
    public void UpgradeSuccessCount()
    {
        playerDataBase.UpgradeSuccessCount += 5;

        Initialize();
    }

    [Button]
    public void UpgradeFailCount()
    {
        playerDataBase.UpgradeFailCount += 5;

        Initialize();
    }

    [Button]
    public void UseUpgradeTicket()
    {
        playerDataBase.UseUpgradeTicket += 30;

        Initialize();
    }

    [Button]
    public void RepairBlockCount()
    {
        playerDataBase.RepairBlockCount += 1;

        Initialize();
    }

    #endregion
}
