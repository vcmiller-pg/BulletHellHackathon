using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBillboard : MonoBehaviour
{
    public Vector3 axis = new Vector3(0,0,1);
    public float speed = 360;
    public float speedDelta = 90;
    public Space space = Space.Self;
    private float flip = 1.0f;

    private void Start()
    {
        flip = ((Random.value > 0.5f) ? 1.0f : -1.0f);
        speed = speed + Random.Range(-speedDelta / 2.0f, speedDelta / 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.Rotate(axis * flip, speed * Time.time, space);
    }
}
