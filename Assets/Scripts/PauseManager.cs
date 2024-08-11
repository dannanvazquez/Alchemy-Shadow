using UnityEngine;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private AudioSource unpausedMusicAudio;
    [SerializeField] private AudioSource pausedMusicAudio;
    [SerializeField] private AudioSource[] pausedAudioSources;

    [Header("Settings")]
    [Tooltip("The amount of seconds it takes to transition between AudioMixerSnapshots.")]
    [SerializeField] private float snapshotTransitionTime;

    public bool isPaused { get; private set; } = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    public void TogglePause() {
        isPaused = !isPaused;
        optionsCanvas.enabled = isPaused;
        Lowpass();
        MuteAudioSources();

        Time.timeScale = isPaused ? 0 : 1;
    }

    private void Lowpass() {
        unpausedMusicAudio.mute = isPaused;
        pausedMusicAudio.mute = !isPaused;
    }

    private void MuteAudioSources() {
        foreach (var source in pausedAudioSources) {
            if (isPaused) {
                source.Pause();
            } else {
                source.UnPause();
            }
        }
    }
}
