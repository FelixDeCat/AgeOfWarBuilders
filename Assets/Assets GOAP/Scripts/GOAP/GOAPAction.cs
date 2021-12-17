using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;
using System.Linq;

public class WorldState
{
    float life;
    float energy;
    float hungry;
    bool hasWork;
    bool gamewin;
    bool hastool;
    bool hasWeapon;
    bool isInDanger;

    public Dictionary<string, object> register = new Dictionary<string, object>();

    public float Life       { get => life;          set { life = value;         Register(VillagerCond.HAS_LIFE, life); } }
    public float Energy     { get => energy;        set { energy = value;       Register(VillagerCond.HAS_ENERGY, energy); } }
    public float Hungry     { get => hungry;        set { hungry = value;       Register(VillagerCond.I_AM_HUNGRY, hungry); } }
    public bool HasWork     { get => hasWork;       set { hasWork = value;      Register(VillagerCond.HAS_WORK, hasWork); } }
    public bool Gamewin     { get => gamewin;       set { gamewin = value;      Register(VillagerCond.GAME_WIN, gamewin); } }
    public bool Hastool     { get => hastool;       set { hastool = value;      Register(VillagerCond.HAS_TOOL, hastool); } }
    public bool HasWeapon   { get => hasWeapon;     set { hasWeapon = value;    Register(VillagerCond.HAS_WEAPON, hasWeapon); } }
    public bool IsInDanger  { get => isInDanger;    set { isInDanger = value;   Register(VillagerCond.IS_IN_DANGER, isInDanger); } }

    void Register(string key, object value)
    {
        if (!register.ContainsKey(key)) register.Add(key, value);
        else register[key] = value;
    }
}

public class GOAPAction
{

    public Dictionary<string, object> preconditions { get; private set; }
    public Dictionary<string, object> effects { get; private set; }
    public string name { get; private set; }
    public float cost { get; private set; }
    public IState linkedState { get; private set; }

    Func<int, bool> asd;

    public Dictionary<string, Func<WorldState, bool>> new_preconditions;
    public Dictionary<string, Action<WorldState>> new_effects;

    //todos tienen que cumplir
    public bool AllPreconditionsMatch(GOAPState world_state)
    {
       //Debug.Log("CheckAction: " + name);
        foreach (var v in new_preconditions)
        {
           // Debug.Log("Precondicion: " + v.Key + " -> StateName: " + world_state.myNameState + " value: <color=" + (v.Value.Invoke(world_state.currentState) ? "green" : "red") + ">" + v.Value.Invoke(world_state.currentState).ToString() + "</color>");
            if (!v.Value.Invoke(world_state.currentState))
            {
                //Debug.Log("<i> { hay uno que no cumple } </i>");
                return false;//si alguna da false, me salgo y cancelo todo
            }
        }
        //Debug.Log("<i> { todos cumplen } </i>");
        return true;
    }

    //todos tienen que cumplir
    //public bool AllPreconditionsMatch(GOAPState world_state)
    //{
    //    foreach (var v in new_preconditions)
    //    {
    //        if (!v.Value.Invoke(world_state.currentState))
    //        {
    //            return false; //si alguna da false, me salgo y cancelo todo
    //        }
    //    }
    //    return true;
    //}

    //al menos uno cumple
    public bool AtLeastOne(GOAPState world_state)
    {
        foreach (var v in new_preconditions)
        {
           // Debug.Log("Precondicion: " + v.Key + " -> State: " + world_state.myNameState + " value: " + v.Value.Invoke(world_state.currentState).ToString());
            if (v.Value.Invoke(world_state.currentState))
            {
              //  Debug.Log("Al Menos uno cumple");
                return true;
            }
        }
       // Debug.Log("Ninguno cumple");
        return false;
    }


    public GOAPAction(string name)
    {
        this.name = name;
        cost = 1f;
        //preconditions = new Dictionary<string, object>();
        //effects = new Dictionary<string, object>();
        new_preconditions = new Dictionary<string, Func<WorldState, bool>>();
        new_effects = new Dictionary<string, Action<WorldState>>();
    }

    public GOAPAction Cost(float cost)
    {
        if (cost < 1f)
        {
            //Costs < 1f make the heuristic non-admissible. h() could overestimate and create sub-optimal results.
            //https://en.wikipedia.org/wiki/A*_search_algorithm#Properties
            Debug.Log(string.Format("Warning: Using cost < 1f for '{0}' could yield sub-optimal results", name));
        }

        this.cost = cost;
        return this;
    }

    public GOAPAction Pre(string s, object value)
    {
        //preconditions[s] = value;
        return this;
    }

    public GOAPAction PreW(string s, Func<WorldState, bool> func)
    {
        new_preconditions[s] = func;
        return this;
    }
    public GOAPAction EffectW(string s, Action<WorldState> act)
    {
        new_effects[s] = act;
        return this;
    }

    public GOAPAction Effect(string s, object value)
    {
        //effects[s] = value;
        return this;
    }

   

    public GOAPAction LinkedState(IState state)
    {
        linkedState = state;
        return this;
    }
}
