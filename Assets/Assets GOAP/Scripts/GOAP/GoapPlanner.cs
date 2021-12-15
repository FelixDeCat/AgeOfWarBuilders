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

    public IEnumerable<GOAPAction> Run(GOAPState from, GOAPState to, IEnumerable<GOAPAction> actions, Func<IEnumerator, Coroutine> startCoroutine = null, Action<string> debug = null)
    {
        _watchdog = _WATCHDOG_MAX;

        var astar = new AStar<GOAPState>();
        bool flag_continue = false;

        if (startCoroutine != null)
        {
            IEnumerable<GOAPState> states = default;
            
            startCoroutine(astar.Run(from,
                             state => SatisfiesW(state, to),
                             node => Explode(node, actions),
                             state => GetHeuristicW(state, to), //esto me devuelve la cantidad que no coinciden
                             col => { states = col; debug("se ejecuta"); flag_continue = true; },
                             debug
                             ));

            debug("asdasd");

            if (states == null)
            {
                debug("los estados son nulos");
                return null;
            }
            else
            {
                debug("Calculando GOAP");
                return CalculateGoap(states);
            }
        }
        else
        {
            throw new System.Exception("FUCK YOU");
            //var path = astar.Run(from,
            //                     state => Satisfies(state, to),
            //                     node => Explode(node, actions/*, ref _watchdog*/),
            //                     state => GetHeuristic(state, to));

            //if (path == null) return null;
            //else return CalculateGoap(path);
            //return null;
        }

        
    }

    public static FiniteStateMachine ConfigureFSM(IEnumerable<GOAPAction> plan, Func<IEnumerator, Coroutine> startCoroutine, Action<string> debugstate = null)
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


    //cantidad de KeyValues del "GOAL" que se encuentran en el "FROM"
    private static float GetHeuristicW(GOAPState from, GOAPState goal) => 
        goal.currentState.register.Count(keyvalue => !keyvalue.IsInTheDictionary(from.currentState.register));

    //true si todas las KeyValues de "TO" coinciden con las KeyValues del "STATE"
    private static bool SatisfiesW(GOAPState state, GOAPState to) => 
        to.currentState.register.All(keyvalue => keyvalue.IsInTheDictionary(state.currentState.register));

    private static IEnumerable<WeightedNode<GOAPState>> Explode(GOAPState node, IEnumerable<GOAPAction> actions)
    {

        return actions

            //filtro todas las acciones que su precondicion coincida con el del node (current state)
            //.Where(x => x.preconditions.All(kv => node.values.Contains(kv)))
            .Where(x => x.AllPreconditionsMatch(node))
            //.Where(x => x.AllPreconditionsMatchWithState(node)) //todas la precondiciones tienen que dar true y ademas el nodo tiene que tener el valor ese
            //.Where(x => x.preconditions_by_worldstate.All(kv =>
            //{
            //    return kv.Value(node.currentState);
            //}
            //))

            //creo una lista con GOAPStates y sus pesos
            .Aggregate(new List<WeightedNode<GOAPState>>(), (possibleList, current_action) =>
                      {
                          //creo un nuevo estado, al pasarle el node, estamos actualizado todos los valores
                          var newState = new GOAPState(node, "MY_GENERATED_ACTION: " + current_action.name);

                          //como mas arriba filtro las acciones que cumplen las precondiciones
                          //puedo actualizar los efectos de este estado con los efectos de la current_action
                          //newState.values.UpdateWith(current_action.effects);
                          newState.RefreshStateWithNewEffects(current_action.new_effects);

                          //importante pasarle la accion que lo generó, para que luego lo comparemos y no volvamos a repetir
                          newState.generatingAction = current_action;

                          //todos los posibles nodos que van a salir de acá son... el step de mi origen +1
                          newState.step = node.step + 1;

                          //se lo agrego a la lista que el agreggate va a memorizar
                          possibleList.Add(new WeightedNode<GOAPState>(newState, current_action.cost));
                          return possibleList;
                      });
    }

}
