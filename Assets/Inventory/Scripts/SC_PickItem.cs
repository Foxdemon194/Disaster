using UnityEngine;

public class SC_PickItem : MonoBehaviour
{
    public string itemName = "Some Item";
    public Texture itemPreview;
    public GameObject gate = null;

    private void Start()
    {        
        gameObject.tag = "Respawn";
    }

    public void PickItem()
    {
        if (gate != null)
        {
            gate.GetComponent<GateScript>().gemAquired = true;
        }
        Destroy(gameObject);
    }
}
