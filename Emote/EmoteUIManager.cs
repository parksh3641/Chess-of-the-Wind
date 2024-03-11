using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteUIManager : MonoBehaviour
{
    public GameObject emoteUIView;

    public EmoteUIContent[] equipEmoteUIArray;

    public EmoteUIContent[] collectionEmoteUIArray;

    public LocalizationContent completedText;


    Sprite[] emoteArray;

    ImageDataBase imageDataBase;


    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        emoteArray = imageDataBase.GetEmoteArray();

        emoteUIView.SetActive(false);
    }

    public void OpenEmoteUIView()
    {
        if (!emoteUIView.activeSelf)
        {
            emoteUIView.SetActive(true);

            Initialize();

            FirebaseAnalytics.LogEvent("Open_Emote");
        }
        else
        {
            emoteUIView.SetActive(false);
        }
    }

    void Initialize()
    {
        completedText.localizationName = "Completed";
        completedText.plusText = " : 5 / 6";
        completedText.ReLoad();

        for(int i = 0; i < equipEmoteUIArray.Length; i ++)
        {
            equipEmoteUIArray[i].Initialize(i, emoteArray[i], this);
        }

        for (int i = 0; i < collectionEmoteUIArray.Length; i++)
        {
            collectionEmoteUIArray[i].Initialize(i, emoteArray[i], this);
        }

        collectionEmoteUIArray[5].EmoteLocked();
    }
}
