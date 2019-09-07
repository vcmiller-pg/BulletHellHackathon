using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SBR;

public class PlayerShip : Motor<FighterChannels> {
    public GameObject explosionPrefab;
    public AudioParameters impactSound;
    public AudioParameters deathSound;
    public float damageDealtOnHit = 100;
    public float damageTakenOnHit = 30;

    private List<ShipWeapon> weapons;
    private int curWeapon;

    protected override void Awake() {
        base.Awake();
        
        weapons = new List<ShipWeapon>(GetComponentsInChildren<ShipWeapon>());
        for (int i = 1; i < weapons.Count; i++) {
            weapons[i].enabled = false;
        }
    }

    protected override void DoOutput(FighterChannels channels) {
        int wep = curWeapon + channels.weaponSwitch;
        if (wep < 0) {
            wep = weapons.Count - 1;
        } else if (wep >= weapons.Count) {
            wep = 0;
        }

        if (wep != curWeapon) {
            SetCurrentWeapon(wep);
        }
    }

    public void AddWeapon(ShipWeapon prefab) {
        ShipWeapon weapon = Instantiate(prefab, transform);
        weapon.gameObject.SetActive(true);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.RefreshControllers();

        weapons.Add(weapon);
        SetCurrentWeapon(weapons.Count - 1);
    }

    public void SetCurrentWeapon(int index) {
        weapons[curWeapon].enabled = false;
        weapons[index].enabled = true;
        curWeapon = index;
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
}
