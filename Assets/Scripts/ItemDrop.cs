using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {
    public GameObject[] items;
    public float spawnChance = 1;

    public int minCoinsDrop = 1;
    public int maxCoinsDrop = 1;

    private void OnZeroHealth() {
        if (Random.value < spawnChance) {
            GameObject item = items[Random.Range(0, items.Length)];
            int amountDrop = Random.Range(minCoinsDrop, maxCoinsDrop);
            for (int i = 0; i < amountDrop; i++) {
                Vector3 vector = transform.position + Random.insideUnitSphere * 10;
                vector.y = 0;
                Instantiate(item, vector, Quaternion.identity);
            }
            
        }
    }
}
