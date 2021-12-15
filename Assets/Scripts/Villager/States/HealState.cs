using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HealState : Villager_MonoStateBase
{
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        villager.Go_To_Heal();
    }

    public override IState ProcessInput()
    {
        if (!villager.LifeIsFull)
        {
            if (villager.inDanger)
            {
                return Transitionate(VillagerStatesNames.HIDE_TO_HEAL);
            }
            else
            {
                if (villager.HP > villager.queryvalues.low_life_min)
                {
                    return Logic_Try_To_Work();
                }
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


        Debug.Log("sigo curandome");
        return this;
    }

    public override void UpdateLoop()
    {
        //Tick_Heal();
    }
}
