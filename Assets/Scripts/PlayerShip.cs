using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SBR;

public class PlayerShip : BasicMotor<FighterChannels> {
    public GameObject explosionPrefab;
    public float hitInvuln = 0.5f;
    public AudioInfo impactSound;
    public AudioInfo deathSound;
    public float damageDealtOnHit = 100;
    public float damageTakenOnHit = 30;

    new private Collider collider;
    private ExpirationTimer hitInvulnTimer;
    private List<ShipWeapon> weapons;
    private int curWeapon;

    protected override void Start() {
        base.Start();
        
        hitInvulnTimer = new ExpirationTimer(hitInvuln);
        collider = GetComponentInChildren<Collider>();

        weapons = new List<ShipWeapon>(GetComponentsInChildren<ShipWeapon>());
        for (int i = 1; i < weapons.Count; i++) {
            weapons[i].enabled = false;
        }
    }

    public override void TakeInput() {
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
        var weapon = Instantiate(prefab, transform);
        weapon.gameObject.SetActive(true);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        GetComponent<Brain>().UpdateMotors();

        weapons.Add(weapon);
        SetCurrentWeapon(weapons.Count - 1);
    }

    public void SetCurrentWeapon(int index) {
        weapons[curWeapon].enabled = false;
        weapons[index].enabled = true;
        curWeapon = index;
    }

    private void OnDamage(Damage dmg) {
        hitInvulnTimer.Set();
        impactSound?.Play(transform.position);
        collider.enabled = false;
        GameStateManager.inst.damageTaken += dmg.amount;
    }

    private void Update() {
        collider.enabled = hitInvulnTimer.expired;
    }

    private void OnZeroHealth() {
        Destroy(gameObject);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        deathSound?.Play(transform.position);

        GameStateManager.inst.LoseGame();
    }

    private void OnTriggerEnter(Collider other) {
        var enemy = other.GetComponent<EnemyShip>();

        if (enemy) {
            enemy.Damage(damageDealtOnHit);
            this.Damage(damageTakenOnHit);
        }
    }
}
