using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.FluidStateMachine.Editors {
    public class StateTest {
        private IFsm _fsm;

        private enum StateId {
            A,
            B,
        }

        [SetUp]
        public void BeforeEach () {
            _fsm = Substitute.For<IFsm>();
        }
        
        public class UpdateMethod : StateTest {
            [Test]
            public void It_should_call_Update_on_all_Actions () {
                var action = Substitute.For<IAction>();
                var state = new State(_fsm, StateId.A);
                state.Actions.Add(action);

                state.Update();
                
                action.Received(1).Update();
            }
        }
        
        public class EnterMethod : StateTest {
            [Test]
            public void It_should_call_Enter_on_all_Actions () {
                var action = Substitute.For<IAction>();
                var state = new State(_fsm, StateId.A);
                state.Actions.Add(action);

                state.Enter();
                
                action.Received(1).Enter();
            }
        }

        public class ExitMethod : StateTest {
            [Test]
            public void It_should_call_Exit_on_all_Actions () {
                var action = Substitute.For<IAction>();
                var state = new State(_fsm, StateId.A);
                state.Actions.Add(action);

                state.Exit();
                
                action.Received(1).Exit();
            }
        }

        public class TransitionMethod : StateTest {
            [Test]
            public void It_should_activate_SetState_on_the_fsm () {
                var state = new State(_fsm, StateId.A);
                
                state.AddTransition(new Transition("b", StateId.B));
                state.Transition("b");

                _fsm.Received(1).SetState(StateId.B);
            }
        }
    }
}