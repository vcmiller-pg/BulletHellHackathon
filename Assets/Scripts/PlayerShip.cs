using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SBR;

public class PlayerShip : MonoBehaviour {
    public GameObject explosionPrefab;
    public GameObject impactVfx;
    public Transform impactSites;
    public AudioParameters impactSound;
    public AudioParameters deathSound;
    public float damageDealtOnHit = 100;
    public float damageTakenOnHit = 30;
    public ShipWeapon[] startingWeapons;

    public static int savedCoins { get; private set; }

    public List<ShipWeapon> weapons { get; private set; }

    public int totalCoins { get; private set; }

	public void Start() {
		totalCoins = 0;
	}

    void Awake() {
        weapons = new List<ShipWeapon>();
        foreach (var weapon in startingWeapons) {
            AddWeapon(weapon);
        }
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

        if (impactVfx && impactSites?.childCount > 0)
        {
            var gameObj = Instantiate(impactVfx, transform);
            var ind = Random.Range(0, impactSites.childCount);
            gameObj.transform.localPosition = impactSites.GetChild(ind).localPosition;
        }
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

    public void AddCoins(int amount) {
        totalCoins += amount;
    }

    public static void SpendCoins(int amount) {
        savedCoins -= amount;
    }

    public void SaveCoins() {
        savedCoins += totalCoins;
        totalCoins = 0;
    }
}
