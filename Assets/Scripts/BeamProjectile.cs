using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class BeamProjectile : MonoBehaviour {
    public Vector3 velocity;
    public float killZ;
    public float killX;
    public UVAnimation beam;
	
	// Update is called once per frame
	void Update () {
        transform.position += velocity * Time.deltaTime;

        if (transform.position.z < killZ || transform.position.x < -killX || transform.position.x > killX) {
            OnZeroHealth();
        }
	}

    void OnZeroHealth() {
        beam.transform.parent = null;
        beam.offsetRate = new Vector2(velocity.magnitude / 25f, 0);
        beam.enabled = true;
        beam.GetComponent<TimeToLive>().enabled = true;
        Destroy(gameObject);
    }
}
