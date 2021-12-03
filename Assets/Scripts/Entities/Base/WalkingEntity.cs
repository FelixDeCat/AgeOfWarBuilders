using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WalkingEntity : PlayObject
{
    [Header("--- Walking Entity Vars ---")]
    [SerializeField] protected bool block_pathfinder_execution = false;
    protected bool executeAStar;
    public void StopPathFinder() => block_pathfinder_execution = true;
    public void PlayPathFinder() => block_pathfinder_execution = false;

    //IA2-P1 [PathFinding]
    [SerializeField] protected PathFinder rig_path_finder;
    [SerializeField] protected Rigidbody myRig;

    float lerp_timer;
    [SerializeField] float lerp_pos_time = 1;
    bool lerp_pos;
    Transform lerp_transform;
    Action EndLerp = delegate { };

    public void LerpPosRot(Transform trans, Action _EndLerp)
    {
       // CanNotUsePathFinder = true;
        lerp_pos = true;
        lerp_transform = trans;
        EndLerp = _EndLerp;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        InitializePathFinder(myRig);
    }

    protected override void OnTick(float DeltaTime)
    {
        base.OnTick(DeltaTime);

        if (lerp_pos)
        {
            if (lerp_timer < lerp_pos_time)
            {
                lerp_timer = lerp_timer + 1 * Time.deltaTime;
                rig_path_finder.transform.position = Vector3.Lerp(rig_path_finder.transform.position, lerp_transform.position, lerp_timer);
                rig_path_finder.transform.eulerAngles = Vector3.Lerp(rig_path_finder.transform.eulerAngles, lerp_transform.eulerAngles, lerp_timer);
            }
            else
            {
                EndLerp.Invoke();
                lerp_timer = 0;
                lerp_pos = false;
                lerp_transform = null;
                //CanNotUsePathFinder = false;
            }
        }

        if (block_pathfinder_execution) return;
        if (executeAStar) { rig_path_finder.Refresh(); }
    }

    public void InitializePathFinder(Rigidbody rb) { if (rig_path_finder) rig_path_finder.Initialize(rb); }
    public virtual void Callback_IHaveArrived(Action EndArrived) { rig_path_finder.AddCallbackEnd(EndArrived); }
    public bool GoToPositionWithPathFinder(Vector3 pos) { var canExecute = rig_path_finder.Execute(pos); executeAStar = true; return canExecute; }
    public void Stop() { executeAStar = false; rig_path_finder.StopMovement(); }
}
