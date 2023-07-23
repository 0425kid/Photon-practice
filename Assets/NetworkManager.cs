using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text StatusText, RoomText;
    public InputField RoomInput, NickNameInput;
    public GameObject UIManager;
    public static NetworkManager instance;
    public GameObject LocalPlayerPrefab;

    public PhotonView Photonview;

    private PlayerManager LocalPlayer;
    public string RoomToMove;

    void Awake()
    {
        instance = this;
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        if(PhotonNetwork.InLobby && RoomToMove != string.Empty)
        {
            JoinOrCreateRoom(RoomToMove);
            RoomToMove = null;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 완료");
        UIManager.GetComponent<UIManager>().ShowSimplePanel();
        PhotonNetwork.JoinLobby();

    }

    public void Join()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);
    }

    public void GeneratePlayer(string name)
    {
        PlayerManager newPlayer;

        // newPlayer = GameObject.Instantiate( network player avatar or model, spawn position, spawn rotation)
        newPlayer = PhotonNetwork.Instantiate("Cube",
                new Vector3(0, 5, 0), Quaternion.identity).GetComponent<PlayerManager>();
        newPlayer.SetName(name);
        LocalPlayer = newPlayer;
        InteractManager.instance.player = newPlayer;
    }

    //public void Connect() => PhotonNetwork.ConnectUsingSettings();
    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("서버 접속 완료");
    //    //PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
    //    UIManager.GetComponent<UIManager>().ShowControlPanel();
    //}


    public void DisConnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogFormat("연결 끊김, 사유 : {0}", cause);
    }


    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");
    }

    //방 만들기 및 참가
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text, new RoomOptions { MaxPlayers = 5 });
    public void JoinRoom(string RoomName) => PhotonNetwork.JoinRoom(RoomName);
    public void JoinOrCreateRoom(string RoomName) => PhotonNetwork.JoinOrCreateRoom(RoomName, new RoomOptions { MaxPlayers = 5 }, null);
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        RoomText.text = "Not in the room";
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
        //PhotonNetwork.JoinOrCreateRoom("Team1", new RoomOptions { MaxPlayers = 5 }, null);
    }


    public override void OnCreatedRoom() => print("방 만들기 완료");
    public override void OnJoinedRoom()
    {
        Debug.LogFormat("방 참가 완료 : {0}", PhotonNetwork.CurrentRoom);
        RoomText.text = PhotonNetwork.CurrentRoom.Name;
        UIManager.GetComponent<UIManager>().HideSimplePanel();
        GeneratePlayer(NickNameInput.text);

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("방 만들기 실패, 사유 : {0} : {1}", returnCode, message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("방 참가 실패, 사유 : {0} : {1}", returnCode, message);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("방 랜덤참가 실패");
    }


    public void SendChatting(string text)
    {
        //Photonview.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + text);
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
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            }
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

    [ContextMenu("상태")]
    void Status()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
    }
    
}
