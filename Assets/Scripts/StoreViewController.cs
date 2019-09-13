using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreViewController : MonoBehaviour
{
    public Text currentCoinsAmountText;
	public Text currentAttackMultiplexerText;
	public Text nextLevelText;
	public Text upgradeCostText;
	private int upgradeCost = -1;

    // Use this for initialization
    void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
		currentCoinsAmountText.text = "Current Coins Amount: " + PlayerShip.savedCoins.ToString();
		currentAttackMultiplexerText.text = "Current Attack Multiplexer: x" + UpgradeManager.currentAttackMultiplexer.ToString();
		float nextLevel = UpgradeManager.currentAttackMultiplexer + UpgradeManager.singleUpgradeAmount;
		nextLevelText.text = "Next Level: x" + nextLevel.ToString();
		upgradeCostText.text = "Upgrade Cost: " + UpgradeManager.upgradeCost.ToString();
    }

	public void DidTapResume()
	{
		Debug.LogWarning("DidTapResume");
		SceneManager.LoadScene("RandomSpawnScene");
	}


	public void DidTapUpgrade()
	{
		Debug.LogWarning("DidTapUpgrade");
		if (UpgradeManager.IsUpgradeAffordable()) {
			PlayerShip.SpendCoins(upgradeCost);
		}				
	}
}
