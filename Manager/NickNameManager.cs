﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class NickNameManager : MonoBehaviour
{
    public GameObject nickNameView;
    public GameObject nickNameFirstView;

    public GameObject exitButton;

    public GameObject[] confirmButton;

    public InputField inputField;
    public InputField inputFieldFree;

    public Text profileNickNameText;

    private int value = 1000000;

    public string[] lines;
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    public UIManager uIManager;
    public PlayerDataBase playerDataBase;
    public FormationManager formationManager;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        nickNameView.SetActive(false);
        nickNameFirstView.SetActive(false);

        exitButton.SetActive(false);

        confirmButton[0].SetActive(false);
        confirmButton[1].SetActive(false);

        string file = SystemPath.GetPath() + "BadWord.txt";

        string source;

        if (File.Exists(file))
        {
            StreamReader word = new StreamReader(file);
            source = word.ReadToEnd();
            word.Close();

            lines = Regex.Split(source, LINE_SPLIT_RE);
        }
    }

    public void Initialize()
    {
        if (GameStateManager.instance.NickName.Length > 15)
        {
            OpenFreeNickName();
        }
        //else
        //{
        //    formationManager.Initialize();
        //}
    }

    public void OpenFreeNickName()
    {
        inputField.text = "";

        nickNameView.SetActive(true);

        exitButton.SetActive(false);

        confirmButton[0].SetActive(true);
        confirmButton[1].SetActive(false);
    }

    public void OpenNickName()
    {
        if (!nickNameView.activeSelf)
        {
            inputField.text = "";

            nickNameView.SetActive(true);

            exitButton.SetActive(true);

            confirmButton[0].SetActive(false);
            confirmButton[1].SetActive(true);
        }
        else
        {
            nickNameView.SetActive(false);
        }
    }

    public void CheckNickName()
    {
        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            NotionManager.instance.UseNotion(NotionType.CheckInternet);

            return;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            if (inputField.text.ToLower().Contains(lines[i]))
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                NotionManager.instance.UseNotion(NotionType.NickNameNotion3);
                return;
            }
        }

        UIManager.instance.Renewal();

        if (playerDataBase.Coin >= value)
        {
            string Check = Regex.Replace(inputField.text, @"[^a-zA-Z0-9가-힣]", "", RegexOptions.Singleline);

            if (inputField.text.Equals(Check) == true)
            {
                string newNickName = ((inputField.text.Trim()).Replace(" ", ""));
                string oldNickName = "";

                if(GameStateManager.instance.NickName != null)
                {
                    oldNickName = GameStateManager.instance.NickName.Trim().Replace(" ", "");
                }
                else
                {
                    oldNickName = "";
                }

                if (newNickName.Length > 2)
                {
                    if (!(newNickName.Equals(oldNickName)))
                    {
                        PlayfabManager.instance.UpdateDisplayName(newNickName, Success, Failure);
                    }
                    else
                    {
                        SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                        NotionManager.instance.UseNotion(NotionType.NickNameNotion1);
                    }
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.NickNameNotion2);
                }
            }
            else
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                NotionManager.instance.UseNotion(NotionType.NickNameNotion3);
            }
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            NotionManager.instance.UseNotion(NotionType.NickNameNotion4);
        }
    }

    public void CheckFreeNickName()
    {
        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            NotionManager.instance.UseNotion(NotionType.CheckInternet);

            return;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            if (inputFieldFree.text.ToLower().Contains(lines[i]))
            {
                Debug.Log("123");

                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                NotionManager.instance.UseNotion(NotionType.NickNameNotion3);
                return;
            }
        }

        string Check = Regex.Replace(inputFieldFree.text, @"[^a-zA-Z0-9가-힣]", "", RegexOptions.Singleline);

        if (inputFieldFree.text.Equals(Check) == true)
        {
            string newNickName = ((inputFieldFree.text.Trim()).Replace(" ", ""));
            string oldNickName = "";

            if (GameStateManager.instance.NickName != null)
            {
                oldNickName = GameStateManager.instance.NickName.Trim().Replace(" ", "");
            }
            else
            {
                oldNickName = "";
            }

            if (newNickName.Length > 2)
            {
                if (!(newNickName.Equals(oldNickName)))
                {
                    PlayfabManager.instance.UpdateDisplayName(newNickName, FreeSuccess, Failure);
                }
                else
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                    NotionManager.instance.UseNotion(NotionType.NickNameNotion1);
                }
            }
            else
            {
                SoundManager.instance.PlaySFX(GameSfxType.Wrong);
                NotionManager.instance.UseNotion(NotionType.NickNameNotion2);
            }
        }
        else
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            NotionManager.instance.UseNotion(NotionType.NickNameNotion3);
        }
    }

    public void Success()
    {
        if (uIManager != null) uIManager.Renewal();

        PlayfabManager.instance.UpdateSubtractGold(value);

        profileNickNameText.text = GameStateManager.instance.NickName;

        NotionManager.instance.UseNotion(NotionType.NickNameNotion6);

        SoundManager.instance.PlaySFX(GameSfxType.Success);

        nickNameView.SetActive(false);

        FirebaseAnalytics.LogEvent("Open_Nickname");
    }

    public void FreeSuccess()
    {
        if(uIManager != null) uIManager.Renewal();

        NotionManager.instance.UseNotion(NotionType.NickNameNotion6);

        SoundManager.instance.PlaySFX(GameSfxType.Success);

        nickNameFirstView.SetActive(false);
    }

    public void Failure()
    {
        SoundManager.instance.PlaySFX(GameSfxType.Wrong);

        NotionManager.instance.UseNotion(NotionType.NickNameNotion5);
    }
}
