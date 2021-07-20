using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [Header("Weapon")]
    public Weapon activeWeapon;
    public Weapon hoveredWeapon;
    private RaycastHit WeaponHit;
    public LayerMask WeaponMask;
    public float weaponPickupRange;
    public bool bHasWeapon = false;

    [Header("Attack")]
    public KeyCode attackKey;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PickupWeapon();
        if (activeWeapon && Input.GetKeyDown(attackKey)) activeWeapon.Attack();


    }

    void PickupWeapon()
    {
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out WeaponHit, weaponPickupRange, WeaponMask))
        {
            hoveredWeapon = WeaponHit.transform.gameObject.GetComponent<Weapon>();

            if (!hoveredWeapon.bInHand)
            {
                hoveredWeapon.HighlightWeapon(true);

                if (Input.GetKeyDown(hoveredWeapon.pickupKey))
                {
                    if (!activeWeapon) hoveredWeapon.Attach();
                    else hoveredWeapon.Swap();
                }
            }
            else
            {
                hoveredWeapon.HighlightWeapon(false);
            }
        }
        else
        {
            if (hoveredWeapon) hoveredWeapon.HighlightWeapon(false);
            hoveredWeapon = null;
        }
    }
}
