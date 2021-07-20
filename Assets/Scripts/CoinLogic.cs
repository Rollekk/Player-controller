using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLogic : MonoBehaviour
{
    public PlayerStats playerStats;
    public CoinTextUpdate coinText;

    public float levitationHeight = 0.005f;
    public float rotationSpeed = 0.5f;

    public bool bShouldFloat;
    public bool bShouldRotate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bShouldFloat) transform.position += new Vector3(0, (Mathf.Sin(Time.time) * levitationHeight), 0);
        if(bShouldRotate) transform.Rotate(Vector3.right * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "PlayerBody")
        {
            playerStats.Coins += 1f;
            coinText.bCoinDestroyed = true;
            Destroy(gameObject);
        }
    }
}
