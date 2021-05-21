using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QueryComponent : MonoBehaviour
{
    [SerializeField] protected Grids grid_type;

    [Header("Esto es solo para OnDrawGizmo, Usar \"Configure(Transform Target)\"")]
    public Transform target;
    protected SpatialGrid myGrid;

    public void Configure(Transform Target)
    {
        target = Target;
        OnConfigure(Target);
        myGrid = GridManager.GetGrid(grid_type);
    }
    

    protected abstract void OnConfigure(Transform target);
    public abstract IEnumerable<IGridEntity> Query();
}
