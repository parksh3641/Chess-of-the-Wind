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
    public PhotonView PV;

    public bool isDelay = false;

    public StateManager stateManager;
    public GameManager gameManager;
    public CharacterManager characterManager;


    void Awake()
    {

    }


    public void Initialize()
    {
        Connect();
    }


    void OnApplicationPause(bool pause) //앱이 꺼질때
    {
        if (pause)
        {
            //Disconnect();
        }
        else
        {

        }
    }

    public void Connect()
    {
        Debug.Log("서버에 접속중입니다.");

        //PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버에 연결되었습니다.");

        JoinLobby();
    }



    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("서버와 연결이 끊겼습니다.");

        gameManager.BetOptionLeaveGame();

        Connect();
    }



    public void JoinLobby()
    {
        Debug.Log("로비에 접속하고 있습니다.");

        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 연결되었습니다.");

        stateManager.ServerConnectComplete();
    }



    public void CreateRoom()
    {
        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            RoomOptions roomOption = new RoomOptions();
            roomOption.MaxPlayers = 2;

            PhotonNetwork.JoinOrCreateRoom("NewBie", roomOption, null);
        }
        else
        {
            RoomOptions roomOption = new RoomOptions();
            roomOption.MaxPlayers = 2;

            PhotonNetwork.JoinOrCreateRoom("Gosu", roomOption, null);
        }
    }

    public void JoinRoom()
    {
        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            PhotonNetwork.JoinRoom("NewBie");
        }
        else
        {
            PhotonNetwork.JoinRoom("Gosu");
        }
    }

    public void JoinOrCreateRoom_Developer()
    {
        if (isDelay) return;

        isDelay = true;

        SetNickName();

        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom("Master", roomOption, null);

        GameStateManager.instance.GameType = GameType.Gosu;
    }

    public void JoinOrCreateRoom_NewBie()
    {
        if (isDelay) return;

        isDelay = true;

        SetNickName();

        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom("NewBie", roomOption, null);

        GameStateManager.instance.GameType = GameType.NewBie;
    }

    public void JoinOrCreateRoom_Gosu()
    {
        if (isDelay) return;

        isDelay = true;

        SetNickName();

        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom("Gosu", roomOption, null);

        GameStateManager.instance.GameType = GameType.Gosu;
    }

    void SetNickName()
    {
        if (GameStateManager.instance.NickName.Length == 0)
        {
            GameStateManager.instance.NickName = "Player_" + Random.Range(0, 999).ToString();
        }

        PlayerPrefs.SetString("NickName", GameStateManager.instance.NickName);

        PhotonNetwork.LocalPlayer.NickName = GameStateManager.instance.NickName;
    }

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom()
    {
        isDelay = false;

        Debug.Log("방을 떠났습니다.");

        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방을 만들었습니다.");
    }

    public override void OnJoinedRoom()
    {
        isDelay = false;

        Debug.Log("방에 참가하였습니다.");

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Waiting" } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Roulette", "Right" } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Pinball", GameStateManager.instance.NickName } });

            int number = Random.Range(0, 24);
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "LeftNumber", number } });

            int number2 = Random.Range(0, 24);
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "RightNumber", number2 } });

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Total", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Total", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player3_Total", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player4_Total", 0 } });

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Minus", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Minus", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player3_Minus", 0 } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player4_Minus", 0 } });
        }

        characterManager.AddAllPlayer();

        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            gameManager.GameStart_NewBie();
        }
        else
        {
            gameManager.GameStart_Gosu();
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        isDelay = false;

        Debug.Log("방 만들기 실패했습니다.");
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        isDelay = false;

        Debug.Log("방 참가 실패했습니다.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        isDelay = false;

        Debug.Log("랜덤방에 참가할 수 없습니다.");
    }



    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            print("현재 방 인게임 상태 : " + ht["Status"].ToString());
            print("다음 핀볼 칠 사람 : " + ht["Pinball"].ToString());

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

        characterManager.AddPlayer(newPlayer.NickName);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=#FF0000>" + otherPlayer.NickName + "님이 퇴장하셨습니다.</color>");

        characterManager.DeletePlayer(otherPlayer.NickName);
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        TalkManager.instance.UseNotion(msg);
    }
}