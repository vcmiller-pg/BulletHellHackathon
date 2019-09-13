using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;
using System;

public class FighterMotor : Motor<FighterChannels> {
    new public Rigidbody rigidbody { get; private set; }

    private Vector3 velocity;

    public float maxSpeed = 5;

    public float rollAngle = 30;
    public float rollSpeed = 90;

    public bool useBounds;
    public Bounds movementBounds;

    public float killZ = -20;

    public RotationMode rotationMode;

    protected override void Awake() {
        base.Awake();

        GameBounds gb = FindObjectOfType<GameBounds>();
        if (gb) {
            movementBounds = gb.bounds;
        }

        rigidbody = GetComponent<Rigidbody>();
    }

    protected override void DoOutput(FighterChannels channels) {
        Vector3 move = channels.movement;
        velocity = move * maxSpeed;
        Vector3 e = transform.eulerAngles;

        if (rotationMode == RotationMode.StrafeRoll) {
            Vector3 right = transform.right;
            right.y = 0;
            right = right.normalized;
            float rollX = -Vector3.Dot(move, right) * rollAngle;
            transform.eulerAngles = new Vector3(e.x, e.y, Mathf.MoveTowardsAngle(e.z, rollX, rollSpeed * Time.deltaTime));
        } else if (rotationMode == RotationMode.FaceDirection) {
            if (move.sqrMagnitude > 0.01f) {
                Vector3 targetDir = velocity;
                targetDir.y = 0;
                Vector3 axis = Vector3.Cross(transform.forward, velocity);

                if (axis.sqrMagnitude < 0.01f) {
                    axis = Vector3.up;
                } else {
                    axis = axis.normalized;
                }

                float angle = Vector3.Angle(transform.forward, velocity);
                transform.Rotate(axis, Mathf.Min(angle, Time.deltaTime * rollSpeed), Space.World);
            }
        }
    }

    private void FixedUpdate() {
        Vector3 newPos = rigidbody.position + velocity * Time.fixedDeltaTime;

        if (useBounds) {
            newPos.x = Mathf.Clamp(newPos.x, movementBounds.min.x, movementBounds.max.x);
            newPos.z = Mathf.Clamp(newPos.z, movementBounds.min.z, movementBounds.max.z);
        }

        rigidbody.MovePosition(newPos);
    }

    private void Update() {
        if (transform.position.z < killZ) {
            GameStateManager.inst.enemiesEscaped++;
            Destroy(gameObject);
        }
    }

    public enum RotationMode {
        StrafeRoll,
        FaceDirection,
        None
    }
}
