using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomMain : MonoBehaviour
{
    public UIPlayerList uiPlayerList;

    void Awake()
    {
        // 방 입장 완료
        EventDispatcher.instance.AddEventHandler(
            (int)EventEnums.EventType.OnJoinedRoom, 
            OnJoinedRoomEvent);
    }

    void OnJoinedRoomEvent(short eventType)
    {
        Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");

        //PhotonNetwork.LocalPlayer.NickName
    }

    
    
}
