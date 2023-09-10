using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class IntroVoiceManager : MonoBehaviour
{
    AudioSource audioSource_;

    private void Awake()
    {
        audioSource_ = transform.GetComponent<AudioSource>();
        GameEvent.OnIntroVoiceStartOrClose += SetSE;
    }

    async void SetSE(int i, bool isOpen)
    {
        if(isOpen)
        {
            Debug.Log("¸ü¤J»y­µ");
            AudioClip clip = await Addressables.LoadAssetAsync<AudioClip>("Assets/Music/IntroVoiceDemo.wav").Task;
            audioSource_.clip = clip;
            audioSource_.Play();
        }
        else
        {
            audioSource_.Stop();
        }    
    }
}
