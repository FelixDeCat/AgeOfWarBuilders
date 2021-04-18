using System;

namespace Tools.StateMachine
{
    public class TransitionState<T>
    {
        T input;

        public T Input { get { return input; } }

        EState<T> targetState;
        public EState<T> TargetState { get { return targetState; } }

        //
        public event Action<T> OnTransition = delegate { };
        public void OnTransitionExecute(T input) { OnTransition(input); }

        // Su constructor
        public TransitionState(T recieveInput, EState<T> state)
        {
            input = recieveInput;
            targetState = state;
        }
    }
}
