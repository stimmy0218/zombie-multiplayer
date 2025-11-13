using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINicknameView : MonoBehaviour
{
    public TMP_InputField nicknameInputField;
    public Button submitButton;
    public System.Action<string> onClickSubmit;

    void Start()
    {
        submitButton.onClick.AddListener(() =>
        {
            Debug.Log(nicknameInputField.text);
            onClickSubmit(nicknameInputField.text);
        });
    }
}