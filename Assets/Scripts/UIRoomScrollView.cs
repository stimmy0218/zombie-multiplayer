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
                Refresh(roomList);
            });
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void Clear()
    {
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }
    }

    public void Refresh(List<RoomInfo> roomList)
    {
        Clear();

        foreach (var info in roomList)
        {
            GameObject itemObj = Instantiate(roomItemPrefab, contentParent);
            var ui = itemObj.GetComponent<UIRoomItem>();
            ui.Setup(info);
        }
    }
}