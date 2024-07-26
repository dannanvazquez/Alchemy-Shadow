using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecapUIController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TMP_Text[] infoTexts;

    private void OnEnable() {
        infoTexts[0].text = $"Clients served: {PlayerPrefs.GetFloat("clientsServed", 0)}";
        infoTexts[1].text = $"Clients refused: {PlayerPrefs.GetFloat("clientsRefused", 0)}";
        infoTexts[2].text = $"Money spent: ${PlayerPrefs.GetFloat("moneySpent", 0)}";
        infoTexts[3].text = $"Money earned: ${PlayerPrefs.GetFloat("moneyEarned", 0)}";
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
}
