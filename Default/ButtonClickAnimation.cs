using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonClickAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isSound = true;

    public UnityEvent clickSoundEvent;

    void Awake()
    {
        if(isSound) clickSoundEvent.AddListener(() => { GameObject.FindWithTag("ClickSound").GetComponent<AudioSource>().Play(); });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 0.95f;

        if(isSound) clickSoundEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}