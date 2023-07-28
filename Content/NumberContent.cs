using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberContent : MonoBehaviour
{
    public Text numberText;

    public void Initialize(int number)
    {
        if(number < 12)
        {
            numberText.text = (number + 1).ToString();
        }
        else if(number == 12)
        {
            numberText.text = LocalizationManager.instance.GetString("Queen");
        }
        else if(number > 12)
        {
            numberText.text = number.ToString();
        }
    }

    public void Initialize_NewBie(int number)
    {
        if (number < 4)
        {
            numberText.text = (number + 1).ToString();
        }
        else if (number == 4)
        {
            numberText.text = LocalizationManager.instance.GetString("Queen");
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
