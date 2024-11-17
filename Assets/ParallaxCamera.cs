using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{
    public Transform mainCamera; // Camera chính (Main Camera)
    public Vector3 offset; // Độ lệch giữa camera phụ và camera chính

    void Start()
    {
        // Tìm camera chính nếu chưa gán
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }

        // Khởi tạo offset nếu chưa đặt giá trị
        offset = transform.position - mainCamera.position;
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Đồng bộ vị trí của camera phụ theo camera chính + offset
            transform.position = mainCamera.position + offset;
        }
    }
}
