using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public GameObject optionView;
    public GameObject languageView;
    public GameObject deleteAccountView;

    public GameObject coupon;
    public GameObject deleteAccount;

    public OptionContent[] optionContents;

    public GameObject[] bottomContents;

    public Text versionText;

    public bool first = false;

    private void Awake()
    {
        optionView.SetActive(false);
        languageView.SetActive(false);
        deleteAccountView.SetActive(false);

        coupon.SetActive(false);
        deleteAccount.SetActive(false);
    }

    private void Start()
    {
        if(GameStateManager.instance.SleepMode)
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            Application.targetFrameRate = 45;
#elif UNITY_IOS
            Application.targetFrameRate = 60;
#endif
        }
        else
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            Application.targetFrameRate = 60;
#elif UNITY_IOS
            Application.targetFrameRate = 90;
#endif
        }
    }

    public void OpenOptionView()
    {
        if (!optionView.activeInHierarchy)
        {
            optionView.SetActive(true);

            if (!first)
            {
                PlayfabManager.instance.GetTitleInternalData("Coupon", CheckVersion);
                first = true;
            }

            Initialize();

            for (int i = 0; i < bottomContents.Length; i++)
            {
                bottomContents[i].SetActive(true);
            }
        }
        else
        {
            optionView.SetActive(false);
            languageView.SetActive(false);
        }
    }
    void CheckVersion(bool check)
    {
#if UNITY_EDITOR
        coupon.SetActive(true);
        deleteAccount.SetActive(true);
#elif UNITY_ANDROID
        coupon.SetActive(true);
        deleteAccount.SetActive(false);
#elif UNITY_IOS
        if(check)
        {
            coupon.SetActive(true);
            deleteAccount.SetActive(false);
        }
        else
        {
            coupon.SetActive(false);
            deleteAccount.SetActive(true);
        }
#endif
    }

    public void OpenOptionView_InGame()
    {
        if (!optionView.activeInHierarchy)
        {
            optionView.SetActive(true);

            Initialize();

            for(int i = 0; i < bottomContents.Length; i ++)
            {
                bottomContents[i].SetActive(false);
            }

            FirebaseAnalytics.LogEvent("Open_Option");
        }
        else
        {
            optionView.SetActive(false);
            languageView.SetActive(false);
        }
    }

    public void OpenLanguageView()
    {
        if (!languageView.activeInHierarchy)
        {
            languageView.SetActive(true);
        }
        else
        {
            languageView.SetActive(false);
        }
    }

    void Initialize()
    {
        versionText.text = "v" + Application.version;

        for(int i = 0; i < optionContents.Length; i ++)
        {
            optionContents[i].Initialize();
        }
    }

    public void OnButton(OptionType type)
    {
        switch (type)
        {
            case OptionType.Music:
                GameStateManager.instance.Music = true;

                SoundManager.instance.PlayBGM();
                break;
            case OptionType.Sfx:
                GameStateManager.instance.Sfx = true;
                break;
            case OptionType.Vibration:
                GameStateManager.instance.Vibration = true;
                break;
            case OptionType.SleepMode:
                GameStateManager.instance.SleepMode = true;

#if UNITY_EDITOR

#elif UNITY_ANDROID
            Application.targetFrameRate = 45;
#elif UNITY_IOS
            Application.targetFrameRate = 60;
#endif
                break;
        }
    }

    public void OffButton(OptionType type)
    {
        switch (type)
        {
            case OptionType.Music:
                GameStateManager.instance.Music = false;

                SoundManager.instance.StopBGM();
                break;
            case OptionType.Sfx:
                GameStateManager.instance.Sfx = false;
                break;
            case OptionType.Vibration:
                GameStateManager.instance.Vibration = false;
                break;
            case OptionType.SleepMode:
                GameStateManager.instance.SleepMode = false;

#if UNITY_EDITOR

#elif UNITY_ANDROID
            Application.targetFrameRate = 60;
#elif UNITY_IOS
            Application.targetFrameRate = 90;
#endif
                break;
        }
    }


    public void RestorePurchase()
    {
        if(PlayfabManager.instance.isActive) PlayfabManager.instance.RestorePurchases();

        FirebaseAnalytics.LogEvent("RestorePurchase");
    }

    public void OpenDeleteAccountView()
    {
        if (!deleteAccountView.activeInHierarchy)
        {
            deleteAccountView.SetActive(true);

            FirebaseAnalytics.LogEvent("DeleteAccount");
        }
        else
        {
            deleteAccountView.SetActive(false);
        }
    }

    public void DeleteAccount()
    {
        PlayfabManager.instance.LogOut();
    }

    public void BugReport()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/dev?id=8493220400768769623&hl=ko&gl=KR");
#elif UNITY_IOS
        Application.OpenURL("https://play.google.com/store/apps/dev?id=8493220400768769623&hl=ko&gl=KR");
#else
        Application.OpenURL("https://play.google.com/store/apps/dev?id=8493220400768769623&hl=ko&gl=KR");
#endif

        FirebaseAnalytics.LogEvent("Open_Feedback");
    }

    public void RateUs()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/dev?id=8493220400768769623&hl=ko&gl=KR");
#elif UNITY_IOS
        Application.OpenURL("https://play.google.com/store/apps/dev?id=8493220400768769623&hl=ko&gl=KR");
#else
        Application.OpenURL("https://play.google.com/store/apps/dev?id=8493220400768769623&hl=ko&gl=KR");
#endif

        FirebaseAnalytics.LogEvent("Open_RateUs");
    }
}
