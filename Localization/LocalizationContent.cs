using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationContent : MonoBehaviour
{
    public bool purchase = false;
    public bool rayCast = true;

    private Text localizationText;
    public string forwardText;
    public string localizationName = "";
    public string localizationName2 = "";
    public string plusText;


    private void Awake()
    {
        localizationText = GetComponent<Text>();

        localizationText.raycastTarget = rayCast;
    }

    public string GetText()
    {
        return localizationText.text;
    }

    public void SetColor(Color color)
    {
        localizationText.color = color;
    }

    private void Start()
    {
        ReLoad();

        if (LocalizationManager.instance != null) LocalizationManager.instance.AddContent(this);
    }

    public void ReLoad()
    {
        if (localizationName.Length > 0)
        {
            localizationText.text = "";

            if (forwardText.Length > 0)
            {
                localizationText.text = forwardText + " ";
            }

            if (!purchase)
            {
                localizationText.text += LocalizationManager.instance.GetString(localizationName);
            }
            else
            {
#if UNITY_IOS || UNITY_EDITOR_OSX
                localizationText.text += LocalizationManager.instance.GetString(localizationName + "_IOS");
#else
                localizationText.text += LocalizationManager.instance.GetString(localizationName + "_AOS");
#endif
            }

            if (localizationName2.Length > 0)
            {
                localizationText.text += " " + LocalizationManager.instance.GetString(localizationName2);
            }

            if(plusText.Length > 0)
            {
                localizationText.text += plusText;
            }

        }
        else
        {
            localizationText.text = "";
        }
    }
}