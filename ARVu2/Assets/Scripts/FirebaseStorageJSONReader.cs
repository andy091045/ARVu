using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;

//[Serializable]
//public class FileList
//{
//    public string[] files;
//}

public class FirebaseStorageJSONReader : MonoBehaviour
{
    // Firebase Storage 參考
    private Firebase.Storage.StorageReference jsonFileRef;

    private void Start()
    {
        // 初始化 Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseStorage storage = FirebaseStorage.DefaultInstance;

            // 设置 Firebase Storage 参考到 JSON 文件
            string jsonFilePath = "data.json"; // 替换为您的 JSON 文件路径
            jsonFileRef = storage.RootReference.Child(jsonFilePath);

            // 下载并解析 JSON 文件
            DownloadAndParseJSON();
        });
    }

    void DownloadAndParseJSON()
    {
        // 下载 JSON 文件的下载 URL
        jsonFileRef.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                string jsonUrl = task.Result.ToString();
                StartCoroutine(ParseJSON(jsonUrl));
            }
            else
            {
                Debug.LogError("Failed to get download URL for JSON file.");
            }
        });
    }

    IEnumerator ParseJSON(string jsonUrl)
    {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(jsonUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download JSON file: " + www.error);
                yield break;
            }

            string jsonText = www.downloadHandler.text;

            // 解析 JSON 文本为 FileList 对象
            FileList fileList = JsonUtility.FromJson<FileList>(jsonText);

            // 打印 JSON 内容
            Debug.Log("JSON Content: " + jsonText);

            // 输出 "files" 数组中的项数
            int numberOfFiles = fileList.files.Length;
            Debug.Log("Number of files: " + numberOfFiles);

            //輸出個別檔案名稱
            for(int i = 0; i < numberOfFiles; i++)
            {
                Debug.Log("file Name: " + fileList.files[i]);
            }
        }
    }
}
