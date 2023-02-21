using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationManager : MonoBehaviour
{
    public GameObject formationView;

    public GameObject warningView;

    public GameObject animationView;

    public Text warningText;

    private int selectNumber = 0;
    bool isWait = false;

    List<string> itemList = new List<string>();

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        formationView.SetActive(false);
        warningView.SetActive(false);
        animationView.SetActive(false);
    }

    public void Initialize()
    {
        if(playerDataBase.Formation == 0)
        {
            formationView.SetActive(true);
        }
    }

    public void SelectFormationButton(int number) //���� ����
    {
        OpenWarningView(number);
    }

    void OpenWarningView(int number)
    {
        selectNumber = number;

        warningView.SetActive(true);

        if (selectNumber == 0)
        {
            warningText.text = "������ ���� ���� ������\n�����Ͻ� �ǰ���?";
        }
        else
        {
            warningText.text = "������ ���� ���� ������\n�����Ͻ� �ǰ���?";
        }
    }
    public void SelectedFormation()
    {
        if (isWait) return;

        isWait = true;

        StartCoroutine(SelectedFormationCoroution());
    }

    IEnumerator SelectedFormationCoroution()
    {
        if (selectNumber == 0)
        {
            playerDataBase.Formation = 1;

            if (PlayfabManager.instance.isActive)
            {
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("Formation", 1);

                itemList.Clear();
                itemList.Add("LeftQueen_2_N");
                itemList.Add("LeftNight_N");
                itemList.Add("Rook_V2_N");
                itemList.Add("Pawn_N");

                PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", itemList);
            }
        }
        else
        {
            playerDataBase.Formation = 2;

            if (PlayfabManager.instance.isActive)
            {
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("Formation", 2);

                itemList.Clear();
                itemList.Add("RightQueen_2_N");
                itemList.Add("RightNight_N");
                itemList.Add("Rook_V2H2_N");
                itemList.Add("Pawn_N");

                PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", itemList);
            }

            Debug.Log("���� ���� ���� ����");
        }

        animationView.SetActive(true);

        yield return new WaitForSeconds(2);

        formationView.SetActive(false);
    }

    public void CancleFormationButton()
    {
        warningView.SetActive(false);
    }
}
