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
        observerHandler.OnTargetFound.AddListener(() => TransferGetTargetEventMessage("SX-70Model"));
        observerHandler.OnTargetLost.AddListener(() => TransferLoseTargetEventMessage("SX-70Model"));
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

    public void TransferGetTargetEventMessage(string name)
    {
        GameEvent.OccurTrackImageTargetChange.Invoke(name, true);
    }

    public void TransferLoseTargetEventMessage(string name)
    {
        GameEvent.OccurTrackImageTargetChange.Invoke(name, false);
        GameEvent.OnIntroVoiceStartOrClose.Invoke(name, false);
    }
}
