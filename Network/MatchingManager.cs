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
    RankInformation rankInformation = new RankInformation();

    [Title("Rank Up")]
    public GameObject rankUpView;

    public GameObject rankUpEffect;

    public Text rankUpTitle;
    public Image rankUpIcon;
    public Text rankUpText;


    [Title("Enter")]
    public Image rankImg;
    public LocalizationContent rankText;

    public LocalizationContent newbieEnterText;
    public LocalizationContent newbieMaxBlockText;

    public LocalizationContent gosuEnterText;
    public LocalizationContent gosuMaxBlockText;

    Sprite[] rankIconArray;

    public Image[] starArray;

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

        rankUpEffect.SetActive(false);
        rankUpView.SetActive(false);
        matchingView.SetActive(false);

        DOTween.RewindAll();
    }

    public void Initialize()
    {
        //int rank = rankDataBase.GetRank(playerDataBase.Gold) - 1;
        //gameRankType = GameRankType.Bronze_4 + rank;

        //if (rank >= System.Enum.GetValues(typeof(GameRankType)).Length)
        //{
        //    gameRankType = GameRankType.Legend_1;
        //}

        int needStar = rankDataBase.GetNeedStar(playerDataBase.NowRank) + 1;

        for (int i = 0; i < starArray.Length; i ++)
        {
            starArray[i].gameObject.SetActive(false);
        }

        for(int i =0; i < needStar - 1; i ++)
        {
            starArray[i].gameObject.SetActive(true);
            starArray[i].color = Color.black;
        }

        if(playerDataBase.Star > starArray.Length)
        {
            for (int i = 0; i < starArray.Length; i++)
            {
                starArray[i].color = Color.white;
            }
        }
        else
        {
            for (int i = 0; i < playerDataBase.Star; i++)
            {
                starArray[i].color = Color.white;
            }
        }

        rankImg.sprite = rankIconArray[playerDataBase.NowRank];

        strArray = rankDataBase.rankInformationArray[playerDataBase.NowRank].gameRankType.ToString().Split("_");

        rankText.localizationName = strArray[0];
        rankText.plusText = " " + strArray[1];
        rankText.ReLoad();

        rankInformation = rankDataBase.GetRankInformation(GameStateManager.instance.GameRankType);

        stakes = rankInformation.stakes;
        limitBlock = rankInformation.limitBlockLevel;

        newbieEnterText.localizationName = "AllowMoney";
        newbieEnterText.plusText = " : " + MoneyUnitString.ToCurrencyString(stakes);

        newbieMaxBlockText.localizationName = "AllowBlockLevel";
        newbieMaxBlockText.plusText = " : " + MoneyUnitString.ToCurrencyString(limitBlock);

        gosuEnterText.localizationName = "AllowMoney";
        gosuEnterText.plusText = " : " + MoneyUnitString.ToCurrencyString(stakes);

        gosuMaxBlockText.localizationName = "AllowBlockLevel";
        gosuMaxBlockText.plusText = " : " + MoneyUnitString.ToCurrencyString(limitBlock);

        newbieEnterText.ReLoad();
        newbieMaxBlockText.ReLoad();
        gosuEnterText.ReLoad();
        gosuMaxBlockText.ReLoad();

    }

    [Button]
    public void CheckRankUp()
    {
        if(GameStateManager.instance.Win)
        {
            GameStateManager.instance.Win = false;

            int needStar = rankDataBase.GetNeedStar(playerDataBase.NowRank) + 1;

            if(GameStateManager.instance.WinStreak >= 3)
            {
                playerDataBase.Star += 2;

                Debug.Log("3연승 이상 승리 :  별 2개 획득");
            }
            else
            {
                playerDataBase.Star += 1;

                Debug.Log("승리 : 별 1개 획득");
            }

            if(GameStateManager.instance.GameType == GameType.NewBie)
            {
                playerDataBase.NewbieWin += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("NewbieWin", playerDataBase.NewbieWin);
            }
            else
            {
                playerDataBase.GosuWin += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("GosuWin", playerDataBase.GosuWin);
            }

            if(playerDataBase.NowRank != rankDataBase.rankInformationArray.Length - 1)
            {
                if (playerDataBase.Star >= needStar)
                {
                    playerDataBase.NowRank += 1;

                    GameStateManager.instance.GameRankType += 1;

                    playerDataBase.Star -= needStar;

                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("NowRank", playerDataBase.NowRank);

                    if(playerDataBase.NowRank > playerDataBase.HighRank)
                    {
                        playerDataBase.HighRank = playerDataBase.NowRank;

                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("HighRank", playerDataBase.HighRank);

                        Debug.Log("최고 랭크 갱신 !");
                    }

                    playerDataBase.Star += 1;

                    OpenRankUpView(true);

                    Debug.Log("랭크 상승 !");
                }
            }
            else
            {
                //playerDataBase.Star = rankDataBase.GetNeedStar(rankDataBase.rankInformationArray.Length - 1);

                Debug.Log("최고 랭크 달성 !");
            }
        }
        else if(GameStateManager.instance.Lose)
        {
            GameStateManager.instance.Lose = false;
            GameStateManager.instance.WinStreak = 0;

            playerDataBase.Star -= 1;

            if (GameStateManager.instance.GameType == GameType.NewBie)
            {
                playerDataBase.NewbieLose += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("NewbieLose", playerDataBase.NewbieLose);
            }
            else
            {
                playerDataBase.GosuLose += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("GosuLose", playerDataBase.GosuLose);
            }

            Debug.Log("패배 : 별 1개 감소");

            if (playerDataBase.Star <= -1)
            {
                if(playerDataBase.NowRank != 0)
                {
                    playerDataBase.Star = rankDataBase.GetNeedStar(playerDataBase.NowRank - 1) - 1;

                    playerDataBase.NowRank -= 1;

                    GameStateManager.instance.GameRankType -= 1;

                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("NowRank", playerDataBase.NowRank);

                    OpenRankUpView(false);

                    Debug.Log("랭크 하락");
                }
                else if(playerDataBase.NowRank == 0)
                {
                    playerDataBase.Star = 0;

                    Debug.Log("최하 랭크 입니다");
                }
            }
        }

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("Star", playerDataBase.Star);

        Initialize();
    }


    void OpenRankUpView(bool check)
    {
        rankUpView.SetActive(true);

        rankUpEffect.SetActive(check);

        rankUpIcon.sprite = rankIconArray[(int)GameStateManager.instance.GameRankType];

        if (check)
        {
            rankUpTitle.text = LocalizationManager.instance.GetString("RankUp");

            strArray = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType - 1].gameRankType.ToString().Split("_");
            strArray2 = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType].gameRankType.ToString().Split("_");

            rankUpText.text = LocalizationManager.instance.GetString(strArray[0]) + " <color=#FFC032>" + strArray[1] + "</color>     ▶     " +
    LocalizationManager.instance.GetString(strArray2[0]) + " <color=#FFC032>" + strArray2[1] + "</color>";

            SoundManager.instance.PlaySFX(GameSfxType.RankUp);

            Invoke("SoundDelay", 2f);

        }
        else
        {
            rankUpTitle.text = LocalizationManager.instance.GetString("RankDown");

            strArray = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType + 1].gameRankType.ToString().Split("_");
            strArray2 = rankDataBase.rankInformationArray[(int)GameStateManager.instance.GameRankType].gameRankType.ToString().Split("_");

            rankUpText.text = LocalizationManager.instance.GetString(strArray[0]) + " <color=#FFC032>" + strArray[1] + "</color>     ▶     " +
    LocalizationManager.instance.GetString(strArray2[0]) + " <color=#FFC032>" + strArray2[1] + "</color>";
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
        if(!NetworkConnect.instance.CheckConnectInternet())
        {
            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        rankInformation = rankDataBase.GetRankInformation(GameStateManager.instance.GameRankType);

        stakes = rankInformation.stakes;
        limitBlock = rankInformation.limitBlockLevel;

        GameStateManager.instance.Stakes = stakes;

        if(!playerDataBase.CheckEquipBlock_Newbie()) //블록은 전부 장착했는지
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NeedEquipBlock);

            return;
        }
        //else
        //{
        //    blockClass = playerDataBase.GetBlockClass(playerDataBase.Newbie);

        //    if (blockClass.level > limitBlock) //블럭 제한을 넘지 않는지?
        //    {
        //        SoundManager.instance.PlaySFX(GameSfxType.Wrong);

        //        NotionManager.instance.UseNotion(NotionType.LimitMaxBlock);

        //        return;
        //    }
        //}

        if(playerDataBase.Gold < stakes) //입장료를 가지고 있는지?
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);

            return;
        }

        OpenMacthingView();

        networkManager.JoinRandomRoom_Newbie();

        Debug.Log("초보방 매칭중입니다.");
    }

    public void GameStartButton_Gosu()
    {
        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        rankInformation = rankDataBase.GetRankInformation(GameStateManager.instance.GameRankType);

        stakes = rankInformation.stakes;
        limitBlock = rankInformation.limitBlockLevel;

        GameStateManager.instance.Stakes = stakes;

        if (playerDataBase.CheckEquipBlock_Gosu() == 0) //1개라도 착용했을 경우
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NeedEquipBlock);

            return;
        }
        //else
        //{
        //    blockClass = playerDataBase.GetBlockClass(playerDataBase.Armor);
        //    blockClass2 = playerDataBase.GetBlockClass(playerDataBase.Weapon);
        //    blockClass3 = playerDataBase.GetBlockClass(playerDataBase.Shield);

        //    if (blockClass.level > limitBlock || blockClass2.level > limitBlock || blockClass3.level > limitBlock) //블럭 제한을 넘지 않는지?
        //    {
        //        NotionManager.instance.UseNotion(NotionType.LimitMaxBlock);

        //        return;
        //    }
        //}

        if (playerDataBase.Gold < stakes) //입장료를 가지고 있는지?
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);

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
            matchingText.text = LocalizationManager.instance.GetString("MatchingInfo") + " : " + matchingWaitTime;

            if(matchingWaitTime <= 1)
            {
                matchingButton.sprite = matchingButtonArray[0];

                isCancle = false;
            }

            yield return waitForSeconds;
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;

        yield return waitForSeconds;

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

    public void StarUp()
    {
        GameStateManager.instance.Win = true;

        GameStateManager.instance.WinStreak += 1;

        CheckRankUp();
    }

    public void StarDown()
    {
        GameStateManager.instance.Lose = true;

        CheckRankUp();
    }
}
