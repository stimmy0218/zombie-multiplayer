using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINicknameView : MonoBehaviour
{
    public TMP_InputField nicknameInputField; // 닉네임 입력창
    public Button submitButton;               // 확인 버튼
    public Action<string> onClickSubmit;  // 닉네임 제출 콜백

    void Start()
    {
        // 버튼 클릭 시 입력된 닉네임을 콜백으로 전달
        submitButton.onClick.AddListener(() =>
        {
            Debug.Log(nicknameInputField.text);
            onClickSubmit(nicknameInputField.text);
        });
    }
}