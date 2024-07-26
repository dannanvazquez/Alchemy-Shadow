using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject quitButton;

    private void Awake() {
#if UNITY_WEBGL && !UNITY_EDITOR
        quitButton.SetActive(false);
#endif
    }

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
