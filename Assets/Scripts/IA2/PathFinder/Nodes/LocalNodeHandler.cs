using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IA_Felix;

public class LocalNodeHandler : MonoBehaviour
{
    List<Node> nodes;
    FindNodesAndAdd parentNodes;

    void Start()
    {
        //Invoke("Find", 0.1f);
    }

    public void Find()
    {
        var finder = GetComponentInChildren<FindNodesAndAdd>();
        parentNodes = finder;
        if (parentNodes == null) return;
        nodes = parentNodes.transform.GetComponentsInChildren<Node>().ToList();
        nodes.ForEach(x => x.OnStart());
        nodes.ForEach(x => x.CheckIfNeighborsCanSeeMe());
        nodes.ForEach(x => x.ShutDown());
    }
    public void Render(bool rend)
    {
        nodes.ForEach(x => x.render.gizmos = rend);
    }
}
