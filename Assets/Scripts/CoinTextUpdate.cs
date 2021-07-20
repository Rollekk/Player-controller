using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinTextUpdate : MonoBehaviour
{
    [Header("Components")]
    public Text coinCount;
    public PlayerStats playerStats;

    public bool bCoinDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        coinCount.text = "Coin count: " + playerStats.Coins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (bCoinDestroyed)
        {
            bCoinDestroyed = false;
            coinCount.text = "Coin count: " + playerStats.Coins.ToString();
        }
    }
}
