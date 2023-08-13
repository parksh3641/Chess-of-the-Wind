using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETCManager : MonoBehaviour
{
    public GameObject etcView;


    public NewsManager newsManager;
    public MailBoxManager mailBoxManager;
    public OptionManager optionManager;


    private void Awake()
    {
        etcView.SetActive(false);
    }


    public void OpenETCView()
    {
        if (!etcView.activeInHierarchy)
        {
            etcView.SetActive(true);
        }
        else
        {
            etcView.SetActive(false);
        }
    }

    public void OpenNewsView()
    {
        OpenETCView();
        newsManager.OpenNews();
    }

    public void OpenMailBoxView()
    {
        OpenETCView();
        mailBoxManager.OpenMail();
    }

    public void OpenOptionView()
    {
        OpenETCView();
        optionManager.OpenOptionView();
    }
}
