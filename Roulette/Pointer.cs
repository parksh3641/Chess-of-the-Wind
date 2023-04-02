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

    public void Initialize_NewBie(int number)
    {
        index = number;

        focus.SetActive(false);

        numberText.text = "";

        if (number % 2 == 0)
        {
            numberText.text = "흰";
        }
        else
        {
            numberText.text = "검";
        }
    }

    public void Betting_Newbie(int number)
    {
        if (number == 0)
        {
            numberText.color = Color.black;

            betting = 0;
        }
        else if (number == 1)
        {
            numberText.color = Color.green;
        }
        else if (number == 2)
        {
            numberText.color = new Color(0, 1, 1);
        }
        else
        {
            numberText.color = Color.red;
        }
    }

    public void Betting_Gosu()
    {
        betting = 1;
        numberText.color = new Color(0, 1, 1);
    }

    public void Betting_Gosu_Other()
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
