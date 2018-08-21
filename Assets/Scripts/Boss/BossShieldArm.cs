using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldArm : MonoBehaviour {
    public float damageTaken { get; private set; }

    private bool _shielded;
    private Health parentHealth;

    public bool shielded {
        set {
            damageTaken = 0;
            _shielded = value;
        }

        get {
            return _shielded;
        }
    }

    public void OnDamage(Damage damage) {
        if (!shielded) {
            damageTaken += damage.amount;

            transform.root.Damage(damage);
        }
    }
}
