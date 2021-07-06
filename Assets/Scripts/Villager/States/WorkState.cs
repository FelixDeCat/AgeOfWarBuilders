using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class WorkState : Villager_MonoStateBase
{
    public override IState ProcessInput()
    {
        /*
        .Effect(c.HAS_WORK, false) //work, idle
        .Effect(c.GAME_WIN, true)
        .Effect(c.HAS_ENERGY, false) /al pre de rest, hide to rest, pre de work, pre de combat
        .Effect(c.I_AM_HUNGRY, true) // eat,work, find food
        */

        if (!villager.inDanger)
        {
            if (villager.MyHungryIsSatisfied)
            {
                if (villager.EnergyIsFull && villager.LifeIsFull)
                {
                    if (!villager.inventory.HasTool)
                    {
                        return Transitionate(VillagerStatesNames.FIND_TOOL);
                    }
                    else
                    {
                        return this;
                    }
                }
                else
                {
                    return Transitionate(VillagerStatesNames.REST);
                }
            }
            else
            {
                return Logic_Try_To_EAT();
            }
        }
        else //si en peligro
        {
            if (villager.EnergyIsFull && villager.LifeIsFull)
            {
                return Logic_Try_To_Combat();
            }

            return Transitionate(VillagerStatesNames.HIDE_TO_REST);
        }

    }

    public override void UpdateLoop()
    {
        Tick_SpendEnergy();
        Tick_AddHungry();
    }
}
