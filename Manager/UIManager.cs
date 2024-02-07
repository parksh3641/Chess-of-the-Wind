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

    public Image[] bottomUIImg;
    public Animator[] bottmUIAnimator;

    public int index = 0;
    Sprite[] characterArray;

    bool first = false;

    private List<string> nicknames = new List<string>();

    [Space]
    [Title("DataBase")]
    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        instance = this;

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

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
        if(!GameStateManager.instance.PrivacyPolicy)
        {
            privacypolicyView.SetActive(true);
        }

#if !UNITY_EDITOR
            GameStateManager.instance.BettingWaitTime = 4;
#endif

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

        FirebaseAnalytics.LogEvent("Privacy Policy");
    }

    public void TermsReadMore()
    {
        Application.OpenURL("https://sites.google.com/view/bluebook-terms");

        FirebaseAnalytics.LogEvent("Terms of Service");
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

    public void OnMatchingSuccess(string player1, string player2, int otherFormation, int otherTitle)
    {
        StartCoroutine(MatchingCoroution(player1, player2, otherFormation, otherTitle, false));
    }

    public void OnMatchingAi(int otherFormation)
    {
        int randomIndex = Random.Range(0, nicknames.Count);
        string randomNickname = nicknames[randomIndex];

        StartCoroutine(MatchingCoroution(randomNickname, GameStateManager.instance.NickName, otherFormation, 0, true));
    }


    IEnumerator MatchingCoroution(string player1, string player2, int otherFormation, int otherTitle , bool aiMode)
    {
        //rankText.text = matchingManager.rankText.text;

        tipText.text = LocalizationManager.instance.GetString("Tip_" + (Random.Range(13,24).ToString()));

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

        RecordManager.instance.OpenRecord();

        resultGoldText.text = "";
        dailyWinText.text = "";

        if (gold >= 0)
        {
            Debug.Log("돈 증가 애니메이션 발동");

            SoundManager.instance.PlaySFX(GameSfxType.GameWin);

            if (GameStateManager.instance.Win)
            {
                if (playerDataBase.DailyWin == 0)
                {
                    playerDataBase.DailyWin = 1;
                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyWin", playerDataBase.DailyWin);

                    dailyWinText.text = LocalizationManager.instance.GetString("DailyWin") + " : +" + MoneyUnitString.ToCurrencyString((int)(gold * 0.5f));

                    gold = gold + (int)(gold * 0.5f);

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
        bottmUIAnimator[number].Play("SizeUp");

        shopManager.CloseShopView();
        collectionManager.CloseCollectionView();

        switch (number)
        {
            case 0:
                shopManager.OpenShopView();

                bottmUIAnimator[1].Play("SizeDown");
                bottmUIAnimator[2].Play("SizeDown");
                break;
            case 1:
                challengeManager.CheckingGoal();
                eventManager.CheckingRankUp();

                bottmUIAnimator[0].Play("SizeDown");
                bottmUIAnimator[2].Play("SizeDown");
                break;
            case 2:
                collectionManager.OpenCollectionView();

                bottmUIAnimator[0].Play("SizeDown");
                bottmUIAnimator[1].Play("SizeDown");
                break;
            case 3:
                break;
            case 4:
                break;
        }    
    }

    public void OnNeedUpdate()
    {
        updateView.SetActive(true);
    }

    public void OnUpdate()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.bluebook.windchess");
#elif UNITY_IOS
        Application.OpenURL("https://apps.apple.com/kr/app/windchess-timing-of-destiny/id6455494059");
#else
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.bluebook.windchess");
#endif
    }

    public void GoToTutorial()
    {
        PlayerPrefs.SetString("LoadScene", "TutorialScene");
        SceneManager.LoadScene("LoadScene");
    }

    public void Feedback()
    {
        Application.OpenURL("https://forms.gle/CixT4KwjQvQL2yDD7");

        FirebaseAnalytics.LogEvent("Feedback");
    }

    public void NaverCafe()
    {
        Application.OpenURL("https://cafe.naver.com/windchess");

        FirebaseAnalytics.LogEvent("NaverCafe");
    }
    public void Instagram()
    {
        Application.OpenURL("https://www.instagram.com/windchess_kr_official/");

        FirebaseAnalytics.LogEvent("Instagram");
    }

    public void Youtube()
    {
        Application.OpenURL("https://www.youtube.com/@windchess_kr_official/");

        FirebaseAnalytics.LogEvent("Youtube");
    }
}
