using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public Combat combat;
    public Animator animator;

    [Header("Stats")]
    public float enemyHealth;
    public float flashTime;

    [Header("Materials")]
    private Material enemyMaterial;
    private Renderer enemyRenderer;
    private Color originalEnemyColor;
    public Color damagedColor;
    public Color deadColor;

    // Start is called before the first frame update
    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        enemyMaterial = Instantiate<Material>(enemyRenderer.material);
        enemyRenderer.material = enemyMaterial;

        originalEnemyColor = enemyMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        if (enemyHealth > 0f)
        {
            enemyMaterial.SetColor("_Color", damagedColor);
            enemyHealth -= combat.activeWeapon.weaponDamage;
            Invoke("SetBackMaterial", flashTime);
            print(enemyHealth.ToString());
        }
        else enemyMaterial.SetColor("_Color", deadColor);

    }

    public void SetBackMaterial()
    {
        enemyMaterial.SetColor("_Color", originalEnemyColor);
    }
}

