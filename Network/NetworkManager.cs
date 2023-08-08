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
    public int index = 0;

    public PhotonView PV;

    public string playerNickName1 = ""; //방장
    public string playerNickName2 = "";

    private RoomOptions roomOption;

    public int otherFormation = 0;

    public bool isDelay = false;

    public GameManager gameManager;
    public StateManager stateManager;
    public MatchingManager matchingManager;
    public UIManager uIManager;

    public GameObject disConnectedView;

    PlayerDataBase playerDataBase;

    void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void Initialize(int number)
    {
        index = number;

        Connect();
    }

    public void ReConnect()
    {
        if(!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);

            return;
        }

        disConnectedView.SetActive(false);

        Connect();
    }

    void OnApplicationQuit()
    {
        //if (GameStateManager.instance.Playing)
        //{
        //    GameStateManager.instance.Penalty = 1;
        //}
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

        if (cause != DisconnectCause.DisconnectByClientLogic)
        {
            matchingManager.CheckServer_Fail();
        }

        if(GameStateManager.instance.Playing)
        {
            uIManager.OpenDisconnectedView();
        }
    }



    public void JoinLobby()
    {
        Debug.Log("로비에 접속하고 있습니다.");

        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 연결되었습니다.");

        if(!GameStateManager.instance.Playing)
        {
            switch (index)
            {
                case 0:
                    matchingManager.CheckServer_NewBie();
                    break;
                case 1:
                    matchingManager.CheckServer_Gosu();
                    break;
            }
        }
        else
        {
            if (GameStateManager.instance.Room.Length != 0)
            {
                Debug.Log(GameStateManager.instance.Room + " 방에 재입장을 시도합니다");

                PhotonNetwork.JoinRoom(GameStateManager.instance.Room);
            }
            else
            {
                Debug.Log("진행 중인 방에 참여할 수 없어 새로운 방을 만듭니다.");

                GameStateManager.instance.Playing = false;
                GameStateManager.instance.Room = "";

                switch (index)
                {
                    case 0:
                        matchingManager.CheckServer_NewBie();
                        break;
                    case 1:
                        matchingManager.CheckServer_Gosu();
                        break;
                }
            }
        }
    }



    //public void CreateRoom()
    //{
    //    if(GameStateManager.instance.GameType == GameType.NewBie)
    //    {
    //        RoomOptions roomOption = new RoomOptions();
    //        roomOption.MaxPlayers = 2;

    //        PhotonNetwork.JoinOrCreateRoom("NewBie", roomOption, null);
    //    }
    //    else
    //    {
    //        RoomOptions roomOption = new RoomOptions();
    //        roomOption.MaxPlayers = 2;

    //        PhotonNetwork.JoinOrCreateRoom("Gosu", roomOption, null);
    //    }
    //}

    //public void JoinRoom()
    //{
    //    if (GameStateManager.instance.GameType == GameType.NewBie)
    //    {
    //        PhotonNetwork.JoinRoom("NewBie");
    //    }
    //    else
    //    {
    //        PhotonNetwork.JoinRoom("Gosu");
    //    }
    //}


    public void JoinRandomRoom_Newbie()
    {
        if (isDelay) return;

        isDelay = true;

        SetNickName();

        GameStateManager.instance.GameType = GameType.NewBie;

        Hashtable roomProperties = new Hashtable() { { "GameRank", GameStateManager.instance.GameRankType }, { "Status", "Waiting" } };

        PhotonNetwork.JoinRandomRoom(roomProperties, 2);
    }

    public void JoinRandomRoom_Gosu()
    {
        if (isDelay) return;

        isDelay = true;

        SetNickName();

        GameStateManager.instance.GameType = GameType.Gosu;

        Hashtable roomProperties = new Hashtable() { { "GameRank", GameStateManager.instance.GameRankType }, { "Status", "Waiting" } };

        PhotonNetwork.JoinRandomRoom(roomProperties, 2);
    }

    void JoinOrCreateRoom_Newbie()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        roomOption.CustomRoomPropertiesForLobby = new string[] { "GameRank", "Status" };
        roomOption.CustomRoomProperties = new Hashtable() { { "GameRank", GameStateManager.instance.GameRankType }, { "Status", "Waiting" } };

        PhotonNetwork.JoinOrCreateRoom(GameStateManager.instance.NickName, roomOption, null);
    }

    void JoinOrCreateRoom_Gosu()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        roomOption.CustomRoomPropertiesForLobby = new string[] { "GameRank", "Status" };
        roomOption.CustomRoomProperties = new Hashtable() { { "GameRank", GameStateManager.instance.GameRankType }, { "Status", "Waiting" } };

        PhotonNetwork.JoinOrCreateRoom(GameStateManager.instance.NickName, roomOption, null);
    }

    void SetNickName()
    {
        if (GameStateManager.instance.NickName.Length == 0)
        {
            GameStateManager.instance.NickName = "Player_" + Random.Range(0, 999).ToString();
        }

        PhotonNetwork.LocalPlayer.NickName = GameStateManager.instance.NickName;
    }

    public void LeaveRoom()
    {
        isDelay = false;

        GameStateManager.instance.Playing = false;
        GameStateManager.instance.Room = "";

        Debug.Log("서버를 떠났습니다.");

        PhotonNetwork.Disconnect();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방을 만들었습니다.");
    }

    public override void OnJoinedRoom()
    {
        isDelay = false;

        Debug.Log("방에 참여하였습니다.");

        if (PhotonNetwork.IsMasterClient)
        {
            GameStateManager.instance.Playing = false;
            GameStateManager.instance.ReEnter = false;

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Bouns", false } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Roulette", "Right" } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Pinball", GameStateManager.instance.NickName } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Status", "Waiting" } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Room", GameStateManager.instance.NickName } });

            GameStateManager.instance.Room = GameStateManager.instance.NickName;

            int number = Random.Range(0, 2);
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "WindCharacter", number.ToString() } });

            int number2 = Random.Range(0, 24);
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "LeftNumber", number2 } });

            int number3 = Random.Range(0, 24);
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "RightNumber", number3 } });

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Total", GameStateManager.instance.Stakes } });
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Total", GameStateManager.instance.Stakes } });

            //PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Minus", 0 } });
            //PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Minus", 0 } });

            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player1_Formation", playerDataBase.Formation } });
        }
        else
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Player2_Formation", playerDataBase.Formation } });

            Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            GameStateManager.instance.Room = ht["Room"].ToString();

            playerNickName1 = PhotonNetwork.PlayerList[0].NickName;
            playerNickName2 = GameStateManager.instance.NickName;

            Debug.Log("룸 이름 : " + GameStateManager.instance.Room);
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            StartCoroutine(DelayEnterGame());
        }

        if(GameStateManager.instance.Playing)
        {
            Debug.Log("방에 재입장하였습니다");

            GameStateManager.instance.ReEnter = true;
        }
        else
        {
            GameStateManager.instance.ReEnter = false;
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

        if(GameStateManager.instance.Playing)
        {
            Debug.Log("재입장 하려고 했으나 방이 없습니다");

            GameStateManager.instance.Playing = false;
            GameStateManager.instance.Room = "";

            switch (index)
            {
                case 0:
                    matchingManager.CheckServer_NewBie();
                    break;
                case 1:
                    matchingManager.CheckServer_Gosu();
                    break;
            }

            //gameManager.Draw();
        }
        else
        {
            Debug.Log("방 참가 실패했습니다.");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        isDelay = false;

        Debug.Log("랜덤방이 없어서 방을 만듭니다.");

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            JoinOrCreateRoom_Newbie();
        }
        else
        {
            JoinOrCreateRoom_Gosu();
        }
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
        Debug.Log(newPlayer.NickName + "님이 입장하셨습니다");

        if (PhotonNetwork.IsMasterClient)
        {
            if(!GameStateManager.instance.Playing)
            {
                StartCoroutine(DelayEnterGame());
            }
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + "님이 퇴장하셨습니다.");

        if (GameStateManager.instance.Playing)
        {
            gameManager.CheckGameState();
        }
    }
       

    IEnumerator DelayEnterGame()
    {
        yield return new WaitForSeconds(1.0f);

        playerNickName1 = GameStateManager.instance.NickName;
        playerNickName2 = PhotonNetwork.PlayerList[1].NickName;

        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        if (PhotonNetwork.IsMasterClient)
        {
            otherFormation = int.Parse(ht["Player2_Formation"].ToString());
        }
        else
        {
            otherFormation = int.Parse(ht["Player1_Formation"].ToString());
        }

        string nickName = "";

        if (!PhotonNetwork.PlayerList[0].NickName.Equals(GameStateManager.instance.NickName))
        {
            nickName = PhotonNetwork.PlayerList[0].NickName;
        }
        else
        {
            nickName = PhotonNetwork.PlayerList[1].NickName;
        }

        matchingManager.PlayerMatching(nickName, GameStateManager.instance.NickName, otherFormation);
    }
}