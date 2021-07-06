using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IA2;

namespace IA_Felix
{
    public class AStar
    {

        public List<Node> open = new List<Node>();
        public List<Node> closed = new List<Node>();
        public List<Node> path = new List<Node>();
        Node current;

        bool drawpath = false;

        private void OnDrawGizmos()
        {
            if (drawpath)
            {
                foreach (var n in path)
                {
                    if (n.parent != null)
                    {
                        Gizmos.DrawLine(n.transform.position, n.parent.transform.position);
                    }
                }
            }
        }

        public List<Node> Execute(Node initial, Node final, int watchDogSteps = int.MaxValue)
        {
            path.Clear();
            open.Clear();
            closed.Clear();
            current = null;

            open.Add(initial);

            int watchdog = 0;

            while (open.Count > 0  && watchdog < watchDogSteps)
            {
                float small = float.MaxValue;
                watchdog++;
                
                //seteo el costo mas chico
                //seteo el nodo con costo mas chico como current

                current = open.OrderBy(x => x.costs.cost).First(); //cambiar lo de abajo por esto

                //foreach (var n in open)
                //{
                //    if (n == null) continue;
                //    if (n.costs.cost < small)
                //    {
                //        small = n.costs.cost;
                //        current = n;
                //    }
                //}

                ///////////////////////////////////////////////////

                open.Remove(current);
                closed.Add(current);

                ///////////////////////////////////////////////////
                ///

                if (current == null) { Debug.LogWarning("Current es null, se va a cortar la ejecucion"); return null; }
                if (final == null) { Debug.LogWarning("Final es null, se va a cortar la ejecucion"); return null; }


                Debug.Log("LLego hasta aca");
                if (current.Equals(final))
                {
                    path.Clear();
                    Node curr = final;
                    while (!curr.Equals(initial))
                    {
                        path.Add(curr);
                        curr = curr.parent;
                    }
                    path.Reverse();
                    return path;
                }

                foreach (var v in current.vecinos)
                {
                    v.costs.fitness = Vector3.Distance(current.transform.position, v.transform.position);

                    if (final == null) 
                    {
                        return null;
                    }
                    v.costs.heuristic = Vector3.Distance(v.transform.position, final.transform.position);
                    v.costs.cost = v.costs.fitness + v.costs.heuristic + v.costs.external_weight;

                    if (!closed.Contains(v))
                    {
                        v.parent = current;
                        open.Add(v);
                    }
                }
            }

            return null;
        }
    }
}

