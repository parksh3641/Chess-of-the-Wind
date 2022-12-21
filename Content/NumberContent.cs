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
            numberText.text = "��";
        }
        else if(number > 12)
        {
            numberText.text = number.ToString();
        }
    }
}
