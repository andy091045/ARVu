using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseModel : MonoBehaviour
{
    public void OnChooseModel(int i)
    {
        SceneManager.LoadScene(i);
    }
}
