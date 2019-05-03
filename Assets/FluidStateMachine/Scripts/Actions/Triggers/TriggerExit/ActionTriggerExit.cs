using System;

namespace CleverCrow.FluidStateMachine {
    public class ActionTriggerExit : ActionTriggerBase {
        public ActionTriggerExit (string tag, Action<IAction> update) : base(tag, update) {
        }
        
        protected override void OnInit () {
            Monitor.EventTriggerExit.AddListener(UpdateTrigger);
        }
    }
}