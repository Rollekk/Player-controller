using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    [Header("Statistics")]
    public float Coins;
    public float Health;
    [Space]
    public float Stamina;
    public float MaxStamina;
    public float StaminaDrainRate;
    public float StaminaRecoverRate;

    // Start is called before the first frame update
    void Start()
    {
        Coins = 0f;
        Health = 100f;
        Stamina = MaxStamina;
    }

    // Update is called once per frame
    void Update()
    {

       // playerHealth.value = Health;
    }
}
