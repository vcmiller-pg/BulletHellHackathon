using UnityEngine;
using System;
using System.Collections;
using SBR;

public class UpgradeManager : MonoBehaviour
{
    public static float currentAttackMultiplier { get; private set; } = 1;
    public static float currentHealthMultiplier { get; private set; } = 1;
    public static int damageUpgradeCost => Mathf.RoundToInt(200 * currentAttackMultiplier);
    public static int healthUpgradeCost => Mathf.RoundToInt(200 * currentHealthMultiplier);
	public static float singleUpgradeAmount = 0.2f;

    private static float maxAttackMultiplier = 3.0f;
    private static float maxHealthMultiplier = 3.0f;
	private PlayerShip playerShip;


    // Use this for initialization
    void Start()
    {
		playerShip = UnityEngine.Object.FindObjectOfType<PlayerShip>();
		ApplyAllMultipliers();
		currentAttackMultiplier = Math.Max(1, currentAttackMultiplier);
	}

    public static void UpgradeAttack()
    {
		currentAttackMultiplier += singleUpgradeAmount;
	}

    public static void UpgradeHealth() {
        currentHealthMultiplier += singleUpgradeAmount;
    }

    void ApplyAllMultipliers() {
        ApplyWeaponMultipliers();
        playerShip.GetComponent<Health>().maxHealth *= maxHealthMultiplier;
    }

    void ApplyWeaponMultipliers() {
        foreach (ShipWeapon weapon in playerShip.weapons) {
            weapon.damageMultiplier = currentAttackMultiplier;
        }
    }

    void ResetUpgradeAmounts()
    {
		currentAttackMultiplier = 1.0f;
    }

	public static bool IsUpgradeAffordable()
	{
		return PlayerShip.savedCoins >= damageUpgradeCost && currentAttackMultiplier < maxAttackMultiplier;
	}

    // Update is called once per frame
    void Update()
    {
        ApplyWeaponMultipliers();
    }
}
