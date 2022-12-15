using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text statusText;
    public TMP_InputField nickNameInput;

    public PhotonView PV;

    public GameManager gameManager;
    public CharacterManager characterManager;


    void Awake()
    {
        statusText.text = "";

        if (PlayerPrefs.GetString("NickName").Length > 0) nickNameInput.text = PlayerPrefs.GetString("NickName");
    }

    private void Start()
    {
        Connect();
    }

    void Update()
    {
        //statusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void Gosu()
    {
        statusText.text = "�����1 �̻� ��ũ�� �ʿ��մϴ�.";
    }

    void OnApplicationPause(bool pause) //���� ������
    {
        if (pause)
        {
            Disconnect();
        }
        else
        {

        }
    }

    public void Connect()
    {
        statusText.text = "������ �������Դϴ�.";

        //PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        statusText.text = "������ ����Ǿ����ϴ�";

        JoinLobby();
    }



    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        statusText.text = "������ ������ ������ϴ�.";

        gameManager.GameStop();

        Connect();
    }



    public void JoinLobby()
    {
        statusText.text = "���� ã�� �ֽ��ϴ�.";

        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        statusText.text = "�κ� ����Ǿ����ϴ�";
    }



    public void CreateRoom() => PhotonNetwork.CreateRoom("Master", new RoomOptions { MaxPlayers = 4 });

    public void JoinRoom() => PhotonNetwork.JoinRoom("Master");

    public void JoinOrCreateRoom()
    {
        if (nickNameInput.text.Length == 0)
        {
            nickNameInput.text = "Player_" + Random.Range(0, 999).ToString();
        }

        PlayerPrefs.SetString("NickName", nickNameInput.text);

        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;

        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom("Master", roomOption, null);
    }

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom()
    {
        Debug.Log("���� �������ϴ�.");

        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("���� ��������ϴ�.");
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "�濡 �����Ͽ����ϴ�.";

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Waiting" } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Pinball", PlayerPrefs.GetString("NickName") } });
        }

        gameManager.GameStart();

        characterManager.AddAllPlayer();

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusText.text = "�� ����� �����߽��ϴ�.";
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = "�� ���� �����߽��ϴ�.";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        statusText.text = "�����濡 ������ �� �����ϴ�.";
    }



    [ContextMenu("����")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            print("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            print("���� �� �ΰ��� ���� : " + ht["Status"].ToString());
            print("���� �ɺ� ĥ ��� : " + ht["Pinball"].ToString());

            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            print("�� ���� : " + PhotonNetwork.CountOfRooms);
            print("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
            print("�κ� �ִ���? : " + PhotonNetwork.InLobby);
            print("����ƴ���? : " + PhotonNetwork.IsConnected);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=#00FF00>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�.</color>");

        characterManager.AddPlayer(newPlayer.NickName);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=#FF0000>" + otherPlayer.NickName + "���� �����ϼ̽��ϴ�.</color>");

        characterManager.DeletePlayer(otherPlayer.NickName);
    }

    [PunRPC] // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
    void ChatRPC(string msg)
    {
        TalkManager.instance.UseNotion(msg);
    }
}