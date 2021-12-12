using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;

public class WorldState
{
    public int gold;
    public float life;
    public float energy;
}

public class GOAPAction {

    public Dictionary<string, object> preconditions { get; private set; }
    public Dictionary<string, object> effects       { get; private set; }
    public string                   name          { get; private set; }
    public float                    cost          { get; private set; }
    public IState                   linkedState   { get; private set; }

    Func<int, bool> asd;

    public Dictionary<string, Func<WorldState, bool>> preconditions_by_worldstate;
    public Dictionary<string, Action<WorldState>> effects_by_worldstate;


    public GOAPAction(string name) {
        this.name     = name;
        cost          = 1f;
        preconditions = new Dictionary<string, object>();
        effects       = new Dictionary<string, object>();
    }

    public GOAPAction Cost(float cost) {
        if (cost < 1f) {
            //Costs < 1f make the heuristic non-admissible. h() could overestimate and create sub-optimal results.
            //https://en.wikipedia.org/wiki/A*_search_algorithm#Properties
            Debug.Log(string.Format("Warning: Using cost < 1f for '{0}' could yield sub-optimal results", name));
        }

        this.cost = cost;
        return this;
    }

    public GOAPAction Pre(string s, object value) {
        preconditions[s] = value;
        return this;
    }

    public GOAPAction Pre(string s, Func<WorldState, bool> func)
    {
        preconditions_by_worldstate[s] = func;
        return this;
    }


    public GOAPAction Effect(string s, object value) {
        effects[s] = value;
        return this;
    }

    public GOAPAction Effect(string s, Action<WorldState> act)
    {
        effects_by_worldstate[s] = act;
        return this;
    }

    public GOAPAction LinkedState(IState state) {
        linkedState = state;
        return this;
    }
}
