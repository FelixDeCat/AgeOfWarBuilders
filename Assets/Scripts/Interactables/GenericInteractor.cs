using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GenericInteractor : MonoBehaviour
{
    HashSet<GenericInteractable> interactables = new HashSet<GenericInteractable>();
    GenericInteractable most_close;

    Func<GenericInteractable, bool> Predicate = delegate { return true; };

    [SerializeField] float safe_time_cd = 0.1f;

    bool isStoped;

    public void AddInteractable(GenericInteractable i) { StopCoroutine(Refresh()); interactables.Add(i); StartCoroutine(Refresh()); }
    public void RemoveInteractable(GenericInteractable i) { StopCoroutine(Refresh()); interactables.Remove(i); StartCoroutine(Refresh()); }

    private void OnDestroy()
    {
        StopCoroutine(Refresh());
    }

    Dictionary<string, Tuple<HashSet<GenericInteractable>, Func<GenericInteractable, bool>>> filters = new Dictionary<string, Tuple<HashSet<GenericInteractable>, Func<GenericInteractable, bool>>>();

    public IEnumerator GetInteractorsByPredicate(Func<GenericInteractable, bool> Pred)
    {
        float distance = float.MaxValue;
        foreach (var i in interactables)
        {
            var currentdistance = Vector3.Distance(this.transform.position, i.transform.position);

            if (currentdistance < distance && Pred(i))
            {
                distance = currentdistance;
                most_close = i;
            }
            yield return null;
        }
    }

    public void AddFilter(string filterName, Func<GenericInteractable, bool> predicate)
    {
        if (!filters.ContainsKey(filterName))
        {
            HashSet<GenericInteractable> col = new HashSet<GenericInteractable>();
            filters.Add(filterName, Tuple.Create(col, predicate));
        }
    }
    public HashSet<GenericInteractable> GetFilteredCollection(string filterName)
    {
        if (filters.ContainsKey(filterName))
        {
            return filters[filterName].Item1;
        }
        else return null;

    }

    public void InitializeInteractor()
    {
        StartCoroutine(Refresh());
    }
    public void DeinitializeInteractor()
    {
        StopCoroutine(Refresh());
    }

    public void ConfigurePredicate(Func<GenericInteractable, bool> predicate) => this.Predicate = predicate;
    public void RemovePredicate(Func<GenericInteractable, bool> predicate) => this.Predicate = delegate { return true; };

    float lastInterval;
    IEnumerator Refresh()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            foreach (var f in filters)
            {
                f.Value.Item1.Clear();
            }

            float distance = float.MaxValue;
            foreach (var i in interactables)
            {
                var currentdistance = Vector3.Distance(this.transform.position, i.transform.position);

                foreach (var f in filters)
                {
                    if (f.Value.Item2(i)) //si cumple con algunos de los filtros lo agrego a la base de datos
                    {
                        f.Value.Item1.Add(i);
                    }
                }

                if (currentdistance < distance)
                {
                    distance = currentdistance;
                    most_close = i;
                }
                //yield return null;
            }
        }
    }

    public void Execute()
    {
        if (most_close != null)
        {
            Debug.Log("Estoy input");
            most_close.Execute();
        }
    }
}
