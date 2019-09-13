using UnityEngine;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{    
    private float attackMultiplexer = 1.0f;
	private float maxAttackMultiplexer = 3.0f;
	private PlayerShip playerShip;
    private int upgradeCost = 200;

    // Use this for initialization
    void Start()
    {
		playerShip = Object.FindObjectOfType<PlayerShip>();
    }

    void DoUpgrade(float amount)
    {
        attackMultiplexer += amount;
        UpdateGameValues();
    }

    void UpdateGameValues() {
        foreach (ShipWeapon weapon in playerShip.weapons) {
            weapon.damageMultiplier = attackMultiplexer;
        }
    }

    void ResetUpgradeAmounts()
    {
        attackMultiplexer = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerShip.totalCoins >= upgradeCost && attackMultiplexer < maxAttackMultiplexer) {
            DoUpgrade(0.2f);
            playerShip.SpendCoins(upgradeCost);
            MessageManager.inst.ShowMessage($"Attack multiplexer increased to {attackMultiplexer}", 3.0f);
        }
    }
}
