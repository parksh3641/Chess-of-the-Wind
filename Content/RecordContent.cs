using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordContent : MonoBehaviour
{
    Text talkText;

    private void Awake()
    {
        talkText = GetComponent<Text>();
    }

    public void Initialize(string text)
    {
        talkText.text = text;
    }
}
