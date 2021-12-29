using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Chatting : MonoBehaviour
{
    public static bool IsChatting { get; private set; }

    [SerializeField] InputField inputField;
    [SerializeField] Text chatText;
    [SerializeField] string nickname;

    List<string> chatRecord = new List<string>();

    private void Start()
    {
        // �̺�Ʈ ���. (Listener : �̺�Ʈ �����)
        inputField.onValueChanged.AddListener(OnValueChanged);
        inputField.onEndEdit.AddListener(OnEndEdit);
    }
    void Update()
    {
        // �Է� �ʵ带 ��Ŀ���ϰ� �ִ���?
        IsChatting = inputField.isFocused;
        if (Input.GetKeyDown(KeyCode.Return) && !IsChatting)
        {
            inputField.Select();
        }
    }

    private void OnValueChanged(string str)
    {
        
    }
    private void OnEndEdit(string str)
    {
        chatRecord.Add(string.Format("{0} : {1}", nickname, str));
        inputField.text = string.Empty;

        UpdateChat();
    }

    private void UpdateChat()
    {
        string content = string.Empty;
        for(int i = 0; i<chatRecord.Count; i++)
        {
            content += chatRecord[i];
            content += "\n";
        }

        chatText.text = content;
    }
}
