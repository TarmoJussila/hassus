using System;
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
    private const float HYPE_CHAT_MESSAGE_GENERATION_INTERVAL = 0.1f;
    private const float HYPE_CHAT_MESSAGE_GENERATION_INTERVAL_VARIANCE = 0.05f;
    private const float HYPE_CHAT_MESSAGE_GENERATION_COUNT = 40;

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

    private void OnEnable()
    {
        StartCoroutine(NormalMessageLoop());
        PlayerHealth.OnPlayerDead += GenerateHypeMessages;
        //GenerateHypeMessages(default, null);
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDead -= GenerateHypeMessages;
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

    private void GenerateHypeMessages(int index, GameObject gameObject)
    {
        IEnumerator HypeMessageLoop()
        {
            for (int i = 0; i < HYPE_CHAT_MESSAGE_GENERATION_COUNT; i++)
            {
                yield return new WaitForSeconds(Random.Range(HYPE_CHAT_MESSAGE_GENERATION_INTERVAL - HYPE_CHAT_MESSAGE_GENERATION_INTERVAL_VARIANCE, HYPE_CHAT_MESSAGE_GENERATION_INTERVAL + HYPE_CHAT_MESSAGE_GENERATION_INTERVAL_VARIANCE));
                ChatMessageData data = new ChatMessageData
                {
                    UserName = ChatUserNames.Usernames[Random.Range(0, ChatUserNames.Usernames.Length)],
                    Message = ChatMessages.HypeMessages[Random.Range(0, ChatMessages.HypeMessages.Length)]
                };
                
                InstantiateMessage(data);
            }
        }

        StartCoroutine(HypeMessageLoop());
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
