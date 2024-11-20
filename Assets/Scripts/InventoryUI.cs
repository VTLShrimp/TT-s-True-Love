using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel; // Panel của Inventory
    public Image KT; // Image KT dùng để hiển thị trạng thái
    public Image[] slots; // Mảng chứa các slot trong Inventory

    void Start()
    {
        // Inventory ban đầu sẽ ẩn
        inventoryPanel.SetActive(false);
        KT.color = Color.white; // Màu mặc định của KT
    }

    void Update()
    {
        // Kiểm tra phím Tab để mở hoặc đóng Inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        // Thay đổi màu slot dựa trên phím số
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeSlotColor(0, Color.blue);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSlotColor(1, Color.blue);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSlotColor(2, Color.blue);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSlotColor(3, Color.blue);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeSlotColor(4, Color.blue);

        if (Input.GetKeyDown(KeyCode.Alpha7)) 
        {
            KT.color = Color.white; // Khi đóng, KT chuyển màu trắng
            inventoryPanel.SetActive(false); // Ẩn Inventory
        };
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            KT.color = Color.white; // Khi đóng, KT chuyển màu trắng
            inventoryPanel.SetActive(false); // Ẩn Inventory
        };
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            KT.color = Color.white; // Khi đóng, KT chuyển màu trắng
            inventoryPanel.SetActive(false); // Ẩn Inventory
        };
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            KT.color = Color.white; // Khi đóng, KT chuyển màu trắng
            inventoryPanel.SetActive(false); // Ẩn Inventory
        };

        // Nếu KT trở về màu trắng, tất cả các slot sẽ chuyển về màu trắng
        if (KT.color == Color.white)
        {
            ResetSlotColors();
        }
    }

    // Chức năng mở hoặc đóng Inventory
    public void ToggleInventory()
    {
        if (!inventoryPanel.activeSelf)
        {
            KT.color = Color.green; // Khi mở, KT chuyển màu xanh lá
            inventoryPanel.SetActive(true); // Hiển thị Inventory
        }
        else
        {
            KT.color = Color.white; // Khi đóng, KT chuyển màu trắng
            inventoryPanel.SetActive(false); // Ẩn Inventory
        }
    }

    // Thay đổi màu của slot theo chỉ số slot và màu truyền vào
    private void ChangeSlotColor(int slotIndex, Color color)
    {
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            slots[slotIndex].color = color;
        }
    }

    // Đặt lại tất cả các slot về màu trắng
    private void ResetSlotColors()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            ChangeSlotColor(i, Color.white);
        }
    }
}
