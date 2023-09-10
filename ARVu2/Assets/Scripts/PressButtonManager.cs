using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressButtonManager : MonoBehaviour
{
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

    public void ClickIntroVoiceButton(int i)
    {
        GameEvent.OnIntroVoiceStartOrClose.Invoke(i, true);
    }
}
