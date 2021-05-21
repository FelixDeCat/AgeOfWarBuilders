using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothLookAt : MonoBehaviour
{
    public float lookSpeed;
    public Transform Owner;
    public Transform Target;

    public bool Use_X = true;
    public bool Use_Y = false;
    public bool Use_Z = true;

    Vector3 LookVector;

    bool IHaveNormalizedDirection;
    
    public void SetTarget(Transform target) { Target = target; IHaveNormalizedDirection = false; }
    public void SetDirection(Vector3 direction) { LookVector = direction; IHaveNormalizedDirection = true; }

    public void Look(float DeltaTime)
    {
        if (!IHaveNormalizedDirection)
        {
            LookVector = Target.position - Owner.transform.position;
        }

        if (!Use_X) LookVector.x = 0;
        if (!Use_Y) LookVector.y = 0;
        if (!Use_Z) LookVector.z = 0;

        Owner.transform.forward = Vector3.Lerp(Owner.forward, LookVector, DeltaTime * lookSpeed);


    }
}
