using System.Collections;
using System.Collections.Generic;
using Tools.StateMachine;
using UnityEngine;

public class EnemyBase_Dying : EnemyBaseState
{
    public override void Enter(EState<Enemy.EnemyInputs> lastState)
    {
        base.Enter(lastState);
        own.view.Anim_Death(true);
    }

    public override void Exit(Enemy.EnemyInputs input)
    {
        base.Exit(input);
        own.view.Anim_Death(false);
    }

}
