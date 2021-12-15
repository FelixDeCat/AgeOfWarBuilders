using System.Collections.Generic;
using System.Linq;
using System;

public class GOAPState
{
    public Dictionary<string, object> values = new Dictionary<string, object>();
    public WorldState currentState;
    public GOAPAction generatingAction = null;
    public int step = 0;

    public string myNameState;

    #region CONSTRUCTOR
    public GOAPState(string _myNameState = "default")
    {
        currentState = new WorldState();
        //generatingAction = gen;
        myNameState = _myNameState;
    }

    public GOAPState(GOAPState source, string _myNameState = "default")//NUEVO CONSTRUCTOR
    {
        //directamente piso todo el objeto WorldState, esto se va a usar para crear nuevos estados con la refe del estado del mundo anterior
        currentState = source.currentState;
        myNameState = _myNameState;
    }

    public void RefreshStateWithNewEffects(Dictionary<string, Action<WorldState>> new_values)
    {
        foreach(var values in new_values)
        {
            new_values[values.Key].Invoke(currentState);
        }
    }

    public GOAPState(WorldState current_state)
    {
        currentState = current_state;
        generatingAction = null;
    }

    #endregion

    public override bool Equals(object obj)
    {
        var other = obj as GOAPState;
        var result =
            other != null
            && other.generatingAction == generatingAction       //Very important to keep! TODO: REVIEW
            && other.currentState.register.Count == currentState.register.Count
            && other.currentState.register.All(kv => kv.IsInTheDictionary(currentState.register));
        //&& other.values.All(kv => values.Contains(kv));
        return result;
    }

    public override int GetHashCode()
    {
        //Better hashing but slow.
        //var x = 31;
        //var hashCode = 0;
        //foreach(var kv in values) {
        //	hashCode += x*(kv.Key + ":" + kv.Value).GetHashCode);
        //	x*=31;
        //}
        //return hashCode;

        //Heuristic count+first value hash multiplied by polynomial primes
        return currentState.register.Count == 0 ? 0 : 31 * currentState.register.Count + 31 * 31 * currentState.register.First().GetHashCode();
    }

    public override string ToString()
    {
        var str = "";
        foreach (var kv in values.OrderBy(x => x.Key))
        {
            str += $"{kv.Key:12} : {kv.Value}\n";
        }
        return "--->" + (generatingAction != null ? "<b>"+generatingAction.name : "<b>NULL") + "</b>" + str;
    }
}
