using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;

public class FindToolState : Villager_MonoStateBase
{
    public override event Action OnNeedsReplan;

    public override IState ProcessInput()
    {
        if (villager.inventory.HasTool)
        {
            return Transitionate(VillagerStatesNames.WORK);
        }
        return this;
    }

    public override void UpdateLoop()
    {
        Tick_LostEnergyGainHungry();

        if (Input.GetKeyDown(KeyCode.F))
        {
            villager.inventory.PickUpTool();
            OnNeedsReplan.Invoke();
        }
    }
}
