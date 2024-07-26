using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour {
    [Header("References")]

    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private GameObject quitButton;

#if UNITY_WEBGL && !UNITY_EDITOR
    private void Awake() {
        // Doesn't make sense to add a quit button in a WebGL since there is nothing to actually quit.
        quitButton.SetActive(false);
}
#endif

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
