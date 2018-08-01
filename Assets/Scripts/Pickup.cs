using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour {
    public AudioInfo sound;
    public Vector3 velocity;
    public float killZ = -20;

    private void OnTriggerEnter(Collider other) {
        if (other.transform.root.CompareTag("Player")) {
            TakeEffect(other.GetComponentInParent<PlayerShip>());
            Destroy(gameObject);
            sound?.Play(transform.position);
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
