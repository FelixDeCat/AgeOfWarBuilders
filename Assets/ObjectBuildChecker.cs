using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBuildChecker : MonoBehaviour
{
    public LayerMask canNotBuild;

    bool canBuild;
    public bool CanBuild { get { return canBuild; } }

    public float radius;

    public Vector3 capsule_point1;
    public Vector3 capsule_point2;

    Renderer[] renders;

    private void Awake()
    {
        renders = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            renders[i].material.SetColor("_Color", Color.black);
        }
       
    }

    private void Update()
    {
        var cols = Physics.OverlapCapsule(transform.position + capsule_point1, transform.position + capsule_point2, radius, canNotBuild);
        canBuild = cols.Length == 0;//si hay algo no puedo contruir


        for (int i = 0; i < renders.Length; i++)
        {
            renders[i].material.SetColor("_Color", canBuild ? Color.green : Color.red);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(capsule_point1, radius);
        Gizmos.DrawSphere(capsule_point2, radius);
    }
}
