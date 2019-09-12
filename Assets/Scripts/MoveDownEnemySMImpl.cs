using UnityEngine;

public class MoveDownEnemySMImpl : BasicEnemySM<FighterChannels> {
    public Vector3 movement;
    private FighterMotor motor;

    protected override void Awake() {
        base.Awake();

        motor = GetComponent<FighterMotor>();
    }

    protected override void State_Spawn() {
        
    }

    protected override void State_Combat() {
        channels.attack1 = true;

        channels.movement = movement;
        if (transform.position.x < motor.movementBounds.min.x) {
            movement.x = Mathf.Abs(movement.x);
        } else if (transform.position.x > motor.movementBounds.max.x) {
            movement.x = -Mathf.Abs(movement.x);
        }
    }

    protected override bool TransitionCond_Spawn_Combat() => true;

}
