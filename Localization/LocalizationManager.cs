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

        localizationContentList.Clear();

        localizationDataBase.Initialize();

        TextAsset textAsset = Resources.Load<TextAsset>("Localization");
        SetLocalization(textAsset.ToString());
    }

    private void Start()
    {
        if (GameStateManager.instance.Language == LanguageType.Default)
        {
            if (Application.systemLanguage == SystemLanguage.Korean)
            {
                GameStateManager.instance.Language = LanguageType.Korean;

                ChangeKorean();
            }
            //else if (Application.systemLanguage == SystemLanguage.Japanese)
            //{
            //    GameStateManager.instance.Language = LanguageType.Japanese;

            //    ChangeJapanese();
            //}
            else if (Application.systemLanguage == SystemLanguage.Chinese)
            {
                GameStateManager.instance.Language = LanguageType.Chinese;

                ChangeChinese();
            }
            else if (Application.systemLanguage == SystemLanguage.Portuguese)
            {
                GameStateManager.instance.Language = LanguageType.Portuguese;

                ChangePortuguese();
            }
            else if (Application.systemLanguage == SystemLanguage.Russian)
            {
                GameStateManager.instance.Language = LanguageType.Russian;

                ChangeRussian();
            }
            else if (Application.systemLanguage == SystemLanguage.German)
            {
                GameStateManager.instance.Language = LanguageType.German;

                ChangeGerman();
            }
            else if (Application.systemLanguage == SystemLanguage.Spanish)
            {
                GameStateManager.instance.Language = LanguageType.Spanish;

                ChangeSpanish();
            }
            else if (Application.systemLanguage == SystemLanguage.Arabic)
            {
                GameStateManager.instance.Language = LanguageType.Arabic;

                ChangeArabic();
            }
            else if (Application.systemLanguage == SystemLanguage.Indonesian)
            {
                GameStateManager.instance.Language = LanguageType.Indonesian;

                ChangeIndonesian();
            }
            else if (Application.systemLanguage == SystemLanguage.Italian)
            {
                GameStateManager.instance.Language = LanguageType.Italian;

                ChangeItalian();
            }
            else if (Application.systemLanguage == SystemLanguage.Dutch)
            {
                GameStateManager.instance.Language = LanguageType.Dutch;

                ChangeDutch();
            }
            else if (Application.systemLanguage.ToString() == "Hindi")
            {
                GameStateManager.instance.Language = LanguageType.Indian;

                ChangeIndian();
            }
            else if (Application.systemLanguage == SystemLanguage.Vietnamese)
            {
                GameStateManager.instance.Language = LanguageType.Vietnamese;

                ChangeVietnamese();
            }
            //else if (Application.systemLanguage == SystemLanguage.Thai)
            //{
            //    GameStateManager.instance.Language = LanguageType.Thai;

            //    ChangeThai();
            //}
            else
            {
                GameStateManager.instance.Language = LanguageType.English;

                ChangeEnglish();
            }
        }
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

            //content.korean = column[1].Replace('$', '\n').Replace("^", "<color=#FF9600>").Replace("&", "<color=#C800FF>").Replace("*", "</color>");
            content.key = column[0];
            content.korean = column[1].Replace('#', '\n');
            content.english = column[2].Replace('#', '\n');
            content.japanese = column[3].Replace('#', '\n');
            content.chinese = column[4].Replace('#', '\n');
            content.indian = column[5].Replace('#', '\n');
            content.portuguese = column[6].Replace('#', '\n');
            content.russian = column[7].Replace('#', '\n');
            content.german = column[8].Replace('#', '\n');
            content.spanish = column[9].Replace('#', '\n');
            content.arabic = column[10].Replace('#', '\n');
            content.bengali = column[11].Replace('#', '\n');
            content.indonesian = column[12].Replace('#', '\n');
            content.italian = column[13].Replace('#', '\n');
            content.dutch = column[14].Replace('#', '\n');
            content.vietnamese = column[15].Replace('#', '\n');
            content.thai = column[16].Replace('#', '\n');

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
                    case LanguageType.Chinese:
                        str = item.chinese;
                        break;
                    case LanguageType.Indian:
                        str = item.indian;
                        break;
                    case LanguageType.Portuguese:
                        str = item.portuguese;
                        break;
                    case LanguageType.Russian:
                        str = item.russian;
                        break;
                    case LanguageType.German:
                        str = item.german;
                        break;
                    case LanguageType.Spanish:
                        str = item.spanish;
                        break;
                    case LanguageType.Arabic:
                        str = item.arabic;
                        break;
                    case LanguageType.Bengali:
                        str = item.bengali;
                        break;
                    case LanguageType.Indonesian:
                        str = item.indonesian;
                        break;
                    case LanguageType.Italian:
                        str = item.italian;
                        break;
                    case LanguageType.Dutch:
                        str = item.dutch;
                        break;
                    case LanguageType.Vietnamese:
                        str = item.vietnamese;
                        break;
                    case LanguageType.Thai:
                        str = item.thai;
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

    public void ChangeLanguage()
    {
        switch (GameStateManager.instance.Language)
        {
            case LanguageType.Korean:
                ChangeEnglish();
                break;
            case LanguageType.English:
                ChangeJapanese();
                break;
            case LanguageType.Japanese:
                ChangeChinese();
                break;
            case LanguageType.Chinese:
                ChangeIndian();
                break;
            case LanguageType.Indian:
                ChangePortuguese();
                break;
            case LanguageType.Portuguese:
                ChangeRussian();
                break;
            case LanguageType.Russian:
                ChangeGerman();
                break;
            case LanguageType.German:
                ChangeSpanish();
                break;
            case LanguageType.Spanish:
                ChangeArabic();
                break;
            case LanguageType.Arabic:
                ChangeBengali();
                break;
            case LanguageType.Bengali:
                ChangeIndonesian();
                break;
            case LanguageType.Indonesian:
                ChangeItalian();
                break;
            case LanguageType.Italian:
                ChangeDutch();
                break;
            case LanguageType.Dutch:
                ChangeVietnamese();
                break;
            case LanguageType.Vietnamese:
                ChangeThai();
                break;
            case LanguageType.Thai:
                ChangeKorean();
                break;
        }
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

    public void ChangeChinese()
    {
        ChangeLanguage(LanguageType.Chinese);
    }

    public void ChangeIndian()
    {
        ChangeLanguage(LanguageType.Indian);
    }

    public void ChangePortuguese()
    {
        ChangeLanguage(LanguageType.Portuguese);
    }

    public void ChangeRussian()
    {
        ChangeLanguage(LanguageType.Russian);
    }

    public void ChangeGerman()
    {
        ChangeLanguage(LanguageType.German);
    }

    public void ChangeSpanish()
    {
        ChangeLanguage(LanguageType.Spanish);
    }

    public void ChangeArabic()
    {
        ChangeLanguage(LanguageType.Arabic);
    }

    public void ChangeBengali()
    {
        ChangeLanguage(LanguageType.Bengali);
    }

    public void ChangeIndonesian()
    {
        ChangeLanguage(LanguageType.Indonesian);
    }

    public void ChangeItalian()
    {
        ChangeLanguage(LanguageType.Italian);
    }

    public void ChangeDutch()
    {
        ChangeLanguage(LanguageType.Dutch);
    }

    public void ChangeVietnamese()
    {
        ChangeLanguage(LanguageType.Vietnamese);
    }

    public void ChangeThai()
    {
        ChangeLanguage(LanguageType.Thai);
    }

    public void ChangeLanguage(LanguageType type)
    {
        if(GameStateManager.instance.Language == type)
        {
            return;
        }

        Debug.Log("Change Language : " + type);

        GameStateManager.instance.Language = type;

        string iso = "";

        switch (type)
        {
            case LanguageType.Korean:
                iso = "ko";
                break;
            //case LanguageType.English:
            //    iso = "en";
            //    break;
            //case LanguageType.Japanese:
            //    iso = "ja";
            //    break;
            default:
                iso = "en";
                break;
        }

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetProfileLanguage(iso);

        for(int i = 0; i < localizationContentList.Count; i ++)
        {
            localizationContentList[i].ReLoad();
        }

        SoundManager.instance.PlaySFX(GameSfxType.Success);
    }

    public void CloseLanguage()
    {
        eChangeLanguage.Invoke();
    }
}
