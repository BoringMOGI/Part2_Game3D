using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    private static string GetPath(string fileName)
    {
        return string.Concat(Application.dataPath, "/", fileName);       // ���.
    }

    public static void SaveToJson<T>(T data, string fileName)
    {
        string json = JsonUtility.ToJson(data, true);               // T �����͸� Json���� ��ȯ�Ѵ�.
        Debug.Log("SAVE : " + json);

        json = CryptManager.Encrypt(json);                          // json�����͸� ��ȣȭ.

        StreamWriter writer = new StreamWriter(GetPath(fileName));  // StreamWriter�� �ش� ��ο� ���� ����.
        writer.Write(json);                                         // ���� ����.
        writer.Close();                                             // StreamWriter ����. (�߿�)
    }
    public static T LoadFromJson<T>(string fileName)
    {
        StreamReader reader = new StreamReader(GetPath(fileName));  // StreamReader�� �ش� ����� ������ ����.
        string json = reader.ReadToEnd();                           // ������ ������ �д´�.
        json = CryptManager.Decrypt(json);                          // ��ȣȭ�� �����͸� ��ȣȭ(������� �ǵ�����).

        Debug.Log("LOAD : " + json);

        return JsonUtility.FromJson<T>(json);                       // ���� �����͸� T �ڷ������� ��ȯ�Ѵ�.
    }




    

}
