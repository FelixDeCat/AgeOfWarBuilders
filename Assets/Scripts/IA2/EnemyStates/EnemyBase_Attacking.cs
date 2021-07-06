using System.Collections;
using System.Collections.Generic;
using Tools.StateMachine;
using UnityEngine;

public class EnemyBase_Attacking  : EnemyBaseState
{
    FindTarget finder;
    

    public override void Enter(EState<Enemy.EnemyInputs> lastState)
    {
        base.Enter(lastState);
        own.view.Anim_Attack();
        own.myAnimEvent.ADD_ANIM_EVENT_LISTENER("OnAttack", ANIM_EVENT_OnAttack);
        own.myAnimEvent.ADD_ANIM_EVENT_LISTENER("EndAttack", ANIM_EVENT_EndAttack);
        finder = own.targetFinder;
    }

    public override void Exit(Enemy.EnemyInputs input)
    {
        base.Exit(input);
        own.myAnimEvent.REMOVE_ANIM_EVENT_LISTENER("OnAttack", ANIM_EVENT_OnAttack);
        own.myAnimEvent.REMOVE_ANIM_EVENT_LISTENER("EndAttack", ANIM_EVENT_EndAttack);
    }

    void ANIM_EVENT_OnAttack()
    {
        if (!finder.Target) throw new System.Exception("La Entity es null");
        if (own.QUERY_IsTooClose)
        {
            finder.Target.ReceiveDamage(own.damage);
        }
    }
    void ANIM_EVENT_EndAttack()
    {
        SendInput(Enemy.EnemyInputs.MyAttackIsFinish);
    }
}
