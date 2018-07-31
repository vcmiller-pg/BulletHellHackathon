using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMeRightRound : MonoBehaviour {
    public Vector3 axis;
    public float speed;
    public Space space = Space.World;
	
	// Update is called once per frame
	protected virtual void Update () {
        transform.Rotate(axis, speed * Time.deltaTime, space);
	}
}
