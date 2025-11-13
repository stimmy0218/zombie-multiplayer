using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Pun2Manager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    public static Pun2Manager instance;

    // 룸 목록 캐시
    Dictionary<string, RoomInfo> cachedRooms = new Dictionary<string, RoomInfo>();

    // 싱글톤
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 마스터 서버 접속 시작
    public void Init()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();   // 세팅값으로 마스터 서버 접속
    }

    // 로비 입장
    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    // 닉네임 설정
    public void SetNickname(string nickname)
    {
        PhotonNetwork.NickName = nickname;
    }

    // 마스터 서버 접속 완료
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터서버에 접속 했습니다.");

        EventDispatcher.instance.SendEvent((int)EventEnums.EventType.OnConnectedToMaster);

        // 로비 입장
        JoinLobby();
    }

    // 로비 입장 시
    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 입장 했습니다.");

        // 로비 새로 들어왔으니 이전 캐시 초기화
        cachedRooms.Clear();

        EventDispatcher.instance.SendEvent((int)EventEnums.EventType.OnJoinedLobby);

        // UI에게 현재 방 0개 상태 알려줌
        EventDispatcher.instance.SendEvent(
            (int)EventEnums.EventType.OnRoomListUpdate,new List<RoomInfo>()
        );
    }

    // 로비에서 방 리스트 변경 (증분 리스트)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 1) 캐시에 반영
        foreach (var info in roomList)
        {
            if (info.RemovedFromList || info.PlayerCount == 0)
            {
                // 아무도 없거나 삭제된 방은 캐시에서 제거
                cachedRooms.Remove(info.Name);
            }
            else
            {
                // 존재하는 방이면 캐시에 추가/갱신
                cachedRooms[info.Name] = info;
            }
        }

        // 2) 전체 캐시 기준으로 리스트 만들기
        var allRooms = new List<RoomInfo>(cachedRooms.Values);

        Debug.Log($"OnRoomListUpdate rawCount: {roomList.Count}, cachedCount: {allRooms.Count}");

        // 3) UI에는 항상 전체 룸 목록만 전달
        EventDispatcher.instance.SendEvent(
            (int)EventEnums.EventType.OnRoomListUpdate,
            allRooms
        );
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"OnDisconnected: {cause}");
        cachedRooms.Clear(); // 끊기면 캐시도 비움
    }

    // 내가 방에 입장
    public override void OnJoinedRoom()
    {
        Debug.Log($"OnJoinedRoom : {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"IsMasterClient: {PhotonNetwork.IsMasterClient}");

        var me = PhotonNetwork.LocalPlayer;

        // 나보다 먼저 들어와 있던 사람들 출력
        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (p == me) continue;
            Debug.Log($"[{p.NickName}]님이 입장 했습니다.");
        }

        // 내 입장 메시지
        Debug.Log($"[{PhotonNetwork.NickName}]님이 입장 했습니다.");

        // 여기서 씬 전환하고 싶으면 (마스터만 씬 전환)
        // if (PhotonNetwork.IsMasterClient)
        // {
        //     PhotonNetwork.LoadLevel("Main");
        // }

        EventDispatcher.instance.SendEvent((int)EventEnums.EventType.OnJoinedRoom);
    }

    // 다른 플레이어가 내 방에 들어옴
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"[{newPlayer.NickName}]님이 입장 했습니다.");
    }

    // 랜덤 입장 실패 → 방 생성 (필요한 경우에만 사용)
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"OnJoinRandomFailed: {returnCode}, {message}");

        // 이름 null이면 자동으로 랜덤 이름의 방이 생성됨
        PhotonNetwork.CreateRoom(
            null,
            new RoomOptions { MaxPlayers = 2 }
        );
    }

    
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom: 방 생성 성공");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"OnCreateRoomFailed : {returnCode}, {message}");
    }

    // 다른 플레이어가 방을 나갔을 때
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"[{otherPlayer.NickName}]님이 퇴장 했습니다.");
    }

    // 내가 방을 나갔을 때
    public override void OnLeftRoom()
    {
        Debug.Log($"[{PhotonNetwork.NickName}]님이 방을 나갔습니다.");
        Debug.Log($"PhotonNetwork.InLobby: {PhotonNetwork.InLobby}");

        // 방 나간 뒤 로비로 자동 복귀
        PhotonNetwork.JoinLobby();
    }

    // UI에서 호출용 메서드들
    public void CreateRoom()
    {
        Debug.Log("방을 만듭니다.");
        PhotonNetwork.CreateRoom(
            null,
            new RoomOptions { MaxPlayers = 2 }
        );
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}