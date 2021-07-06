using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;

public class EatState : Villager_MonoStateBase
{
    public override event Action OnNeedsReplan;

    public override IState ProcessInput()
    {
        /*
            .Effect(c.HAS_FOOD_IN_MY_INVENTORY, false) /al pre de find food y eat
            .Effect(c.I_AM_HUNGRY,              false) /al pre de work, find food
            .Effect(c.HAS_TOOL,                 false) /al pre de work, find tool
            .Effect(c.HAS_WEAPON,               false) /al pre de combat, y find weapon
            .Effect(c.HAS_ENERGY,               true) /al pre de rest, hide to rest, work, combat
         */

        if (villager.inDanger)
        {
            if (villager.EnergyIsFull && villager.MyHungryIsSatisfied)
            {
                return Logic_Try_To_Combat();
            }
            else
            {
                return Transitionate(VillagerStatesNames.HIDE_TO_REST);
            }
        }
        else
        {
            if (villager.MyHungryIsSatisfied)
            {
                if (villager.EnergyIsFull)
                {
                    return Logic_Try_To_Work();
                }
            }
            else
            {
                if (!villager.inventory.HasFood)
                {
                    return Transitionate(VillagerStatesNames.FIND_FOOD);
                }
            }
        }

        return this;
    }

    public override void UpdateLoop()
    {
        Tick_RemoveHungry();

        if (villager.MyHungryIsSatisfied)
        {
            villager.inventory.EatFood();
            OnNeedsReplan.Invoke();
        }
    }
}
