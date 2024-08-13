using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class EndRecapUIController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    private Canvas canvas;

    private void Awake() {
        canvas = GetComponent<Canvas>();
    }

    public void EnableCanvas(bool isWinner) {
        if (isWinner) {
            titleText.text = "Well done!";
            descriptionText.text = "You made, through the power friendship or through your tough decision making, you're now debt free! Congrats.";
        } else {
            titleText.text = "Game over";
            descriptionText.text = "Seems like you could've made better choices. Do you think you can figure out what went wrong?";
        }

        canvas.enabled = true;
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
}
