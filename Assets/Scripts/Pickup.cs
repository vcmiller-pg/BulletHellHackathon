using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public abstract class Pickup : MonoBehaviour {
    public AudioParameters sound;
    public Vector3 velocity;
    public float killZ = -20;

    private void OnTriggerEnter(Collider other) {
        if (other.transform.root.CompareTag("Player")) {
            TakeEffect(other.GetComponentInParent<PlayerShip>());
            Destroy(gameObject);
            sound?.PlayAtPoint(transform.position);
        }
    }

    private void Update() {
        transform.position += velocity * Time.deltaTime;

        if (transform.position.z < killZ) {
            Destroy(gameObject);
        }
    }

    protected abstract void TakeEffect(PlayerShip ship);
}
