using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberContent : MonoBehaviour
{
    public Text numberText;
    public LocalizationContent queenText;

    public void Initialize(int number)
    {
        queenText.enabled = false;

        if (number < 12)
        {
            numberText.text = (number + 1).ToString();
        }
        else if(number == 12)
        {
            numberText.enabled = false;
            queenText.enabled = true;
            queenText.localizationName = "Queen";
            //numberText.text = LocalizationManager.instance.GetString("Queen");
        }
        else if(number > 12)
        {
            numberText.text = number.ToString();
        }
    }

    public void Initialize_NewBie(int number)
    {
        queenText.enabled = false;

        if (number < 4)
        {
            numberText.text = (number + 1).ToString();
        }
        else if (number == 4)
        {
            numberText.enabled = false;
            queenText.enabled = true;
            queenText.localizationName = "Queen";
        }
        else if (number > 4)
        {
            numberText.text = number.ToString();
        }
    }

    public void Initialize()
    {
        numberText.color = Color.white;
    }

    public void Overlap()
    {
        numberText.color = new Color(99 / 255f, 192 / 255f, 49 / 255f);
    }
}
