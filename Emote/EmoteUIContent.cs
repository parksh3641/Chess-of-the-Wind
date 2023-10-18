using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteUIContent : MonoBehaviour
{
    public EmoteType emoteType = EmoteType.Emote1;

    public Image icon;

    public GameObject alarm;
    public GameObject selectedObj;
    public GameObject lockedObj;


    EmoteUIManager emoteUIManager;


    private void Awake()
    {
        alarm.SetActive(false);
        selectedObj.SetActive(false);
        lockedObj.SetActive(false);
    }


    public void Initialize(int number, Sprite sp, EmoteUIManager manager)
    {
        emoteType = EmoteType.Emote1 + number;

        icon.sprite = sp;

        emoteUIManager = manager;
    }

    public void EmoteSelected()
    {
        selectedObj.SetActive(true);
    }

    public void EmoteLocked()
    {
        lockedObj.SetActive(true);
    }
}
