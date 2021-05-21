using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public abstract class MonoState<T> : MonoBehaviour
{
    Action<T> callbackInput;
    public virtual MonoState<T> Configure(Action<T> action) { callbackInput = action; return this; }
    protected virtual void SendInput(T input) => callbackInput.Invoke(input);
    public virtual void Enter(EState<T> lastState) { }
    public virtual void Tick() { }
    public virtual void LateTick() { }
    public virtual void FixedTick() { }
    public virtual void Exit(T input) { }
}
