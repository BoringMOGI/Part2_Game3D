using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// ���� ������ ���ӽ����̽�.
using Photon.Pun;                       // ���� ���ӿ�.
using Photon.Chat;                      // ä��, ä�ÿ�.
using ExitGames.Client.Photon;          // Ŭ���̾�Ʈ ������.


public partial class Chatting : MonoBehaviour, IChatClientListener
{
    public static bool IsChatting { get; private set; }

    [SerializeField] RectTransform contentRect;
    [SerializeField] InputField inputField;
    [SerializeField] Text chatText;

    List<string> chatRecord = new List<string>();
    const int maxChatCount = 30;        

    private void Start()
    {
        // �̺�Ʈ ���. (Listener : �̺�Ʈ �����)
        inputField.onEndEdit.AddListener(OnEndEdit);

        OnStart();
    }
    private void Update()
    {
        // �Է� �ʵ带 ��Ŀ���ϰ� �ִ���?
        IsChatting = inputField.isFocused;
        if (Input.GetKeyDown(KeyCode.Return) && !IsChatting)
        {
            inputField.Select();
        }

        OnUpdate();
    }

    private void OnEndEdit(string str)
    {
        // �Էµ� ���ڰ� ��������� �������� �ʴ´�.
        if (string.IsNullOrEmpty(str))
            return;
        
        // Ŭ���̾�Ʈ�� �����ϰ� ���� ä�� ä�ο� ������ ������ ���.
        if (client != null && client.State == ChatState.ConnectedToFrontEnd)
        {
            // ���� ä�� �Է��� ������ �� ������ �Էµ� �޽��� ����.
            client.PublishMessage(channelName, str);
        }

        EventSystem.current.SetSelectedGameObject(null, null);
    }
    private void OnAddText(string str)
    {
        // ���ڿ��� �Է��ߴµ� �װ��� ������� ���.
        if (string.IsNullOrEmpty(str))
            return;

        // �ִ� ä�� ������ �Ѿ��ٸ� ���� �ֱ��� ����� ����.
        if (chatRecord.Count >= maxChatCount)
            chatRecord.RemoveAt(0);

        // ä�� ��Ͽ� ��ȭ ���� �߰�.
        chatRecord.Add(str);
        inputField.text = string.Empty;

        UpdateChat();
    }
    private void UpdateChat()
    {
        // ���ڿ� ����
        // ���� �迭�� ������ ���� ���� ������ ����.
        // "AA\nBB\nCC\nDD\nEE"
        chatText.text = string.Join("\n", chatRecord);

        Rect textRect = chatText.rectTransform.rect;            // ä�� â�� Rect ����.
        float textHeight = chatText.GetHeight();                // ä�� â�� ���� ���.
        //float offsetHeight = textRect.yMin + textRect.yMax;     // ä�� â�� ���� ����.

        // ���� ���� ����.
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, textHeight + 20);
    }
   
}
public partial class Chatting
{
    [Header("Chatting")]
    [SerializeField] string userName;       // ���� �̸�.
    [SerializeField] string channelName;    // ���� �� ä�� �̸�.

    ChatClient client;                      // ä�� Ŭ���̾�Ʈ (��)

    void OnStart()
    {
        // ��׶��� ���°� �Ǹ� ���� ������ ��������. (�⺻�� �Ͻ� ����)
        // ������ ��׶��� ���¿����� ������ ���ư����� ó���ؾ��Ѵ�.
        //Application.runInBackground = true;

        client = new ChatClient(this);                  // ������ �������̽��� �̿��� Ŭ���̾�Ʈ ��ü ����.
        //client.UseBackgroundWorkerForSending = true;    // Ŭ���̾�Ʈ�� ��׶��� ���¿����� ������ ������ ���.


        string chatId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat;       // ���õ� ä�ÿ� AppID.
        string appVersion = PhotonNetwork.AppVersion;                                   // ���� ������ �� ����.
        AuthenticationValues authValues = new AuthenticationValues(userName);           // ���� ������ ����.

        client.Connect(chatId, appVersion, authValues);                                 // ������ ����.
        OnAddText("������ ������...");
    }
    void OnUpdate()
    {
        if(client != null)
            client.Service();           // Ŭ���̾�Ʈ�� ����.
    }

    // �������� �������� ����� �޼����� ������ ���� �з�.
    public void DebugReturn(DebugLevel level, string message)
    {
        switch(level)
        {
            case DebugLevel.ERROR:
                Debug.LogError(message);
                break;

            case DebugLevel.WARNING:
                Debug.LogWarning(message);
                break;

            default:
                Debug.Log(message);
                break;
        }
    }

    // ������ ������ �������� ��.
    public void OnDisconnected()
    {
        OnAddText("������ ������ ���������ϴ�.");
    }

    // ������ ������ �������� ��.
    public void OnConnected()
    {
        //OnEndEdit("���� ���� ����!");
        OnAddText(string.Format("'{0}' ä�� ���� ��...", channelName));
        client.Subscribe(new string[] { channelName }, 0);
    }

    // ä���� ���°� ����Ǿ��� ��.
    public void OnChatStateChange(ChatState state)
    {
        //Debug.Log("OnChatStateChange : " + state.ToString());
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (string.IsNullOrEmpty(channelName))
            return;

        for(int i = 0; i< senders.Length; i++)
        {
            string context = string.Concat(senders[i], " : ", messages[i]);
            OnAddText(context);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        
    }

    // ä�� ����.
    public void OnSubscribed(string[] channels, bool[] results)
    {
        OnAddText("ä�� ���� ����!");
    }

    // ä�ο��� ����.
    public void OnUnsubscribed(string[] channels)
    {
        
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }
    public void OnUserSubscribed(string channel, string user)
    {
        
    }
    public void OnUserUnsubscribed(string channel, string user)
    {
        
    }
}

public static class Method
{
    public static float GetHeight(this Text text)
    {
        // Text UI�� �Էµ� ������ �� ����.
        TextGenerator textGen = new TextGenerator();
        TextGenerationSettings generationSettings = text.GetGenerationSettings(text.rectTransform.rect.size);
        float height = textGen.GetPreferredHeight(text.text, generationSettings);

        return height;
    }
}

