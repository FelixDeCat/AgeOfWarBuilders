using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ThreatReceptor : MonoBehaviour
{

    float timer;
    public float time_to_recalculate_Threat = 0.5f;

    public PlayObjectType preferences;

    [SerializeField] Threat[] threats_collection = new Threat[0];
    public IEnumerable<Threat> Threats => threats_collection;

    Threat own;

    private void Start()
    {
        own = GetComponent<Threat>();
    }

    public void Update()
    {
        if (timer < time_to_recalculate_Threat)
        {
            timer = timer + 1 * Time.deltaTime;
        }
        else
        {
            timer = 0;
            threats_collection = ThreatManager.GetThreatsAtPoint(transform.position).OrderByDescending(x => x.MyType == preferences).ToArray();
        }
    }
}
