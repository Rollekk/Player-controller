using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTextUpdate : MonoBehaviour
{
    [Header("Components")]
    public Combat combat;
    private Weapon hoveredWeapon;
    private Text keybindText;

    // Start is called before the first frame update
    void Start()
    {
        keybindText = GetComponent<Text>();
        keybindText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (combat.hoveredWeapon)
        {
            hoveredWeapon = combat.hoveredWeapon;
        } 
        else hoveredWeapon = null;

        if (hoveredWeapon && hoveredWeapon.bShowKeybind)
        {
            keybindText.enabled = true;
            if (hoveredWeapon)
            {
                keybindText.text = "[" + hoveredWeapon.weaponKeybind + "] " + hoveredWeapon.name;
            }
        }
        else
        {
            keybindText.enabled = false;
        }
    }
}
