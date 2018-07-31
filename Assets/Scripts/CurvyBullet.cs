using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvyBullet : SpinMeRightRound {
    public float rotationAmount;

    private Projectile projectile;
    private float curRotation;
    private Vector3 initialVelocity;

    private void Start() {
        projectile = GetComponent<Projectile>();
        initialVelocity = projectile.velocity;

        if (space == Space.Self) {
            axis = transform.TransformDirection(axis);
        }
    }

    protected override void Update() {
        curRotation += speed * Time.deltaTime;
        if (rotationAmount > 0) {
            curRotation = Mathf.Min(curRotation, rotationAmount);
        }
        
        projectile.velocity = Quaternion.AngleAxis(curRotation, axis) * initialVelocity;
        transform.forward = projectile.velocity;
    }
}
