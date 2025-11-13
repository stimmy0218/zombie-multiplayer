using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMain : MonoBehaviour
{
    public UINicknameView nicknameView;
    public UILoading uiLoading;
    public UIRoomScrollview uiRoomScrollview;
    public Button createRoomButton;
    public Button leaveRoombutton;

    void Start()
    {
        uiRoomScrollview.Init();
        AddEventListeners();

        // 마스터 서버 접속 시작
        ConnectToMasterServer();

        createRoomButton.onClick.AddListener(() =>
        {
            Pun2Manager.instance.CreateRoom();
        });

        leaveRoombutton.onClick.AddListener(() =>
        {
            Pun2Manager.instance.LeaveRoom();
        });

        // 닉네임 입력 완료 시
        nicknameView.onClickSubmit = (nickname) =>
        {
            if (string.IsNullOrEmpty(nickname))
            {
                Debug.Log("nickname is empty");
            }
            else
            {
                Debug.Log($"nickname: {nickname}");
                Pun2Manager.instance.SetNickname(nickname);

                uiRoomScrollview.Show();
                createRoomButton.gameObject.SetActive(true);
                nicknameView.gameObject.SetActive(false);
            }
        };
    }

    private void ConnectToMasterServer()
    {
        uiLoading.Show();
        Pun2Manager.instance.Init();   // 마스터 서버 접속
    }

    private void AddEventListeners()
    {
        // 마스터 서버 접속 완료
        EventDispatcher.instance.AddEventHandler(
            (int)EventEnums.EventType.OnConnectedToMaster,
            (short eventType) =>
            {
                Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");
                uiLoading.Hide();

                // 닉네임 없으면 닉네임 입력창 켜기
                nicknameView.gameObject.SetActive(string.IsNullOrEmpty(PhotonNetwork.NickName));
            });

        // 로비 입장 완료
        EventDispatcher.instance.AddEventHandler(
            (int)EventEnums.EventType.OnJoinedLobby,
            (short eventType) =>
            {
                Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");
                uiLoading.Hide();

                if (!string.IsNullOrEmpty(PhotonNetwork.NickName))
                {
                    uiRoomScrollview.Show();
                    createRoomButton.gameObject.SetActive(true);
                    leaveRoombutton.gameObject.SetActive(false);
                }
            });

        // 방 입장 완료
        EventDispatcher.instance.AddEventHandler(
            (int)EventEnums.EventType.OnJoinedRoom,
            (short eventType) =>
            {
                Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");

                leaveRoombutton.gameObject.SetActive(true);

                uiRoomScrollview.Hide();
                createRoomButton.gameObject.SetActive(false);
            });
    }
}