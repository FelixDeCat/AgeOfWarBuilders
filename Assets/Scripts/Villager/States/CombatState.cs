using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class CombatState : Villager_MonoStateBase
{
    public override IState ProcessInput()
    {
        /*
            .Effect(c.IS_IN_DANGER, false) //heal, rest, hidetoheal, hidetorest, work, combat
            .Effect(c.HAS_ENERGY,   false) //rest, hide to rest, work,combat
            .Effect(c.HAS_LIFE,     false) //heal, hide to heal, work, combat, find weapon
            .Effect(c.HAS_TOOL,     false) //work, find tool
            .Effect(c.I_AM_HUNGRY,  true) // eat, work, find food
         */

        if (!villager.inDanger)
        {
            if (villager.LifeIsFull)
            {
                if (villager.EnergyIsFull)
                {
                    if (villager.MyHungryIsSatisfied)
                    {
                        return Logic_Try_To_Work();
                    }
                    else
                    {
                        return Logic_Try_To_EAT();
                    }
                }
                else
                {

                    return Transitionate(VillagerStatesNames.REST);
                }
            }
            else
            {
                return Transitionate(VillagerStatesNames.HEAL);

            }
        }
        else
        {
            if (villager.LifeIsFull)
            {
                if (villager.EnergyIsFull)
                {
                    if (!villager.inventory.HasWeapon)
                    {
                        return Transitionate(VillagerStatesNames.FIND_WEAPON);
                    }
                    else
                    {
                        return this;
                    }
                }
                else
                {
                    return Transitionate(VillagerStatesNames.HIDE_TO_REST);
                }
            }
            else
            {
                return Transitionate(VillagerStatesNames.HIDE_TO_HEAL);
            }
        }

    }

    public override void UpdateLoop()
    {
        Tick_LostEnergyGainHungry();
        Tick_TakeDamage();
    }
}
