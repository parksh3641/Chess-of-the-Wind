using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterContent : MonoBehaviour
{
    public Image icon;
    public Text nickNameText;
    public GameObject myObj;

    public GameObject focus;

    private void Awake()
    {
        focus.SetActive(false);
    }

    public void Initialize(string name)
    {
        nickNameText.text = name;
    }

    public void CheckMy()
    {
        myObj.SetActive(true);
    }
}
