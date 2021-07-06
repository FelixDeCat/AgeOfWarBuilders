using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlaceFinderCombat : MonoBehaviour
{
    float timer;
    float timer_recalculate;
    [SerializeField] float cd_execute = 1f;
    [SerializeField] float cd_recalculate = 0.5f;
    bool execute;

    bool active;

    [SerializeField] Villager villager;
    [SerializeField] GenericInteractor interactor;
    [SerializeField] CircleQuery QuerieResourceFinder;

    public const string REST = "rest";

    public void ConfigureFilter()
    {
        //le digo al interactor que me filtre y me guarde los resources del tipo que quiero
        interactor.AddFilter(REST, x =>
        {
            var r = x.GetComponent<PlaceToCombat>();
            if (r != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        });
    }

    public void FindPlace()
    {
        var col = interactor.GetFilteredCollection(REST);
        if (col.Count <= 0)
        {
            var place = QuerieResourceFinder.Query()
                .OfType<GridComponent>()
                .Where(x => x.Grid_Object.GetComponent<PlaceToCombat>())
                .Select(x => x.Grid_Object.GetComponent<PlaceToCombat>())
                .FirstOrDefault();

            if (place)
            {
                Debug.Log(":) El place que encontre es::: " + place.gameObject.name);

                villager.transform.position = place.transform.position;
                BeginCombat();
            }
            else
            {
                Debug.LogWarning("Ni el interactor, ni el Query encontraron algo");
            }
        }
        else
        {
            BeginCombat();
        }
    }

    public void Initialize()
    {
        active = true;
        ConfigureFilter();
        QuerieResourceFinder.Configure(villager.transform);
    }
    public void Deinitialize()
    {
        active = false;
        timer = 0;
        timer_recalculate = 0;
        execute = false;
    }

    public void BeginCombat()
    {
        execute = true;
        timer = 0;
    }
    public void EndCombat()
    {
        execute = false;
        timer = 0;
    }

    void Update_RecalculatePulseToBeginBehaviour()
    {
        if (timer_recalculate < cd_recalculate)
        {
            timer_recalculate = timer_recalculate + 1 * Time.deltaTime;
            var col = interactor.GetFilteredCollection(villager.GetProfession().ToString());
            if (col.Count > 0)
            {
                execute = true;
            }
            else
            {
                execute = false;
            }
        }
        else
        {
            timer_recalculate = 0;
        }
    }
    void Update_ExecuteBehaviour()
    {
        if (execute)
        {
            if (timer < cd_execute)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                var place = interactor
                    .GetFilteredCollection(REST)
                    .Select(x => x.GetComponent<PlaceToCombat>())
                    .FirstOrDefault();

                //ACA LO HAGO DESCANZAR

                //villager descanzo++

                timer = 0;
            }
        }
    }

    private void Update()
    {
        if (!active) return;
        Update_RecalculatePulseToBeginBehaviour();
        Update_ExecuteBehaviour();
    }
}
