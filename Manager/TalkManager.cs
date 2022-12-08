using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    public static TalkManager instance;

    public TalkContent talkContent;
    public RectTransform talkContentTransform;

    List<TalkContent> talkContentList = new List<TalkContent>();


    private int talkIndex = 0;


    private void Awake()
    {
        instance = this;

        for (int i = 0; i < 20; i++)
        {
            TalkContent content = Instantiate(talkContent);
            content.transform.parent = talkContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize("");
            content.gameObject.SetActive(false);
            talkContentList.Add(content);
        }
    }

    public void UseNotion(string text, Color color)
    {
        if (talkIndex > talkContentList.Count - 1)
        {
            talkIndex = 0;
        }

        talkContentList[talkIndex].GetComponent<Text>().color = color;
        talkContentList[talkIndex].Initialize("Player1 ´ÔÀÌ " + text);
        talkContentList[talkIndex].gameObject.SetActive(true);
        talkContentList[talkIndex].transform.SetAsLastSibling();

        talkIndex++;
    }
}
