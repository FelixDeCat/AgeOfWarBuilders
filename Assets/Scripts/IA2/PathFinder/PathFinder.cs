using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.Extensions;
using IA2;
using IA_Felix;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class PathFinder : MonoBehaviour
{
    public bool canMove;

    [Header("Configuration")]
    public float movement_speed = 0.3f;
    public float radius_to_find_nodes = 1.5f;
    public float distance_to_close = 0.1f;
    public float forwardspeed = 5f;
    [SerializeField] Rigidbody rb;

    float auxspeed;

    NodeFinder nodefinder;
    AStar astar = new AStar();
    Queue<Node> nodosASeguir = new Queue<Node>();
    public List<Node> debugNodes = new List<Node>();
    Node currentNode;
    Node initialNode;
    Node finalNode;

    public LineRenderer lr_debug;

    public Action callbackEndDinamic = delegate { };
    public Action callbackOnBeginMove = delegate { };
    public Action callbackOnEndMove = delegate { };

    public void SpeedRelaxSearch() => movement_speed = auxspeed;
    public void SpeedAlertSearch(float _speed) => movement_speed = _speed;

    public void Initialize(Rigidbody _rb)
    {
        rb = _rb;
        rb.isKinematic = true;
        nodefinder = new NodeFinder(radius_to_find_nodes);
        auxspeed = movement_speed;
        //Configure(roottt, rb, groundSensorrr);
    }

    public void StopMovement()
    {
        callbackOnEndMove.Invoke();
        canMove = false;
    }

    public void AddCallbackEnd(Action _callbackEndDinamic)
    {
        callbackEndDinamic = _callbackEndDinamic;
    }
    internal void RemoveCallbackEnd()
    {
        callbackEndDinamic = delegate { };
    }

    public void AddCallback_OnBeginMove(Action _cbk)
    {
        callbackOnBeginMove = _cbk;
    }
    public void AddCallback_OnEndMove(Action _cbk)
    {
        callbackOnEndMove = _cbk;
    }


    public bool Execute(Vector3 pos)
    {
        rb.isKinematic = false;

        initialNode = nodefinder.FindMostCloseNode(rb.transform.position);
        finalNode = nodefinder.FindMostCloseNode(pos);

        if (initialNode == null || finalNode == null)
        {
            Debug.LogWarning("" +
            "PATHFINDER IMPORTANT MESSAGE: { Initial o Final node es nulo } " +
            "=> Cortando la ejecucion::: REVISAR QUE... >>> " +
            "[ el Node finder tiene suficiente radio ] " +
            "[ los layers son los correctos ] " +
            "[ Hay nodos en el punto de busqueda ]");
            return false;
        }

        var col = astar.Execute(initialNode, finalNode,100);

        //var positions = col.Select(x => x.transform.position).ToArray();

        //if (lr_debug)
        //{
        //    lr_debug.positionCount = positions.Length;

        //    for (int i = 0; i < positions.Length; i++)
        //    {
        //        lr_debug.SetPosition(i, positions[i] + Vector3.up/2);
        //    }
        //}

        if (col == null) { Debug.Log("Tengo una Lista nula"); return false; }

        nodosASeguir = new Queue<Node>(col);
        currentNode = initialNode;

        //render
        foreach (var n in nodosASeguir) n.render.PintarNegro();
        initialNode.render.PintarRojo();
        finalNode.render.PintarVerde();

        canMove = true;
        callbackOnBeginMove.Invoke();

        return true;
    }

    public bool CanExecute(Vector3 pos)
    {
        initialNode = nodefinder.FindMostCloseNode(rb.transform.position);
        finalNode = nodefinder.FindMostCloseNode(pos);
        if (initialNode == null || finalNode == null)
        {
            Debug.LogWarning("" +
            "PATHFINDER IMPORTANT MESSAGE: { Initial o Final node es nulo } " +
            "=> Cortando la ejecucion::: REVISAR QUE... >>> " +
            "[ el Node finder tiene suficiente radio ] " +
            "[ los layers son los correctos ] " +
            "[ Hay nodos en el punto de busqueda ]");
            return false;
        }
        var col = astar.Execute(initialNode, finalNode, 100);
        if (col == null) { Debug.Log("Tengo una Lista nula"); return false; }
        return true;
    }

    void Execute()
    {

    }

    bool dequeueNext = false;
    float currentdist = float.MaxValue;
    public void Refresh()
    {
        if (!canMove) return;

        if (currentNode == null) { Debug.LogError("El CurrentNode es Nulo"); }

        currentdist = Vector3.Distance(rb.transform.position, currentNode.transform.position);

        if (currentdist < distance_to_close)
        {

            currentNode.render.PintarNegro();
            if (currentNode == finalNode)
            {
                movement_speed = auxspeed;
                canMove = false;
                rb.isKinematic = true;
                callbackEndDinamic.Invoke();
                callbackOnEndMove.Invoke();
            }
            dequeueNext = true;
        }

        if (dequeueNext)
        {
            dequeueNext = false;
            if (nodosASeguir.Count > 0)
            {
                currentNode = nodosASeguir.Dequeue();
            }
            else
            {
                return;
            }
        }

        Vector3 dir = currentNode.transform.position - rb.transform.position;
        dir = new Vector3(dir.x, dir.y, dir.z);
        dir.Normalize();
        rb.velocity = new Vector3(dir.x * movement_speed, dir.y * movement_speed, dir.z * movement_speed);
        // Vector3 fowardRotation = ObstacleAvoidance(new Vector3(dir.x, 0, dir.z));
        //// Rotation(fowardRotation.normalized);
        // MoveWRigidbodyV(ObstacleAvoidance(fowardRotation));
        rb.transform.forward = Vector3.Lerp(rb.transform.forward, new Vector3(dir.x, 0, dir.z), forwardspeed * Time.deltaTime);


    }
}




















