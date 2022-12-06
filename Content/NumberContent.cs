using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberContent : MonoBehaviour
{
    public Text numberText;

    public void Initialize(int number)
    {
        numberText.text = (number + 1).ToString();
    }
}
