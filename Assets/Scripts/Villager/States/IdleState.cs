using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class IdleState : Villager_MonoStateBase
{
    public override IState ProcessInput()
    {
        if (villager.HasWork)
        {
            return Transitionate(VillagerStatesNames.WORK);
        }
        else
        {
            return this;
        }
    }

    float timer;

    public override void UpdateLoop()
    {

    }
}
