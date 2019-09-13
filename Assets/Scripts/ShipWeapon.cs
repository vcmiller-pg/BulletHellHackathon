using SBR;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapon : Motor<FighterChannels> {
    public CooldownTimer shootTimer { get; private set; }
    public Magazine magazine { get; private set; }

    public string displayName;

    public Projectile bulletPrefab;
    public float startCooldown = 0;
    public float fireRate = 10;
    public int magazineSize;
    public float magazineReload;
    public Transform[] guns;
    public AudioParameters shootSound;
    public Tag targetTag;
    public bool parentProjectiles = false;
    public bool followRoll = true;
    public SpawnMode gunMode = SpawnMode.All;
    public bool aimAtTarget;
    public float damageMultiplier = 1.0f;

    private Dictionary<Transform, Quaternion> startRotations;
    private int currentGun;
    private GameObject target;

    protected override void Awake() {
        base.Awake();
        startRotations = new Dictionary<Transform, Quaternion>();
        foreach (var gun in guns) {
            startRotations[gun] = gun.localRotation;
        }
    }

    void Start () {
        shootTimer = new CooldownTimer(1.0f / fireRate, startCooldown);

        if (magazineReload > 0) {
            magazine = new Magazine(magazineSize, magazineReload);
        }

        if (targetTag != 0) {
            target = Tags.FindGameObjectWithTag(targetTag);
        }
    }

    protected override void DoOutput(FighterChannels channels) {
        if (channels.attack1 && shootTimer.canUse && (magazine?.Fire() != false)) {
            LateUpdate();
            shootTimer.Use();
            shootSound?.PlayAtPoint(transform.position);

            if (gunMode == SpawnMode.All) {
                for (int i = 0; i < guns.Length; i++) {
                    FireGun(i);
                }
            } else if (gunMode == SpawnMode.Cycle) {
                FireGun(currentGun);
                currentGun = (currentGun + 1) % guns.Length;
            } else {
                FireGun(Random.Range(0, guns.Length));
            }
        }
    }

    private void FireGun(int index) {
        var gun = guns[index];
        var bullet = Instantiate(bulletPrefab, gun.position, gun.rotation);
        bullet.damage *= damageMultiplier;
        if (parentProjectiles) {
            bullet.transform.parent = transform;
        }
        if (target) {
            bullet.Fire(target.transform.position - bullet.transform.position);
        } else {
            bullet.Fire();
        }

        if (transform.root.CompareTag("Player")) {
            GameStateManager.inst.shotsTaken++;
        }

        if (aimAtTarget) {
            if (target) {
                Vector3 v = target.transform.position - gun.position;
                v.y = 0;
                gun.rotation = Quaternion.LookRotation(v, Vector3.up);
            } else {
                gun.localRotation = startRotations[gun];
            }
        }
    }

    private void LateUpdate() {
        if (!followRoll) {
            Vector3 e = transform.eulerAngles;
            e.z = 0;
            e.x = 0;
            transform.eulerAngles = e;
        }
    }
}
