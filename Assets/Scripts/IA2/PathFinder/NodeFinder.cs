using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA_Felix;
using Tools.Extensions;

public class NodeFinder
{
    float radius;
    public NodeFinder(float _radius) => radius = _radius;

    public Node FindMostCloseNode(Vector3 pos) => 
        pos.FindMostClose<Node>(radius);
}
