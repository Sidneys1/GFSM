using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MoreLinq;

namespace GFSM {
    /// <summary>
    /// Describes a Deterministic Finite State Machine
    /// </summary>
    /// <typeparam name="TInitialState">The starting IState of this FSM</typeparam>
    public class FiniteStateMachine<TInitialState> where TInitialState : class, IState, new() {
        #region Classes

        /// <summary>
        /// Represents a transition from one state to another
        /// </summary>
        public class Transition {
            #region Fields

            private readonly int _hash;

            #endregion Fields

            #region Properties

            public string Token { get; }

            public IState To { get; }

            public IState From { get; }

            public Mode TransitionMode { get; }

            #endregion Properties

            #region Constructors

            public Transition(string token, IState frm, IState to, Mode tMode = Mode.Push) {
                Token = token;
                To = to;
                From = frm;
                TransitionMode = tMode;
                _hash = Token.GetHashCode() ^ To?.GetHashCode() ?? 0 ^ From?.GetHashCode() ?? 0 ^ tMode.GetHashCode();
            }

            #endregion Constructors

            #region Methods

            public override bool Equals(object obj) {
                var t = obj as Transition;
                if (t == null)
                    return false;
                return t.Token == Token && t.To == To && t.From == From && t.TransitionMode == TransitionMode;
            }

            public override string ToString() => $"{From?.ToString() ?? "null"} + '{Token}' = {To?.ToString() ?? "null"}";

            public override int GetHashCode() => _hash;

            #endregion Methods
        }

        #endregion

        public FiniteStateMachine() {
            Stack.Add(HandleTransition(typeof(TInitialState)));
            CurrentState.Enter();
        }

        private IState HandleTransition(Type type) {
            if (type == null) return null;

            var match = States.FirstOrDefault(s => s.GetType() == type);
            if (match != null) return match;

            var state = (IState) Activator.CreateInstance(type);
            States.Add(state);

            foreach (var transition in type.GetCustomAttributes<TransitionAttribute>()) {
                var t = new Transition(transition.Trigger, state, HandleTransition(transition.ToState), transition.Mode);
                Transitions.Add(t);
            }
            
            return state;
        }

        #region Properties

        public List<IState> States { get; } = new List<IState>();

        public IEnumerable<string> Tokens => Transitions.Select(t => t.Token).Distinct();

        public List<Transition> Transitions { get; } = new List<Transition>();

        public IState CurrentState => Stack.Count > 0 ? Stack[0] : null;

        public T GetCurrentState<T>() where T : class, IState => CurrentState as T;

        private List<IState> Stack { get; } = new List<IState>();

        #endregion Properties

        #region Events

        /// <summary>
        /// Is fired when a transition successfully completes.
        /// </summary>
        public event Action<Transition> Transitioned;

        /// <summary>
        /// Is fired when a transition begins.
        /// </summary>
        public event Action<Transition> Transitioning;
        
        #endregion Events

        #region Methods

        /// <summary>
        /// Attempt to trigger a transition with the specified token
        /// </summary>
        /// <param name="token">The token to trigger</param>
        /// <exception cref="ArgumentOutOfRangeException">Token does not exist</exception>
        /// <exception cref="ArgumentOutOfRangeException">No valid transition exists</exception>
        /// <exception cref="ArgumentException">Cannot perform a Pop</exception>
        public void DoTransition(string token) {
            // First check that we have that token defined
            if (!Tokens.Contains(token))
                throw new ArgumentOutOfRangeException(nameof(token), token, "Given token is not defined.");

            // And that we have a transition for this token
            if (!Transitions.Any(t => t.Token == token && t.From == CurrentState))
                throw new ArgumentOutOfRangeException(nameof(token), token, $"No transition exists for {CurrentState} + {token}.");

            // Get list of possible transitions...
            var possible = Transitions.Where(t => t.From == CurrentState && t.Token == token).ToList();


            // Check validity of transition
            do {
                var transition = possible.MinBy(t =>
                {
                    if (t.TransitionMode != Mode.Pop) return 0;
                    var distanceTo = Stack.IndexOf(t.To);
                    return distanceTo == -1 ? int.MaxValue : distanceTo;
                });
                Transitioning?.Invoke(transition);
                CurrentState.Leave();
                switch (transition.TransitionMode) {
                    case Mode.Pop:
                        var pop = Stack.IndexOf(transition.To);
                        if (pop == -1) {
                            possible.Remove(transition);
                            continue;
                        }
                        for (var i = 0; i < pop; i++)
                            Stack.RemoveAt(0);
                        break;

                    case Mode.Push:
                        transition = Transitions.First(t => t.Token == token && t.From == CurrentState);
                        Stack.Insert(0, transition.To);
                        break;

                    case Mode.PushPop:
                        transition = Transitions.First(t => t.Token == token && t.From == CurrentState);
                        if (Stack.Count > 0) Stack.RemoveAt(0);
                        Stack.Insert(0, transition.To);
                        break;
                }
                CurrentState?.Enter();
                Transitioned?.Invoke(transition);
                return;
            } while (Transitions.Count > 0);
        }
        
        #endregion Methods
    }
}