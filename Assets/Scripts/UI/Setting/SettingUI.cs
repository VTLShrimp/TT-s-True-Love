using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void Start()
    {
        // Thiết lập độ phân giải
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

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

        // Thiết lập chất lượng đồ họa
        graphicsDropdown.value = QualitySettings.GetQualityLevel();

        // Thiết lập các giá trị ban đầu cho sliders
        brightnessSlider.value = PlayerPrefs.GetFloat("brightness", 0.5f);
        contrastSlider.value = PlayerPrefs.GetFloat("contrast", 0.5f);
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);

        ApplySettings();
    }

    public void SetResolution(int resolutionIndex)
    {

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetBrightness(float brightness)
    {

        PlayerPrefs.SetFloat("brightness", brightness);

    }

    public void SetContrast(float contrast)
    {
        // Giả lập độ tương phản bằng cách lưu giá trị (trong thực tế cần xử lý shader hoặc post-processing)
        PlayerPrefs.SetFloat("contrast", contrast);
        // Gọi hàm để áp dụng độ tương phản trong game nếu có
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void ApplySettings()
    {
        SetBrightness(PlayerPrefs.GetFloat("brightness"));
        SetContrast(PlayerPrefs.GetFloat("contrast"));
        SetVolume(PlayerPrefs.GetFloat("volume"));
    }
    public void ExitButton()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
