using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
    public GameObject helpView;





    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        helpView.SetActive(false);
    }


    public void OpenHelpView()
    {
        if (!helpView.activeSelf)
        {
            helpView.SetActive(true);
        }
        else
        {
            helpView.SetActive(false);
        }
    }
}
