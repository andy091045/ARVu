using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        GameEvent.OccurTrackImageTargetChange += OpenOrCloseObj;
    }

    public void OpenOrCloseObj(int i, bool open)
    {
        var obj = GameObject.Find("ExhibitsIntro_" + i.ToString());
        if(obj != null)
        {
            Debug.Log("����������UI");
            if (open)
            {
                obj.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                foreach (Transform item in obj.gameObject.transform.GetChild(0).gameObject.transform)
                {
                    item.gameObject.SetActive(true);
                }
            }
            else
            {
                obj.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("�䤣���������UI");
        } 
    }

    private void OnDestroy()
    {
        GameEvent.OccurTrackImageTargetChange -= OpenOrCloseObj;
    }
}
