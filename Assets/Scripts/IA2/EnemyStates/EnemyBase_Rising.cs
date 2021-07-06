using System;
using System.Collections;
using System.Collections.Generic;
using Tools.StateMachine;
using UnityEngine;

public class EnemyBase_Rising : EnemyBaseState
{
    public override void Enter(EState<Enemy.EnemyInputs> lastState)
    {
        base.Enter(lastState);
        own.view.Anim_Rising();
        own.myAnimEvent.ADD_ANIM_EVENT_LISTENER("EndRising", OnEndRising);
        own.targetFinder.BeginFinding();
    }

    void OnEndRising()
    {
        if (!own.targetFinder.IHaveATarget) { SendInput(Enemy.EnemyInputs.ImRelax); return; }
        if(!own.QUERY_IsTooClose) SendInput(Enemy.EnemyInputs.MyObjetiveIsFar);
        else SendInput(Enemy.EnemyInputs.MyObjetiveIsNear);
    }

    public override void Exit(Enemy.EnemyInputs input)
    {
        base.Exit(input);
        own.myAnimEvent.REMOVE_ANIM_EVENT_LISTENER("EndRising", OnEndRising);
    }
}
