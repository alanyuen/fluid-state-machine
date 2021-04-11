using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.Fluid.FSMs {
    public interface IState {
        Enum Id { get; }
        List<IAction> Actions { get; }
        IFsm ParentFsm { get; }
        void Enter ();
        void Exit ();
        void Update ();
        void Transition (Enum id);
    }
}
