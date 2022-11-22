using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChildContent : MonoBehaviour
{
    public BlockChild[] blockChildArray;


    public void ResetNumber()
    {
        for(int i = 0; i < blockChildArray.Length; i ++)
        {
            blockChildArray[i].ResetNumber();
        }
    }

    public void SetNumber(int[] number)
    {
        for (int i = 0; i < number.Length; i++)
        {
            blockChildArray[i].numberText.text = number[i].ToString();
        }
    }
}
