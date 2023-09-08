using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressButton : MonoBehaviour
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
}
