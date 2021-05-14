using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObserverQuery : QueryComponent, IQuery {

    public static string Static_ToString() => "ObserverQuery";

    // public SpatialGrid targetGrid;
    public float radius = 5f;
    public float min_radius = 0f;
    public float angle = 45;
    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();

    protected override void OnConfigure(Transform target) { }

    public override IEnumerable<IGridEntity> Query() {
        return SpatialGrid.instance.Query(
                                target.position - new Vector3(radius, 0, radius),
                                target.position + new Vector3(radius, 0, radius),
                                x => InRadius(x) && InAngle(x));
    }

    float dist;
    bool InRadius(Vector3 v3)
    {
        dist = (target.position - v3).sqrMagnitude;
        return  dist < radius * radius && dist > min_radius * min_radius;
    }
    bool InAngle(Vector3 v3)
    {
        return Vector3.Angle(v3 - target.position, target.forward) < angle;
    }

    void OnDrawGizmos() {
        if (target == null || SpatialGrid.instance) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.transform.position, radius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(target.transform.position, min_radius);
        Gizmos.color = Color.yellow;
        var vdir = Quaternion.AngleAxis(angle, target.up) * target.forward;
        var vdir2 = Quaternion.AngleAxis(-angle, target.up) * target.forward;
        Gizmos.DrawRay(target.transform.position, vdir * radius);
        Gizmos.DrawRay(target.transform.position, vdir2 * radius);
    }
}