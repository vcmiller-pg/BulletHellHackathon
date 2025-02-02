﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class TriggerProjectile : Projectile {
    protected virtual void Update() {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) {
        OnHitCollider(collision.collider, transform.position);
    }

    private void OnTriggerEnter(Collider other) {
        OnHitCollider(other, transform.position);
    }
}
