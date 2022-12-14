using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text statusText;
    public TMP_InputField nickNameInput;

    public PhotonView PV;

    public GameManager gameManager;


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

        if (nickNameInput.text.Length == 0)
        {
            nickNameInput.text = "Player_" + Random.Range(0,999).ToString();
        }

        PlayerPrefs.SetString("NickName", nickNameInput.text); 

        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;

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

        PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("NickName");
    }



    public void CreateRoom() => PhotonNetwork.CreateRoom("Master", new RoomOptions { MaxPlayers = 4 });

    public void JoinRoom() => PhotonNetwork.JoinRoom("Master");

    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom("Master", new RoomOptions { MaxPlayers = 4 }, null);

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

        Debug.Log("�濡 �����Ͽ����ϴ�.");

        gameManager.GameStart();
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => print("�游������");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("����������");

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        statusText.text = "���� ��� ���Ӱ� ���� ����ϴ�.";

        JoinOrCreateRoom();
    }



    [ContextMenu("����")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            print("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

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
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=#FF0000>" + otherPlayer.NickName + "���� �����ϼ̽��ϴ�.</color>");
    }

    [PunRPC] // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
    void ChatRPC(string msg)
    {
        TalkManager.instance.UseNotion(msg);
    }
}