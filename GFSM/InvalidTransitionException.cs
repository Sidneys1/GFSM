using System;

namespace GFSM {
    public class InvalidTransitionException : Exception {
        #region Constructors

        public InvalidTransitionException(string message) : base(message) {
        }

        #endregion Constructors
    }
}