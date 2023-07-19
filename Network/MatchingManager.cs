using DG.Tweening;
using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MatchingManager : MonoBehaviour
{
    GameRankType gameRankType = GameRankType.Bronze_4;
    RankInformation rankInformation = new RankInformation();

    [Title("Rank Up")]
    public GameObject rankUpView;

    public GameObject rankUpEffect;

    public Text rankUpTitle;
    public Image rankUpIcon;
    public Text rankUpText;


    [Title("Enter")]
    public Image rankImg;
    public Text rankText;
    public Text newbieEnterText;
    public Text newbieMaxBlockText;
    public Text gosuEnterText;
    public Text gosuMaxBlockText;

    Sprite[] rankIconArray;

    string[] strArray = new string[2];
    string[] strArray2 = new string[2];


    [Title("Matching")]
    public GameObject matchingView;
    public Text matchingText;
    public Image matchingButton;

    public Sprite[] matchingButtonArray;

    private int stakes = 0;
    private int limitBlock = 0;
    private int matchingWaitTime = 0;

    bool isCancle = false;
    bool isWait = false;

    BlockClass blockClass;
    BlockClass blockClass2;
    BlockClass blockClass3;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    public FadeInOut mainFadeInOut;
    public NetworkManager networkManager;
    public UIManager uIManager;
    public AiManager aiManager;

    PlayerDataBase playerDataBase;
    RankDataBase rankDataBase;
    UpgradeDataBase upgradeDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rankIconArray = imageDataBase.GetRankIconArray();

        rankText.text = "";

        rankUpEffect.SetActive(false);
        rankUpView.SetActive(false);
        matchingView.SetActive(false);

        DOTween.RewindAll();
    }

    public void Initialize()
    {
        int rank = rankDataBase.GetRank(playerDataBase.Gold) - 1;
        gameRankType = GameRankType.Bronze_4 + rank;

        if (rank >= System.Enum.GetValues(typeof(GameRankType)).Length)
        {
            gameRankType = GameRankType.Legend_1;
        }

        rankImg.sprite = rankIconArray[rank];

        strArray = rankDataBase.rankInformationArray[rank].gameRankType.ToString().Split("_");

        rankText.text = LocalizationManager.instance.GetString(strArray[0]) + " " + strArray[1];

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Rank", (int)gameRankType);

        rankInformation = rankDataBase.GetRankInformation(gameRankType);

        stakes = rankInformation.stakes;
        limitBlock = rankInformation.limitBlockValue;

        newbieEnterText.text = "입장료 : " + MoneyUnitString.ToCurrencyString(stakes / 2);
        newbieMaxBlockText.text = "최대 블럭 레벨 : " + MoneyUnitString.ToCurrencyString(limitBlock / 2);

        gosuEnterText.text = "입장료 : " + MoneyUnitString.ToCurrencyString(stakes);
        gosuMaxBlockText.text = "최대 블럭 레벨 : " + MoneyUnitString.ToCurrencyString(limitBlock);

        if(GameStateManager.instance.GameRankType < gameRankType)
        {
            Debug.Log("랭크 상승!");

            if(uIManager.isFirst || uIManager.isHome)
            {
                OpenRankUpView(true);
            }

            if((int)gameRankType > playerDataBase.HighRank)
            {
                playerDataBase.HighRank = (int)gameRankType;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("HighRank", (int)gameRankType);

                Debug.Log("최고 랭크 달성!");
            }
        }
        else if(GameStateManager.instance.GameRankType > gameRankType)
        {
            Debug.Log("랭크 하락");

            if (uIManager.isFirst)
            {
                OpenRankUpView(false);
            }
        }

        GameStateManager.instance.GameRankType = gameRankType;
    }

    void OpenRankUpView(bool check)
    {
        rankUpView.SetActive(true);

        rankUpEffect.SetActive(check);

        rankUpIcon.sprite = rankIconArray[(int)gameRankType];

        strArray = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType].gameRankType.ToString().Split("_");
        strArray2 = rankDataBase.rankInformationArray[(int)gameRankType].gameRankType.ToString().Split("_");

        rankUpText.text = LocalizationManager.instance.GetString(strArray[0]) + " <color=#FFC032>" + strArray[1] + "</color>     ▶     " + 
            LocalizationManager.instance.GetString(strArray2[0]) + " <color=#FFC032>" + strArray2[1] +"</color>";

        if (check)
        {
            rankUpTitle.text = "랭크 상승!";

            SoundManager.instance.PlaySFX(GameSfxType.RankUp);

            Invoke("SoundDelay", 2f);

        }
        else
        {
            rankUpTitle.text = "랭크 하락";
        }

        isWait = true;
        Invoke("Delay", 1.0f);
    }

    void SoundDelay()
    {
        SoundManager.instance.StopSFX(GameSfxType.RankUp);
    }

    public void CloseRankUpView()
    {
        if(!isWait)
        {
            rankUpView.SetActive(false);
        }
    }

    void Delay()
    {
        isWait = false;
    }

    public void GameStartButton_Newbie()
    {
        rankInformation = rankDataBase.GetRankInformation(gameRankType);

        stakes = rankInformation.stakes;
        limitBlock = rankInformation.limitBlockValue;

        blockClass = playerDataBase.GetBlockClass(playerDataBase.Newbie);

        int number = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);

        if(!playerDataBase.CheckEquipBlock_Newbie()) //블록은 전부 장착했는지
        {
            NotionManager.instance.UseNotion(NotionType.NeedEquipBlock);
            return;
        }

        if(playerDataBase.Gold < (stakes / 2)) //입장료를 가지고 있는지?
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);

            return;
        }

        if(number > (limitBlock / 2)) //블럭 제한을 넘지 않는지?
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.LimitMaxBlock);

            return;
        }


        OpenMacthingView();

        networkManager.JoinRandomRoom_Newbie();

        Debug.Log("초보방 매칭중입니다.");
    }

    public void GameStartButton_Gosu()
    {
        blockClass = playerDataBase.GetBlockClass(playerDataBase.Armor);
        blockClass2 = playerDataBase.GetBlockClass(playerDataBase.Weapon);
        blockClass3 = playerDataBase.GetBlockClass(playerDataBase.Shield);

        int number = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);
        int number2 = upgradeDataBase.GetUpgradeValue(blockClass2.rankType).GetValueNumber(blockClass2.level);
        int number3 = upgradeDataBase.GetUpgradeValue(blockClass3.rankType).GetValueNumber(blockClass3.level);

        if (!playerDataBase.CheckEquipBlock_Gosu()) //블록은 전부 장착했는지
        {
            NotionManager.instance.UseNotion(NotionType.NeedEquipBlock);
            return;
        }

        if (playerDataBase.Gold < stakes) //입장료를 가지고 있는지?
        {
            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);

            return;
        }

        if (number > limitBlock || number2 > limitBlock || number3 > limitBlock) //블럭 제한을 넘지 않는지?
        {
            NotionManager.instance.UseNotion(NotionType.LimitMaxBlock);

            return;
        }

        OpenMacthingView();

        networkManager.JoinRandomRoom_Gosu();

        Debug.Log("고수방 매칭중입니다.");
    }

    public void OpenMacthingView()
    {
        if (!matchingView.activeSelf)
        {
            matchingView.SetActive(true);

            matchingButton.sprite = matchingButtonArray[1];

            isCancle = true;

            GameStateManager.instance.Stakes = stakes;

            matchingWaitTime = GameStateManager.instance.MatchingTime;
            StartCoroutine(WaitingPlayer());
        }
        else
        {
            if (isCancle)
            {
                StopAllCoroutines();

                networkManager.LeaveRoom();

                matchingView.SetActive(false);
            }
        }
    }

    IEnumerator WaitingPlayer()
    {
        while(matchingWaitTime > 0)
        {
            matchingWaitTime -= 1;
            matchingText.text = "상대방을 찾고 있습니다...\n예상 대기 시간 : " + matchingWaitTime + "초";

            if(matchingWaitTime <= 1)
            {
                matchingButton.sprite = matchingButtonArray[0];

                isCancle = false;
            }

            yield return waitForSeconds;
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;

        AlMatching();
    }

    public void PlayerMatching(string player1, string player2, int otherFormation)
    {
        StopAllCoroutines();

        matchingView.SetActive(false);

        uIManager.OnMatchingSuccess(player1, player2, otherFormation);

        mainFadeInOut.FadeOutToIn();

        Invoke("ChangeBGM", 1.5f);

        Debug.Log("플레이어와 매칭되었습니다.");
    }

    public void AlMatching()
    {
        StopAllCoroutines();

        matchingView.SetActive(false);

        uIManager.OnMatchingAi(aiManager.RandomCharacter());

        mainFadeInOut.FadeOutToIn();

        Invoke("ChangeBGM", 1.5f);

        Debug.Log("사람이 없는 관계로 인공지능과 매칭됩니다.");
    }

    public void ChangeBGM()
    {
        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            SoundManager.instance.PlayBGM(GameBgmType.Game_Newbie);
        }
        else
        {
            SoundManager.instance.PlayBGM(GameBgmType.Game_Gosu);
        }

    }
}
