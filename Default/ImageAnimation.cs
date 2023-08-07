using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    Image image;

    float size = 0;

    bool check = false;

    WaitForSeconds WaitForSeconds = new WaitForSeconds(0.01f);


    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void StateOn()
    {
        transform.localScale = Vector3.one;
    }

    public void StateOff()
    {
        transform.localScale = Vector3.zero;
    }

    [Button]
    public void StarUp()
    {
        size = 0;

        check = false;

        transform.localScale = Vector3.zero;

        StopAllCoroutines();
        StartCoroutine(StarUpAnimation());
    }

    [Button]
    public void StarDown()
    {
        size = 1;

        check = false;

        transform.localScale = Vector3.one;

        StopAllCoroutines();
        StartCoroutine(StarDownAnimation());
    }

    IEnumerator StarUpAnimation()
    {
        if(!check)
        {
            if (size < 1.2f)
            {
                size += 0.04f;
            }
            else
            {
                check = true;
            }
        }
        else
        {
            if(size > 1f)
            {
                size -= 0.04f;
            }
            else
            {
                SoundManager.instance.PlaySFX(GameSfxType.GetStar);

                transform.localScale = Vector3.one;
                yield break;
            }
        }

        transform.localScale = new Vector3(size, size, size);

        yield return WaitForSeconds;
        StartCoroutine(StarUpAnimation());
    }

    IEnumerator StarDownAnimation()
    {
        if(size > 0)
        {
            size -= 0.04f;
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.LoseStar);

            transform.localScale = Vector3.zero;
            yield break;
        }

        transform.localScale = new Vector3(size, size, size);

        yield return WaitForSeconds;
        StartCoroutine(StarDownAnimation());
    }
}
