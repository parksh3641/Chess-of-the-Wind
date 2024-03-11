using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberContent : MonoBehaviour
{
    public Text numberText;
    public LocalizationContent queenText;

    public bool myBetting = false;
    public bool otherBetting = false;

    Color myBettingColor = new Color(35 / 255f, 141 / 255f, 241 / 255f);
    Color otherBettingColor = new Color(200 / 255f, 52 / 255f, 92 / 255f);
    Color allBettingColor = new Color(99 / 255f, 192 / 255f, 49 / 255f);

    public void Initialize(int number)
    {
        queenText.enabled = false;
        queenText.GetComponent<Text>().raycastTarget = false;

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
        queenText.GetComponent<Text>().raycastTarget = false;

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

    public void Betting_Initialize()
    {
        myBetting = false;
        otherBetting = false;
        CheckBetting();
    }

    public void Betting_My_Initialize()
    {
        myBetting = false;
        CheckBetting();
    }

    public void Betting_Other_Initialize()
    {
        otherBetting = false;
        CheckBetting();
    }

    public void Betting()
    {
        myBetting = true;
        CheckBetting();
    }

    public void Betting_Other()
    {
        otherBetting = true;
        CheckBetting();
    }

    void CheckBetting()
    {
        if(!myBetting && !otherBetting)
        {
            numberText.color = Color.white;

            if(queenText.gameObject.activeInHierarchy)
            {
                queenText.SetColor(Color.white);
            }
        }
        else if(myBetting && !otherBetting)
        {
            numberText.color = myBettingColor;

            if (queenText.gameObject.activeInHierarchy)
            {
                queenText.SetColor(myBettingColor);
            }
        }
        else if(!myBetting && otherBetting)
        {
            numberText.color = otherBettingColor;

            if (queenText.gameObject.activeInHierarchy)
            {
                queenText.SetColor(otherBettingColor);
            }
        }
        else
        {
            numberText.color = allBettingColor;

            if (queenText.gameObject.activeInHierarchy)
            {
                queenText.SetColor(allBettingColor);
            }
        }
    }
}
