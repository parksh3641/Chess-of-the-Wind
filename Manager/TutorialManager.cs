using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialView;
    public GameObject bettingView;
    public GameObject rouletteView;



    public Image character;

    public Text npcText;
    public Text talkText;
    public Text nextText;

    public int talkIndex = 0;
    public int talkReplace = 0;

    public bool talkSkip = false;

    string str = "";

    public NickNameManager nickNameManager;
    public FormationManager formationManager;

    Sprite[] characterArray;

    WaitForSeconds talkDelay = new WaitForSeconds(0.04f);

    ImageDataBase imageDataBase;


    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        characterArray = imageDataBase.GetCharacterArray();

        tutorialView.SetActive(false);
    }

    void Start()
    {
        tutorialView.SetActive(true);

        talkIndex = 0;

        npcText.text = "";

        character.enabled = false;

        Initialize(talkIndex);
    }

    void Initialize(int number)
    {
        nextText.text = "";

        talkSkip = false;

        switch (number)
        {
            case 3:

                if(GameStateManager.instance.NickName.Length > 10)
                {
                    nickNameManager.OpenNickName();
                }
                break;
            case 8:
                SetCharacter(0);
                break;
            case 11:
                Debug.Log("인게임 진입");
                break;
            case 20:
                InitCharacter();
                break;
            case 21:
                SetCharacter(1);
                break;
            case 24:
                InitCharacter();
                break;
            case 25:
                SetCharacter(1);
                break;
            case 27:
                InitCharacter();
                break;
            case 29:
                formationManager.Initialize();
                break;
        }

        str = LocalizationManager.instance.GetString("Tutorial_" + (number + 1).ToString()).Replace("%%",
            "<color=#00C800>" + GameStateManager.instance.NickName + "</color>");

        StartCoroutine(Talking(str));
    }

    public void NextButton()
    {
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

        nextText.text = "▼";
    }

    void InitCharacter()
    {
        character.enabled = false;
        npcText.text = "";
    }

    public void SetCharacter(int number)
    {
        character.enabled = true;

        character.sprite = characterArray[number];

        if(number == 0)
        {
            npcText.text = LocalizationManager.instance.GetString("Npc_Winter");
        }
        else
        {
            npcText.text = LocalizationManager.instance.GetString("Npc_Under");
        }
    }
}
