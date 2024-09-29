using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Image KT;
    public Image[] slots;

    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeSlotColor(0, Color.blue);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSlotColor(1, Color.blue);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSlotColor(2, Color.blue);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSlotColor(3, Color.blue);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeSlotColor(4, Color.blue);
        if(KT.color == Color.white)
        {
            ChangeSlotColor(0, Color.white);
            ChangeSlotColor(1, Color.white);
            ChangeSlotColor(2, Color.white);
            ChangeSlotColor(3, Color.white);
            ChangeSlotColor(4, Color.white);
        }
    }

    public void ToggleInventory()
    {
        if (!inventoryPanel.activeSelf)
        {
            KT.color = Color.green;
            inventoryPanel.SetActive(true);
        }
        else
        { 
            KT.color= Color.white;
            inventoryPanel.SetActive(false);
        } 
    }

    private void ChangeSlotColor(int slotIndex, Color color)
    {

        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            slots[slotIndex].color = color;
        }
    }
}
