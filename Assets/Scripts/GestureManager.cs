using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GestureManager : MonoBehaviour
{
    public WorldCursor cursor;
    GestureRecognizer recognizer;

    public static GestureManager Instance { get; private set; }
    public GameObject FocusedObject { get; private set; }
    public GameObject OverrideFocusedObject { get; set; }

    public bool isManipulating { get; private set; }
    public Vector3 ManipiulationPosition { get; private set; }


    private RaycastHit hitInfo;

    public RaycastHit HitInfo
    {
        get { return hitInfo; }
    }

    // Use this for initialization
    void Start () {

        Instance = this;

        recognizer = new GestureRecognizer();

        recognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.ManipulationTranslate);

        recognizer.ManipulationStartedEvent += Recognizer_ManipulationStartedEvent;
        recognizer.ManipulationUpdatedEvent += Recognizer_ManipulationUpdatedEvent;
        recognizer.ManipulationCompletedEvent += Recognizer_ManipulationCompletedEvent;
        recognizer.ManipulationCanceledEvent += Recognizer_ManipulationCancelledEvent;


        recognizer.TappedEvent += Recognizer_TappedEvent;
        

        recognizer.StartCapturingGestures();
	}
    private void Recognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        OverrideFocusedObject = null;
        isManipulating = false;
    }
    private void Recognizer_ManipulationCancelledEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        OverrideFocusedObject = null;
        isManipulating = false;
    }
    private void Recognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        if (!OverrideFocusedObject) return;

        isManipulating = true;
        ManipiulationPosition = cumulativeDelta;
    }

    private void Recognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        if (!HandsManager.Instance.FocusedGameObject) return;

        isManipulating = true;
        ManipiulationPosition = cumulativeDelta;
        FocusedObject = OverrideFocusedObject = HandsManager.Instance.FocusedGameObject;
    }

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if(cursor.HitInfo.collider.gameObject == null)
        {
            return;
        }

        cursor.HitInfo.collider.gameObject.SendMessage("OnSelect");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var headPos = Camera.main.transform.position;
        var headDirection = Camera.main.transform.forward;

        if (Physics.Raycast(new Ray(headPos, headDirection), out hitInfo))
        {
            FocusedObject = hitInfo.collider.gameObject;
        }
    }

    private void OnDestroy()
    {
        if(recognizer != null)
        {
            recognizer.StopCapturingGestures();
            recognizer.TappedEvent -= Recognizer_TappedEvent;
            recognizer.ManipulationStartedEvent -= Recognizer_ManipulationStartedEvent;
            recognizer.ManipulationUpdatedEvent -= Recognizer_ManipulationUpdatedEvent;
            recognizer.ManipulationCompletedEvent -= Recognizer_ManipulationCompletedEvent;
            recognizer.ManipulationCanceledEvent -= Recognizer_ManipulationCancelledEvent;
        }
    }


}
