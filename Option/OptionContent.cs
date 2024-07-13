using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionContent : MonoBehaviour
{
    public OptionType optionType = OptionType.Music;

    public LocalizationContent titleText;

    public GameObject[] checkMarks;

    public OptionManager optionManager;

    public void Initialize()
    {
        titleText.localizationName = optionType.ToString();
        titleText.ReLoad();

        switch (optionType)
        {
            case OptionType.Music:
                ChangeState(GameStateManager.instance.Music);
                break;
            case OptionType.Sfx:
                ChangeState(GameStateManager.instance.Sfx);
                break;
            case OptionType.Vibration:
                ChangeState(GameStateManager.instance.Vibration);
                break;
            case OptionType.SleepMode:
                ChangeState(GameStateManager.instance.SleepMode);
                break;
            case OptionType.Graphics:
                ChangeState(GameStateManager.instance.Graphics);
                break;
        }
    }

    public void OnButton()
    {
        optionManager.OnButton(optionType);

        ChangeState(true);
    }

    public void OffButton()
    {
        optionManager.OffButton(optionType);

        ChangeState(false);
    }

    public void ChangeState(bool check)
    {
        checkMarks[0].SetActive(false);
        checkMarks[1].SetActive(false);

        if (check)
        {
            checkMarks[0].SetActive(true);
        }
        else
        {
            checkMarks[1].SetActive(true);
        }
    }
}
