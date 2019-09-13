using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPickup : Pickup {
    public string displayName;
    public string childObjectToActivate;

    protected override void TakeEffect(PlayerShip ship) {
        MessageManager.inst.ShowMessage($"Acquired <color=red>{displayName}</color>", 3.0f);
        var child = ship.transform.Find("PowerupObjects").Find(childObjectToActivate);
        child.GetComponent<PowerupObject>().Activate();
    }
}
