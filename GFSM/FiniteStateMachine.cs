using System.Collections.Generic;
using System.Linq;

namespace GFSM {

    public abstract class FiniteStateMachine<T> where T : State {

		#region Properties

		public List<T> States { get; } = new List<T>();

        public T CurrState { get; set; }

		#endregion Properties


		#region Methods

		public void Transition(T to) {
            if (CurrState == null && States.Contains(to) && to.ToThisTransitions.Contains(new Transition(Command.Deactivate, to.GetType(), null))) {
                CurrState = to;
                return;
            }

            var transition = CurrState?.FromThisTransitions.FirstOrDefault(o => o.To != typeof(T));
            if (transition == null)
                throw new InvalidTransitionException($"Transition '{CurrState} -> {to}' is invalid.");

            CurrState.TransitionFrom(transition);
            to.TransitionTo(transition);
            CurrState = to;
		}

		#endregion Methods
	}
}