using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    public Pinball2D pinball;
    public Rotation_Roulette roulette;

    public GameObject rouletteCamera;
    public GameObject rouletteView;

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

    public GameManager gameManager;
    public PointerManager pointerManager;
    public SoundManager soundManager;

    public PhotonView PV;

    private void Awake()
    {
        click = false;

        bounsView.SetActive(false);

        rouletteCamera.SetActive(false);
        rouletteView.SetActive(false);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (movePinball)
        {
            if (pinball.speed <= 0)
            {
                StartCoroutine(RandomTargetNumber());
            }

            if (pinball.wind && pinball.rigid.velocity.x == 0)
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
        maxNumber = number;

        rouletteCamera.SetActive(true);
        rouletteView.SetActive(true);

        if(bounsCount > 0)
        {
            PV.RPC("PlayRoulette", RpcTarget.All, bounsCount);
        }
        else
        {
            PV.RPC("PlayBouns", RpcTarget.All);
        }

        soundManager.PlayLoopSFX(GameSfxType.Roulette);
    }

    [PunRPC]
    void PlayRoulette(int number)
    {
        targetView.SetActive(false);
        bounsView.SetActive(false);

        click = false;
        movePinball = true;
        bouns = false;

        titleText.text = "숫자 룰렛";

        bounsText.text = "보너스 룰렛까지 남은 턴 : " + number.ToString();

        if (PhotonNetwork.IsMasterClient)
        {
            movePinball = true;

            pinball.StartRotate();
        }

        Debug.Log("숫자 룰렛 실행");
    }

    [PunRPC]
    void PlayBouns()
    {
        targetView.SetActive(false);
        bounsView.SetActive(true);

        click = true;
        movePinball = false;
        bouns = true;

        titleText.text = "보너스 룰렛";

        if (PhotonNetwork.IsMasterClient)
        {
            bounsCount = 3;
            roulette.StartRoulette();
        }

        Debug.Log("보너스 룰렛 실행");
    }

    public void CloseRouletteView()
    {
        rouletteCamera.SetActive(false);
        rouletteView.SetActive(false);
    }

    public void BlowWind()
    {
        if (click) return;

        if(PhotonNetwork.IsMasterClient)
        {
            click = true;

            pinball.StartPinball();
        }
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

        if(PhotonNetwork.IsMasterClient)
        {
            bounsCount -= 1;
            targetNumber = pointerManager.CheckNumber();
        }

        PV.RPC("CheckNumber", RpcTarget.All, targetNumber);

        yield return new WaitForSeconds(3);

        PV.RPC("ResultNumber", RpcTarget.All, targetNumber);
    }

    [PunRPC]
    void CheckNumber(int number)
    {
        targetView.SetActive(true);
        targetText.text = number.ToString();
    }

    [PunRPC]
    void ResultNumber(int number)
    {
        gameManager.CloseRouletteView(number);

        CloseRouletteView();
    }

    IEnumerator BounsRoulette()
    {
        bouns = false;

        PV.RPC("ChangeMoney", RpcTarget.All, 5000);

        yield return new WaitForSeconds(2);

        PV.RPC("PlayRoulette", RpcTarget.All, bounsCount);

        //Initialize(maxNumber);
    }

    [PunRPC]
    void ChangeMoney(int number)
    {
        gameManager.ChangeMoney(number);

        NotionManager.instance.UseNotion("돈 " + number + " 획득!", ColorType.Green);
    }
}
