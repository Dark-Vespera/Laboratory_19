using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour {
    [Header("UI Элементы")]
    public Slider brightnessSlider;
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public Image brightnessOverlay; // Черная картинка на весь экран для яркости

    void Start() {
        // Загружаем сохраненные настройки
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 2f);

        ApplyAllSettings();
    }

    public void SetBrightness(float value) {
        PlayerPrefs.SetFloat("Brightness", value);
        ApplyAllSettings();
    }

    public void SetVolume(float value) {
        PlayerPrefs.SetFloat("Volume", value);
        ApplyAllSettings();
    }

    public void SetSensitivity(float value) {
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }

    public GameObject settingsPanel;

    public void OpenSettings() {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings() {
        settingsPanel.SetActive(false);
    }

    void ApplyAllSettings() {
        AudioListener.volume = volumeSlider.value;

        if (brightnessOverlay != null) {
            float alpha = 1f - brightnessSlider.value;
            Color c = brightnessOverlay.color;
            c.a = alpha;
            brightnessOverlay.color = c;
        }
    }
}