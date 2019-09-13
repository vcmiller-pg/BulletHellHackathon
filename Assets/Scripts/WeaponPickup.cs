using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup {
    public ShipWeapon weaponObject;
    public bool showMessage;

    protected override void TakeEffect(PlayerShip ship) {
        ship.AddWeapon(weaponObject);

        if (showMessage) {
            MessageManager.inst.ShowScreen(1);
        }

        MessageManager.inst.ShowMessage($"Acquired the <color=green>{weaponObject.displayName}</color>", 3.0f);
    }
}
