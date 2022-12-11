using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    public int index = 0;
    public Text numberText;

    public void Initialize(int number)
    {
        index = number;
        numberText.text = index.ToString();
    }
}
