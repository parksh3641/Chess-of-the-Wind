using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementContent : MonoBehaviour
{
    public AchievementType achievementType;

    public LocalizationContent titleText;

    public Text pointText;

    public Image gaugeFillamount;
    public Text gaugeText;

    public LocalizationContent rewardText;

    public Image buttonImg;

    public Sprite[] buttonImgArray;

    public bool isActive = false;
    public bool isDelay = false;

    int goal = 0;
    int count = 0;

    AchievementInformation achievementInformation = new AchievementInformation();
    AchievementInfo achievementInfo = new AchievementInfo();

    AchievementManager achievementManager;

    PlayerDataBase playerDataBase;
    AchievementDataBase achievementDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (achievementDataBase == null) achievementDataBase = Resources.Load("AchievementDataBase") as AchievementDataBase;
    }

    public void Initialize(AchievementManager manager)
    {
        achievementManager = manager;

        achievementInformation = achievementDataBase.GetAchievementInfomation(achievementType);
        achievementInfo = playerDataBase.GetAchievementInfo(achievementType);

        goal = (achievementInformation.startGoal * (achievementInfo.count + 1));
        count = playerDataBase.GetAchievementCount(achievementType);

        if(achievementType != AchievementType.WinGetMoney)
        {
            goal = (achievementInformation.startGoal * (achievementInfo.count + 1));
        }
        else
        {
            int number = (achievementInfo.count + 1) / 10;

            goal = (achievementInformation.startGoal * (achievementInfo.count + 1)) * (number + 1);
        }

        titleText.localizationName = "Achievement" + (int)(achievementType + 1);
        titleText.ReLoad();

        pointText.text = "P + " + achievementInformation.reward;

        if(count > 0)
        {
            gaugeText.text = MoneyUnitString.ToCurrencyString(count) + "/" + MoneyUnitString.ToCurrencyString(goal);
            gaugeFillamount.fillAmount = count * 1.0f / goal * 1.0f;
        }
        else
        {
            gaugeText.text = "0/" + MoneyUnitString.ToCurrencyString(goal);
            gaugeFillamount.fillAmount = 0;
        }

        if(count >= goal)
        {
            rewardText.localizationName = "Achieve";

            buttonImg.sprite = buttonImgArray[1];

            isActive = true;
        }
        else
        {
            rewardText.localizationName = "NotAchieved";

            buttonImg.sprite = buttonImgArray[0];

            isActive = false;
        }

        rewardText.ReLoad();
    }

    public void OnClick()
    {
        if (isDelay) return;

        if(isActive)
        {
            isActive = false;

            achievementManager.GetReward(achievementType);

            Initialize(achievementManager);

            isDelay = true;
            Invoke("Delay", 0.5f);
        }
    }

    void Delay()
    {
        isDelay = false;
    }
}
