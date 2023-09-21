using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject MissionList;
    GameObject exhibitsIntro_;

    DataManager dataManager_;

    private void Awake()
    {
        dataManager_ = GameContainer.Get<DataManager>();
        GameEvent.OccurTrackImageTargetChange += OpenOrCloseObj;
        GameEvent.OnMissionComplete += ChangeMissionText;
    }

    public void OpenOrCloseObj(string name, bool open)
    {
        exhibitsIntro_ = GameObject.Find("ExhibitsIntro");
        if(exhibitsIntro_ != null)
        {
            ExhibitData temp = new();
            Debug.Log("找到對應介紹UI");
            if (open)
            {
                exhibitsIntro_.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                foreach (Transform item in exhibitsIntro_.gameObject.transform.GetChild(0).gameObject.transform)
                {
                    item.gameObject.SetActive(true);
                }

                
                foreach (var item in dataManager_.AllExhibitData)
                {
                    if(item.ExhibitName == name)
                    {
                        if (item.IsDownload)
                        {
                            temp = item;                            
                        }
                        else
                        {
                            //傳遞下載資料事件
                            GameEvent.OnDownloadExhibitByName.Invoke(name);
                        }
                        break;
                    }
                }

                StartCoroutine(WaitForDownloadAndUpdateUI(name, 3));               
            }
            else
            {
                exhibitsIntro_.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("找不到對應介紹UI");
        } 
    }

    void ChangeMissionText(int i)
    {
        var obj = MissionList.transform.GetChild(i - 1).transform.gameObject;
        var text = obj.transform.GetChild(0).transform.gameObject.GetComponent<Text>();
        text.text = "任務" + i + "完成";
        Debug.Log(text);
    }

    private void OnDestroy()
    {
        GameEvent.OccurTrackImageTargetChange -= OpenOrCloseObj;
        GameEvent.OnMissionComplete -= ChangeMissionText;
    }

    private IEnumerator WaitForDownloadAndUpdateUI(string name, float second)
    {
        yield return new WaitForSeconds(second);

        // 在数据下载完成后执行其他操作
        ExhibitData temp = new();
        foreach (var item in dataManager_.AllExhibitData)
        {
            if (item.ExhibitName == name)
            {
                temp = item;
                break;
            }
        }
        Debug.Log("数据已下载完毕，可以执行其他操作" + temp.IntroTextCH);
        Text text = exhibitsIntro_.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        text.text = temp.IntroTextCH;

        //呼叫修改導覽語音事件
        GameEvent.OnUpdateIntroVoice.Invoke(name);
    }
}
