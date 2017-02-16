# GFSM
A Generic Finite State Machine in C#

Implementation is easy:

```C#
// Entirely unnecessary, just here to implement a common DoWork()
public abstract class MyStateBase : IState {
  public virtual void Enter() {}
  public abstract void DoWork();
  public virtual void Leave() {}
}

// Define a state and it's transitions
[Transition("next", typeof(EndState))]
public class StartState : MyStateBase {
  public override void DoWork() => Console.WriteLine("In StartState");
  public override void Leave() => Console.WriteLine("\tLeaving StartState");
}

[Transition("next", null)]
public class EndState : MyStateBase {
  public override void Enter() => Console.WriteLine("\tEntered EndState");
  public override void DoWork() => Console.WriteLine("In EndState");
  public override void Leave() => Console.WriteLine("\tLeaving EndState");
}

internal class Program {
  private static void Main() {
    var fsm = new FiniteStateMachine<StartState>();
    
    fsm.Transitioning += transition => Console.WriteLine($"Beginning transition: {transition}");
    fsm.Transitioned += transition => {
      Console.WriteLine($"Done transitioning: {transition}");
      if (transition.To == null)
        Console.WriteLine("\nExited");
    };

    Console.WriteLine("Started\n");
    fsm.GetCurrentState<MyStateBase>().DoWork();
    fsm.DoTransition("next");
    fsm.GetCurrentState<MyStateBase>().DoWork();
    fsm.DoTransition("next");

    Console.ReadLine();
  }
}
/*
Will print:
Started

In StartState
Beginning transition: DemoApp.StartState + 'next' = DemoApp.EndState
    Leaving StartState
    Entered EndState
Done transitioning: DemoApp.StartState + 'next' = DemoApp.EndState
In EndState
Beginning transition: DemoApp.EndState + 'next' = null
    Leaving EndState
Done transitioning: DemoApp.EndState + 'next' = null

Exited
*/
```
