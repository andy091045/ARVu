using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public delegate void OccurTrackImageTarget(int i, bool IsOpen);
    public static OccurTrackImageTarget OccurTrackImageTargetChange;

    public delegate void OccurIntroVoiceStartOrClose(int i, bool IsOpen);
    public static OccurIntroVoiceStartOrClose OnIntroVoiceStartOrClose;

    public void TransferGetTargetEventMessage(int i)
    {
        Debug.Log("�Ϥ�ID:" + i + "�}��");
        OccurTrackImageTargetChange.Invoke(i, true);
    }

    public void TransferLoseTargetEventMessage(int i)
    {
        Debug.Log("�Ϥ�ID:" + i + "��");
        OccurTrackImageTargetChange.Invoke(i, false);
        OnIntroVoiceStartOrClose.Invoke(i, false);
    }
}
