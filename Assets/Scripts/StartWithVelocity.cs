using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StartWithVelocity : MonoBehaviour {

    public Vector3 StartingVector;
    public Vector3 StartingTorque;

	void Start () {
        Rigidbody body = GetComponent<Rigidbody>();
        body.AddRelativeForce(StartingVector);
        body.AddTorque(StartingTorque);
    }
}
