using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject updateView;

    [Space]
    [Title("Login")]
    public GameObject loginUI;
    public GameObject[] loginButtonList;
    public Text versionText;

    [Space]
    [Title("Main")]
    public Text goldText;
    public Text crystalText;
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
    public Text rankText;
    public Image player1Img;
    public Image player2Img;
    public Text player1Text;
    public Text player2Text;
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

    [Space]
    [Title("MainCanvas")]
    public ShopManager shopManager;
    public CollectionManager collectionManager;
    public GameManager gameManager;
    public MatchingManager matchingManager;
    public MoneyAnimation moneyAnimation;

    public Image[] bottomUIImg;

    public int index = 0;
    public bool isFirst = false;

    Sprite[] characterArray;

    [Space]
    [Title("DataBase")]
    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
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

        isFirst = false;
    }

    private void Start()
    {
        GameStateManager.instance.Playing = false;

        SetLoginUI();

        OpenMainCanvas(1);
    }

    public void Initialize()
    {
        Renewal();
    }

    public void Renewal()
    {
        goldText.text = MoneyUnitString.ToCurrencyString(playerDataBase.Gold);
        crystalText.text = MoneyUnitString.ToCurrencyString(playerDataBase.Crystal);

        nickNameText.text = GameStateManager.instance.NickName;

        matchingManager.Initialize();

        Debug.Log("Main UI Renewal");
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

            loginButtonList[0].SetActive(true);

            //#if UNITY_ANDROID
            //            loginButtonList[1].SetActive(true);
            //#elif UNITY_IOS
            //            loginButtonList[2].SetActive(true);
            //#endif
        }
    }

    public void OnLoginSuccess()
    {
        if(!GameStateManager.instance.Tutorial)
        {
            SceneManager.LoadScene("TutorialScene");
        }
        else
        {
            loginView.SetActive(false);
            mainView.SetActive(true);
        }
    }

    public void OnMatchingSuccess(string player1, string player2, int otherFormation)
    {
        StartCoroutine(MatchingCoroution(player1, player2, otherFormation, false));
    }

    public void OnMatchingAi(int otherFormation)
    {
        StartCoroutine(MatchingCoroution("인공지능", GameStateManager.instance.NickName, otherFormation, true));
    }


    IEnumerator MatchingCoroution(string player1, string player2, int otherFormation, bool aiMode)
    {
        rankText.text = matchingManager.rankText.text;

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

        mainCanvas.enabled = true;
        gameCanvas.enabled = false;

        mainView.SetActive(true);
        bettingView.SetActive(false);
        rouletteView.SetActive(false);

        dontTouchObj.SetActive(false);
        waitingObj.SetActive(false);

        resultView.SetActive(false);

        Renewal();
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
        resultView.SetActive(true);

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
            resultTitleText.text = "승리";

            if (playerDataBase.Formation == 2)
            {
                resultTalkText.text = "좋았어! 우리의 승리야.";
            }
            else
            {
                resultTalkText.text = "저희의 승리입니다.";
            }

            SoundManager.instance.PlaySFX(GameSfxType.GameWin);
        }
        else if(number == 1)
        {
            resultTitleText.text = "패배";

            if (playerDataBase.Formation == 2)
            {
                resultTalkText.text = "흥. 아쉬운걸?";
            }
            else
            {
                resultTalkText.text = "아쉽네요. 다음번에 더 잘해봐요.";
            }

            SoundManager.instance.PlaySFX(GameSfxType.GameLose);
        }
        else if (number == 2)
        {
            resultTitleText.text = "상대방 항복으로 승리";

            if (playerDataBase.Formation == 2)
            {
                resultTalkText.text = "승리는 언제나 달콤해.";
            }
            else
            {
                resultTalkText.text = "완벽한 승리입니다.";
            }

            SoundManager.instance.PlaySFX(GameSfxType.GameWin);
        }
        else
        {
            resultTitleText.text = "무승부";

            if (playerDataBase.Formation == 2)
            {
                resultTalkText.text = "무승부라고? 이럴수가..";
            }
            else
            {
                resultTalkText.text = "무승부라니 상대팀도 잘했군요.";
            }

            SoundManager.instance.PlaySFX(GameSfxType.GameLose);
        }


        resultGoldText.text = "";

        gold = 9999999;

        if (gold > 0)
        {
            moneyAnimation.ResultAddMoney(gold, resultGoldText);
            //resultGoldText.text = "+<color=#27FFFC>" + MoneyUnitString.ToCurrencyString(Mathf.Abs(gold)) + "</color> 만큼 돈 증가!";
        }
        else
        {
            moneyAnimation.ResultMinusMoney(gold, resultGoldText);
            //resultGoldText.text = "-<color=#FF712B>" + MoneyUnitString.ToCurrencyString(Mathf.Abs(gold)) + "</color> 만큼 돈 감소";
        }

        SoundManager.instance.PlaySFX(GameSfxType.ResultMoney);

        RecordManager.instance.OpenRecord();
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

        for (int i = 0; i < bottomUIImg.Length; i++)
        {
            bottomUIImg[i].enabled = false;
        }

        bottomUIImg[number].enabled = true;


        shopManager.CloseShopView();
        collectionManager.CloseCollectionView();


        switch (number)
        {
            case 0:
                shopManager.OpenShopView();
                break;
            case 1:
                if(!isFirst)
                {
                    isFirst = true;
                }
                else
                {
                    matchingManager.Initialize();
                }
                break;
            case 2:
                collectionManager.OpenCollectionView();
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
        Application.OpenURL("https://play.google.com/store/apps/dev?id=8493220400768769623&hl=ko&gl=KR");
#elif UNITY_IOS
        Application.OpenURL("https://play.google.com/store/apps/dev?id=8493220400768769623&hl=ko&gl=KR");
#else
        Application.OpenURL("https://play.google.com/store/apps/dev?id=8493220400768769623&hl=ko&gl=KR");
#endif
    }
}
