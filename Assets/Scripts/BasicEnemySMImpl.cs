using UnityEngine;

public class BasicEnemySMImpl : BasicEnemySM<FighterChannels> {
    public float startZ = 50;
    public float enterSpeed = 200;
    public bool strafe;
    public float zMove;

    public float strafeDir = 1;
    private Vector3 startPos;
    private FighterMotor motor;

    protected override void Awake() {
        base.Awake();

        Vector3 pos = transform.position;
        startPos = pos;
        pos.z = startZ;
        transform.position = pos;
        motor = GetComponent<FighterMotor>();
    }

    protected override void State_Spawn() {
        transform.position = Vector3.MoveTowards(transform.position, startPos, enterSpeed * Time.deltaTime);
    }

    protected override void State_Combat() {
        channels.attack1 = true;

        if (strafe) {
            channels.movement = Vector3.right * strafeDir;

            if (transform.position.x < motor.movementBounds.min.x) {
                strafeDir = 1;
            } else if (transform.position.x > motor.movementBounds.max.x) {
                strafeDir = -1;
            }
        }

        channels.movement += Vector3.forward * zMove;
    }

    protected override bool TransitionCond_Spawn_Combat() {
        return transform.position == startPos;
    }

}
