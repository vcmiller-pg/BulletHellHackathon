using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVAnimation : MonoBehaviour {
    public Vector2 scaleRate;
    public Vector2 offsetRate;
    public Texture2D[] textures;
    public int framerate = 10;

    private Material material;
    private int curTexture = 0;
    private CooldownTimer changeTimer;

	// Use this for initialization
	void Start () {
        material = GetComponent<Renderer>().material;
        if (framerate > 0 && textures?.Length > 1) {
            changeTimer = new CooldownTimer(1.0f / framerate);
        }
	}
	
	// Update is called once per frame
	void Update () {
        material.mainTextureOffset += offsetRate * Time.deltaTime;
        material.mainTextureScale += scaleRate * Time.deltaTime;

        if (changeTimer?.Use() == true) {
            curTexture = (curTexture + 1) % textures.Length;
            material.mainTexture = textures[curTexture];
            material.SetTexture("_EmissionMap", textures[curTexture]);
        }
	}
}
