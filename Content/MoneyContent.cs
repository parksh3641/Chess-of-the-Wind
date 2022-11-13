using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyContent : MonoBehaviour
{
    public MoneyType moneyType = MoneyType.One;

    public GameObject main;
    public Text nameText;
    public Image backgroundFrame;

    GameManager gameManager;

    private int[] moneyArray = { 1, 2, 5, 10, 25, 50, 100, 500, 1000, 99999 };

    private void Awake()
    {
        main.transform.localScale = Vector3.one;
    }

    public void Initialize(GameManager manager, MoneyType type)
    {
        moneyType = type;
        gameManager = manager;

        switch (moneyType)
        {
            case MoneyType.One:
                nameText.text = "1";
                backgroundFrame.color = new Color(0, 1, 1);
                break;
            case MoneyType.Two:
                nameText.text = "2";
                backgroundFrame.color = new Color(1, 1, 0);
                break;
            case MoneyType.Three:
                nameText.text = "5";
                backgroundFrame.color = new Color(1, 0, 0);
                break;
            case MoneyType.Four:
                nameText.text = "10";
                backgroundFrame.color = new Color(0, 0, 1);
                break;
            case MoneyType.Five:
                nameText.text = "25";
                backgroundFrame.color = new Color(0, 1, 0);
                break;
            case MoneyType.Six:
                nameText.text = "50";
                backgroundFrame.color = new Color(1, 0, 1);
                break;
            case MoneyType.Seven:
                nameText.text = "100";
                backgroundFrame.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);
                break;
            case MoneyType.Eight:
                nameText.text = "500";
                backgroundFrame.color = new Color(150 / 255f, 0, 1);
                break;
            case MoneyType.Nine:
                nameText.text = "1K";
                backgroundFrame.color = new Color(150 / 255f, 50 / 255f, 150 / 255f);
                break;
        }
    }

    void SetBackgroundColor(int number)
    {
        int index = 0;

        for(int i = 0; i < moneyArray.Length; i ++)
        {
            if(number >= moneyArray[i + 1])
            {
                index++;
            }
            else
            {
                break;
            }
        }

        switch(index)
        {
            case 0:
                backgroundFrame.color = new Color(0, 1, 1);
                break;
            case 1:
                backgroundFrame.color = new Color(1, 1, 0);
                break;
            case 2:
                backgroundFrame.color = new Color(1, 0, 0);
                break;
            case 3:
                backgroundFrame.color = new Color(0, 0, 1);
                break;
            case 4:
                backgroundFrame.color = new Color(0, 1, 0);
                break;
            case 5:
                backgroundFrame.color = new Color(1, 0, 1);
                break;
            case 6:
                backgroundFrame.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);
                break;
            case 7:
                backgroundFrame.color = new Color(150 / 255f, 0, 1);
                break;
            case 8:
                backgroundFrame.color = new Color(150 / 255f, 50 / 255f, 150 / 255f);
                break;
        }
    }

    public void ChangeBetAnimation(bool check)
    {
        if(check)
        {
            main.transform.localScale = new Vector3(1.2f, 1.2f);
        }
        else
        {
            main.transform.localScale = Vector3.one;
        }
    }

    public void OnClick()
    {
        gameManager.ChangeBettingMoney(moneyType);
    }

    public void SetBettingMoney(int number)
    {
        nameText.text = number.ToString();

        SetBackgroundColor(number);
    }
}
