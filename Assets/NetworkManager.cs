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

    //�����ϰ��� �ϴ� �� �ε��� ����
    public int TeamIndex;

    public PhotonView Photonview;

    public string PlayerPrefabName = "PlayerPrefab";

    private PlayerManager LocalPlayer;
    public string RoomToMove;

    void Awake()
    {
        instance = this;
        Debug.Log("���� ����");
        //Screen.SetResolution(1920, 1080, false);
        //Screen.SetResolution(960, 540, false);
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
        Debug.Log("���� ���� �Ϸ�");
        UIManager.GetComponent<UIManager>().ShowSimplePanel();
        PhotonNetwork.JoinLobby();

    }

    public void Join()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);
    }

    public void GeneratePlayer(string name)
    {
        player newPlayer;
        newPlayer = PhotonNetwork.Instantiate(PlayerPrefabName,
                new Vector3(0, 5, 0), Quaternion.identity).GetComponent<player>();

        newPlayer.playername = name;

        //CameraMovement.instance.Set();
        CameraMovement.instance.objectTofollow = newPlayer.followCam.transform;
        
    }


    public void DisConnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogFormat("���� ����, ���� : {0}", cause);
    }


    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� �Ϸ�");
    }

    //�� ����� �� ����
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text, new RoomOptions { MaxPlayers = 5 });
    public void JoinRoom(string RoomName) => PhotonNetwork.JoinRoom(RoomName);
    public void JoinOrCreateRoom(string RoomName) => PhotonNetwork.JoinOrCreateRoom(RoomName, new RoomOptions { MaxPlayers = 5 }, null);
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    public void LeaveRoom(string RoomName)
    {
        PhotonNetwork.LeaveRoom();
        RoomToMove = RoomName;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        RoomText.text = "Not in the room";
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
        //PhotonNetwork.JoinOrCreateRoom("Team1", new RoomOptions { MaxPlayers = 5 }, null);
    }


    public override void OnCreatedRoom() => print("�� ����� �Ϸ�");
    public override void OnJoinedRoom()
    {
        Debug.LogFormat("�� ���� �Ϸ� : {0}", PhotonNetwork.CurrentRoom);
        RoomText.text = PhotonNetwork.CurrentRoom.Name;
        UIManager.GetComponent<UIManager>().ClearPanels();
        GeneratePlayer(NickNameInput.text);
        try
        {
            TeamIndex = int.Parse(TeamInput.text);
        }
        catch
        {
            TeamIndex = 0;
        }

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("�� ����� ����, ���� : {0} : {1}", returnCode, message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("�� ���� ����, ���� : {0} : {1}", returnCode, message);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("�� �������� ����");
    }


    public void SendChatting(string text)
    {
        //Photonview.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + text);
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
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            }
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

    public void SetPlayerPrefab(string PrefabName)
    {
        PlayerPrefabName = PrefabName;
    }

    [ContextMenu("����")]
    void Status()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
    }
    
}
