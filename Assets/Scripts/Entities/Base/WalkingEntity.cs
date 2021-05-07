using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WalkingEntity : PlayObject
{
    [Header("--- Walking Entity Vars ---")]
    [SerializeField] bool CanNotWalk = false;
    private bool executeAStar;

    [SerializeField] protected RigidbodyPathFinder rig_path_finder;
    [SerializeField] protected Rigidbody myRig;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        InitializePathFinder(myRig);
    }

    protected override void OnTick(float DeltaTime)
    {
        base.OnTick(DeltaTime);
        if (!CanNotWalk) return;
        if (executeAStar) { rig_path_finder.Refresh(); }
    }

    public void InitializePathFinder(Rigidbody rb) { if (rig_path_finder) rig_path_finder.Initialize(rb); }
    protected virtual void Callback_IHaveArrived(Action EndArrived) { rig_path_finder.AddCallbackEnd(EndArrived); }
    public void GoToPosition(Vector3 pos) { rig_path_finder.Execute(pos); executeAStar = true; }
    public void Stop() { executeAStar = false; rig_path_finder.StopMovement(); }
}
