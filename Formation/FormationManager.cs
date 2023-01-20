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

            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Formation", 1);

            Debug.Log("���� ���� ���� ����");
        }
        else
        {
            playerDataBase.Formation = 2;

            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Formation", 2);

            Debug.Log("���� ���� ���� ����");
        }

        formationView.SetActive(false);
    }

    public void CancleFormationButton()
    {
        warningView.SetActive(false);
    }
}
