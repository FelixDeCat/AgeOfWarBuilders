using System.Collections;
using System.Collections.Generic;
using Tools.StateMachine;
using UnityEngine;

public class EnemyBase_Fighting : EnemyBaseState
{
    public float attack_cd;
    float timer;


    public override void Enter(EState<Enemy.EnemyInputs> lastState)
    {
        base.Enter(lastState);
        own.view.Anim_CombatIdle(true);
    }


    public override void Tick()
    {
        base.Tick();

        if (own.QUERY_IsTooClose)
        {
            if (timer < attack_cd)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                timer = 0;
                SendInput(Enemy.EnemyInputs.ICanAttack);
            }
        }
        else
        {
            timer = 0;
            own.view.Anim_CombatIdle(false);
            SendInput(Enemy.EnemyInputs.MyObjetiveIsFar);
        }
        
    }
}
