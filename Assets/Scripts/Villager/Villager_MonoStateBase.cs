using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public abstract class Villager_MonoStateBase : MonoBaseState
{
    protected Villager villager;
    public Villager_MonoStateBase SetVillager(Villager villager)
    {
        this.villager = villager;
        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        villager.DebugState("From: " + from.Name + " this: " + Name);
        base.Enter(from, transitionParameters);
    }

    protected IState Transitionate(string state)
    {
        if (Transitions.ContainsKey(state))
        {
            return Transitions[VillagerStatesNames.COMBAT];
        }
        else
        {
            return this;
        }
    }

    protected IState Logic_Try_To_Combat()
    {
        if (villager.inventory.HasWeapon)
        {
            return Transitionate(VillagerStatesNames.COMBAT);
        }
        else
        {
            return Transitionate(VillagerStatesNames.FIND_WEAPON);
        }
    }
    protected IState Logic_Try_To_Work()
    {
        if (villager.inventory.HasTool)
        {
            Debug.Log("Tengo herramienta, me voy a trabajar");
            return Transitionate(VillagerStatesNames.WORK);
        }
        else
        {
            Debug.Log("no Tengo herramienta, voy a buscar la herramienta");
            return Transitionate(VillagerStatesNames.FIND_TOOL);
        }
    }
    protected IState Logic_Try_To_EAT()
    {
        if (villager.inventory.HasFood)
        {
            return Transitionate(VillagerStatesNames.EAT);
        }
        else
        {
            return Transitionate(VillagerStatesNames.FIND_FOOD);
        }
    }

    protected IState Logic() //lógica general scripteada
    {
        if (!villager.inDanger)
        {
            if (villager.LifeIsFull)
            {
                if (villager.EnergyIsFull)
                {
                    if (villager.MyHungryIsSatisfied)
                    {
                        if (villager.HasWork)
                        {
                            if (villager.inventory.HasTool)
                            {
                                return Transitions[VillagerStatesNames.WORK];
                            }
                            else
                            {
                                return Transitions[VillagerStatesNames.FIND_TOOL];
                            }
                        }
                        else
                        {
                            return Transitions[VillagerStatesNames.IDLE];
                        }
                    }
                    else
                    {
                        if (villager.inventory.HasFood)
                        {
                            return Transitions[VillagerStatesNames.EAT];
                        }
                        else
                        {
                            return Transitions[VillagerStatesNames.FIND_FOOD];
                        }
                    }
                }
                else
                {
                    return Transitions[VillagerStatesNames.REST];
                }
            }
            else
            {
                return Transitions[VillagerStatesNames.HEAL];
            }
        }
        else
        {
            if (villager.LifeIsFull)
            {
                if (villager.EnergyIsFull)
                {
                    if (villager.inventory.HasWeapon)
                    {
                        return Transitions[VillagerStatesNames.COMBAT];
                    }
                    else
                    {
                        return Transitions[VillagerStatesNames.FIND_WEAPON];
                    }
                }
                else
                {
                    return Transitions[VillagerStatesNames.HIDE_TO_REST];
                }
            }
            else
            {
                return Transitions[VillagerStatesNames.HIDE_TO_HEAL];
            }
        }
    }

    float timer_energy_hungry;
    protected void Tick_LostEnergyGainHungry()
    {
        if (timer_energy_hungry < 1f)
        {
            timer_energy_hungry = timer_energy_hungry + 1 * Time.deltaTime;
        }
        else
        {
            villager.SpendEnergy(1);
            villager.AddHungry(1);
        }
    }

    float timer_add_energy;
    protected void Tick_AddEnergy()
    {
        if (timer_add_energy < 1f)
        {
            timer_add_energy = timer_add_energy + 1 * Time.deltaTime;
        }
        else
        {
            villager.AddEnergy(1);
        }
    }
    float timer_Spend_energy;
    protected void Tick_SpendEnergy()
    {
        if (timer_Spend_energy < 1f)
        {
            timer_Spend_energy = timer_Spend_energy + 1 * Time.deltaTime;
        }
        else
        {
            villager.SpendEnergy(1);
        }
    }

    float timer_add_hungry;
    protected void Tick_AddHungry()
    {
        if (timer_add_hungry < 1f)
        {
            timer_add_hungry = timer_add_hungry + 1 * Time.deltaTime;
        }
        else
        {
            villager.AddHungry(1);
        }
    }
    float timer_remove_hungry;
    protected void Tick_RemoveHungry()
    {
        if (timer_remove_hungry < 1f)
        {
            timer_remove_hungry = timer_remove_hungry + 1 * Time.deltaTime;
        }
        else
        {
            villager.RemoveHungry(1);
        }
    }

    float timer_heal;
    protected void Tick_Heal()
    {
        if (timer_heal < 1f)
        {
            timer_heal = timer_heal + 1 * Time.deltaTime;
        }
        else
        {
            villager.Heal(1);
        }
    }
    float timer_takedamage;
    protected void Tick_TakeDamage()
    {
        if (timer_takedamage < 1f)
        {
            timer_takedamage = timer_takedamage + 1 * Time.deltaTime;
        }
        else
        {
            villager.ReceiveDamage(1);
        }
    }
}
