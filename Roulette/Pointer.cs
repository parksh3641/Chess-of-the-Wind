using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    public int index = 0;
    public Text numberText;
    public GameObject focus;

    public int betting = 0;

    public void Initialize(int number)
    {
        focus.SetActive(false);

        if (number > 24)
        {
            index = number - 24;
        }
        else
        {
            index = number;
        }

        numberText.text = index.ToString();
    }

    public void Betting_Initialize()
    {
        betting = 0;

        numberText.color = Color.black;
    }

    public void Betting()
    {
        betting = 1;
        numberText.color = new Color(0, 1, 1);
    }

    public void Betting_Other()
    {
        if(betting == 0)
        {
            numberText.color = Color.red;
        }
        else
        {
            numberText.color = Color.green;
        }
    }

    public void FocusOn()
    {
        focus.SetActive(true);
    }
}
