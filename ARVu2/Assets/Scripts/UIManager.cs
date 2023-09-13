using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject MissionList;

    private void Awake()
    {
        GameEvent.OccurTrackImageTargetChange += OpenOrCloseObj;
        GameEvent.OnMissionComplete += ChangeMissionText;
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

    void ChangeMissionText(int i)
    {
        var obj = MissionList.transform.GetChild(i - 1).transform.gameObject;
        var text = obj.transform.GetChild(0).transform.gameObject.GetComponent<Text>();
        text.text = "����" + i + "����";
        Debug.Log(text);
    }

    private void OnDestroy()
    {
        GameEvent.OccurTrackImageTargetChange -= OpenOrCloseObj;
        GameEvent.OnMissionComplete -= ChangeMissionText;
    }
}
