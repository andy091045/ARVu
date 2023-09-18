﻿using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;

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
                //從中隨機選取五個展品名稱
                if(dataList_.files.Length >= missionCount_)
                {
                    List<int> randomNumbers = RandomNumberGenerator.GenerateRandomNumbers(missionCount_, 0, dataList_.files.Length-1);
                    for (int i = 0; i < randomNumbers.Count; i++)
                    {                        
                        Debug.Log(dataList_.files[randomNumbers[i]]);
                    }
                }
            }
        }
    }
}

public class RandomNumberGenerator
{
    public static List<int> GenerateRandomNumbers(int count, int minValue, int maxValue)
    {
        if (count > (maxValue - minValue + 1) || minValue > maxValue)
        {
            throw new ArgumentException("Invalid arguments");
        }

        List<int> numbers = new List<int>();
        System.Random random = new System.Random();

        while (numbers.Count < count)
        {
            int randomNumber = random.Next(minValue, maxValue + 1);

            if (!numbers.Contains(randomNumber))
            {
                numbers.Add(randomNumber);
            }
        }

        return numbers;
    }
}
