using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    public float shakeAmount = 0.2f; // Độ mạnh của rung
    public float shakeDuration = 0.5f; // Thời gian rung

    void Start()
    {
        originalPosition = transform.position; // Lưu vị trí ban đầu của camera
    }

    public void Shake()
    {
        // Start shaking the camera
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Tạo hiệu ứng rung bằng cách thay đổi vị trí ngẫu nhiên của camera
            Vector3 randomShake = originalPosition + Random.insideUnitSphere * shakeAmount;

            transform.position = new Vector3(randomShake.x, randomShake.y, originalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đặt lại vị trí của camera sau khi kết thúc rung
        transform.position = originalPosition;
    }
}
