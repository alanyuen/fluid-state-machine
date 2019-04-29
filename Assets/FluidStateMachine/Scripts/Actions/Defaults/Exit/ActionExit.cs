using System;

namespace CleverCrow.FluidStateMachine {
    public class ActionExit : ActionBase {
        private readonly Action _exit;
        
        public override string Name { get; set; } = "Exit";

        public ActionExit (Action exit) {
            _exit = exit;
        }

        protected override void OnExit () {
            _exit();
        }
    }
}