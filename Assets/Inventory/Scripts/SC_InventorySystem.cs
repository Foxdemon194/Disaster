using UnityEngine;

public class SC_InventorySystem : MonoBehaviour
{
    public Texture crosshairTexture;
    public SC_CharacterController playerController;
    public SC_PickItem[] availableItems;

    int[] itemSlots = new int[12];
    bool showInventory = false;
    float windowAnimation = 1;
    float animationTimer = 0;

    int hoveringOverIndex = -1;
    int itemIndexToDrag = -1;
    Vector2 dragOffset = Vector2.zero;

    SC_PickItem detectedItem;
    int detectedItemIndex;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        for(int i=0; i<itemSlots.Length; i++)
        {
            itemSlots[i] = -1;
        }
    }

    public void RemoveItem(string itemName)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] > -1 && availableItems[itemSlots[i]].itemName == itemName)
            {
                itemSlots[i] = -1;
                break; // Exit the loop once the item is found and removed
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            showInventory = !showInventory;
            animationTimer = 0;
            if(showInventory)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
        }

        if(animationTimer < 1)
        {
            animationTimer += Time.deltaTime;
        }

        if(showInventory)
        {
            windowAnimation = Mathf.Lerp(windowAnimation, 0, animationTimer);
            playerController.canMove = false;
        }
        else
        {
            windowAnimation = Mathf.Lerp(windowAnimation, 1f, animationTimer);
            playerController.canMove = true;
        }

        if(Input.GetMouseButtonDown(0) && hoveringOverIndex > -1 && itemSlots[hoveringOverIndex] > -1)
        {
            itemIndexToDrag = hoveringOverIndex;
        }
         if(Input.GetMouseButtonUp(0) && itemIndexToDrag > -1)
        {
            if(hoveringOverIndex < 0)
            {
                Instantiate(availableItems[itemSlots[itemIndexToDrag]], playerController.playerCamera.transform.position + (playerController.playerCamera.transform.forward), Quaternion.identity);
                itemSlots[itemIndexToDrag] = -1;
            }
            else
            {
                int itemIndexTmp = itemSlots[itemIndexToDrag];
                itemSlots[itemIndexToDrag] = itemSlots[hoveringOverIndex];
                itemSlots[hoveringOverIndex] = itemIndexTmp;

            }
            itemIndexToDrag = -1;
        }

         if(detectedItem && detectedItemIndex > -1)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                int slotToAddTo = -1;
                for(int i=0; i<itemSlots.Length; i++)
                {
                    if(itemSlots[i] == -1)
                    {
                        slotToAddTo = i;
                        break;
                    }
                }
                if(slotToAddTo > -1)
                {
                    itemSlots[slotToAddTo] = detectedItemIndex;
                    detectedItem.PickItem();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = playerController.playerCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        if(Physics.Raycast(ray, out hit, 2.5f))
        {
            Transform objectHit = hit.transform;

            if(objectHit.CompareTag("Respawn"))
            {
                if ((detectedItem == null || detectedItem.transform != objectHit)
                    && objectHit.GetComponent<SC_PickItem>() != null)
                {
                    SC_PickItem itemTmp = objectHit.GetComponent<SC_PickItem>();

                    for(int i=0; i<availableItems.Length; i++)
                    {
                        if(availableItems[i].itemName == itemTmp.itemName)
                        {
                            detectedItem = itemTmp;
                            detectedItemIndex = i;
                        }
                    }
                }
            }
            else
            {
                detectedItem = null;
            }
        }
        else
        {
            detectedItem = null;
        }
    }

    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 60;



        //GUI.Label(new Rect(30, 30, 200, 25), "Press 'Tab' to open Inventory", labelStyle);

        if (windowAnimation < 1)
        {
            GUILayout.BeginArea(new Rect(10 - (430 * windowAnimation), Screen.height / 2 - 200, 302, 430), GUI.skin.GetStyle("box"));
            GUILayout.Label("Inventory", GUILayout.Height(25));

            GUILayout.BeginVertical();
            for(int i=0; i<itemSlots.Length; i +=3)
            {
                GUILayout.BeginHorizontal();
                for(int a=0; a<3; a++)
                {
                    if(i+a < itemSlots.Length)
                    {
                        if(itemIndexToDrag == i+a || (itemIndexToDrag > -1 && hoveringOverIndex == i+a))
                        {
                            GUI.enabled = false;
                        }

                        if(itemSlots[i+a] > -1)
                        {
                            if(availableItems[itemSlots[i+a]].itemPreview)
                            {
                                GUILayout.Box(availableItems[itemSlots[i + a]].itemPreview, GUILayout.Width(95), GUILayout.Height(95));
                            }
                            else
                            {
                                GUILayout.Box(availableItems[itemSlots[i + a]].itemName, GUILayout.Width(95), GUILayout.Height(95));
                            }
                        }
                        else
                        {
                            GUILayout.Box("", GUILayout.Width(95), GUILayout.Height(95));
                        }

                        Rect lastRect = GUILayoutUtility.GetLastRect();
                        Vector2 eventMousePosition = Event.current.mousePosition;
                        if(Event.current.type == EventType.Repaint && lastRect.Contains(eventMousePosition))
                        {
                            hoveringOverIndex = i + a;
                            if(itemIndexToDrag < 0)
                            {
                                dragOffset = new Vector2(lastRect.x - eventMousePosition.x, lastRect.y - eventMousePosition.y);
                            }
                        }

                        GUI.enabled = true;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            if(Event.current.type == EventType.Repaint && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                hoveringOverIndex = -1;
            }
            GUILayout.EndArea();
        }
        if(itemIndexToDrag > -1)
        {
            if (availableItems[itemSlots[itemIndexToDrag]].itemPreview)
            {
                GUI.Box(new Rect(Input.mousePosition.x + dragOffset.x, Screen.height - Input.mousePosition.y + dragOffset.y, 95, 95), availableItems[itemSlots[itemIndexToDrag]].itemPreview);
            }
            else
            {
                GUI.Box(new Rect(Input.mousePosition.x + dragOffset.x, Screen.height - Input.mousePosition.y + dragOffset.y, 95, 95), availableItems[itemSlots[itemIndexToDrag]].itemName);
            }
        }
        if(hoveringOverIndex > -1 && itemSlots[hoveringOverIndex] > -1 && itemIndexToDrag < 0)
        {
            GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y - 30, 100, 25), availableItems[itemSlots[hoveringOverIndex]].itemName);
        }
        if(!showInventory)
        {
            GUI.color = detectedItem ? Color.green : Color.white;
            GUI.DrawTexture(new Rect(Screen.width / 2 - 4, Screen.height / 2 - 4, 8, 8), crosshairTexture);
            GUI.color = Color.white;

            if(detectedItem)
            {
                GUIStyle labelStyle2 = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 40,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.green },
                    alignment = TextAnchor.MiddleCenter
                };

                GUI.color = new Color(0, 0, 0, 0.84f);
                GUI.Label(new Rect(Screen.width / 2 - 400 + 1, Screen.height / 2 - 250 + 1, 800, 200), "Press 'F' to pick up '" + detectedItem.itemName + "'", labelStyle);
                GUI.color = Color.green;
                GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 250, 800, 200), "Press 'F' to pick up '" + detectedItem.itemName + "'", labelStyle);
            }
        }
    }
}
