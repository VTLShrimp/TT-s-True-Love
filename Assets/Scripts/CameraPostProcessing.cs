using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraPostProcessing : MonoBehaviour
{
    private PostProcessVolume volume;
    private ColorGrading colorGrading;

    void Start()
    {
        // Gán PostProcessVolume cho camera
        volume = gameObject.AddComponent<PostProcessVolume>();
        volume.isGlobal = true;

        // Tạo profile cho volume
        PostProcessProfile profile = ScriptableObject.CreateInstance<PostProcessProfile>();

        // Thêm hiệu ứng ColorGrading vào profile
        colorGrading = profile.AddSettings<ColorGrading>();
        volume.profile = profile;

        // Cấu hình mặc định cho độ sáng và tương phản
        colorGrading.postExposure.value = 0.5f;
        colorGrading.contrast.value = 20f;
    }
}
