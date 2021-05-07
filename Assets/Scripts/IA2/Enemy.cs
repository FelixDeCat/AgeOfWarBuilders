using System;
using UnityEngine;
using AgeOfWarBuilders.Entities;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Enemy : LivingEntity, IGridEntity
{
    #region Grid Things
    public event Action<IGridEntity> OnMove;
    public Vector3 Position
    {
        get {
            if (transform != null)
                return transform.position;
            else
                return new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        }
        set => transform.position = value;
    } 

    MeshChangeColorCollection colorDebug;

    public LayerMask mypartersLayer;
    public float partner_detection_radius = 5f;

    bool isAlive;
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    #endregion

    protected override void OnInitialize()
    {
        if (colorDebug == null) colorDebug = GetComponent<MeshChangeColorCollection>();
        base.OnInitialize();
        colorDebug.Change(Color.cyan);
        Resurrect();
        isAlive = true;
        SpatialGrid.instance.AddEntityToGrid(this);
        //OnMove.Invoke(this);
    }
    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
        colorDebug.Change(Color.magenta);
        SpatialGrid.instance.RemoveEntityToGrid(this);
    }

    Action<Enemy> CbkOnDeath;
    public void CallbackOnDeath(Action<Enemy> callback)
    {
        CbkOnDeath = callback;
    }

    protected override void OnTick(float DeltaTime)
    {
        base.OnTick(DeltaTime);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        colorDebug.Change(Color.black);
        Invoke("AnimDeath", 1f);

    }
    void AnimDeath()
    {
        isAlive = false;
        CbkOnDeath.Invoke(this);
    }

    public IEnumerable<Enemy> GetPartners()
    {
        return Physics
            .OverlapSphere(this.transform.position, 5f, mypartersLayer)
            .Select(x => x.GetComponent<Enemy>());
    }

    public int GetPartnersLegth()
    {
        return Physics
            .OverlapSphere(this.transform.position, 5f, mypartersLayer).Length;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, partner_detection_radius);
    }


}