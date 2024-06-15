using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    public GameObject rankDownObj;

    public GameObject attendanceObj;
    public GameObject eventObj;
    public GameObject packageObj;
    public GameObject rankingObj;

    public GameObject tutorialObj;
    public GameObject achievementObj;
    public GameObject questObj;

    public GameObject[] collectionGosuObj;

    public GameObject upgradeObj;
    public GameObject sellObj;
    public GameObject synthesisObj;


    public ChallengeManager challengeManager;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        rankDownObj.SetActive(false);
        attendanceObj.SetActive(false);
        eventObj.SetActive(false);
        packageObj.SetActive(false);
        rankingObj.SetActive(false);
        tutorialObj.SetActive(true);
        achievementObj.SetActive(false);
        questObj.SetActive(false);

        for (int i = 0; i < collectionGosuObj.Length; i ++)
        {
            collectionGosuObj[i].SetActive(false);
        }

        upgradeObj.SetActive(false);
        sellObj.SetActive(false);
        synthesisObj.SetActive(false);
    }



    public void Initialize()
    {
        if(playerDataBase.ChallengeCount == 0)
        {
            challengeManager.OpenChallengeView();

            challengeManager.exitButton.SetActive(false);
        }

        if(playerDataBase.ChallengeCount >= 3) //컬렉션 장착 해제
        {
            for (int i = 0; i < collectionGosuObj.Length; i++)
            {
                collectionGosuObj[i].SetActive(true);
            }

        }

        if (playerDataBase.ChallengeCount >= 4) //강화 합성 해제
        {
            upgradeObj.SetActive(true);
        }

        if (playerDataBase.ChallengeCount >= 6) //튜토리얼 끝났을 때
        {
            sellObj.SetActive(true);
            synthesisObj.SetActive(false);
            attendanceObj.SetActive(true);
            eventObj.SetActive(true);
            packageObj.SetActive(false);
            rankingObj.SetActive(true);
            tutorialObj.SetActive(false);
            rankDownObj.SetActive(true);
            achievementObj.SetActive(true);
            //questObj.SetActive(true);

            if(GameStateManager.instance.StoreType != StoreType.OneStore)
            {
                UIManager.instance.OpenAppReview();
                packageObj.SetActive(true);
            }
        }
    }
}
