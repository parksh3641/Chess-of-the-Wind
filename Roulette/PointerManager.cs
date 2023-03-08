using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerManager : MonoBehaviour
{
    public List<Pointer> pointerList = new List<Pointer>();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            pointerList.Add(child.GetComponent<Pointer>());
        }
    }

    public int CheckNumber(Transform target)
    {
        int number = 0;
        float temp = 0;
        float dist = 0;

        for (int i = 0; i < pointerList.Count; i++)
        {
            dist = Vector3.Distance(pointerList[i].transform.position, target.transform.position);

            //Debug.Log(i + 1 + "와의 거리 : " + dist);

            if (temp == 0)
            {
                temp = dist;
                number = pointerList[i].index;
            }
            else
            {
                if (temp > dist)
                {
                    temp = dist;
                    number = pointerList[i].index;
                }
            }
        }

        for(int i = 0; i < pointerList.Count; i ++)
        {
            if(pointerList[i].index == number)
            {
                pointerList[i].FocusOn();
                break;
            }
        }

        return number;
    }

    public int CheckQueenNumber(Transform target)
    {
        int number = 0;
        float temp = 0;
        float dist = 0;

        for (int i = 0; i < pointerList.Count; i++)
        {
            dist = Vector3.Distance(pointerList[i].transform.position, target.transform.position);

            //Debug.Log(i + 1 + "와의 거리 : " + dist);

            if (temp == 0)
            {
                temp = dist;
                number = pointerList[i].index;
            }
            else
            {
                if (temp > dist)
                {
                    temp = dist;
                    number = pointerList[i].index;
                }
            }
        }

        return number;
    }

    public void Initialize(int number)
    {
        for (int i = 0; i < pointerList.Count; i++)
        {
            pointerList[i].Initialize(number + i + 1);
        }
    }

    public void Initialize_NewBie()
    {
        for (int i = 0; i < pointerList.Count; i++)
        {
            pointerList[i].Initialize_NewBie(i);
        }
    }
}
