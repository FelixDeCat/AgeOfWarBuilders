using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class RestState : Villager_MonoStateBase
{
    float timer;

    public override event Action OnNeedsReplan;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        villager.Go_To_Rest();
    }

    public override IState ProcessInput()
    {
        //    .Pre(c.IS_IN_DANGER, false)
        //    .Pre(c.HAS_ENERGY, false)
        //    .Effect(c.HAS_TOOL, false) /al pre de work, al pre de find tool
        ////    .Effect(c.HAS_WEAPON, false) /al pre de combat, al pre de find weapon
        //    .Effect(c.HAS_ENERGY, true) /al pre de hide to rest, pre de work, pre de combat

        Debug.Log("Entro a descanzar");

        if (!villager.inDanger)
        {
            Debug.Log("No estoy en peligro");

            if (villager.EnergyIsFull)
            {
                Debug.Log("Tengo energia, voy a trabajar");
                return Transitionate(VillagerStatesNames.WORK);
            }
            else
            {
                Debug.Log("No tengo energia, sigo en rest");
                return this;
            }
        }
        else
        {
            Debug.Log("Estoy en peligro");

            if (villager.EnergyIsFull)
            {
                return Logic_Try_To_Combat();
            }
            else
            {
                return Transitionate(VillagerStatesNames.HIDE_TO_REST);
            }
        }
    }

    public override void UpdateLoop()
    {
        //Tick_AddEnergy();
    }
}
