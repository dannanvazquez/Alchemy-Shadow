using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class RecapUIController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject nextDayButton;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text[] infoTexts;

    private Canvas canvas;

    private void Awake() {
        canvas = GetComponent<Canvas>();
    }

    public void EnableCanvas(int day, string[] stats) {
        titleText.text = $"Day {day} Recap";

        infoTexts[0].text = $"Clients served: {stats[0]}";
        infoTexts[1].text = $"Clients refused: {stats[1]}";
        infoTexts[2].text = $"Money spent: {stats[2]}";
        infoTexts[3].text = $"Money earned: {stats[3]}";

        canvas.enabled = true;
    }

    public void EnableMainMenuButton() {
        nextDayButton.SetActive(false);
        mainMenuButton.SetActive(true);
    }

    public void NextDay() {
        canvas.enabled = false;

        GameManager.Instance.NextDay();
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
}
