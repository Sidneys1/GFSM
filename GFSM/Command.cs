namespace GFSM {

    public enum Command {

        /// <summary>
        /// Deativates the 'from' State
        /// </summary>
        Deactivate,

        /// <summary>
        /// Pauses the 'from' State, allowing it to resume instead of start anew.
        /// </summary>
        Pause
    }
}