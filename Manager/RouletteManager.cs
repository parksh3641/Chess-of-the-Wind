using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    public Rotate_Sphere sphere;

    public Image powerFillAmount;


    public float power = 0;
    public float upPower = 0.05f;
    public float maxPower = 2f;

    private int targetNumber = 0;
    private int maxNumber = 0;

    public GameObject targetView;
    public Text targetText;




    bool click = false;
    bool start = false;
    bool check = false;


    public GameManager gameManager;

    private void Awake()
    {
        click = false;
    }

    public void Initialize(int number)
    {
        click = false;

        power = 0;
        powerFillAmount.fillAmount = 0;
        sphere.speed = 0;

        targetView.SetActive(false);

        maxNumber = number;
    }

    public void ButtonDown()
    {
        if (click) return;

        start = true;

        StartCoroutine(PowerCoroution());
    }

    public void ButtonUp()
    {
        if (click) return;

        start = false;
        click = true;

        StopAllCoroutines();
        StartCoroutine(RandomTargetNumber());
    }

    IEnumerator PowerCoroution()
    {
        if(!check)
        {
            if(power <= maxPower)
            {
                power += upPower;
            }
            else
            {
                check = true;
            }
        }
        else
        {
            if(power > 0)
            {
                power -= upPower;
            }
            else
            {
                check = false;
            }
        }

        powerFillAmount.fillAmount = power / maxPower;

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(PowerCoroution());
    }

    IEnumerator RandomTargetNumber()
    {
        sphere.SetSpeed(power);

        while (sphere.speed > 0) yield return null;

        yield return new WaitForSeconds(0.5f);

        targetNumber = Random.Range(1, maxNumber);

        targetView.SetActive(true);
        targetText.text = targetNumber.ToString();

        yield return new WaitForSeconds(3);

        gameManager.CloseRouletteView(targetNumber);
    }
}
