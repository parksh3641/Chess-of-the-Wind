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
        numberText.color = new Color(35 / 255f, 141 / 255f, 241 / 255f);
    }

    public void Betting_Other()
    {
        if(betting == 0)
        {
            numberText.color = new Color(200 / 255f, 52 / 255f, 92 / 255f);
        }
        else
        {
            numberText.color = new Color(99 / 255f, 192 / 255f, 49 / 255f);
        }
    }

    public void FocusOn()
    {
        focus.SetActive(true);
    }
}
