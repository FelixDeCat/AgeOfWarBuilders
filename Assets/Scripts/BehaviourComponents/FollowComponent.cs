using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowComponent : MonoBehaviour
{
    [SerializeField] Rigidbody myRig;
    Vector3 MyPosition => myRig.gameObject.transform.position;
    [SerializeField] float movement_speed;
    Vector3 dir_to_target = Vector3.zero;

    public void Tick_Follow(float DeltaTime, Vector3 direction)
    {
        myRig.isKinematic = false;
        myRig.velocity = transform.forward + direction * movement_speed * DeltaTime;
    }
}
