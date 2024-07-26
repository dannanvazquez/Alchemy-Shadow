using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsUIManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private GameObject mainMenuButton;

    private void OnEnable() {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.5f);

        fullscreenToggle.isOn = Screen.fullScreen;

        if (SceneManager.GetActiveScene().buildIndex == 0) {
            mainMenuButton.SetActive(false);
        }
    }

    private void OnDisable() {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }

    private void Start() {
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    public void SetMusicVolume(float volume) {
        masterMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume) {
        masterMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
    }

    public void ToggleFullscreen(bool isOn) {
        Screen.fullScreen = isOn;
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
}
