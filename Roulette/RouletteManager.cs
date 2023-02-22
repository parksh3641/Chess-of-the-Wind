using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RouletteManager : MonoBehaviour
{
    public Pinball3D pinball;

    public MoveCamera rouletteCamera;
    public GameObject roulette3D;
    public GameObject characterIndexUI;

    [Title("Bouns")]
    public Rotation_Roulette bounsRoulette;

    public Text[] bounsTexts;
    public int[] bounsRewards = new int[4];

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

    private int power = 0;
    private int upPower = 1;
    private int maxPower = 20;

    bool pinballPower = false;
    bool pinballPowerReturn = false;

    public GameObject targetView;
    public Text targetText;
    public Text buttonText;

    [Title("Value")]
    public int windIndex = 0;
    private int targetNumber = 0;
    private int targetQueenNumber = 0;
    private int rouletteIndex = 0;
    private int pinballIndex = 0;

    private int leftNumber = 0;
    private int rightNumber = 0;

    private int averageMinus = 0;

    bool buttonClick = false;
    bool bouns = false;

    public GameManager gameManager;
    public UIManager uIManager;
    public CharacterManager characterManager;
    public SoundManager soundManager;
    public WindCharacterManager windCharacterManager;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);

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

        leftNumber = int.Parse(ht["LeftNumber"].ToString());
        rightNumber = int.Parse(ht["RightNumber"].ToString());

        if (PhotonNetwork.IsMasterClient)
        {
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

            int total = int.Parse(ht["Player1_Total"].ToString());
            total += int.Parse(ht["Player2_Total"].ToString());
            total += int.Parse(ht["Player3_Total"].ToString());
            total += int.Parse(ht["Player4_Total"].ToString());

            int minus = int.Parse(ht["Player1_Minus"].ToString());
            minus += int.Parse(ht["Player2_Minus"].ToString());
            minus += int.Parse(ht["Player3_Minus"].ToString());
            minus += int.Parse(ht["Player4_Minus"].ToString());

            Debug.Log(total + " / " + minus);

            if (total * 0.6f <= minus || GameStateManager.instance.FirstBouns) //전체적으로 60%이상 돈을 잃었을 경우
            {
                averageMinus = minus;

                bounsRewards[0] = (int)(averageMinus * 0.01f);
                bounsRewards[1] = (int)(averageMinus * 0.05f);
                bounsRewards[2] = (int)(averageMinus * 0.1f);
                bounsRewards[3] = (int)(averageMinus * 0.3f);

                for (int i = 0; i < bounsTexts.Length; i++)
                {
                    bounsTexts[i].text = "돈 " + bounsRewards[i];
                }

                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Bouns" } });
                PV.RPC("PlayBouns", RpcTarget.All);
            }
            else
            {

                PV.RPC("SelectRoulette", RpcTarget.All, rouletteIndex);
            }
        }
    }

    public void ShowBettingNumber()
    {
        leftQueen.material = defaultQueenMat;
        rightQueen.material = defaultQueenMat;

        for (int i = 0; i < leftPointerManager.pointerList.Count; i++)
        {
            leftPointerManager.pointerList[i].Betting(false);
            rightPointerManager.pointerList[i].Betting(false);
        }

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            if (gameManager.bettingNumberList_NewBie[0] == 1)
            {
                for (int i = 0; i < leftPointerManager.pointerList.Count; i++)
                {
                    if (leftPointerManager.pointerList[i].index % 2 == 0)
                    {
                        leftPointerManager.pointerList[i].Betting(true);
                        rightPointerManager.pointerList[i].Betting(true);
                    }
                }
            }
            else if (gameManager.bettingNumberList_NewBie[1] == 1)
            {
                for (int i = 0; i < leftPointerManager.pointerList.Count; i++)
                {
                    if (leftPointerManager.pointerList[i].index % 2 != 0)
                    {
                        leftPointerManager.pointerList[i].Betting(true);
                        rightPointerManager.pointerList[i].Betting(true);
                    }
                }
            }
            else if (gameManager.bettingNumberList_NewBie[2] == 1)
            {
                leftQueen.material = choiceQueenMat;
                rightQueen.material = choiceQueenMat;
            }
        }
        else
        {
            for (int i = 0; i < gameManager.bettingNumberList_Gosu.Count; i++)
            {
                for (int j = 0; j < leftPointerManager.pointerList.Count; j++)
                {
                    if (gameManager.bettingNumberList_Gosu[i] == 1 && i + 1 == leftPointerManager.pointerList[j].index)
                    {
                        if (i < 12)
                        {
                            leftPointerManager.pointerList[j].Betting(true);
                        }
                        else if (i == 12)
                        {
                            leftQueen.material = choiceQueenMat;
                        }
                        else
                        {
                            leftPointerManager.pointerList[j - 1].Betting(true);
                        }
                    }

                    if (gameManager.bettingNumberList_Gosu[i] == 1 && i + 1 == rightPointerManager.pointerList[j].index)
                    {
                        if (i < 12)
                        {
                            rightPointerManager.pointerList[j].Betting(true);
                        }
                        else if (i == 12)
                        {
                            rightQueen.material = choiceQueenMat;
                        }
                        else
                        {
                            rightPointerManager.pointerList[j - 1].Betting(true);
                        }
                    }
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


        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            leftPointerManager.Initialize_NewBie();
            rightPointerManager.Initialize_NewBie();
        }
        else
        {
            leftPointerManager.Initialize(leftNumber);
            rightPointerManager.Initialize(rightNumber);
        }

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
                for (int i = 0; i < leftClock.Length; i++)
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
            PV.RPC("PlayRoulette", RpcTarget.All);
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

    //void CheckGameMode()
    //{
    //    if (GameStateManager.instance.BounsCount > 0)
    //    {
    //        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Roulette" } });
    //        PV.RPC("PlayRoulette", RpcTarget.All);
    //    }
    //    else
    //    {
    //        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Bouns" } });
    //        PV.RPC("PlayBouns", RpcTarget.All);
    //    }
    //}

    [PunRPC]
    void PlayRoulette()
    {
        targetView.SetActive(false);
        uIManager.OpenBounsView(false);

        bouns = false;

        power = 0;
        powerFillAmount.fillAmount = 0;

        pinballPower = false;
        pinballPowerReturn = false;

        soundManager.PlayLoopSFX(GameSfxType.Roulette);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName.Equals(GameStateManager.instance.NickName))
            {
                windIndex = i; //내가 몇 번째 캐릭터지?
            }
        }

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        if (ht["Pinball"].Equals(GameStateManager.instance.NickName)) //각자 내 차례인지 확인하도록!
        {
            pinball.MyTurn(pinballIndex);

            buttonText.text = "당신은 " + (windIndex + 1) + "번째 캐릭터 위치입니다.\n꾹 눌러서 원하는 타이밍에 바람을 발사하세요!";

            NotionManager.instance.UseNotion(NotionType.YourTurn);
        }
        else
        {
            buttonText.text = "다음 차례에 바람을 불 수 있습니다.";
        }

        if (PhotonNetwork.IsMasterClient) //다음 사람 설정
        {
            if (PhotonNetwork.PlayerList.Length > 1) //혼자가 아닐경우
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

        if (PhotonNetwork.IsMasterClient)
        {
            bounsRoulette.StartRoulette();
            StartCoroutine(CheckBouns());

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Minus", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Minus", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player3_Minus", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player4_Minus", 0 } });
        }

        gameManager.SetTotalMoney();

        Debug.Log("보너스 룰렛 실행");
    }

    public void CloseRouletteView()
    {
        roulette3D.SetActive(false);
    }

    public void BlowWindDown()
    {
        if (!pinball.PV.IsMine || buttonClick) return;

        pinballPower = true;

        StartCoroutine(PowerCoroution());
    }

    public void BlowWindUp()
    {
        if (!pinball.PV.IsMine || buttonClick) return;

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
        if (pinball.PV.IsMine && pinball.move)
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
        if (!pinballPower)
        {
            yield break;

        }

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
            if (power > 0f)
            {
                power -= upPower;
            }
            else
            {
                pinballPowerReturn = false;
            }
        }

        powerFillAmount.fillAmount = (power * 1.0f) / (maxPower * 1.0f);

        yield return waitForSeconds;
        StartCoroutine(PowerCoroution());
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

        targetNumber = 0;

        if (rouletteIndex == 0)
        {
            targetNumber = leftPointerManager.CheckNumber(pinball.transform);

            leftQueenPoint.parent = roulette1Obj;
            targetQueenNumber = leftPointerManager.CheckQueenNumber(leftQueenPoint);
            leftQueenPoint.parent = leftClock[2].transform;

            if (leftNumber + 1 > 23)
            {
                leftNumber = 0;
            }
            else
            {
                leftNumber++;
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "LeftNumber", leftNumber } });
        }
        else
        {
            targetNumber = rightPointerManager.CheckNumber(pinball.transform);

            rightQueenPoint.parent = roulette2Obj;
            targetQueenNumber = rightPointerManager.CheckQueenNumber(rightQueenPoint);
            rightQueenPoint.parent = rightClock[2].transform;

            if (rightNumber + 1 > 23)
            {
                rightNumber = 0;
            }
            else
            {
                rightNumber++;
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "RightNumber", rightNumber } });
        }

        while (targetNumber == 0)
        {
            yield return null;
        }

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            if (targetNumber % 2 == 0)
            {
                PV.RPC("ShowTargetNumber_NewBie", RpcTarget.All, 0);
            }
            else
            {
                PV.RPC("ShowTargetNumber_NewBie", RpcTarget.All, 1);
            }
        }
        else
        {
            PV.RPC("ShowTargetNumber", RpcTarget.All, targetNumber);
        }

        yield return new WaitForSeconds(3);

        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Waiting" } });

        string[] target = new string[2];

        target[0] = targetNumber.ToString();

        if (targetNumber == targetQueenNumber)
        {
            target[1] = "1";
        }
        else
        {
            target[1] = "0";
        }

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
    void ShowTargetNumber_NewBie(int number)
    {
        targetView.SetActive(true);

        if (number == 0)
        {
            targetText.text = "흰";
        }
        else
        {
            targetText.text = "검";
        }
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
        while (bouns)
        {
            if (bounsRoulette.speed <= 0)
            {
                bouns = false;

                StartCoroutine(BounsRoulette(bounsRoulette.GetRotate()));
            }
            yield return null;
        }
    }
    IEnumerator BounsRoulette(int rotate)
    {
        if (rotate >= 0 && rotate < 45)
        {
            PV.RPC("ChangeMoney", RpcTarget.All, bounsRewards[0]);
        }
        else if (rotate >= 45 && rotate < 90)
        {
            PV.RPC("ChangeMoney", RpcTarget.All, bounsRewards[3]);
        }
        else if (rotate >= 90 && rotate < 135)
        {
            PV.RPC("ChangeMoney", RpcTarget.All, bounsRewards[3]);
        }
        else if (rotate >= 135 && rotate < 180)
        {
            PV.RPC("ChangeMoney", RpcTarget.All, bounsRewards[2]);
        }
        else if (rotate >= 180 && rotate < 225)
        {
            PV.RPC("ChangeMoney", RpcTarget.All, bounsRewards[2]);
        }
        else if (rotate >= 225 && rotate < 270)
        {
            PV.RPC("ChangeMoney", RpcTarget.All, bounsRewards[1]);
        }
        else if (rotate >= 270 && rotate < 315)
        {
            PV.RPC("ChangeMoney", RpcTarget.All, bounsRewards[1]);
        }
        else if (rotate >= 315 && rotate < 360)
        {
            PV.RPC("ChangeMoney", RpcTarget.All, bounsRewards[0]);
        }

        yield return new WaitForSeconds(2);

        PV.RPC("SelectRoulette", RpcTarget.All, rouletteIndex);
    }

    [PunRPC]
    void ChangeMoney(int number)
    {
        if (uIManager.waitingObj.activeInHierarchy) return;

        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, number);

        NotionManager.instance.UseNotion("보너스 룰렛 보상 : 돈 " + number + " 만큼 획득!", ColorType.Green);
    }

    #endregion

    #region Spectator

    public void SpectatorRoulette()
    {
        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        if (ht["Status"].ToString().Equals("Roulette"))
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
