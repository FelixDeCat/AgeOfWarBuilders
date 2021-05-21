using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleQuery : QueryComponent, IQuery {

    public SpatialGrid             targetGrid;
    public float radius = 5f;
    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();
    [SerializeField] bool drawGizmos;

    protected override void OnConfigure(Transform target)
    {

    }

    public override IEnumerable<IGridEntity> Query()
    {
        return myGrid.Query(
                                target.position - new Vector3(radius, 0, radius),
                                target.position + new Vector3(radius, 0, radius),
                                x => (target.position - x).sqrMagnitude < radius * radius);
    }

    public IEnumerable<IGridEntity> QueryAtPosition(Vector3 pos)
    {
        return GridManager.GetGrid(grid_type).Query(
                                pos - new Vector3(radius, 0, radius),
                                pos + new Vector3(radius, 0, radius),
                                x => (pos - x).sqrMagnitude < radius * radius);
    }

    

    void OnDrawGizmos() {
        if (!drawGizmos) return;
        if (targetGrid == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.transform.position, radius);
    }
}