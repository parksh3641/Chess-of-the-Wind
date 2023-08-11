using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyAnimation : MonoBehaviour
{
    [Space]
    [Title("Target Pos")]
    public Transform heartTransform;
    public Transform moneyTransform;

    public Transform moneyStartTransform;
    public Transform otherMoneyStartTransform;
    public Transform moneyMidTransform;
    public Transform otherMoneyMidTransform;

    [Space]
    [Title("Text")]
    public Text[] moneyText;
    public Text[] changeMoneyText;

    bool isStart = false;

    [Space]
    [Title("Plus")]
    public GameObject plusMoneyView;
    public Text myMoneyText;

    public Transform plusMoneyStartTransform;
    public Transform plusMoneyEndTransform;

    private int gold = 0;

    [Space]
    [Title("Prefab")]
    public MoneyContent heartPrefab;
    public MoneyContent moneyPrefab;

    List<MoneyContent> heartPrefabList = new List<MoneyContent>();
    List<MoneyContent> heartPrefabList_Enemy = new List<MoneyContent>();

    List<MoneyContent> moneyPrefabList = new List<MoneyContent>();

    public GameManager gameManager;
    public UIManager uIManager;

    PlayerDataBase playerDataBase;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.03f);

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        for (int i = 0; i < 10; i++)
        {
            MoneyContent monster = Instantiate(heartPrefab);
            monster.transform.parent = heartTransform;
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            heartPrefabList.Add(monster);
        }

        for (int i = 0; i < 10; i++)
        {
            MoneyContent monster = Instantiate(heartPrefab);
            monster.transform.parent = heartTransform;
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            heartPrefabList_Enemy.Add(monster);
        }

        for (int i = 0; i < 10; i++)
        {
            MoneyContent monster = Instantiate(moneyPrefab);
            monster.transform.parent = moneyTransform;
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            moneyPrefabList.Add(monster);
        }

        plusMoneyView.gameObject.SetActive(false);
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

        isStart = false;

        StopAllCoroutines();
    }

    public void AddMoneyAnimation(int money, int otherMoney, int value)
    {
        for (int i = 0; i < heartPrefabList.Count; i++)
        {
            heartPrefabList[i].gameObject.SetActive(false);
        }

        changeMoneyText[0].text = "<color=#27FFFC>+" + Mathf.Abs(value) + "</color>";
        changeMoneyText[1].text = "<color=#FF712B>-" + Mathf.Abs(value) + "</color>";

        if (Random.Range(0, 2) == 0)
        {
            SoundManager.instance.PlaySFX(GameSfxType.PlusMoney1);
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.PlusMoney2);
        }

        StartCoroutine(AddMoneyCoroution(money, otherMoney, value, heartPrefabList, moneyText));
    }

    public void MinusMoneyAnimation(int money, int otherMoney, int value)
    {
        for (int i = 0; i < heartPrefabList.Count; i++)
        {
            heartPrefabList[i].gameObject.SetActive(false);
        }

        changeMoneyText[0].text = "<color=#FF712B>-" + Mathf.Abs(value) + "</color>";
        changeMoneyText[1].text = "<color=#27FFFC>+" + Mathf.Abs(value) + "</color>";

        SoundManager.instance.PlaySFX(GameSfxType.MinusMoney);

        StartCoroutine(MinusMoneyCoroution(money, otherMoney, value, heartPrefabList, moneyText));
    }

    IEnumerator ChangeMoneyCoroution()
    {
        if (isStart)
        {
            SoundManager.instance.PlaySFX(GameSfxType.ChangeMoney);

            yield return waitForSeconds;

            StartCoroutine(ChangeMoneyCoroution());

        }
        else
        {
            SoundManager.instance.StopSFX(GameSfxType.ChangeMoney);

            yield break;
        }
    }

    IEnumerator AddMoneyCoroution(int money, int otherMoney, int value, List<MoneyContent> list, Text[] text)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(true);
            list[i].GoToTarget(otherMoneyStartTransform.localPosition, moneyStartTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

        isStart = true;
        StartCoroutine(ChangeMoneyCoroution());

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

        isStart = true;
        StartCoroutine(ChangeMoneyCoroution());

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
        SoundManager.instance.PlaySFX(GameSfxType.MinusMoney);

        StartCoroutine(MinusMoneyMidCoroution(money, bettingMoney, txt));
    }

    public void MinusMoneyAnimationMidEnemy(int money, int bettingMoney, Text txt) //상대방 돈이 가운데로 이동되는 애니메이션
    {
        SoundManager.instance.PlaySFX(GameSfxType.MinusMoney);

        StartCoroutine(MinusMoneyMidEnemyCoroution(money, bettingMoney, txt));
    }

    IEnumerator MinusMoneyMidCoroution(int money, int bettingMoney, Text txt)
    {
        for (int i = 0; i < heartPrefabList.Count; i++)
        {
            heartPrefabList[i].gameObject.SetActive(true);
            heartPrefabList[i].GoToTarget(moneyStartTransform.localPosition, moneyMidTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

        isStart = true;
        StartCoroutine(ChangeMoneyCoroution());

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

        txt.text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";

        EndAnimation();
    }


    IEnumerator MinusMoneyMidEnemyCoroution(int money, int bettingMoney, Text txt)
    {
        for (int i = 0; i < heartPrefabList_Enemy.Count; i++)
        {
            heartPrefabList_Enemy[i].gameObject.SetActive(true);
            heartPrefabList_Enemy[i].GoToTarget(otherMoneyStartTransform.localPosition, otherMoneyMidTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

        isStart = true;
        StartCoroutine(ChangeMoneyCoroution());

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

        txt.text = "LP  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";

        EndAnimation();
    }




    void EndAnimation()
    {
        StopAllCoroutines();

        isStart = false;

        changeMoneyText[0].text = "";
        changeMoneyText[1].text = "";

        if (gameManager != null) gameManager.CheckWinnerPlayer();
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

        yield return new WaitForSeconds(0.5f);

        PlusMoney(Mathf.Abs(max));
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

            txt.text = "<color=#FF712B>" + LocalizationManager.instance.GetString("MinusMoney") + " : " + MoneyUnitString.ToCurrencyString(max) + "</color>";

            yield return waitForSeconds2;
        }

        txt.text = "<color=#FF712B>" + LocalizationManager.instance.GetString("MinusMoney") + " : " + MoneyUnitString.ToCurrencyString(max) + "</color>";

        uIManager.EndResultGoldAnimation();
    }

    [Button]
    void PlusMoney()
    {
        PlusMoney(1000);
    }

    public void PlusMoney(int target)
    {
        if (plusMoneyView.activeInHierarchy) return;

        plusMoneyView.SetActive(true);

        gold = playerDataBase.Gold;
        myMoneyText.text = MoneyUnitString.ToCurrencyString(gold);

        StartCoroutine(PlusMoneyCoroution(target));
    }

    IEnumerator PlusMoneyCoroution(int target)
    {
        for (int i = 0; i < heartPrefabList_Enemy.Count; i++)
        {
            moneyPrefabList[i].gameObject.SetActive(true);
            moneyPrefabList[i].GoToTarget(plusMoneyStartTransform.localPosition, plusMoneyEndTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

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

            myMoneyText.text = MoneyUnitString.ToCurrencyString(gold + max);

            yield return waitForSeconds2;
        }

        myMoneyText.text = MoneyUnitString.ToCurrencyString(gold + max);

        yield return new WaitForSeconds(0.5f);

        plusMoneyView.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        uIManager.EndResultGoldAnimation();
    }
}