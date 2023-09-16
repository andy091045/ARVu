using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System.Collections;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    // Firebase Storage 參考
    private Firebase.Storage.StorageReference audioRef;

    void Start()
    {
        // 初始化 Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseStorage storage = FirebaseStorage.DefaultInstance;

            // 設置 Firebase Storage 參考，使用 Firebase Storage 路徑
            string audioPath = "ExhibitData/Untitled(Charlie+Tom,L.A.)June1986/Untitled(Charlie+Tom,L.A.)June1986.wav"; // 路徑應該是 Firebase Storage 中的正確路徑
            audioRef = storage.RootReference.Child(audioPath);

            // 下載並播放音訊
            DownloadAndPlayAudio();
        });
    }

    void DownloadAndPlayAudio()
    {
        // 下載音訊檔案的下載 URL
        audioRef.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                string audioUrl = task.Result.ToString();
                StartCoroutine(PlayAudio(audioUrl));
            }
            else
            {
                Debug.LogError("Failed to get download URL for audio file.");
            }
        });
    }

    IEnumerator PlayAudio(string audioUrl)
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
                // 指定 AudioSource 播放音訊
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("Failed to create AudioClip from downloaded audio data.");
            }
        }
    }
}
