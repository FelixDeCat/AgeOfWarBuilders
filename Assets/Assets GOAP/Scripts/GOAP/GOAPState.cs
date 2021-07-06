using System.Collections.Generic;
using System.Linq;

public class GOAPState
{
    public Dictionary<string, bool> values = new Dictionary<string, bool>();
    public GOAPAction generatingAction = null;
    public int step = 0;

    #region CONSTRUCTOR
    public GOAPState(GOAPAction gen = null)
    {
        generatingAction = gen;
    }

    public GOAPState(GOAPState source, GOAPAction gen = null)
    {
        foreach (var elem in source.values)
        {
            if (values.ContainsKey(elem.Key))
                values[elem.Key] = elem.Value;
            else
                values.Add(elem.Key, elem.Value);
        }
        generatingAction = gen;
    }
    #endregion

    public override bool Equals(object obj)
    {
        var other = obj as GOAPState;
        var result =
            other != null
            && other.generatingAction == generatingAction       //Very important to keep! TODO: REVIEW
            && other.values.Count == values.Count
            && other.values.All(kv => kv.In(values));
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
        return values.Count == 0 ? 0 : 31 * values.Count + 31 * 31 * values.First().GetHashCode();
    }

    public override string ToString()
    {
        var str = "";
        foreach (var kv in values.OrderBy(x => x.Key))
        {
            str += $"{kv.Key:12} : {kv.Value}\n";
        }
        return "--->" + (generatingAction != null ? generatingAction.name : "NULL") + "\n" + str;
    }
}
