using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown graphicsDropdown;
    public Slider brightnessSlider;
    public Slider contrastSlider;
    public Slider volumeSlider;
    public GameObject settingsPanel;
    public GameObject menuPanel;

    private Resolution[] resolutions;
    private PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;
    private int currentResolutionIndex = 0;

    void Start()
    {
        // Tìm hoặc thiết lập Post-Processing Volume
        postProcessVolume = FindObjectOfType<PostProcessVolume>();
        if (postProcessVolume != null && postProcessVolume.profile.TryGetSettings(out colorGrading))
        {
            Debug.Log("Color Grading found");
        }
        else
        {
            Debug.LogError("Color Grading not found. Ensure that the Post-Processing Profile has Color Grading enabled.");
        }

        // Thiết lập danh sách độ phân giải
        SetupResolutionOptions();

        // Thiết lập chất lượng đồ họa ban đầu
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
        graphicsDropdown.RefreshShownValue();

        // Thiết lập giá trị ban đầu cho các sliders
        brightnessSlider.value = PlayerPrefs.GetFloat("brightness", 0.5f);
        contrastSlider.value = PlayerPrefs.GetFloat("contrast", 0.5f);
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);


        // Áp dụng cài đặt ban đầu
        ApplySettings();
    }

    // Phương thức thiết lập danh sách độ phân giải
    private void SetupResolutionOptions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Thiết lập độ phân giải khi người dùng thay đổi tùy chọn
    public void SetResolution(int resolutionIndex)
    {
        resolutionIndex = resolutionDropdown.value;  // Lấy giá trị trực tiếp từ Dropdown

        if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length)
        {
            Debug.LogWarning("Resolution index is out of bounds!");
            return;
        }

        // Đặt chế độ toàn màn hình Exclusive để dễ dàng thấy sự thay đổi độ phân giải
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;

        // Thiết lập độ phân giải mới
        Resolution resolution = resolutions[resolutionIndex];
        Debug.Log("Setting resolution to: " + resolution.width + " x " + resolution.height);
        Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.ExclusiveFullScreen);

        // Lưu độ phân giải đã chọn vào PlayerPrefs
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
    }

    // Thiết lập chất lượng đồ họa
    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("graphicsQuality", qualityIndex);
    }

    // Thiết lập độ sáng qua post-exposure
    public void SetBrightness(float brightness)
    {
        if (colorGrading != null)
        {
            colorGrading.postExposure.value = brightness * 2 - 1; // Đặt dải từ -1 đến 1
            PlayerPrefs.SetFloat("brightness", brightness);
            Debug.Log("Brightness: " + colorGrading.postExposure.value);
        }
    }

    public void SetContrast(float contrast)
    {
        if (colorGrading != null)
        {
            Debug.Log($"Contrast: {contrast}"); // Log giá trị để kiểm tra
            colorGrading.contrast.value = contrast * 100; // Dải từ 0 đến 100
            PlayerPrefs.SetFloat("contrast", contrast);
        }
        else
        {
            Debug.LogError("Color Grading is null. Ensure Post-Processing is set up correctly.");
        }
    }

    // Thiết lập âm lượng toàn bộ game
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);
    }

    // Áp dụng các cài đặt từ PlayerPrefs khi khởi động game
    public void ApplySettings()
    {
        int savedResolutionIndex = PlayerPrefs.GetInt("resolutionIndex", currentResolutionIndex);
        SetResolution(savedResolutionIndex);

        int savedGraphicsQuality = PlayerPrefs.GetInt("graphicsQuality", QualitySettings.GetQualityLevel());
        SetGraphicsQuality(savedGraphicsQuality);
        SetBrightness(PlayerPrefs.GetFloat("brightness", 0.5f));
        SetContrast(PlayerPrefs.GetFloat("contrast", 0.5f));
        SetVolume(PlayerPrefs.GetFloat("volume", 0.5f));
    }

    // Đóng bảng cài đặt và quay lại menu
    public void ExitButton()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
