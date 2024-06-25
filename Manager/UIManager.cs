#if UNITY_ANDROID
using Google.Play.AppUpdate;
using Google.Play.Common;
#endif
using Firebase.Analytics;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject updateView;

    public GameObject privacypolicyView;

    [Space]
    [Title("Login")]
    public GameObject loginUI;
    public GameObject[] loginButtonList;
    public Text versionText;

    public GameObject testMode;

    [Space]
    [Title("Main")]
    public Text goldText;
    public Text crystalText;
    public LocalizationContent titleText;
    public Text nickNameText;

    [Space]
    [Title("View")]
    public Canvas mainCanvas;
    public Canvas gameCanvas;

    public GameObject loginView;
    public GameObject mainView;

    [Space]
    public GameObject bettingView;
    public GameObject rouletteView;
    public GameObject bounsView;
    public GameObject surrenderView;
    public GameObject disconnectedView;

    [Space]
    [Title("AppReview")]
    public GameObject appReview;

    [Space]
    public GameObject dontTouchObj;
    public GameObject waitingObj;

    [Space]
    [Title("VS")]
    public GameObject vsView;
    public Text tipText;
    public Text rankText;
    public Image player1Img;
    public Image player2Img;
    public Text player1Text;
    public Text player1TitleText;
    public Text player2Text;
    public Text player2TitleText;
    public FadeInOut vsFadeInOut;
    public FadeInOut mainFadeInOut;
    public Image player1Rank;
    public Text player1RankText;
    public Image player2Rank;
    public Text player2RankText;

    [Space]
    [Title("Roulette")]
    public Image mainPlayer1Img;
    public Image mainPlayer2Img;

    [Space]
    [Title("Result")]
    public GameObject resultView;
    public Image characterImg;
    public Text resultTitleText;
    public Text resultTalkText;
    public Text resultGoldText;
    public Text dailyWinText;
    public GameObject resultButton;
    public Text seasonPassText;

    [Space]
    [Title("MainCanvas")]
    public ShopManager shopManager;
    public CollectionManager collectionManager;
    public GameManager gameManager;
    public MatchingManager matchingManager;
    public MoneyAnimation moneyAnimation;
    public ChallengeManager challengeManager;
    public TitleManager titleManager;
    public EventManager eventManager;
    public InventoryManager inventoryManager;

    public Image[] bottomUIImg;
    public Animator[] bottmUIAnimator;

    public int index = 0;
    Sprite[] characterArray;
    private int seasonPass = 0;

    string[] strArray = new string[2];

    private List<string> nicknames = new List<string>();

    [Space]
    [Title("DataBase")]
    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;
    RankDataBase rankDataBase;

    private void Awake()
    {
        instance = this;

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;

        characterArray = imageDataBase.GetCharacterArray();

        updateView.SetActive(false);

        goldText.text = "0";
        crystalText.text = "0";
        nickNameText.text = "";

        mainCanvas.enabled = true;
        gameCanvas.enabled = false;

        loginView.SetActive(true);
        mainView.SetActive(false);

        vsView.SetActive(false);
        bettingView.SetActive(false);
        rouletteView.SetActive(false);
        bounsView.SetActive(false);
        resultView.SetActive(false);
        surrenderView.SetActive(false);
        disconnectedView.SetActive(false);
        appReview.SetActive(false);

        dontTouchObj.SetActive(false);
        waitingObj.SetActive(false);

        versionText.text = "v" + Application.version;

        index = -1;

        testMode.SetActive(false);

        privacypolicyView.SetActive(false);

        TextAsset nicknameTextAsset = Resources.Load<TextAsset>("Nicknames");

        if (nicknameTextAsset != null)
        {
            string[] lines = nicknameTextAsset.text.Split('\n');
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    nicknames.Add(trimmedLine);
                }
            }
        }
        else
        {
            Debug.LogError("Nicknames.txt not found in Resources folder.");
        }
    }

    private void Start()
    {
        GameStateManager.instance.StoreType = StoreType.None;

#if !UNITY_EDITOR && UNITY_ANDROID
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
        string installerPackageName = packageManager.Call<string>("getInstallerPackageName", Application.identifier);

        // 패키지명으로부터 설치된 스토어 확인

        if (installerPackageName.Equals("com.android.vending"))
        {
            GameStateManager.instance.StoreType = StoreType.Google;

            Debug.Log("앱은 Google Play 스토어에서 설치되었습니다.");
        }
        else if (installerPackageName.Equals("com.amazon.venezia"))
        {
            GameStateManager.instance.StoreType = StoreType.Amazon;

            Debug.Log("앱은 Amazon Appstore에서 설치되었습니다.");
        }
        else if (installerPackageName.Equals("com.skt.skaf.A000Z00040") || installerPackageName.Equals("com.kt.olleh.storefront")
            || installerPackageName.Equals("android.lgt.appstore") || installerPackageName.Equals("com.lguplus.appstore"))
        {
            GameStateManager.instance.StoreType = StoreType.OneStore;

            Debug.Log("앱은 OneStore에서 설치되었습니다.");
        }
        else
        {
            GameStateManager.instance.StoreType = StoreType.None;

            Debug.Log("앱은 알 수 없는 소스에서 설치되었습니다.");
        }
#endif

        //GameStateManager.instance.StoreType = StoreType.OneStore;

        if (!GameStateManager.instance.PrivacyPolicy)
        {
            privacypolicyView.SetActive(true);
        }

        GameStateManager.instance.BettingTime = 9;
        GameStateManager.instance.MatchingTime = 9;
        GameStateManager.instance.BettingWaitTime = 4;

        SetLoginUI();
    }

    public void PrivacyAgree()
    {
        GameStateManager.instance.PrivacyPolicy = true;
        privacypolicyView.SetActive(false);
    }

    public void PrivacyDecline()
    {
        Application.Quit();
    }

    public void PrivacyReadMore()
    {
        Application.OpenURL("https://sites.google.com/view/bluebook-privacypolicy");

        FirebaseAnalytics.LogEvent("Open_Privacy Policy");
    }

    public void TermsReadMore()
    {
        Application.OpenURL("https://sites.google.com/view/bluebook-terms");

        FirebaseAnalytics.LogEvent("Open_Terms of Service");
    }

    public void Initialize()
    {
        loginView.SetActive(false);
        mainView.SetActive(true);

        Renewal();

        if (playerDataBase.TestAccount > 0)
        {
            testMode.SetActive(true);
        }

        OpenMainCanvas(1);

        titleText.localizationName = playerDataBase.GetMainTitleName();
        titleText.ReLoad();

        if(playerDataBase.Update > 0)
        {
            playerDataBase.Update = 0;
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("Update", playerDataBase.Update);

            OnNeedUpdate();
        }

#if UNITY_EDITOR || UNITY_EDITOR_OSX
        testMode.SetActive(true);
#endif
    }

    //public void TestMode(bool check)
    //{
    //    testMode.SetActive(check);
    //}

    public void Renewal()
    {
        playerDataBase.Coin = playerDataBase.CoinA + (playerDataBase.CoinB * 100000000);

        goldText.text = MoneyUnitString.ToCurrencyString(playerDataBase.Coin);
        //crystalText.text = MoneyUnitString.ToCurrencyString(playerDataBase.Crystal);

        nickNameText.text = GameStateManager.instance.NickName;

        Debug.Log("메인화면 갱신");
    }

    public void SetLoginUI()
    {
        loginUI.SetActive(false);

        for (int i = 0; i < loginButtonList.Length; i++)
        {
            loginButtonList[i].SetActive(false);
        }

        if (!GameStateManager.instance.AutoLogin)
        {
            loginUI.SetActive(true);

#if UNITY_EDITOR || UNITY_EDITOR_OSX
            loginButtonList[0].SetActive(true);
#else
            loginButtonList[0].SetActive(false);
#endif

#if UNITY_ANDROID
            loginButtonList[1].SetActive(true);
#elif UNITY_IOS
            loginButtonList[2].SetActive(true);
#endif
            if(GameStateManager.instance.StoreType == StoreType.OneStore)
            {
                loginButtonList[0].SetActive(true);
                loginButtonList[1].SetActive(false);
            }
        }
    }

    public void LoginFail()
    {
        loginUI.SetActive(true);

#if UNITY_EDITOR || UNITY_EDITOR_OSX
        loginButtonList[0].SetActive(true);
#else
        loginButtonList[0].SetActive(false);
#endif

#if UNITY_ANDROID
        loginButtonList[1].SetActive(true);
#elif UNITY_IOS
        loginButtonList[2].SetActive(true);
#endif
    }

    public void OnMatchingSuccess(string player1, string player2, GameRankType gameRankType, int otherFormation, int otherTitle)
    {
        StartCoroutine(MatchingCoroution(player1, player2, gameRankType, otherFormation, otherTitle, false));
    }

    public void OnMatchingAi(int otherFormation)
    {
        string randomNickname = nicknames[Random.Range(0, nicknames.Count)];

        Debug.Log(randomNickname);

        StartCoroutine(MatchingCoroution(randomNickname, GameStateManager.instance.NickName, GameStateManager.instance.GameRankType, otherFormation, 0, true));
    }


    IEnumerator MatchingCoroution(string player1, string player2, GameRankType gameRankType, int otherFormation, int otherTitle , bool aiMode)
    {
        //rankText.text = matchingManager.rankText.text;

        tipText.text = LocalizationManager.instance.GetString("Tip_" + Random.Range(13,24).ToString());

        strArray = rankDataBase.rankInformationArray[(int)gameRankType].gameRankType.ToString().Split("_");

        player1Rank.sprite = imageDataBase.GetRankIconArray(GameStateManager.instance.GameRankType);
        player1RankText.text = strArray[1];

        strArray = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType].gameRankType.ToString().Split("_");

        player2Rank.sprite = imageDataBase.GetRankIconArray(GameStateManager.instance.GameRankType);
        player2RankText.text = strArray[1];

        if (otherFormation == 2)
        {
            player1Img.sprite = characterArray[1];
            player1Img.transform.rotation = Quaternion.Euler(0, 180, 0);

            mainPlayer1Img.sprite = characterArray[1];
            mainPlayer1Img.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            player1Img.sprite = characterArray[0];
            player1Img.transform.rotation = Quaternion.identity;

            mainPlayer1Img.sprite = characterArray[0];
            mainPlayer1Img.transform.rotation = Quaternion.identity;
        }

        player1TitleText.text = playerDataBase.GetTitleName(otherTitle);
        player2TitleText.text = playerDataBase.GetTitleName();

        if (playerDataBase.Formation == 2)
        {
            player2Img.sprite = characterArray[1];
            player2Img.transform.rotation = Quaternion.identity;

            mainPlayer2Img.sprite = characterArray[1];
            mainPlayer2Img.transform.rotation = Quaternion.identity;
        }
        else
        {
            player2Img.sprite = characterArray[0];
            player2Img.transform.rotation = Quaternion.Euler(0, 180, 0);

            mainPlayer2Img.sprite = characterArray[0];
            mainPlayer2Img.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        GameStateManager.instance.Playing = true;

        yield return new WaitForSeconds(1f);

        mainCanvas.enabled = false;
        gameCanvas.enabled = true;

        vsView.SetActive(true);

        player1Text.text = player1;
        player2Text.text = player2;

        GameStart();

        gameManager.GameStart_Initialize();

        yield return new WaitForSeconds(3f);

        vsFadeInOut.FadeOut();

        yield return new WaitForSeconds(1.5f);

        if(!aiMode)
        {
            if (GameStateManager.instance.GameType == GameType.NewBie)
            {
                gameManager.GameStart_Newbie();
            }
            else
            {
                gameManager.GameStart_Gosu();
            }
        }
        else
        {
            if (GameStateManager.instance.GameType == GameType.NewBie)
            {
                gameManager.GameStart_Newbie_Ai();
            }
            else
            {
                gameManager.GameStart_Gosu_Ai();
            }
        }
    }

    public void GameStart()
    {
        mainView.SetActive(false);
        bettingView.SetActive(true);

        dontTouchObj.SetActive(true);
    }

    public void GameEnd()
    {
        mainFadeInOut.FadeOutToIn();

        StartCoroutine(GameEndCoroution());
    }

    IEnumerator GameEndCoroution()
    {
        yield return new WaitForSeconds(1f);

        SoundManager.instance.PlayBGM();

        mainCanvas.enabled = true;
        gameCanvas.enabled = false;

        mainView.SetActive(true);
        bettingView.SetActive(false);
        rouletteView.SetActive(false);

        dontTouchObj.SetActive(false);
        waitingObj.SetActive(false);

        resultView.SetActive(false);

        Renewal();

        matchingManager.CheckRankUp();
        titleManager.CheckGoal();
    }

    public void OpenRouletteView()
    {
        bettingView.SetActive(false);
        rouletteView.SetActive(true);
    }

    public void CloseRouletteView()
    {
        bettingView.SetActive(true);
        rouletteView.SetActive(false);

        dontTouchObj.SetActive(true);
    }

    public void SetWaitingView(bool check)
    {
        waitingObj.SetActive(check);
    }

    public void OpenBounsView(bool check)
    {
        bounsView.SetActive(check);
    }

    public void OpenResultView(int number, int gold)
    {
        if (resultView.activeInHierarchy) return;

        resultView.SetActive(true);

        seasonPass = 0;

        resultButton.SetActive(false);

        if (playerDataBase.Formation == 2)
        {
            characterImg.sprite = characterArray[1];
        }
        else
        {
            characterImg.sprite = characterArray[0];
        }

        if (number == 0)
        {
            seasonPass += 250;

            resultTitleText.text = LocalizationManager.instance.GetString("Win");

            if (playerDataBase.Formation == 2)
            {
                resultTalkText.text = LocalizationManager.instance.GetString("Win_Under");
            }
            else
            {
                resultTalkText.text = LocalizationManager.instance.GetString("Win_Winter");
            }

            GameStateManager.instance.Win = true;
        }
        else if(number == 1)
        {
            seasonPass += 50;

            resultTitleText.text = LocalizationManager.instance.GetString("Lose");

            if (playerDataBase.Formation == 2)
            {
                resultTalkText.text = LocalizationManager.instance.GetString("Lose_Under");
            }
            else
            {
                resultTalkText.text = LocalizationManager.instance.GetString("Lose_Winter");
            }

            GameStateManager.instance.Lose = true;
        }
        else if (number == 2)
        {
            seasonPass += 200;

            resultTitleText.text = LocalizationManager.instance.GetString("Surrender_Enemy");

            if (playerDataBase.Formation == 2)
            {
                resultTalkText.text = LocalizationManager.instance.GetString("Win2_Under");
            }
            else
            {
                resultTalkText.text = LocalizationManager.instance.GetString("Win2_Winter");
            }

            GameStateManager.instance.Win = true;
        }
        else
        {
            seasonPass += 100;

            resultTitleText.text = LocalizationManager.instance.GetString("Tie");

            if (playerDataBase.Formation == 2)
            {
                resultTalkText.text = LocalizationManager.instance.GetString("Tie_Under");
            }
            else
            {
                resultTalkText.text = LocalizationManager.instance.GetString("Tie_Winter");
            }
        }

        seasonPassText.text = "+" + seasonPass.ToString();

        playerDataBase.SeasonPassLevel += seasonPass;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SeasonPassLevel", playerDataBase.SeasonPassLevel);

        RecordManager.instance.OpenRecord();

        resultGoldText.text = "";
        dailyWinText.text = "";

        if (gold >= 0)
        {
            Debug.Log("돈 증가 애니메이션 발동");

            gold += GameStateManager.instance.Stakes;

            Debug.Log("획득한 돈 : " + gold);

            SoundManager.instance.PlaySFX(GameSfxType.GameWin);

            if (GameStateManager.instance.Win)
            {
                if (playerDataBase.ResetInfo.dailyWin == 0)
                {
                    ResetManager.instance.SetResetInfo(ResetType.DailyWin);

                    dailyWinText.text = LocalizationManager.instance.GetString("DailyWin") + " : " + MoneyUnitString.ToCurrencyString(gold)
                        + "  (+" + MoneyUnitString.ToCurrencyString((int)(gold * 0.5f)) + ")";

                    gold += (int)(gold * 0.5f);

                    Debug.Log("오늘 첫 승 1.5배 보너스");
                }
            }

            moneyAnimation.ResultAddMoney(gold, resultGoldText);

            playerDataBase.WinGetMoney += gold;
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("WinGetMoney", playerDataBase.WinGetMoney);

            PlayfabManager.instance.UpdateAddGold(gold);
        }
        else
        {
            Debug.Log("돈 감소 애니메이션 발동");

            SoundManager.instance.PlaySFX(GameSfxType.GameLose);

            moneyAnimation.ResultMinusMoney(-GameStateManager.instance.Stakes, resultGoldText);

            PlayfabManager.instance.UpdateSubtractGold(GameStateManager.instance.Stakes);
        }

        SoundManager.instance.PlaySFX(GameSfxType.ResultMoney);
    }

    public void EndResultGoldAnimation()
    {
        if(resultView.activeInHierarchy)
        {
            resultButton.SetActive(true);
        }
    }

    public void OpenSurrenderView()
    {
        if (!surrenderView.activeSelf)
        {
            surrenderView.SetActive(true);
        }
        else
        {
            surrenderView.SetActive(false);
        }
    }

    public void CloseSurrenderView()
    {
        surrenderView.SetActive(false);
    }

    public void OpenDisconnectedView()
    {
        disconnectedView.SetActive(true);
    }

    public void GoToMain()
    {
        if(!NetworkConnect.instance.CheckConnectInternet())
        {
            NotionManager.instance.UseNotion(NotionType.CheckInternet);
        }
        else
        {
            disconnectedView.SetActive(false);

            gameManager.ExitRoom();
        }

        //SceneManager.LoadScene("LoginScene");
    }

    public void OpenMainCanvas(int number)
    {
        if (index == number) return;

        index = number;

        for (int i = 0; i < bottomUIImg.Length; i++)
        {
            bottomUIImg[i].enabled = false;
        }

        bottomUIImg[number].enabled = true;

        for(int i = 0; i < bottmUIAnimator.Length; i ++)
        {
            bottmUIAnimator[i].Play("SizeDown");
        }

        shopManager.CloseShopView();
        collectionManager.CloseCollectionView();
        inventoryManager.CloseInventoryView();

        switch (number)
        {
            case 0:
                shopManager.OpenShopView();
                break;
            case 1:
                challengeManager.CheckingGoal();
                eventManager.CheckingRankUp();

                break;
            case 2:
                collectionManager.OpenCollectionView();
                break;
            case 3:
                inventoryManager.OpenInventoryView();
                break;
            case 4:
                break;
        }

        bottmUIAnimator[number].Play("SizeUp");
    }

    public void OnNeedUpdate()
    {
        updateView.SetActive(true);
    }

    public void OnUpdate()
    {
#if UNITY_EDITOR
        updateView.SetActive(false);
#elif UNITY_ANDROID
        StartCoroutine(CheckForUpdate());
#elif UNITY_IOS
        Application.OpenURL("https://apps.apple.com/kr/app/windchess-timing-of-destiny/id6455494059");
#endif

        FirebaseAnalytics.LogEvent("Open_Update");
    }


#if UNITY_ANDROID
    IEnumerator CheckForUpdate()
    {
        yield return new WaitForSeconds(0.5f);

        AppUpdateManager appUpdateManager = new AppUpdateManager();

        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();

        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
                var startUpdateRequest = appUpdateManager.StartUpdate(appUpdateInfoResult, appUpdateOptions);

                while (!startUpdateRequest.IsDone)
                {
                    if (startUpdateRequest.Status == AppUpdateStatus.Downloading)
                    {
                        Debug.Log("업데이트 다운로드 진행중");

                    }
                    else if (startUpdateRequest.Status == AppUpdateStatus.Downloaded)
                    {
                        Debug.Log("다운로드가 완료");
                    }

                    yield return null;
                }

                var result = appUpdateManager.CompleteUpdate();

                while (!result.IsDone)
                {
                    yield return new WaitForEndOfFrame();
                }

                yield return (int)startUpdateRequest.Status;
            }
            else if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateNotAvailable)
            {
                Debug.Log("업데이트가 없습니다");
            }
        }
        else
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.bluebook.windchess");
            Debug.Log("업데이트 에러");
        }
    }
#endif

    public void GoToTutorial()
    {
        PlayerPrefs.SetString("LoadScene", "TutorialScene");
        SceneManager.LoadScene("LoadScene");
    }

    public void Feedback()
    {
        Application.OpenURL("https://forms.gle/CixT4KwjQvQL2yDD7");

        FirebaseAnalytics.LogEvent("Open_Feedback");
    }

    public void NaverCafe()
    {
        Application.OpenURL("https://cafe.naver.com/windchess");

        FirebaseAnalytics.LogEvent("Open_NaverCafe");
    }
    public void Instagram()
    {
        Application.OpenURL("https://www.instagram.com/windchess_kr_official/");

        FirebaseAnalytics.LogEvent("Open_Instagram");
    }

    public void Youtube()
    {
        Application.OpenURL("https://www.youtube.com/@windchess_kr_official/");

        FirebaseAnalytics.LogEvent("Open_Youtube");
    }

    public void KakaoTalk()
    {
        Application.OpenURL("https://open.kakao.com/o/gtU2erhg");

        FirebaseAnalytics.LogEvent("Open_KakaoTalk");
    }

    public void OpenAppReview()
    {
        if (playerDataBase.AppReview == 1) return;

        appReview.SetActive(true);
    }

    public void CloseAppReview()
    {
        appReview.SetActive(false);

        playerDataBase.AppReview = 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("AppReview", 1);
    }

    public void OpenAppReviewEvent()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.bluebook.windchess");
#elif UNITY_IOS
        Application.OpenURL("https://apps.apple.com/kr/app/windchess-timing-of-destiny/id6455494059");
#endif

        FirebaseAnalytics.LogEvent("Open__AppReview_Event");
    }

    public void OpenReview()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.bluebook.windchess");
#elif UNITY_IOS
        Application.OpenURL("https://apps.apple.com/kr/app/windchess-timing-of-destiny/id6455494059");
#endif

        appReview.SetActive(false);

        playerDataBase.AppReview = 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("AppReview", 1);

        FirebaseAnalytics.LogEvent("Open__AppReview");
    }

    public void ComingSoon()
    {
        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
        NotionManager.instance.UseNotion(NotionType.ComingSoon);
    }
}
