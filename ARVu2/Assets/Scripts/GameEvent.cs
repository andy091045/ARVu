using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public delegate void OccurTrackImageTarget(string name, bool IsOpen);
    public static OccurTrackImageTarget OccurTrackImageTargetChange;

    public delegate void OccurIntroVoiceStartOrClose(string name, bool IsOpen);
    public static OccurIntroVoiceStartOrClose OnIntroVoiceStartOrClose;

    public delegate void OccurMissionComplete(int i);
    public static OccurMissionComplete OnMissionComplete;

    //當讀取到所有的展品名稱後，生成ImageTarget
    public delegate void OccurGetAllExhibitName();
    public static OccurGetAllExhibitName OnGetAllExhibitName;

    public void TransferGetTargetEventMessage(string name)
    {
        Debug.Log("圖片:" + name + "開啟");
        OccurTrackImageTargetChange.Invoke(name, true);
    }

    public void TransferLoseTargetEventMessage(string name)
    {
        Debug.Log("圖片:" + name + "遺失");
        OccurTrackImageTargetChange.Invoke(name, false);
        OnIntroVoiceStartOrClose.Invoke(name, false);
    }

    public void MissionComplete(int i)
    {
        OnMissionComplete.Invoke(i);
    }
}
