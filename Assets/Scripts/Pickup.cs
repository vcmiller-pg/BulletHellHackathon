using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public abstract class Pickup : MonoBehaviour {
    public AudioParameters sound;
    public Vector3 velocity;
    public float killZ = -20;
    public Vector3? overrideVelocity { get; set; }
    public Bounds bounds;

    private void Awake() {
        GameBounds gb = FindObjectOfType<GameBounds>();
        if (gb) {
            bounds = gb.bounds;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.isTrigger && other.transform.root.CompareTag("Player")) {
            TakeEffect(other.GetComponentInParent<PlayerShip>());
            Destroy(gameObject);
            sound?.PlayAtPoint(transform.position);
        }
    }

    private void Update() {
        Vector3 vel = (overrideVelocity ?? velocity);
        if (transform.position.x < bounds.min.x) {
            vel.x += 10;
        } else if (transform.position.x > bounds.max.x) {
            vel.x -= 10;
        }
        transform.position += vel * Time.deltaTime;

        if (transform.position.z < killZ) {
            Destroy(gameObject);
        }
    }

    protected abstract void TakeEffect(PlayerShip ship);
}
