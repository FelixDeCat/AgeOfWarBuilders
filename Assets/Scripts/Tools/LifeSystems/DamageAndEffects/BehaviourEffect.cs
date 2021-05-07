using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BehaviourEffect
{
    Action<BehaviourEffect> OnEndEffect;
    public BehaviourEffect(Action<BehaviourEffect> callback_End)
    {
        OnEndEffect = callback_End;
    }
    public void StartEffect()
    {
        OnStartEffect();
    }
    public void StopEffect()
    {
        OnStopEffect();
    }
    public void TickEffect(float DeltaTime)
    {
        Tick(DeltaTime);
    }

    protected abstract void OnStartEffect();
    protected abstract void OnStopEffect();
    protected abstract void Tick(float DeltaTime);
}
