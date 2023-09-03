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

    public GameObject myPlusMoneyObj;
    public Text myPlusMoneyText;

    public Transform plusMoneyStartTransform;
    public Transform plusMoneyEndTransform;

    private int gold = 0;
    private int max = 0;
    private int maxOther = 0;
    private int Money = 0;
    private int OtherMoney = 0;

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

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.15f);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.03f);

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        for (int i = 0; i < 10; i++)
        {
            MoneyContent monster = Instantiate(heartPrefab);
            monster.transform.SetParent(heartTransform);
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            heartPrefabList.Add(monster);
        }

        for (int i = 0; i < 10; i++)
        {
            MoneyContent monster = Instantiate(heartPrefab);
            monster.transform.SetParent(heartTransform);
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            heartPrefabList_Enemy.Add(monster);
        }

        for (int i = 0; i < 10; i++)
        {
            MoneyContent monster = Instantiate(moneyPrefab);
            monster.transform.SetParent(moneyTransform);
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            moneyPrefabList.Add(monster);
        }

        if(plusMoneyView != null) plusMoneyView.gameObject.SetActive(false);
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

        changeMoneyText[0].text = "<color=#27FFFC>+" + MoneyUnitString.ToCurrencyString(Mathf.Abs(value)) + "</color>";
        changeMoneyText[1].text = "<color=#FF712B>-" + MoneyUnitString.ToCurrencyString(Mathf.Abs(value)) + "</color>";

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

        changeMoneyText[0].text = "<color=#FF712B>-" + MoneyUnitString.ToCurrencyString(Mathf.Abs(value)) + "</color>";
        changeMoneyText[1].text = "<color=#27FFFC>+" + MoneyUnitString.ToCurrencyString(Mathf.Abs(value)) + "</color>";

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
            yield return waitForSeconds;

            SoundManager.instance.StopSFX(GameSfxType.ChangeMoney);

            yield break;
        }
    }

    IEnumerator AddMoneyCoroution(int money, int otherMoney, int value, List<MoneyContent> list, Text[] text)
    {
        Money = money;
        OtherMoney = otherMoney;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(true);
            list[i].GoToTarget(otherMoneyStartTransform.localPosition, moneyStartTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

        isStart = true;
        StartCoroutine(ChangeMoneyCoroution());

        max = money + value;

        while (money < max)
        {
            if (money + 100000000 < max)
            {
                money += 100000000;
                otherMoney -= 100000000;
            }
            else
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
            }

            text[0].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
            text[1].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";

            yield return waitForSeconds2;
        }

        text[0].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(Money + value) + "</size>";
        text[1].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(OtherMoney - value) + "</size>";

        EndAnimation();
    }

    IEnumerator MinusMoneyCoroution(int money, int otherMoney, int value, List<MoneyContent> list, Text[] text)
    {
        Money = money;
        OtherMoney = otherMoney;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(true);
            list[i].GoToTarget(moneyStartTransform.localPosition, otherMoneyStartTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

        isStart = true;
        StartCoroutine(ChangeMoneyCoroution());

        max = money - value;

        while (money > max)
        {
            if (money - 100000000 > max)
            {
                money -= 100000000;
                otherMoney += 100000000;
            }
            else
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
            }

            text[0].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";
            text[1].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(otherMoney) + "</size>";

            yield return waitForSeconds2;
        }

        text[0].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(Money - value) + "</size>";
        text[1].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(OtherMoney + value) + "</size>";

        EndAnimation();
    }

    public void MinusMoneyAnimationMid(int money, int bettingMoney) //내 돈이 가운데로 이동되는 애니메이션
    {
        SoundManager.instance.PlaySFX(GameSfxType.MinusMoney);

        changeMoneyText[0].text = "<color=#FF712B>-" + MoneyUnitString.ToCurrencyString(bettingMoney) + "</color>";

        StartCoroutine(MinusMoneyMidCoroution(money, bettingMoney));
    }

    public void MinusMoneyAnimationMidEnemy(int money, int bettingMoney) //상대방 돈이 가운데로 이동되는 애니메이션
    {
        SoundManager.instance.PlaySFX(GameSfxType.MinusMoney);

        changeMoneyText[1].text = "<color=#FF712B>-" + MoneyUnitString.ToCurrencyString(bettingMoney) + "</color>";

        StartCoroutine(MinusMoneyMidEnemyCoroution(money, bettingMoney));
    }

    IEnumerator MinusMoneyMidCoroution(int money, int bettingMoney)
    {
        max = money - bettingMoney;

        for (int i = 0; i < heartPrefabList.Count; i++)
        {
            heartPrefabList[i].gameObject.SetActive(true);
            heartPrefabList[i].GoToTarget(moneyStartTransform.localPosition, moneyMidTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

        isStart = true;
        StartCoroutine(ChangeMoneyCoroution());

        while (money > max)
        {
            if (money - 100000000 > max)
            {
                money -= 100000000;
            }
            else
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
            }

            moneyText[0].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";

            yield return waitForSeconds2;
        }

        moneyText[0].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(max) + "</size>";

        EndAnimation();
    }


    IEnumerator MinusMoneyMidEnemyCoroution(int money, int bettingMoney)
    {
        maxOther = money - bettingMoney;

        for (int i = 0; i < heartPrefabList_Enemy.Count; i++)
        {
            heartPrefabList_Enemy[i].gameObject.SetActive(true);
            heartPrefabList_Enemy[i].GoToTarget(otherMoneyStartTransform.localPosition, otherMoneyMidTransform.localPosition);
        }

        yield return new WaitForSeconds(2.0f);

        isStart = true;
        StartCoroutine(ChangeMoneyCoroution());

        while (money > maxOther)
        {
            if (money - 100000000 > maxOther)
            {
                money -= 100000000;
            }
            else
            {
                if (money - 10000000 > maxOther)
                {
                    money -= 10000000;
                }
                else
                {
                    if (money - 1000000 > maxOther)
                    {
                        money -= 1000000;
                    }
                    else
                    {
                        if (money - 100000 > maxOther)
                        {
                            money -= 100000;
                        }
                        else
                        {
                            if (money - 10000 > maxOther)
                            {
                                money -= 10000;
                            }
                            else
                            {
                                if (money - 1000 > maxOther)
                                {
                                    money -= 1000;
                                }
                                else
                                {
                                    if (money - 100 > maxOther)
                                    {
                                        money -= 100;
                                    }
                                    else
                                    {
                                        if (money - 10 > maxOther)
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
            }

            moneyText[1].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(money) + "</size>";

            yield return waitForSeconds2;
        }

        moneyText[1].text = "Raf  <size=25>" + MoneyUnitString.ToCurrencyString(maxOther) + "</size>";

        EndAnimation();
    }




    void EndAnimation()
    {
        isStart = false;

        if (gameManager != null)
        {
            if (!gameManager.inputTargetNumber.gameObject.activeInHierarchy)
            {
                changeMoneyText[0].text = "";
                changeMoneyText[1].text = "";
            }

            gameManager.CheckWinnerPlayer();
        }
        else
        {
            changeMoneyText[0].text = "";
            changeMoneyText[1].text = "";
        }
    }


    public void ResultAddMoney(int target, Text txt)
    {
        StartCoroutine(ResultAddMoneyCoroution(target, txt));
    }

    IEnumerator ResultAddMoneyCoroution(int target, Text txt)
    {
        max = 0;

        while (max < target)
        {
            if (max + 100000000 < target)
            {
                max += 100000000;
            }
            else
            {
                if (max + 10000000 < target)
                {
                    max += 10000000;
                }
                else
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
        max = 0;

        while (max > target)
        {
            if (max - 100000000 > target)
            {
                max -= 100000000;
            }
            else
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
        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 1000);
    }

    public void PlusMoney(int target)
    {
        if (plusMoneyView.activeInHierarchy)
        {
            StopAllCoroutines();
        }

        plusMoneyView.SetActive(true);

        gold = playerDataBase.Gold;
        myMoneyText.text = MoneyUnitString.ToCurrencyString(gold);

        myPlusMoneyObj.SetActive(true);
        myPlusMoneyText.text = "+" + MoneyUnitString.ToCurrencyString(target);

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

        isStart = true;
        StartCoroutine(ChangeMoneyCoroution());

        max = 0;

        while (max < target)
        {
            if (max + 100000000 < target)
            {
                max += 100000000;
            }
            else
            {
                if (max + 10000000 < target)
                {
                    max += 10000000;
                }
                else
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
                }
            }

            myMoneyText.text = MoneyUnitString.ToCurrencyString(gold + max);

            yield return waitForSeconds2;
        }

        isStart = false;

        SoundManager.instance.StopSFX(GameSfxType.ChangeMoney);

        myMoneyText.text = MoneyUnitString.ToCurrencyString(gold + max);
        myPlusMoneyObj.SetActive(false);

        yield return new WaitForSeconds(1f);

        plusMoneyView.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        uIManager.EndResultGoldAnimation();
    }
}