using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyAnimation : MonoBehaviour
{
    [Space]
    [Title("Target Pos")]
    public Transform moneyTransform;

    public Transform moneyStartTransform;
    public Transform otherMoneyStartTransform;

    [Space]
    [Title("Text")]
    public Text[] moneyText;

    [Space]
    [Title("Prefab")]
    public MoneyContent moneyPrefab;

    List<MoneyContent> moneyPrefabList = new List<MoneyContent>();

    private void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            MoneyContent monster = Instantiate(moneyPrefab);
            monster.transform.parent = moneyTransform;
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            moneyPrefabList.Add(monster);
        }
    }

    [Button]
    public void PlayAddMoneyAnimation()
    {
        AddMoneyAnimation(50000, 50000, 25000);
    }

    [Button]
    public void PlayMinusMoneyAnimation()
    {
        MinusMoneyAnimation(50000, 50000, 25000);
    }

    public void AddMoneyAnimation(int money, int otherMoney, int value) //내가 상대방 돈 가져감
    {
        for (int i = 0; i < moneyPrefabList.Count; i++)
        {
            moneyPrefabList[i].gameObject.SetActive(false);
        }

        StartCoroutine(AddMoneyCoroution(money, otherMoney, value, moneyPrefabList, moneyText));
    }

    public void MinusMoneyAnimation(int money, int otherMoney, int value) //상대방이 내 돈 가져감
    {
        for (int i = 0; i < moneyPrefabList.Count; i++)
        {
            moneyPrefabList[i].gameObject.SetActive(false);
        }

        StartCoroutine(MinusMoneyCoroution(money, otherMoney, value, moneyPrefabList, moneyText));
    }



    IEnumerator AddMoneyCoroution(int money, int otherMoney, int value, List<MoneyContent> list, Text[] text)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(true);
            list[i].GoToTarget(otherMoneyStartTransform.localPosition, moneyStartTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

        int max = money + value;

        while (money < max)
        {
            if(money + 1000 < max)
            {
                money += 1000;
                otherMoney -= 1000;
            }
            else
            {
                if (money + 100 < max)
                {
                    money += 100;
                    otherMoney -= 100;
                }
                else
                {
                    money += 1;
                    otherMoney -= 1;
                }
            }

            text[0].text = "GOLD  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
            text[1].text = "GOLD  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";

            yield return null;
        }
    }

    IEnumerator MinusMoneyCoroution(int money, int otherMoney, int value, List<MoneyContent> list, Text[] text)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(true);
            list[i].GoToTarget(moneyStartTransform.localPosition, otherMoneyStartTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

        int max = money - value;

        while (money > max)
        {
            if (money - 1000 > max)
            {
                money -= 1000;
                otherMoney += 1000;
            }
            else
            {
                if (money - 100 > max)
                {
                    money -= 100;
                    otherMoney += 100;
                }
                else
                {
                    money -= 1;
                    otherMoney += 1;
                }
            }

            text[0].text = "GOLD  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
            text[1].text = "GOLD  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";

            yield return null;
        }
    }
}