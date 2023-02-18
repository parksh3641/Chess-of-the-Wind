using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationManager : MonoBehaviour
{
    public GameObject formationView;

    public GameObject warningView;

    public Text warningText;

    private int selectNumber = 0;

    List<string> itemList = new List<string>();

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        formationView.SetActive(false);
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
        if(selectNumber == 0)
        {
            playerDataBase.Formation = 1;

            if (PlayfabManager.instance.isActive)
            {
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("Formation", 1);

                itemList.Clear();
                itemList.Add("LeftQueen_2_D");
                itemList.Add("LeftNight_D");
                itemList.Add("Rook_V2_D");
                itemList.Add("Pawn_D");

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
                itemList.Add("RightQueen_2_D");
                itemList.Add("RightNight_D");
                itemList.Add("Rook_V2H2_D");
                itemList.Add("Pawn_D");

                PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", itemList);
            }

            Debug.Log("���� ���� ���� ����");
        }

        formationView.SetActive(false);
    }

    public void CancleFormationButton()
    {
        warningView.SetActive(false);
    }
}
