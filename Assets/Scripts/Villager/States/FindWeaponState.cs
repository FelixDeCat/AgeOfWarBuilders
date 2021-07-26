using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class FindWeaponState : Villager_MonoStateBase
{
    public override event Action OnNeedsReplan;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        villager.Go_To_Take_Weapon();
    }

    public override IState ProcessInput()
    {
        if (villager.inventory.HasWeapon)
        {
            return Transitionate(VillagerStatesNames.COMBAT);
        }
        return this;
    }

    public override void UpdateLoop()
    {
        //Tick_LostEnergyGainHungry();

        if (Input.GetKeyDown(KeyCode.F))
        {
            villager.inventory.PickUpWeapon();
            OnNeedsReplan.Invoke();
        }
    }
}
