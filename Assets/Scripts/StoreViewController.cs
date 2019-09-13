using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreViewController : MonoBehaviour
{
    public Text currentCoinsAmountText;

	public Text currentAttackMultiplierText;
	public Text nextAttackLevelText;
	public Text attackUpgradeCostText;

    public Text currentHealthMultiplierText;
    public Text nextHealthLevelText;
    public Text healthUpgradeCostText;

    // Use this for initialization
    void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
		currentCoinsAmountText.text = "Current Coins Amount: " + PlayerShip.savedCoins.ToString();

		currentAttackMultiplierText.text = "Current Attack Multiplier: x" + UpgradeManager.currentAttackMultiplier.ToString();
		float nextLevel = UpgradeManager.currentAttackMultiplier + UpgradeManager.singleUpgradeAmount;
		nextAttackLevelText.text = "Next Level: x" + nextLevel.ToString();
        attackUpgradeCostText.text = "Upgrade Cost: " + UpgradeManager.damageUpgradeCost.ToString();

        currentHealthMultiplierText.text = "Current Health Multiplier: x" + UpgradeManager.currentHealthMultiplier.ToString();
        nextLevel = UpgradeManager.currentHealthMultiplier + UpgradeManager.singleUpgradeAmount;
        nextHealthLevelText.text = "Next Level: x" + nextLevel.ToString();
        attackUpgradeCostText.text = "Upgrade Cost: " + UpgradeManager.healthUpgradeCost.ToString();
    }

	public void DidTapResume()
	{
		SceneManager.LoadScene("RandomSpawnScene");
	}


	public void DidTapDamageUpgrade()
	{
		if (UpgradeManager.IsUpgradeAffordable()) {
			PlayerShip.SpendCoins(UpgradeManager.damageUpgradeCost);
			UpgradeManager.UpgradeAttack();
		}				
	}

    public void DidTapHealthUpgrade() {
        if (PlayerShip.savedCoins >= UpgradeManager.healthUpgradeCost) {
            PlayerShip.SpendCoins(UpgradeManager.healthUpgradeCost);
            UpgradeManager.UpgradeHealth();
        }
    }
}
