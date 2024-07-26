using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsUIManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Toggle fullscreenToggle;

    private void Awake() {
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void SetMusicVolume(float volume) {
        masterMixer.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume) {
        masterMixer.SetFloat("sfxVolume", volume);
    }

    public void ToggleFullscreen(bool isOn) {
        Screen.fullScreen = isOn;
    }
}
