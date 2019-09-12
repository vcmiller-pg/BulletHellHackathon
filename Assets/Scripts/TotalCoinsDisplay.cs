using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalCoinsDisplay : MonoBehaviour
{
    /// <summary>
    /// Text used to show amount.
    /// </summary>
    [Tooltip("Text used to show amount.")]
    public Text amountText;
    private PlayerShip playerShip;

    // Start is called before the first frame update
    void Start()
    {
        playerShip = Object.FindObjectOfType<PlayerShip>();        
    }

    // Update is called once per frame
    void Update()
    {
        amountText.text = playerShip.totalCoins.ToString();
    }
}
