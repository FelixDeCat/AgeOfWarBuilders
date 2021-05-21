using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.StateMachine;
using System;

namespace Tools.StateMachine
{
    public abstract class StatesFunctionsMonoBehaviour<T> : MonoBehaviour
    {
        Action<T> callbackInput;
        public virtual void Configure(EState<T> myState, Action<T> CallbackSendInput)
        {
            myState.OnEnter += Enter;
            myState.OnUpdate += Tick;
            myState.OnLateUpdate += LateTick;
            myState.OnFixedUpdate += FixedTick;
            myState.OnExit += Exit;
            callbackInput = CallbackSendInput;
        }
        protected virtual void SendInput(T input) => callbackInput.Invoke(input);
        protected virtual void Enter(EState<T> lastState) { }
        protected virtual void Tick() { }
        protected virtual void LateTick() { }
        protected virtual void FixedTick() { }
        protected virtual void Exit(T input) { }
    }
}
