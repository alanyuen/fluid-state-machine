using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.Fluid.FSMs {
    public class State : IState {
        public Enum Id { get; }
        public List<IAction> Actions { get; } = new List<IAction>();
        public IFsm ParentFsm { get; }

        public State (IFsm fsm, Enum id) {
            ParentFsm = fsm;
            Id = id;
        }
        
        public void Update () {
            foreach (var action in Actions) {
                action.Update();
            }
        }

        public void Enter () {
            foreach (var action in Actions) {
                action.Enter();
            }
        }

        public void Exit () {
            foreach (var action in Actions) {
                action.Exit();
            }
        }

        public void Transition (Enum id) {
            ParentFsm.SetState(id);
        }
    }
}