using System;
using System.Collections.Generic;

namespace Tools.StateMachine
{
    public class ConfigureState<T>
    {
		EState<T> _state;

		Dictionary<T, TransitionState<T>> transitions = new Dictionary<T, TransitionState<T>>();
        Dictionary<T, Action> transitionAction = new Dictionary<T, Action>();

		public ConfigureState(EState<T> state)
        {
            _state = state;
        }

        public ConfigureState<T> SetTransition(T input, EState<T> target, Action actionInput = null)
        {
            transitions.Add(input, new TransitionState<T>(input, target));

            if (actionInput != null)
                transitionAction.Add(input, actionInput);

            return this;
        }

		public void Done()
        {
            _state.Configure(transitions, transitionAction);
        }
    }

    public static class ConfigureState
    {
        public static ConfigureState<T> Create<T>(EState<T> state)
        {
            return new ConfigureState<T>(state);
        }
    }
}
