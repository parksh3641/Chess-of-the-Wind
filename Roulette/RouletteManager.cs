using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RouletteManager : MonoBehaviour
{
    public MoveCamera rouletteCamera;
    public FollowCamera rouletteBallCamera;

    public GameObject roulette3D;

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
    public GameObject[] roulettePlane;
    public Transform[] roulette1Obj;
    public Transform[] roulette2Obj;

    public MeshRenderer[] roulette1WindPoint;
    public MeshRenderer[] roulette2WindPoint;

    public ParticleSystem roulette1Particle;
    public ParticleSystem roulette2Particle;

    public GameObject[] vectorArray;

    public GameObject windGauge;
    public Image windButton;

    public Sprite[] windButtonArray;

    public Image player1Img;
    public Image player2Img;

    Sprite[] blockArray;

    [Space]
    [Title("Finger")]
    public FingerController leftFingerController;
    public FingerController rightFingerController;

    public PointerManager[] leftPointerManager;
    public PointerManager[] rightPointerManager;

    public PointerManager mainLeftPointerManager;
    public PointerManager mainRightPointerManager;

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
    private int upPower = 3;
    private int maxPower = 150;

    bool windDelay = false;
    bool pinballPower = false;
    bool pinballPowerReturn = false;

    [Title("Target")]
    public GameObject targetView;
    public GameObject targetNormal;
    public GameObject targetQueen;
    public Text targetText;
    public Text buttonText;

    public GameObject normalEffect;
    public GameObject queenEffect;

    [Space]
    [Title("Value")]
    private int rouletteIndex = 0;
    private int windIndex = 0;

    private int leftNumber = 0;
    private int rightNumber = 0;

    private int targetNumber = 0;
    private int targetQueenNumber = 0;
    private int queenNumber = 0;

    private int aiTargetNumber = 0;

    [Space]
    [Title("bool")]
    bool buttonClick = false;
    bool bouns = false;
    bool aiMode = false;
    bool playing = false;

    [Space]
    public int[] bettingPos0, bettingPos1, bettingPos2, bettingPos3, bettingPos4, bettingPos5;
    public bool bettingPosCheck0, bettingPosCheck1, bettingPosCheck2, bettingPosCheck3, bettingPosCheck4, bettingPosCheck5;

    [Space]
    public GameManager gameManager;
    public UIManager uIManager;
    public CharacterManager characterManager;
    public WindCharacterManager windCharacterManager;

    ImageDataBase imageDataBase;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.5f);

    public PhotonView PV;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        blockArray = imageDataBase.GetBlockArray();

        buttonClick = false;
        bouns = false;

        rouletteCamera.gameObject.SetActive(false);
        rouletteBallCamera.gameObject.SetActive(false);
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
        rouletteBallCamera.SetBall(pinball.gameObject);

        for (int i = 0; i < leftClock.Length; i++)
        {
            if(leftClock[i] != null)
            {
                PhotonNetwork.Destroy(leftClock[i].gameObject);
                PhotonNetwork.Destroy(rightClock[i].gameObject);
            }
        }

        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            leftClock[0] = PhotonNetwork.Instantiate("ClockSecObj_Left", roulette1Obj[0].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
            leftClock[1] = PhotonNetwork.Instantiate("ClockMinObj_Left", roulette1Obj[0].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
            leftClock[2] = PhotonNetwork.Instantiate("ClockQueenObj_Left", roulette1Obj[0].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();

            leftQueen = leftClock[2].meshRenderer;
            leftQueenPoint = leftClock[2].queenPoint;

            rightClock[0] = PhotonNetwork.Instantiate("ClockSecObj_Right", roulette2Obj[0].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
            rightClock[1] = PhotonNetwork.Instantiate("ClockMinObj_Right", roulette2Obj[0].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
            rightClock[2] = PhotonNetwork.Instantiate("ClockQueenObj_Right", roulette2Obj[0].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();

            rightQueen = rightClock[2].meshRenderer;
            rightQueenPoint = rightClock[2].queenPoint;

            for (int i = 0; i < leftClock.Length; i++)
            {
                leftClock[i].transform.parent = roulette1Obj[0].transform;
                rightClock[i].transform.parent = roulette2Obj[0].transform;
            }

            leftClock[0].transform.localPosition = new Vector3(0, 0.03f, 0);
            leftClock[1].transform.localPosition = new Vector3(0, 0.03f, 0);
            leftClock[2].transform.localPosition = new Vector3(0, 0.03f, 0);

            rightClock[0].transform.localPosition = new Vector3(0, 0.03f, 0);
            rightClock[1].transform.localPosition = new Vector3(0, 0.03f, 0);
            rightClock[2].transform.localPosition = new Vector3(0, 0.03f, 0);
        }
        else
        {
            leftClock[0] = PhotonNetwork.Instantiate("ClockSecObj_Left", roulette1Obj[1].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
            leftClock[1] = PhotonNetwork.Instantiate("ClockMinObj_Left", roulette1Obj[1].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
            leftClock[2] = PhotonNetwork.Instantiate("ClockQueenObj_Left", roulette1Obj[1].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();

            leftQueen = leftClock[2].meshRenderer;
            leftQueenPoint = leftClock[2].queenPoint;

            rightClock[0] = PhotonNetwork.Instantiate("ClockSecObj_Right", roulette2Obj[1].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
            rightClock[1] = PhotonNetwork.Instantiate("ClockMinObj_Right", roulette2Obj[1].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();
            rightClock[2] = PhotonNetwork.Instantiate("ClockQueenObj_Right", roulette2Obj[1].transform.localPosition, Quaternion.identity, 0).GetComponent<Rotation_Clock>();

            rightQueen = rightClock[2].meshRenderer;
            rightQueenPoint = rightClock[2].queenPoint;

            for (int i = 0; i < leftClock.Length; i++)
            {
                leftClock[i].transform.parent = roulette1Obj[1].transform;
                rightClock[i].transform.parent = roulette2Obj[1].transform;
            }
        }

        Debug.Log("포톤 오브젝트 재생성 완료");
    }

    public void Initialize()
    {
        uIManager.OpenRouletteView();
        uIManager.CloseSurrenderView();

        if (gameManager.blockType != BlockType.Default)
        {
            player2Img.enabled = true;
            player2Img.sprite = blockArray[(int)gameManager.blockType - 1];
        }
        else
        {
            player2Img.enabled = false;
        }

        if (gameManager.otherBlockType != BlockType.Default)
        {
            player1Img.enabled = true;
            player1Img.sprite = blockArray[(int)gameManager.otherBlockType - 1];
        }
        else
        {
            player1Img.enabled = false;
        }

        characterManager.ResetFocus();

        rouletteCamera.gameObject.SetActive(true);
        roulette3D.SetActive(false);

        roulettePlane[0].SetActive(false);
        roulettePlane[1].SetActive(false);

        roulette1Obj[0].gameObject.SetActive(false);
        roulette2Obj[0].gameObject.SetActive(false);

        roulette1Obj[1].gameObject.SetActive(false);
        roulette2Obj[1].gameObject.SetActive(false);

        roulette1WindPoint[0].gameObject.SetActive(false);
        roulette1WindPoint[1].gameObject.SetActive(false);

        roulette2WindPoint[0].gameObject.SetActive(false);
        roulette2WindPoint[1].gameObject.SetActive(false);

        vectorArray[0].SetActive(false);
        vectorArray[1].SetActive(false);

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            roulettePlane[0].SetActive(true);
            windGauge.SetActive(false);
        }
        else
        {
            roulettePlane[1].SetActive(true);
            windGauge.SetActive(true);
        }

        queenNumber = 0;

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            queenNumber = 5;
        }
        else
        {
            queenNumber = 13;
        }

        windCharacterManager.Stop();

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

            //bool check = (bool)ht["Bouns"];

            //int total = GameStateManager.instance.Stakes * 2;
            //int minus = int.Parse(ht["Player1_Minus"].ToString()) + int.Parse(ht["Player2_Minus"].ToString());

            //Debug.Log("보너스 룰렛 체크 : " + total + " / " + minus + " / " + check);

            //if (total * 0.6f <= minus || GameStateManager.instance.CheckBouns && !check) //공중 분해된 돈이 전체 판돈에 60% 이상일 경우 매판 1번 등장.
            //{
            //    bounsRewards[0] = (int)(minus * 0.1f);
            //    bounsRewards[1] = (int)(minus * 0.2f);
            //    bounsRewards[2] = (int)(minus * 0.3f);
            //    bounsRewards[3] = (int)(minus * 0.5f);

            //    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Bouns" } });
            //    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Bouns", true } });

            //    PV.RPC("PlayBouns", RpcTarget.All, bounsRewards);

            //    GameStateManager.instance.CheckBouns = false;
            //}
            //else
            //{

            //    PV.RPC("SelectRoulette", RpcTarget.All, rouletteIndex);
            //}

            PV.RPC("SelectRoulette", RpcTarget.All, rouletteIndex);
        }
    }

    [PunRPC]
    void SelectRoulette(int number)
    {
        rouletteIndex = number;

        uIManager.OpenBounsView(false);

        roulette3D.SetActive(true);

        if (!PhotonNetwork.IsMasterClient && pinball == null)
        {
            pinball = GameObject.FindWithTag("Pinball").GetComponent<Pinball3D>();
            pinball.transform.parent = roulette3D.transform;
            pinball.rouletteManager = this;

            rouletteBallCamera.SetBall(pinball.gameObject);

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

            if (GameStateManager.instance.GameType == GameType.NewBie)
            {
                for (int i = 0; i < leftClock.Length; i++)
                {
                    leftClock[i].transform.parent = roulette1Obj[0].transform;
                    rightClock[i].transform.parent = roulette2Obj[0].transform;
                }
            }
            else
            {
                for (int i = 0; i < leftClock.Length; i++)
                {
                    leftClock[i].transform.parent = roulette1Obj[1].transform;
                    rightClock[i].transform.parent = roulette2Obj[1].transform;
                }
            }

            leftClock[0].transform.localPosition = new Vector3(0, 0.03f, 0);
            leftClock[1].transform.localPosition = new Vector3(0, 0.03f, 0);
            leftClock[2].transform.localPosition = new Vector3(0, 0.03f, 0);

            rightClock[0].transform.localPosition = new Vector3(0, 0.03f, 0);
            rightClock[1].transform.localPosition = new Vector3(0, 0.03f, 0);
            rightClock[2].transform.localPosition = new Vector3(0, 0.03f, 0);
        }

        roulette1Particle.gameObject.SetActive(false);
        roulette2Particle.gameObject.SetActive(false);

        pinball.transform.localPosition = new Vector3(5000, 5000);

        rouletteCamera.gameObject.SetActive(true);
        rouletteCamera.Initialize(startCameraPos);
        rouletteBallCamera.gameObject.SetActive(false);

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            roulette1Obj[0].gameObject.SetActive(true);
            roulette2Obj[0].gameObject.SetActive(true);

            leftPointerManager[0].Initialize(0);
            rightPointerManager[0].Initialize(0);

            mainLeftPointerManager = leftPointerManager[0];
            mainRightPointerManager = rightPointerManager[0];
        }
        else
        {
            roulette1Obj[1].gameObject.SetActive(true);
            roulette2Obj[1].gameObject.SetActive(true);

            leftPointerManager[1].Initialize(leftNumber);
            rightPointerManager[1].Initialize(rightNumber);

            mainLeftPointerManager = leftPointerManager[1];
            mainRightPointerManager = rightPointerManager[1];
        }

        leftFingerController.gameObject.SetActive(true);
        rightFingerController.gameObject.SetActive(true);

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

        for (int i = 0; i < mainLeftPointerManager.pointerList.Count; i++)
        {
            mainLeftPointerManager.pointerList[i].Betting_Initialize();
            mainRightPointerManager.pointerList[i].Betting_Initialize();
        }

        for (int i = 0; i < gameManager.bettingNumberList.Count; i++)
        {
            if (rouletteIndex == 0)
            {
                for (int j = 0; j < mainLeftPointerManager.pointerList.Count; j++)
                {
                    if (gameManager.bettingNumberList[i] > queenNumber)
                    {
                        if (gameManager.bettingNumberList[i] - 1 == mainLeftPointerManager.pointerList[j].index)
                        {
                            mainLeftPointerManager.pointerList[j].Betting();
                        }
                    }
                    else if (gameManager.bettingNumberList[i] < queenNumber)
                    {
                        if (gameManager.bettingNumberList[i] == mainLeftPointerManager.pointerList[j].index)
                        {
                            mainLeftPointerManager.pointerList[j].Betting();
                        }
                    }

                    if (gameManager.bettingNumberList.Contains(queenNumber) && gameManager.otherBettingNumberList.Contains(queenNumber))
                    {
                        leftQueen.material.color = new Color(99 / 255f, 192 / 255f, 49 / 255f);
                    }
                    else if (gameManager.bettingNumberList.Contains(queenNumber))
                    {
                        leftQueen.material.color = new Color(35 / 255f, 141 / 255f, 241 / 255f);
                    }
                    else if (gameManager.otherBettingNumberList.Contains(queenNumber))
                    {
                        leftQueen.material.color = new Color(200 / 255f, 52 / 255f, 92 / 255f);
                    }
                }
            }
            else
            {
                for (int j = 0; j < mainRightPointerManager.pointerList.Count; j++)
                {
                    if (gameManager.bettingNumberList[i] > queenNumber)
                    {
                        if (gameManager.bettingNumberList[i] - 1 == mainRightPointerManager.pointerList[j].index)
                        {
                            mainRightPointerManager.pointerList[j].Betting();
                        }
                    }
                    else if(gameManager.bettingNumberList[i] < queenNumber)
                    {
                        if (gameManager.bettingNumberList[i] == mainRightPointerManager.pointerList[j].index)
                        {
                            mainRightPointerManager.pointerList[j].Betting();
                        }
                    }

                    if (gameManager.bettingNumberList.Contains(queenNumber) && gameManager.otherBettingNumberList.Contains(queenNumber))
                    {
                        rightQueen.material.color = new Color(99 / 255f, 192 / 255f, 49 / 255f);
                    }
                    else if (gameManager.bettingNumberList.Contains(queenNumber))
                    {
                        rightQueen.material.color = new Color(35 / 255f, 141 / 255f, 241 / 255f);
                    }
                    else if (gameManager.otherBettingNumberList.Contains(queenNumber))
                    {
                        rightQueen.material.color = new Color(200 / 255f, 52 / 255f, 92 / 255f);
                    }
                }
            }
        }

        for (int i = 0; i < gameManager.otherBettingNumberList.Count; i++)
        {
            if (rouletteIndex == 0)
            {
                for (int j = 0; j < mainLeftPointerManager.pointerList.Count; j++)
                {
                    if (gameManager.otherBettingNumberList[i] > queenNumber)
                    {
                        if (gameManager.otherBettingNumberList[i] - 1 == mainLeftPointerManager.pointerList[j].index)
                        {
                            mainLeftPointerManager.pointerList[j].Betting_Other();
                        }
                    }
                    else if(gameManager.otherBettingNumberList[i] < queenNumber)
                    {
                        if (gameManager.otherBettingNumberList[i] == mainLeftPointerManager.pointerList[j].index)
                        {
                            mainLeftPointerManager.pointerList[j].Betting_Other();
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < mainRightPointerManager.pointerList.Count; j++)
                {
                    if (gameManager.otherBettingNumberList[i] > queenNumber)
                    {
                        if (gameManager.otherBettingNumberList[i] - 1 == mainRightPointerManager.pointerList[j].index)
                        {
                            mainRightPointerManager.pointerList[j].Betting_Other();
                        }
                    }
                    else if(gameManager.otherBettingNumberList[i] < queenNumber)
                    {
                        if (gameManager.otherBettingNumberList[i] == mainRightPointerManager.pointerList[j].index)
                        {
                            mainRightPointerManager.pointerList[j].Betting_Other();
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

            StartCoroutine(InitializeWindCharacter1(roulette1Obj[0]));
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

            StartCoroutine(InitializeWindCharacter2(roulette2Obj[0]));
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Roulette" } });
            PV.RPC("PlayRoulette", RpcTarget.All);
        }
    }

    IEnumerator InitializeWindCharacter1(Transform target)
    {
        windCharacterManager.Initialize(target);

        roulette2Obj[0].gameObject.SetActive(false);
        roulette2Obj[1].gameObject.SetActive(false);
        rightFingerController.gameObject.SetActive(false);
        leftFingerController.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        buttonClick = false;
    }

    IEnumerator InitializeWindCharacter2(Transform target)
    {
        windCharacterManager.Initialize(target);

        roulette1Obj[0].gameObject.SetActive(false);
        roulette1Obj[1].gameObject.SetActive(false);
        rightFingerController.gameObject.SetActive(false);
        leftFingerController.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        buttonClick = false;
    }

    IEnumerator CheckPinBallCoroution()
    {
        yield return new WaitForSeconds(3);

        if(pinball.PV.IsMine)
        {
            bool check = false;

            while (playing && !check)
            {
                if (pinball.rigid.velocity.magnitude < 0.1f)
                {
                    rouletteCamera.gameObject.SetActive(true);
                    rouletteBallCamera.gameObject.SetActive(false);

                    PV.RPC("CheckPinball", RpcTarget.Others);

                    buttonClick = true;
                    check = true;
                }

                yield return waitForSeconds2;
            }
        }

        while (playing)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2);

        rouletteCamera.gameObject.SetActive(true);
        rouletteBallCamera.gameObject.SetActive(false);

        //if (rouletteIndex == 0) //폭죽
        //{
        //    for (int i = 0; i < roulette1Particle.Length; i++)
        //    {
        //        roulette1Particle[i].gameObject.SetActive(true);
        //        roulette1Particle[i].Play();
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < roulette2Particle.Length; i++)
        //    {
        //        roulette2Particle[i].gameObject.SetActive(true);
        //        roulette2Particle[i].Play();
        //    }
        //}
    }

    [PunRPC]
    void CheckPinball()
    {
        rouletteCamera.gameObject.SetActive(true);
        rouletteBallCamera.gameObject.SetActive(false);
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

        windDelay = false;
        pinballPower = false;
        pinballPowerReturn = false;

        SoundManager.instance.PlayBGMLow();
        SoundManager.instance.PlayLoopSFX(GameSfxType.Roulette);

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

        windButton.sprite = windButtonArray[0];

        vectorArray[0].SetActive(false);
        vectorArray[1].SetActive(false);

        if (ht["Pinball"].Equals(GameStateManager.instance.NickName)) //각자 내 차례인지 확인하도록!
        {
            pinball.MyTurn(rouletteIndex);

            windCharacterManager.MyWhich(windIndex);

            windButton.sprite = windButtonArray[1];

            //vectorArray[windIndex].SetActive(true);

            if(GameStateManager.instance.GameType == GameType.NewBie)
            {
                buttonText.text = LocalizationManager.instance.GetString("Newbie_MyTurn");
            }
            else
            {
                buttonText.text = LocalizationManager.instance.GetString("Gosu_MyTurn");
            }

            NotionManager.instance.UseNotion(NotionType.YourTurn);
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                if (windIndex == 0)
                {
                    Debug.Log("Ai가 2번째 자리에서 바람을 불려고 합니다.");
                }
                else
                {
                    Debug.Log("Ai가 1번째 자리에서 바람을 불려고 합니다.");
                }

                pinball.MyTurn(rouletteIndex);

                aiMode = true;
                StartCoroutine(BlowWindCoroution_Ai());
            }

            windCharacterManager.MyWhich(1 - windIndex);

            NotionManager.instance.UseNotion(NotionType.EnemyTurn);

            buttonText.text = LocalizationManager.instance.GetString("EnemyTurn");
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

        playing = true;
        StartCoroutine(CheckPinBallCoroution());
    }

    public void PlayRouletteDelay()
    {
        if (pinball.PV.IsMine)
        {
            if (!aiMode)
            {
                vectorArray[windIndex].SetActive(true);
            }
        }
    }

    [PunRPC]
    void PlayBouns(int[] bounsReward)
    {
        for (int i = 0; i < bounsTexts.Length; i++)
        {
            bounsTexts[i].text = "돈 " + bounsReward[i];
        }

        targetView.SetActive(false);
        uIManager.OpenBounsView(true);

        buttonClick = true;
        bouns = true;

        if (PhotonNetwork.IsMasterClient)
        {
            bounsRoulette.StartRoulette();
            StartCoroutine(BounsRouletteCoroution());

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Minus", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Minus", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player3_Minus", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player4_Minus", 0 } });
        }

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

        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            if (!windDelay)
            {
                float[] blow = new float[2];
                blow[0] = 15;
                blow[1] = windIndex;

                PV.RPC("BlowingWind", RpcTarget.All, blow);

                windDelay = true;
                Invoke("WindDelay", 0.5f);
            }
        }
        else
        {
            pinballPower = true;

            StartCoroutine(PowerCoroution());

            if (rouletteIndex == 0)
            {
                if (windIndex == 0)
                {
                    roulette1WindPoint[0].gameObject.SetActive(true);

                    StartCoroutine(FlickerCoroution(roulette1WindPoint[0]));
                }
                else
                {
                    roulette1WindPoint[1].gameObject.SetActive(true);

                    StartCoroutine(FlickerCoroution(roulette1WindPoint[1]));
                }
            }
            else
            {
                if (windIndex == 0)
                {
                    roulette2WindPoint[0].gameObject.SetActive(true);

                    StartCoroutine(FlickerCoroution(roulette2WindPoint[0]));
                }
                else
                {
                    roulette2WindPoint[1].gameObject.SetActive(true);

                    StartCoroutine(FlickerCoroution(roulette2WindPoint[1]));
                }
            }
        }
    }

    void WindDelay()
    {
        windDelay = false;
    }

    IEnumerator FlickerCoroution(MeshRenderer mesh)
    {
        float alpha = 0;
        bool check = false;

        while(pinballPower)
        {
            if(!check)
            {
                if(alpha < 60)
                {
                    alpha += 2;
                }
                else
                {
                    alpha = 60;

                    check = true;
                }
            }
            else
            {
                if (alpha > 0)
                {
                    alpha -= 2;
                }
                else
                {
                    alpha = 0;

                    mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 0);

                    check = false;

                    yield return waitForSeconds2;
                }
            }

            mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, alpha / 255);

            yield return waitForSeconds;
        }
    }

    public void BlowWindUp()
    {
        if (aiMode) return;

        if (!pinball.PV.IsMine || buttonClick) return;

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            return;
        }

        buttonClick = true;
        pinballPower = false;

        float[] blow = new float[2];
        blow[0] = power;
        blow[1] = windIndex;

        PV.RPC("BlowingWind", RpcTarget.All, blow);

        roulette1WindPoint[0].gameObject.SetActive(false);
        roulette1WindPoint[1].gameObject.SetActive(false);

        roulette2WindPoint[0].gameObject.SetActive(false);
        roulette2WindPoint[1].gameObject.SetActive(false);

        windButton.sprite = windButtonArray[0];
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

        SoundManager.instance.StopSFX(GameSfxType.BlowWind);
        SoundManager.instance.PlaySFX(GameSfxType.BlowWind);
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

        yield return new WaitForSeconds(2f);

        targetNumber = 0;

        if (rouletteIndex == 0)
        {
            targetNumber = mainLeftPointerManager.CheckNumber(pinball.transform);

            if(GameStateManager.instance.GameType == GameType.NewBie)
            {
                leftQueenPoint.parent = roulette1Obj[0];
            }
            else
            {
                leftQueenPoint.parent = roulette1Obj[1];
            }
            targetQueenNumber = mainLeftPointerManager.CheckQueenNumber(leftQueenPoint);
            leftQueenPoint.parent = leftClock[2].transform;
        }
        else
        {
            targetNumber = mainRightPointerManager.CheckNumber(pinball.transform);

            if (GameStateManager.instance.GameType == GameType.NewBie)
            {
                rightQueenPoint.parent = roulette2Obj[0];
            }
            else
            {
                rightQueenPoint.parent = roulette2Obj[1];
            }
            targetQueenNumber = mainRightPointerManager.CheckQueenNumber(rightQueenPoint);
            rightQueenPoint.parent = rightClock[2].transform;
        }

        if (leftNumber + 1 > 23)
        {
            leftNumber = 0;
        }
        else
        {
            leftNumber++;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "LeftNumber", leftNumber } });

        if (rightNumber + 1 > 23)
        {
            rightNumber = 0;
        }
        else
        {
            rightNumber++;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "RightNumber", rightNumber } });

        while (targetNumber == 0)
        {
            yield return null;
        }

        if(targetNumber != targetQueenNumber)
        {
            PV.RPC("ShowTargetNumberNormal", RpcTarget.All, targetNumber);
        }
        else
        {
            PV.RPC("ShowTargetNumberQueen", RpcTarget.All, targetNumber);
        }

        yield return new WaitForSeconds(2.5f);

        PV.RPC("PlayParticle", RpcTarget.All);

        yield return new WaitForSeconds(2.5f);

        if(GameStateManager.instance.Vibration)
        {
            Handheld.Vibrate();
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Waiting" } });

        string[] target = new string[2];

        target[0] = targetNumber.ToString();
        target[1] = "0";

        if (targetNumber == targetQueenNumber)
        {
            target[1] = "1";
        }

        PV.RPC("GameResult", RpcTarget.All, target);
    }

    [PunRPC]
    void PlayParticle()
    {
        //if (rouletteIndex == 0)
        //{
        //    for (int i = 0; i < roulette1Particle.Length; i++)
        //    {
        //        roulette1Particle[i].gameObject.SetActive(true);
        //        roulette1Particle[i].Play();
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < roulette2Particle.Length; i++)
        //    {
        //        roulette2Particle[i].gameObject.SetActive(true);
        //        roulette2Particle[i].Play();
        //    }
        //}
    }

    [PunRPC]
    void EndRoulette()
    {
        buttonClick = true;
        playing = false;

        SoundManager.instance.PlayBGM();
        SoundManager.instance.StopLoopSFX(GameSfxType.Roulette);

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
    void ShowTargetNumberNormal(int number)
    {
        targetView.SetActive(true);
        targetNormal.SetActive(true);
        targetQueen.SetActive(false);


        if (!PhotonNetwork.IsMasterClient)
        {
            if (rouletteIndex == 0)
            {
                mainLeftPointerManager.ShowTarget(number, queenNumber);
            }
            else
            {
                mainRightPointerManager.ShowTarget(number, queenNumber);
            }
        }

        targetText.text = number.ToString();

        if (number >= queenNumber)
        {
            number += 1;
        }

        if (gameManager.bettingNumberList.Contains(number))
        {
            normalEffect.SetActive(true);

            SoundManager.instance.PlaySFX(GameSfxType.GetNumber);

            if (rouletteIndex == 0)
            {
                roulette1Particle.gameObject.SetActive(true);
                roulette1Particle.Play();
            }
            else
            {
                roulette2Particle.gameObject.SetActive(true);
                roulette2Particle.Play();
            }

            Debug.Log("숫자에 당첨되었습니다");
        }
        else
        {
            normalEffect.SetActive(false);

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            Debug.Log("숫자에 당첨되지 않았습니다");
        }
    }

    [PunRPC]
    void ShowTargetNumberQueen(int number)
    {
        targetView.SetActive(true);
        targetNormal.SetActive(false);
        targetQueen.SetActive(true);

        //targetText.text = number.ToString();

        if (gameManager.bettingNumberList.Contains(queenNumber))
        {
            queenEffect.SetActive(true);

            SoundManager.instance.PlaySFX(GameSfxType.GetQueen);

            if (rouletteIndex == 0)
            {
                roulette1Particle.gameObject.SetActive(true);
                roulette1Particle.Play();
            }
            else
            {
                roulette2Particle.gameObject.SetActive(true);
                roulette2Particle.Play();
            }

            Debug.Log("퀸에 당첨되었습니다");
        }
        else
        {
            queenEffect.SetActive(false);

            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            Debug.Log("퀸에 당첨되지 않았습니다");
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
    }

    [PunRPC]
    void GameResult(string[] target)
    {
        gameManager.GameResult(target);
        windCharacterManager.Stop();

        roulette1Obj[0].gameObject.SetActive(false);
        roulette1Obj[1].gameObject.SetActive(false);

        roulette2Obj[0].gameObject.SetActive(false);
        roulette2Obj[1].gameObject.SetActive(false);

        CloseRouletteView();
    }

    #region Bouns
    IEnumerator BounsRouletteCoroution()
    {
        while (bouns)
        {
            if (bounsRoulette.speed <= 0)
            {
                bouns = false;

                StartCoroutine(ResultBounsRoulette(bounsRoulette.GetRotate()));
            }
            yield return null;
        }
    }
    IEnumerator ResultBounsRoulette(int rotate)
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
        gameManager.moneyText.text = MoneyUnitString.ToCurrencyString(gameManager.money);

        gameManager.otherMoney += number;
        gameManager.otherMoneyText.text = MoneyUnitString.ToCurrencyString(gameManager.otherMoney);

        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, number);

        NotionManager.instance.UseNotion("보너스 룰렛 보상 : 돈 " + number + " 만큼 획득!", ColorType.Green);
    }

    #endregion

    #region Spectator

    //public void SpectatorRoulette()
    //{
    //    Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

    //    if (ht["Status"].ToString().Equals("Roulette"))
    //    {
    //        if (ht["Roulette"].ToString().Equals("Left"))
    //        {
    //            rouletteIndex = 0;
    //        }
    //        else
    //        {
    //            rouletteIndex = 1;
    //        }

    //        SelectRoulette(rouletteIndex);
    //    }
    //    else
    //    {
    //        PlayBouns();
    //    }
    //}

    #endregion

    #region Ai
    IEnumerator BlowWindCoroution_Ai()
    {
        SetBettingPos();

        buttonClick = false;

        yield return new WaitForSeconds(Random.Range(5, 10));

        while (!buttonClick)
        {
            if (windIndex == 0)
            {
                switch (pinball.ballPos)
                {
                    case 3:
                        if (bettingPosCheck0)
                        {
                            BlowWind_Ai(2, 30, 1);

                            Debug.Log("Ai가 2번 위치 - 1번째 자리에서 바람을 불었습니다");
                        }
                        else
                        {
                            if (!bettingPosCheck0 && !bettingPosCheck1 && !bettingPosCheck2)
                            {
                                BlowWind_Ai(2, 50, 1);

                                Debug.Log("바람이 닿지 않는 곳이라 2번 위치에서 아무데나 그냥 불었습니다");
                            }
                        }
                        break;
                    case 4:
                        if (bettingPosCheck1)
                        {
                            yield return new WaitForSeconds(0.2f);

                            BlowWind_Ai(2, 10, 1);

                            Debug.Log("Ai가 2번 위치 - 2번째 자리에서 바람을 불었습니다");
                        }
                        else
                        {
                            if (!bettingPosCheck0 && !bettingPosCheck1 && !bettingPosCheck2)
                            {
                                BlowWind_Ai(2, 50, 1);

                                Debug.Log("바람이 닿지 않는 곳이라 2번 위치에서 아무데나 그냥 불었습니다");
                            }
                        }
                        break;
                    case 5:
                        if (bettingPosCheck2)
                        {
                            yield return new WaitForSeconds(0.2f);

                            BlowWind_Ai(2, 30, 1);

                            Debug.Log("Ai가 2번 위치 - 3번째 자리에서 바람을 불었습니다");
                        }
                        else
                        {
                            if (!bettingPosCheck0 && !bettingPosCheck1 && !bettingPosCheck2)
                            {
                                BlowWind_Ai(2, 50, 1);

                                Debug.Log("바람이 닿지 않는 곳이라 2번 위치에서 아무데나 그냥 불었습니다");
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (pinball.ballPos)
                {
                    case 0:
                        if (bettingPosCheck3)
                        {
                            yield return new WaitForSeconds(0.2f);

                            BlowWind_Ai(2, 30, 0);

                            Debug.Log("Ai가 1번 위치 - 1번째 자리에서 바람을 불었습니다");
                        }
                        else
                        {
                            if (!bettingPosCheck3 && !bettingPosCheck4 && !bettingPosCheck5)
                            {
                                BlowWind_Ai(2, 50, 0);

                                Debug.Log("바람이 닿지 않는 곳이라 1번 위치에서 아무데나 불었습니다");
                            }
                        }
                        break;
                    case 1:
                        if (bettingPosCheck4)
                        {
                            yield return new WaitForSeconds(0.2f);

                            BlowWind_Ai(2, 10, 0);

                            Debug.Log("Ai가 1번 위치 - 2번째 자리에서 바람을 불었습니다");
                        }
                        else
                        {
                            if (!bettingPosCheck3 && !bettingPosCheck4 && !bettingPosCheck5)
                            {
                                BlowWind_Ai(2, 50, 0);

                                Debug.Log("바람이 닿지 않는 곳이라 1번 위치에서 아무데나 불었습니다");
                            }
                        }
                        break;
                    case 2:
                        if (bettingPosCheck5)
                        {
                            BlowWind_Ai(2, 30, 0);

                            Debug.Log("Ai가 1번 위치 - 3번째 자리에서 바람을 불었습니다");
                        }
                        else
                        {
                            if (!bettingPosCheck3 && !bettingPosCheck4 && !bettingPosCheck5)
                            {
                                BlowWind_Ai(2, 50, 0);

                                Debug.Log("바람이 닿지 않는 곳이라 1번 위치에서 아무데나 불었습니다");
                            }
                        }
                        break;
                }
            }

            yield return waitForSeconds2;
        }
    }

    void BlowWind_Ai(int min, int max, int index)
    {
        float[] blow = new float[2];
        blow[0] = Random.Range(min, max);
        blow[1] = index;

        PV.RPC("BlowingWind", RpcTarget.All, blow);

        if (GameStateManager.instance.GameType == GameType.Gosu)
        {
            buttonClick = true;
        }
    }

    void SetBettingPos()
    {
        bettingPosCheck0 = false;
        bettingPosCheck1 = false;
        bettingPosCheck2 = false;
        bettingPosCheck3 = false;
        bettingPosCheck4 = false;
        bettingPosCheck5 = false;

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            if(gameManager.otherBettingNumberList.Contains(1))
            {
                bettingPosCheck1 = true;
            }

            if (gameManager.otherBettingNumberList.Contains(2) || gameManager.otherBettingNumberList.Contains(3))
            {
                bettingPosCheck2 = true;
            }

            if (gameManager.otherBettingNumberList.Contains(4))
            {
                bettingPosCheck3 = true;
            }

            if (gameManager.otherBettingNumberList.Contains(5))
            {
                bettingPosCheck4 = true;
            }

            if (gameManager.otherBettingNumberList.Contains(6))
            {
                bettingPosCheck5 = true;
            }

            if (gameManager.otherBettingNumberList.Contains(7) || gameManager.otherBettingNumberList.Contains(8))
            {
                bettingPosCheck0 = true;
            }
        }
        else
        {
            bettingPos0 = new int[4];
            bettingPos1 = new int[4];
            bettingPos2 = new int[4];
            bettingPos3 = new int[4];
            bettingPos4 = new int[4];
            bettingPos5 = new int[4];

            aiTargetNumber = 0;

            if (rouletteIndex == 0)
            {
                aiTargetNumber = leftNumber;
            }
            else
            {
                aiTargetNumber = rightNumber;
            }

            SetPos(bettingPos1);
            SetPos(bettingPos2);
            SetPos(bettingPos3);
            SetPos(bettingPos4);
            SetPos(bettingPos5);
            SetPos(bettingPos0);

            for (int i = 0; i < bettingPos0.Length; i++)
            {
                if (gameManager.otherBettingNumberList.Contains(bettingPos0[i]))
                {
                    bettingPosCheck0 = true;
                    break;
                }
            }

            for (int i = 0; i < bettingPos1.Length; i++)
            {
                if (gameManager.otherBettingNumberList.Contains(bettingPos1[i]))
                {
                    bettingPosCheck1 = true;
                    break;
                }
            }

            for (int i = 0; i < bettingPos2.Length; i++)
            {
                if (gameManager.otherBettingNumberList.Contains(bettingPos2[i]))
                {
                    bettingPosCheck2 = true;
                    break;
                }
            }

            for (int i = 0; i < bettingPos3.Length; i++)
            {
                if (gameManager.otherBettingNumberList.Contains(bettingPos3[i]))
                {
                    bettingPosCheck5 = true;
                    break;
                }
            }

            for (int i = 0; i < bettingPos4.Length; i++)
            {
                if (gameManager.otherBettingNumberList.Contains(bettingPos4[i]))
                {
                    bettingPosCheck4 = true;
                    break;
                }
            }

            for (int i = 0; i < bettingPos5.Length; i++)
            {
                if (gameManager.otherBettingNumberList.Contains(bettingPos5[i]))
                {
                    bettingPosCheck3 = true;
                    break;
                }
            }
        }
    }

    void SetPos(int[] betting)
    {
        for (int i = 0; i < betting.Length; i++)
        {
            betting[i] = aiTargetNumber;

            aiTargetNumber++;

            if (aiTargetNumber > 24)
            {
                aiTargetNumber = 0;
            }
        }
    }

    #endregion
}
