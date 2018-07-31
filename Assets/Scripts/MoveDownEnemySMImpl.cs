using UnityEngine;

public class MoveDownEnemySMImpl : BasicEnemySM {
    public Vector3 movement;
    new private FighterChannels channels;

    public override void Initialize() {
        base.Initialize();

        channels = base.channels as FighterChannels;
    }

    protected override void State_Spawn() {
        
    }

    protected override void State_Combat() {
        channels.attack1 = true;

        channels.movement = movement;
    }

    protected override bool TransitionCond_Spawn_Combat() => true;

}
