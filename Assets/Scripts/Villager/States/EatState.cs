using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;

public class EatState : Villager_MonoStateBase
{
    public override event Action OnNeedsReplan;

    int steps;

    public override Dictionary<string, object> Exit(IState to)
    {
        steps = 0;
        return base.Exit(to);
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        Debug.ClearDeveloperConsole();
        base.Enter(from, transitionParameters);
    }

    public override IState ProcessInput()
    {
        //steps++;
        //if (steps > 100) return null;

        /*
            .Effect(c.HAS_FOOD_IN_MY_INVENTORY, false) /al pre de find food y eat
            .Effect(c.I_AM_HUNGRY,              false) /al pre de work, find food
            .Effect(c.HAS_TOOL,                 false) /al pre de work, find tool
            .Effect(c.HAS_WEAPON,               false) /al pre de combat, y find weapon
            .Effect(c.HAS_ENERGY,               true) /al pre de rest, hide to rest, work, combat
         */

        if (villager.inDanger)
        {
            Debug.Log("estoy en peligro");

            if (villager.EnergyIsFull && villager.MyHungryIsSatisfied)
            {
                return Logic_Try_To_Combat();
            }
            else
            {
                return Transitionate(VillagerStatesNames.HIDE_TO_REST);
            }
        }
        else
        {
            Debug.Log("no estoy en peligro");

            if (villager.MyHungryIsSatisfied)
            {
                Debug.Log("Mi hambre esta satisfecha por completo");

                if (villager.EnergyIsFull)
                {
                    

                    Debug.Log("Mi energia esta full");
                    return Logic_Try_To_Work();
                }
                else
                {
                    Debug.Log("no tengo energia, me voy a descanzar");
                    return Transitionate(VillagerStatesNames.REST);
                }
            }
            else
            {
                Debug.Log("Mi hambre no esta satisfecha por completo");
                

                if (villager.VeryHungry())
                {
                    if (!villager.inventory.HasFood)
                    {
                        return Transitionate(VillagerStatesNames.FIND_FOOD);
                    }
                }
                else
                {
                    Debug.Log("No estoy muy hambiento");

                    if (!villager.IAmVeryTired)
                    {
                        Debug.Log("No estoy muy hambiento");
                        return Logic_Try_To_Work();
                    }
                    else
                    {
                        Debug.Log("no tengo energia, me voy a rest");
                        return Transitionate(VillagerStatesNames.REST);
                    }
                }
            }
        }

        Debug.Log("Sigo comiendo");
        return this;
    }

    public override void UpdateLoop()
    {
        Tick_RemoveHungry();
       // Tick_AddEnergy();

        if (villager.MyHungryIsSatisfied)
        {
            villager.inventory.EatFood();
            OnNeedsReplan.Invoke();
        }
    }
}
