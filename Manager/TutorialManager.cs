using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public WindCharacterType windCharacterType = WindCharacterType.Winter;

    public GameObject tutorialView;

    public Image mainCanvasBackground;

    public Sprite[] mainCanvasBackgroundArray;

    public Canvas mainCanvas;
    public Canvas gameCanvas;

    public GameObject bettingView;
    public GameObject rouletteView;

    public GameObject talkBar;

    public Image leftCharacter;
    public Image rightCharacter;

    public Transform talkBarTransform;

    public Text npcText;
    public Text talkText;
    public Text nextText;

    public int talkIndex = 0;
    public int talkReplace = 0;

    public bool talkSkip = false;

    string str = "";

    [Space]
    [Title("Betting")]
    public RouletteContent rouletteContent;
    public NumberContent numberContent;

    public RectTransform rouletteContentTransform;
    public RectTransform numberContentTransform;

    public Transform blockParent;
    public Transform blockGridParent;

    public Image playerImg;

    int index = 0;
    int count = 0;

    List<RouletteContent> rouletteContentList = new List<RouletteContent>();
    List<RouletteContent> numberContentList = new List<RouletteContent>();

    [Title("Timer")]
    private int timer = 0;
    public Text timerText;
    public Image timerFillAmount;

    private int bettingTime = 0;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

    [Title("Other")]
    public OtherBlockContent otherBlockContent;

    public Pinball3D pinball;

    public GameObject vectorObj_Enemy;
    public GameObject vectorObj;

    public PointerManager pointerManager;

    public ParticleSystem[] windParticleArray;

    public Transform blowTargetAi;

    public GameObject targetView;
    public GameObject targetEffect;
    public Text targetText;

    [Title("Betting")]
    public GameObject targetObj;

    public MoneyAnimation moneyAnimation;

    public GameObject lpPointObj;

    public BlockContent blockContent;

    public GameObject bottomTalkbar;
    public Text bottomTalkbarText;

    public GameObject targetBlock;

    public Transform blowTarget;

    public Image windButton;

    public Sprite[] windButtonArray;

    public GameObject windButtonTarget;

    public ParticleSystem rouletteParticle;

    bool myTurn = false;

    bool isTalkSound = false;

    public GameObject homeButton;


    public NickNameManager nickNameManager;
    public FormationManager formationManager;

    Sprite[] characterArray;

    WaitForSeconds talkDelay = new WaitForSeconds(0.04f);

    public FadeInOut fadeInOut;
    public FadeInOut fadeInOut2;

    ImageDataBase imageDataBase;
    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        characterArray = imageDataBase.GetCharacterArray();

        mainCanvas.enabled = true;
        gameCanvas.enabled = false;

        mainCanvasBackground.gameObject.SetActive(true);

        mainCanvasBackground.sprite = mainCanvasBackgroundArray[0];

        lpPointObj.SetActive(false);

        tutorialView.SetActive(false);

        index = 0;
        count = 0;

        for (int i = 0; i < 9; i++)
        {
            int[] setIndex = new int[2];

            if (index >= 3)
            {
                index = 0;
                count++;
            }

            setIndex[0] = index;
            setIndex[1] = count;

            RouletteContent content = Instantiate(rouletteContent);
            content.transform.parent = rouletteContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize_Tutorial(this, blockParent.transform, RouletteType.StraightBet, setIndex, i);
            rouletteContentList.Add(content);

            NumberContent numContent = Instantiate(numberContent);
            numContent.transform.parent = numberContentTransform;
            numContent.transform.localPosition = Vector3.zero;
            numContent.transform.localScale = Vector3.one;
            numContent.Initialize_NewBie(i);
            numberContentList.Add(content);

            index++;
        }

        targetObj.SetActive(false);

        targetView.SetActive(false);
        targetEffect.SetActive(false);

        tutorialView.SetActive(true);

        npcText.text = "";

        leftCharacter.enabled = false;
        rightCharacter.enabled = false;

        talkBarTransform.transform.localPosition = new Vector3(0, -540);

        bottomTalkbar.SetActive(false);

        targetBlock.SetActive(false);

        windButtonTarget.SetActive(false);

        homeButton.SetActive(false);

        rouletteParticle.gameObject.SetActive(false);

        playerImg.enabled = false;
    }

    void Start()
    {
        if (playerDataBase.Formation > 0) homeButton.SetActive(true);

        talkIndex = 0;

        fadeInOut.canvasGroup.alpha = 1;
        fadeInOut2.canvasGroup.alpha = 0;

        Initialize(talkIndex);
    }

    void Initialize(int number)
    {
        nextText.enabled = false;

        talkSkip = false;
        isTalkSound = false;

        Debug.Log(number);

        switch (number)
        {
            case 1:
                fadeInOut.FadeIn();
                break;
            case 3:
                if (GameStateManager.instance.NickName.Length > 15)
                {
                    nickNameManager.OpenFreeNickName();
                }
                break;
            case 8:
                SetCharacter(0, 0);
                break;
            case 9:
                InitCharacter();
                break;
            case 11:
                SetCharacter(0, 0);
                break;
            case 12:
                InitCharacter();
                break;
            case 15:
                UserNickName();
                break;
            case 16:
                InitCharacter();
                break;
            case 22:
                SetCharacter(0, 0);
                break;
            case 23:
                fadeInOut.StoryFadeOut();

                talkBar.SetActive(false);

                Invoke("GameStart", 1.0f);

                //GameStart();
                break;
            case 24:
                mainCanvas.enabled = false;
                StartCoroutine(TimerCoroution());
                break;
            case 25:
                mainCanvas.enabled = false;

                windParticleArray[0].Play();

                SoundManager.instance.PlaySFX(GameSfxType.BlowWind);

                pinball.BlowTargetBlow(blowTargetAi);
                break;
            case 26:
                mainCanvas.enabled = false;

                bettingView.SetActive(true);
                rouletteView.SetActive(false);

                targetObj.SetActive(true);
                targetObj.transform.position = rouletteContentList[0].transform.position;

                StartCoroutine(HitDamage());
                break;
            case 27:
                talkBarTransform.transform.localPosition = new Vector3(0, -540);

                lpPointObj.SetActive(false);

                SetCharacter(1, 0);
                break;
            case 28:
                SetCharacter(1, 0);
                break;
            case 29:
                UserNickName();
                break;
            case 30:
                SetCharacter(1, 0);
                break;
            case 31:
                mainCanvas.enabled = false;

                timerText.text = "";
                timerFillAmount.fillAmount = 1;

                targetObj.SetActive(false);

                otherBlockContent.gameObject.SetActive(false);

                blockContent.gameObject.SetActive(true);

                bottomTalkbar.SetActive(true);
                bottomTalkbarText.text = LocalizationManager.instance.GetString("Tutorial_Block");

                targetBlock.SetActive(true);
                targetBlock.transform.position = rouletteContentList[6].transform.position;
                break;
            case 32:
                SetCharacter(1, 0);
                break;
            case 33:
                SetCharacter(1, 0);
                break;
            case 34:
                SetCharacter(1, 0);
                break;
            case 35:
                mainCanvas.enabled = false;

                pinball.MyTurn(0);

                StartCoroutine(MyTurnCoroution());
                break;
            case 36:
                SetCharacter(1, 0);
                break;
            case 37:
                UserNickName();
                break;
            case 38:
                SetCharacter(1, 0);
                break;
            case 39:
                SetCharacter(1, 1);

                windCharacterType = WindCharacterType.UnderWorld;

                SoundManager.instance.PlayBGM(GameBgmType.Story_Under);
                break;
            case 40:
                rightCharacter.enabled = false;
                npcText.text = "";

                break;
            case 41:
                fadeInOut.StoryFadeOut();

                talkBar.SetActive(false);

                Invoke("GoToUnder", 1.0f);
                break;
            case 42:
                SetCharacter(1, 1);
                break;
            case 43:
                InitCharacter();
                break;
            case 45:
                SetCharacter(1, 1);
                break;
            case 46:
                SetCharacter(1, 1);
                break;
            case 47:
                SetCharacter(1, 1);
                break;
            case 48:
                UserNickName();
                break;
            case 49:
                SetCharacter(1, 1);
                break;
            case 50:
                UserNickName();
                break;
            case 51:
                SetCharacter(1, 1);
                break;
            case 52:
                UserNickName();
                break;
            case 53:
                SetCharacter(1, 1);
                break;
            case 54:
                UserNickName();
                break;
            case 55:
                formationManager.Initialize();
                break;
            case 60:
                fadeInOut.StoryFadeOut();

                talkBar.SetActive(false);

                Invoke("GoToHome", 1.0f);
                break;
        }

        if (talkIndex > 59) return;

        str = LocalizationManager.instance.GetString("Tutorial_" + (number + 1).ToString()).Replace("%%",
            GameStateManager.instance.NickName);

        StartCoroutine(Talking(str));

        if(number != 3 && number != 23 && number != 24 && number != 25 && number != 26 && number != 31 && number != 35 && number != 41 && number != 55)
        {
            if(!isTalkSound)
            {
                SoundManager.instance.PlaySFX(GameSfxType.TalkMy);
            }
            else
            {
                if (windCharacterType == WindCharacterType.Winter)
                {
                    SoundManager.instance.PlaySFX(GameSfxType.TalkWinter);
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.TalkUnder);
                }
            }
        }
    }

    public void NextButton()
    {
        if (myTurn) return;

        if(!talkSkip)
        {
            talkSkip = true;
        }
        else
        {
            talkIndex++;
            Initialize(talkIndex);
        }
    }

    IEnumerator Talking(string talk)
    {
        talkText.text = "";

        talkReplace = 0;

        string[] replaceTextStr = new string[talk.Length];

        for (int i = 0; i < replaceTextStr.Length; i++)
        {
            replaceTextStr[i] = talk.Substring(i, 1);
        }

        while(!talkSkip && talkReplace < replaceTextStr.Length)
        {
            talkText.text += replaceTextStr[talkReplace];

            talkReplace++;

            yield return talkDelay;
        }

        talkText.text = talk;

        nextText.enabled = true;
    }

    void InitCharacter()
    {
        leftCharacter.color = new Color(80 / 255f, 80 / 255f, 80 / 255f);
        rightCharacter.color = new Color(80 / 255f, 80 / 255f, 80 / 255f);
        npcText.text = "";
    }

    void UserNickName()
    {
        leftCharacter.color = Color.white;
        rightCharacter.color = Color.white;

        npcText.color = new Color(99 / 255f, 192 / 255f, 49 / 255f);
        npcText.text = GameStateManager.instance.NickName;
    }

    public void SetCharacter(int vector, int number)
    {
        isTalkSound = true;

        if (vector == 0)
        {
            leftCharacter.enabled = true;
            leftCharacter.color = Color.white;
            leftCharacter.sprite = characterArray[number];

            rightCharacter.enabled = false;

            npcText.color = new Color(35 / 255f, 141 / 255f, 241 / 255f);
        }
        else
        {
            rightCharacter.enabled = true;
            rightCharacter.color = Color.white;
            rightCharacter.sprite = characterArray[number];

            leftCharacter.enabled = false;

            npcText.color = new Color(200 / 255f, 52 / 255f, 92 / 255f);
        }

        if(number == 0)
        {
            npcText.text = LocalizationManager.instance.GetString("Npc_Winter");

            windCharacterType = WindCharacterType.Winter;

            npcText.color = new Color(35 / 255f, 141 / 255f, 241 / 255f);
        }
        else
        {
            npcText.text = LocalizationManager.instance.GetString("Npc_Under");

            windCharacterType = WindCharacterType.UnderWorld;

            npcText.color = new Color(200 / 255f, 52 / 255f, 92 / 255f);
        }
    }

    public void GameStart()
    {
        fadeInOut2.FadeIn();

        mainCanvas.enabled = false;
        gameCanvas.enabled = true;

        talkBar.SetActive(true);

        BlockClass blockClass = new BlockClass();

        blockClass.blockType = BlockType.Pawn_Snow;
        blockClass.rankType = RankType.N;
        blockClass.level = 0;

        blockContent.gameObject.SetActive(true);
        blockContent.Initialize(null, blockParent, blockGridParent);
        blockContent.InGame_Initialize(blockClass, 0, 0);
        blockContent.gameObject.SetActive(false);

        bettingView.SetActive(true);
        rouletteView.SetActive(false);

        SoundManager.instance.PlayBGM(GameBgmType.Game_Newbie);

        timer = 8;
        timerText.text = "";
        timerFillAmount.fillAmount = 1;
        StartCoroutine(TimerCoroution());
    }

    IEnumerator TimerCoroution()
    {
        while (timer > 0)
        {
            timer -= 1;

            timerFillAmount.fillAmount = timer / 7.0f;
            timerText.text = timer.ToString();

            if(timer < 5 && talkIndex == 23)
            {
                otherBlockContent.gameObject.SetActive(true);
                otherBlockContent.transform.position = rouletteContentList[0].transform.position;
                otherBlockContent.SetOtherBlock(BlockType.Pawn_Under, "", "0");

                ContinueTalk();

                mainCanvasBackground.gameObject.SetActive(false);

                SetCharacter(1, 0);

                break;
            }

            yield return waitForSeconds;
        }

        if(talkIndex == 24)
        {
            yield return new WaitForSeconds(1.0f);

            RouletteStart();
        }
    }

    public void RouletteStart()
    {
        SoundManager.instance.PlayBGMLow();
        SoundManager.instance.PlayLoopSFX(GameSfxType.Roulette);

        vectorObj_Enemy.SetActive(true);

        bettingView.SetActive(false);
        rouletteView.SetActive(true);

        pinball.MyTurn(0);

        pointerManager.Initialize(0);

        pointerManager.pointerList[0].Betting_Other();

        StartCoroutine(BlowWindAi());
    }

    IEnumerator BlowWindAi()
    {
        yield return new WaitForSeconds(3f);

        while(true)
        {
            if(pinball.ballPos == 4)
            {
                if(talkIndex == 24)
                {
                    pinball.rigid.isKinematic = true;

                    ContinueTalk();

                    break;
                }
                break;
            }

            yield return null;
        }
    }

    public void EndBallAi()
    {
        if (talkIndex == 35) return;

        StartCoroutine(EndBallAiCoroution());
    }

    IEnumerator EndBallAiCoroution()
    {
        yield return new WaitForSeconds(3f);

        pointerManager.pointerList[0].FocusOn();

        targetView.SetActive(true);

        targetText.text = "1";

        SoundManager.instance.PlaySFX(GameSfxType.Wrong);

        yield return new WaitForSeconds(3f);

        SoundManager.instance.StopAllSFX();
        SoundManager.instance.PlayBGM();

        ContinueTalk();

        vectorObj_Enemy.SetActive(false);
    }

    public void SetBlockPos()
    {
        blockContent.transform.parent = blockParent;
        blockContent.transform.position = rouletteContentList[6].transform.position;
        blockContent.blockIcon.color = new Color(1, 1, 1, 1);
        blockContent.enabled = false;

        targetBlock.SetActive(false);

        otherBlockContent.gameObject.SetActive(true);
        otherBlockContent.transform.position = rouletteContentList[7].transform.position;

        SoundManager.instance.PlaySFX(GameSfxType.Click);

        Invoke("MyTurn", 1.5f);
    }

    void MyTurn()
    {
        SoundManager.instance.PlayBGMLow();
        SoundManager.instance.PlayLoopSFX(GameSfxType.Roulette);

        SoundManager.instance.PlaySFX(GameSfxType.TalkWinter);

        playerImg.enabled = true;

        bettingView.SetActive(false);
        rouletteView.SetActive(true);

        targetView.SetActive(false);

        pinball.transform.localPosition = new Vector3(500, 500);

        vectorObj.SetActive(true);

        //pinball.MyTurn(0);

        pointerManager.Initialize(0);

        pointerManager.pointerList[0].Betting_Initialize();
        pointerManager.pointerList[5].Betting();
        pointerManager.pointerList[6].Betting_Other();

        ContinueTalk();

        SetCharacter(1, 0);

        windButton.sprite = windButtonArray[1];
    }

    IEnumerator MyTurnCoroution()
    {
        yield return new WaitForSeconds(7);

        while(true)
        {
            if(pinball.ballPos == 1)
            {
                pinball.Stop();

                ContinueTalk();

                talkBarTransform.transform.localPosition = new Vector3(0, -200);

                windButtonTarget.SetActive(true);

                myTurn = true;

                break;
            }

            yield return null;
        }
    }

    public void BlowWind()
    {
        if (!myTurn) return;

        myTurn = false;

        mainCanvas.enabled = false;

        windButtonTarget.SetActive(false);

        windParticleArray[1].Play();

        SoundManager.instance.PlaySFX(GameSfxType.BlowWind);

        pinball.BlowTargetBlow(blowTarget);

        StartCoroutine(BlowWindCoroution());
    }

    IEnumerator BlowWindCoroution()
    {
        yield return new WaitForSeconds(3f);

        pointerManager.pointerList[5].FocusOn();

        targetView.SetActive(true);
        targetEffect.SetActive(true);
        targetText.text = "6";

        rouletteParticle.gameObject.SetActive(true);
        rouletteParticle.Play();

        SoundManager.instance.PlaySFX(GameSfxType.GetNumber);

        yield return new WaitForSeconds(3f);

        targetObj.SetActive(true);
        targetObj.transform.position = rouletteContentList[6].transform.position;

        bettingView.SetActive(true);
        rouletteView.SetActive(false);

        bottomTalkbar.SetActive(false);

        moneyAnimation.AddMoneyAnimation(20, 60, 60);

        SoundManager.instance.StopAllSFX();
        SoundManager.instance.PlayBGM();

        if (Random.Range(0, 2) == 0)
        {
            SoundManager.instance.PlaySFX(GameSfxType.PlusMoney1);
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.PlusMoney2);
        }

        yield return new WaitForSeconds(4f);

        talkIndex++;
        Initialize(talkIndex);

        ContinueTalk();

        SetCharacter(1, 0);

        talkBarTransform.transform.localPosition = new Vector3(0, -540);
    }

    IEnumerator HitDamage()
    {
        moneyAnimation.MinusMoneyAnimation(40, 40, 20);

        yield return new WaitForSeconds(2f);

        ContinueTalk();

        talkBarTransform.transform.localPosition = new Vector3(0, -200);

        lpPointObj.SetActive(true);
    }

    void GoToUnder()
    {
        fadeInOut2.FadeIn();

        talkBar.SetActive(true);

        gameCanvas.enabled = false;

        ContinueTalk();

        mainCanvasBackground.gameObject.SetActive(true);

        SoundManager.instance.PlaySFX(GameSfxType.Bomb);

        mainCanvasBackground.sprite = mainCanvasBackgroundArray[1];
    }

    public void ChoiceFormation()
    {
        rightCharacter.enabled = false;
        npcText.text = "";

        if (playerDataBase.Formation == 1)
        {
            mainCanvasBackground.sprite = mainCanvasBackgroundArray[2];
        }
        else
        {
            mainCanvasBackground.sprite = mainCanvasBackgroundArray[3];
        }

        str = LocalizationManager.instance.GetString("Tutorial_" + (talkIndex + 1).ToString()).Replace("%%",
    GameStateManager.instance.NickName);

        StartCoroutine(Talking(str));

        SoundManager.instance.PlaySFX(GameSfxType.TalkMy);
    }

    public void ContinueTalk()
    {
        mainCanvas.enabled = true;

        str = LocalizationManager.instance.GetString("Tutorial_" + (talkIndex + 1).ToString()).Replace("%%",
            GameStateManager.instance.NickName);

        StartCoroutine(Talking(str));

        SoundManager.instance.PlaySFX(GameSfxType.TalkWinter);
    }

    public void GoToHome()
    {
        if (playerDataBase.Formation == 0)
        {
            formationManager.Initialize();
        }
        else
        {
            PlayerPrefs.SetString("LoadScene", "MainScene");
            SceneManager.LoadScene("LoadScene");
        }
    }
}
