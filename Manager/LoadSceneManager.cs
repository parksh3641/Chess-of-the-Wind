using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public Image background;
    public Image progressBar;
    public Text progressText;

    public Sprite[] backgroundArray;

    private string nextScene = "MainScene";


    private void Start()
    {
        int random = Random.Range(0, backgroundArray.Length - 1);

        //background.sprite = backgroundArray[random];

        progressBar.fillAmount = 0;
        progressText.text = "";

        nextScene = PlayerPrefs.GetString("LoadScene");

        StartCoroutine(LoadScene());
    }


    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                float progress = op.progress * 100.0f;
                int pRounded = Mathf.RoundToInt(progress);
                progressText.text = ((pRounded + 10).ToString() + "%");

                if (progressBar.fillAmount == 1.0f)
                    op.allowSceneActivation = true;
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                float progress = op.progress * 100.0f;
                int pRounded = Mathf.RoundToInt(progress);
                progressText.text = (pRounded.ToString() + "%");
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
        }
    }
}
