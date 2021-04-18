using System.Collections.Generic;
using System;

namespace Tools.StateMachine
{
	public class EventStateMachine<T>
	{
		public bool Active
		{
			get;

			set;
		}

		private EState<T> current;
		Action<string> debug = delegate { };
		public EventStateMachine(EState<T> initial, Action<string> _debug = null)
		{
			debug = _debug;
			current = initial;
			current.Enter(null);
			Active = true;
		}
		public void SendInput(T input)
		{
			if (!Active) return;

			EState<T> newState;
			if (current.CheckInput(input, out newState))
			{
				current.Exit(input);
				var oldState = current;
				current = newState;
				debug?.Invoke(current.Name);
				current.Enter(oldState);
			}
		}

		public EState<T> Current { get { return current; } }
		public void Update() { if (!Active) return; current.Update(); }
		public void LateUpdate() { if (!Active) return; current.LateUpdate(); }
		public void FixedUpdate() { if (!Active) return; current.FixedUpdate(); }

	}
}
