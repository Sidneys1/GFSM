namespace GFSM {
    /// <summary>
    /// Represents a FSM state.
    /// </summary>
    /// <typeparam name="T">The type of the state</typeparam>
    public abstract class State<T> where T : State<T> {
        #region Properties

        public FiniteStateMachine<T> StateMachine { get; }

        #endregion Properties
        
        #region Constructors

        protected State(FiniteStateMachine<T> stateMachine) {
            StateMachine = stateMachine;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Is called when this state is transitioned into
        /// </summary>
        public virtual void Enter() {
        }

        public override string ToString() {
            return GetType().Name;
        }

        #endregion Methods
    }
}