using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QueryComponent : MonoBehaviour
{
    [Header("Esto es solo para OnDrawGizmo, Usar Configure(Transform)")]
    public Transform target;
    public void Configure(Transform Target)
    {
        target = Target;
        OnConfigure(Target);
    }

    protected abstract void OnConfigure(Transform target);
    public abstract IEnumerable<IGridEntity> Query();
}
