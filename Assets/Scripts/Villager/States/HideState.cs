using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HideState : Villager_MonoStateBase
{
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        villager.StopAction();
        villager.Go_To_Rest();
    }

    public override IState ProcessInput()
    {
        if (!villager.inDanger)
        {
            if (villager.LifeIsFull && villager.EnergyIsFull)
            {
                return Logic_Try_To_Work();
            }
            else
            {
                return this;
            }
        }
        else
        {
            if (villager.LifeIsFull)
            {
                return Logic_Try_To_Combat();
            }
        }

        return this;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        if (HasStarted)
        {
            Debug.Log("Executing HIDE");
        }
    }
}
