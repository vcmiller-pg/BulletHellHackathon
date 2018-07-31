using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVAnimation : MonoBehaviour {
    public Vector2 scaleRate;
    public Vector2 offsetRate;

    private Material material;

	// Use this for initialization
	void Start () {
        material = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        material.mainTextureOffset += offsetRate * Time.deltaTime;
        material.mainTextureScale += scaleRate * Time.deltaTime;
	}
}
