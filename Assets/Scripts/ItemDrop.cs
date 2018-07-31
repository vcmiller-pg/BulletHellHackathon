using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {
    public GameObject[] items;
    public float spawnChance = 1;

    private void OnZeroHealth() {
        if (Random.value < spawnChance) {
            GameObject item = items[Random.Range(0, items.Length)];
            Instantiate(item, transform.position, Quaternion.identity);
        }
    }
}
