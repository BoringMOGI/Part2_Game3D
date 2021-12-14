using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    private static string GetPath(string fileName)
    {
        return string.Concat(Application.dataPath, "/", fileName);       // 경로.
    }

    public static void SaveToJson<T>(T data, string fileName)
    {
        string json = JsonUtility.ToJson(data, true);               // T 데이터를 Json으로 변환한다.
        Debug.Log("SAVE : " + json);

        json = CryptManager.Encrypt(json);                          // json데이터를 암호화.

        StreamWriter writer = new StreamWriter(GetPath(fileName));  // StreamWriter로 해당 경로에 파일 생성.
        writer.Write(json);                                         // 파일 쓰기.
        writer.Close();                                             // StreamWriter 종료. (중요)
    }
    public static T LoadFromJson<T>(string fileName)
    {
        StreamReader reader = new StreamReader(GetPath(fileName));  // StreamReader로 해당 경로의 파일을 연다.
        string json = reader.ReadToEnd();                           // 파일을 끝까지 읽는다.
        json = CryptManager.Decrypt(json);                          // 암호화된 데이터를 복호화(원래대로 되돌린다).

        Debug.Log("LOAD : " + json);

        return JsonUtility.FromJson<T>(json);                       // 읽은 데이터를 T 자료형으로 변환한다.
    }




    

}
