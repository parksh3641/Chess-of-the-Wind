using Firebase.Analytics;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TitleNormalInformation
{
    public TitleNormalType titleNormalType = TitleNormalType.Default;
    public int check = 0;
}

[System.Serializable]
public class TitleSpeicalInformation
{
    public TitleSpeicalType titleSpeicalType = TitleSpeicalType.Default;
    public int check = 0;
}

public class TitleManager : MonoBehaviour
{
    public GameObject titleView;

    public LocalizationContent mainTitleText;
    public Text profileTitleText;

    public GameObject alarmObj;
    public GameObject profileAlarmObj;
    public GameObject normalAlarmObj;
    public GameObject speicalAlarmObj;

    [Title("TopMenu")]
    public Image[] topMenuImgArray;
    public Sprite[] topMenuSpriteArray;
    public GameObject[] scrollView;

    private int topNumber = 0;

    private bool isDelay = false;

    public TitleContent titleContent;

    public RectTransform titleNormalRectTransform;
    public RectTransform titleSpeicalRectTransform;

    private int ssrCount = 0;

    List<string> itemList = new List<string>();

    public List<TitleContent> titleNormalContentList = new List<TitleContent>();
    public List<TitleContent> titleSpeicalContentList = new List<TitleContent>();

    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        titleView.SetActive(false);
        alarmObj.SetActive(false);
        profileAlarmObj.SetActive(false);
        normalAlarmObj.SetActive(false);
        speicalAlarmObj.SetActive(false);

        topNumber = -1;

        for (int i = 0; i < System.Enum.GetValues(typeof(TitleNormalType)).Length; i++)
        {
            TitleContent content = Instantiate(titleContent);
            content.transform.SetParent(titleNormalRectTransform);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.titleNormalType = TitleNormalType.Default + i;
            content.titleManager = this;
            titleNormalContentList.Add(content);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(TitleSpeicalType)).Length - 1; i++)
        {
            TitleContent content = Instantiate(titleContent);
            content.transform.SetParent(titleSpeicalRectTransform);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.titleSpeicalType = TitleSpeicalType.TitleSpeical1 + i;
            content.titleManager = this;
            titleSpeicalContentList.Add(content);
        }

        //titleNormalRectTransform.offsetMax = Vector3.zero;
        titleSpeicalRectTransform.offsetMax = Vector3.zero;

        titleNormalRectTransform.anchoredPosition = new Vector2(0, -9999);
        //titleSpeicalRectTransform.anchoredPosition = new Vector2(0, -9999);
    }

    private void OnEnable()
    {
        PlayerDataBase.eGetNormalTitle += SetNormalAlarm;
        PlayerDataBase.eGetSpeicalTitle += SetSpeicalAlarm;
    }

    private void OnDisable()
    {
        PlayerDataBase.eGetNormalTitle -= SetNormalAlarm;
        PlayerDataBase.eGetSpeicalTitle -= SetSpeicalAlarm;
    }

    public void Initialize()
    {
        alarmObj.SetActive(false);
        profileAlarmObj.SetActive(false);
        normalAlarmObj.SetActive(false);
        speicalAlarmObj.SetActive(false);

        CheckGoal();
    }

    public void OpenTitleView()
    {
        if (!titleView.activeInHierarchy)
        {
            titleView.SetActive(true);

            isDelay = false;

            CheckGoal();

            if (topNumber == -1)
            {
                ChangeTopMenu(0);
            }
            else
            {
                switch (topNumber)
                {
                    case 0:
                        CheckNormalTitle();
                        break;
                    case 1:
                        CheckSpeicalTitle();
                        break;
                }
            }

            FirebaseAnalytics.LogEvent("Title");
        }
        else
        {
            titleView.SetActive(false);

            alarmObj.SetActive(false);
            profileAlarmObj.SetActive(false);
            normalAlarmObj.SetActive(false);
            speicalAlarmObj.SetActive(false);

            for (int i = 0; i < titleNormalContentList.Count; i++)
            {
                titleNormalContentList[i].SetAlarm(false);
            }

            for (int i = 0; i < titleSpeicalContentList.Count; i++)
            {
                titleSpeicalContentList[i].SetAlarm(false);
            }
        }
    }
    public void ChangeTopMenu(int number)
    {
        if (topNumber != number)
        {
            topNumber = number;

            for (int i = 0; i < topMenuImgArray.Length; i++)
            {
                topMenuImgArray[i].sprite = topMenuSpriteArray[0];
                scrollView[i].SetActive(false);
            }
            topMenuImgArray[number].sprite = topMenuSpriteArray[1];
            scrollView[number].SetActive(true);

            switch (number)
            {
                case 0:
                    CheckNormalTitle();
                    break;
                case 1:
                    CheckSpeicalTitle();
                    break;
            }
        }
    }


    void CheckNormalTitle()
    {
        for(int i = 0; i < titleNormalContentList.Count; i ++)
        {
            titleNormalContentList[i].Initialize();
        }

        if (playerDataBase.TitleNumber >= 500)
        {
            for (int i = 0; i < titleNormalContentList.Count; i++)
            {
                titleNormalContentList[i].UnEquip();
            }
        }
        else
        {
            titleNormalContentList[playerDataBase.TitleNumber].Equip();
        }

        //titleNormalContentList = titleNormalContentList.OrderByDescending(x => x.isActive).ToList();

        //for (int i = 0; i < titleNormalContentList.Count; i++)
        //{
        //    titleNormalContentList[i].transform.SetSiblingIndex(i);
        //}
    }

    void CheckSpeicalTitle()
    {
        for (int i = 0; i < titleSpeicalContentList.Count; i++)
        {
            titleSpeicalContentList[i].Initialize();
        }

        if (playerDataBase.TitleNumber < 500)
        {
            for (int i = 0; i < titleSpeicalContentList.Count; i++)
            {
                titleSpeicalContentList[i].UnEquip();
            }
        }
        else
        {
            titleSpeicalContentList[playerDataBase.TitleNumber - 500].Equip();
        }

        //titleSpeicalContentList = titleSpeicalContentList.OrderByDescending(x => x.isActive).ToList();

        //for (int i = 0; i < titleSpeicalContentList.Count; i++)
        //{
        //    titleSpeicalContentList[i].transform.SetSiblingIndex(i);
        //}
    }

    public void SetNormalTitle(TitleNormalType type)
    {
        if (isDelay) return;

        if(playerDataBase.TitleNumber != (int)type)
        {
            playerDataBase.TitleNumber = (int)type;
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("TitleNumber", playerDataBase.TitleNumber);

            SoundManager.instance.PlaySFX(GameSfxType.BlockEquip);
            NotionManager.instance.UseNotion(NotionType.EquipTitle);

            for(int i = 0; i < titleNormalContentList.Count; i ++)
            {
                titleNormalContentList[i].UnEquip();
            }

            titleNormalContentList[(int)type].Equip();

            mainTitleText.localizationName = playerDataBase.GetMainTitleName();
            mainTitleText.ReLoad();

            profileTitleText.text = playerDataBase.GetTitleName();

            isDelay = true;
            Invoke("Delay", 0.5f);
        }
    }

    public void SetSpeicalTitle(TitleSpeicalType type)
    {
        if (isDelay) return;

        if (playerDataBase.TitleNumber != 499 + (int)type)
        {
            playerDataBase.TitleNumber = 499 + (int)type;
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("TitleNumber", playerDataBase.TitleNumber);

            SoundManager.instance.PlaySFX(GameSfxType.BlockEquip);
            NotionManager.instance.UseNotion(NotionType.EquipTitle);

            for (int i = 0; i < titleSpeicalContentList.Count; i++)
            {
                titleSpeicalContentList[i].UnEquip();
            }

            titleSpeicalContentList[(int)type - 1].Equip();

            profileTitleText.text = playerDataBase.GetTitleName();

            isDelay = true;
            Invoke("Delay", 0.5f);
        }
    }

    void SetNormalAlarm()
    {
        alarmObj.SetActive(true);
        profileAlarmObj.SetActive(true);
        normalAlarmObj.SetActive(true);
    }

    void SetSpeicalAlarm()
    {
        alarmObj.SetActive(true);
        profileAlarmObj.SetActive(true);
        speicalAlarmObj.SetActive(true);
    }

    void GetSound()
    {
        SoundManager.instance.PlaySFX(GameSfxType.Success);
        NotionManager.instance.UseNotion(NotionType.NewTitleNotion);
    }

    public void CheckGoal()
    {
        if(playerDataBase.GosuWin >= 3)
        {
            if(playerDataBase.CheckNormalTitle(TitleNormalType.Title1) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title1.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title1].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title1);

                GetSound();
            }
        }

        if (playerDataBase.GosuWin >= 10)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title2) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title2.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title2].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title2);

                GetSound();
            }
        }

        if (playerDataBase.GosuWin >= 50)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title3) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title3.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title3].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title3);

                GetSound();
            }
        }

        if (playerDataBase.GosuWin >= 100)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title4) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title4.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title4].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title4);

                GetSound();
            }
        }

        if (playerDataBase.GosuWin >= 200)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title5) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title5.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title5].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title5);

                GetSound();
            }
        }

        if (playerDataBase.NowRank > (int)GameRankType.Bronze_1)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title6) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title6.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title6].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title6);

                GetSound();
            }
        }

        if (playerDataBase.NowRank > (int)GameRankType.Sliver_1)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title7) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title7.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title7].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title7);

                GetSound();
            }
        }

        if (playerDataBase.NowRank > (int)GameRankType.Gold_1)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title8) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title8.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title8].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title8);

                GetSound();
            }
        }

        if (playerDataBase.NowRank > (int)GameRankType.Platinum_1)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title9) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title9.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title9].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title9);

                GetSound();
            }
        }

        if (playerDataBase.NowRank > (int)GameRankType.Diamond_1)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title10) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title10.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title10].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title10);

                GetSound();
            }
        }

        if (GameStateManager.instance.WinStreak >= 3)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title11) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title11.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title11].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title11);

                GetSound();
            }
        }

        if (GameStateManager.instance.WinStreak >= 5)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title12) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title12.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title12].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title12);

                GetSound();
            }
        }

        if (GameStateManager.instance.WinStreak >= 7)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title13) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title13.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title13].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title13);

                GetSound();
            }
        }

        if (GameStateManager.instance.WinStreak >= 14)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title14) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title14.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title14].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title14);

                GetSound();
            }
        }

        if (GameStateManager.instance.WinStreak >= 28)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title15) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title15.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title15].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title15);

                GetSound();
            }
        }

        if (playerDataBase.CheckBlockLevel(12))
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title16) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title16.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title16].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title16);

                GetSound();
            }
        }

        if (playerDataBase.CheckBlockLevel(14))
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title17) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title17.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title17].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title17);

                GetSound();
            }
        }

        if (playerDataBase.CheckBlockLevel(15))
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title18) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title18.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title18].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title18);

                GetSound();
            }
        }

        if (playerDataBase.CheckBlockLevel(19))
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title19) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title19.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title19].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title19);

                GetSound();
            }
        }

        if (playerDataBase.CheckBlockLevel(24))
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title20) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title20.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title20].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title20);

                GetSound();
            }
        }

        if (playerDataBase.DestroyBlockCount >= 3)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title21) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title21.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title21].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title21);

                GetSound();
            }
        }

        if (playerDataBase.DestroyBlockCount >= 5)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title22) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title22.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title22].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title22);

                GetSound();
            }
        }

        if (playerDataBase.DestroyBlockCount >= 7)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title23) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title23.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title23].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title23);

                GetSound();
            }
        }

        if (playerDataBase.DestroyBlockCount >= 11)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title24) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title24.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title24].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title24);

                GetSound();
            }
        }

        if (playerDataBase.DestroyBlockCount >= 20)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title25) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title25.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title25].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title25);

                GetSound();
            }
        }

        if (playerDataBase.WinGetMoney >= 10000)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title26) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title26.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title26].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title26);

                GetSound();
            }
        }

        if (playerDataBase.WinGetMoney >= 100000)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title27) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title27.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title27].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title27);

                GetSound();
            }
        }

        if (playerDataBase.WinGetMoney >= 1000000)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title28) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title28.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title28].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title28);

                GetSound();
            }
        }

        if (playerDataBase.WinGetMoney >= 10000000)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title29) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title29.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title29].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title29);

                GetSound();
            }
        }

        if (playerDataBase.WinGetMoney >= 100000000)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title30) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title30.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title30].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title30);

                GetSound();
            }
        }

        if (playerDataBase.SynthesisGetBlock >= 5)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title31) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title31.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title31].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title31);

                GetSound();
            }
        }

        if (playerDataBase.SynthesisGetBlock >= 10)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title32) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title32.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title32].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title32);

                GetSound();
            }
        }

        if (playerDataBase.SynthesisGetBlock >= 20)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title33) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title33.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title33].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title33);

                GetSound();
            }
        }

        if (playerDataBase.SynthesisGetBlock >= 50)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title34) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title34.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title34].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title34);

                GetSound();
            }
        }

        if (playerDataBase.SynthesisGetBlock >= 100)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title35) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title35.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title35].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title35);

                GetSound();
            }
        }

        if (playerDataBase.RankDownStreak >= 2)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title36) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title36.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title36].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title36);

                GetSound();
            }
        }

        if (playerDataBase.RankDownStreak >= 4)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title37) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title37.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title37].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title37);

                GetSound();
            }
        }

        if (playerDataBase.RankDownStreak >= 6)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title38) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title38.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title38].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title38);

                GetSound();
            }
        }

        if (playerDataBase.RankDownStreak >= 10)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title39) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title39.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title39].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title39);

                GetSound();
            }
        }

        if (playerDataBase.RankDownStreak >= 16)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title40) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title40.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title40].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title40);

                GetSound();
            }
        }

        if (playerDataBase.WinNumber >= 30)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title41) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title41.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title41].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title41);

                GetSound();
            }
        }

        if (playerDataBase.WinNumber >= 50)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title42) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title42.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title42].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title42);

                GetSound();
            }
        }

        if (playerDataBase.WinNumber >= 100)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title43) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title43.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title43].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title43);

                GetSound();
            }
        }

        if (playerDataBase.WinNumber >= 300)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title44) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title44.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title44].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title44);

                GetSound();
            }
        }

        if (playerDataBase.WinNumber >= 1000)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title45) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title45.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title45].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title45);

                GetSound();
            }
        }

        if (playerDataBase.WinQueen >= 3)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title46) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title46.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title46].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title46);

                GetSound();
            }
        }

        if (playerDataBase.WinQueen >= 5)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title47) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title47.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title47].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title47);

                GetSound();
            }
        }

        if (playerDataBase.WinQueen >= 10)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title48) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title48.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title48].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title48);

                GetSound();
            }
        }

        if (playerDataBase.WinQueen >= 30)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title49) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title49.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title49].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title49);

                GetSound();
            }
        }

        if (playerDataBase.WinQueen >= 100)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title50) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title50.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title50].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title50);

                GetSound();
            }
        }

        if (playerDataBase.GetTitleHoldNumber() >= 10)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title51) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title51.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title51].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title51);

                GetSound();
            }
        }

        if (playerDataBase.GetTitleHoldNumber() >= 20)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title52) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title52.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title52].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title52);

                GetSound();
            }
        }

        if (playerDataBase.GetTitleHoldNumber() >= 30)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title53) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title53.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title53].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title53);

                GetSound();
            }
        }

        if (playerDataBase.GetTitleHoldNumber() >= 50)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title54) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title54.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title54].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title54);

                GetSound();
            }
        }

        if (playerDataBase.GetTitleHoldNumber() >= 100)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title55) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title55.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title55].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title55);

                GetSound();
            }
        }

        if (playerDataBase.BoxOpenCount >= 50)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title56) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title56.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title56].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title56);

                GetSound();
            }
        }

        if (playerDataBase.BoxOpenCount >= 100)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title57) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title57.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title57].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title57);

                GetSound();
            }
        }

        if (playerDataBase.BoxOpenCount >= 300)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title58) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title58.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title58].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title58);

                GetSound();
            }
        }

        if (playerDataBase.BoxOpenCount >= 3000)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title59) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title59.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title59].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title59);

                GetSound();
            }
        }

        if (playerDataBase.BoxOpenCount >= 6000)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title60) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title60.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title60].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title60);

                GetSound();
            }
        }

        if (playerDataBase.AccessDate >= 7)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title61) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title61.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title61].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title61);

                GetSound();
            }
        }

        if (playerDataBase.AccessDate >= 30)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title62) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title62.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title62].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title62);

                GetSound();
            }
        }

        if (playerDataBase.AccessDate >= 90)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title63) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title63.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title63].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title63);

                GetSound();
            }
        }

        if (playerDataBase.AccessDate >= 180)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title64) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title64.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title64].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title64);

                GetSound();
            }
        }

        if (playerDataBase.AccessDate >= 365)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title65) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title65.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title65].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title65);

                GetSound();
            }
        }

        if (playerDataBase.UpgradeSuccessCount >= 50)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title66) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title66.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title66].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title66);

                GetSound();
            }
        }

        if (playerDataBase.UpgradeSuccessCount >= 100)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title67) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title67.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title67].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title67);

                GetSound();
            }
        }

        if (playerDataBase.UpgradeSuccessCount >= 200)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title68) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title68.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title68].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title68);

                GetSound();
            }
        }

        if (playerDataBase.UpgradeSuccessCount >= 500)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title69) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title69.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title69].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title69);

                GetSound();
            }
        }

        if (playerDataBase.UpgradeSuccessCount >= 1000)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title70) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title70.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title70].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title70);

                GetSound();
            }
        }

        ssrCount = playerDataBase.CheckSSRBlockCount();

        if (ssrCount >= 3)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title71) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title71.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title71].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title71);

                GetSound();
            }
        }

        if (ssrCount >= 9)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title72) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title72.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title72].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title72);

                GetSound();
            }
        }

        if (ssrCount >= 18)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title73) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title73.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title73].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title73);

                GetSound();
            }
        }

        if (ssrCount >= 30)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title74) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title74.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title74].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title74);

                GetSound();
            }
        }

        if (ssrCount >= 60)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title75) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title75.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title75].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title75);

                GetSound();
            }
        }

        if (playerDataBase.RepairBlockCount >= 1)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title76) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title76.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title76].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title76);

                GetSound();
            }
        }

        if (playerDataBase.RepairBlockCount >= 10)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title77) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title77.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title77].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title77);

                GetSound();
            }
        }

        if (playerDataBase.RepairBlockCount >= 20)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title78) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title78.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title78].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title78);

                GetSound();
            }
        }

        if (playerDataBase.RepairBlockCount >= 30)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title79) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title79.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title79].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title79);

                GetSound();
            }
        }

        if (playerDataBase.RepairBlockCount >= 50)
        {
            if (playerDataBase.CheckNormalTitle(TitleNormalType.Title80) == 0)
            {
                itemList.Clear();
                itemList.Add(TitleNormalType.Title80.ToString());
                PlayfabManager.instance.GrantItemsToUser("Title", itemList);

                titleNormalContentList[(int)TitleNormalType.Title80].SetAlarm(true);
                playerDataBase.SetNormalTitle(TitleNormalType.Title80);

                GetSound();
            }
        }
    }

    void Delay()
    {
        isDelay = false;
    }
}
