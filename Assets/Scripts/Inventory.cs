using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Image[] slots; // Mảng chứa các slot
    private Color selectedColor = Color.green; // Màu sắc khi slot được chọn
    private Color defaultColor = Color.white; // Màu sắc mặc định

    private void Start()
    {
        // Đặt tất cả các slot về màu mặc định
        foreach (var slot in slots)
        {
            slot.color = defaultColor;
        }
    }

    private void Update()
    {
        // Kiểm tra phím được nhấn và gọi SelectSlot tương ứng
        if (Input.GetKeyDown(KeyCode.Alpha0)) // Phím "7"
        {
            SelectSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) // Phím "8"
        {
            SelectSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) // Phím "9"
        {
            SelectSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) // Phím "0"
        {
            SelectSlot(3);
        }
    }

    public void SelectSlot(int index)
    {
        // Kiểm tra chỉ số hợp lệ
        if (index < 0 || index >= slots.Length)
            return;

        // Đặt tất cả slot về màu mặc định
        foreach (var slot in slots)
        {
            slot.color = defaultColor;
        }

        // Đặt màu cho slot được chọn
        slots[index].color = selectedColor;
    }
}
