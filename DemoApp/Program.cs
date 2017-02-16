using System;
using GFSM;

namespace DemoApp {
    public abstract class MyStateBase : State<MyStateBase> {
        protected MyStateBase(FiniteStateMachine<MyStateBase> stateMachine) : base(stateMachine) { }

        public abstract void DoWork();
    }

    public class MyFiniteStateMachine : FiniteStateMachine<MyStateBase> { }

    public class StartState : MyStateBase {
        public StartState(FiniteStateMachine<MyStateBase> stateMachine) : base(stateMachine) {}

        public override void Enter() => Console.WriteLine("Entered StartState");

        public override void DoWork() => Console.WriteLine("In StartState");
    }

    public class EndState : MyStateBase {
        public EndState(FiniteStateMachine<MyStateBase> stateMachine) : base(stateMachine) {}

        public override void Enter() => Console.WriteLine("Entered EndState");

        public override void DoWork() => Console.WriteLine("In EndState");
    }

    internal class Program {
        private static void Main() {
            var fsm = new MyFiniteStateMachine();
            var start = new StartState(fsm);
            var end = new EndState(fsm);
            fsm.States.Add(start);
            fsm.States.Add(end);

            fsm.AddTransition(new Transition<MyStateBase>("start", null, start));
            fsm.AddTransition(new Transition<MyStateBase>("next", start, end));
            fsm.AddTransition(new Transition<MyStateBase>("next", end, null));

            fsm.Transitioned += transition => {
                Console.WriteLine(transition);
                if (transition.To == null)
                    Console.WriteLine("Exited");
            };

            fsm.Transition("start");
            fsm.CurrentState.DoWork();
            fsm.Transition("next");
            fsm.CurrentState.DoWork();
            fsm.Transition("next");

            Console.ReadLine();
        }
    }
}
