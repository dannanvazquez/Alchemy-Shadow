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

        infoTexts[0].text = $"Clients served: <color=#8ABB62>{stats[0]}</color>";
        infoTexts[1].text = $"Clients refused: <color=#8ABB62>{stats[1]}</color>";
        infoTexts[2].text = $"Money spent: <color=#8ABB62>${stats[2]}</color>";
        infoTexts[3].text = $"Money earned: <color=#8ABB62>${stats[3]}</color>";
        infoTexts[4].text = $"<color=#8ABB62>${stats[4]}</color> is missing.";

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
