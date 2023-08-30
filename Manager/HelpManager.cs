using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{
    public GameObject helpView;


    public GameObject mainView;

    public GameObject newbieView;
    public GameObject rankView;
    public GameObject tacticsView;

    public RectTransform newbieTransform;
    public RectTransform rankTransform;
    public RectTransform tacticsTransform;

    public GameObject infoView;
    public Text infoTitleText;
    public Text infoText;

    private int index = 0;


    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        helpView.SetActive(false);

        mainView.SetActive(true);
        newbieView.SetActive(false);
        rankView.SetActive(false);
        tacticsView.SetActive(false);
        infoView.SetActive(false);

        newbieTransform.offsetMax = new Vector2(0, -999);
        rankTransform.offsetMax = new Vector2(0, -999);
        tacticsTransform.offsetMax = new Vector2(0, -999);
    }


    public void OpenHelpView()
    {
        if (!helpView.activeSelf)
        {
            helpView.SetActive(true);

            index = 0;

            mainView.SetActive(true);
            newbieView.SetActive(false);
            rankView.SetActive(false);
            tacticsView.SetActive(false);
            infoView.SetActive(false);

            FirebaseAnalytics.LogEvent("Help");
        }
        else
        {
            helpView.SetActive(false);
        }
    }

    public void OpenView(int number)
    {
        mainView.SetActive(false);
        newbieView.SetActive(false);
        rankView.SetActive(false);
        tacticsView.SetActive(false);

        index = number;

        switch (number)
        {
            case 0:
                newbieView.SetActive(true);
                break;
            case 1:
                rankView.SetActive(true);
                break;
            case 2:
                tacticsView.SetActive(true);
                break;
        }
    }

    public void OpenNewbieView(int number)
    {
        infoView.SetActive(true);
        newbieView.SetActive(false);

        switch (number)
        {
            case 0:
                infoTitleText.text = LocalizationManager.instance.GetString("Role1");
                infoText.text = LocalizationManager.instance.GetString("Role1_Info");
                break;
            case 1:
                infoTitleText.text = LocalizationManager.instance.GetString("Role2");
                infoText.text = LocalizationManager.instance.GetString("Role2_Info");
                break;
            case 2:
                infoTitleText.text = LocalizationManager.instance.GetString("Role3");
                infoText.text = LocalizationManager.instance.GetString("Role3_Info");
                break;
            case 3:
                infoTitleText.text = LocalizationManager.instance.GetString("Role4");
                infoText.text = LocalizationManager.instance.GetString("Role4_Info");
                break;
            case 4:
                infoTitleText.text = LocalizationManager.instance.GetString("Role5");
                infoText.text = LocalizationManager.instance.GetString("Role5_Info");
                break;
            case 5:
                infoTitleText.text = LocalizationManager.instance.GetString("Role6");
                infoText.text = LocalizationManager.instance.GetString("Role6_Info");
                break;
            case 6:
                infoTitleText.text = LocalizationManager.instance.GetString("Role7");
                infoText.text = LocalizationManager.instance.GetString("Role7_Info");
                break;
            case 7:
                infoTitleText.text = LocalizationManager.instance.GetString("Role8");
                infoText.text = LocalizationManager.instance.GetString("Role8_Info");
                break;
            case 8:
                infoTitleText.text = LocalizationManager.instance.GetString("Role9");
                infoText.text = LocalizationManager.instance.GetString("Role9_Info");
                break;
        }
    }

    public void OpenRankView(int number)
    {
        infoView.SetActive(true);
        rankView.SetActive(false);

        switch (number)
        {
            case 0:
                infoTitleText.text = LocalizationManager.instance.GetString("Gosu");
                infoText.text = LocalizationManager.instance.GetString("Role10_Info");
                break;
            case 1:
                infoTitleText.text = LocalizationManager.instance.GetString("Role2");
                infoText.text = LocalizationManager.instance.GetString("Role11_Info");
                break;
            case 2:
                infoTitleText.text = LocalizationManager.instance.GetString("Role3");
                infoText.text = LocalizationManager.instance.GetString("Role12_Info");
                break;
            case 3:
                infoTitleText.text = LocalizationManager.instance.GetString("Role4");
                infoText.text = LocalizationManager.instance.GetString("Role13_Info");
                break;
            case 4:
                infoTitleText.text = LocalizationManager.instance.GetString("Role5");
                infoText.text = LocalizationManager.instance.GetString("Role14_Info");
                break;
            case 5:
                infoTitleText.text = LocalizationManager.instance.GetString("Role6");
                infoText.text = LocalizationManager.instance.GetString("Role15_Info");
                break;
            case 6:
                infoTitleText.text = LocalizationManager.instance.GetString("Role8");
                infoText.text = LocalizationManager.instance.GetString("Role16_Info");
                break;
            case 7:
                infoTitleText.text = LocalizationManager.instance.GetString("Role9");
                infoText.text = LocalizationManager.instance.GetString("Role17_Info");
                break;
            case 8:
                infoTitleText.text = LocalizationManager.instance.GetString("Role10");
                infoText.text = LocalizationManager.instance.GetString("Role18_Info");
                break;
            case 9:
                infoTitleText.text = LocalizationManager.instance.GetString("AllowBlockLevel");
                infoText.text = LocalizationManager.instance.GetString("Role19_Info");
                break;
            case 10:
                infoTitleText.text = LocalizationManager.instance.GetString("Role11");
                infoText.text = LocalizationManager.instance.GetString("Role20_Info");
                break;
            case 11:
                infoTitleText.text = LocalizationManager.instance.GetString("Role12");
                infoText.text = LocalizationManager.instance.GetString("Role21_Info");
                break;
        }
    }

    public void OpenTacticsView(int number)
    {
        infoView.SetActive(true);
        tacticsView.SetActive(false);

        switch (number)
        {
            case 0:
                infoTitleText.text = LocalizationManager.instance.GetString("Role13");
                infoText.text = LocalizationManager.instance.GetString("Role22_Info");
                break;
            case 1:
                infoTitleText.text = LocalizationManager.instance.GetString("Role14");
                infoText.text = LocalizationManager.instance.GetString("Role23_Info");
                break;
            case 2:
                infoTitleText.text = LocalizationManager.instance.GetString("Role15");
                infoText.text = LocalizationManager.instance.GetString("Role24_Info");
                break;
            case 3:
                infoTitleText.text = LocalizationManager.instance.GetString("Role16");
                infoText.text = LocalizationManager.instance.GetString("Role25_Info");
                break;
            case 4:
                infoTitleText.text = LocalizationManager.instance.GetString("Role17");
                infoText.text = LocalizationManager.instance.GetString("Role26_Info");
                break;
            case 5:
                infoTitleText.text = LocalizationManager.instance.GetString("Role18");
                infoText.text = LocalizationManager.instance.GetString("Role27_Info");
                break;
            case 6:
                infoTitleText.text = LocalizationManager.instance.GetString("Role19");
                infoText.text = LocalizationManager.instance.GetString("Role28_Info");
                break;
            case 7:
                infoTitleText.text = LocalizationManager.instance.GetString("Role20");
                infoText.text = LocalizationManager.instance.GetString("Role29_Info");
                break;
            case 8:
                infoTitleText.text = LocalizationManager.instance.GetString("Role21");
                infoText.text = LocalizationManager.instance.GetString("Role30_Info");
                break;
            case 9:
                infoTitleText.text = LocalizationManager.instance.GetString("Role22");
                infoText.text = LocalizationManager.instance.GetString("Role31_Info");
                break;
            case 10:
                infoTitleText.text = LocalizationManager.instance.GetString("Role23");
                infoText.text = LocalizationManager.instance.GetString("Role32_Info");
                break;
            case 11:
                infoTitleText.text = LocalizationManager.instance.GetString("Role24");
                infoText.text = LocalizationManager.instance.GetString("Role33_Info");
                break;
        }
    }

    public void NewbieBackButton()
    {
        newbieView.SetActive(false);
        mainView.SetActive(true);
    }

    public void RankBackButton()
    {
        rankView.SetActive(false);
        mainView.SetActive(true);
    }

    public void TacticsBackButton()
    {
        tacticsView.SetActive(false);
        mainView.SetActive(true);
    }

    public void InfoBackButton()
    {
        infoView.SetActive(false);

        switch (index)
        {
            case 0:
                newbieView.SetActive(true);
                break;
            case 1:
                rankView.SetActive(true);
                break;
            case 2:
                tacticsView.SetActive(true);
                break;
        }
    }
}
