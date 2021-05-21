using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ThreatManager : MonoBehaviour
{
    [SerializeField] CircleQuery querie;

    public static ThreatManager instance;
    private void Awake() => instance = this;

    public static IEnumerable<Threat> GetThreatsAtPoint(Vector3 position) { return instance.GetThreats(position); }

    private IEnumerable<Threat> GetThreats(Vector3 position)
    {
        var threats = querie.QueryAtPosition(position)
            .OfType<GridComponent>()
            .Select(x => x.Grid_Object.GetComponent<Threat>())
           .OrderByDescending(x => x.ThreatByDistanceMultiplier(position));
        return threats;
    }
}
