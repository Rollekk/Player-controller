using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamaginPool : MonoBehaviour
{
    public PlayerStats playerStats;
    public float damageSpeed = 1.5f;
    private bool bIsInside;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bIsInside)
        {
            if(playerStats.Health > 0f) playerStats.Health -= damageSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerBody") bIsInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "PlayerBody") bIsInside = false;
    }
}
