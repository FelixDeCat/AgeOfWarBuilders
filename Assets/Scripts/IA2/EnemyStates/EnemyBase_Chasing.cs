using System.Collections;
using System.Collections.Generic;
using Tools.StateMachine;
using UnityEngine;

public class EnemyBase_Chasing : EnemyBaseState
{
    public Transform debug_target;
    public bool debug_flag_ISFar;

    public override void Enter(EState<Enemy.EnemyInputs> lastState)
    {
        base.Enter(lastState);
        own.view.Anim_Run(true);
    }
    public override void Exit(Enemy.EnemyInputs input)
    {
        base.Exit(input);
        own.view.Anim_Run(false);
    }

    float timer;
    public override void Tick()
    {
        base.Tick();

        var finder = own.targetFinder;
        var lookat = own.smoothLookAt;

        debug_target = finder.Target.transform;

        lookat.SetDirection(finder.DirectionToTarget);

        if (own.QUERY_TargetIsFar)
        {
            debug_flag_ISFar = true;
            if (timer < 1) timer = timer + 1 * Time.deltaTime;
            else
            {
                own.CanNotUsePathFinder = false;
                own.GoToPositionWithPathFinder(finder.Target.transform.position);
                timer = 0;
            } 
        }
        else
        {
            debug_flag_ISFar = false;
            lookat.Look(Time.deltaTime);

            own.CanNotUsePathFinder = true;

            if (own.QUERY_IsTooClose)
            {
                SendInput(Enemy.EnemyInputs.MyObjetiveIsNear);
            }
            else
            {
                lookat.SetTarget(finder.Target.transform);
                own.follow_component.Tick_Follow(Time.deltaTime, finder.DirectionToTarget);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!own) return;
        if (!own.targetFinder)
        if (!own.targetFinder.Target)
        {
            Gizmos.DrawLine(own.transform.position, own.targetFinder.Target.transform.position);
        }
    }
}
