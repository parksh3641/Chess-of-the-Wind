using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerManager : MonoBehaviour
{
    public int maxNumber = 0;

    public Pinball2D pinball;
    public Pointer pointer;
    public Transform pointerTransform;

    public GameObject target;
    public GameObject targetPointer;

    public float speed = 100.0f;
    public float rotateSpeed = 3.59f;
    private int pointerIndex = 0;

    bool move = false;

    List<int> numberList = new List<int>();

    public List<Pointer> pointerList = new List<Pointer>();


    private void Awake()
    {
        CreateUnDuplicateRandom(maxNumber);

        for (int i = 0; i < maxNumber; i ++)
        {
            Pointer content = Instantiate(pointer);
            content.transform.parent = pointerTransform;
            content.transform.localScale = Vector3.one;
            content.transform.localPosition = new Vector3(0, 0, -0.5f);
            content.Initialize(numberList[i]);
            pointerList.Add(content);
        }

        numberList.Clear();
    }

    private void Start()
    {
        move = true;

        Initialize();
    }

    void Initialize()
    {
        StartCoroutine(DeploymentCoroution());
    }

    IEnumerator DeploymentCoroution()
    {
        if (pointerIndex < pointerList.Count)
        {
            pointerList[pointerIndex].transform.localPosition = new Vector3(float.Parse(targetPointer.transform.localPosition.x.ToString("0.00")),
                float.Parse(targetPointer.transform.localPosition.y.ToString("0.00")),
                float.Parse(targetPointer.transform.localPosition.z.ToString("0.00")));


            pointerIndex++;
        }
        else
        {
            move = false;

            targetPointer.gameObject.SetActive(false);

            yield break;
        }

        yield return new WaitForSeconds(rotateSpeed / (maxNumber + 1));
        StartCoroutine(DeploymentCoroution());
    }

    void CreateUnDuplicateRandom(int max)
    {
        int currentNumber = Random.Range(0, max);

        for (int i = 0; i < max;)
        {
            if (numberList.Contains(currentNumber))
            {
                currentNumber = Random.Range(0, max);
            }
            else
            {
                numberList.Add(currentNumber);
                i++;
            }
        }
    }

    public int CheckNumber()
    {
        int number = 0;
        float temp = 0;
        float dist = 0;

        for(int i = 0; i < pointerList.Count; i ++)
        {
            dist = Vector2.Distance(pinball.transform.position, pointerList[i].transform.position);

            //Debug.Log(i + "���� �Ÿ� : " + dist);

            if(temp == 0)
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

    float time = 0;

    private void Update()
    {
        if (move)
        {
            targetPointer.transform.RotateAround(target.transform.position + new Vector3(0, 0.08f, 0), Vector3.forward, speed * Time.deltaTime);
        }

        //if (targetPointer.transform.localPosition.x <= 0.02f && targetPointer.transform.localPosition.x >= -0.02f)
        //{
        //    Debug.LogError(time);

        //    time = 0;
        //}
        //else
        //{
        //    time += Time.deltaTime;
        //}
    }
}
