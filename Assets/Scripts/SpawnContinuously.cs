using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class SpawnContinuously : MonoBehaviour {
    public GameObject[] templates;
    public float cooldown;
    public SpawnMode mode;
    public bool atSelfLocation;
    public bool parentToSelf;

    private CooldownTimer spawnTimer;
    private int cycleIndex;

	// Use this for initialization
	void Start () {
        spawnTimer = new CooldownTimer(cooldown);
	}
	
	// Update is called once per frame
	void Update () {
		if (spawnTimer.Use() && templates != null && templates.Length > 0) {
            if (mode == SpawnMode.All) {
                for (int i = 0; i < templates.Length; i++) {
                    SpawnTemplate(i);
                }
            } else if (mode == SpawnMode.Random) {
                SpawnTemplate(Random.Range(0, templates.Length));
            } else {
                SpawnTemplate(cycleIndex);
                cycleIndex = (cycleIndex + 1) % templates.Length;
            }
        }
	}

    void SpawnTemplate(int index) {
        if (templates == null || index >= templates.Length || templates[index] == null) return;

        GameObject obj;
        if (atSelfLocation) {
            obj = Instantiate(templates[index], transform.position, Quaternion.identity);
        } else {
            obj = Instantiate(templates[index]);
        }

        obj.SetActive(true);
        obj.transform.parent = parentToSelf ? transform : null;
    }
}
