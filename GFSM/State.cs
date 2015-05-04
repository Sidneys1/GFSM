using System.Linq;

namespace GFSM {

    public abstract class State {

		#region Properties

		public StateMode Mode { get; private set; }

        public abstract Transition[] ToThisTransitions { get; }

        public abstract Transition[] FromThisTransitions { get; }

		#endregion Properties


		#region Constructors

		protected State(StateMode initialMode) {
            Mode = initialMode;
		}

		#endregion Constructors


		#region Methods

		public void TransitionFrom(Transition transition) {
            if (!FromThisTransitions.Contains(transition))
                throw new InvalidTransitionException($"Transition '{transition}' is invalid.");

            switch (transition.Command) {
                case Command.Deactivate:
                    Exit();
                    break;

                case Command.Pause:
                    Pause();
                    break;
            }
        }

        public void TransitionTo(Transition transition) {
            if (!ToThisTransitions.Contains(transition))
                throw new InvalidTransitionException($"Transition '{transition}' is invalid.");

            Enter();
        }

        public virtual void Enter() {
        }

        public virtual void Pause() {
        }

        public virtual void Exit() {
		}

		#endregion Methods
	}
}