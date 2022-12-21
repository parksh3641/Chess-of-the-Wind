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

        for (int i = 0; i < pointerList.Count; i ++)
        {
            pointerList[i].Initialize(i + 1);
        }
    }

    public int CheckNumber(Transform pinball)
    {
        int number = 0;
        float temp = 0;
        float dist = 0;

        for (int i = 0; i < pointerList.Count; i++)
        {
            dist = Vector3.Distance(pointerList[i].transform.position, pinball.transform.position);

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

}
