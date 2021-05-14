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
        //var debug = querie.QueryAtPosition(position)
        //    .OfType<Threat>().ToArray();

        var threats = querie.QueryAtPosition(position)
            .OfType<Threat>()
           .OrderByDescending(x => x.ThreatByDistanceMultiplier(position));

        //for (int i = 0; i < debug.Length; i++)
        //{
        //    Debug.LogWarning("DEB THREATH: " + debug[i].gameObject.name);
        //}

        return threats;
    }
}
