using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChatMessageGenerator : MonoBehaviour
{ 
    [Header("References")]
    [SerializeField] private ChatMessage chatMessagePrefab;
    [SerializeField] private RectTransform chatMessageContainer;
    [SerializeField] private ScrollRect scrollRect;

    private Stack<ChatMessage> chatMessagePool;
    private const int CHAT_MESSAGE_POOL_COUNT = 51;
    private const float CHAT_MESSAGE_GENERATION_INTERVAL = 1f;
    private const float CHAT_MESSAGE_GENERATION_INTERVAL_VARIANCE = 0.8f;

    private void Awake()
    {
        chatMessagePool = new Stack<ChatMessage>();
        for (int i = 0; i < CHAT_MESSAGE_POOL_COUNT; i++)
        {
            ChatMessage chatMessage = Instantiate(chatMessagePrefab, transform);
            chatMessage.gameObject.SetActive(false);
            chatMessagePool.Push(chatMessage);
        }
    }

    private void Start()
    {
        StartCoroutine(NormalMessageLoop());
    }

    private void GeneratorRandomMessage()
    {
        ChatMessageData data = new ChatMessageData
        {
            UserName = ChatUserNames.Usernames[Random.Range(0, ChatUserNames.Usernames.Length)],
            Message = ChatMessages.Messages[Random.Range(0, ChatMessages.Messages.Length)]
        };
       
        InstantiateMessage(data);
    }

    private void InstantiateMessage(ChatMessageData data)
    {
        ChatMessage chatMessage = chatMessagePool.Pop();
        chatMessage.transform.SetParent(chatMessageContainer, false);
        chatMessage.transform.SetAsLastSibling();
        chatMessage.Init(data);
        scrollRect.normalizedPosition = new Vector2(0, 0);
       
        if (chatMessageContainer.childCount > CHAT_MESSAGE_POOL_COUNT - 2)
        {
            Transform lastSibling = chatMessageContainer.GetChild(0);
            chatMessagePool.Push(lastSibling.GetComponent<ChatMessage>());
        }
    }
    
    private IEnumerator NormalMessageLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(CHAT_MESSAGE_GENERATION_INTERVAL - CHAT_MESSAGE_GENERATION_INTERVAL_VARIANCE, CHAT_MESSAGE_GENERATION_INTERVAL + CHAT_MESSAGE_GENERATION_INTERVAL_VARIANCE));
            GeneratorRandomMessage();
        }
    }
}
