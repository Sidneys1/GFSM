namespace GFSM {
    /// <summary>
    /// Represents a FSM state.
    /// </summary>
    public interface IState {
        /// <summary>
        /// Is called when this state is transitioned into
        /// </summary>
        void Enter();

        /// <summary>
        /// Is called when this state is transitioned out of
        /// </summary>
        void Leave();
    }
}