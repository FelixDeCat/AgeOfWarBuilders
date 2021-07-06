using System.Collections;
using System.Collections.Generic;
using Tools.StateMachine;
using UnityEngine;

public class EnemyBase_Idle : EnemyBaseState
{
    public override void Enter(EState<Enemy.EnemyInputs> lastState)
    {
        base.Enter(lastState);
        own.view.Anim_Run(false);
    }
}
