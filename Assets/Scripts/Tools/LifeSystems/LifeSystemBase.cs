using UnityEngine;
using System;

public class LifeSystemBase
{
    LifeBase life;
    FrontendStatBase uilife; //monovehaviour que hay que pasarle por constructor

    public LifeSystemBase(int life_count_Max, Action OnLoseLife, Action OnGainLife, Action OnDeath, FrontendStatBase _uilife, int initial_life)
    {
        uilife = _uilife;
        life = new LifeBase(life_count_Max, uilife, initial_life);
        life.AddEventListener_GainLife(OnGainLife);
        life.AddEventListener_LoseLife(OnLoseLife);
        life.AddEventListener_Death(OnDeath);
    }
    public LifeSystemBase(int life_count_Max, Action OnLoseLife, Action OnGainLife, Action OnDeath, int initial_life)
    {
        life = new LifeBase(life_count_Max, initial_life);
        life.AddEventListener_GainLife(OnGainLife);
        life.AddEventListener_LoseLife(OnLoseLife);
        life.AddEventListener_Death(OnDeath);
    }
    public LifeSystemBase(int life_count_Max, Action OnLoseLife, Action OnGainLife, Action OnDeath, Action<float, float> change, int initial_life)
    {
        life = new LifeBase(life_count_Max, initial_life);
        life.AddEventListener_GainLife(OnGainLife);
        life.AddEventListener_LoseLife(OnLoseLife);
        life.AddEventListener_Death(OnDeath);
        life.AddEventListener_OnLifeChange(change);
    }
    public LifeSystemBase(int life_count_Max, Action OnLoseLife, Action OnGainLife, Action OnDeath, Action OnCanNotAddMore, Action OnCanNotRemoveMore, int initial_life)
    {
        life = new LifeBase(life_count_Max, uilife, initial_life);
        life.AddEventListener_GainLife(OnGainLife);
        life.AddEventListener_LoseLife(OnLoseLife);
        life.AddEventListener_Death(OnDeath);
        life.AddEventListener_CannotAddMore(OnCanNotAddMore);
        life.AddEventListener_CannotRemoveMore(OnCanNotRemoveMore);
    }
    public LifeSystemBase(int life_count_Max, Action OnLoseLife, Action OnGainLife, Action OnDeath, Action<float, float> change, Action OnCanNotAddMore, Action OnCanNotRemoveMore, int initial_life)
    {
        life = new LifeBase(life_count_Max, uilife, initial_life);
        life.AddEventListener_GainLife(OnGainLife);
        life.AddEventListener_LoseLife(OnLoseLife);
        life.AddEventListener_Death(OnDeath);
        life.AddEventListener_CannotAddMore(OnCanNotAddMore);
        life.AddEventListener_CannotRemoveMore(OnCanNotRemoveMore);
        life.AddEventListener_OnLifeChange(change);
    }

    ///////////////////////////////////////////////////////////////////////////////
    /// GETTERS
    ///////////////////////////////////////////////////////////////////////////////
    public float Life                           => life.Value;
    public int Max                              => life.MaxVal;
    public bool CanHealth()                     => life.Value < life.MaxVal;
    public bool CanThisBlowKillHim(int val)     => life.Value - val <= 0;
    public override string ToString()           => life.Value.ToString();
    public bool IsFull()                        => life.Value >= life.MaxVal;
    public bool IsEmpty() => life.Value <= 0;

    ///////////////////////////////////////////////////////////////////////////////
    /// SETTERS
    ///////////////////////////////////////////////////////////////////////////////

    //Sets that modify life value
    public void SetLife(int currentlife)        => life.Value = currentlife;
    public void ResetLife()                     => life.ResetValueToMax();

    //Sets that modify the Maximum
    public void SetNewMaxLife(int newMax)       => life.SetValue(newMax);
    public void IncreaseLife()                  => life.IncreaseValue(1);
    public void IncreaseLife(int val)           => life.IncreaseValue(val);

    //Hit and Heal
    public void Hit()                           => life.Value--;
    public void Hit(int val)                    => life.Value -= val;
    public void Heal()                          => life.Value++;
    public void Heal(int val)                   => life.Value += val;

}
