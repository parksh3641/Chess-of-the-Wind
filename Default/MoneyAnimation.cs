﻿using Sirenix.OdinInspector;
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
    public Transform moneyMidTransform;
    public Transform otherMoneyMidTransform;

    [Space]
    [Title("Text")]
    public Text[] moneyText;
    public Text[] changeMoneyText;

    [Space]
    [Title("Prefab")]
    public MoneyContent moneyPrefab;

    List<MoneyContent> moneyPrefabList = new List<MoneyContent>();
    List<MoneyContent> moneyPrefabList2 = new List<MoneyContent>();

    public GameManager gameManager;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.03f);

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

        for (int i = 0; i < 10; i++)
        {
            MoneyContent monster = Instantiate(moneyPrefab);
            monster.transform.parent = moneyTransform;
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            moneyPrefabList2.Add(monster);
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

    public void Initialize()
    {
        changeMoneyText[0].text = "";
        changeMoneyText[1].text = "";
    }

    public void AddMoneyAnimation(int money, int otherMoney, int value)
    {
        for (int i = 0; i < moneyPrefabList.Count; i++)
        {
            moneyPrefabList[i].gameObject.SetActive(false);
        }

        changeMoneyText[0].text = "<color=#27FFFC>+" + Mathf.Abs(value) + "</color>";
        changeMoneyText[1].text = "<color=#FF712B>-" + Mathf.Abs(value) + "</color>";

        if(Random.Range(0,2) == 0)
        {
            SoundManager.instance.PlaySFX(GameSfxType.PlusMoney1);
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.PlusMoney2);
        }

        StartCoroutine(ChangeMoneyCoroution());

        StartCoroutine(AddMoneyCoroution(money, otherMoney, value, moneyPrefabList, moneyText));
    }

    public void MinusMoneyAnimation(int money, int otherMoney, int value)
    {
        for (int i = 0; i < moneyPrefabList.Count; i++)
        {
            moneyPrefabList[i].gameObject.SetActive(false);
        }

        changeMoneyText[0].text = "<color=#FF712B>-" + Mathf.Abs(value) + "</color>";
        changeMoneyText[1].text = "<color=#27FFFC>+" + Mathf.Abs(value) + "</color>";

        SoundManager.instance.PlaySFX(GameSfxType.MinusMoney);

        StartCoroutine(ChangeMoneyCoroution());

        StartCoroutine(MinusMoneyCoroution(money, otherMoney, value, moneyPrefabList, moneyText));
    }

    IEnumerator ChangeMoneyCoroution()
    {
        SoundManager.instance.PlaySFX(GameSfxType.ChangeMoney);

        yield return waitForSeconds;
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
            if (money + 10000000 < max)
            {
                money += 10000000;
                otherMoney -= 10000000;
            }
            else
            {
                if (money + 1000000 < max)
                {
                    money += 1000000;
                    otherMoney -= 1000000;
                }
                else
                {
                    if (money + 100000 < max)
                    {
                        money += 100000;
                        otherMoney -= 100000;
                    }
                    else
                    {
                        if (money + 10000 < max)
                        {
                            money += 10000;
                            otherMoney -= 10000;
                        }
                        else
                        {
                            if (money + 1000 < max)
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
                                    if (money + 10 < max)
                                    {
                                        money += 10;
                                        otherMoney -= 10;
                                    }
                                    else
                                    {
                                        money += 1;
                                        otherMoney -= 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            text[0].text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
            text[1].text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";

            yield return waitForSeconds2;
        }

        EndAnimation();
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
            if (money - 10000000 > max)
            {
                money -= 10000000;
                otherMoney += 10000000;
            }
            else
            {
                if (money - 1000000 > max)
                {
                    money -= 1000000;
                    otherMoney += 1000000;
                }
                else
                {
                    if (money - 100000 > max)
                    {
                        money -= 100000;
                        otherMoney += 100000;
                    }
                    else
                    {
                        if (money - 10000 > max)
                        {
                            money -= 10000;
                            otherMoney += 10000;
                        }
                        else
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
                                    if (money - 10 > max)
                                    {
                                        money -= 10;
                                        otherMoney += 10;
                                    }
                                    else
                                    {
                                        money -= 1;
                                        otherMoney += 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            text[0].text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
            text[1].text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";

            yield return waitForSeconds2;
        }

        EndAnimation();
    }

    public void MinusMoneyAnimationMid(int money, int bettingMoney, Text txt) //내 돈이 가운데로 이동되는 애니메이션
    {
        StartCoroutine(MinusMoneyMidCoroution(money, bettingMoney, txt));
    }

    public void MinusMoneyAnimationMidEnemy(int money, int bettingMoney, Text txt) //상대방 돈이 가운데로 이동되는 애니메이션
    {
        StartCoroutine(MinusMoneyMidEnemyCoroution(money, bettingMoney, txt));
    }

    IEnumerator MinusMoneyMidCoroution(int money, int bettingMoney, Text txt)
    {
        for (int i = 0; i < moneyPrefabList.Count; i++)
        {
            moneyPrefabList[i].gameObject.SetActive(true);
            moneyPrefabList[i].GoToTarget(moneyStartTransform.localPosition, moneyMidTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

        int max = money - bettingMoney;

        while (money > max)
        {
            if (money - 10000000 > max)
            {
                money -= 10000000;
            }
            else
            {
                if (money - 1000000 > max)
                {
                    money -= 1000000;
                }
                else
                {
                    if (money - 100000 > max)
                    {
                        money -= 100000;
                    }
                    else
                    {
                        if (money - 10000 > max)
                        {
                            money -= 10000;
                        }
                        else
                        {
                            if (money - 1000 > max)
                            {
                                money -= 1000;
                            }
                            else
                            {
                                if (money - 100 > max)
                                {
                                    money -= 100;
                                }
                                else
                                {
                                    if (money - 10 > max)
                                    {
                                        money -= 10;
                                    }
                                    else
                                    {
                                        money -= 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            txt.text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";

            yield return waitForSeconds2;
        }
    }


    IEnumerator MinusMoneyMidEnemyCoroution(int money, int bettingMoney, Text txt)
    {
        for (int i = 0; i < moneyPrefabList2.Count; i++)
        {
            moneyPrefabList2[i].gameObject.SetActive(true);
            moneyPrefabList2[i].GoToTarget(otherMoneyStartTransform.localPosition, otherMoneyMidTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

        int max = money - bettingMoney;

        while (money > max)
        {
            if (money - 10000000 > max)
            {
                money -= 10000000;
            }
            else
            {
                if (money - 1000000 > max)
                {
                    money -= 1000000;
                }
                else
                {
                    if (money - 100000 > max)
                    {
                        money -= 100000;
                    }
                    else
                    {
                        if (money - 10000 > max)
                        {
                            money -= 10000;
                        }
                        else
                        {
                            if (money - 1000 > max)
                            {
                                money -= 1000;
                            }
                            else
                            {
                                if (money - 100 > max)
                                {
                                    money -= 100;
                                }
                                else
                                {
                                    if (money - 10 > max)
                                    {
                                        money -= 10;
                                    }
                                    else
                                    {
                                        money -= 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            txt.text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";

            yield return waitForSeconds2;
        }
    }




    void EndAnimation()
    {
        StopAllCoroutines();

        changeMoneyText[0].text = "";
        changeMoneyText[1].text = "";

        if(gameManager != null) gameManager.CheckWinnerPlayer();
    }


    public void ResultAddMoney(int target, Text txt)
    {
        StartCoroutine(ResultAddMoneyCoroution(target, txt));
    }

    IEnumerator ResultAddMoneyCoroution(int target, Text txt)
    {
        int max = 0;

        while (max < target)
        {
            if (max + 1000000 < target)
            {
                max += 1000000;
            }
            else
            {
                if (max + 100000 < target)
                {
                    max += 100000;
                }
                else
                {
                    if (max + 10000 < target)
                    {
                        max += 10000;
                    }
                    else
                    {
                        if (max + 10000 < target)
                        {
                            max += 10000;
                        }
                        else
                        {
                            if (max + 1000 < target)
                            {
                                max += 1000;
                            }
                            else
                            {
                                if (max + 100 < target)
                                {
                                    max += 100;
                                }
                                else
                                {
                                    if (max + 10 < target)
                                    {
                                        max += 10;
                                    }
                                    else
                                    {
                                        max += 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            txt.text = "<color=#27FFFC>" + LocalizationManager.instance.GetString("AddMoney") + " : " + MoneyUnitString.ToCurrencyString(Mathf.Abs(max)) + "</color>";

            yield return waitForSeconds2;
        }

        txt.text = "<color=#27FFFC>" + LocalizationManager.instance.GetString("AddMoney") + " : " + MoneyUnitString.ToCurrencyString(Mathf.Abs(max)) + "</color>";
    }

    public void ResultMinusMoney(int target, Text txt)
    {
        StartCoroutine(ResultMinusMoneyCoroution(target, txt));
    }

    IEnumerator ResultMinusMoneyCoroution(int target, Text txt)
    {
        int max = 0;

        while (max > target)
        {
            if (max - 10000000 > target)
            {
                max -= 10000000;
            }
            else
            {
                if (max - 1000000 > target)
                {
                    max -= 1000000;
                }
                else
                {
                    if (max - 100000 > target)
                    {
                        max -= 100000;
                    }
                    else
                    {
                        if (max - 10000 > target)
                        {
                            max -= 10000;
                        }
                        else
                        {
                            if (max - 1000 > target)
                            {
                                max -= 1000;
                            }
                            else
                            {
                                if (max - 100 > target)
                                {
                                    max -= 100;
                                }
                                else
                                {
                                    if (max - 10 > target)
                                    {
                                        max -= 10;
                                    }
                                    else
                                    {
                                        max -= 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            txt.text = "<color=#FF712B>" + LocalizationManager.instance.GetString("MinusMoney") + " : " + MoneyUnitString.ToCurrencyString(Mathf.Abs(max)) + "</color>";

            yield return waitForSeconds2;
        }

        txt.text = "<color=#FF712B>" + LocalizationManager.instance.GetString("MinusMoney") + " : " + MoneyUnitString.ToCurrencyString(target) + "</color>";
    }
}