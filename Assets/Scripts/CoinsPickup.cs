using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class CoinsPickup : Pickup {
    public int singleCoinValue = 10;

    protected override void TakeEffect(PlayerShip ship) {
        ship.AddCoins(singleCoinValue);
        MessageManager.inst.ShowMessage("Acquired <color=green>Coin</color>", 3.0f);
    }
}
