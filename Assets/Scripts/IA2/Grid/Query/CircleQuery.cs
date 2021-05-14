using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleQuery : MonoBehaviour, IQuery {

    public SpatialGrid             targetGrid;
    public float radius = 5f;
    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();

    public IEnumerable<IGridEntity> Query() {
        return targetGrid.Query(
                                transform.position - new Vector3(radius, 0, radius),
                                transform.position + new Vector3(radius, 0, radius),
                                x => (transform.position - x).sqrMagnitude < radius * radius);
    }

    public IEnumerable<IGridEntity> QueryAtPosition(Vector3 pos)
    {
        return targetGrid.Query(
                                pos - new Vector3(radius, 0, radius),
                                pos + new Vector3(radius, 0, radius),
                                x => (pos - x).sqrMagnitude < radius * radius);
    }

    void OnDrawGizmos() {
        if (targetGrid == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.transform.position, radius);
    }
}