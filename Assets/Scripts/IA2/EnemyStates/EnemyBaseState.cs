using System;
using Tools.StateMachine;
using UnityEngine;

public abstract class EnemyBaseState : MonoState<Enemy.EnemyInputs>
{
    public MonoState<Enemy.EnemyInputs> Configure(Action<Enemy.EnemyInputs> action, Enemy own)
    {
        this.own = own;
        return base.Configure(action);
    }
    protected Enemy own;
}
