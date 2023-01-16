using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    public int index = 0;
    public Text numberText;
    public GameObject focus;

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

        if (number % 2 == 0)
        {
            numberText.text = "»Ú";
        }
        else
        {
            numberText.text = "∞À";
        }
    }

    public void Betting(bool check)
    {
        if(check)
        {
            numberText.color = Color.blue;
        }
        else
        {
            numberText.color = Color.black;
        }
    }

    public void FocusOn()
    {
        focus.SetActive(true);
    }
}
