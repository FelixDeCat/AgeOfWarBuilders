using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlaceFinder : MonoBehaviour
{
    float timer;
    float timer_recalculate;
    [SerializeField] float cd_harvest = 1f;
    [SerializeField] float cd_recalculate = 0.5f;
    bool execute;

    bool active;

    [SerializeField] Villager villager;
    [SerializeField] GenericInteractor interactor;
    [SerializeField] CircleQuery QuerieResourceFinder;

    PlaceToAction current;

    Func<PlaceToAction, bool> predicate = delegate { return true; };
    Action Exe = delegate { };

    public void ReconfigurePredicate(Func<PlaceToAction, bool> pred)
    {
        predicate = pred;
    }

    public void ReconfigureExecution(Action _Execution)
    {
        Exe = _Execution;
    }

    void CalculatePlace()
    {
        current = QuerieResourceFinder.Query()
                .OfType<GridComponent>()
                .Where(x => x.Grid_Object.GetComponent<PlaceToAction>())
                .Select(x => x.Grid_Object.GetComponent<PlaceToAction>())
                .Where(x => predicate(x))
                .OrderBy(x => Vector3.Distance(x.transform.position, this.transform.position))
                .First();
    }

    public void Go_To_Execute_Action()
    {
        current = null;
        CalculatePlace();
        villager.GoToPositionWithPathFinder(current.Position_to_Action.position);
        villager.Callback_IHaveArrived(() => { villager.LerpPosRot(current.Position_to_Action, BeginExecution); villager.view.PLAY_ANIM_Idle(); });
    }

    public void StopWork()
    {
        active = false;
        timer = 0;
        EndHarvest();
    }

    public void Initialize()
    {
        active = true;
        CalculatePlace();
        QuerieResourceFinder.Configure(villager.transform);
    }
    public void Deinitialize()
    {
        active = false;
        timer = 0;
        timer_recalculate = 0;
        execute = false;
    }

    public void BeginExecution()
    {


        if (execute) return;

        Exe.Invoke();
        current.OnExecuted();

        //if (current.isInstantOneShot)
        //{
        //    Exe.Invoke();
        //    current.OnExecuted();
        //}
        //else
        //{
        //    execute = true;
        //    timer = 0;
        //}
    }
    public void EndHarvest()
    {
        execute = false;
        timer = 0;
    }

    //private void Update()
    //{
    //    if (!active) return;

    //    if (execute)
    //    {
    //        if (timer < cd_harvest)
    //        {
    //            timer = timer + 1 * Time.deltaTime;
    //        }
    //        else
    //        {
    //            if (current)
    //            {
    //                Exe.Invoke();
    //                current.OnExecuted();
    //                timer = 0;
    //            }
    //        }
    //    }
    //}
}
