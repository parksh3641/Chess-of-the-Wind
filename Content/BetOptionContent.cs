using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetOptionContent : MonoBehaviour
{
    public BetOptionType betOptionType = BetOptionType.Double;

    public Text nameText;

    public GameManager gameManager;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        switch (betOptionType)
        {
            case BetOptionType.Double:
                nameText.text = "X2";
                break;
            case BetOptionType.Cancle:
                nameText.text = "취소";
                break;
            case BetOptionType.Repeat:
                nameText.text = "반복";
                break;
        }
    }

    public void OnClick()
    {
        gameManager.BetOptionButton(betOptionType);
    }
}
