﻿using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RouletteManager : MonoBehaviour
{
    public MoveCamera rouletteCamera;
    public GameObject roulette3D;
    public GameObject characterIndexUI;

    [Space]
    [Title("Bouns")]
    public Rotation_Roulette bounsRoulette;

    public Text[] bounsTexts;
    public int[] bounsRewards = new int[4];

    [Space]
    [Title("CameraPos")]
    public Transform startCameraPos;
    public Transform roulette1Pos;
    public Transform roulette2Pos;

    [Space]
    [Title("Roulette")]
    public Transform roulette1Obj;
    public Transform roulette2Obj;

    public MeshRenderer roulette1Mesh;
    public MeshRenderer roulette2Mesh;

    [Space]
    [Title("Roulette_Particle")]
    public ParticleSystem[] roulette1Particle;
    public ParticleSystem[] roulette2Particle;

    [Space]
    [Title("Finger")]
    public FingerController leftFingerController;
    public FingerController rightFingerController;

    public PointerManager leftPointerManager;
    public PointerManager rightPointerManager;

    [Space]
    [Title("Photon Obj")]
    public Pinball3D pinball;

    public Transform[] leftWindPoint;
    public Transform[] rightWindPoint;

    Rotation_Clock[] leftClock = new Rotation_Clock[3];
    Rotation_Clock[] rightClock = new Rotation_Clock[3];

    MeshRenderer leftQueen;
    MeshRenderer rightQueen;

    Transform leftQueenPoint;
    Transform rightQueenPoint;


   [Title("Gauge")]
    public Image powerFillAmount;

    private int power = 0;
    private int upPower = 2;
    private int maxPower = 100;

    bool pinballPower = false;
    bool pinballPowerReturn = false;

    public GameObject targetView;
    public Text targetText;
    public Text buttonText;

    [Space]
    [Title("Value")]
    public int windIndex = 0;
    private int targetNumber = 0;
    private int targetQueenNumber = 0;
    private int rouletteIndex = 0;
    private int pinballIndex = 0;

    private int leftNumber = 0;
    private int rightNumber = 0;

    private int colorNumber = 0;
    private int colorNumber2 = 0;

    [Space]
    [Title("bool")]
    bool buttonClick = false;
    bool bouns = false;
    bool aiMode = false;

    public GameManager gameManager;
    public UIManager uIManager;
    public CharacterManager characterManager;
    public SoundManager soundManager;
    public WindCharacterManager windCharacterManager;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.5f);

    public PhotonView PV;

    private void Awake()
    {
        buttonClick = false;
        bouns = false;

        rouletteCamera.gameObject.SetActive(false);
        roulette3D.SetActive(false);
    }

    public void CreateObj()
    {
        if (pinball != null)
        {
            PhotonNetwork.Destroy(pinball.gameObject);
        }

        pinball = PhotonNetwork.Instantiate("PinballObj", Vector3.zero, Quaternion.identity, 0).GetComponent<Pinball3D>();
        pinball.transform.parent = roulette3D.transform;
        pinball.rouletteManager = this;

        for (int i = 0; i < leftClock.Length; i++)
        {
            if(leftClock[i] != null)
            {
                PhotonNetwork.Destroy(leftClock[i].gameObject);
                PhotonNetwork.Destroy(rightClock[i].gameObject);
            }
        }

        leftClock[0] = PhotonNetwork.Instantiate("ClockSecObj_Left", roulette1Obj.transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
        leftClock[1] = PhotonNetwork.Instantiate("ClockMinObj_Left", roulette1Obj.transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
        leftClock[2] = PhotonNetwork.Instantiate("ClockQueenObj_Left", roulette1Obj.transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();

        leftQueen = leftClock[2].meshRenderer;
        leftQueenPoint = leftClock[2].queenPoint;

        rightClock[0] = PhotonNetwork.Instantiate("ClockSecObj_Right", roulette2Obj.transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
        rightClock[1] = PhotonNetwork.Instantiate("ClockMinObj_Right", roulette2Obj.transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
        rightClock[2] = PhotonNetwork.Instantiate("ClockQueenObj_Right", roulette2Obj.transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();

        rightQueen = rightClock[2].meshRenderer;
        rightQueenPoint = rightClock[2].queenPoint;

        for (int i = 0; i < leftClock.Length; i++)
        {
            leftClock[i].transform.parent = roulette1Obj.transform;
            rightClock[i].transform.parent = roulette2Obj.transform;
        }

        Debug.Log("포톤 오브젝트 재생성 완료");
    }

    public void Initialize()
    {
        uIManager.OpenRouletteView();
        uIManager.CloseSurrenderView();

        characterManager.ResetFocus();

        rouletteCamera.gameObject.SetActive(true);
        roulette3D.SetActive(false);
        characterIndexUI.SetActive(false);

        targetView.SetActive(false);

        power = 0;
        powerFillAmount.fillAmount = 0;

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

            bool check = (bool)ht["Bouns"];

            int total = GameStateManager.instance.Stakes * 2;

            int minus = int.Parse(ht["Player1_Minus"].ToString());
            minus += int.Parse(ht["Player2_Minus"].ToString());

            Debug.Log("보너스 룰렛 체크 : " + total + " / " + minus);

            if (total * 0.7f <= minus || GameStateManager.instance.CheckBouns && !check) //공중 분해된 돈이 전체 판돈에 30% 이상일 경우 매판 1번 등장.
            {
                bounsRewards[0] = (int)(minus * 0.05f);
                bounsRewards[1] = (int)(minus * 0.1f);
                bounsRewards[2] = (int)(minus * 0.25f);
                bounsRewards[3] = (int)(minus * 0.5f);

                for (int i = 0; i < bounsTexts.Length; i++)
                {
                    bounsTexts[i].text = "돈 " + bounsRewards[i];
                }

                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Bouns" } });
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Bouns", true } });
                PV.RPC("PlayBouns", RpcTarget.All);

                GameStateManager.instance.CheckBouns = false;
            }
            else
            {

                PV.RPC("SelectRoulette", RpcTarget.All, rouletteIndex);
            }
        }
    }

    [PunRPC]
    void SelectRoulette(int number)
    {
        uIManager.OpenBounsView(false);

        roulette3D.SetActive(true);

        if(!PhotonNetwork.IsMasterClient && pinball == null)
        {
            pinball = GameObject.FindWithTag("Pinball").GetComponent<Pinball3D>();
            pinball.transform.parent = roulette3D.transform;
            pinball.rouletteManager = this;

            leftClock[0] = GameObject.FindWithTag("ClockSecObj_Left").GetComponent<Rotation_Clock>();
            leftClock[1] = GameObject.FindWithTag("ClockMinObj_Left").GetComponent<Rotation_Clock>();
            leftClock[2] = GameObject.FindWithTag("ClockQueenObj_Left").GetComponent<Rotation_Clock>();

            leftQueen = leftClock[2].meshRenderer;
            leftQueenPoint = leftClock[2].queenPoint;

            rightClock[0] = GameObject.FindWithTag("ClockSecObj_Right").GetComponent<Rotation_Clock>();
            rightClock[1] = GameObject.FindWithTag("ClockMinObj_Right").GetComponent<Rotation_Clock>();
            rightClock[2] = GameObject.FindWithTag("ClockQueenObj_Right").GetComponent<Rotation_Clock>();

            rightQueen = rightClock[2].meshRenderer;
            rightQueenPoint = rightClock[2].queenPoint;

            for (int i = 0; i < leftClock.Length; i++)
            {
                leftClock[i].transform.parent = roulette1Obj.transform;
                rightClock[i].transform.parent = roulette2Obj.transform;
            }
        }

        pinballIndex = number;

        for (int i = 0; i < roulette1Particle.Length; i++)
        {
            roulette1Particle[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < roulette2Particle.Length; i++)
        {
            roulette2Particle[i].gameObject.SetActive(false);
        }

        rouletteCamera.gameObject.SetActive(true);
        rouletteCamera.Initialize(startCameraPos);

        roulette1Obj.gameObject.SetActive(true);
        roulette2Obj.gameObject.SetActive(true);

        roulette1Mesh.materials[1].color = Color.white;
        roulette1Mesh.materials[2].color = Color.white;

        roulette2Mesh.materials[1].color = Color.white;
        roulette2Mesh.materials[2].color = Color.white;


        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            leftPointerManager.Initialize_NewBie();
            rightPointerManager.Initialize_NewBie();

            roulette1Mesh.materials[2].color = Color.black;
            roulette2Mesh.materials[2].color = Color.black;
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

    public void ShowBettingNumber()
    {
        leftQueen.material.color = Color.white;
        rightQueen.material.color = Color.white;

        colorNumber = 0;
        colorNumber2 = 0;

        for (int i = 0; i < leftPointerManager.pointerList.Count; i++)
        {
            leftPointerManager.pointerList[i].Betting_Newbie(0);
            rightPointerManager.pointerList[i].Betting_Newbie(0);
        }

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            if (gameManager.bettingNumberList_NewBie[0] == 1 && gameManager.otherBettingNumberList_Newbie[0] == 1)
            {
                colorNumber = 1;
            }
            else if (gameManager.bettingNumberList_NewBie[0] == 1)
            {
                colorNumber = 2;
            }
            else if (gameManager.otherBettingNumberList_Newbie[0] == 1)
            {
                colorNumber = 3;
            }

            if (gameManager.bettingNumberList_NewBie[1] == 1 && gameManager.otherBettingNumberList_Newbie[1] == 1)
            {
                colorNumber2 = 1;
            }
            else if (gameManager.bettingNumberList_NewBie[1] == 1)
            {
                colorNumber2 = 2;
            }
            else if (gameManager.otherBettingNumberList_Newbie[1] == 1)
            {
                colorNumber2 = 3;
            }

            for (int i = 0; i < leftPointerManager.pointerList.Count; i++)
            {
                if (leftPointerManager.pointerList[i].index % 2 == 0)
                {
                    leftPointerManager.pointerList[i].Betting_Newbie(colorNumber);
                    rightPointerManager.pointerList[i].Betting_Newbie(colorNumber);
                }

                if (leftPointerManager.pointerList[i].index % 2 != 0)
                {
                    leftPointerManager.pointerList[i].Betting_Newbie(colorNumber2);
                    rightPointerManager.pointerList[i].Betting_Newbie(colorNumber2);
                }
            }

            if (gameManager.bettingNumberList_NewBie[2] == 1 && gameManager.otherBettingNumberList_Newbie[2] == 1)
            {
                leftQueen.material.color = Color.green;
                rightQueen.material.color = Color.green;
            }
            else if (gameManager.bettingNumberList_NewBie[2] == 1)
            {
                leftQueen.material.color = Color.blue;
                rightQueen.material.color = Color.blue;
            }
            else if (gameManager.otherBettingNumberList_Newbie[2] == 1)
            {
                leftQueen.material.color = Color.red;
                rightQueen.material.color = Color.red;
            }

        }
        else
        {
            for (int i = 0; i < gameManager.bettingNumberList_Gosu.Count; i++)
            {
                if (rouletteIndex == 0)
                {
                    for (int j = 0; j < leftPointerManager.pointerList.Count; j++)
                    {
                        if (gameManager.bettingNumberList_Gosu[i] == leftPointerManager.pointerList[j].index)
                        {
                            leftPointerManager.pointerList[j].Betting_Gosu();
                        }

                        if (gameManager.bettingNumberList_Gosu.Contains(13) && gameManager.otherBettingNumberList_Gosu.Contains(13))
                        {
                            leftQueen.material.color = Color.green;
                        }
                        else if (gameManager.bettingNumberList_Gosu.Contains(13))
                        {
                            leftQueen.material.color = Color.blue;
                        }
                        else if(gameManager.otherBettingNumberList_Gosu.Contains(13))
                        {
                            leftQueen.material.color = Color.red;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < rightPointerManager.pointerList.Count; j++)
                    {
                        if (gameManager.bettingNumberList_Gosu[i] == rightPointerManager.pointerList[j].index)
                        {
                            rightPointerManager.pointerList[j].Betting_Gosu();
                        }

                        if (gameManager.bettingNumberList_Gosu.Contains(13) && gameManager.otherBettingNumberList_Gosu.Contains(13))
                        {
                            rightQueen.material.color = Color.green;
                        }
                        else if (gameManager.bettingNumberList_Gosu.Contains(13))
                        {
                            rightQueen.material.color = Color.blue;
                        }
                        else if (gameManager.otherBettingNumberList_Gosu.Contains(13))
                        {
                            rightQueen.material.color = Color.red;
                        }
                    }
                }
            }

            for (int i = 0; i < gameManager.otherBettingNumberList_Gosu.Count; i++)
            {
                if (rouletteIndex == 0)
                {
                    for (int j = 0; j < leftPointerManager.pointerList.Count; j++)
                    {
                        if (gameManager.bettingNumberList_Gosu[i] == leftPointerManager.pointerList[j].index)
                        {
                            leftPointerManager.pointerList[j].Betting_Gosu_Other();
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < leftPointerManager.pointerList.Count; j++)
                    {
                        if (gameManager.bettingNumberList_Gosu[i] == leftPointerManager.pointerList[j].index)
                        {
                            rightPointerManager.pointerList[j].Betting_Gosu_Other();
                        }
                    }
                }
            }
        }
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

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        if(ht["WindCharacter"].Equals("0")) //그대로
        {
            if(PhotonNetwork.IsMasterClient)
            {
                windIndex = 0;
            }
            else
            {
                windIndex = 1;
            }
        }
        else //A번 B번 위치 변경
        {
            if (PhotonNetwork.IsMasterClient)
            {
                windIndex = 1;
            }
            else
            {
                windIndex = 0;
            }
        }

        characterManager.CheckMyTurn(ht["Pinball"].ToString());

        aiMode = false;

        if (ht["Pinball"].Equals(GameStateManager.instance.NickName)) //각자 내 차례인지 확인하도록!
        {
            pinball.MyTurn(pinballIndex);

            buttonText.text = "당신은 " + (windIndex + 1) + "번째 위치입니다.\n꾹 눌러서 바람 게이지를 조절하세요!";

            NotionManager.instance.UseNotion(NotionType.YourTurn);
        }
        else
        {
            if(windIndex == 0)
            {          
                Debug.Log("Ai가 2번째 자리에서 바람을 불려고 합니다.");
            }
            else
            {
                Debug.Log("Ai가 1번째 자리에서 바람을 불려고 합니다.");
            }

            pinball.MyTurn(pinballIndex);

            aiMode = true;

            StartCoroutine(AiBlowWindCoroution());

            buttonText.text = "다음 차례에 바람을 불 수 있습니다.";
        }

        if (PhotonNetwork.IsMasterClient) //다음 사람 설정
        {
            if (PhotonNetwork.PlayerList.Length >= 2) //혼자가 아닐경우
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
            else
            {
                if(ht["Pinball"].Equals(GameStateManager.instance.NickName))
                {
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Pinball", "인공지능" } });

                    Debug.Log("Ai 대전 다음 사람 : 인공지능");
                }
                else
                {
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Pinball", GameStateManager.instance.NickName } });

                    Debug.Log("Ai 대전 다음 사람 : " + GameStateManager.instance.NickName);
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
        if (aiMode) return;

        if (!pinball.PV.IsMine || buttonClick) return;

        pinballPower = true;

        StartCoroutine(PowerCoroution());
    }

    public void BlowWindUp()
    {
        if (aiMode) return;

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

        if (rouletteIndex == 0)
        {
            for (int i = 0; i < roulette1Particle.Length; i++)
            {
                roulette1Particle[i].gameObject.SetActive(true);
                roulette1Particle[i].Play();
            }
        }
        else
        {
            for (int i = 0; i < roulette2Particle.Length; i++)
            {
                roulette2Particle[i].gameObject.SetActive(true);
                roulette2Particle[i].Play();
            }
        }
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

        if (rouletteIndex == 0)
        {
            for (int i = 0; i < roulette1Particle.Length; i++)
            {
                roulette1Particle[i].gameObject.SetActive(true);
                roulette1Particle[i].Play();
            }
        }
        else
        {
            for (int i = 0; i < roulette2Particle.Length; i++)
            {
                roulette2Particle[i].gameObject.SetActive(true);
                roulette2Particle[i].Play();
            }
        }
    }

    [PunRPC]
    void GameResult(string[] target)
    {
        gameManager.GameResult(target);
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

        gameManager.money += number;
        gameManager.otherMoney += number;

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

    #region Ai
    IEnumerator AiBlowWindCoroution()
    {
        yield return new WaitForSeconds(Random.Range(10, 15));

        while(!buttonClick)
        {
            if (windIndex == 0 && pinball.ballPos > 2)
            {
                float[] blow = new float[2];
                blow[0] = Random.Range(2, 100);
                blow[1] = 1;

                PV.RPC("BlowingWind", RpcTarget.All, blow);

                buttonClick = true;

                Debug.Log("Ai가 2번째 자리에서 바람을 불었습니다");
            }
            else if (windIndex == 1 && pinball.ballPos < 3)
            {
                float[] blow = new float[2];
                blow[0] = Random.Range(2, 100);
                blow[1] = 0;

                PV.RPC("BlowingWind", RpcTarget.All, blow);

                buttonClick = true;

                Debug.Log("Ai가 1번째 자리에서 바람을 불었습니다");
            }
            yield return waitForSeconds2;
        }
    }
    #endregion
}
