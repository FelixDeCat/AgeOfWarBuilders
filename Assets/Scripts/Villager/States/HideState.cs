using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HideState : Villager_MonoStateBase
{
    public bool heal = false;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        if (heal)
        {
            villager.Go_To_Heal();
        }
        else
        {
            villager.Go_To_Rest();
        }
    }

    public override IState ProcessInput()
    {
        if (heal)
        {
            /*
            .Effect(c.HAS_LIFE,     true)//pre de heal, work, combat, find weapon
            .Effect(c.HAS_TOOL,     false)// work, find tool
            .Effect(c.HAS_WEAPON,   false)//combat, find weapon
            */

            if (!villager.inDanger)
            {
                if (villager.LifeIsFull)
                {
                    return Logic_Try_To_Work();
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
                    return Logic_Try_To_Combat();
                }
            }
        }
        else
        {
            /*
            .Effect(c.HAS_ENERGY, true) // rest, hide to rest, work, combat
            .Effect(c.HAS_TOOL, false) //find tool, work
            .Effect(c.HAS_WEAPON, false) //find weapon, combat
            */

            if (!villager.inDanger)
            {
                if (villager.EnergyIsFull)
                {
                    return Logic_Try_To_Work();
                }
                else
                {
                    return Transitionate(VillagerStatesNames.REST);
                }
            }
            else
            {
                if (villager.EnergyIsFull)
                {
                    return Logic_Try_To_Combat();
                }
            }
        }

        return this;
    }

    public override void UpdateLoop()
    {
        
    }
}
