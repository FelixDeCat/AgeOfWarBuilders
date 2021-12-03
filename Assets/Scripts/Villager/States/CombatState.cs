using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class CombatState : Villager_MonoStateBase
{
    NPC_CombatComponent combat_Component;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        Debug.Log("EnterCombat");
        base.Enter(from, transitionParameters);
        villager.Go_To_Combat();
    }
    public override Dictionary<string, object> Exit(IState to)
    {
        Debug.Log("ExitCombat");
        return base.Exit(to);
    }

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
                if (villager.EnergyIsFull && villager.MyHungryIsSatisfied)
                {
                    return Logic_Try_To_Work();
                }
            }
            else
            {
                return Transitionate(VillagerStatesNames.HIDE);
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
                    return Transitionate(VillagerStatesNames.HIDE);
                }
            }
            else
            {
                return Transitionate(VillagerStatesNames.HIDE_TO_HEAL);
            }
        }

        return this;

    }

    public override void UpdateLoop()
    {
        //Tick_LostEnergyGainHungry();
        //Tick_TakeDamage();
    }
}
