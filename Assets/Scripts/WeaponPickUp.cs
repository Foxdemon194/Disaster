using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickUp : MonoBehaviour
{

    public SC_WeaponManager weaponManager;
    public Text textModification;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            textModification.text = "Press F to pick up Fire Bolt Wand!";
            if(Input.GetKeyDown(KeyCode.F))
            {
                weaponManager.secondWeapon = true;
                textModification.text = null;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            textModification.text = null;
        }                
    }
}
