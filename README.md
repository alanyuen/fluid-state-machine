# Fluid State Machine [![Build Status](https://travis-ci.org/ashblue/fluid-state-machine.svg?branch=master)](https://travis-ci.org/ashblue/fluid-state-machine)

Fluid State Machine is a Unity plugin aimed at creating state machines in pure code. It allows state actions to be re-used and customized on a per-project basis.

* Extendable, write your own re-usable state actions
* Heavily tested with TDD
* Open source and free

## Support

Join the [Discord Community](https://discord.gg/8QHFfzn) if you have questions or need help.

See upcoming features and development progress on the [Trello Board](https://trello.com/b/4EXulH1t/fluid-state-machine).

## Getting Started

Here we have a door that demonstrates a simple open and close mechanism. By changing the `Open` variable, the state machine will automatically change the door's state.

```c#
using UnityEngine;
using CleverCrow.Fluid.FSMs;

public class Door : MonoBehaviour {
    private IFsm _door;
    public bool Open { private get; set; }

    public enum DoorState {
        Opened,
        Closed,
    }

    private void Start () {
        _door = new FsmBuilder()
            // Declares the FSMs associated GameObject
            .Owner(gameObject)
            
            // What is the default starting state?
            .Default(DoorState.Closed)
            
            // Creates a state called DoorState.Closed and assigns new actions to it
            .State(DoorState.Closed, (close) => {
                close.SetTransition("open", DoorState.Opened)
                    .Update((action) => {
                        if (Open) action.Transition("open");
                    });
            })
            
            .State(DoorState.Opened, (open) => {
                open.SetTransition("close", DoorState.Closed)
                    .Update((action) => {
                        if (!Open) action.Transition("close");
                    });
            })
            
            .Build();
    }

    private void Update () {
        // Update the state machine every frame
        _door.Tick();
    }
}
```

If you want to write you own custom state actions to bundle up complex chunks of code. You can easily do so with [C# extensions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods). This allows you to adjust the functionality of Fluid State Machine on a per project basis or write your own custom extension library for it. All without forking the core library, which allows you to still get future version updates.

```c#
using CleverCrow.Fluid.FSMs;

public class MyCustomAction : ActionBase {
    protected override void OnUpdate () {
        // Custom logic goes here
    }
}

public static class StateBuilderExtensions {
    public static StateBuilder MyCustomAction (this StateBuilder builder) {
        return builder.AddAction(new MyCustomAction());
    }
}

public class ExampleUsage {
    private enum StateId {
        A,
    }

    public void Init () {
        var fsmBuilder = new FsmBuilder()
            .State(StateId.A, (state) => {
                state
                    .MyCustomAction()
                    .Update((action) => { });
            })
            .Build();
    }
}
```

### Installation

Fluid State Machine is used through [Unity's Package Manager](https://docs.unity3d.com/Manual/CustomPackages.html). In order to use it you'll need to add the following lines to your `Packages/manifest.json` file. After that you'll be able to visually control what specific version of Fluid Behavior Tree you're using from the package manager window in Unity. This has to be done so your Unity editor can connect to NPM's package registry.

```json
{
  "scopedRegistries": [
    {
      "name": "NPM",
      "url": "https://registry.npmjs.org",
      "scopes": [
        "com.fluid"
      ]
    }
  ],
  "dependencies": {
    "com.fluid.state-machine": "2.0.1"
  }
}
```

Archives of specific versions and release notes are available on the [releases page](https://github.com/ashblue/fluid-state-machine/releases).

### Examples

An [examples repo](https://github.com/ashblue/fluid-state-machine-examples) is available that demonstrates concepts found in this README.md file. Useful for seeing how these concepts might be used in an actual game.

## Table of Contents

* [Action Library](#action-library)
  + [Defaults](#defaults)
    - [Enter](#enter)
    - [Exit](#exit)
    - [Update](#update)
    - [RunFsm](#runfsm)
  + [Triggers](#triggers)
    - [Enter](#enter-1)
    - [Exit](#exit-1)
    - [Stay](#stay)
  + [Animators](#animators)
    - [Set Animator Bool](#set-animator-bool)
    - [Set Animator Float](#set-animator-float)
    - [Set Animator Int](#set-animator-int)
    - [Set Animator Trigger](#set-animator-trigger)
* [Creating Custom Actions](#creating-custom-actions)
* [Development](#development)

## Action Library

Pre-made actions included in this library are as follows.

### Defaults

Actions targeted at hooking the default state machine lifecycle.

#### Enter

Triggers whenever a state is initially entered.

```c#
.State(MyEnum.MyState, (state) => {
    state.Enter((action) => Debug.Log("Code goes here"));
})
```

#### Exit

Triggers whenever a state is exited.

```c#
.State(MyEnum.MyState, (state) => {
    state.Exit((action) => Debug.Log("Code goes here"));
})
```

#### Update

Every frame a FSM's `Fsm.Tick()` method is called and the state is active this will run.

```c#
.State(MyEnum.MyState, (state) => {
    state.Update((action) => Debug.Log("Code goes here"));
})
```

#### RunFsm

Used to run a nested FSM inside of a state. This action will continue running until the nested FSM triggers an Exit event through `Fsm.Exit()`. When exit is triggered the passed transition will automatically fire.

```c#
var nestedFsm = new FsmBuilder()
    .Default(OtherStateId.A)
    .State(OtherStateId.A, (state) => {
        state.Enter((action) => Debug.Log("Nested FSM triggered"));
        // This will notify the fsm that triggered nestedFsm to stop running it
        state.FsmExit();
    })
    .Build();

var fsm = new FsmBuilder()
    .Default(StateId.A)
    .State(StateId.A, (state) => {
        state.SetTransition("next", StateId.B);
        // First argument is the transition triggered when `nestedFsm.Exit()` is detected
        state.RunFsm("next", nestedFsm);
    })
    .State(StateId.B, (state) => {
        state.Enter((action) => Debug.Log("Success"));
    })
    .Build();
```

### Triggers

Hook's Unity's collider trigger system. Note that a collider component set to trigger must be included in order for this to work.

#### Enter

Logic fired when trigger is entered with a specific tag.

```c#
.State(MyEnum.MyState, (state) => {
    state.TriggerEnter("Player", (action) => Debug.Log("Code goes here"));
})
```

#### Exit

Logic fired when trigger is exited with a specific tag.

```c#
.State(MyEnum.MyState, (state) => {
    state.TriggerExit("Player", (action) => Debug.Log("Code goes here"));
})
```

#### Stay

Logic fired when Unity's stay trigger activates.

```c#
.State(MyEnum.MyState, (state) => {
    state.TriggerStay("Player", (action) => Debug.Log("Code goes here"));
})
```

### Animators

Talks to the current Animator. Note that an Animator component must be included on the passed GameObject owner.

#### Set Animator Bool

Sets an animator bool by string.

```c#
.State(MyEnum.MyState, (state) => {
    state.SetAnimatorBool("myBool", true);
})
```

#### Set Animator Float

Sets an animator float by string.

```c#
.State(MyEnum.MyState, (state) => {
    state.SetAnimatorFloat("myFloat", 2.2);
})
```

#### Set Animator Int

Sets an animator int by string.

```c#
.State(MyEnum.MyState, (state) => {
    state.SetAnimatorInt("myInt", 7);
})
```

#### Set Animator Trigger

Sets an animator trigger by string.

```c#
.State(MyEnum.MyState, (state) => {
    state.SetAnimatorTrigger("myInt");
})
```

## Creating Custom Actions

Here we'll cover how to create a custom action and use it in a way that gets free updates from this library. It's important you create new actions this way to prevent new versions from causing errors.


The first thing you'll need to do is create a **custom action**.

```c#
using UnityEngine;
using CleverCrow.Fluid.FSMs;

public class MyAction : ActionBase {
    public MyAction (string newName) {
        Name = newName;
    }

    // Triggers when entering the state
    protected override void OnEnter () {
        Debug.Log($"Custom action {Name} activated");
    }
    
    // Triggers when exiting the state
    protected override void OnExit () {
    }
    
    // Runs every time `Fsm.Tick()` is called
    protected override void OnUpdate () {
    }
}
```

After the custom action is complete you'll need to create a `StateBuilder` C# extension that adds it. Then you'll be able to call it as if it's a native method on the library.

```c#
using CleverCrow.Fluid.FSMs;

public static class StateBuilderExtensions {
    public static StateBuilder MyAction (this StateBuilder builder, string name) {
        return builder.AddAction(new MyAction(name));
    }
}
```

That's it! You're done. Try it out with this snippet.

```c#
using UnityEngine;
using CleverCrow.Fluid.FSMs;

public class FsmBuilderCustomUsage : MonoBehaviour {
    private enum StateId {
        A,
    }
    
    private void Awake () {
        var fsmBuilder = new FsmBuilder()
            .State(StateId.A, (state) => {
                state
                    .MyAction("custom name")
                    .Update((action) => { });
            });
        
        var fsm = fsmBuilder.Build();
        fsm.Tick();
    }
}
```

## Development

If you want to work on the code in this repo you'll need to install Node.js and Git. Then run the following command to setup Node.js from the repo's root.

```bash
npm install
```

### Making Commits

All commits should be made using [Commitizen](https://github.com/commitizen/cz-cli) (which is automatically installed when running `npm install`). Commits are automatically compiled to version numbers on release so this is very important. PRs that don't have Commitizen based commits will be rejected.

To make a commit type the following into a terminal from the root.

```bash
npm run commit
```

### Build testing

Builds can be manually run with the following command

```bash
npm run build
```
