using TMPro;
using UnityEngine;

public class UIPlayerItem : MonoBehaviour
{
    public TMP_Text nicknameText;
    public TMP_Text isMasterClientText;

    public void Setup(string nickname, bool isMasterClient)
    {
        nicknameText.text = nickname;
        isMasterClientText.text = isMasterClient ? "방장" : "player";
    }
}
