using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankInfoContent : MonoBehaviour
{
    Image background;

    public Image icon;

    public Text index;

    public Text rankNameText;

    public Text rankUpValue;

    public GameObject checkMark;

    private void Awake()
    {
        background = GetComponent<Image>();
    }

    public void Initialize(Sprite sp, int number, string name, int value)
    {
        icon.sprite = sp;
        index.text = number.ToString();
        rankNameText.text = name;
        rankUpValue.text = value.ToString();
    }

    public void SetBackground(bool check)
    {
        background.enabled = check;
    }

    public void CheckMy(bool check)
    {
        checkMark.SetActive(check);
    }
}
