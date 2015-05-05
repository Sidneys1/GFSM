# GFSM
A Generic Finite State Machine in C#

Implementation is easy:

```C#
// First you will need a base class for your States:
public abstract class MyStateBase : State {
  protected MyStateBase() : base(StateMode.Active) { }
}

// Then we can create our FSM:
public class MyFiniteStateMachine : FiniteStateMachine<MyStateBase> { }

// And can create our own states:
public class StartState : MyStateBase {
  public override Transition[] ToThisTransitions { get; } = {
    // Allow transition in from nothing  
    new Transition(Command.Deactivate, typeof(StartState), null)
  };
  public override Transition[] FromThisTransitions { get; } = {
    new Transition(Command.Deactivate, typeof(EndState), typeof(StartState))
  };
  
  public override void Enter() {
    base.Enter();
    Console.WriteLine("Entered StartState");
  }

  public override void Exit() {
    base.Exit();
    Console.WriteLine("Exited StartState");
  }
}
public class EndState : MyStateBase {
  public override Transition[] ToThisTransitions { get; } = {
    new Transition(Command.Deactivate, typeof(EndState), typeof(StarState))
  };
  
  // No out transitions needed.
  public override void Enter() {
    base.Enter();
    Console.WriteLine("Entered EndState");
  }
}

public static void main() {
  var fsm = new MyFiniteStateMachine();
  var start = new StartState();
  var end = new EndState();
  fsm.States.Add(start);
  fsm.States.Add(end);
  
  // We can transition into StartState from a null state
  fsm.Transition(start);
  // And transition from StartState into EndState
  fsm.Transition(end);
}
/*
Will print:
Entered StartState
Exited StartState
Entered EndState
*/
```
