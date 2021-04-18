using UnityEngine;
using System;

public class LifeSystemBase
{
    LifeBase life;
    FrontendStatBase uilife; //monovehaviour que hay que pasarle por constructor

    public void Config(int life_count_Max, Action OnLoseLife, Action OnGainLife, Action OnDeath, FrontendStatBase _uilife, int initial_life = -1)
    {
        uilife = _uilife;
        life = new LifeBase(life_count_Max, uilife, initial_life);
        life.AddEventListener_GainLife(OnGainLife);
        life.AddEventListener_LoseLife(OnLoseLife);
        life.AddEventListener_Death(OnDeath);
    }
    public void Config(int life_count_Max, Action OnLoseLife, Action OnGainLife, Action OnDeath, int initial_life = -1)
    {
        life = new LifeBase(life_count_Max, initial_life);
        life.AddEventListener_GainLife(OnGainLife);
        life.AddEventListener_LoseLife(OnLoseLife);
        life.AddEventListener_Death(OnDeath);
    }

    public void AddCallback_LifeChange(Action<int, int> callback)
    {
        life.AddEventListener_OnLifeChange(callback);
    }

    public float Life
    {
        get
        {
            if (life != null)
            {
                return life.Val;
            }
            else
            {
                return 0;
            }
        }
    }

    public void SetCurrentLifeNoMax(int currentlife) => life.Val = currentlife;
    public int GetMax() => life.MaxVal;
    public void Hit() => life.Val--;
    public bool Hit(int val) {  life.Val -= val; return life.Val <= 0; }
    public void AddHealth() => life.Val++;
    public void AddHealth(int val) => life.Val += val;
    public bool CanHealth() => life.Val < life.MaxVal;
    public void IncreaseLife() => life.IncreaseValue(1);
    public void IncreaseLife(int val) => life.IncreaseValue(val);
    public void SetLife(int newMax) => life.SetValue(newMax);
    public void ResetLife() => life.ResetValueToMax();
    public override string ToString() => life.Val.ToString();
}
