using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WalkingEntity : PlayObject
{
    [Header("--- Walking Entity Vars ---")]
    [SerializeField] protected bool CanNotWalk = false;
    protected bool executeAStar;

    [SerializeField] protected PathFinder rig_path_finder;
    [SerializeField] protected Rigidbody myRig;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        InitializePathFinder(myRig);
    }

    protected override void OnTick(float DeltaTime)
    {
        base.OnTick(DeltaTime);
        if (CanNotWalk) return;
        if (executeAStar) { rig_path_finder.Refresh(); }
    }

    public void InitializePathFinder(Rigidbody rb) { if (rig_path_finder) rig_path_finder.Initialize(rb); }
    protected virtual void Callback_IHaveArrived(Action EndArrived) { rig_path_finder.AddCallbackEnd(EndArrived); }
    public bool GoToPosition(Vector3 pos) { var canExecute = rig_path_finder.Execute(pos); executeAStar = true; return canExecute; }
    public void Stop() { executeAStar = false; rig_path_finder.StopMovement(); }
}
