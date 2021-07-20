using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Components")]
    public Combat combat;
    private Outline outline;
    private BoxCollider bladeCollider;
    public Enemy attackedEnemy;
    public Animator animator;

    [Header("Stats")]
    public string weaponName;
    public string weaponKeybind;
    public float weaponDamage;
    public bool bShouldDamage;
    public int attackCombo;

    [Header("Keybind")]
    public KeyCode pickupKey;
    public bool bInHand = false;
    public bool bShowKeybind = false;

    [Header("Materials")]
    private Material katanaMaterial;
    private Renderer katanaRednerer;
    public float highlightStrength = 0.75f;
    private Color materialDefaultColor;

    // Start is called before the first frame update
    void Start()
    {
        bladeCollider = GetComponent<BoxCollider>();

        outline = GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.enabled = false;

        katanaRednerer = GetComponent<Renderer>();
        katanaMaterial = Instantiate<Material>(katanaRednerer.material);
        katanaRednerer.material = katanaMaterial;

        materialDefaultColor = katanaMaterial.color;

        weaponName = name;
        weaponKeybind = pickupKey.ToString();
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void HighlightWeapon(bool bShouldHighlight)
    {
        if (bShouldHighlight)
        {
            katanaMaterial.SetColor("_Color", new Color(materialDefaultColor.r + highlightStrength, materialDefaultColor.g + highlightStrength, materialDefaultColor.b + highlightStrength, materialDefaultColor.a));
            outline.enabled = true;
            bShowKeybind = true;
        }
        else
        {
            katanaMaterial.SetColor("_Color", materialDefaultColor);
            outline.enabled = false;
            bShowKeybind = false;
        }
    }

    public void Attach()
    {
            transform.parent = GameObject.Find("WeaponPos_end").transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -90f));

            gameObject.layer = 12;
            combat.bHasWeapon = true;
            combat.activeWeapon = combat.hoveredWeapon;

            bInHand = true;

            katanaMaterial.SetColor("_Color", materialDefaultColor);
            combat.hoveredWeapon.outline.enabled = false;

            combat.hoveredWeapon = null;
    }

    public void Swap()
    {
        combat.activeWeapon.transform.parent = null;
        combat.activeWeapon.transform.position = combat.hoveredWeapon.transform.position;
        combat.activeWeapon.transform.rotation = combat.hoveredWeapon.transform.rotation;
        combat.activeWeapon.gameObject.layer = 13;
        combat.activeWeapon.bInHand = false;

        combat.hoveredWeapon.Attach();
    }

    public void Attack()
    {
        animator.SetBool("bIsAttacking", true);
        bShouldDamage = true;
        if (attackCombo < 2) attackCombo++;
        else attackCombo = 0;
        Invoke("CancelAttackAnim", 0.1f);
        Invoke("CancelAttack", 0.3f);
    }

    void CancelAttackAnim()
    {
       animator.SetBool("bIsAttacking", false);

    }

    void CancelAttack()
    {
        bShouldDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (bShouldDamage)
            {
                attackedEnemy = other.gameObject.GetComponent<Enemy>();
                attackedEnemy.TakeDamage();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        attackedEnemy = null;
    }
}
