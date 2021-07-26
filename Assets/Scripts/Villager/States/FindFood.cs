using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class FindFood : Villager_MonoStateBase
{
    public override event Action OnNeedsReplan;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        villager.Go_To_Take_Food();
    }

    public override IState ProcessInput()
    {
        if (villager.inventory.HasFood && !villager.MyHungryIsSatisfied)
        {
            return Transitionate(VillagerStatesNames.EAT);
        }
        return this;
    }

    public override void UpdateLoop()
    {
        //Tick_SpendEnergy();

        if (Input.GetKeyDown(KeyCode.F))
        {
            villager.inventory.PickUpFood();
            OnNeedsReplan.Invoke();
        }
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        return base.Exit(to);
    }
}
