using System;
using UnityEngine;
using AgeOfWarBuilders.Entities;

public class Enemy : LivingEntity, IGridEntity
{
    #region Grid Things
    public event Action<IGridEntity> OnMove;
    public event Action<IGridEntity> OnRemove;
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
    #endregion

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Debug.Log("Initialize Enemy");
        
    }

    Action<Enemy> CbkOnDeath;
    public void CallbackOnDeath(Action<Enemy> callback)
    {
        CbkOnDeath = callback;
    }
    public void Respawn()
    {
        Resurrect();
    }

    protected override void OnTick(float DeltaTime)
    {
        base.OnTick(DeltaTime);
    }
    protected override void OnDeath()
    {
        base.OnDeath();

        CbkOnDeath.Invoke(this);

    }

    
}