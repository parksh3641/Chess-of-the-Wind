using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEventManager : MonoBehaviour
{
    public GameObject eventView;

    public GameObject eventInfoButton;

    public GameObject eventInfoView;

    public LocalizationContent eventTitleText;
    public Text eventText;
    public Text eventInfoText;

    public ButtonScaleAnimation eventTextAnimation;


    private int eventMaxNumber = 0;
    private int eventNumber = 0;

    private float time = 3;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(3);

    string[] eventString;

    public GameManager gameManager;



    private void Awake()
    {
        eventView.SetActive(false);

        eventInfoButton.SetActive(false);
    }

    public void Initialize()
    {
        eventView.SetActive(false);
    }

    public void OnEventStart(string number)
    {
        eventNumber = int.Parse(number);

        eventMaxNumber = System.Enum.GetValues(typeof(GameEventType)).Length;

        eventString = new string[eventMaxNumber];

        for(int i = 0; i < eventMaxNumber; i ++)
        {
            Debug.Log(i);
            eventString[i] = LocalizationManager.instance.GetString("GameEvent" + (i + 1));
        }

        eventView.SetActive(true);

        eventTitleText.localizationName = "GameEventTitle";
        eventTitleText.ReLoad();

        eventTextAnimation.StopAnim();

        time = 3;
        StartCoroutine(EventCoroution());
    }

    IEnumerator EventCoroution()
    {
        while(time > 0)
        {
            eventText.text = eventString[Random.Range(0, eventString.Length - 1)];;

            SoundManager.instance.PlaySFX(GameSfxType.Click);

            time -= 0.1f;

            yield return waitForSeconds;
        }

        eventText.text = eventString[eventNumber];
        eventTextAnimation.PlayAnim();

        eventTitleText.localizationName = "GameEventInfo";
        eventTitleText.ReLoad();

        GameStateManager.instance.GameEventType = GameEventType.GameEvent1 + eventNumber;

        Debug.LogError("이벤트 설정 완료 : " + GameStateManager.instance.GameEventType);

        SoundManager.instance.PlaySFX(GameSfxType.Success);

        yield return waitForSeconds2;

        OnEventEnd();
    }


    public void OnEventEnd()
    {
        eventView.SetActive(false);

        eventInfoButton.SetActive(true);

        gameManager.GameStart();
    }

    public void OpenEventInfoView()
    {
        if(!eventInfoView.activeInHierarchy)
        {
            eventInfoView.SetActive(true);

            eventInfoText.text = eventString[eventNumber];
        }
        else
        {
            eventInfoView.SetActive(false);
        }
    }
}
