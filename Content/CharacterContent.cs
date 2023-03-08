using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterContent : MonoBehaviour
{
    public Image icon;
    public Text nickNameText;
    public GameObject focus;

    public void Initialize(string name)
    {
        nickNameText.text = name;
    }
}
