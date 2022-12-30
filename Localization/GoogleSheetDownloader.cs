using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleSheetDownloader : MonoBehaviour
{
    const string LocalizationURL = "https://docs.google.com/spreadsheets/d/1nTQjgAQ631ayvzsWQeXt0PwTpneVV5sPs173vgpg05w/export?format=tsv&gid=0";
    const string BadWordURL = "https://docs.google.com/spreadsheets/d/1nTQjgAQ631ayvzsWQeXt0PwTpneVV5sPs173vgpg05w/export?format=tsv&gid=582114712";

    public bool isActive = false;
    public bool isLocalization = false;

    public float percent = 0;

    public Text messageText;
    public Image barFillAmount;
    public Text barPercentText;

    public LoginManager loginManager;

    LocalizationDataBase localizationDataBase;

    private void Awake()
    {
        Time.timeScale = 1;

        messageText.text = "";

        barFillAmount.fillAmount = 0f;
        barPercentText.text = "0%";

        if (localizationDataBase == null) localizationDataBase = Resources.Load("LocalizationDataBase") as LocalizationDataBase;

        if (!Directory.Exists(SystemPath.GetPath()))
        {
            Directory.CreateDirectory(SystemPath.GetPath());
        }

        StartCoroutine(LoadingCoroution());

        SyncFile();
    }

    [Button]
    void OnResetVersion()
    {
        PlayerPrefs.SetInt("Version", 0);
    }

    IEnumerator LoadingCoroution()
    {
        if(!isLocalization)
        {
            if(percent <= 0.25f)
            {
                percent += 0.01f;
                barFillAmount.fillAmount = percent;
                barPercentText.text = ((int)(percent * 100)).ToString() + "%";
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(LoadingCoroution());
        }
        else
        {
            messageText.text = LocalizationManager.instance.GetString("Downloading");
            yield return new WaitForSeconds(0.5f);
            messageText.text = LocalizationManager.instance.GetString("Downloading") + ".";
            yield return new WaitForSeconds(0.5f);
            messageText.text = LocalizationManager.instance.GetString("Downloading") + "..";
            yield return new WaitForSeconds(0.5f);
            messageText.text = LocalizationManager.instance.GetString("Downloading") + "...";
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(LoadingCoroution());
        }
    }

    IEnumerator DownloadFile()
    {
        int saveVersion = PlayerPrefs.GetInt("Version");
        int version = int.Parse(Application.version.Replace(".", "").ToString());

        if (saveVersion < version)
        {
            Debug.Log("Localization File Downloading...");
            UnityWebRequest www = UnityWebRequest.Get(LocalizationURL);
            yield return www.SendWebRequest();
            SetLocalization(www.downloadHandler.text);
            Debug.Log("Localization File Download Complete!");

            isLocalization = true;

            CheckPercent(25);

            CheckPercent(50);

            CheckPercent(75);

            PlayerPrefs.SetInt("Version", version);
        }
        else
        {
            if (!File.Exists(SystemPath.GetPath() + "Localization.txt"))
            {
                Debug.Log("Localization File Downloading...");

                UnityWebRequest www = UnityWebRequest.Get(LocalizationURL);
                yield return www.SendWebRequest();
                SetLocalization(www.downloadHandler.text);

                Debug.Log("Localization File Download Complete!");

                CheckPercent(25);

                PlayerPrefs.SetInt("Version", version);
            }
            else
            {
                StreamReader reader = new StreamReader(SystemPath.GetPath() + "Localization.txt");
                string value = reader.ReadToEnd();
                reader.Close();
                SetLocalization(value);
                Debug.Log("Localization File is exists");

                CheckPercent(25);
            }

            isLocalization = true;
        }

        if (!File.Exists(SystemPath.GetPath() + "BadWord.txt"))
        {
            Debug.Log("BadWord File Downloading...");
            UnityWebRequest www3 = UnityWebRequest.Get(BadWordURL);
            yield return www3.SendWebRequest();
            File.WriteAllText(SystemPath.GetPath() + "BadWord.txt", www3.downloadHandler.text);
            Debug.Log("BadWord File Download Complete!");
        }
        else
        {
            Debug.Log("BadWord File is exists");
        }

        CheckPercent(100);


        isActive = true;
    }

    void SetLocalization(string tsv)
    {
        File.WriteAllText(SystemPath.GetPath() + "Localization.txt", tsv);

        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        //int columnSize = row[0].Split('\t').Length;

        for (int i = 1; i < rowSize; i ++)
        {
            string[] column = row[i].Split('\t');
            LocalizationData content = new LocalizationData();

            content.key = column[0];
            content.korean = column[1].Replace('$','\n');
            content.english = column[2].Replace('$', '\n');
            content.japanese = column[3].Replace('$', '\n');
            content.chinese = column[4].Replace('$', '\n');
            content.indian = column[5].Replace('$', '\n');
            content.portuguese = column[6].Replace('$', '\n');
            content.russian = column[7].Replace('$', '\n');
            content.german = column[8].Replace('$', '\n');
            content.spanish = column[9].Replace('$', '\n');
            content.arabic = column[10].Replace('$', '\n');
            content.bengali = column[11].Replace('$', '\n');
            content.indonesian = column[12].Replace('$', '\n');
            content.italian = column[13].Replace('$', '\n');
            content.dutch = column[14].Replace('$', '\n');

            localizationDataBase.SetLocalization(content);
        }
    }

    void SyncFile()
    {
        if(NetworkConnect.instance.CheckConnectInternet())
        {
            isActive = false;

            localizationDataBase.Initialize();

            StartCoroutine(DownloadFile());
        }
        else
        {
            StopAllCoroutines();
            messageText.text = LocalizationManager.instance.GetString("NetworkConnectNotion");
            StartCoroutine(DelayCorution());
        }
    }

    IEnumerator DelayCorution()
    {
        yield return new WaitForSeconds(3f);
        SyncFile();
    }

    void CheckPercent(float number)
    {
        barFillAmount.fillAmount = number / 100.0f;
        barPercentText.text = number.ToString() + "%";
    }
}
