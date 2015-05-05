# GFSM
A Generic Finite State Machine in C#

Implementation is easy:

```C#
// First you will need a base class for your States:
public abstract class MyStateBase : State {
  protected MyStateBase(FiniteStateMachine<MyStateBase> stateMachine) : base(stateMachine) { }
}

// Then we can create our FSM:
public class MyFiniteStateMachine : FiniteStateMachine<MyStateBase> { }

// And can create our own states:
public class StartState : MyStateBase {
  public override void Enter() {
    base.Enter();
    Console.WriteLine("Entered StartState");
    StateMachine.Transition("next");
  }
}
public class EndState : MyStateBase {
  // No out transitions needed.
  public override void Enter() {
    base.Enter();
    Console.WriteLine("Entered EndState");
    StateMachine.Transition("end");
  }
}

public static void main() {
  var fsm = new MyFiniteStateMachine();
  var start = new StartState(fsm);
  var end = new EndState(fsm);
  fsm.States.Add(start);
  fsm.States.Add(end);
  
  fsm.AddTransition(new Transition<MyStateBase>("start", null, start));
  fsm.AddTransition(new Transition<MyStateBase>("next", start, end));
  fsm.AddTransition(new Transition<MyStateBase>("end", end, null));
  
  fsm.Transitioned += t => if (t.To == null) Console.WriteLine("Exited!");
  
  // We can transition into StartState from a null state
  fsm.Transition("start");
}
/*
Will print:
Entered StartState
Entered EndState
Exited!
*/
```
