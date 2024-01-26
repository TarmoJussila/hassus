using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI messageText;
    
    public void Init(ChatMessageData chatMessageData)
    {
        messageText.text = $"<color={ChatUserNameColors.UsernameColors[Random.Range(0, ChatUserNameColors.UsernameColors.Length)]}>{chatMessageData.UserName}</color>: {chatMessageData.Message}";
        gameObject.SetActive(true);
    }
}
