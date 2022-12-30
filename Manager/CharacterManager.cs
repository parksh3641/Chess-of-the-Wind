using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterContent[] characterContents;

    private int index = 0;

    private void Awake()
    {
        for(int i = 0; i < characterContents.Length; i ++)
        {
            characterContents[i].gameObject.SetActive(false);
        }
    }

    public void AddPlayer(string name)
    {
        if (index > characterContents.Length - 1)
        {
            return;
        }

        characterContents[index].gameObject.SetActive(true);
        characterContents[index].Initialize(name);

        index++;
    }

    public void AddAllPlayer()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            AddPlayer(PhotonNetwork.PlayerList[i].NickName);

            if (PhotonNetwork.PlayerList[i].NickName.Equals(GameStateManager.instance.NickName))
            {
                characterContents[i].CheckMy();
            }
        }
    }

    public void DeletePlayer(string name)
    {
        for (int i = 0; i < characterContents.Length; i++)
        {
            if (characterContents[i].nickNameText.text.Equals(name))
            {
                characterContents[i].gameObject.SetActive(false);
                break;
            }
        }

        index--;
    }

    public void DeleteAllPlayer()
    {
        for (int i = 0; i < characterContents.Length; i++)
        {
            characterContents[i].gameObject.SetActive(false);
        }
    }

    public void CheckPlayer(string name)
    {
        for (int i = 0; i < characterContents.Length; i++)
        {
            characterContents[i].focus.SetActive(false);
        }

        for (int i = 0; i < characterContents.Length; i++)
        {
            if(characterContents[i].nickNameText.text.Equals(name))
            {
                characterContents[i].focus.SetActive(true);
                break;
            }
        }
    }
}
