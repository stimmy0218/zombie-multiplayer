using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomMain : MonoBehaviourPunCallbacks   
{
    public UIPlayerList uiPlayerList;
    private List<Player> playerList;
    public Button leaveButton;
    
    
    void Awake()
    {
        // // 방 입장 완료
        // EventDispatcher.instance.AddEventHandler(
        //     (int)EventEnums.EventType.OnJoinedRoom, 
        //     OnJoinedRoomEvent);
    }

    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.LogWarning("RoomMain: 현재 Photon 방이 없습니다.");
            return;
        }

        Debug.Log($"RoomMain Start / 현재 방: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"플레이어 수: {PhotonNetwork.PlayerList.Length}");

        RefreshPlayerList();  // 처음 한 번 그리기
    }

    // ✅ 플레이어 리스트 UI 다시 그리는 함수 (기존 Start 안에 있던 코드 분리만 했다고 생각하면 됨)
    private void RefreshPlayerList()
    {
        if (uiPlayerList == null || uiPlayerList.uiPlayerItemPrefab == null)
        {
            Debug.LogWarning("RoomMain: uiPlayerList 또는 uiPlayerItemPrefab 이 할당되지 않았습니다.");
            return;
        }

        Transform parent = uiPlayerList.transform;

        // 기존 아이템 제거
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }

        // 현재 방에 있는 모든 플레이어에 대해 UI 생성
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject itemObj = Instantiate(uiPlayerList.uiPlayerItemPrefab, parent);
            UIPlayerItem ui = itemObj.GetComponent<UIPlayerItem>();

            if (ui != null)
            {
                bool isMaster = player.ActorNumber == PhotonNetwork.MasterClient.ActorNumber;
                ui.Setup(player.NickName, isMaster);
            }
        }
    }

    // ✅ 새로운 플레이어가 들어올 때마다 호출
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"RoomMain: [{newPlayer.NickName}] 입장");
        RefreshPlayerList();
    }

    // ✅ 플레이어가 나갈 때마다 호출
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"RoomMain: [{otherPlayer.NickName}] 퇴장");
        RefreshPlayerList();
    }
}
    
    // void OnJoinedRoomEvent(short eventType)
    // {
    //     Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");
    // }

