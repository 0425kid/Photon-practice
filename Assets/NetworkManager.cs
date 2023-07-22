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

    void Awake()
    {
        instance = this;
        Screen.SetResolution(960, 540, false);
    }

    // Update is called once per frame
    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void GeneratePlayer(string name)
    {
        PlayerManager newPlayer;

        // newPlayer = GameObject.Instantiate( network player avatar or model, spawn position, spawn rotation)
        newPlayer = PhotonNetwork.Instantiate("Cube",
                new Vector3(0, 5, 0), Quaternion.identity).GetComponent<PlayerManager>();
        newPlayer.SetName(name);
        LocalPlayer = newPlayer;
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ���� �Ϸ�");
        //PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        UIManager.GetComponent<UIManager>().ShowControlPanel();
    }


    public void DisConnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogFormat("���� ����, ���� : {0}", cause);
    }


    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        Debug.Log("�κ����ӿϷ�");
    }

    //�� ����� �� ����
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text, new RoomOptions { MaxPlayers = 5 });
    public void JoinRoom() => PhotonNetwork.JoinRoom(RoomInput.text);
    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(RoomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        RoomText.text = "Not in the room";
    }
    public override void OnCreatedRoom() => print("�� ����� �Ϸ�");
    public override void OnJoinedRoom()
    {
        Debug.LogFormat("�� ���� �Ϸ� : {0}", PhotonNetwork.CurrentRoom);
        RoomText.text = PhotonNetwork.CurrentRoom.Name;
        UIManager.GetComponent<UIManager>().StartGame();

        GeneratePlayer(NickNameInput.text);

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
}
