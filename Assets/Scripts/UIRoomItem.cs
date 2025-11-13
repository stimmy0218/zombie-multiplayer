using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomItem : MonoBehaviour
{
    public TMP_Text roomNameText; // 방 이름 + 인원 표시 텍스트
    public Button joinButton;   // 입장 버튼
    private RoomInfo info;   // 이 아이템이 나타내는 방 정보

    // 방 정보 UI 세팅
    public void Setup(RoomInfo info)
    {
        this.info = info;
        roomNameText.text = $"{info.Name} ({info.PlayerCount}/{info.MaxPlayers})";

        // 버튼 클릭 이벤트 재설정
        joinButton.onClick.RemoveAllListeners();
        joinButton.onClick.AddListener(OnClickJoin);
    }

    // 방 입장 버튼 눌렀을 때
    void OnClickJoin()
    {
        Debug.Log($"방 입장 시도: {info.Name}");
        PhotonNetwork.JoinRoom(info.Name);
    }
}