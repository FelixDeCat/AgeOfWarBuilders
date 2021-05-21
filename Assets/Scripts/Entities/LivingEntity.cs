using System;
using UnityEngine;
using Tools.Structs;


public class LivingEntity : WalkingEntity
{
    [Header("--- Living Entity Vars ---")]
    [SerializeField] int initial_hp = 100;
    [SerializeField] LifeSystemBase life;
    [SerializeField] FrontendStatBase ui_life;

    public float HP { get => life.Life; }

    protected override void Awake()
    {
        base.Awake();
    }

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

    protected virtual void Feedback_ReceiveDamage() { }
    protected virtual void Feedback_OnHeal() { }
    protected virtual void OnDeath()
    {
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Debug.Log("Initialize LivingEntity [" + this.gameObject.name + "]");
        life = new LifeSystemBase(
            initial_hp,
            Feedback_ReceiveDamage,
            Feedback_OnHeal,
            OnDeath,
            ui_life,
            initial_hp);
    }

    
}