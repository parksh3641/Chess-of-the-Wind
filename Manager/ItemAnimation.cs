using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAnimation : MonoBehaviour
{
    public GameObject getObj;
    public Text getText;

    public Transform startTransform;
    public Transform endTransform;

    private int number = 0;

    public ItemContent prefab;

    public RectTransform prefabTransform;

    List<ItemContent> prefabList = new List<ItemContent>();

    PlayerDataBase playerDataBase;

    WaitForSeconds waitForSeconds = new WaitForSeconds(2.0f);


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        for (int i = 0; i < 15; i++)
        {
            ItemContent monster = Instantiate(prefab);
            monster.transform.SetParent(prefabTransform);
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = Vector3.one;
            monster.gameObject.SetActive(false);
            prefabList.Add(monster);
        }

        getObj.SetActive(false);
    }

    public void SetRank(RankType type)
    {
        for(int i = 0; i < prefabList.Count; i ++)
        {
            prefabList[i].SetRank(type);
        }
    }

    public void PlusItem(int target)
    {
        number = target;

        if (number >= 15)
        {
            number = 15;
        }

        StopAllCoroutines();

        getObj.SetActive(false);
        getObj.SetActive(true);
        if (getText != null)
        {
            getText.text = "+" + MoneyUnitString.ToCurrencyString(target);
        }

        StartCoroutine(PlusMoneyCoroution(target));
    }

    IEnumerator PlusMoneyCoroution(int target)
    {
        for (int i = 0; i < number; i++)
        {
            prefabList[i].gameObject.SetActive(true);
            prefabList[i].GoToTarget(startTransform.localPosition, endTransform.localPosition);
        }

        yield return waitForSeconds;

        getObj.gameObject.SetActive(false);
    }
}
