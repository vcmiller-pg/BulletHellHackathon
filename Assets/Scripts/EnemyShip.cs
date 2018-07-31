using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class EnemyShip : MonoBehaviour {
    public GameObject explosionPrefab;
    public Health health;

    private void Start() {
        health = GetComponent<Health>();
    }

    private void OnDamage(Damage dmg) {
        GameStateManager.inst.shotsHit++;
    }

    private void OnZeroHealth() {
        Destroy(gameObject);
        GameStateManager.inst.enemiesDestroyed++;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
