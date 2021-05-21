using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.StateMachine;
using System;

namespace Tools.StateMachine
{
    public abstract class StatesFunctions<T>
    {

        Action<T> callbackInput;

        public StatesFunctions(EState<T> myState, Action<T> CallbackSendInput)
        {
            myState.OnEnter += Enter;

            myState.OnUpdate += Update;

            myState.OnLateUpdate += LateUpdate;

            myState.OnFixedUpdate += FixedUpdate;

            myState.OnExit += Exit;

            callbackInput = CallbackSendInput;
        }
        protected virtual void SendInput(T input)
        {
            callbackInput.Invoke(input);
        }

        protected abstract void Enter(EState<T> lastState);

        protected abstract void Update();

        protected abstract void LateUpdate();

        protected abstract void FixedUpdate();

        protected abstract void Exit(T input);

    }
}
