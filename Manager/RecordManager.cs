using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : MonoBehaviour
{
    public static RecordManager instance;

    public RecordContent recordContent;
    public RectTransform gameRecordTransform;
    public RectTransform endRecordContentTransform;

    List<RecordContent> gameRecordContentList = new List<RecordContent>();
    List<RecordContent> endRecordContentList = new List<RecordContent>();

    List<string> recordList = new List<string>();

    private int recordIndex = 0;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < 10; i++)
        {
            RecordContent content = Instantiate(recordContent);
            content.transform.parent = gameRecordTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.Initialize("");
            content.gameObject.SetActive(false);
            gameRecordContentList.Add(content);
        }
    }

    public void Initialize()
    {
        recordIndex = 0;

        for(int i = 0; i < endRecordContentList.Count; i ++)
        {
            Destroy(endRecordContentList[i].gameObject);
        }

        for (int i = 0; i < gameRecordContentList.Count; i++)
        {
            gameRecordContentList[i].gameObject.SetActive(false);
        }

        endRecordContentList.Clear();

        recordList.Clear();
    }

    public void GameRecordInitialize()
    {
        recordIndex = 0;

        for (int i = 0; i < gameRecordContentList.Count; i++)
        {
            gameRecordContentList[i].gameObject.SetActive(false);
        }
    }

    public void SetRecord(string text)
    {
        recordList.Add(text);

        Debug.Log("게임 결과 기록 완료");
    }

    public void SetGameRecord(string text)
    {
        if (recordIndex > gameRecordContentList.Count - 1)
        {
            recordIndex = 0;
        }

        gameRecordContentList[recordIndex].Initialize(text);
        gameRecordContentList[recordIndex].gameObject.SetActive(true);
        gameRecordContentList[recordIndex].transform.SetAsLastSibling();

        recordIndex++;
    }

    public void OpenRecord()
    {
        StartCoroutine(OpenCoroution());
    }

    IEnumerator OpenCoroution()
    {
        for (int i = 0; i < recordList.Count; i++)
        {
            RecordContent content = Instantiate(recordContent);
            content.transform.parent = endRecordContentTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;

            if (int.Parse(recordList[i]) == 0)
            {
                content.Initialize(LocalizationManager.instance.GetString("Turn") + " " + (i + 1) + " : <color=#FFFFFF>+" + recordList[i] + "</color>");
            }
            else if (int.Parse(recordList[i]) > 0)
            {
                content.Initialize(LocalizationManager.instance.GetString("Turn") + " " + (i + 1) + " : <color=#27FFFC>+" + recordList[i] + "</color>");
            }
            else if (int.Parse(recordList[i]) < 0)
            {
                content.Initialize(LocalizationManager.instance.GetString("Turn") + " " + (i + 1) + " : <color=#FF712B>" + recordList[i] + "</color>");
            }
            content.gameObject.SetActive(true);
            endRecordContentList.Add(content);

            yield return waitForSeconds;
        }
    }
}
