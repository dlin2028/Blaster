using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballShooter : MonoBehaviour {

    public Transform target;
    public GameObject fireball;
    public GestureManager gestures;

    private float reloadTimePassed;
    public float reloadTime = 1;

    private bool fireReady = false;

    private void Start()
    {
    }

    void Update ()
    {
        if (!fireReady)
        {
            reloadTimePassed += Time.deltaTime;
        }

        if (reloadTime < reloadTimePassed)
        {
            if(gestures.isManipulating && !fireReady)
            {
                fireReady = true;
                GameObject createdObject = Instantiate(fireball);
                createdObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
                createdObject.transform.SetParent(transform);
            }
        }

        if(fireReady && !gestures.isManipulating)
        {
            reloadTimePassed = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                childTransform.LookAt(target);
                childTransform.GetComponent<Rigidbody>().isKinematic = false;
                childTransform.SendMessage("launch");
            }
            transform.DetachChildren();
            fireReady = false;
        }
	}
}
