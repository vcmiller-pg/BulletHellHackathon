using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class HealthPickup : Pickup {
    public float healing = 50;

    protected override void TakeEffect(PlayerShip ship) {
        ship.Heal(healing);

        MessageManager.inst.ShowMessage("Acquired <color=red>Health</color>", 3.0f);
    }
}
