using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class HandsManager : MonoBehaviour {

    public static HandsManager Instance { get; private set; }
    public bool HandDetected { get { return trackedHands.Count > 0; } }
    
    public GameObject FocusedGameObject { get; private set; }
    private List<uint> trackedHands = new List<uint>();

    // Use this for initialization
    void Start () {
        Instance = this;
        InteractionManager.SourceDetected += InteractionManager_SourceDetected;
        InteractionManager.SourceLost += InteractionManager_SourceLost;
        InteractionManager.SourcePressed += InteractionManager_SourcePressed;
        InteractionManager.SourceReleased += InteractionManager_SourceReleased;
	}

    private void InteractionManager_SourceReleased(InteractionSourceState state)
    {
        FocusedGameObject = null;
    }

    private void InteractionManager_SourcePressed(InteractionSourceState state)
    {
        if(GestureManager.Instance.FocusedObject != null)
        {
            FocusedGameObject = GestureManager.Instance.FocusedObject;
        }
    }

    private void InteractionManager_SourceLost(InteractionSourceState state)
    {
        if (state.source.kind != InteractionSourceKind.Hand) return;

        if(trackedHands.Contains(state.source.id))
        {
            trackedHands.Remove(state.source.id);
        }

        FocusedGameObject = null;
    }

    private void InteractionManager_SourceDetected(InteractionSourceState state)
    {
        if (state.source.kind != InteractionSourceKind.Hand) return;

        trackedHands.Add(state.source.id);
    }

    private void OnDestroy()
    {
        InteractionManager.SourceDetected -= InteractionManager_SourceDetected;
        InteractionManager.SourceLost -= InteractionManager_SourceLost;
        InteractionManager.SourceReleased -= InteractionManager_SourceReleased;
        InteractionManager.SourcePressed -= InteractionManager_SourcePressed;
    } 
}
