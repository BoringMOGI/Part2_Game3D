using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 포톤 서버용 네임스페이스.
using Photon.Pun;                       // 서버 접속용.
using Photon.Chat;                      // 채널, 채팅용.
using ExitGames.Client.Photon;          // 클라이언트 생성용.


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
        // 이벤트 등록. (Listener : 이벤트 등록자)
        inputField.onEndEdit.AddListener(OnEndEdit);

        OnStart();
    }
    private void Update()
    {
        // 입력 필드를 포커싱하고 있는지?
        IsChatting = inputField.isFocused;
        if (Input.GetKeyDown(KeyCode.Return) && !IsChatting)
        {
            inputField.Select();
        }

        OnUpdate();
    }

    private void OnEndEdit(string str)
    {
        // 입력된 문자가 비어있으면 전송하지 않는다.
        if (string.IsNullOrEmpty(str))
            return;
        
        // 클라이언트가 존재하고 현재 채팅 채널에 접속한 상태일 경우.
        if (client != null && client.State == ChatState.ConnectedToFrontEnd)
        {
            // 나의 채팅 입력이 끝났을 때 서버로 입력된 메시지 전달.
            client.PublishMessage(channelName, str);
        }

        EventSystem.current.SetSelectedGameObject(null, null);
    }
    private void OnAddText(string str)
    {
        // 문자열을 입력했는데 그것이 비어있을 경우.
        if (string.IsNullOrEmpty(str))
            return;

        // 최대 채팅 개수를 넘었다면 가장 최근의 기록을 삭제.
        if (chatRecord.Count >= maxChatCount)
            chatRecord.RemoveAt(0);

        // 채팅 기록에 대화 내용 추가.
        chatRecord.Add(str);
        inputField.text = string.Empty;

        UpdateChat();
    }
    private void UpdateChat()
    {
        // 문자열 조합
        // 문자 배열의 값들을 기준 문자 값으로 조합.
        // "AA\nBB\nCC\nDD\nEE"
        chatText.text = string.Join("\n", chatRecord);

        Rect textRect = chatText.rectTransform.rect;            // 채팅 창의 Rect 정보.
        float textHeight = chatText.GetHeight();                // 채팅 창의 높이 계산.
        //float offsetHeight = textRect.yMin + textRect.yMax;     // 채팅 창의 높이 간격.

        // 계산된 높이 적용.
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, textHeight + 20);
    }
   
}
public partial class Chatting
{
    [Header("Chatting")]
    [SerializeField] string userName;       // 나의 이름.
    [SerializeField] string channelName;    // 내가 들어갈 채널 이름.

    ChatClient client;                      // 채팅 클라이언트 (나)

    void OnStart()
    {
        // 백그라운드 상태가 되면 서버 연결이 끊어진다. (기본은 일시 정지)
        // 때문에 백그라운드 상태에서도 게임이 돌아가도록 처리해야한다.
        //Application.runInBackground = true;

        client = new ChatClient(this);                  // 구현한 인터페이스를 이용해 클라이언트 객체 생성.
        //client.UseBackgroundWorkerForSending = true;    // 클라이언트가 백그라운드 상태에서도 정보가 오도록 허용.


        string chatId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat;       // 세팅된 채팅용 AppID.
        string appVersion = PhotonNetwork.AppVersion;                                   // 현재 포톤의 앱 버전.
        AuthenticationValues authValues = new AuthenticationValues(userName);           // 나의 인증서 생성.

        client.Connect(chatId, appVersion, authValues);                                 // 서버에 접속.
        OnAddText("서버에 접속중...");
    }
    void OnUpdate()
    {
        if(client != null)
            client.Service();           // 클라이언트의 교신.
    }

    // 서버에서 내려오는 디버깅 메세지를 레벨에 따라 분류.
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

    // 서버와 접속이 끊어졌을 때.
    public void OnDisconnected()
    {
        OnAddText("서버와 접속이 끊어졌습니다.");
    }

    // 서버와 접속이 성공했을 때.
    public void OnConnected()
    {
        //OnEndEdit("서버 접속 성공!");
        OnAddText(string.Format("'{0}' 채널 입장 중...", channelName));
        client.Subscribe(new string[] { channelName }, 0);
    }

    // 채팅의 상태가 변경되었을 때.
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

    // 채널 입장.
    public void OnSubscribed(string[] channels, bool[] results)
    {
        OnAddText("채널 입장 성공!");
    }

    // 채널에서 나감.
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
        // Text UI에 입력된 글자의 총 높이.
        TextGenerator textGen = new TextGenerator();
        TextGenerationSettings generationSettings = text.GetGenerationSettings(text.rectTransform.rect.size);
        float height = textGen.GetPreferredHeight(text.text, generationSettings);

        return height;
    }
}

