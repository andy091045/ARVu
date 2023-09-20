using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PressButtonManager : MonoBehaviour
{
    public GameObject MissionHintObject;

    DataManager data_;
    private void Awake()
    {
        data_ = GameContainer.Get<DataManager>();
    }

    public void ChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void OpenSetActiveFalseObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void CloseSetActiveTrueObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ClickIntroVoiceButton(string name)
    {
        GameEvent.OnIntroVoiceStartOrClose.Invoke(name, true);
    }

    public void OpenMissionHintObject(int i)
    {
        Text text = MissionHintObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        text.text = data_.MissionExhibit[i].TreatureHint;
        MissionHintObject.SetActive(true);
    }
}
