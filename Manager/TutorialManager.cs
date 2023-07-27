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

    public GameObject mainCanvasBackground;

    public Canvas mainCanvas;
    public Canvas gameCanvas;

    public GameObject bettingView;
    public GameObject rouletteView;

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

    bool myTurn = false;

    public GameObject homeButton;


    public NickNameManager nickNameManager;
    public FormationManager formationManager;

    public WindCharacterManager windCharacterManager;

    Sprite[] characterArray;

    WaitForSeconds talkDelay = new WaitForSeconds(0.04f);

    ImageDataBase imageDataBase;
    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        characterArray = imageDataBase.GetCharacterArray();

        mainCanvas.enabled = true;
        gameCanvas.enabled = false;

        mainCanvasBackground.SetActive(true);

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
    }

    void Start()
    {
        if (playerDataBase.Formation > 0) homeButton.SetActive(true);

        talkIndex = 0;

        Initialize(talkIndex);
    }

    void Initialize(int number)
    {
        nextText.enabled = false;

        talkSkip = false;

        Debug.Log(number);

        switch (number)
        {
            case 3:
                if (GameStateManager.instance.NickName.Length > 15)
                {
                    nickNameManager.OpenFreeNickName();
                }
                break;
            case 8:
                SetCharacter(0, 0);
                break;
            case 11:
                GameStart();
                break;
            case 12:
                vectorObj.SetActive(true);
                break;
            case 13:
                vectorObj.SetActive(false);

                bettingView.SetActive(true);
                rouletteView.SetActive(false);

                targetObj.SetActive(true);
                targetObj.transform.position = rouletteContentList[0].transform.position;

                break;
            case 14:
                moneyAnimation.MinusMoneyAnimation(40, 40, 20);

                talkBarTransform.transform.localPosition = new Vector3(0, -200);

                lpPointObj.SetActive(true);
                break;
            case 15:
                talkBarTransform.transform.localPosition = new Vector3(0, -540);

                lpPointObj.SetActive(false);
                break;
            case 17:
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
            case 18:
                mainCanvas.enabled = false;

                pinball.MyTurn(0);
                break;
            case 19:
                gameCanvas.enabled = false;
                mainCanvas.enabled = true;
                mainCanvasBackground.SetActive(true);

                SoundManager.instance.PlayBGM(GameBgmType.Story_Snow);
                break;
            case 20:
                InitCharacter();
                break;
            case 21:
                SetCharacter(1, 1);
                break;
            case 22:
                InitCharacter();
                break;
            case 24:
                SetCharacter(1, 1);
                break;
            case 26:
                InitCharacter();
                break;
            case 27:
                SetCharacter(1, 1);
                break;
            case 29:
                InitCharacter();
                break;
            case 30:
                SetCharacter(1, 1);
                break;
            case 31:
                InitCharacter();
                break;
            case 32:
                if(playerDataBase.Formation == 0)
                {
                    formationManager.Initialize();
                }
                else
                {
                    PlayerPrefs.SetString("LoadScene", "MainScene");
                    SceneManager.LoadScene("LoadScene");
                }
                break;
        }

        str = LocalizationManager.instance.GetString("Tutorial_" + (number + 1).ToString()).Replace("%%",
            GameStateManager.instance.NickName);

        StartCoroutine(Talking(str));

        if(number != 11 && number != 17 && number != 18 && number != 32)
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
        leftCharacter.enabled = false;
        rightCharacter.enabled = false;
        npcText.text = "";
    }

    public void SetCharacter(int vector, int number)
    {
        if(vector == 0)
        {
            leftCharacter.enabled = true;
            leftCharacter.sprite = characterArray[number];

            rightCharacter.enabled = false;
        }
        else
        {
            rightCharacter.enabled = true;
            rightCharacter.sprite = characterArray[number];

            leftCharacter.enabled = false;
        }

        if(number == 0)
        {
            npcText.text = LocalizationManager.instance.GetString("Npc_Winter");

            windCharacterType = WindCharacterType.Winter;
        }
        else
        {
            npcText.text = LocalizationManager.instance.GetString("Npc_Under");

            windCharacterType = WindCharacterType.UnderWorld;
        }
    }

    public void GameStart()
    {
        mainCanvas.enabled = false;
        gameCanvas.enabled = true;

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

        timer = 5;
        timerText.text = "";
        timerFillAmount.fillAmount = 1;
        StartCoroutine(TimerCoroution());
    }

    IEnumerator TimerCoroution()
    {
        while (timer > 0)
        {
            timer -= 1;

            timerFillAmount.fillAmount = timer / 4.0f;
            timerText.text = timer.ToString();

            if(timer < 3)
            {
                otherBlockContent.gameObject.SetActive(true);
                otherBlockContent.transform.position = rouletteContentList[0].transform.position;
                otherBlockContent.SetOtherBlock(BlockType.Pawn_Under, "", "0");
            }

            yield return waitForSeconds;
        }

        yield return new WaitForSeconds(1.0f);

        RouletteStart();
    }

    public void RouletteStart()
    {
        SoundManager.instance.PlayBGMLow();
        SoundManager.instance.PlayLoopSFX(GameSfxType.Roulette);

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
                windParticleArray[0].Play();

                pinball.BlowTargetBlow(blowTargetAi);

                break;
            }

            yield return null;
        }
    }

    public void EndBallAi()
    {
        if (talkIndex > 15) return;

        StartCoroutine(EndBallAiCoroution());
    }

    IEnumerator EndBallAiCoroution()
    {
        yield return new WaitForSeconds(3f);

        pointerManager.pointerList[0].FocusOn();

        targetView.SetActive(true);

        targetText.text = "1";

        yield return new WaitForSeconds(3f);

        mainCanvas.enabled = true;

        mainCanvasBackground.SetActive(false);

        SetCharacter(1, 0);

        SoundManager.instance.StopAllSFX();
        SoundManager.instance.PlayBGM();
    }

    public void SetBlockPos()
    {
        blockContent.transform.parent = blockParent;
        blockContent.transform.position = rouletteContentList[6].transform.position;
        blockContent.blockIcon.color = new Color(1, 1, 1, 1);
        blockContent.enabled = false;

        targetBlock.SetActive(false);

        SoundManager.instance.PlaySFX(GameSfxType.Click);

        Invoke("MyTurn", 1.5f);
    }

    void MyTurn()
    {
        SoundManager.instance.PlayBGMLow();
        SoundManager.instance.PlayLoopSFX(GameSfxType.Roulette);

        bettingView.SetActive(false);
        rouletteView.SetActive(true);

        targetView.SetActive(false);

        pinball.transform.localPosition = new Vector3(500, 500);

        vectorObj.SetActive(true);

        //pinball.MyTurn(0);

        pointerManager.Initialize(0);

        pointerManager.pointerList[0].Betting_Initialize();
        pointerManager.pointerList[5].Betting();

        mainCanvas.enabled = true;

        SetCharacter(1, 0);

        windButton.sprite = windButtonArray[1];

        StartCoroutine(MyTurnCoroution());
    }

    IEnumerator MyTurnCoroution()
    {
        while(true)
        {
            if(pinball.ballPos == 1)
            {
                pinball.Stop();

                mainCanvas.enabled = true;

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

        SoundManager.instance.PlaySFX(GameSfxType.GetNumber);

        yield return new WaitForSeconds(3f);

        bettingView.SetActive(true);
        rouletteView.SetActive(false);

        bottomTalkbar.SetActive(false);

        moneyAnimation.AddMoneyAnimation(20, 60, 30);

        SoundManager.instance.StopAllSFX();
        SoundManager.instance.PlayBGM();

        yield return new WaitForSeconds(4f);

        mainCanvas.enabled = true;

        SetCharacter(0, 0);

        talkIndex++;
        Initialize(talkIndex);

        talkBarTransform.transform.localPosition = new Vector3(0, -540);
    }

    public void GoToHome()
    {
        PlayerPrefs.SetString("LoadScene", "MainScene");
        SceneManager.LoadScene("LoadScene");
    }
}
