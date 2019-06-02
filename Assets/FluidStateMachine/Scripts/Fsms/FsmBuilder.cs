using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.Fluid.FSMs {
    public class FsmBuilder {
        private readonly List<StateData> _stateData = new List<StateData>();
        private GameObject _owner;
        private Enum _defaultState;

        private class StateData {
            public Enum id;
            public Action<StateBuilder> callback;
        }

        public FsmBuilder Owner (GameObject owner) {
            _owner = owner;
            return this;
        }
        
        public FsmBuilder State (Enum id, Action<StateBuilder> stateCallback) {
            var builder = new StateBuilder {Id = id};
            stateCallback(builder);
            _stateData.Add(new StateData {
                id = id,
                callback = stateCallback
            });
            
            return this;
        }
        
        public FsmBuilder Default (Enum id) {
            _defaultState = id;
            return this;
        }
        
        public IFsm Build () {
            var fsm = new Fsm(_owner) as IFsm;
            StateData defaultState = null;
            
            foreach (var state in _stateData) {
                var builder = new StateBuilder {Id = state.id};
                state.callback(builder);
                fsm.AddState(builder.Build(fsm));

                if (Equals(_defaultState, state.id)) {
                    defaultState = state;
                }
            }

            if (defaultState != null) {
                fsm.DefaultState = fsm.GetState(defaultState.id);
            }

            return fsm;
        }
    }
}