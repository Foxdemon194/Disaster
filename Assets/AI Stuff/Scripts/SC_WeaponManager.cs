using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_WeaponManager : MonoBehaviour
{
    public Camera playerCamera;
    public SC_Weapon primaryWeapon;
    public SC_Weapon secondaryWeapon;

    [HideInInspector]
    public SC_Weapon selectedWeapon;

    public bool secondWeapon = false;

    private void Start()
    {
        primaryWeapon.ActivateWeapon(true);
        secondaryWeapon.ActivateWeapon(false);
        selectedWeapon = primaryWeapon;
        primaryWeapon.manager = this;
        secondaryWeapon.manager = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && secondWeapon) //1
        {
            primaryWeapon.ActivateWeapon(false);
            secondaryWeapon.ActivateWeapon(true);
            selectedWeapon = secondaryWeapon;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)) //2
        {
            primaryWeapon.ActivateWeapon(true);
            secondaryWeapon.ActivateWeapon(false);
            selectedWeapon = primaryWeapon;
        }
    }
}
