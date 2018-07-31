using UnityEngine;

public class BasicEnemySMImpl : BasicEnemySM {
    public float startZ = 50;
    public float enterSpeed = 200;
    public float strafeXMin;
    public float strafeXMax;
    public bool strafe;
    public float zMove;

    public float strafeDir = 1;
    private Vector3 startPos;
    new private FighterChannels channels;

    public override void Initialize() {
        base.Initialize();

        channels = base.channels as FighterChannels;
    }

    void Awake() {
        Vector3 pos = transform.position;
        startPos = pos;
        pos.z = startZ;
        transform.position = pos;
    }

    protected override void State_Spawn() {
        transform.position = Vector3.MoveTowards(transform.position, startPos, enterSpeed * Time.deltaTime);
    }

    protected override void State_Combat() {
        channels.attack1 = true;

        if (strafe) {
            channels.movement = Vector3.right * strafeDir;

            if (transform.position.x < strafeXMin) {
                strafeDir = 1;
            } else if (transform.position.x > strafeXMax) {
                strafeDir = -1;
            }
        }

        channels.movement += Vector3.forward * zMove;
    }

    protected override bool TransitionCond_Spawn_Combat() {
        return transform.position == startPos;
    }

}
