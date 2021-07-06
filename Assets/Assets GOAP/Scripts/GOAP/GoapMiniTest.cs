using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;

public class GoapMiniTest : MonoBehaviour
{

    public PatrolState patrolState;
    public ChaseState chaseState;
    public MeleeAttackState meleeAttackState;

    private FiniteStateMachine _fsm;

    private float _lastReplanTime;
    private float _replanRate = .5f;


    void Start()
    {
        patrolState.OnNeedsReplan += OnReplan;
        chaseState.OnNeedsReplan += OnReplan;
        meleeAttackState.OnNeedsReplan += OnReplan;

        //OnlyPlan();
        PlanAndExecute();
    }

    private void OnlyPlan()
    {
        //var distanceKnife = 

        var actions = new List<GOAPAction>{
                                              new GOAPAction("Patrol")
                                                 .Effect("isPlayerInSight", true),

                                              new GOAPAction("Chase")
                                                 .Pre("isPlayerInSight", true)
                                                 .Effect("isPlayerInRange", true)
                                                 .Effect("isPlayerNear",    true),

                                              new GOAPAction("Melee Attack")
                                                 .Pre("isPlayerNear",   true)
                                                 .Pre("hasMeleeWeapon", true)
                                                 .Effect("isPlayerAlive", false)
                                                 .Cost(2f),

                                              new GOAPAction("Range Attack")
                                                 .Pre("isPlayerInRange", true)
                                                 .Pre("hasRangeWeapon",  true)
                                                 .Effect("isPlayerAlive", false),

                                              new GOAPAction("Pick Melee Weapon")
                                                 .Effect("hasMeleeWeapon", true),

                                              new GOAPAction("Pick Range Weapon")
                                                 .Effect("hasRangeWeapon", true)
                                          };
        var from = new GOAPState();
        from.values["isPlayerInSight"] = false;
        from.values["isPlayerNear"] = false;
        from.values["isPlayerInRange"] = false;
        from.values["isPlayerAlive"] = true;
        from.values["hasRangeWeapon"] = false;
        from.values["hasMeleeWeapon"] = false;

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;

        var planner = new GoapPlanner();

        planner.Run(from, to, actions);
    }

    private void PlanAndExecute()
    {
        var actions = new List<GOAPAction>{
                                              new GOAPAction("Patrol")
                                                 .Effect("isPlayerInSight", true)
                                                 .LinkedState(patrolState),


                                              new GOAPAction("Chase")
                                                 .Pre("isPlayerInSight", true)
                                                 .Effect("isPlayerNear",    true)
                                                 .LinkedState(chaseState),

                                              new GOAPAction("Melee Attack")
                                                 .Pre("isPlayerNear",   true)
                                                 .Effect("isPlayerAlive", false)
                                                 .LinkedState(meleeAttackState)
                                          };

        var from = new GOAPState();
        from.values["isPlayerInSight"] = false;
        from.values["isPlayerNear"] = false;
        from.values["isPlayerAlive"] = true;

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;

        var planner = new GoapPlanner();

        var plan = planner.Run(from, to, actions);

        ConfigureFsm(plan);
    }

    private void OnReplan()
    {
        if (Time.time >= _lastReplanTime + _replanRate)
        {
            _lastReplanTime = Time.time;
        }
        else
        {
            return;
        }

        var actions = new List<GOAPAction>{
                                              new GOAPAction("Patrol")
                                                 .Effect("isPlayerInSight", true)
                                                 .LinkedState(patrolState),

                                              new GOAPAction("Chase")
                                                 .Pre("isPlayerInSight", true)
                                                 .Effect("isPlayerNear", true)
                                                 .LinkedState(chaseState),

                                              new GOAPAction("Melee Attack")
                                                 .Pre("isPlayerNear", true)
                                                 .Effect("isPlayerAlive", false)
                                                 .LinkedState(meleeAttackState)
                                          };

        var from = new GOAPState();
        from.values["isPlayerInSight"] = false;
        from.values["isPlayerNear"] = false;
        from.values["isPlayerAlive"] = true;

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;

        var planner = new GoapPlanner();

        var plan = planner.Run(from, to, actions);

        ConfigureFsm(plan);
    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        Debug.Log("Completed Plan");
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

}
