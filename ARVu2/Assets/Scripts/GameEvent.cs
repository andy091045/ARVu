using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public delegate void OccurTrackImageTarget(int i, bool open);
    public static OccurTrackImageTarget OccurTrackImageTargetChange;

    public void TransferGetTargetEventMessage(int i)
    {
        Debug.Log("圖片ID:" + i + "開啟");
        OccurTrackImageTargetChange.Invoke(i, true);
    }

    public void TransferLoseTargetEventMessage(int i)
    {
        Debug.Log("圖片ID:" + i + "遺失");
        OccurTrackImageTargetChange.Invoke(i, false);
    }
}
