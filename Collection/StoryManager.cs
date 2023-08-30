using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public Text talkText;
    public Text nextText;

    public int talkIndex = 0;
    public int talkReplace = 0;

    public bool talkSkip = false;

    string str = "";

    WaitForSeconds talkDelay = new WaitForSeconds(0.04f);

    PlayerDataBase playerDataBase;

    void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        talkSkip = true;
    }

    public void NextButton()
    {
        if (!talkSkip)
        {
            talkSkip = true;

            SoundManager.instance.StopSFX(GameSfxType.TalkWinter);
            SoundManager.instance.StopSFX(GameSfxType.TalkUnder);
        }
        else
        {
            talkIndex++;
            Initialize_Talk();
        }
    }

    void Initialize_Talk()
    {
        nextText.enabled = false;

        talkSkip = false;

        str = LocalizationManager.instance.GetString("Tip_" + (Random.Range(0, 24).ToString()));
        StartCoroutine(Talking(str));

        if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
        {
            SoundManager.instance.StopSFX(GameSfxType.TalkWinter);
            SoundManager.instance.PlaySFX(GameSfxType.TalkWinter);
        }
        else
        {
            SoundManager.instance.StopSFX(GameSfxType.TalkUnder);
            SoundManager.instance.PlaySFX(GameSfxType.TalkUnder);
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

        while (!talkSkip && talkReplace < replaceTextStr.Length)
        {
            talkText.text += replaceTextStr[talkReplace];

            talkReplace++;

            yield return talkDelay;
        }

        talkText.text = talk;

        nextText.enabled = true;
    }
}
