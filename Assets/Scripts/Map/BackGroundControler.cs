using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    private float startPos, length;
    private GameObject cam; // Không cần gán thủ công trong Inspector
    public float paralaxEffect;

    void Start()
    {
        // Tìm Camera khi bắt đầu
        FindCamera();

        // Lấy vị trí ban đầu và chiều dài của Sprite
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // Đăng ký sự kiện khi scene mới được tải
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện khi đối tượng bị hủy
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Tìm lại Camera khi scene mới được tải
        FindCamera();
    }

    private void FindCamera()
    {
        // Tìm đối tượng Camera với Tag "MainCamera"
        cam = GameObject.FindWithTag("MainCamera");

        if (cam == null)
        {
            Debug.LogWarning("Main Camera not found in the scene!");
        }
    }

    void LateUpdate()
    {
        if (cam == null) return; // Kiểm tra nếu không tìm thấy Camera

        // Tính toán hiệu ứng parallax
        float distance = cam.transform.position.x * paralaxEffect;
        float movement = cam.transform.position.x * (1 - paralaxEffect);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // Lặp lại chuyển động khi đi quá chiều dài của sprite
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
