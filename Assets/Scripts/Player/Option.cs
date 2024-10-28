using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class OptionsMenu : MonoBehaviour
{
    // Slider âm lượng và độ sáng
    public Slider volumeSlider;
    public Slider brightnessSlider;

    // Post Processing và Exposure
    public PostProcessProfile brightness;
    private AutoExposure exposure;

    // Giá trị để lưu mức âm lượng và độ sáng hiện tại
    private float currentVolume;
    private float currentBrightness;

    public GameObject optionsPanel; // Tham chiếu đến panel Options

    public GameObject HealthBar;
    public GameObject _4Nut;
    public GameObject InventoryHealth;

    void Start()
    {
        // Ensure brightness is not null and TryGetSettings is successful
        if (brightness != null && brightness.TryGetSettings(out exposure))
        {
            // Thiết lập giá trị ban đầu cho các slider
            currentVolume = AudioListener.volume; // Lấy âm lượng hiện tại
            currentBrightness = exposure.keyValue.value; // Lấy độ sáng từ Auto Exposure

            // Gán giá trị cho các slider dựa trên giá trị hiện tại
            volumeSlider.value = currentVolume;
            brightnessSlider.value = currentBrightness;

            // Gán hàm xử lý sự kiện cho các slider
            volumeSlider.onValueChanged.AddListener(SetVolume);
            brightnessSlider.onValueChanged.AddListener(AdjustBrightness);
        }
        else
        {
            Debug.LogError("Brightness profile or AutoExposure settings are not assigned properly.");
        }
    }

    private void SetVolume(float volume)
    {
        // Cập nhật âm lượng
        AudioListener.volume = volume;
        Debug.Log("Volume set to: " + volume);
    }

    public void AdjustBrightness(float value)
    {
        // Cập nhật độ sáng
        exposure.keyValue.value = value;
        Debug.Log("Brightness set to: " + value);
    }

    // Hàm này để hiển thị Option Panel và dừng game
    public void OpenOptionsMenu()
    {
        optionsPanel.SetActive(true);
        Time.timeScale = 0f; // Dừng game
    }
}