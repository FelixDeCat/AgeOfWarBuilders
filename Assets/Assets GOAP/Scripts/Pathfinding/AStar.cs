using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using VSCodeEditor;

public class AStar<T> {

    public IEnumerable<T> Run(T                                     start,
                              Func<T, bool>                         isGoal,
                              Func<T, IEnumerable<WeightedNode<T>>> explode,
                              Func<T, float>                        getHeuristic)
    {
        
        var queue     = new PriorityQueue<T>();
        var distances = new Dictionary<T, float>();
        var parents   = new Dictionary<T, T>();
        var visited   = new HashSet<T>();

        distances[start] = 0;
        queue.Enqueue(new WeightedNode<T>(start, 0));
        
        while (!queue.IsEmpty) {
            var dequeued = queue.Dequeue();
            visited.Add(dequeued.Element);

            if (isGoal(dequeued.Element)) return CommonUtils.CreatePath(parents, dequeued.Element);

            var toEnqueue = explode(dequeued.Element);

            foreach (var transition in toEnqueue) {
                var neighbour                   = transition.Element;
                var neighbourToDequeuedDistance = transition.Weight;

                var startToNeighbourDistance =
                    distances.ContainsKey(neighbour) ? distances[neighbour] : float.MaxValue;
                var startToDequeuedDistance = distances[dequeued.Element];

                var newDistance = startToDequeuedDistance + neighbourToDequeuedDistance;

                if (!visited.Contains(neighbour) && startToNeighbourDistance > newDistance) {
                    distances[neighbour] = newDistance;
                    parents[neighbour]   = dequeued.Element;

                    queue.Enqueue(new WeightedNode<T>(neighbour, newDistance + getHeuristic(neighbour)));
                }
            }
        }

        return null;
    }



    public IEnumerator Run(T start,
                             Func<T, bool> isGoal,
                             Func<T, IEnumerable<WeightedNode<T>>> explode,
                             Func<T, float> getHeuristic,
                             Action<IEnumerable<T>> result, 
                             Action<string> debug = null)
    {

        var queue = new PriorityQueue<T>();
        var distances = new Dictionary<T, float>();
        var parents = new Dictionary<T, T>();
        var visited = new HashSet<T>();

        distances[start] = 0;
        queue.Enqueue(new WeightedNode<T>(start, 0));

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        debug("//////////////////RUN from: " + start.ToString());

        while (!queue.IsEmpty)
        {
            debug("<color=blue>### BEGIN While</color>");
            var dequeued = queue.Dequeue();
            visited.Add(dequeued.Element);

            debug(">>>>> Dequeue: " + dequeued.Element.ToString());

            if (isGoal(dequeued.Element))
            {
                debug("<color=green><b>Creando Path</b></color>");
                var path = CommonUtils.CreatePath(parents, dequeued.Element);
                foreach (var p in path)
                {
                    debug(p.ToString());
                }
                result(path);
                yield break;
            }

            if (stopwatch.ElapsedMilliseconds >= 1f / 60f * 1000f)
            {
                debug("<color=green><b>volviendo por el stopwatch</b></color>");
                stopwatch.Restart();
                result(CommonUtils.CreatePath(parents, dequeued.Element));
                yield return null;
            }

            debug("<color=red><b>[[ Explode</b></color>"); 
            var toEnqueue = explode(dequeued.Element);
            debug("<color=red><b>Explode ]]</b></color>");

            debug("<color=cyan>BEGIN_FOREACH_neighbours</color>");

            foreach (var transition in toEnqueue)
            {
                
                var neighbour = transition.Element;
                var neighbourToDequeuedDistance = transition.Weight;

                debug(">>>> CHECKING : " + neighbour.ToString() + " WEIGHT: " + neighbourToDequeuedDistance);

                var startToNeighbourDistance = distances.ContainsKey(neighbour) ? distances[neighbour] : float.MaxValue;

                float startToDequeuedDistance = 0;

                if (distances.ContainsKey(dequeued.Element))
                {
                    startToDequeuedDistance = distances[dequeued.Element];
                }
                else
                {
                    debug("<color=red>en el diccionario de distancias no está [" + dequeued.Element.ToString() + "]</color>");
                }
                

                var newDistance = startToDequeuedDistance + neighbourToDequeuedDistance;

                debug(">>>> StartToNeig : " + startToNeighbourDistance.ToString("00f") + " StartToDeq: " + startToDequeuedDistance + " NewDist: " + newDistance);

                if (!visited.Contains(neighbour) && startToNeighbourDistance > newDistance)
                {
                    debug(">>>> Visitados no contiene Vecino y La distancia al vecino es mayor que la nueva distancia");

                    distances[neighbour] = newDistance;
                    parents[neighbour] = dequeued.Element;

                    queue.Enqueue(new WeightedNode<T>(neighbour, newDistance + getHeuristic(neighbour)));

                    debug("encolando..." + neighbour + "-con-el-peso:[" + (newDistance + getHeuristic(neighbour)).ToString() +"]");
                }
            }

            debug("<color=cyan>END_FOREACH_neighbours</color>");

            debug("<color=blue>### END While</color>");
        }

        yield return null;
    }
}