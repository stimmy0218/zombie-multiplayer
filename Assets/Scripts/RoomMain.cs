using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomMain : MonoBehaviour
{
    public UIPlayerList uiPlayerList;

    // void Awake()
    // {
    //     // 방 입장 완료
    //     EventDispatcher.instance.AddEventHandler(
    //         (int)EventEnums.EventType.OnJoinedRoom, 
    //         OnJoinedRoomEvent);
    // }

    void Start()
    {
        // 방이 없는 상태로 이 씬에 들어오는 경우 방어 코드
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.LogWarning("RoomMain: 현재 Photon 방이 없습니다.");
            return;
        }

        Debug.Log($"RoomMain Start / 현재 방: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"플레이어 수: {PhotonNetwork.PlayerList.Length}");
        
        // uiPlayerList가 연결 안 되어 있으면 바로 로그 찍고 종료
        if (uiPlayerList == null || uiPlayerList.uiPlayerItemPrefab == null)
        {
            Debug.LogWarning("RoomMain: uiPlayerList 또는 uiPlayerItemPrefab이 설정되지 않았습니다.");
            return;
        }

        Transform parent = uiPlayerList.transform;

        // 기존에 남아 있을 수도 있는 자식 UI들 제거 (선택 사항이지만 안전)
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }

        // Photon 방에 있는 모든 플레이어에 대해 UI 아이템 생성
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // 프리팹 인스턴스 생성 → 부모를 uiPlayerList로
            GameObject itemObj = Object.Instantiate(uiPlayerList.uiPlayerItemPrefab, parent);

            // UI 세팅
            UIPlayerItem uiItem = itemObj.GetComponent<UIPlayerItem>();
            if (uiItem != null)
            {
                bool isMasterClient = (player.ActorNumber == PhotonNetwork.MasterClient.ActorNumber);
                uiItem.Setup(player.NickName, isMasterClient);
            }
            else
            {
                Debug.LogWarning("RoomMain: UIPlayerItem 컴포넌트를 찾을 수 없습니다.");
            }
        }
    }
    
    // void OnJoinedRoomEvent(short eventType)
    // {
    //     Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");
    // }
}
