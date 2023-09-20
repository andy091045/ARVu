using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]
public class FileList
{
    public string[] files;
}

public class LoadExhibitDataManager : MonoBehaviour
{
    //所有展品的資料名稱
    FileList dataList_;
    DataManager data_;

    //選取的任務數
    int missionCount_ = 5;

    // Firebase Storage 參考
    private Firebase.Storage.StorageReference fileRef_;

    private void Awake()
    {
        data_ = GameContainer.Get<DataManager>();
        GameEvent.OnDownloadExhibitByName += DownloadOneDataByName;
    }

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

    public void ChangeAndDownloadExhibitHint(string newFileName, int number)
    {
        ExhibitData newData = new();
        newData.ExhibitName = newFileName;
        data_.MissionExhibit.Add(newData);
        // 下载展品導覽語音
        fileRef_ = FirebaseStorage.DefaultInstance.RootReference.Child("ExhibitData/" + newFileName + "/" + newFileName + "_Hint.txt");
        fileRef_.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                string fileUrl = task.Result.ToString();
                StartCoroutine(DownloadAndParseHintTextFile(fileUrl, number));
            }
            else
            {
                Debug.LogError("Failed to get download URL for audio file.");
            }
        });

    }

    public void DownloadOneDataByName(string name)
    {
        ExhibitData temp = new();
        //搜尋展品
        foreach (var item in data_.AllExhibitData)
        {
            if (item.ExhibitName == name)
            {
                temp = item;
                break;
            }
        }

        temp.IsTreasureFound = false;

        // 下载展品導覽語音
        fileRef_ = FirebaseStorage.DefaultInstance.RootReference.Child("ExhibitData/" + name + "/" + name + ".wav");
        fileRef_.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                string fileUrl = task.Result.ToString();
                StartCoroutine(DownloadAudioClip(fileUrl, temp));
            }
            else
            {
                Debug.LogError("Failed to get download URL for audio file.");
            }
        });

        // 下载展品導覽中文文字說明
        fileRef_ = FirebaseStorage.DefaultInstance.RootReference.Child("ExhibitData/" + name + "/" + name + "_CH.txt");
        fileRef_.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                string fileUrl = task.Result.ToString();
                StartCoroutine(DownloadAndParseIntroTextFile(fileUrl, temp.IntroTextCH));
            }
            else
            {
                Debug.LogError("Failed to get download URL for audio file.");
            }
        });

        // 下载展品導覽英文文字說明
        fileRef_ = FirebaseStorage.DefaultInstance.RootReference.Child("ExhibitData/" + name + "/" + name + "_EN.txt");
        fileRef_.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                string fileUrl = task.Result.ToString();
                StartCoroutine(DownloadAndParseIntroTextFile(fileUrl, temp.IntroTextEN));
            }
            else
            {
                Debug.LogError("Failed to get download URL for audio file.");
            }
        });
        temp.IsDownload = true;
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

            for (int i = 0; i < dataList_.files.Length; i++)
            {
                ExhibitData item = new();
                item.ExhibitName = dataList_.files[i];
                item.IsDownload = false;
                data_.AllExhibitData.Add(item);
            }
            GameEvent.OnGetAllExhibitName.Invoke();

            if (dataList_ != null)
            {
                //從中隨機選取五個展品名稱
                if(dataList_.files.Length >= missionCount_)
                {
                    List<int> randomNumbers = RandomNumberGenerator.GenerateRandomNumbers(missionCount_, 0, dataList_.files.Length-1);
                    for (int i = 0; i < randomNumbers.Count; i++)
                    {                                                
                        //data_.MissionExhibit.Add(dataList_.files[randomNumbers[i]]);
                        //Debug.Log(data_.MissionExhibit[i] + "哈哈");
                        //ChangeAndDownloadExhibitData("ExhibitData/" + dataList_.files[randomNumbers[i]] + "/" + dataList_.files[randomNumbers[i]]);
                        ChangeAndDownloadExhibitHint(dataList_.files[randomNumbers[i]], i);
                    }
                }
            }
        }
    }

    IEnumerator DownloadAudioClip(string audioUrl, ExhibitData newData)
    {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip(audioUrl, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download audio file: " + www.error);
                yield break;
            }

            AudioClip audioClip = UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(www);

            if (audioClip != null)
            {
               newData.IntroVoice = audioClip;
                Debug.LogWarning(newData.ExhibitName + "語音下載成功");
            }
            else
            {
                Debug.LogError("Failed to create AudioClip from downloaded audio data.");
            }
        }
    }

    IEnumerator DownloadAndParseHintTextFile(string textUrl, int number)
    {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(textUrl))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError && !www.isHttpError)
            {
                string textContent = www.downloadHandler.text;

                // 处理文本内容                
                data_.MissionExhibit[number].TreatureHint = textContent;
                // 在这里可以将文本内容传递给其他函数或进行其他处理
                // 例如：ProcessTextContent(textContent);
            }
            else
            {
                Debug.LogError("Failed to download text file: " + www.error);
            }
        }
    }

    IEnumerator DownloadAndParseIntroTextFile(string textUrl, string content)
    {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(textUrl))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError && !www.isHttpError)
            {
                string textContent = www.downloadHandler.text;          
                content = textContent;
                Debug.LogWarning("導覽內容: " + content);
            }
            else
            {
                Debug.LogError("Failed to download text file: " + www.error);
            }
        }
    }

    private void OnDestroy()
    {
        GameEvent.OnDownloadExhibitByName -= DownloadOneDataByName;
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
