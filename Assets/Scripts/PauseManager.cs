using UnityEngine;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private AudioMixerSnapshot pausedMixerSnapshot;
    [SerializeField] private AudioMixerSnapshot unpausedMixerSnapshot;

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
        Time.timeScale = isPaused ? 0 : 1;

        Lowpass();
    }

    private void Lowpass() {
        if (Time.timeScale == 0) {
            pausedMixerSnapshot.TransitionTo(snapshotTransitionTime);
        } else {
            unpausedMixerSnapshot.TransitionTo(snapshotTransitionTime);
        }
    }
}
