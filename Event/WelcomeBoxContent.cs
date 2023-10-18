using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeBoxContent : MonoBehaviour
{
    public int index = 0;

    public LocalizationContent titleText;

    public ReceiveContent receiveContent;

    public GameObject lockObj;
    public GameObject clearObj;


    public EventManager eventManager;


    public void Initialize(int number, bool check, EventManager manager)
    {
        eventManager = manager;

        titleText.localizationName = (index + 1) + "Day";
        titleText.ReLoad();

        clearObj.SetActive(false);

        if (index == number)
        {
            if (!check)
            {
                lockObj.SetActive(false);
            }
            else
            {
                lockObj.SetActive(true);
            }
        }
        else
        {
            if (index > number)
            {
                lockObj.SetActive(true);
            }
            else
            {
                clearObj.SetActive(true);
            }
        }
    }

    public void ReceiveButton()
    {
        eventManager.WelcomeBoxReceiveButton(SuccessReceive);
    }

    public void SuccessReceive()
    {
        clearObj.SetActive(true);
    }
}
