using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUse : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            other.GetComponent<SC_DamageReceiver>().ApplyDamage(-10);
            Destroy(gameObject);
        }
    }
}
