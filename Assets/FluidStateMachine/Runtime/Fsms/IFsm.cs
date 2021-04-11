using System;
using UnityEngine;
using UnityEngine.Events;

namespace CleverCrow.Fluid.FSMs {
    public interface IFsm {
        IState GetState (Enum id);
        void AddState (IState state);
        void SetState (Enum id);
        object Owner { get; }
        IState CurrentState { get; }
        IState DefaultState { get; set; }
        UnityEvent EventExit { get; }

        void Tick ();
        void Reset ();
        void Exit ();
    }
}