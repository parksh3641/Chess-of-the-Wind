using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : MonoBehaviour
{
    public static RecordManager instance;

    public RecordContent recordContent;
    public RectTransform recordContentTransform;

    List<RecordContent> recordContentList = new List<RecordContent>();

    List<string> recordList = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        for(int i = 0; i < recordContentList.Count; i ++)
        {
            Destroy(recordContentList[i].gameObject);
        }

        recordContentList.Clear();

        recordList.Clear();
    }

    public void SetRecord(string text)
    {
        recordList.Add(text);
    }

    public void OpenRecord()
    {
        for (int i = 0; i < recordList.Count; i++)
        {
            RecordContent content = Instantiate(recordContent);
            content.transform.parent = recordContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize(i + 1 + "번째 턴 : " + recordList[i]);
            content.gameObject.SetActive(true);
            recordContentList.Add(content);
        }
    }
}
