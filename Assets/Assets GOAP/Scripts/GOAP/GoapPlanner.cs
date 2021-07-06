using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FSM;
using UnityEngine;

public class GoapPlanner
{

    private const int _WATCHDOG_MAX = 200;

    private int _watchdog;

    Func<IEnumerable<GOAPState>> func = delegate { return default; };

    public IEnumerable<GOAPAction> Run(GOAPState from, GOAPState to, IEnumerable<GOAPAction> actions, Func<IEnumerator, Coroutine> startCoroutine = null)
    {
        _watchdog = _WATCHDOG_MAX;

        var astar = new AStar<GOAPState>();

        if (startCoroutine != null)
        {

            IEnumerable<GOAPState> states = default;

            startCoroutine(astar.Run(from,
                             state => Satisfies(state, to),
                             node => Explode(node, actions, ref _watchdog),
                             state => GetHeuristic(state, to),
                             col => states = col
                             ));

            if (states == null) return null;
            else return CalculateGoap(states);
        }
        else
        {
            var path = astar.Run(from,
                                 state => Satisfies(state, to),
                                 node => Explode(node, actions, ref _watchdog),
                                 state => GetHeuristic(state, to));

            if (path == null) return null;
            else return CalculateGoap(path);
        }
    }

    public static FiniteStateMachine ConfigureFSM(IEnumerable<GOAPAction> plan, Func<IEnumerator, Coroutine> startCoroutine, Action<string> debugstate = null, IState defaultState = null)
    {
        var prevState = plan.First().linkedState;

        var fsm = new FiniteStateMachine(prevState, startCoroutine, debugstate);

        foreach (var action in plan.Skip(1))
        {
            if (prevState == action.linkedState) continue;
            fsm.AddTransition(action.linkedState.Name, prevState, action.linkedState);

            prevState = action.linkedState;
        }

        return fsm;
    }

    private IEnumerable<GOAPAction> CalculateGoap(IEnumerable<GOAPState> sequence)
    {

        foreach (var act in sequence.Skip(1))
        {
            //Debug.Log(act);
        }

        //Debug.Log("WATCHDOG " + _watchdog);

        return sequence.Skip(1).Select(x => x.generatingAction);
    }

    private IEnumerator CalculateGoap(IEnumerable<GOAPState> sequence, Func<List<GOAPAction>> actions)
    {
        List<GOAPAction> col = new List<GOAPAction>();

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        foreach (var act in sequence.Skip(1))
        {
            if (stopWatch.ElapsedMilliseconds >= 1f / 60f)
            {
                yield return null;
                stopWatch.Restart();
            }
            col.Add(act.generatingAction);
        }

        actions = () => col;
    }

    private static float GetHeuristic(GOAPState from, GOAPState goal) => goal.values.Count(kv => !kv.In(from.values));
    private static bool Satisfies(GOAPState state, GOAPState to) => to.values.All(kv => kv.In(state.values));

    private static IEnumerable<WeightedNode<GOAPState>> Explode(GOAPState node, IEnumerable<GOAPAction> actions,
                                                                ref int watchdog)
    {
        if (watchdog == 0) return Enumerable.Empty<WeightedNode<GOAPState>>();
        watchdog--;

        return actions.Where(action => action.preconditions.All(kv => kv.In(node.values)))
                      .Aggregate(new List<WeightedNode<GOAPState>>(), (possibleList, action) =>
                      {
                          var newState = new GOAPState(node);
                          newState.values.UpdateWith(action.effects);
                          newState.generatingAction = action;
                          newState.step = node.step + 1;

                          possibleList.Add(new WeightedNode<GOAPState>(newState, action.cost));
                          return possibleList;
                      });
    }
}
