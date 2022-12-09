using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    public Pinball pinball;
    public Rotation_Roulette roulette;

    public GameObject rouletteCamera;
    public GameObject pinballCamera;

    public GameObject bounsView;

    public Image powerFillAmount;


    private float power = 0.1f;
    private float upPower = 0.05f;
    private float maxPower = 1f;
    public int bounsCount = 2;

    private int targetNumber = 0;
    private int maxNumber = 0;

    public GameObject targetView;
    public Text targetText;
    public Text titleText;
    public Text bounsText;




    bool click = false;
    bool start = false;
    bool check = false;
    bool moveSphere = false;
    bool moveRoulette = false;

    float time = 0;


    public GameManager gameManager;
    public SoundManager soundManager;

    private void Awake()
    {
        click = false;

        pinballCamera.SetActive(false);
        bounsView.SetActive(false);
    }

    private void Update()
    {
        if (moveSphere)
        {
            if (pinball.rigid.velocity.magnitude < 0.5f)
            {
                time += Time.deltaTime;

                if(time >= 4)
                {
                    StartCoroutine(RandomTargetNumber());
                }
            }
        }

        if(moveRoulette)
        {
            if(roulette.speed <= 0)
            {
                StartCoroutine(BounsRoulette());
            }
        }
    }

    public void EnlargementPinball()
    {
        rouletteCamera.SetActive(false);
        pinballCamera.SetActive(true);
    }

    public void Initialize(int number)
    {
        click = false;
        moveSphere = false;
        moveRoulette = false;

        time = 0;

        power = 0;
        powerFillAmount.fillAmount = 0;

        targetView.SetActive(false);
        bounsView.SetActive(false);

        rouletteCamera.SetActive(true);
        pinballCamera.SetActive(false);

        maxNumber = number;

        if(bounsCount > 0)
        {
            moveSphere = true;

            pinball.StartPinball();

            titleText.text = "숫자 룰렛";

            bounsText.text = "보너스 룰렛까지 남은 턴 : " + bounsCount.ToString();
        }
        else
        {
            click = true;

            moveRoulette = true;

            titleText.text = "보너스 룰렛";

            bounsView.SetActive(true);

            roulette.StartRoulette();
        }


        soundManager.PlayLoopSFX(GameSfxType.Roulette);
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

        pinball.AddSpeed(power);
    }

    IEnumerator PowerCoroution()
    {
        if (start)
        {
            if (!check)
            {
                if (power <= maxPower)
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
                if (power > 0.1f)
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
        else
        {
            yield break;
        }
    }

    IEnumerator RandomTargetNumber()
    {
        click = true;
        moveSphere = false;

        EnlargementPinball();

        soundManager.StopSFX(GameSfxType.Roulette);

        targetNumber = Random.Range(1, maxNumber);

        targetView.SetActive(true);
        targetText.text = targetNumber.ToString();

        bounsCount -= 1;

        yield return new WaitForSeconds(3);

        pinballCamera.SetActive(false);

        gameManager.CloseRouletteView(targetNumber);
    }

    IEnumerator BounsRoulette()
    {
        moveRoulette = false;

        gameManager.ChangeMoney(5000);

        bounsCount = 3;

        NotionManager.instance.UseNotion("돈 5000 획득!", ColorType.Green);

        yield return new WaitForSeconds(2);

        Initialize(maxNumber);
    }
}
