using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.Structs;
using System.Linq;

public class PlayerDamageComponent : PlayerComponent
{

    protected override void OnInitialized()
    {
        base.OnInitialized();
        query.Configure(GetOwnerTransform);
    }

    #region TICK
    protected override void OnTick(float DeltaTime)
    {
        Update_CD(DeltaTime);
    }
    #endregion

    #region [LOGIC] Execution logic
    float timer;
    public float CD = 0.1f;
    bool CD_Is_Active;
    public bool DoDamage()
    {
        if (!CanUseThisComponent) return false;

        if (CD_Is_Active) return false;
        DamageLogic();
        CD_Is_Active = true;
        return true;
    }
    void Update_CD(float DeltaTime)
    {
        if (CD_Is_Active)
        {
            if (timer < CD)
            {
                timer = timer + 1 * DeltaTime;
            }
            else
            {
                CD_Is_Active = false;
                timer = 0;
            }
        }
    }
    #endregion

    #region [LOGIC] Deliver Damage
    [SerializeField] Damage damage;
    [SerializeField] ObserverQuery query;
    void DamageLogic()
    {
        if (query == null) { throw new Exception("[ERROR] I do not have a " + ObserverQuery.Static_ToString()); }

        var col = query
            .Query()
            .OfType<Enemy>()
            .DefaultIfEmpty();

        foreach (var e in col)
        {
            if (e != null)
            {
                Debug.Log("Do DAMAGE: {"+ damage.Physical_damage + "} => " + e.gameObject.name);
                e.ReceiveDamage(damage);
            }
                
        }
    }
    #endregion
}
