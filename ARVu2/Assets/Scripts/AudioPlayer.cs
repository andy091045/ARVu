using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System.Collections;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    // Firebase Storage �Ѧ�
    private Firebase.Storage.StorageReference audioRef;

    void Start()
    {
        // ��l�� Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            FirebaseStorage storage = FirebaseStorage.DefaultInstance;

            // �]�m Firebase Storage �ѦҡA�ϥ� Firebase Storage ���|
            string audioPath = "ExhibitData/Untitled(Charlie+Tom,L.A.)June1986/Untitled(Charlie+Tom,L.A.)June1986.wav"; // ���|���ӬO Firebase Storage �������T���|
            audioRef = storage.RootReference.Child(audioPath);

            // �U���ü��񭵰T
            DownloadAndPlayAudio();
        });
    }

    void DownloadAndPlayAudio()
    {
        // �U�����T�ɮת��U�� URL
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
                // ���w AudioSource ���񭵰T
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
