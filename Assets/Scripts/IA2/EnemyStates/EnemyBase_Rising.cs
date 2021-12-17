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
        if (own.type == PlayObjectType.enemy_bat) Debug.Log("<color=green>BEGIN RISING</color>");
    }

    void OnEndRising()
    {
        Debug.Log("<color=green>END RISING </color>" + own.name);
        if (!own.targetFinder.IHaveATarget) { SendInput(Enemy.EnemyInputs.ImRelax); Debug.Log("ENDRISIGN_ " + own.name + "I DON HAVE A TARGET"); return; }
        if (!own.QUERY_IsTooClose) { SendInput(Enemy.EnemyInputs.MyObjetiveIsFar); Debug.Log("ENDRISIGN_" + own.name + "istoclose"); }
        else { SendInput(Enemy.EnemyInputs.MyObjetiveIsNear); Debug.Log("ENDRISIGN_ " +own.name + "is far"); }
    }

    public override void Exit(Enemy.EnemyInputs input)
    {
        base.Exit(input);
        own.myAnimEvent.REMOVE_ANIM_EVENT_LISTENER("EndRising", OnEndRising);
    }
}
