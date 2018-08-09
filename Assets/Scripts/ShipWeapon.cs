using SBR;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapon : BasicMotor<FighterChannels> {
    public CooldownTimer shootTimer { get; private set; }
    public Magazine magazine { get; private set; }

    public Projectile bulletPrefab;
    public float startCooldown = 0;
    public float fireRate = 10;
    public int magazineSize;
    public float magazineReload;
    public Transform[] guns;
    public AudioInfo shootSound;
    public GameObject target;
    public bool parentProjectiles = false;
    public bool followRoll = true;
    public SpawnMode gunMode = SpawnMode.All;
    public bool aimAtTarget;

    private Dictionary<Transform, Quaternion> startRotations;
    private int currentGun;

    private void Awake() {
        startRotations = new Dictionary<Transform, Quaternion>();
        foreach (var gun in guns) {
            startRotations[gun] = gun.localRotation;
        }
    }

    protected override void Start () {
        base.Start();

        shootTimer = new CooldownTimer(1.0f / fireRate, startCooldown);

        if (magazineReload > 0) {
            magazine = new Magazine(magazineSize, magazineReload);
        }
    }

    public override void TakeInput() {
        if (channels.attack1 && shootTimer.canUse && (magazine?.Use() != false)) {
            shootTimer.Use();
            shootSound?.Play(transform.position);

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
            transform.eulerAngles = e;
        }
    }
}
