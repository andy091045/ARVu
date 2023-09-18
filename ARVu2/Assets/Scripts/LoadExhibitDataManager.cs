using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;

[Serializable]
public class FileList
{
    public string[] files;
}

public class LoadExhibitDataManager : MonoBehaviour
{
    FileList dataList_;

    //選取的任務數
    int missionCount_ = 5;

    // Firebase Storage 參考
    private Firebase.Storage.StorageReference fileRef_;

    private void Start()
    {
        // 初始化 Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseStorage storage = FirebaseStorage.DefaultInstance;

            // 设置 Firebase Storage 参考到 JSON 文件
            string jsonFilePath = "data.json"; // 替换为您的 JSON 文件路径
            fileRef_ = storage.RootReference.Child(jsonFilePath);

            // 下载并解析 JSON 文件
            DownloadAndParseJSON();
        });        
    }

    void DownloadAndParseJSON()
    {
        // 下载 JSON 文件的下载 URL
        fileRef_.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
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

    public void ChangeAndDownloadExhibitData(string newFilePath)
    {
        // 更新 Firebase Storage 引用
        fileRef_ = FirebaseStorage.DefaultInstance.RootReference.Child(newFilePath);

        // 下载展品資料    
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
            dataList_ = JsonUtility.FromJson<FileList>(jsonText);

            if(dataList_ != null)
            {
                //從中隨機選取五個展品
                for (int i = 0; i < dataList_.files.Length; i++)
                {
                    Debug.Log(dataList_.files[i]);
                }
            }
        }
    }
}
