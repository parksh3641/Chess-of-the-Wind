using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RouletteManager : MonoBehaviour
{
    public Pinball3D pinball;

    public MoveCamera rouletteChoiceCamera;
    public GameObject rouletteCamera;
    public GameObject rouletteView;

    public GameObject bounsView;
    public Rotation_Roulette bounsRoulette;

    [Title("CameraPos")]
    public Transform startCameraPos;
    public Transform roulette1Pos;
    public Transform roulette2Pos;

    public Transform roulette1Obj;
    public Transform roulette2Obj;

    [Title("Finger")]
    public FingerController leftFingerController;
    public FingerController rightFingerController;

    public PointerManager leftPointerManager;
    public PointerManager rightPointerManager;

    [Title("Clock")]
    public Rotation_Clock[] leftClock;
    public Rotation_Clock[] rightClock;

    public Text leftQueenText;
    public Text rightQueenText;

    [Title("Gauge")]
    public Image powerFillAmount;

    private float power = 0;
    private float upPower = 0.025f;
    private float maxPower = 1f;

    bool pinballPower = false;
    bool pinballPowerReturn = false;

    public GameObject targetView;
    public Text targetText;
    public Text titleText;
    public Text bounsText;

    [Title("Value")]
    private int targetNumber = 0;
    private int maxNumber = 0;
    private int rouletteIndex = 0;
    private int pinballIndex = 0;

    bool buttonClick = false;
    bool bouns = false;

    public GameManager gameManager;
    public CharacterManager characterManager;
    public SoundManager soundManager;
    public WindCharacterManager windCharacterManager;

    public PhotonView PV;

    private void Awake()
    {
        buttonClick = false;
        bouns = false;

        bounsView.SetActive(false);

        rouletteChoiceCamera.gameObject.SetActive(false);
        rouletteCamera.SetActive(false);
        rouletteView.SetActive(false);

        powerFillAmount.fillAmount = 0;
    }

    private void Update()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if(bouns)
        {
            if(bounsRoulette.speed <= 0)
            {
                StartCoroutine(BounsRoulette());
            }
        }
    }

    public void Initialize(int number)
    {
        maxNumber = number;

        rouletteChoiceCamera.gameObject.SetActive(true);
        rouletteCamera.SetActive(false);
        rouletteView.SetActive(true);

        targetView.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            if (rouletteIndex == 0) rouletteIndex = 1;
            else rouletteIndex = 0;

            PV.RPC("SelectRoulette", RpcTarget.All, rouletteIndex);
        }
    }

    public void CheckBettingNumber()
    {
        for (int i = 0; i < gameManager.bettingNumberList.Count - 1; i++)
        {
            leftPointerManager.pointerList[i].Betting(false);
            rightPointerManager.pointerList[i].Betting(false);
        }

        leftQueenText.color = Color.black;
        rightQueenText.color = Color.black;

        for(int i = 0; i < gameManager.bettingNumberList.Count; i ++)
        {
            if (gameManager.bettingNumberList[i] == 1)
            {
                if(i < 13)
                {
                    leftPointerManager.pointerList[i].Betting(true);
                    rightPointerManager.pointerList[i].Betting(true);
                }
                else if (i > 13)
                {
                    leftPointerManager.pointerList[i - 1].Betting(true);
                    rightPointerManager.pointerList[i - 1].Betting(true);
                }
            }
        }

        if (gameManager.bettingNumberList[12] == 1)
        {
            leftQueenText.color = Color.red;
            rightQueenText.color = Color.red;
        }
    }

    [PunRPC]
    void SelectRoulette(int number)
    {
        pinballIndex = number;

        rouletteChoiceCamera.gameObject.SetActive(true);
        rouletteChoiceCamera.Initialize(startCameraPos);

        leftFingerController.Initialize();
        rightFingerController.Initialize();

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < leftClock.Length; i++)
            {
                leftClock[i].StopClock();
            }

            for (int i = 0; i < rightClock.Length; i++)
            {
                rightClock[i].StopClock();
            }
        }

        if (number == 0)
        {
            leftFingerController.MoveFinger();
        }
        else
        {
            rightFingerController.MoveFinger();
        }
    }

    public void EndMoveFinger(int number)
    {
        if (number == 0)
        {
            rouletteChoiceCamera.SetTarget(roulette1Pos);

            if (PhotonNetwork.IsMasterClient)
            {
                for(int i = 0; i < leftClock.Length; i ++)
                {
                    leftClock[i].StartClock();
                }
            }

            StartCoroutine(WaitWindCharacter(roulette1Obj));
        }
        else
        {
            rouletteChoiceCamera.SetTarget(roulette2Pos);

            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < rightClock.Length; i++)
                {
                    rightClock[i].StartClock();
                }
            }

            StartCoroutine(WaitWindCharacter(roulette2Obj));
        }

        if (PhotonNetwork.IsMasterClient)
        {
            CheckGameMode();
        }
    }

    IEnumerator WaitWindCharacter(Transform target)
    {
        yield return new WaitForSeconds(1.0f);
        windCharacterManager.Initialize(target);
    }

    void CheckGameMode()
    {
        if (gameManager.bounsCount > 0)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Roulette" } });
            PV.RPC("PlayRoulette", RpcTarget.All, gameManager.bounsCount);
        }
        else
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Bouns" } });
            PV.RPC("PlayBouns", RpcTarget.All);
        }
    }

    [PunRPC]
    void PlayRoulette(int number)
    {
        targetView.SetActive(false);
        bounsView.SetActive(false);

        buttonClick = false;
        bouns = false;

        power = 0;
        powerFillAmount.fillAmount = 0;

        pinballPower = false;
        pinballPowerReturn = false;

        titleText.text = "숫자 룰렛";

        bounsText.text = "보너스 룰렛까지 남은 턴 : " + number.ToString();

        soundManager.PlayLoopSFX(GameSfxType.Roulette);

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        if (ht["Pinball"].Equals(PlayerPrefs.GetString("NickName"))) //각자 내 차례인지 확인하도록!
        {
            PV.RPC("CheckPlayer", RpcTarget.All, PlayerPrefs.GetString("NickName"));

            pinball.MyTurn(pinballIndex);

            NotionManager.instance.UseNotion(NotionType.YourTurn);

            Debug.Log("내 차례입니다.");
        }

        if(PhotonNetwork.IsMasterClient) //다음 사람 설정
        {
            if(PhotonNetwork.PlayerList.Length > 1) //혼자가 아닐경우
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if (PhotonNetwork.PlayerList[i].NickName.Equals(ht["Pinball"]))
                    {
                        if (i < PhotonNetwork.PlayerList.Length - 1) //돌린 사람 다음 사람이 핀볼을 돌리세요!
                        {
                            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Pinball", PhotonNetwork.PlayerList[i + 1].NickName } });
                            Debug.Log("다음 사람 : " + PhotonNetwork.PlayerList[i + 1].NickName);
                        }
                        else
                        {
                            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Pinball", PhotonNetwork.PlayerList[0].NickName } });
                            Debug.Log("다시 처음부터 돌아갑니다.");
                        }

                        break;
                    }
                }
            }
        }


        Debug.Log("숫자 룰렛 실행");
    }

    [PunRPC]
    void CheckPlayer(string name)
    {
        characterManager.CheckPlayer(name);
    }

    [PunRPC]
    void PlayBouns()
    {
        targetView.SetActive(false);
        bounsView.SetActive(true);

        buttonClick = true;
        bouns = true;

        titleText.text = "보너스 룰렛";

        if (PhotonNetwork.IsMasterClient)
        {
            gameManager.bounsCount = 3;
            bounsRoulette.StartRoulette();
        }

        Debug.Log("보너스 룰렛 실행");
    }

    public void CloseRouletteView()
    {
        rouletteCamera.SetActive(false);
        rouletteView.SetActive(false);
    }

    public void BlowWindDown()
    {
        if(pinball.PV.IsMine && pinball.move)
        {
            if (buttonClick) return;

            pinballPower = true;

            StartCoroutine(PowerCoroution());
        }
    }

    public void BlowWindUp()
    {
        if (pinball.PV.IsMine && pinball.move)
        {
            if (buttonClick) return;

            buttonClick = true;
            pinballPower = false;

            pinball.StartPinball(power);
        }
    }

    IEnumerator PowerCoroution()
    {
        if (pinballPower)
        {
            if (!pinballPowerReturn)
            {
                if (power <= maxPower)
                {
                    power += upPower;
                }
                else
                {
                    pinballPowerReturn = true;
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
                    pinballPowerReturn = false;
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

    public void EndPinball()
    {
        PV.RPC("GetNumber", RpcTarget.MasterClient);

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < leftClock.Length; i++)
            {
                leftClock[i].StopClock();
            }

            for (int i = 0; i < rightClock.Length; i++)
            {
                rightClock[i].StopClock();
            }
        }
    }

    [PunRPC]
    void GetNumber()
    {
        StartCoroutine(GetNumberCoroution());
    }

    IEnumerator GetNumberCoroution()
    {
        PV.RPC("StopLoopSFX", RpcTarget.All);

        yield return new WaitForSeconds(1f);

        gameManager.bounsCount -= 1;

        if(rouletteIndex == 0)
        {
            targetNumber = leftPointerManager.CheckNumber(pinball.transform);
        }
        else
        {
            targetNumber = rightPointerManager.CheckNumber(pinball.transform);
        }

        PV.RPC("CheckNumber", RpcTarget.All, targetNumber);

        yield return new WaitForSeconds(3);

        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Waiting" } });

        if(targetNumber > 11)
        {
            targetNumber++;
        }

        PV.RPC("ResultNumber", RpcTarget.All, targetNumber);
    }

    [PunRPC]
    void StopLoopSFX()
    {
        soundManager.StopLoopSFX(GameSfxType.Roulette);
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
        windCharacterManager.Stop();

        CloseRouletteView();
    }

    #region Bouns
    IEnumerator BounsRoulette()
    {
        bouns = false;

        PV.RPC("ChangeMoney", RpcTarget.All, 5000);

        yield return new WaitForSeconds(2);

        PV.RPC("PlayRoulette", RpcTarget.All, 3);

        //Initialize(maxNumber);
    }

    [PunRPC]
    void ChangeMoney(int number)
    {
        gameManager.ChangeMoney(number);

        NotionManager.instance.UseNotion("돈 " + number + " 획득!", ColorType.Green);
    }

    #endregion
}
