using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationContent : MonoBehaviour
{
    private Text localizationText;
    public string localizationName = "";
    public string localizationName2 = "";
    public string plusText;


    private void Awake()
    {
        localizationText = GetComponent<Text>();
    }

    public string GetText()
    {
        return localizationText.text;
    }

    private void Start()
    {
        if (localizationName.Length > 0)
        {
            localizationText.text = LocalizationManager.instance.GetString(localizationName) + " " +
                LocalizationManager.instance.GetString(localizationName2) + plusText;
        }
        else
        {
            localizationText.text = "";
        }

        if (LocalizationManager.instance != null) LocalizationManager.instance.AddContent(this);
    }

    public void ReLoad()
    {
        if (localizationName.Length > 0)
        {
            localizationText.text = LocalizationManager.instance.GetString(localizationName) + " " +
                LocalizationManager.instance.GetString(localizationName2) + plusText;
        }
        else
        {
            localizationText.text = "";
        }
    }
}