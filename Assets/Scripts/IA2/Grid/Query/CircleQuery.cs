using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleQuery : QueryComponent, IQuery {

    public float radius = 5f;
    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();
    [SerializeField] bool drawGizmos;
    [SerializeField] Color gizmo_color = Color.yellow;

    protected override void OnConfigure(Transform target)
    {

    }

    public override IEnumerable<IGridEntity> Query()
    {
        if (myGrid == null) throw new System.Exception("Este Query no fue Configurado");

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
        Gizmos.color = gizmo_color;
        Gizmos.DrawWireSphere(transform.transform.position, radius);
    }
}