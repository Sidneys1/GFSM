using System;

namespace GFSM {

    public class Transition {

		#region Fields

		public readonly Command Command;
        public readonly Type To;
        public readonly Type From;

		#endregion Fields


		#region Constructors

		public Transition(Command command, Type to, Type frm) {
            Command = command;
            To = to;
            From = frm;
		}

		#endregion Constructors


		#region Methods

		public override bool Equals(object obj) {
            var t = obj as Transition;
            if (t == null) return false;
            return t.Command == Command && t.To == To && t.From == From;
        }

        public override string ToString() {
            return $"{From.Name} is {Command}d by {To.Name}";
        }

        public override int GetHashCode() {
            return Command.GetHashCode() ^ To.GetHashCode() ^ From.GetHashCode();
		}

		#endregion Methods
	}
}