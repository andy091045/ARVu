using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class IntroVoiceManager : MonoBehaviour
{
    AudioSource audioSource_;
    DataManager dataManager_;

    private void Awake()
    {
        dataManager_ = GameContainer.Get<DataManager>();
        audioSource_ = transform.GetComponent<AudioSource>();
        GameEvent.OnIntroVoiceStartOrClose += SetSE;
    }

    async void SetSE(string name, bool isOpen)
    {
        if(isOpen)
        {
            ExhibitData temp = new();
            foreach (var item in dataManager_.AllExhibitData)
            {
                if (item.ExhibitName == name)
                {
                    temp = item;
                    break;
                }
            }
            
            AudioClip clip = temp.IntroVoice;
            audioSource_.clip = clip;
            audioSource_.Play();
        }
        else
        {
            audioSource_.Stop();
        }    
    }

    private void OnDestroy()
    {
        GameEvent.OnIntroVoiceStartOrClose -= SetSE;
    }
}
