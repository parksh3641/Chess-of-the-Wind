using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    public Pinball2D pinball;
    public Rotation_Roulette roulette;

    public GameObject bounsView;

    public int bounsCount = 2;

    private int targetNumber = 0;
    private int maxNumber = 0;

    public GameObject targetView;
    public Text targetText;
    public Text titleText;
    public Text bounsText;




    bool click = false;
    bool movePinball = false;
    bool bouns = false;

    float time = 0;


    public GameManager gameManager;
    public PointerManager pointerManager;
    public SoundManager soundManager;

    private void Awake()
    {
        click = false;

        bounsView.SetActive(false);
    }

    private void Update()
    {
        if (movePinball)
        {
            if(pinball.speed <= 0)
            {
                StartCoroutine(RandomTargetNumber());
            }

            if(pinball.wind && pinball.rigid.velocity.x == 0)
            {
                StartCoroutine(RandomTargetNumber());
            }
        }

        if(bouns)
        {
            if(roulette.speed <= 0)
            {
                StartCoroutine(BounsRoulette());
            }
        }
    }
    public void Initialize(int number)
    {
        click = false;
        movePinball = false;
        bouns = false;

        time = 0;

        targetView.SetActive(false);
        bounsView.SetActive(false);

        maxNumber = number;

        if(bounsCount > 0)
        {
            movePinball = true;

            pinball.StartRotate();

            titleText.text = "숫자 룰렛";

            bounsText.text = "보너스 룰렛까지 남은 턴 : " + bounsCount.ToString();
        }
        else
        {
            click = true;

            bouns = true;

            titleText.text = "보너스 룰렛";

            bounsView.SetActive(true);

            roulette.StartRoulette();
        }


        soundManager.PlayLoopSFX(GameSfxType.Roulette);
    }

    public void BlowWind()
    {
        if (click) return;

        click = true;

        pinball.StartPinball();
    }

    //IEnumerator PowerCoroution()
    //{
    //    if (start)
    //    {
    //        if (!check)
    //        {
    //            if (power <= maxPower)
    //            {
    //                power += upPower;
    //            }
    //            else
    //            {
    //                check = true;
    //            }
    //        }
    //        else
    //        {
    //            if (power > 0.1f)
    //            {
    //                power -= upPower;
    //            }
    //            else
    //            {
    //                check = false;
    //            }
    //        }

    //        powerFillAmount.fillAmount = power / maxPower;

    //        yield return new WaitForSeconds(0.01f);
    //        StartCoroutine(PowerCoroution());
    //    }
    //    else
    //    {
    //        yield break;
    //    }
    //}

    IEnumerator RandomTargetNumber()
    {
        click = true;
        movePinball = false;

        soundManager.StopSFX(GameSfxType.Roulette);

        yield return new WaitForSeconds(1f);

        //targetNumber = Random.Range(1, maxNumber);

        targetNumber = pointerManager.CheckNumber();

        targetView.SetActive(true);
        targetText.text = targetNumber.ToString();

        bounsCount -= 1;

        yield return new WaitForSeconds(3);

        gameManager.CloseRouletteView(targetNumber);
    }

    IEnumerator BounsRoulette()
    {
        bouns = false;

        gameManager.ChangeMoney(5000);

        bounsCount = 3;

        NotionManager.instance.UseNotion("돈 5000 획득!", ColorType.Green);

        yield return new WaitForSeconds(2);

        Initialize(maxNumber);
    }
}
