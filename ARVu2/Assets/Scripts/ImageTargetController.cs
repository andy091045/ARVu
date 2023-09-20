using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;
using static UnityEngine.GraphicsBuffer;

public class ImageTargetController : MonoBehaviour
{
    DataManager dataManager_;
    private void Awake()
    {
        dataManager_ = GameContainer.Get<DataManager>();
        GameEvent.OnGetAllExhibitName += spawnAllImageTarget;
    }

    void spawnAllImageTarget()
    {
        var itBehaviour = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget("Vuforia/ARImage.xml", "SX-70Model");
        DefaultObserverEventHandler observerHandler = itBehaviour.AddComponent<DefaultObserverEventHandler>();
        observerHandler.StatusFilter = DefaultObserverEventHandler.TrackingStatusFilter.Tracked;
        observerHandler.OnTargetFound = new UnityEvent();
        observerHandler.OnTargetLost = new UnityEvent();
        observerHandler.OnTargetFound.AddListener(() => TransferGetTargetEventMessage(1));
        observerHandler.OnTargetLost.AddListener(() => TransferLoseTargetEventMessage(1));
        itBehaviour.transform.parent = this.transform;

        //foreach (var target in dataManager_.AllExhibitData)
        //{
        //    var itBehaviour = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget("Vuforia/ARImage.xml", target.ExhibitName);
        //    DefaultObserverEventHandler observerHandler = itBehaviour.AddComponent<DefaultObserverEventHandler>();
        //    observerHandler.StatusFilter = DefaultObserverEventHandler.TrackingStatusFilter.Tracked;
        //    observerHandler.OnTargetFound = new UnityEvent();
        //    observerHandler.OnTargetLost = new UnityEvent();
        //    observerHandler.OnTargetFound.AddListener(() => TransferGetTargetEventMessage(target.ExhibitName));
        //    observerHandler.OnTargetLost.AddListener(() => TransferLoseTargetEventMessage(target.ExhibitName));
        //    itBehaviour.transform.parent = this.transform;
        //}
    }

    private void OnDestroy()
    {
        GameEvent.OnGetAllExhibitName -= spawnAllImageTarget;
    }

    public void TransferGetTargetEventMessage(int i)
    {
        Debug.Log("圖片ID:" + i + "開啟!!!!!!!!!!!!");
        GameEvent.OccurTrackImageTargetChange.Invoke(i, true);
    }

    public void TransferLoseTargetEventMessage(int i)
    {
        Debug.Log("圖片ID:" + i + "遺失!!!!!!!!");
        GameEvent.OccurTrackImageTargetChange.Invoke(i, false);
        GameEvent.OnIntroVoiceStartOrClose.Invoke(i, false);
    }
}
