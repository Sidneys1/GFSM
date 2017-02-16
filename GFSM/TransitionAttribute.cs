using System;

namespace GFSM {
    /// <summary>
    /// Used to mark IStates with their valid transitions
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class TransitionAttribute : Attribute {
        public TransitionAttribute(string trigger, Type toState, Mode mode = Mode.Push) {
            Trigger = trigger;
            ToState = toState;
            Mode = mode;
        }
        public string Trigger { get; }
        public Type ToState { get; }
        public Mode Mode { get; }
    }
}