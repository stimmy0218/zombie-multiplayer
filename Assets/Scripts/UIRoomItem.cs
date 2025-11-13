using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomItem : MonoBehaviour
{
    public TMP_Text roomNameText;
    public Button joinButton;

    private RoomInfo info;

    public void Setup(RoomInfo info)
    {
        this.info = info;
        roomNameText.text = $"{info.Name} ({info.PlayerCount}/{info.MaxPlayers})";

        joinButton.onClick.RemoveAllListeners();
        joinButton.onClick.AddListener(OnClickJoin);
    }

    void OnClickJoin()
    {
        Debug.Log($"방 입장 시도: {info.Name}");
        PhotonNetwork.JoinRoom(info.Name);
    }
}