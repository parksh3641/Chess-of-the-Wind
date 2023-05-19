using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

    public Font normalFont;

    public UnityEvent eChangeLanguage;

    public LocalizationDataBase localizationDataBase;

    public List<LocalizationContent> localizationContentList = new List<LocalizationContent>();


    private void Awake()
    {
        instance = this;

        if (localizationDataBase == null) localizationDataBase = Resources.Load("LocalizationDataBase") as LocalizationDataBase;

        localizationDataBase.Initialize();
        localizationContentList.Clear();

        if (!Directory.Exists(SystemPath.GetPath()))
        {
            Directory.CreateDirectory(SystemPath.GetPath());
        }

        StreamReader reader = new StreamReader(SystemPath.GetPath() + "Localization.txt");
        string value = reader.ReadToEnd();
        reader.Close();
        SetLocalization(value);
    }

    void SetLocalization(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        //int columnSize = row[0].Split('\t').Length;

        for (int i = 1; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            LocalizationData content = new LocalizationData();

            content.key = column[0];
            content.korean = column[1].Replace('$', '\n').Replace("^", "<color=#FF9600>").Replace("&", "<color=#C800FF>").Replace("*", "</color>");
            content.english = column[2].Replace('$', '\n').Replace("^", "<color=#FF9600>").Replace("&", "<color=#C800FF>").Replace("*", "</color>");
            content.japanese = column[3].Replace('$', '\n').Replace("^", "<color=#FF9600>").Replace("&", "<color=#C800FF>").Replace("*", "</color>");

            localizationDataBase.SetLocalization(content);
        }
    }

    public void AddContent(LocalizationContent content)
    {
        localizationContentList.Add(content);

        content.GetComponent<Text>().font = normalFont;
    }

    public string GetString(string name)
    {
        string str = "";

        foreach (var item in localizationDataBase.localizationDatas)
        {
            if(name.Equals(item.key))
            {
                switch (GameStateManager.instance.Language)
                {
                    case LanguageType.Korean:
                        str = item.korean;
                        break;
                    case LanguageType.English:
                        str = item.english;
                        break;
                    case LanguageType.Japanese:
                        str = item.japanese;
                        break;
                }
            }
        }

        if (str.Length == 0)
        {
            str = name;
        }

        return str;
    }

    public void ChangeKorean()
    {
        ChangeLanguage(LanguageType.Korean);

        eChangeLanguage.Invoke();
    }

    public void ChangeEnglish()
    {
        ChangeLanguage(LanguageType.English);

        eChangeLanguage.Invoke();
    }

    public void ChangeJapanese()
    {
        ChangeLanguage(LanguageType.Japanese);

        eChangeLanguage.Invoke();
    }

    public void ChangeLanguage(LanguageType type)
    {
        Debug.Log("Change Language : " + type);

        GameStateManager.instance.Language = type;

        string iso = "";

        switch (type)
        {
            case LanguageType.Default:
                break;
            case LanguageType.Korean:
                iso = "ko";
                break;
            case LanguageType.English:
                iso = "en";
                break;
            case LanguageType.Japanese:
                iso = "ja";
                break;
            default:
                iso = "en";
                break;
        }

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetProfileLanguage(iso);

        for(int i = 0; i < localizationContentList.Count; i ++)
        {
            localizationContentList[i].ReLoad();
        }
    }

    public void CloseLanguage()
    {
        eChangeLanguage.Invoke();
    }
}
