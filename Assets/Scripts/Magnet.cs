using UnityEngine;

public class Magnet : MonoBehaviour {
    public float moveSpeed;
    public float acceleration;

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out CoinsPickup c)) {
            c.overrideVelocity = c.velocity;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.TryGetComponent(out CoinsPickup c)) {
            Vector3 desiredVel = (transform.position - other.transform.position).normalized * moveSpeed;
            c.overrideVelocity = Vector3.MoveTowards(c.overrideVelocity.Value, desiredVel, acceleration * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.TryGetComponent(out CoinsPickup c)) {
            c.overrideVelocity = null;
        }
    }
}
