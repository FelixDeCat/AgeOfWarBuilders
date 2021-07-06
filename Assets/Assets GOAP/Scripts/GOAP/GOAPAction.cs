using System.Collections.Generic;
using FSM;
using UnityEngine;

public class GOAPAction {

    public Dictionary<string, bool> preconditions { get; private set; }
    public Dictionary<string, bool> effects       { get; private set; }
    public string                   name          { get; private set; }
    public float                    cost          { get; private set; }
    public IState                   linkedState   { get; private set; }


    public GOAPAction(string name) {
        this.name     = name;
        cost          = 1f;
        preconditions = new Dictionary<string, bool>();
        effects       = new Dictionary<string, bool>();
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

    public GOAPAction Pre(string s, bool value) {
        preconditions[s] = value;
        return this;
    }

    public GOAPAction Effect(string s, bool value) {
        effects[s] = value;
        return this;
    }

    public GOAPAction LinkedState(IState state) {
        linkedState = state;
        return this;
    }
}
