using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    public Button btn;
    public string Nickname;
    
    //게임 버전 체크, 버튼 클릭시 접속
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();  //마스터 서버 접속 요청
        
        btn.interactable = false;
        btn.onClick.AddListener(() => 
        { 
            Debug.Log("룸 접속 요청");
            btn.interactable = false;
            Connect();
        });
    }

    //접속 메서드
    void Connect()
    {
        Debug.Log($"IsConnected: {PhotonNetwork.IsConnected}");
        
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();  //마스터 서버 접속 요청
        }
    }

    //마스터 서버 접속
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        btn.interactable = true;
    }

    //접속 실패
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
    }

    //방 입장
    public override void OnJoinedRoom()
    {
        Debug.Log($"OnJoinedRoom: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log(PhotonNetwork.IsMasterClient);  //방장
        
        //내가 마스터가 아니라면 , 현재 마스터의 닉네임 출력
        // if (!PhotonNetwork.IsMasterClient && PhotonNetwork.MasterClient != null)
        // {
        //     Debug.Log($"[{PhotonNetwork.MasterClient.NickName}]님이 입장 했습니다.");    
        // }
        var me = PhotonNetwork.LocalPlayer;
        //Debug.Log(me.NickName);
        //Debug.Log(PhotonNetwork.PlayerList);
        
        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (p == me) continue;
            Debug.Log($"[{p.NickName}]님이 입장 했습니다.");
        }

        //내 입장 메시지 출력 
        Debug.Log($"[{PhotonNetwork.NickName}]님이 입장 했습니다.");
        
        //씬전환 
        PhotonNetwork.LoadLevel("Main");
        
    }

    //방 입장 실패
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"OnJoinRandomFailed: {returnCode}, {message}");
        //방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
    
    //방 만들기
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    //방 만들기 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"OnCreateRoomFailed: {returnCode}, {message}");
    }
    
    //플레이어 입장
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"[{newPlayer.NickName}]님이 입장 했습니다.");
    }
    
    //플레이어 퇴장
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"[{otherPlayer.NickName}]님이 퇴장 했습니다.");
    }

    //내가 퇴장
    public override void OnLeftRoom()
    {
        Debug.Log($"[{PhotonNetwork.NickName}]님이 방을 나갔습니다.");
    }
}

