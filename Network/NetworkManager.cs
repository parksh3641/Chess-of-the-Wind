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
        statusText.text = "브론즈1 이상 랭크가 필요합니다.";
    }

    void OnApplicationPause(bool pause) //앱이 꺼질때
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
        statusText.text = "서버에 접속중입니다.";

        //PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        statusText.text = "서버에 연결되었습니다";

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
        statusText.text = "서버와 연결이 끊겼습니다.";

        gameManager.GameStop();

        Connect();
    }



    public void JoinLobby()
    {
        statusText.text = "방을 찾고 있습니다.";

        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        statusText.text = "로비에 연결되었습니다";

        PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("NickName");
    }



    public void CreateRoom() => PhotonNetwork.CreateRoom("Master", new RoomOptions { MaxPlayers = 4 });

    public void JoinRoom() => PhotonNetwork.JoinRoom("Master");

    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom("Master", new RoomOptions { MaxPlayers = 4 }, null);

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom()
    {
        Debug.Log("방을 떠났습니다.");

        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방을 만들었습니다.");
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "방에 참가하였습니다.";

        Debug.Log("방에 참가하였습니다.");

        gameManager.GameStart();
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => print("방만들기실패");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("방참가실패");

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        statusText.text = "방이 없어서 새롭게 방을 만듭니다.";

        JoinOrCreateRoom();
    }



    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=#00FF00>" + newPlayer.NickName + "님이 참가하셨습니다.</color>");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=#FF0000>" + otherPlayer.NickName + "님이 퇴장하셨습니다.</color>");
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        TalkManager.instance.UseNotion(msg);
    }
}