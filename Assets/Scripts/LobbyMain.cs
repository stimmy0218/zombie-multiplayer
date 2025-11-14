using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMain : MonoBehaviour
{
    public UINicknameView nicknameView;
    public UILoading uiLoading;
    public UIRoomScrollview uiRoomScrollview;
    public Button createRoomButton;
    public Button leaveRoombutton;
    public TMP_InputField roomNameInput;
    public GameObject uiCreateRoom;
    
    void Start()
    {
        Debug.Log(PhotonNetwork.LocalPlayer);
        uiRoomScrollview.Init();   // 방 리스트 UI 초기화
        AddEventListeners();       // 포톤 관련 이벤트 구독
        ConnectToMasterServer();   // 마스터 서버 접속 시작

        // 방 생성 버튼
        createRoomButton.onClick.AddListener(OnClickCreateRoom);
        
        // cash 방 생성 버튼
        // createRoomButton.onClick.AddListener(() =>
        // {
        //     Pun2Manager.instance.CreateRoom();
        // });

        // 방 나가기 버튼
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

                // 닉네임 설정 후 방 리스트 UI 보여주기
                nicknameView.gameObject.SetActive(false);
                uiRoomScrollview.Show();
                uiCreateRoom.gameObject.SetActive(true);
            }
        };
    }

    // 방 생성 버튼 클릭 시 실행될 메서드
    private void OnClickCreateRoom()
    {
        // 인풋필드에서 방 이름 읽기
        string roomName = roomNameInput != null ? roomNameInput.text : string.Empty;
        
        // 미입력 막기
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("방 이름 입력");
            return;
        }
        Pun2Manager.instance.CreateRoom(roomName);
    }
    
    // 포톤 마스터 서버 연결 요청
    private void ConnectToMasterServer()
    {
        uiLoading.Show();               // 로딩 UI 켜기
        Pun2Manager.instance.Init();   // 마스터 서버 접속
    }

    // 포톤 이벤트 → UI 반영
    private void AddEventListeners()
    {
        // 마스터 서버 접속 완료
        EventDispatcher.instance.AddEventHandler(
            (int)EventEnums.EventType.OnConnectedToMaster,
            (short eventType) =>
            {
                Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");
                if (uiLoading != null) 
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
                if (uiLoading != null) 
                    uiLoading.Hide();

                // 이미 닉네임이 있으면 방 리스트 바로 보여줌
                if (!string.IsNullOrEmpty(PhotonNetwork.NickName))
                {
                    uiRoomScrollview.Show();
                    createRoomButton.gameObject.SetActive(true);
                    leaveRoombutton.gameObject.SetActive(false);
                }
            });
        
        //-----------------------------------
        // 방 입장 완료 이벤트 구독 
        EventDispatcher.instance.AddEventHandler(
            (int)EventEnums.EventType.OnJoinedRoom,
            OnJoinedRoomEvent);   // 메서드 그룹으로 전달
        //-----------------------------------
    }

    // 방 입장 완료
        private void OnJoinedRoomEvent(short eventType)
        {
             Debug.Log($"OnJoinedRoomEvent: {(EventEnums.EventType)eventType}");
             // EventDispatcher.instance.AddEventHandler(
             // (int)EventEnums.EventType.OnJoinedRoom,
             // (short eventType) =>
             {
                 //Debug.Log($"AddEventListeners: {(EventEnums.EventType)eventType}");
                 
                 // 방 안에 있을 때는 나가기 버튼만 보이게
                 leaveRoombutton.gameObject.SetActive(true);
                 uiRoomScrollview.Hide();
                 createRoomButton.gameObject.SetActive(false);

                 // RoomMain roomMain = GameManager.FindFirstObjectByType<RoomMain>();
                 // Debug.Log(roomMain);
                 // Debug.Log(SceneManager.GetActiveScene().name);
                 
                 //-----------------------------
                 // Room 씬으로 이동
                 SceneManager.LoadScene("Room");
                 //-----------------------------
             }
             //});
        }

        void OnDestroy()
        {
            if (EventDispatcher.instance != null)
            {
                
            }
        }
}