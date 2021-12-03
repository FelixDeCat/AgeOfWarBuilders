using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class WorkState : Villager_MonoStateBase
{
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        Debug.Log("enter work");

       // villager.StopAction();
        villager.GoToWork();
        
    }
    public override Dictionary<string, object> Exit(IState to)
    {
        villager.StopWork();
        return base.Exit(to);
    }

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
                    return Transitionate(VillagerStatesNames.HIDE);
                }
            }
            else
            {
                return Transitionate(VillagerStatesNames.HIDE);
                //if (villager.VeryHungry())
                //{
                //    return Logic_Try_To_EAT();
                //}
                //else
                //{
                //    return this;
                //}
            }
        }
        else //si en peligro
        {
            if (villager.EnergyIsFull && villager.LifeIsFull)
            {
                return Logic_Try_To_Combat();
            }

            return Transitionate(VillagerStatesNames.HIDE);
        }

    }

    public override void UpdateLoop()
    {
        //if (HasStarted)
        //{
        //    Debug.Log("Executing WORK");
        //}
    }
}
