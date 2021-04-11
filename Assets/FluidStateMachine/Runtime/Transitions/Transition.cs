using System;

namespace CleverCrow.Fluid.FSMs {
    public class Transition : ITransition {
        public string Name => Target.ToString();
        public Enum Target { get; }
        
        public Transition (Enum target) {
            Target = target;
        }
    }
}