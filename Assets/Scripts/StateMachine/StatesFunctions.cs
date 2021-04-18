using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.StateMachine;
using System;

namespace Tools.StateMachine
{
    public abstract class StatesFunctions<T>
    {
        protected EventStateMachine<T> sm;
        protected EState<T> state;

        public StatesFunctions(EState<T> myState, EventStateMachine<T> _sm)
        {
            myState.OnEnter += Enter;

            myState.OnUpdate += Update;

            myState.OnLateUpdate += LateUpdate;

            myState.OnFixedUpdate += FixedUpdate;

            myState.OnExit += Exit;

            sm = _sm;
            state = myState;
        }

        protected abstract void Enter(EState<T> lastState);

        protected abstract void Update();

        protected abstract void LateUpdate();

        protected abstract void FixedUpdate();

        protected abstract void Exit(T input);

    }
}
