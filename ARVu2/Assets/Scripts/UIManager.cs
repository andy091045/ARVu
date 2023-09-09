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
            Debug.Log("找到對應介紹UI");
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
            Debug.Log("找不到對應介紹UI");
        } 
    }

    private void OnDestroy()
    {
        GameEvent.OccurTrackImageTargetChange -= OpenOrCloseObj;
    }
}
