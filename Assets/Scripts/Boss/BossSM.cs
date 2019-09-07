using UnityEngine;
using SBR;
using System.Collections.Generic;
using SBR.StateMachines;

#pragma warning disable 649
public abstract class BossSM : StateMachine<FighterChannels> {
    public enum StateID {
        Spawn, AttackCycle, BeamAttack, ArmAttack, SpawnGuns, SpawnLasers
    }

    private class State : SBR.StateMachines.State {
        public StateID id;

        public override string ToString() {
            return id.ToString();
        }
    }

    public BossSM() {
        allStates = new State[6];

        State stateSpawn = new State() {
            id = StateID.Spawn,
            enter = StateEnter_Spawn,
            transitions = new List<Transition>(1)
        };
        allStates[0] = stateSpawn;

        State stateAttackCycle = new State() {
            id = StateID.AttackCycle,
            subMachine = new SubStateMachine(),
            transitions = new List<Transition>(0)
        };
        allStates[1] = stateAttackCycle;

        State stateBeamAttack = new State() {
            id = StateID.BeamAttack,
            enter = StateEnter_BeamAttack,
            transitions = new List<Transition>(1)
        };
        allStates[2] = stateBeamAttack;

        State stateArmAttack = new State() {
            id = StateID.ArmAttack,
            enter = StateEnter_ArmAttack,
            during = State_ArmAttack,
            transitions = new List<Transition>(1)
        };
        allStates[3] = stateArmAttack;

        State stateSpawnGuns = new State() {
            id = StateID.SpawnGuns,
            enter = StateEnter_SpawnGuns,
            transitions = new List<Transition>(1)
        };
        allStates[4] = stateSpawnGuns;

        State stateSpawnLasers = new State() {
            id = StateID.SpawnLasers,
            enter = StateEnter_SpawnLasers,
            transitions = new List<Transition>(1)
        };
        allStates[5] = stateSpawnLasers;

        rootMachine.defaultState = stateSpawn;
        stateSpawn.parentMachine = rootMachine;
        stateAttackCycle.parentMachine = rootMachine;
        stateAttackCycle.subMachine.defaultState = stateArmAttack;
        stateBeamAttack.parent = stateAttackCycle;
        stateBeamAttack.parentMachine = stateAttackCycle.subMachine;
        stateArmAttack.parent = stateAttackCycle;
        stateArmAttack.parentMachine = stateAttackCycle.subMachine;
        stateSpawnGuns.parent = stateAttackCycle;
        stateSpawnGuns.parentMachine = stateAttackCycle.subMachine;
        stateSpawnLasers.parent = stateAttackCycle;
        stateSpawnLasers.parentMachine = stateAttackCycle.subMachine;

        Transition transitionSpawnAttackCycle = new Transition() {
            from = stateSpawn,
            to = stateAttackCycle,
            exitTime = 2f,
            mode = StateMachineDefinition.TransitionMode.Time,
            cond = TransitionCond_Spawn_AttackCycle
        };
        stateSpawn.transitions.Add(transitionSpawnAttackCycle);

        Transition transitionBeamAttackSpawnLasers = new Transition() {
            from = stateBeamAttack,
            to = stateSpawnLasers,
            exitTime = 4.5f,
            mode = StateMachineDefinition.TransitionMode.Time,
            cond = TransitionCond_BeamAttack_SpawnLasers
        };
        stateBeamAttack.transitions.Add(transitionBeamAttackSpawnLasers);

        Transition transitionArmAttackBeamAttack = new Transition() {
            from = stateArmAttack,
            to = stateBeamAttack,
            exitTime = 8f,
            mode = StateMachineDefinition.TransitionMode.Condition,
            cond = TransitionCond_ArmAttack_BeamAttack
        };
        stateArmAttack.transitions.Add(transitionArmAttackBeamAttack);

        Transition transitionSpawnGunsArmAttack = new Transition() {
            from = stateSpawnGuns,
            to = stateArmAttack,
            exitTime = 15f,
            mode = StateMachineDefinition.TransitionMode.Time,
            cond = TransitionCond_SpawnGuns_ArmAttack
        };
        stateSpawnGuns.transitions.Add(transitionSpawnGunsArmAttack);

        Transition transitionSpawnLasersSpawnGuns = new Transition() {
            from = stateSpawnLasers,
            to = stateSpawnGuns,
            exitTime = 5f,
            mode = StateMachineDefinition.TransitionMode.Time,
            cond = TransitionCond_SpawnLasers_SpawnGuns
        };
        stateSpawnLasers.transitions.Add(transitionSpawnLasersSpawnGuns);


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
    protected virtual void StateEnter_BeamAttack() { }
    protected virtual void StateEnter_ArmAttack() { }
    protected virtual void State_ArmAttack() { }
    protected virtual void StateEnter_SpawnGuns() { }
    protected virtual void StateEnter_SpawnLasers() { }

    protected virtual bool TransitionCond_Spawn_AttackCycle() { return false; }
    protected virtual bool TransitionCond_BeamAttack_SpawnLasers() { return false; }
    protected virtual bool TransitionCond_ArmAttack_BeamAttack() { return false; }
    protected virtual bool TransitionCond_SpawnGuns_ArmAttack() { return false; }
    protected virtual bool TransitionCond_SpawnLasers_SpawnGuns() { return false; }

}
#pragma warning restore 649
