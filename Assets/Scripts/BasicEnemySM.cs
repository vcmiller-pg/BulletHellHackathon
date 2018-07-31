using UnityEngine;
using SBR;
using System.Collections.Generic;

#pragma warning disable 649
public abstract class BasicEnemySM : SBR.StateMachine {
    public enum StateID {
        Spawn, Combat
    }

    new private class State : StateMachine.State {
        public StateID id;

        public override string ToString() {
            return id.ToString();
        }
    }

    public BasicEnemySM() {
        allStates = new State[2];

        State stateSpawn = new State() {
            id = StateID.Spawn,
            enter = StateEnter_Spawn,
            during = State_Spawn,
            transitions = new List<Transition>(1)
        };
        allStates[0] = stateSpawn;

        State stateCombat = new State() {
            id = StateID.Combat,
            during = State_Combat,
            transitions = new List<Transition>(0)
        };
        allStates[1] = stateCombat;

        rootMachine.defaultState = stateSpawn;
        stateSpawn.parentMachine = rootMachine;
        stateCombat.parentMachine = rootMachine;

        Transition transitionSpawnCombat = new Transition() {
            from = stateSpawn,
            to = stateCombat,
            exitTime = 0f,
            mode = StateMachineDefinition.TransitionMode.ConditionOnly,
            cond = TransitionCond_Spawn_Combat
        };
        stateSpawn.transitions.Add(transitionSpawnCombat);


    }

    public StateID state {
        get {
            State st = rootMachine.activeLeaf as State;
            return st.id;
        }

        set {
            stateName = value.ToString();
        }
    }

    protected virtual void StateEnter_Spawn() { }
    protected virtual void State_Spawn() { }
    protected virtual void State_Combat() { }

    protected virtual bool TransitionCond_Spawn_Combat() { return false; }

}
#pragma warning restore 649
