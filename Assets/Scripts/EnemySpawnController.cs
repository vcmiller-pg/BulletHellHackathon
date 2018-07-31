using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class EnemySpawnController : MonoBehaviour {
    public Wave[] waves;

    public int currentWave = 0;

    private ExpirationTimer waveTimer;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

	// Use this for initialization
	void Start () {
        waveTimer = new ExpirationTimer(0);

        SpawnWave(currentWave);
	}
	
	// Update is called once per frame
	void Update () {
        spawnedEnemies.RemoveAll(e => !e);

        if (spawnedEnemies.Count == 0 || (waveTimer.expiration > 0 && waveTimer.expired)) {
            if (currentWave < waves.Length - 1) {
                currentWave++;
                SpawnWave(currentWave);
            } else {
                GameStateManager.inst.WinGame();
                enabled = false;
            }
        }
	}

    private void SpawnWave(int index) {
        var wave = waves[index];

        wave.spawnParent.SetActive(true);
        for (int i = 0; i < wave.spawnParent.transform.childCount; i++) {
            spawnedEnemies.Add(wave.spawnParent.transform.GetChild(i).gameObject);
        }

        waveTimer.expiration = wave.timeLimit;
        waveTimer.Set();

        if (index == 0) {
            MessageManager.inst.ShowScreen(0);
        }
    }

    [System.Serializable]
    public struct Wave {
        public GameObject spawnParent;
        public float timeLimit;
    }
}
