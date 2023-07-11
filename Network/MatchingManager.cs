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

    [Title("Limit")]
    public Image rankImg;
    public Text rankText;
    public Text newbieEnterText;
    public Text newbieMaxBlockText;
    public Text gosuEnterText;
    public Text gosuMaxBlockText;

    public Sprite[] rankImgArray;


    [Title("Matching")]
    public GameObject matchingView;
    public Text matchingText;


    private int stakes = 0;
    private int limitBlock = 0;
    private int matchingWaitTime = 0;

    BlockClass blockClass;
    BlockClass blockClass2;
    BlockClass blockClass3;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    public FadeInOut mainFadeInOut;
    public NetworkManager networkManager;
    public UIManager uIManager;
    public AiManager aiManager;
    public SoundManager soundManager;

    PlayerDataBase playerDataBase;
    RankDataBase rankDataBase;
    UpgradeDataBase upgradeDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;

        rankText.text = "";

        matchingView.SetActive(false);

        DOTween.RewindAll();
    }

    public void Initialize()
    {
        int rank = rankDataBase.GetRank(playerDataBase.Gold);
        gameRankType = GameRankType.Bronze_4 + rank;

        if (rank >= System.Enum.GetValues(typeof(GameRankType)).Length)
        {
            gameRankType = GameRankType.Legend;
        }

        rankImg.sprite = rankImgArray[(int)gameRankType];

        switch (gameRankType)
        {
            case GameRankType.Bronze_4:
                rankText.text = LocalizationManager.instance.GetString("Bronze") + " 4";
                break;
            case GameRankType.Bronze_3:
                rankText.text = LocalizationManager.instance.GetString("Bronze") + " 3";
                break;
            case GameRankType.Bronze_2:
                rankText.text = LocalizationManager.instance.GetString("Bronze") + " 2";
                break;
            case GameRankType.Bronze_1:
                rankText.text = LocalizationManager.instance.GetString("Bronze") + " 1";
                break;
            case GameRankType.Sliver_4:
                rankText.text = LocalizationManager.instance.GetString("Sliver") + " 4";
                break;
            case GameRankType.Sliver_3:
                rankText.text = LocalizationManager.instance.GetString("Sliver") + " 3";
                break;
            case GameRankType.Sliver_2:
                rankText.text = LocalizationManager.instance.GetString("Sliver") + " 2";
                break;
            case GameRankType.Sliver_1:
                rankText.text = LocalizationManager.instance.GetString("Sliver") + " 1";
                break;
            case GameRankType.Gold_4:
                rankText.text = LocalizationManager.instance.GetString("Gold") + " 4";
                break;
            case GameRankType.Gold_3:
                rankText.text = LocalizationManager.instance.GetString("Gold") + " 3";
                break;
            case GameRankType.Gold_2:
                rankText.text = LocalizationManager.instance.GetString("Gold") + " 2";
                break;
            case GameRankType.Gold_1:
                rankText.text = LocalizationManager.instance.GetString("Gold") + " 1";
                break;
            case GameRankType.Platinum_4:
                rankText.text = LocalizationManager.instance.GetString("Platinum") + " 4";
                break;
            case GameRankType.Platinum_3:
                rankText.text = LocalizationManager.instance.GetString("Platinum") + " 3";
                break;
            case GameRankType.Platinum_2:
                rankText.text = LocalizationManager.instance.GetString("Platinum") + " 2";
                break;
            case GameRankType.Platinum_1:
                rankText.text = LocalizationManager.instance.GetString("Platinum") + " 1";
                break;
            case GameRankType.Diamond_4:
                rankText.text = LocalizationManager.instance.GetString("Diamond") + " 4";
                break;
            case GameRankType.Diamond_3:
                rankText.text = LocalizationManager.instance.GetString("Diamond") + " 3";
                break;
            case GameRankType.Diamond_2:
                rankText.text = LocalizationManager.instance.GetString("Diamond") + " 2";
                break;
            case GameRankType.Diamond_1:
                rankText.text = LocalizationManager.instance.GetString("Diamond") + " 1";
                break;
            case GameRankType.Legend:
                rankText.text = LocalizationManager.instance.GetString("Legend");
                break;
        }

        rankInformation = rankDataBase.GetRankInformation(gameRankType);

        stakes = rankInformation.stakes;
        limitBlock = rankInformation.limitBlockValue;

        newbieEnterText.text = "입장료 : " + MoneyUnitString.ToCurrencyString(stakes / 2);
        newbieMaxBlockText.text = "최대 블럭 값 : " + MoneyUnitString.ToCurrencyString(limitBlock / 2);

        gosuEnterText.text = "입장료 : " + MoneyUnitString.ToCurrencyString(stakes);
        gosuMaxBlockText.text = "최대 블럭 값 : " + MoneyUnitString.ToCurrencyString(limitBlock);

        GameStateManager.instance.GameRankType = gameRankType;
    }

    public void GameStartButton_Newbie()
    {
        rankInformation = rankDataBase.GetRankInformation(gameRankType);

        stakes = rankInformation.stakes;
        limitBlock = rankInformation.limitBlockValue;

        blockClass = playerDataBase.GetBlockClass(playerDataBase.Newbie);

        int number = upgradeDataBase.GetUpgradeValue(blockClass.rankType).GetValueNumber(blockClass.level);

        if(playerDataBase.Gold < (stakes / 2)) //입장료를 가지고 있는지?
        {
            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);

            return;
        }

        if(number > (limitBlock / 2)) //블럭 제한을 넘지 않는지?
        {
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

            GameStateManager.instance.Stakes = stakes;

            matchingWaitTime = GameStateManager.instance.MatchingTime;
            StartCoroutine(WaitingPlayer());
        }
        else
        {
            StopAllCoroutines();

            networkManager.LeaveRoom();

            matchingView.SetActive(false);
        }
    }

    IEnumerator WaitingPlayer()
    {
        while(matchingWaitTime > 0)
        {
            matchingWaitTime -= 1;
            matchingText.text = "상대방을 찾고 있습니다...\n예상 대기 시간 : " + matchingWaitTime + "초";

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
            soundManager.PlayBGM(GameBgmType.Game_Newbie);
        }
        else
        {
            soundManager.PlayBGM(GameBgmType.Game_Gosu);
        }

    }
}
