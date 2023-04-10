using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteManager : MonoBehaviour
{
    PhotonView PV;

    public GameObject emoteChoiceView;

    public GameObject myEmote;
    public Image myEmoteImg;

    public GameObject otherEmote;
    public Image otherEmoteImg;

    public Sprite[] emoteImgArray;

    public Image emoteFillAmount;

    private float emoteCoolTime = 5.0f;
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
        isUseEmote = true;
    }

    public void OpenEmoteView()
    {
        if (isUseEmote)
        {
            emoteChoiceView.SetActive(true);
        }
    }

    public void CloseEmoteView()
    {
        emoteChoiceView.SetActive(false);
    }

    public void UseEmoteButton(int number)
    {
        if (isUseEmote)
        {
            isUseEmote = false;

            CloseEmoteView();

            myEmote.gameObject.SetActive(true);
            myEmoteImg.sprite = emoteImgArray[number];

            Invoke("CloseMyEmote", 4.0f);

            PV.RPC("UseEmote", RpcTarget.Others, number);

            StartCoroutine(ResetEmoteCoroutine());
        }
        else
        {
            Debug.Log("이모티콘 재사용 대기중입니다");
        }
    }

    IEnumerator ResetEmoteCoroutine() //스킬 쿨타임
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
        otherEmote.gameObject.SetActive(true);
        otherEmoteImg.sprite = emoteImgArray[number];

        Invoke("CloseOtherEmote", 4.0f);
    }

    void CloseMyEmote()
    {
        myEmote.SetActive(false);
    }

    void CloseOtherEmote()
    {
        otherEmote.SetActive(false);
    }
}
