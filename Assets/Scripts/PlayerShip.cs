using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SBR;

public class PlayerShip : MonoBehaviour {
    public GameObject explosionPrefab;
    public AudioParameters impactSound;
    public AudioParameters deathSound;
    public float damageDealtOnHit = 100;
    public float damageTakenOnHit = 30;
    public ShipWeapon[] startingWeapons;

    private List<ShipWeapon> weapons;

    public int totalCoins { get; private set; }

    void Awake() {
        weapons = new List<ShipWeapon>();
        foreach (var weapon in startingWeapons) {
            AddWeapon(weapon);
        }

        totalCoins = 0;
    }

    public void AddWeapon(ShipWeapon prefab) {
        ShipWeapon weapon = Instantiate(prefab, transform);
        weapon.gameObject.SetActive(true);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.RefreshControllers();

        weapons.Add(weapon);
    }

    private void OnDamage(Damage dmg) {
        impactSound?.PlayAtPoint(transform.position);
        GameStateManager.inst.damageTaken += dmg.amount;
    }

    private void OnZeroHealth() {
        Destroy(gameObject);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        deathSound?.PlayAtPoint(transform.position);

        GameStateManager.inst.LoseGame();
    }

    private void OnTriggerEnter(Collider other) {
        var enemy = other.GetComponentInParent<EnemyShip>();

        if (enemy) {
            enemy.Damage(damageDealtOnHit);
            this.Damage(damageTakenOnHit);
        }
    }

    public void AddCoins(int coins) {
        totalCoins += coins;
    }
}
