using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : LivingEntity
{
    Threat mythreat;

    protected override void OnInitialize()
    {
        mythreat = GetComponent<Threat>();
        base.OnInitialize();
        mythreat.Initialize();
        mythreat.Rise();
        
    }
    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
        mythreat.Deinitialize();
    }
    protected override void OnTick(float DeltaTime)
    {
        base.OnTick(DeltaTime);
        mythreat.Tick(DeltaTime);
    }


    protected override void Feedback_ReceiveDamage() { }
    protected override void Feedback_OnHeal() { }
    protected override void OnDeath()
    {
        mythreat.Death();
        GameLoop.Pause();
        GameLoop.Lose();
    }
}
