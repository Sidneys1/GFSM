namespace GFSM {
    /// <summary>
    /// Represents a transition from one state to another
    /// </summary>
    /// <typeparam name="T">The type of state.</typeparam>
    public class Transition<T> where T : State<T> {
        #region Fields

        private readonly int _hash;

        #endregion Fields

        #region Properties

        public string Token { get; }

        public State<T> To { get; }

        public State<T> From { get; }

        public Mode TransitionMode { get; }

        #endregion Properties

        #region Constructors

        public Transition(string token, State<T> to, State<T> frm, Mode tMode = Mode.PushPop) {
            Token = token;
            To = to;
            From = frm;
            TransitionMode = tMode;
            _hash = Token.GetHashCode() ^ To?.GetHashCode() ?? 0 ^ From?.GetHashCode() ?? 0 ^ tMode.GetHashCode();
        }

        #endregion Constructors

        #region Methods

        public override bool Equals(object obj) {
            var t = obj as Transition<T>;
            if (t == null) return false;
            return t.Token == Token && t.To == To && t.From == From && t.TransitionMode == TransitionMode;
        }

        public override string ToString() {
            return $"{From} + '{Token}' = {To}";
        }

        public override int GetHashCode() {
            return _hash;
        }

        #endregion Methods

        #region Enums

        public enum Mode {
            Pop,
            Push,
            PushPop
        }

        #endregion Enums
    }
}