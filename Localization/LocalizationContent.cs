using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationContent : MonoBehaviour
{
    private Text localizationText;
    public string localizationName = "";


    private void Awake()
    {
        localizationText = GetComponent<Text>();
    }

    private void Start()
    {
        if (localizationName.Length > 0) localizationText.text = LocalizationManager.instance.GetString(localizationName);

        if (LocalizationManager.instance != null) LocalizationManager.instance.AddContent(this);
    }

    public void ReLoad()
    {
        if (localizationName.Length > 0) localizationText.text = LocalizationManager.instance.GetString(localizationName);
    }
}