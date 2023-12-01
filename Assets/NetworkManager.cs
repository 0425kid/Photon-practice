using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text StatusText, RoomText;
    public InputField RoomInput, NickNameInput, TeamInput;
    public GameObject UIManager;
    public static NetworkManager instance;
    public GameObject LocalPlayerPrefab;
    public GameObject LocalPlayer;


    //접속하고자 하는 팀 인덱스 저장
    public int TeamIndex;

    public PhotonView Photonview;

    public string PlayerPrefabName = "PlayerPrefab";

    public string RoomToMove;
    public Text PhotonStatusText;

    public string nickName;

    void Awake()
    {
        instance = this;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        //StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        if(!PhotonNetwork.InRoom)
        {
            PhotonStatusText.text =
                $"포톤에 접속한 인원 : {PhotonNetwork.CountOfPlayers}\n" +
                $"모든 방에 있는 인원 : {PhotonNetwork.CountOfPlayersInRooms}\n";
            return;
        }
        else
        {
            PhotonStatusText.text =
                $"현재 방 이름 : {PhotonNetwork.CurrentRoom.Name}\n" +
                $"현재 방에 있는 인원 : {PhotonNetwork.CurrentRoom.PlayerCount}\n";

        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 완료");
        //UIManager.GetComponent<UIManager>().ShowSimplePanel();
        PhotonNetwork.JoinLobby();

    }

    //public void Join()
    //{

    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);
    //}
    public void Join()
    {
        string roomName = $"{TeamIndex}";
        Debug.Log("로딩 이미지");
        UIManager.GetComponent<UIManager>().ToggleLoading(true);
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 7 }, null);
    }

    //public void Join(int teamIndex)
    //{
    //    string roomName = "Team" + teamIndex;
    //    PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 6 }, null);
    //}

    

    public void GeneratePlayer(string name)
    {
        
        player newPlayer;
        LocalPlayer = PhotonNetwork.Instantiate(PlayerPrefabName,
                new Vector3(0, 5, 0), Quaternion.identity);
        newPlayer = LocalPlayer.GetComponent<player>();

        //newPlayer = PhotonNetwork.Instantiate(PlayerPrefabName,
        //        new Vector3(0, 5, 0), Quaternion.identity).GetComponent<player>();

        newPlayer.playername = name;
        ModelManager.instance.SetHongbo();
        UIManager.GetComponent<UIManager>().ToggleLoading(false);

        //CameraMovement.instance.Set();
        //CameraMovement.instance.objectTofollow = newPlayer.followCam.transform;

    }


    public void DisConnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause == DisconnectCause.MaxCcuReached)
        {
            PopupManager.instance.EmitPopup("최대 CCU를 초과했습니다!");
            Debug.LogError("최대 CCU를 초과했습니다!");
            return;
        }
        Debug.LogFormat("연결 끊김, 사유 : {0}", cause);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    public void JoinLobby()
    {
        Debug.Log("로비에 접속할게요!");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");
        if (RoomToMove != string.Empty)
        {
            JoinOrCreateRoom(RoomToMove);
            RoomToMove = null;
        }
    }

    //방 만들기 및 참가
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text, new RoomOptions { MaxPlayers = 5 });
    public void JoinRoom(string RoomName) => PhotonNetwork.JoinRoom(RoomName);
    public void JoinOrCreateRoom(string RoomName) => PhotonNetwork.JoinOrCreateRoom(RoomName, new RoomOptions { MaxPlayers = 5 }, null);
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    public void LeaveRoom(string RoomName)
    {
        PhotonNetwork.LeaveRoom();
        RoomToMove = RoomName;
        Debug.Log("room to move : " + RoomName);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        try
        {
            RoomText.text = "Not in the room";
        }
        catch
        {
            return;
        }
        
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
    }


    public override void OnCreatedRoom() => print("방 만들기 완료");
    public override void OnJoinedRoom()
    {
        Debug.LogFormat("방 참가 완료 : {0}", PhotonNetwork.CurrentRoom);
        RoomText.text = PhotonNetwork.CurrentRoom.Name;
        UIManager.GetComponent<UIManager>().HideLobbyCanvas();
        UIManager.GetComponent<UIManager>().ShowForumCanvas();
        GeneratePlayer(nickName);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        PopupManager.instance.EmitPopup($"방 참가 실패, 사유 : {message}");
        Debug.LogErrorFormat("방 만들기 실패, 사유 : {0} : {1}", returnCode, message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PopupManager.instance.EmitPopup($"방 참가 실패, 사유 : {message}");
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

    public void SetNickName()
    {
        nickName = NickNameInput.text;
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

    public void SetPlayerPrefab(string PrefabName)
    {
        PlayerPrefabName = PrefabName;
    }

    [ContextMenu("상태")]
    void Status()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
    }

    public void ChangeTeam(int index)
    {
        Debug.Log($"index {index}로 팀변경");
        TeamIndex = index;
        UIManager.GetComponent<UIManager>().ToggleLoading(true);
        LeaveRoom(index.ToString());
        ImageManager.instance.ChangeTeam(index);
    }

    public void SetTeamIndex(int index)
    {
        //Debug.Log($"Teamindex : {index}");
        TeamIndex = index;
    }

    public void Hello()
    {
        Debug.Log("hello");
    }
    
}
