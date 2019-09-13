using UnityEngine;
using System;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{    
    public static float currentAttackMultiplexer { get; private set; }
	public static int upgradeCost = 200;
	public static float singleUpgradeAmount = 0.2f;

	private static float maxAttackMultiplexer = 3.0f;	
	private PlayerShip playerShip;


	static UpgradeManager()
	{
		currentAttackMultiplexer = 1.0f;
	}

    // Use this for initialization
    void Start()
    {
		playerShip = UnityEngine.Object.FindObjectOfType<PlayerShip>();
		UpdateAllWeaponsMultiplexers();
		currentAttackMultiplexer = Math.Max(1, currentAttackMultiplexer);
	}

    public static void DoUpgrade()
    {
		currentAttackMultiplexer += singleUpgradeAmount;
		MessageManager.inst.ShowMessage($"Attack multiplexer increased to {currentAttackMultiplexer}", 3.0f);
	}

    void UpdateAllWeaponsMultiplexers() {
        foreach (ShipWeapon weapon in playerShip.weapons) {
            weapon.damageMultiplier = currentAttackMultiplexer;
        }
    }

    void ResetUpgradeAmounts()
    {
		currentAttackMultiplexer = 1.0f;
    }

	public static bool IsUpgradeAffordable()
	{
		return PlayerShip.savedCoins >= upgradeCost && currentAttackMultiplexer < maxAttackMultiplexer;
	}

    // Update is called once per frame
    void Update()
    {
    }
}
