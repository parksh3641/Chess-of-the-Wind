using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeContent : MonoBehaviour
{
    public int index = 0;

    public LocalizationContent titleText;
    public LocalizationContent infoText;

    public ReceiveContent receiveContent;

    public GameObject foucsObj;
    public GameObject lockObj;
    public GameObject clearObj;

    public EventManager eventManager;

    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

    }

    public void Initialize()
    {
        titleText.localizationName = "Welcome" + (index + 1);
        titleText.ReLoad();

        infoText.localizationName = "Welcome" + (index + 1) + "_Info";

        foucsObj.SetActive(false);
        lockObj.SetActive(false);
        clearObj.SetActive(false);
    }

    public void ReceiveButton()
    {
        eventManager.WelcomeReceiveButton(index, SuccessReceive);
    }

    public void SuccessReceive()
    {
        foucsObj.SetActive(false);
        clearObj.SetActive(true);
    }
}
