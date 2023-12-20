using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateScript : MonoBehaviour
{
    public string keyColor;
    public Text displayText;

    public bool gemAquired = false;

    public GameObject parentOfMine;

    public GameObject player;

    public string gem;


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && gemAquired)
        {
            displayText.text = null;
            player.GetComponent<SC_InventorySystem>().RemoveItem(gem);
            Destroy(parentOfMine);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !gemAquired)
        {
            displayText.text = "You need a " + keyColor + " gem to open this gate.";
            if (keyColor == "Black")
            {
                displayText.color = Color.black;
            }
            if (keyColor == "Blue")
            {
                displayText.color = Color.blue;
            }
            if (keyColor == "Red")
            {
                displayText.color = Color.red;
            }
            if (keyColor == "Yellow")
            {
                displayText.color = Color.yellow;
            }
            if (keyColor == "Green")
            {
                displayText.color = Color.green;
            }
        }

        if(other.CompareTag("Player") && gemAquired == true)
        {
            displayText.text = "Press E to open.";
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            displayText.text = null;
        }
    }
}
