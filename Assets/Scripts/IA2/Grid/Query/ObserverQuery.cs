using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObserverQuery : MonoBehaviour, IQuery {

   // public SpatialGrid targetGrid;
    public float radius = 5f;
    public float angle = 45;
    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();

    public Transform observer;

    public IEnumerable<IGridEntity> Query() {
        return SpatialGrid.instance.Query(
                                observer.position - new Vector3(radius, 0, radius),
                                observer.position + new Vector3(radius, 0, radius),
                                x => InRadius(x) && InAngle(x));
    }

    bool InRadius(Vector3 v3)
    {
        return (observer.position - v3).sqrMagnitude < radius * radius;
    }
    bool InAngle(Vector3 v3)
    {
        return Vector3.Angle(v3 - observer.position, observer.forward) < angle;
    }

    void OnDrawGizmos() {
        if (SpatialGrid.instance == null || observer == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(observer.transform.position, radius);
        Gizmos.color = Color.yellow;
        var vdir = Quaternion.AngleAxis(angle, observer.up) * observer.forward;
        var vdir2 = Quaternion.AngleAxis(-angle, observer.up) * observer.forward;
        Gizmos.DrawRay(observer.transform.position, vdir * radius);
        Gizmos.DrawRay(observer.transform.position, vdir2 * radius);
    }
}