using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class CoinsPickup : Pickup {
    public int singleCoinValue = 10;

    private void Start()
    {
       transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
    }

    protected override void TakeEffect(PlayerShip ship) {
        ship.AddCoins(singleCoinValue);
        MessageManager.inst.ShowMessage("Acquired <color=green>Coin</color>", 3.0f);
    }
}
