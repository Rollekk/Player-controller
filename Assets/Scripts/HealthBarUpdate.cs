using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpdate : MonoBehaviour
{
    [Header("Components")]
    public PlayerStats playerStats;
    private Slider HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar = GetComponent<Slider>();
        HealthBar.value = playerStats.Health;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.value = playerStats.Health;
    }
}
