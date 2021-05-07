using System;
using UnityEngine;
using AgeOfWarBuilders.Entities;
using Tools.Structs;

public class LivingEntity : WalkingEntity
{
    [Header("--- Living Entity Vars ---")]
    [SerializeField] int initial_hp = 100;
    [SerializeField] LifeSystemBase life;
    [SerializeField] FrontendStatBase ui_life;

    public float HP { get => life.Life; }

    public void ReceiveDamage(int dmg = 5)
    {
        life.Hit(dmg);
    }
    public void ReceiveDamage(Damage damage)
    {
        life.Hit(damage.Physical_damage);
    }
    public void Resurrect()
    {
        life.ResetLife();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Debug.Log("Initialize LivingEntity [" + this.gameObject.name + "]");
        life = new LifeSystemBase(
            initial_hp,
            () => { /*OnLose*/ },
            () => { /*OnGain*/ },
            OnDeath,
            ui_life,
            initial_hp);
    }

    protected virtual void OnDeath()
    {
        //GameLoop.RemoveObject(this);
        //transform.position = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        //Destroy(this);
    }
}