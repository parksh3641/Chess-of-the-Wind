using DG.Tweening;
using Firebase.Analytics;
using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingManager : MonoBehaviour
{
    [Title("Newbie Limit Rank")]
    public GameRankType newbieLimitRank = GameRankType.Sliver_3;
    [Title("Gosu Limit Rank")]
    public GameRankType gosuLimitRank = GameRankType.Bronze_2;

    RankInformation rankInformation = new RankInformation();

    public GameObject dontTouchObj;
    public GameObject cancelButtonObj;

    [Title("Package")]
    public GameObject packageObj;
    public PackageContent packageContent;

    public Image rankButtonImg;
    public GameObject rankLocked;
    public ButtonScaleAnimation rankButtonAnimation;

    public Sprite[] rankImgArray;

    [Title("Rank Up")]
    public GameObject rankUpView;
    public GameObject rankUpEffect;

    public Text rankUpWiningTitle;
    public Text rankUpWiningReward;
    public Text rankUpWiningDaily;

    public Text rankUpTitle;
    public Image rankUpIcon;
    public Text rankUpText;

    public GameObject[] starBackground;
    public ImageAnimation[] star;

    public GameObject tapToContinue;

    [Title("Enter")]
    public Image rankImg;
    public LocalizationContent rankText;

    public LocalizationContent newbieEnterText;
    public LocalizationContent newbieMaxBlockText;

    public LocalizationContent gosuEnterText;
    public LocalizationContent gosuMaxBlockText;

    Sprite[] rankIconArray;

    public Image[] starArray;

    string[] strArray = new string[2];
    string[] strArray2 = new string[2];

    public Image newbieLimitRankImg;
    public LocalizationContent newbieLimitRankText;

    public Image gosuLimitRankImg;
    public LocalizationContent gosuLimitRankText;

    [Title("LowAllowMoney")]
    public GameObject lowAllowMoneyView;
    public Text lowAllowMoneyText;
    private int lowAllowMoney = 0;


    [Title("Matching")]
    public GameObject matchingView;
    public Text matchingText;
    public Image matchingButton;

    public Sprite[] matchingButtonArray;

    private int starSave = 0;
    private int stakes = 0;
    private int limitBlock = 0;
    private int matchingWaitTime = 0;

    bool isCancle = false;
    bool isWait = false;
    bool isServer = false;

    BlockClass blockClass;
    BlockClass blockClass2;
    BlockClass blockClass3;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.5f);

    public FadeInOut mainFadeInOut;
    public NetworkManager networkManager;
    public UIManager uIManager;
    public AiManager aiManager;
    public PackageManager packageManager;

    PlayerDataBase playerDataBase;
    RankDataBase rankDataBase;
    UpgradeDataBase upgradeDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rankIconArray = imageDataBase.GetRankIconArray();

        rankUpEffect.SetActive(false);
        rankUpView.SetActive(false);
        matchingView.SetActive(false);
        dontTouchObj.SetActive(false);
        packageObj.SetActive(false);
        lowAllowMoneyView.SetActive(false);

        DOTween.RewindAll();
    }

    public void Initialize()
    {
        int needStar = rankDataBase.GetNeedStar(playerDataBase.NowRank) + 1;

        for (int i = 0; i < starArray.Length; i ++)
        {
            starArray[i].gameObject.SetActive(false);
        }

        for(int i =0; i < needStar - 1; i ++)
        {
            starArray[i].gameObject.SetActive(true);
            starArray[i].color = Color.black;
        }

        if(playerDataBase.Star > starArray.Length)
        {
            for (int i = 0; i < starArray.Length; i++)
            {
                starArray[i].color = Color.white;
            }
        }
        else
        {
            for (int i = 0; i < playerDataBase.Star; i++)
            {
                starArray[i].color = Color.white;
            }
        }

        rankImg.sprite = rankIconArray[playerDataBase.NowRank];

        strArray = rankDataBase.rankInformationArray[playerDataBase.NowRank].gameRankType.ToString().Split("_");

        rankText.localizationName = strArray[0];
        rankText.plusText = " " + strArray[1];
        rankText.ReLoad();

        rankInformation = rankDataBase.GetRankInformation(GameStateManager.instance.GameRankType);

        stakes = rankInformation.stakes;
        limitBlock = rankInformation.limitBlockLevel;

        newbieEnterText.localizationName = "AllowMoney";
        newbieEnterText.plusText = " : " + MoneyUnitString.ToCurrencyString(stakes);

        newbieMaxBlockText.localizationName = "AllowBlockLevel";
        newbieMaxBlockText.plusText = " : " + MoneyUnitString.ToCurrencyString(limitBlock);

        gosuEnterText.localizationName = "AllowMoney";
        gosuEnterText.plusText = " : " + MoneyUnitString.ToCurrencyString(stakes);

        gosuMaxBlockText.localizationName = "AllowBlockLevel";
        gosuMaxBlockText.plusText = " : " + MoneyUnitString.ToCurrencyString(limitBlock);

        newbieEnterText.ReLoad();
        newbieMaxBlockText.ReLoad();
        gosuEnterText.ReLoad();
        gosuMaxBlockText.ReLoad();

        strArray = rankDataBase.rankInformationArray[(int)newbieLimitRank].gameRankType.ToString().Split("_");

        newbieLimitRankImg.sprite = rankIconArray[(int)newbieLimitRank];
        newbieLimitRankText.localizationName = strArray[0];
        newbieLimitRankText.plusText = " " + strArray[1] + " ▼";
        newbieLimitRankText.ReLoad();


        strArray = rankDataBase.rankInformationArray[(int)gosuLimitRank].gameRankType.ToString().Split("_");

        gosuLimitRankImg.sprite = rankIconArray[(int)gosuLimitRank];
        gosuLimitRankText.localizationName = strArray[0];
        gosuLimitRankText.plusText = " " + strArray[1] + " ▲";
        gosuLimitRankText.ReLoad();

        if (playerDataBase.NowRank < (int)gosuLimitRank)
        {
            rankButtonAnimation.StopAnim();

            rankLocked.gameObject.SetActive(true);
        }
        else
        {
            rankButtonAnimation.PlayAnim();

            rankLocked.gameObject.SetActive(false);
        }
    }

    [Button]
    public void CheckRankUp()
    {
        if (!GameStateManager.instance.Win && !GameStateManager.instance.Lose)
        {
            return;
        }

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);

            return;
        }

        rankUpView.SetActive(true);

        rankUpEffect.SetActive(false);

        tapToContinue.SetActive(false);

        rankUpWiningTitle.text = "";
        rankUpWiningReward.text = "";
        rankUpWiningDaily.text = "";

        rankUpText.text = "";

        rankUpIcon.sprite = rankIconArray[(int)GameStateManager.instance.GameRankType];

        isWait = true;

        if (GameStateManager.instance.Win)
        {
            rankUpTitle.text = LocalizationManager.instance.GetString("Win");

            GameStateManager.instance.Win = false;
            GameStateManager.instance.LoseStreak = 0;

            int needStar = rankDataBase.GetNeedStar(playerDataBase.NowRank) + 1;

            starSave = playerDataBase.Star;

            GameStateManager.instance.WinStreak += 1;

            if(playerDataBase.ResetInfo.dailyStar == 0)
            {
                playerDataBase.Star += 1;

                rankUpWiningDaily.text = LocalizationManager.instance.GetString("WinningDaily");

                ResetManager.instance.SetResetInfo(ResetType.DailyStar);

                Debug.Log("첫 승리 보상 : 별 1개 추가 획득");
            }

            if (GameStateManager.instance.WinStreak >= 3)
            {
                playerDataBase.Star += 2;

                rankUpWiningReward.text = LocalizationManager.instance.GetString("WinningReward");

                Debug.Log("3연승 이상 승리 :  별 2개 획득");
            }
            else
            {
                playerDataBase.Star += 1;

                Debug.Log("승리 : 별 1개 획득");
            }

            rankUpWiningTitle.text = LocalizationManager.instance.GetString("Winning") + " : " + GameStateManager.instance.WinStreak;

            FirebaseAnalytics.LogEvent("Win_StarUp");

            if (GameStateManager.instance.GameType == GameType.NewBie)
            {
                playerDataBase.NewbieWin += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("NewbieWin", playerDataBase.NewbieWin);
            }
            else
            {
                playerDataBase.GosuWin += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("GosuWin", playerDataBase.GosuWin);
            }

            if(playerDataBase.NowRank != rankDataBase.rankInformationArray.Length - 1)
            {
                if (playerDataBase.Star >= needStar)
                {
                    playerDataBase.NowRank += 1;

                    GameStateManager.instance.GameRankType += 1;

                    playerDataBase.Star -= needStar;

                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("NowRank", playerDataBase.NowRank);

                    if(playerDataBase.NowRank > playerDataBase.HighRank)
                    {
                        if (playerDataBase.TestAccount > 0)
                        {
                            playerDataBase.HighRank = 0;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("HighRank", 0);
                        }
                        else
                        {
                            playerDataBase.HighRank = playerDataBase.NowRank;
                            PlayfabManager.instance.UpdatePlayerStatisticsInsert("HighRank", playerDataBase.HighRank);
                        }

                        switch(playerDataBase.HighRank)
                        {
                            case 4:
                                OpenPackage(PackageType.Sliver);
                                break;
                            case 8:
                                OpenPackage(PackageType.Gold);
                                break;
                            case 12:
                                OpenPackage(PackageType.Platinum);
                                break;
                            case 16:
                                OpenPackage(PackageType.Diamond);
                                break;
                            case 20:
                                OpenPackage(PackageType.Legend);
                                break;
                        }

                        Debug.Log("최고 랭크 갱신 !");
                    }

                    playerDataBase.Star += 1;

                    StarAnimation(true);

                    OpenRankUpView(true);

                    Debug.Log("랭크 상승 !");
                }
                else
                {
                    StarAnimation(true);

                    Debug.Log("별 개수 상승");

                    Invoke("Delay", 0.8f);
                }
            }
            else
            {
                if(playerDataBase.Star < 11)
                {
                    StarAnimation(true);

                    Debug.Log("별 개수 상승");

                    Invoke("Delay", 0.8f);
                }
                else
                {
                    rankUpView.SetActive(false);

                    Debug.Log("최고 랭크 달성 !");
                }
            }
        }
        else if(GameStateManager.instance.Lose)
        {
            if(playerDataBase.NowRank < 4)
            {
                rankUpView.SetActive(false);

                GameStateManager.instance.Lose = false;
                GameStateManager.instance.WinStreak = 0;
                GameStateManager.instance.LoseStreak += 1;

                if (GameStateManager.instance.LoseStreak >= 3)
                {
                    GameStateManager.instance.LoseStreak = 0;

                    if (GameStateManager.instance.StoreType != StoreType.OneStore)
                    {
                        OpenPackage(PackageType.Supply);
                    }
                }

                Debug.Log("브론즈 단계에서는 랭크가 하락하지 않습니다");

                return;
            }

            rankUpTitle.text = LocalizationManager.instance.GetString("Lose");

            GameStateManager.instance.Lose = false;
            GameStateManager.instance.WinStreak = 0;
            GameStateManager.instance.LoseStreak += 1;

            starSave = playerDataBase.Star;

            playerDataBase.Star -= 1;

            if (GameStateManager.instance.GameType == GameType.NewBie)
            {
                playerDataBase.NewbieLose += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("NewbieLose", playerDataBase.NewbieLose);
            }
            else
            {
                playerDataBase.GosuLose += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("GosuLose", playerDataBase.GosuLose);
            }

            Debug.Log("패배 : 별 1개 감소");

            FirebaseAnalytics.LogEvent("Lose_StarDown");

            if (playerDataBase.Star <= -1)
            {
                if(playerDataBase.NowRank != 0)
                {
                    playerDataBase.Star = rankDataBase.GetNeedStar(playerDataBase.NowRank - 1) - 1;

                    playerDataBase.NowRank -= 1;

                    GameStateManager.instance.GameRankType -= 1;

                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("NowRank", playerDataBase.NowRank);

                    StarAnimation(false);

                    OpenRankUpView(false);

                    Debug.Log("랭크 하락");
                }
                else if(playerDataBase.NowRank == 0)
                {
                    rankUpView.SetActive(false);

                    playerDataBase.Star = 0;

                    Debug.Log("최하 랭크 입니다");
                }
            }
            else
            {
                StarAnimation(false);

                Debug.Log("별 개수 하락");

                Invoke("Delay", 0.8f);
            }
        }

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Star", playerDataBase.Star);

        Initialize();
    }

    void StarAnimation(bool check)
    {
        int needStar = rankDataBase.GetNeedStar(playerDataBase.NowRank) + 1;

        for (int i = 0; i < starBackground.Length; i++)
        {
            starBackground[i].gameObject.SetActive(false);
            star[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < needStar - 1; i++)
        {
            starBackground[i].gameObject.SetActive(true);
            star[i].gameObject.SetActive(true);
            star[i].StateOff();
        }

        for (int i = 0; i < playerDataBase.Star; i++)
        {
            star[i].StateOn();
        }

        if(check)
        {
            for (int i = 0; i < playerDataBase.Star - starSave; i++)
            {
                star[starSave + i].StarUp();
            }
        }
        else
        {
            for (int i = 0; i < starSave - playerDataBase.Star; i++)
            {
                star[playerDataBase.Star - i].StarDown();
            }
        }
    }

    void OpenRankUpView(bool check)
    {
        rankUpView.SetActive(true);

        rankUpEffect.SetActive(check);

        rankUpIcon.sprite = rankIconArray[(int)GameStateManager.instance.GameRankType];

        if (check)
        {
            rankUpTitle.text = LocalizationManager.instance.GetString("RankUp");

            strArray = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType - 1].gameRankType.ToString().Split("_");
            strArray2 = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType].gameRankType.ToString().Split("_");

            rankUpText.text = LocalizationManager.instance.GetString(strArray[0]) + " <color=#FFC032>" + strArray[1] + "</color>     ▶     " +
    LocalizationManager.instance.GetString(strArray2[0]) + " <color=#FFC032>" + strArray2[1] + "</color>";

            SoundManager.instance.PlaySFX(GameSfxType.RankUp);

            FirebaseAnalytics.LogEvent("RankUp_" + GameStateManager.instance.GameRankType.ToString());

            Invoke("SoundDelay", 2f);

        }
        else
        {
            rankUpTitle.text = LocalizationManager.instance.GetString("RankDown");

            strArray = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType + 1].gameRankType.ToString().Split("_");
            strArray2 = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType].gameRankType.ToString().Split("_");

            rankUpText.text = LocalizationManager.instance.GetString(strArray[0]) + " <color=#FFC032>" + strArray[1] + "</color>     ▶     " +
    LocalizationManager.instance.GetString(strArray2[0]) + " <color=#FFC032>" + strArray2[1] + "</color>";

            SoundManager.instance.PlaySFX(GameSfxType.RankDown);

            FirebaseAnalytics.LogEvent("RankDown_" + GameStateManager.instance.GameRankType.ToString());
        }

        Invoke("Delay", 1.5f);
    }

    void SoundDelay()
    {
        SoundManager.instance.StopSFX(GameSfxType.RankUp);
    }

    public void CloseRankUpView()
    {
        if(!isWait)
        {
            rankUpView.SetActive(false);

            dontTouchObj.SetActive(false);
        }
    }

    void Delay()
    {
        isWait = false;

        tapToContinue.SetActive(true);
    }

    public void CheckServer_Fail()
    {
        isServer = false;

        NotionManager.instance.UseNotion(NotionType.CheckInternet);
    }

    public void GameStartButton_Newbie()
    {
        if(isServer)
        {
            return;
        }

        isServer = true;

        if (playerDataBase.NowRank > (int)newbieLimitRank)
        {
            isServer = false;

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.LimitRank);
            return;
        }

        rankInformation = rankDataBase.GetRankInformation(GameStateManager.instance.GameRankType);

        stakes = rankInformation.stakes;

        GameStateManager.instance.Stakes = stakes;

        if (!playerDataBase.CheckEquipBlock_Newbie()) //블록은 장착했는지
        {
            isServer = false;

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NeedEquipBlock);

            return;
        }

        if (playerDataBase.Coin < stakes) //입장료를 가지고 있는지?
        {
            isServer = false;

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            if(GameStateManager.instance.GameRankType < GameRankType.Sliver_4)
            {
                lowAllowMoneyView.SetActive(true);
                lowAllowMoney = rankInformation.stakes;
                lowAllowMoneyText.text = lowAllowMoney.ToString();
            }
            else
            {
                NotionManager.instance.UseNotion(NotionType.LowAllowMoney);
            }

            return;
        }

        OpenMacthingView();

        cancelButtonObj.SetActive(true);

        matchingText.text = LocalizationManager.instance.GetString("MatchingInfo");

        if (playerDataBase.NewbieWin < 2)
        {
            cancelButtonObj.SetActive(false);
        }

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            isServer = false;

            isCancle = true;
            OpenMacthingView();

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        if (!PhotonNetwork.IsConnected)
        {
            networkManager.Initialize(0);
            return;
        }

        CheckServer_NewBie();
    }

    public void CheckServer_NewBie()
    {
        PlayfabManager.instance.GetTitleInternalData("Newbie", GameStart_Newbie);
    }

    void GameStart_Newbie(bool check)
    {
        if(!check)
        {
            isServer = false;

            isCancle = true;
            OpenMacthingView();

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.LockedMode);
            return;
        }

        isCancle = true;
        StopAllCoroutines();
        StartCoroutine(WaitingPlayer());
        StartCoroutine(MatchingCoroution());

        networkManager.JoinRandomRoom_Newbie();

        Debug.Log("초보방 매칭중입니다.");
    }

    public void GameStartButton_Gosu()
    {
        if (isServer)
        {
            return;
        }

        isServer = true;

        if (playerDataBase.NowRank < (int)gosuLimitRank)
        {
            isServer = false;

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.LimitRank);
            return;
        }

        rankInformation = rankDataBase.GetRankInformation(GameStateManager.instance.GameRankType);

        stakes = rankInformation.stakes;
        limitBlock = rankInformation.limitBlockLevel;

        GameStateManager.instance.Stakes = stakes;

        if (playerDataBase.CheckEquipBlock_Gosu() == 0) //1개라도 착용했을 경우
        {
            isServer = false;

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NeedEquipBlock);

            return;
        }

        if (playerDataBase.Coin < stakes) //입장료를 가지고 있는지?
        {
            isServer = false;

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            if (GameStateManager.instance.GameRankType < GameRankType.Sliver_4)
            {
                lowAllowMoneyView.SetActive(true);
                lowAllowMoney = rankInformation.stakes;
                lowAllowMoneyText.text = lowAllowMoney.ToString();
            }
            else
            {
                NotionManager.instance.UseNotion(NotionType.LowAllowMoney);
            }

            return;
        }

        OpenMacthingView();

        cancelButtonObj.SetActive(true);

        matchingText.text = LocalizationManager.instance.GetString("MatchingInfo");

        //if (playerDataBase.GosuWin < 1)
        //{
        //    cancelButtonObj.SetActive(false);
        //}

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            isServer = false;

            isCancle = true;
            OpenMacthingView();

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        if (!PhotonNetwork.IsConnected)
        {
            networkManager.Initialize(1);
            return;
        }

        CheckServer_Gosu();
    }

    public void CheckServer_Gosu()
    {
        PlayfabManager.instance.GetTitleInternalData("Rank", GameStart_Gosu);
    }

    void GameStart_Gosu(bool check)
    {
        if (!check)
        {
            isServer = false;

            isCancle = true;
            OpenMacthingView();

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            NotionManager.instance.UseNotion(NotionType.LockedMode);
            return;
        }

        isCancle = true;
        StopAllCoroutines();
        StartCoroutine(WaitingPlayer());
        StartCoroutine(MatchingCoroution());

        networkManager.JoinRandomRoom_Gosu();

        Debug.Log("고수방 매칭중입니다.");
    }

    public void CancelMatching()
    {
        isServer = false;

        StopAllCoroutines();

        networkManager.LeaveRoom();

        matchingView.SetActive(false);

        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
        NotionManager.instance.UseNotion(NotionType.WaitTimeNotion);
    }

    public void OpenMacthingView()
    {
        if (!matchingView.activeSelf)
        {
            matchingView.SetActive(true);

            matchingButton.sprite = matchingButtonArray[1];

            matchingWaitTime = GameStateManager.instance.MatchingTime + Random.Range(0, 3);
            //matchingText.text = = LocalizationManager.instance.GetString("MatchingInfo") + " : " + matchingWaitTime;
        }
        else
        {
            if (isCancle)
            {
                isServer = false;

                StopAllCoroutines();

                networkManager.LeaveRoom();

                matchingView.SetActive(false);
            }
        }
    }

    IEnumerator WaitingPlayer()
    {
        while(matchingWaitTime > 0)
        {
            matchingWaitTime -= 1;

            if (matchingWaitTime <= 1)
            {
                matchingButton.sprite = matchingButtonArray[0];

                isCancle = false;
            }

            yield return waitForSeconds;
        }

        yield return waitForSeconds;

        AiMatching();
    }

    IEnumerator MatchingCoroution()
    {
        matchingText.text = LocalizationManager.instance.GetString("MatchingInfo");

        yield return waitForSeconds2;

        matchingText.text = LocalizationManager.instance.GetString("MatchingInfo") + ".";

        yield return waitForSeconds2;

        matchingText.text = LocalizationManager.instance.GetString("MatchingInfo") + "..";

        yield return waitForSeconds2;

        matchingText.text = LocalizationManager.instance.GetString("MatchingInfo") + "...";

        yield return waitForSeconds2;

        StartCoroutine(MatchingCoroution());
    }

    public void PlayerMatching(string player1, string player2, GameRankType gameRankType, int otherFormation, int otherTitle)
    {
        Debug.Log("플레이어와 매칭되었습니다.");

        isServer = false;

        dontTouchObj.SetActive(true);

        StopAllCoroutines();

        matchingView.SetActive(false);

        uIManager.OnMatchingSuccess(player1, player2, gameRankType, otherFormation, otherTitle);

        mainFadeInOut.FadeOutToIn();

        Invoke("ChangeBGM", 1.5f);

        FirebaseAnalytics.LogEvent("Match_" + GameStateManager.instance.GameType.ToString());
    }

    public void AiMatching()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;

        isServer = false;

        dontTouchObj.SetActive(true);

        StopAllCoroutines();

        matchingView.SetActive(false);

        uIManager.OnMatchingAi(aiManager.RandomCharacter());

        mainFadeInOut.FadeOutToIn();

        Invoke("ChangeBGM", 1.5f);

        Debug.Log("사람이 없는 관계로 인공지능과 매칭됩니다.");

        FirebaseAnalytics.LogEvent("Match_Ai");
    }

    public void ChangeBGM()
    {
        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            SoundManager.instance.PlayBGM(GameBgmType.Game_Newbie);
        }
        else
        {
            SoundManager.instance.PlayBGM(GameBgmType.Game_Gosu);
        }

        dontTouchObj.SetActive(false);
    }

    public void StarUp()
    {
        GameStateManager.instance.Win = true;
        CheckRankUp();
    }

    public void StarDown()
    {
        GameStateManager.instance.Lose = true;
        CheckRankUp();
    }

    public void Legendary()
    {
        GameStateManager.instance.GameRankType = GameRankType.Trials_3;
        playerDataBase.NowRank = 25;
        playerDataBase.Star = 14;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("NowRank", playerDataBase.NowRank);

        playerDataBase.HighRank = 0;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("HighRank", 0);

        GameStateManager.instance.Win = true;
        CheckRankUp();
    }

    [Button]
    public void OpenPackage(int number)
    {
        switch(number)
        {
            case 0:
                OpenPackage(PackageType.Newbie);
                break;
            case 1:
                OpenPackage(PackageType.Supply);
                break;
            case 2:
                OpenPackage(PackageType.Sliver);
                break;
            case 3:
                OpenPackage(PackageType.Gold);
                break;
            case 4:
                OpenPackage(PackageType.Platinum);
                break;
            case 5:
                OpenPackage(PackageType.Diamond);
                break;
            case 6:
                OpenPackage(PackageType.Legend);
                break;
        }
    }


    public void OpenPackage(PackageType packageType)
    {
        packageObj.SetActive(true);

        packageManager.rankUpPackage = true;

        packageContent.packageType = packageType;
        packageContent.Initialize(packageManager);
    }

    public void CloseSupplyPackage()
    {
        packageManager.rankUpPackage = false;

        packageObj.SetActive(false);
    }


    public void GetAdReward()
    {
        lowAllowMoneyView.SetActive(false);

        PlayfabManager.instance.UpdateAddGold(rankInformation.stakes);

        NotionManager.instance.UseNotion(NotionType.GetWatchAdReward);
    }

    public void CloseLowAllowMoneyView()
    {
        lowAllowMoneyView.SetActive(false);
    }
}
