using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera25D : MonoBehaviour
{
    Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(cam);
        transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
    }
}
