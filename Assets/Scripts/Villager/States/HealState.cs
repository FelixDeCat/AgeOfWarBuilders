using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HealState : Villager_MonoStateBase
{
    public override IState ProcessInput()
    {
        if (!villager.LifeIsFull)
        {
            if (villager.inDanger)
            {
                return Transitionate(VillagerStatesNames.HIDE_TO_HEAL);
            }
        }
        else
        {
            if (villager.inDanger)
            {
                return Logic_Try_To_Combat();
            }
            else
            {
                return Logic_Try_To_Work(); 
            }
        }

        return this;
    }

    public override void UpdateLoop()
    {
        Tick_Heal();
    }
}
