using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomScrollview : MonoBehaviour
{
    public Transform contentParent;         // ScrollView의 Content
    public GameObject roomItemPrefab;      // 방 하나를 표시할 프리팹

    public void Init()
    {
        // 룸 리스트 업데이트 이벤트 구독
        EventDispatcher.instance.AddEventHandler<List<RoomInfo>>(
            (int)EventEnums.EventType.OnRoomListUpdate,
            (short eventType, List<RoomInfo> roomList) =>
            {
                Debug.Log($"UIRoomScrollview RoomCount: {roomList.Count}");
                Refresh(roomList);  // UI 갱신
            });
    }

    //UI 표시
    public void Show()
    {
        gameObject.SetActive(true);
    }

    //UI 해제
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // 현재 방 아이템들 모두 제거
    void Clear()
    {
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }
    }

    // 새로운 방 리스트로 UI 다시 만들기
    public void Refresh(List<RoomInfo> roomList)
    {
        Clear();

        foreach (var info in roomList)
        {
            GameObject itemObj = Instantiate(roomItemPrefab, contentParent);
            var ui = itemObj.GetComponent<UIRoomItem>();
            ui.Setup(info);  // 각 아이템에 방 정보 세팅
        }
    }
}