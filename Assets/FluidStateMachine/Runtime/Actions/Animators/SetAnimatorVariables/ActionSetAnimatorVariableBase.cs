using UnityEngine;

namespace CleverCrow.Fluid.FSMs {
    public abstract class ActionSetAnimatorVariableBase : ActionBase {
        protected Animator _animator;

        protected override void OnInit () {
            var obj = ParentState.ParentFsm.Owner as GameObject;
            _animator = obj.GetComponent<Animator>();
        }
    }
}