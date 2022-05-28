using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody myRigidBody;
    Vector3 velocity;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    // we use this for movment so that we dont move at different speeds
    // depending on the current framerate of the computer
    public void FixedUpdate()
    {
        // fixedDeltaTime is the time between frames that unity uses to keep the
        // game running at a controlled tick rate. 
        myRigidBody.MovePosition(myRigidBody.position + velocity * Time.fixedDeltaTime);
    }

    public void LookAt(Vector3 lookPoint)
    {
        Vector3 highCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(highCorrectedPoint);
    }
}
