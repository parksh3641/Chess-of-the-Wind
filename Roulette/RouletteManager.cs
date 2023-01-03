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

    public MoveCamera rouletteCamera;
    public GameObject roulette3D;
    public GameObject characterIndexUI;

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

    public Transform leftQueenPoint;
    public Transform rightQueenPoint;

    public MeshRenderer leftQueen;
    public MeshRenderer rightQueen;

    public Material defaultQueenMat;
    public Material choiceQueenMat;

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
    public Text buttonText;

    [Title("Value")]
    public int windIndex = 0;
    private int targetNumber = 0;
    private int targetQueenNumber = 0;
    private int rouletteIndex = 0;
    private int pinballIndex = 0;
    private int pointerNumber = 0;

    bool buttonClick = false;
    bool bouns = false;

    public GameManager gameManager;
    public UIManager uIManager;
    public CharacterManager characterManager;
    public SoundManager soundManager;
    public WindCharacterManager windCharacterManager;

    public PhotonView PV;

    private void Awake()
    {
        buttonClick = false;
        bouns = false;

        rouletteCamera.gameObject.SetActive(false);
        roulette3D.SetActive(false);

        powerFillAmount.fillAmount = 0;
    }

    public void Initialize()
    {
        uIManager.OpenRouletteView();

        rouletteCamera.gameObject.SetActive(true);
        roulette3D.SetActive(false);
        characterIndexUI.SetActive(false);

        targetView.SetActive(false);

        buttonText.text = "";

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        pointerNumber = int.Parse(ht["Number"].ToString());

        if (PhotonNetwork.IsMasterClient)
        {
            if(pointerNumber + 1 > 23)
            {
                pointerNumber = 0;
            }
            else
            {
                pointerNumber++;
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Number", pointerNumber } });

            if (rouletteIndex == 0)
            {
                rouletteIndex = 1;
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Roulette", "Right" } });
            }
            else
            {
                rouletteIndex = 0;
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Roulette", "Left" } });
            }

            if (GameStateManager.instance.BounsCount > 0)
            {
                PV.RPC("SelectRoulette", RpcTarget.All, rouletteIndex);
            }
            else
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Bouns" } });
                PV.RPC("PlayBouns", RpcTarget.All);
            }
        }
    }

    public void ShowBettingNumber()
    {
        for (int i = 0; i < gameManager.bettingNumberList.Count - 1; i++)
        {
            leftPointerManager.pointerList[i].Betting(false);
            rightPointerManager.pointerList[i].Betting(false);
        }

        for (int i = 0; i < gameManager.bettingNumberList.Count; i ++)
        {
            for(int j = 0; j < leftPointerManager.pointerList.Count; j ++)
            {
                if (gameManager.bettingNumberList[i] == 1 && i + 1 == leftPointerManager.pointerList[j].index)
                {
                    if (i < 13)
                    {
                        leftPointerManager.pointerList[j].Betting(true);
                        rightPointerManager.pointerList[j].Betting(true);
                    }
                    else if (i > 13)
                    {
                        leftPointerManager.pointerList[j - 1].Betting(true);
                        rightPointerManager.pointerList[j - 1].Betting(true);
                    }

                    break;
                }
            }
        }

        if (gameManager.bettingNumberList[12] > 0) //퀸 예외 처리
        {
            leftQueen.material = choiceQueenMat;
            rightQueen.material = choiceQueenMat;
        }
        else
        {
            leftQueen.material = defaultQueenMat;
            rightQueen.material = defaultQueenMat;
        }

        if(gameManager.bettingNumberList[24] > 0) //마지막 숫자 배팅 예외 처리
        {
            for(int i = 0; i < leftPointerManager.pointerList.Count; i ++)
            {
                if(leftPointerManager.pointerList[i].index == 24)
                {
                    leftPointerManager.pointerList[i].Betting(true);
                    rightPointerManager.pointerList[i].Betting(true);
                }
            }
        }
    }

    [PunRPC]
    void SelectRoulette(int number)
    {
        uIManager.OpenBounsView(false);

        roulette3D.SetActive(true);

        pinballIndex = number;

        rouletteCamera.gameObject.SetActive(true);
        rouletteCamera.Initialize(startCameraPos);

        roulette1Obj.gameObject.SetActive(true);
        roulette2Obj.gameObject.SetActive(true);

        leftPointerManager.Initialize(pointerNumber);
        rightPointerManager.Initialize(pointerNumber);

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

        ShowBettingNumber();
    }

    public void EndMoveFinger(int number)
    {
        if (number == 0)
        {
            rouletteCamera.SetTarget(roulette1Pos);

            if (PhotonNetwork.IsMasterClient)
            {
                for(int i = 0; i < leftClock.Length; i ++)
                {
                    leftClock[i].StartClock();
                }
            }

            StartCoroutine(WaitWindCharacter1(roulette1Obj));
        }
        else
        {
            rouletteCamera.SetTarget(roulette2Pos);

            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < rightClock.Length; i++)
                {
                    rightClock[i].StartClock();
                }
            }

            StartCoroutine(WaitWindCharacter2(roulette2Obj));
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Roulette" } });
            PV.RPC("PlayRoulette", RpcTarget.All, GameStateManager.instance.BounsCount);
        }
    }

    IEnumerator WaitWindCharacter1(Transform target)
    {
        yield return new WaitForSeconds(1.0f);
        windCharacterManager.Initialize(target);

        characterIndexUI.SetActive(true);

        roulette2Obj.gameObject.SetActive(false);
        leftFingerController.Disable();

        buttonClick = false;
    }

    IEnumerator WaitWindCharacter2(Transform target)
    {
        yield return new WaitForSeconds(1.0f);
        windCharacterManager.Initialize(target);

        characterIndexUI.SetActive(true);

        roulette1Obj.gameObject.SetActive(false);
        rightFingerController.Disable();

        buttonClick = false;
    }

    void CheckGameMode()
    {
        if (GameStateManager.instance.BounsCount > 0)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Roulette" } });
            PV.RPC("PlayRoulette", RpcTarget.All, GameStateManager.instance.BounsCount);
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
        uIManager.OpenBounsView(false);

        bouns = false;

        power = 0;
        powerFillAmount.fillAmount = 0;

        pinballPower = false;
        pinballPowerReturn = false;

        titleText.text = "숫자 룰렛";

        bounsText.text = "보너스 룰렛까지 남은 턴 : " + number.ToString();

        soundManager.PlayLoopSFX(GameSfxType.Roulette);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName.Equals(GameStateManager.instance.NickName))
            {
                windIndex = i; //내가 몇 번째 캐릭터지?

                buttonText.text = "당신은 " + (windIndex + 1) + "번째 캐릭터 위치입니다.\n꾹 눌러서 원하는 타이밍에 바람을 발사하세요!";
            }
        }

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        if (ht["Pinball"].Equals(GameStateManager.instance.NickName)) //각자 내 차례인지 확인하도록!
        {
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
    void PlayBouns()
    {
        targetView.SetActive(false);
        uIManager.OpenBounsView(true);

        buttonClick = true;
        bouns = true;

        titleText.text = "보너스 룰렛";

        if (PhotonNetwork.IsMasterClient)
        {
            GameStateManager.instance.BounsCount = 3;
            bounsRoulette.StartRoulette();
            StartCoroutine(CheckBouns());
        }

        Debug.Log("보너스 룰렛 실행");
    }

    public void CloseRouletteView()
    {
        roulette3D.SetActive(false);
    }

    public void BlowWindDown()
    {
        if (buttonClick) return;

        pinballPower = true;

        StartCoroutine(PowerCoroution());
    }

    public void BlowWindUp()
    {
        if (buttonClick) return;

        buttonClick = true;
        pinballPower = false;

        float[] blow = new float[2];
        blow[0] = power;
        blow[1] = windIndex;

        PV.RPC("BlowingWind", RpcTarget.All, blow);
    }

    [PunRPC]
    void BlowingWind(float[] wind)
    {
        if(pinball.PV.IsMine && pinball.move)
        {
            pinball.BlowingWind(wind[0], (int)wind[1]);
        }

        PV.RPC("BlowingParticle", RpcTarget.All, (int)wind[1]);
    }

    [PunRPC]
    void BlowingParticle(int number)
    {
        windCharacterManager.WindBlowing(number);
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

            yield return new WaitForSeconds(0.02f);
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
    }

    [PunRPC]
    void GetNumber()
    {
        StartCoroutine(GetNumberCoroution());
    }

    IEnumerator GetNumberCoroution()
    {
        PV.RPC("EndRoulette", RpcTarget.All);

        yield return new WaitForSeconds(1f);

        GameStateManager.instance.BounsCount -= 1;

        targetNumber = 0;

        if (rouletteIndex == 0)
        {
            targetNumber = leftPointerManager.CheckNumber(pinball.transform);

            leftQueenPoint.parent = roulette1Obj;
            targetQueenNumber = leftPointerManager.CheckNumber(leftQueenPoint);
            leftQueenPoint.parent = leftClock[2].transform;
        }
        else
        {
            targetNumber = rightPointerManager.CheckNumber(pinball.transform);

            rightQueenPoint.parent = roulette2Obj;
            targetQueenNumber = rightPointerManager.CheckNumber(rightQueenPoint);
            rightQueenPoint.parent = rightClock[2].transform;
        }

        while(targetNumber == 0)
        {
            yield return null;
        }

        PV.RPC("ShowTargetNumber", RpcTarget.All, targetNumber);

        yield return new WaitForSeconds(3);

        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Waiting" } });

        string[] target = new string[2];

        target[0] = targetNumber.ToString();
        target[1] = targetQueenNumber.ToString();

        PV.RPC("GameResult", RpcTarget.All, target);
    }

    [PunRPC]
    void EndRoulette()
    {
        soundManager.StopLoopSFX(GameSfxType.Roulette);

        for (int i = 0; i < leftClock.Length; i++)
        {
            leftClock[i].StopClock();
        }

        for (int i = 0; i < rightClock.Length; i++)
        {
            rightClock[i].StopClock();
        }
    }

    [PunRPC]
    void ShowTargetNumber(int number)
    {
        targetView.SetActive(true);
        targetText.text = number.ToString();
    }

    [PunRPC]
    void GameResult(string[] target)
    {
        gameManager.CloseRouletteView(target);
        windCharacterManager.Stop();

        roulette1Obj.gameObject.SetActive(false);
        roulette2Obj.gameObject.SetActive(false);

        CloseRouletteView();
    }

    #region Bouns
    IEnumerator CheckBouns()
    {
        while(bouns)
        {
            if (bounsRoulette.speed <= 0)
            {
                bouns = false;

                StartCoroutine(BounsRoulette());
            }
            yield return null;
        }
    }
    IEnumerator BounsRoulette()
    {
        PV.RPC("ChangeMoney", RpcTarget.All, 5000);

        yield return new WaitForSeconds(2);

        PV.RPC("SelectRoulette", RpcTarget.All, rouletteIndex);
    }

    [PunRPC]
    void ChangeMoney(int number)
    {
        if (uIManager.waitingObj.activeInHierarchy) return;

        gameManager.ChangeMoney(number);

        NotionManager.instance.UseNotion("돈 " + number + " 획득!", ColorType.Green);
    }

    #endregion

    #region Spectator

    public void SpectatorRoulette()
    {
        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        if(ht["Status"].ToString().Equals("Roulette"))
        {
            if (ht["Roulette"].ToString().Equals("Left"))
            {
                rouletteIndex = 0;
            }
            else
            {
                rouletteIndex = 1;
            }

            SelectRoulette(rouletteIndex);
        }
        else
        {
            PlayBouns();
        }
    }

    #endregion
}
