using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutDownRenderOnPlay : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
        enabled = false;
    }
}
