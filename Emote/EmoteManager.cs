using Firebase.Analytics;
using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteManager : MonoBehaviour
{
    PhotonView PV;

    [Title("Betting")]
    public GameObject emoteChoiceView;
    public GameObject closeEmoteView;

    public GameObject myEmote;
    public Image myEmoteImg;

    public GameObject otherEmote;
    public Image otherEmoteImg;

    public Image emoteFillAmount;

    [Space]
    [Title("Roulette")]
    public GameObject emoteChoiceView2;
    public GameObject closeEmoteView2;

    public GameObject myEmote2;
    public Image myEmoteImg2;

    public GameObject otherEmote2;
    public Image otherEmoteImg2;

    public Image emoteFillAmount2;


    public Sprite[] emoteImgArray;


    private float emoteCoolTime = 2.0f;
    private float emoteCoolTime2 = 2.0f;
    bool isUseEmote = true;



    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        Initialize();
    }

    public void Initialize()
    {
        StopAllCoroutines();

        emoteChoiceView.SetActive(false);
        myEmote.SetActive(false);
        otherEmote.SetActive(false);

        emoteFillAmount.fillAmount = 0;

        emoteChoiceView2.SetActive(false);
        myEmote2.SetActive(false);
        otherEmote2.SetActive(false);

        emoteFillAmount2.fillAmount = 0;

        isUseEmote = true;
    }

    public void OpenEmoteView()
    {
        if (isUseEmote)
        {
            emoteChoiceView.SetActive(true);

            closeEmoteView.SetActive(true);
        }
    }

    public void CloseEmoteView()
    {
        emoteChoiceView.SetActive(false);

        closeEmoteView.SetActive(false);
    }

    public void UseEmoteButton(int number)
    {
        if (isUseEmote)
        {
            isUseEmote = false;

            CloseEmoteView();

            myEmote.gameObject.SetActive(false);
            myEmote.gameObject.SetActive(true);
            myEmoteImg.sprite = emoteImgArray[number];

            PV.RPC("UseEmote", RpcTarget.Others, number);

            SoundManager.instance.PlaySFX(GameSfxType.UseEmotion);

            FirebaseAnalytics.LogEvent("UseEmote_Betting");

            StartCoroutine(ResetEmoteCoroutine());
        }
    }

    IEnumerator ResetEmoteCoroutine()
    {
        while (emoteFillAmount.fillAmount < 1)
        {
            emoteFillAmount.fillAmount += 1 * Time.smoothDeltaTime / emoteCoolTime;

            yield return null;
        }

        emoteFillAmount.fillAmount = 0;

        isUseEmote = true;
    }


    [PunRPC]
    private void UseEmote(int number)
    {
        otherEmote.gameObject.SetActive(false);
        otherEmote.gameObject.SetActive(true);
        otherEmoteImg.sprite = emoteImgArray[number];

        SoundManager.instance.PlaySFX(GameSfxType.UseEmotion);
    }



    public void OpenEmoteView2()
    {
        if (isUseEmote)
        {
            emoteChoiceView2.SetActive(true);

            closeEmoteView2.SetActive(true);
        }
    }

    public void CloseEmoteView2()
    {
        emoteChoiceView2.SetActive(false);

        closeEmoteView2.SetActive(false);
    }

    public void UseEmoteButton2(int number)
    {
        if (isUseEmote)
        {
            isUseEmote = false;

            CloseEmoteView2();

            myEmote2.gameObject.SetActive(false);
            myEmote2.gameObject.SetActive(true);
            myEmoteImg2.sprite = emoteImgArray[number];

            PV.RPC("UseEmote2", RpcTarget.Others, number);

            SoundManager.instance.PlaySFX(GameSfxType.UseEmotion);

            FirebaseAnalytics.LogEvent("UseEmote_InGame");

            StartCoroutine(ResetEmoteCoroutine2());
        }
    }

    IEnumerator ResetEmoteCoroutine2()
    {
        while (emoteFillAmount2.fillAmount < 1)
        {
            emoteFillAmount2.fillAmount += 1 * Time.smoothDeltaTime / emoteCoolTime2;

            yield return null;
        }

        emoteFillAmount2.fillAmount = 0;

        isUseEmote = true;
    }


    [PunRPC]
    private void UseEmote2(int number)
    {
        otherEmote2.gameObject.SetActive(false);
        otherEmote2.gameObject.SetActive(true);
        otherEmoteImg2.sprite = emoteImgArray[number];

        SoundManager.instance.PlaySFX(GameSfxType.UseEmotion);
    }


    public void UseEmote_Ai()
    {
        UseEmote(Random.Range(0, 5));
    }
}
